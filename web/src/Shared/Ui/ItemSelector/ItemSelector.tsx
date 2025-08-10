import React, { useState, useRef, useEffect } from 'react';
import { UUID } from 'crypto';
import styles from './ItemSelector.module.css';

interface Item {
  id: UUID;
  name: string;
}

interface ItemSelectorProps<T extends Item> {
  items: T[];
  selectedIds: UUID[];
  onSelectionChange: (selectedIds: UUID[]) => void;
  placeholder?: string;
  title?: string;
  maxDisplayItems?: number;
}

const ItemSelector = <T extends Item>({
  items,
  selectedIds,
  onSelectionChange,
  placeholder = 'Выберите элементы',
  title='',
  maxDisplayItems = 3,
}: ItemSelectorProps<T>) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleItemClick = (id: UUID) => {
    const newSelectedIds = selectedIds.includes(id)
      ? selectedIds.filter(itemId => itemId !== id)
      : [...selectedIds, id];
    
    onSelectionChange(newSelectedIds);
  };

  const toggleDropdown = () => setIsOpen(!isOpen);

  const selectedItems = items.filter(item => selectedIds.includes(item.id));
  const displayItems = selectedItems.slice(0, maxDisplayItems);
  const remainingCount = selectedItems.length - maxDisplayItems;

  return (
    <div className={styles.container} ref={dropdownRef}>
      <div className={styles.title}>{title}</div>
      <div 
        className={styles.selectorInput}
        onClick={toggleDropdown}
      >
        {selectedItems.length === 0 ? (
          <div className={styles.placeholder}>{placeholder}</div>
        ) : (
          <div className={styles.selectedItems}>
            {displayItems.map(item => (
              <span key={item.id} className={styles.selectedTag}>
                {item.name}
              </span>
            ))}
            {remainingCount > 0 && (
              <span className={styles.moreCount}>+{remainingCount}</span>
            )}
          </div>
        )}
        <span className={`${styles.arrow} ${isOpen ? styles.open : ''}`}>▼</span>
      </div>

      {isOpen && (
        <div className={styles.dropdown}>
          <div className={styles.dropdownContent}>
            {items.map(item => (
              <div
                key={item.id}
                className={`${styles.dropdownItem} ${
                  selectedIds.includes(item.id) ? styles.selected : ''
                }`}
                onClick={() => handleItemClick(item.id)}
              >
                <span>{item.name}</span>
                {selectedIds.includes(item.id) && (
                  <span className={styles.checkmark}>✓</span>
                )}
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default ItemSelector;