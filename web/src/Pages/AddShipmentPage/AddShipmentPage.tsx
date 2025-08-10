import React, { useState, useEffect } from 'react';
import styles from './AddShipmentPage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import PostShipment from './Api/PostShipment';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';
import DateSelect from '../../Shared/Ui/DateSelect/DateSelect';
import { Table } from '../../Shared/Ui/Table/table';
import { UUID } from 'crypto';
import SingleItemSelector from '../../Shared/Ui/ItemSelector/SingleItemSelector';
import { AddShipment } from '../../Entities/AddShipment';
import Client from '../../Entities/Client';
import GetBalance from '../../Shared/Api/GetBalance';
import { BalanceShipmentResources } from '../../Entities/BalanceShipmentResources';
import GetActiveClients from '../../Shared/Api/GetActiveClients';
import SignedShipment from '../../Shared/Api/SignedShipment';

export const AddShipmentPage: React.FC = () => {
  const navigate = useNavigate();

  const [number, setNumber] = useState<string>('');
  const [clients, setClients] = useState<Client[]>([]);
  const [date, setDate] = useState<Date| null>(new Date());
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [balance, setBalance] = useState<BalanceShipmentResources[]>([]);
  const [selectedClientId, setSelectedClientId] = useState<UUID | null>(null);

  useEffect(() => {
    const fetchShipment = async () => {
      const data = await GetBalance();
      let newData: BalanceShipmentResources[] = [];
      data.forEach(i => {
        const balance: BalanceShipmentResources = 
        {
          id: i.id,
          resourceId: i.resourceId, 
          resourceName:i.resourceName, 
          unitId: i.unitId, 
          unitName: i.unitName, 
          availableQuantity: i.quantity, 
          shipmentQuantity: 0
        }

        newData = [...newData, balance]
      });

      setBalance(newData);

      const clientData = await GetActiveClients();
      setClients(clientData);
    };

    fetchShipment();
  }, []);


  const ShipmentAdd= async () => {
    const shipmentResources = balance
      .filter(i => i.shipmentQuantity > 0)
      .map(i => ({
        id: crypto.randomUUID() as UUID, 
        resourceId: i.resourceId,
        unitId: i.unitId,
        quantity: i.shipmentQuantity
      }));

    const shipment: AddShipment = {number: number, clientId: selectedClientId as UUID,  date: date, resources: shipmentResources}
    const newShipment = await PostShipment(shipment);
    return newShipment
  }

  const SaveShipment = async () => {
    try {
      await ShipmentAdd();
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

  const SaveAndSignedShipment = async () => {
    try {
      const newShipment = await ShipmentAdd();
      if(newShipment)
        await SignedShipment(newShipment.id);
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


  const columns = [
  {
    header: 'Ресурс',
    accessor: 'resourceName',
  },
  {
    header: 'Единица измерения',
    accessor: 'unitName',
  },
  {
    header: 'Количество',
    accessor: 'shipmentQuantity',
    cellRenderer: ({ id, value }: { id: UUID; value: number }) => (
      <FieldInput 
        initialValue={value.toString()}
        onChange={(newValue: string) => {
          const num = Number(newValue)
          if (!isNaN(num)) {
            setBalance(prev => 
              prev.map(item => 
                item.id === id ? { ...item, shipmentQuantity: newValue === '' ? 0 : num } : item
              )
            );
          }
        }}
        onlyNumbers={true}
        maxValue={balance.find(item => item.id == id)?.availableQuantity}
      />
    )
  },
  {
    header: 'Доступно',
    accessor: 'availableQuantity',
  }
];

  return (
    <div className={styles.container}>
      {errorMessage && (
        <ErrorNotification 
          message={errorMessage} 
          onClose={() => setErrorMessage(null)} 
        />
      )}
      <h1>Отгрузка</h1>
      <WarehouseButton color="#5a97ecff" onClick={SaveShipment} margin='20px 0'>Сохранить</WarehouseButton>
      <WarehouseButton color="#5a97ecff" onClick={SaveAndSignedShipment} margin='20px 20px'>Сохранить и подписать</WarehouseButton>
      <FieldInput label='Номер' initialValue={number} onChange={setNumber} />
      <SingleItemSelector title='Клиент' items={clients} selectedId={selectedClientId as UUID} onSelectionChange={setSelectedClientId} />
      <div className={styles.date}>
        <div className={styles.dateTitle}>Дата</div>
        <DateSelect selectedDate={date} onDateChange={setDate}/>
      </div>
      <Table columns={columns} data={balance} />
    </div>
  );
};