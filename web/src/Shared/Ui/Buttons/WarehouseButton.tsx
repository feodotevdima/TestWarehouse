import styles from './ButtonPage.module.css';

interface ButtonProps {
  children: React.ReactNode;
  color: string;
  margin?: string;
  padding?: string;
  onClick?: () => void; 
}

export const WarehouseButton = ({ children, color, onClick, margin = '0px', padding = '15px' }: ButtonProps) => {
  return (
    <button 
      className={styles.warehouseButton}
      style={{ backgroundColor: color, margin: margin, padding: padding }}
      onClick={onClick}
    >
      {children}
    </button>
  );
}
