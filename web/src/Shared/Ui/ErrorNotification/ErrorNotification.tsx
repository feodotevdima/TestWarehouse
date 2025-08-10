import React, { useEffect } from 'react';
import styles from './ErrorNotification.module.css';

interface ErrorNotificationProps {
  message: string;
  onClose: () => void;
  duration?: number;
}

const ErrorNotification: React.FC<ErrorNotificationProps> = ({
  message,
  onClose,
  duration = 20000,
}) => {
  useEffect(() => {
    const timer = setTimeout(() => {
      onClose();
    }, duration);

    return () => clearTimeout(timer);
  }, [duration, onClose]);

  return (
    <div className={styles.errorContainer}>
      <div className={styles.errorContent}>
        <span>{message}</span>
        <button className={styles.closeButton} onClick={onClose}>×</button>
      </div>
    </div>
  );
};

export default ErrorNotification;