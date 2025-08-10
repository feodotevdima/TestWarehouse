import React, { useState } from 'react';
import styles from './FieldInput.module.css';

interface FieldInputProps {
  label?: string;
  initialValue: string;
  onChange: (newValue: string) => void;
  onlyNumbers?: boolean;
  maxValue?: number;
}

export const FieldInput: React.FC<FieldInputProps> = ({ label, initialValue, onChange, onlyNumbers = false, maxValue = null }) => {
  const [value, setValue] = useState<string>(initialValue);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let newValue = e.target.value;
    if (onlyNumbers) {
      newValue = newValue.replace(/\D/g, '');

      if(maxValue && maxValue < Number(newValue))
        newValue = maxValue.toString();
    }

    onChange(newValue);
    setValue(newValue);
  };

  return (
    <div className={styles.fieldContainer}>
      <label className={styles.label}>
        {label}
      </label>
      <input
        type="text"
        value={value}
        onChange={handleChange}
        className={styles.input}
        inputMode={onlyNumbers ? "numeric" : "text"}
      />
    </div>
  );
};