import React, { useEffect, useState } from 'react';
import { Table } from '../../Shared/Ui/Table/table';
import GetBalance from '../../Shared/Api/GetBalance';
import Balance from '../../Entities/Balance';
import styles from './BalancePage.module.css';
import ItemSelector from '../../Shared/Ui/ItemSelector/ItemSelector';
import { UUID } from 'crypto';
import Resource from '../../Entities/Resource';
import GetResourcesUsedInBalance from './Api/GetResourcesUsedInBalance';
import GetUnitsUsedInBalance from './Api/GetUnitsUsedInBalance';
import Unit from '../../Entities/Unit';

export const BalancePage: React.FC = () => {
  const [balance, setBalance] = useState<Balance[]>([]);
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectResources, setSelectResources] = useState<UUID[]>([]);
  const [selectUnits, setSelectUnits] = useState<UUID[]>([]);

  useEffect(() => {
    const fetchBalance = async () => {
      const dataResources = await GetResourcesUsedInBalance();
      setResources(dataResources);
      const dataUnits = await GetUnitsUsedInBalance();
      setUnits(dataUnits);
    };

    fetchBalance();
  }, []);

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const data = await GetBalance({resourcesIds: selectResources, unitsIds: selectUnits});
        setBalance(data);
      } catch (error) {
        console.error('Ошибка при загрузке баланса:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchBalance();
  }, [selectUnits, selectResources]);

  const columns = [
    { header: 'Ресурс', accessor: 'resourceName' },
    { header: 'Единица измерения', accessor: 'unitName' },
    { header: 'Количество', accessor: 'quantity' }
  ];

  return (
    <div className={styles.balancePage}>
      <h1>Баланс</h1>
      
      <div className={styles.ItemSelectContainer}>
        <ItemSelector
          items = {resources}
          selectedIds = {selectResources}
          onSelectionChange = {setSelectResources}
          title='Ресурс'
        />
        <ItemSelector
          items = {units}
          selectedIds = {selectUnits}
          onSelectionChange = {setSelectUnits}
          title='Единица измерения'
        />
       </div>
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <Table 
          columns={columns} 
          data={balance} 
          emptyMessage="Нет данных о балансе"
        />
      )}
    </div>
  );
};