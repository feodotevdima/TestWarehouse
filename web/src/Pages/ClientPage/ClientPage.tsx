import React, { useEffect, useState } from 'react';
import { Table } from '../../Shared/Ui/Table/table';
import GetActiveClients  from '../../Shared/Api/GetActiveClients';
import GetArchiveClients from './Api/GetArchiveCients';
import Client from '../../Entities/Client';
import styles from './ClientPage.module.css';
import { UUID } from 'crypto';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const ClientPage: React.FC = () => {
  const navigate = useNavigate();

  const [clients, setClients] = useState<Client[]>([]);
  const [loading, setLoading] = useState(true);
  const [active, setActive] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    const fetchClients = async () => {
      try {
        if(active)
        {
          const data = await GetActiveClients();
          setClients(data);
        }
        else
        {
          const data = await GetArchiveClients();
          setClients(data);
        }
      } catch (error) {
        setErrorMessage('Не удалось подключиться к серверу. Проверьте интернет-соединение');
      } finally {
        setLoading(false);
      }
    };

    fetchClients();
  }, [active]);

  const OnRowClick = (id: UUID) => {navigate(`/client-update/${id}`)}

  const columns = [
    { header: 'Наименование', accessor: 'name' },
    { header: 'Адрес', accessor: 'address' },
  ];

  return (
    <div className={styles.clientPage}>
      {errorMessage && (
        <ErrorNotification 
          message={errorMessage} 
          onClose={() => setErrorMessage(null)} 
        />
      )}
      <h1>Клиенты</h1>
      
        {active? 
          <div className={styles.buttons}>
            <WarehouseButton color="#5a97ecff" onClick = {() =>{navigate('/client-add')}} >Добавить</WarehouseButton>
            <WarehouseButton color="rgba(209, 209, 209, 1)" onClick = {() =>{setActive(false)}} margin='0px 15px'>К архиву</WarehouseButton>
          </div> : 
          <div className={styles.buttons}>
            <WarehouseButton color="#5a97ecff" onClick = {() =>{setActive(true)}}>К рабочим</WarehouseButton>
          </div >
        }
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <Table 
          columns={columns} 
          data={clients} 
          emptyMessage="Нет данных о клиентах"
          onRowClick={OnRowClick}
        />
      )}
    </div>
  );
};