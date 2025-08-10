import React, { useState } from 'react';
import styles from './AddResourcePage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import AddResource from './Api/AddResource';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const AddResourcePage: React.FC = () => {
  const navigate = useNavigate();

  const [resourceName, setResourceName] = useState<string>('');
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const SaveResource = async () => {
    try {
      await AddResource(resourceName);
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
      <h1>Ресурс</h1>
      <FieldInput label='Наименование' initialValue={resourceName} onChange={setResourceName} />
      <WarehouseButton color="#5a97ecff" onClick={SaveResource} margin='20px 0'>Сохранить</WarehouseButton>
    </div>
  );
};