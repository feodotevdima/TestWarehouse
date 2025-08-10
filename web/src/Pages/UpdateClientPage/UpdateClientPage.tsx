import React, { useState, useEffect } from 'react';
import styles from './UpdateClientPage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import GetClientById from './Api/GetCientById';
import { UUID } from 'crypto';
import UpdateClient from './Api/UpdateClient';
import ChangeClientStatus from './Api/ChangeClientStatus';
import DeleteClient from './Api/DeleteClient';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const UpdateClientPage: React.FC = () => {
  const navigate = useNavigate();
  const { clientIdParam } = useParams();

  const [loading, setLoading] = useState(true);
  const [clientId, setClientId] = useState<UUID>();
  const [clientName, setClientName] = useState<string>('');
  const [clientAdress, setClientAdress] = useState<string>('');
  const [clientIsActive, setClientIsActive] = useState<boolean>(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

   useEffect(() => {
    const fetchClients = async () => {
      try {
        const data = await GetClientById(clientIdParam as `${string}-${string}-${string}-${string}-${string}`);
        setClientId(data.id);
        setClientName(data.name);
        setClientAdress(data.address);
        setClientIsActive(data.isActive);
      } 
      catch (error)
      {
          setErrorMessage('Не удалось подключиться к серверу. Проверьте интернет-соединение');
      } 
      finally
      {
        setLoading(false);
      }
    };

    fetchClients();
  }, []);

  const SaveClient = async () => {
    if(clientId === undefined)
      return;
    try {
      await UpdateClient({id: clientId, name: clientName, address: clientAdress, isActive: clientIsActive});
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

  const DelClient = async () => {
    if(clientId === undefined)
      return;
    try {
      await DeleteClient(clientId);
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

  const ArchiveClient = async () => {
    if(clientId === undefined)
      return;
    try {
      await ChangeClientStatus(clientId);
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
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <div>
          <FieldInput label='Наименование' initialValue={clientName} onChange={setClientName} />
          <FieldInput label='Адрес' initialValue={clientAdress} onChange={setClientAdress} />
          <WarehouseButton color="#5a97ecff" onClick={SaveClient} margin='20px 0'>Сохранить</WarehouseButton>
          <WarehouseButton color="#fc6e6eff" onClick={DelClient} margin='20px 15px'>Удалить</WarehouseButton>
          <WarehouseButton color="rgba(209, 209, 209, 1)" onClick={ArchiveClient} margin='20px 0'>{clientIsActive? 'В архив' : 'В работу'}</WarehouseButton>
        </div>)}
    </div>
  );
};