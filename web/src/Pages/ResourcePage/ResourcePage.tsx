import React, { useEffect, useState } from 'react';
import { Table } from '../../Shared/Ui/Table/table';
import GetActiveResources  from '../../Shared/Api/GetActiveResources';
import GetArchiveResources from './Api/GetArchiveResources';
import Resource from '../../Entities/Resource';
import styles from './ResourcePage.module.css';
import { UUID } from 'crypto';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const ResourcePage: React.FC = () => {
  const navigate = useNavigate();

  const [resources, setResources] = useState<Resource[]>([]);
  const [loading, setLoading] = useState(true);
  const [active, setActive] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    const fetchResources = async () => {
      try {
        if(active)
        {
          const data = await GetActiveResources();
          setResources(data);
        }
        else
        {
          const data = await GetArchiveResources();
          setResources(data);
        }
      } catch (error) {
        setErrorMessage('Не удалось подключиться к серверу. Проверьте интернет-соединение');
      } finally {
        setLoading(false);
      }
    };

    fetchResources();
  }, [active]);

  const OnRowClick = (id: UUID) => {navigate(`/resource-update/${id}`)}

  const columns = [
    { header: 'Наименование', accessor: 'name' },
  ];

  return (
    <div className={styles.resourcePage}>
      {errorMessage && (
        <ErrorNotification 
          message={errorMessage} 
          onClose={() => setErrorMessage(null)} 
        />
      )}
      <h1>Ресурсы</h1>
      
        {active? 
          <div className={styles.buttons}>
            <WarehouseButton color="#5a97ecff" onClick = {() =>{navigate('/resource-add')}} >Добавить</WarehouseButton>
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
          data={resources} 
          emptyMessage="Нет данных о ресурсах"
          onRowClick={OnRowClick}
        />
      )}
    </div>
  );
};