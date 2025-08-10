import React, { useState, useRef, useEffect } from 'react';
import styles from './ItemSelector.module.css';

interface StringSelectorProps{
  items: string[];
  selectedIds: string[];
  onSelectionChange: (selectedIds: string[]) => void;
  placeholder?: string;
  title?: string;
  maxDisplayItems?: number;
}

const StringSelector = ({
  items,
  selectedIds,
  onSelectionChange,
  placeholder = 'Выберите элементы',
  title='',
  maxDisplayItems = 3,
}: StringSelectorProps) => {
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

  const handleItemClick = (item: string) => {
    const newSelectedIds = selectedIds.includes(item)
      ? selectedIds.filter(itemId => itemId !== item)
      : [...selectedIds, item];
    
    onSelectionChange(newSelectedIds);
  };

  const toggleDropdown = () => setIsOpen(!isOpen);

  const selectedItems = items.filter(item => selectedIds.includes(item));
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
              <span key={item} className={styles.selectedTag}>
                {item}
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
                key={item}
                className={`${styles.dropdownItem} ${
                  selectedIds.includes(item) ? styles.selected : ''
                }`}
                onClick={() => handleItemClick(item)}
              >
                <span>{item}</span>
                {selectedIds.includes(item) && (
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

export default StringSelector;