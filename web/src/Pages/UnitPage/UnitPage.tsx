import React, { useEffect, useState } from 'react';
import { Table } from '../../Shared/Ui/Table/table';
import GetActiveUnits  from '../../Shared/Api/GetActiveUnits';
import GetArchiveUnits from './Api/GetArchiveUnits';
import Unit from '../../Entities/Unit';
import styles from './UnitPage.module.css';
import { UUID } from 'crypto';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const UnitPage: React.FC = () => {
  const navigate = useNavigate();

  const [units, setUnits] = useState<Unit[]>([]);
  const [loading, setLoading] = useState(true);
  const [active, setActive] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    const fetchUnits = async () => {
      try {
        if(active)
        {
          const data = await GetActiveUnits();
          setUnits(data);
        }
        else
        {
          const data = await GetArchiveUnits();
          setUnits(data);
        }
      } catch (error) {
        setErrorMessage('Не удалось подключиться к серверу. Проверьте интернет-соединение');
      } finally {
        setLoading(false);
      }
    };

    fetchUnits();
  }, [active]);

  const OnRowClick = (id: UUID) => {navigate(`/unit-update/${id}`)}

  const columns = [
    { header: 'Наименование', accessor: 'name' },
  ];

  return (
    <div className={styles.unitPage}>
      {errorMessage && (
        <ErrorNotification 
          message={errorMessage} 
          onClose={() => setErrorMessage(null)} 
        />
      )}
      <h1>Единицы измерения</h1>
      
        {active? 
          <div className={styles.buttons}>
            <WarehouseButton color="#5a97ecff" onClick = {() =>{navigate('/unit-add')}} >Добавить</WarehouseButton>
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
          data={units} 
          emptyMessage="Нет данных о единицах измерения"
          onRowClick={OnRowClick}
        />
      )}
    </div>
  );
};