import React, { useEffect, useState } from 'react';
import { Table } from '../../Shared/Ui/Table/table';
import GetShipments from './Api/GetShipments';
import { Shipment } from '../../Entities/Shipment';
import styles from './ShipmentPage.module.css';
import { UUID } from 'crypto';
import DateSelect from '../../Shared/Ui/DateSelect/DateSelect';
import Unit from '../../Entities/Unit';
import Resource from '../../Entities/Resource';
import GetResourcesUsedInShipment from './Api/GetResourcesUsedInShipment';
import GetUnitsUsedInShipment from './Api/GetUnitsUsedInShipment';
import GetShipmentNumbers from './Api/GetShipmentNumbers';
import ItemSelector from '../../Shared/Ui/ItemSelector/ItemSelector';
import StringSelector from '../../Shared/Ui/ItemSelector/StringSelector';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import GetClientsUsedInShipment from './Api/GetClientsUsedInShipment copy';
import Client from '../../Entities/Client';

export const ShipmentPage: React.FC = () => {
  const navigate = useNavigate();

  const [shipment, setShipment] = useState<Shipment[]>([]);
  const [loading, setLoading] = useState(true);
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [numbers, setNumbers] = useState<string[]>([]);
  const [clients, setClients] = useState<Client[]>([]);
  const [start, setStart] = useState<Date | null>(null);
  const [end, setEnd] = useState<Date | null>(null);
  const [selectResources, setSelectResources] = useState<UUID[]>([]);
  const [selectUnits, setSelectUnits] = useState<UUID[]>([]);
  const [selectNumbers, setSelectNumbers] = useState<string[]>([]);
  const [selectClients, setSelectClients] = useState<UUID[]>([]);

  useEffect(() => {
    const fetchShipments = async () => {
      const dataResources = await GetResourcesUsedInShipment();
      setResources(dataResources);
      const dataUnits = await GetUnitsUsedInShipment();
      setUnits(dataUnits);
      const dataNumbers = await GetShipmentNumbers();
      setNumbers(dataNumbers);
      const dataClients = await GetClientsUsedInShipment();
      setClients(dataClients);
    };
    fetchShipments();
  }, []);

  useEffect(() => {
    const fetchShipments = async () => {
      try {
        const data = await GetShipments({numbers: selectNumbers, start: start, end: end, resourcesIds: selectResources, unitsIds: selectUnits});
        setShipment(data);
      } catch (error) {
        console.error('Ошибка при загрузке баланса:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchShipments();
  }, [selectUnits, selectResources, selectNumbers, start, end, clients]);

  const OnRowClick = (id: UUID) => {navigate(`/shipment-update/${id}`)}

  const columns = [
    { header: 'Номер', accessor: 'number' },
    { header: 'Дата', accessor: 'date' },
    { header: 'Клиент', accessor: 'clientName' },
    { header: 'Статус', accessor: 'isSigned', 
      cellRenderer: ({ id, value }: { id: UUID; value: number }) => (
        <>{value? 
          <div style={{
            backgroundColor: '#5a97ecff',
            border: 'none',
            borderRadius: '4px',
            padding: '15px 18px',
        }}>Подписан</div>: 
          <div style={{
            backgroundColor: 'rgba(209, 209, 209, 1)',
            border: 'none',
            borderRadius: '4px',
            padding: '10px 10px',
        }}>Не подписан</div>}
        </>
      )},
    { header: 'Ресурс', accessor: 'resources.resourceName', isResource: true },
    { header: 'Единица измерения', accessor: 'resources.unitName', isResource: true },
    { header: 'Колличество', accessor: 'resources.quantity', isResource: true }
  ];

  return (
    <div className={styles.shipmentPage}>
      <h1>Отгрузки</h1>

      <div className={styles.dateConteyner} >

        <div className={styles.periodTitle}>Период</div>
        <DateSelect selectedDate={start} onDateChange={setStart} />
        <DateSelect selectedDate={end} onDateChange={setEnd} />

      </div>

      <div className={styles.filterConteyner} >
        <StringSelector
          items = {numbers}
          selectedIds = {selectNumbers}
          onSelectionChange = {setSelectNumbers}
          title='Номер отгрузки'
        /> 

        <ItemSelector
          items = {clients}
          selectedIds = {selectClients}
          onSelectionChange = {setSelectClients}
          title='Клиент'
        />

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

      <WarehouseButton color="#5a97ecff" onClick = {() =>{navigate('/shipment-add')}} margin='0 0 40px 0' >Добавить</WarehouseButton>

      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <Table 
          columns={columns} 
          data={shipment} 
          emptyMessage="Нет данных об отгрузках"
          onRowClick={OnRowClick}
        />
      )}
    </div>
  );
};