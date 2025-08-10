import React, { useState } from 'react';
import styles from './AddClientPage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import AddClient from './Api/AddClient';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const AddClientPage: React.FC = () => {
  const navigate = useNavigate();

  const [clientName, setClientName] = useState<string>('');
  const [clientAdress, setClientAdress] = useState<string>('');
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const SaveClient = async () => {
    try {
      await AddClient({name: clientName, address: clientAdress});
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
      <h1>Клиент</h1>
      <FieldInput label='Наименование' initialValue={clientName} onChange={setClientName} />
      <FieldInput label='Адрес' initialValue={clientAdress} onChange={setClientAdress} />
      <WarehouseButton color="#5a97ecff" onClick={SaveClient} margin='20px 0'>Сохранить</WarehouseButton>
    </div>
  );
};