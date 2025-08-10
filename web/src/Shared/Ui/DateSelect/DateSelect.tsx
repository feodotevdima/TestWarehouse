import React, { useState, useRef, useEffect } from 'react';
import styles from './DateSelect.module.css';

interface DateSelectProps {
  selectedDate: Date | null;
  onDateChange: (date: Date | null) => void;
}

const DateSelect: React.FC<DateSelectProps> = ({ selectedDate, onDateChange }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const containerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const toggleCalendar = (e: React.MouseEvent) => {
    e.stopPropagation();
    setIsOpen(!isOpen);
  };

  const handleDateSelect = (date: Date) => {
    onDateChange(date);
    setIsOpen(false);
  };

  const prevMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1));
  };

  const nextMonth = () => {
    setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1));
  };

  const setToday = () => {
    const today = new Date();
    onDateChange(today);
    setCurrentMonth(new Date(today.getFullYear(), today.getMonth(), 1));
    setIsOpen(false);
  };

  const clearDate = () => {
    onDateChange(null);
    setIsOpen(false);
  };

  const renderDays = () => {
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();

    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const firstDayOfWeek = firstDay.getDay() === 0 ? 6 : firstDay.getDay() - 1;
    const daysInMonth = lastDay.getDate();

    const prevMonthLastDay = new Date(year, month, 0).getDate();
    const daysFromNextMonth = 6 - (firstDayOfWeek + daysInMonth - 1) % 7;

    const days = [];

    for (let i = 0; i < firstDayOfWeek; i++) {
      days.push(
        <div key={`prev-${i}`} className={`${styles.day} ${styles.otherMonth}`}>
          {prevMonthLastDay - firstDayOfWeek + i + 1}
        </div>
      );
    }

    for (let i = 1; i <= daysInMonth; i++) {
      const date = new Date(year, month, i);
      const isSelected = selectedDate && isSameDay(date, selectedDate);
      const isToday = isSameDay(date, new Date());

      days.push(
        <div
          key={`current-${i}`}
          className={`${styles.day} ${isSelected ? styles.selected : ''} ${isToday ? styles.today : ''}`}
          onClick={() => handleDateSelect(date)}
        >
          {i}
        </div>
      );
    }

    for (let i = 1; i <= daysFromNextMonth; i++) {
      days.push(
        <div key={`next-${i}`} className={`${styles.day} ${styles.otherMonth}`}>
          {i}
        </div>
      );
    }

    return days;
  };

  return (
    <div className={styles.container} ref={containerRef}>
      <div className={styles.input} onClick={toggleCalendar}>
        {selectedDate ? formatDate(selectedDate) : 'Выберите дату'}
      </div>

      {isOpen && (
        <div className={styles.popup}>
          <div className={styles.header}>
            <button type="button" className={styles.navButton} onClick={prevMonth}>&lt;</button>
            <h3 className={styles.title}>
              {currentMonth.toLocaleString('ru-RU', { month: 'long' })} {currentMonth.getFullYear()}
            </h3>
            <button type="button" className={styles.navButton} onClick={nextMonth}>&gt;</button>
          </div>

          <div className={styles.weekdays}>
            <div>Пн</div>
            <div>Вт</div>
            <div>Ср</div>
            <div>Чт</div>
            <div>Пт</div>
            <div>Сб</div>
            <div>Вс</div>
          </div>

          <div className={styles.daysGrid}>{renderDays()}</div>

          <div className={styles.footer}>
            <button type="button" className={styles.footerButton} onClick={clearDate}>Удалить</button>
            <button type="button" className={styles.footerButton} onClick={setToday}>Сегодня</button>
          </div>
        </div>
      )}
    </div>
  );
};

function isSameDay(date1: Date, date2: Date): boolean {
  return (
    date1.getFullYear() === date2.getFullYear() &&
    date1.getMonth() === date2.getMonth() &&
    date1.getDate() === date2.getDate()
  );
}

function formatDate(date: Date): string {
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  return `${day}.${month}.${year}`;
}

export default DateSelect;