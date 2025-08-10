import React, { useState } from 'react';
import styles from './AddUnitPage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import AddUnit from './Api/AddUnit';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const AddUnitPage: React.FC = () => {
  const navigate = useNavigate();

  const [unitName, setUnitName] = useState<string>('');
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const SaveUnit = async () => {
    try {
      await AddUnit(unitName);
      navigate(-1);
    }
    catch (error)
    {
      if (error instanceof Error) {
        setErrorMessage(error.message);
      } else {
        setErrorMessage(String(error));
      }
    } 
  }

  return (
    <div className={styles.container}>
      {errorMessage && (
        <ErrorNotification 
          message={errorMessage} 
          onClose={() => setErrorMessage(null)} 
        />
      )}
      <h1>Единица измерения</h1>
      <FieldInput label='Наименование' initialValue={unitName} onChange={setUnitName} />
      <WarehouseButton color="#5a97ecff" onClick={SaveUnit} margin='20px 0'>Сохранить</WarehouseButton>
    </div>
  );
};