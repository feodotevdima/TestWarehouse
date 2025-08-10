import React, { useState, useRef, useEffect } from 'react';
import { UUID } from 'crypto';
import styles from './ItemSelector.module.css';

interface Item {
  id: UUID;
  name: string;
}

interface SingleItemSelectorProps<T extends Item> {
  items: T[];
  selectedId: UUID | null;
  onSelectionChange: (selectedId: UUID | null) => void;
  placeholder?: string;
  title?: string;
}

const SingleItemSelector = <T extends Item>({
  items,
  selectedId,
  onSelectionChange,
  placeholder = 'Выберите элемент',
  title = '',
}: SingleItemSelectorProps<T>) => {
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
    const newSelectedId = selectedId === id ? null : id;
    onSelectionChange(newSelectedId);
    setIsOpen(false);
  };

  const toggleDropdown = () => setIsOpen(!isOpen);

  const selectedItem = items.find(item => item.id === selectedId);

  return (
    <div className={styles.container} ref={dropdownRef}>
      <div className={styles.title}>{title}</div>
      <div 
        className={styles.selectorInput}
        onClick={toggleDropdown}
      >
        {selectedItem ? (
          <div className={styles.selectedItem}>
            {selectedItem.name}
          </div>
        ) : (
          <div className={styles.placeholder}>{placeholder}</div>
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
                  selectedId === item.id ? styles.selected : ''
                }`}
                onClick={() => handleItemClick(item.id)}
              >
                <span>{item.name}</span>
                {selectedId === item.id && (
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

export default SingleItemSelector;