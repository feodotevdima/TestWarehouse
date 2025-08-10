import React, { useState, useEffect } from 'react';
import styles from './UpdateShipmentPage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
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
import { useParams } from 'react-router-dom';
import { Shipment } from '../../Entities/Shipment';
import GetShipmentById from './Api/GetShipmentById';
import UpdateShipment from './Api/UpdateShipment';
import DeleteShipment from './Api/DeleteShipment';
import GetClientById from '../UpdateClientPage/Api/GetCientById';

export const UpdateShipmentPage: React.FC = () => {
  const navigate = useNavigate();
  const { shipmentIdParam } = useParams();

  const [id, setId] = useState<UUID | null>(null);
  const [number, setNumber] = useState<string>('');
  const [clients, setClients] = useState<Client[]>([]);
  const [date, setDate] = useState<Date | null>(new Date());
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [balance, setBalance] = useState<BalanceShipmentResources[]>([]);
  const [selectedClientId, setSelectedClientId] = useState<UUID | null>(null);
  const [shipment, setShipment] = useState<Shipment>();

  useEffect(() => {
    const fetchShipment = async () => {
      const dataShipment = await GetShipmentById(shipmentIdParam as UUID);
      setShipment(dataShipment);

      setId(dataShipment.id)
      setNumber(dataShipment.number);
      setSelectedClientId(dataShipment.clientId);
      setDate(new Date(dataShipment.date))

      const data = await GetBalance();
      let newData: BalanceShipmentResources[] = [];
      data.forEach(i => {
        const balance: BalanceShipmentResources =
        {
          id: i.id,
          resourceId: i.resourceId,
          resourceName: i.resourceName,
          unitId: i.unitId,
          unitName: i.unitName,
          availableQuantity: i.quantity,
          shipmentQuantity: dataShipment.resources.find(item => (i.resourceId == item.resourceId && i.unitName == item.unitName))?.quantity || 0
        }

        newData = [...newData, balance]
      });

      setBalance(newData);

      let clientData = await GetActiveClients();

      const existsInClient = clientData.some(
        i => i.id == dataShipment.clientId);

      if(!existsInClient)
      {
        const newClient = await GetClientById(dataShipment.clientId);
        clientData = [...clientData, newClient];
      }

      setClients(clientData);
    };

    fetchShipment();
  }, []);


  const ShipmentAdd = async () => {
    const shipmentResources = balance
      .filter(i => i.shipmentQuantity > 0)
      .map(i => ({
        id: crypto.randomUUID() as UUID,
        shipmentId: id as UUID,
        resourceId: i.resourceId as UUID,
        unitId: i.unitId as UUID,
        quantity: i.shipmentQuantity
      }));

    const shipment: AddShipment = { id: id as UUID, number: number, clientId: selectedClientId as UUID, date: date, resources: shipmentResources }
    const newShipment = await UpdateShipment(shipment);
    return newShipment
  }

  const SaveShipment = async () => {
    try {
      await ShipmentAdd();
      navigate(-1);
    }
    catch (error) {
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
      if (newShipment)
        await SignedShipment(newShipment.id);
      navigate(-1);
    }
    catch (error) {
      if (error instanceof Error) {
        setErrorMessage(error.message);
      } else {
        setErrorMessage(String(error));
      }
    }
  }

  const Del = async () => {
    try {
      await DeleteShipment(id as UUID)
      navigate(-1);
    }
    catch (error) {
      if (error instanceof Error) {
        setErrorMessage(error.message);
      } else {
        setErrorMessage(String(error));
      }
    }
  }

  if (shipment?.isSigned) {
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
        header: 'Колличетво',
        accessor: 'quantity',
      },
    ]

    if (!shipment) return <div>Загрузка...</div>;
    console.log(shipment.clientName)
    return (
      <div className={styles.container}>
        {errorMessage && (
          <ErrorNotification
            message={errorMessage}
            onClose={() => setErrorMessage(null)}
          />
        )}

        <h1>Отгрузка</h1>
        <WarehouseButton color="#fc6e6eff" onClick={() => { SignedShipment(shipment.id); navigate(-1); }} margin='20px 20px'>Отозвать</WarehouseButton>
        <div className={styles.info}>Номер: {number}</div>
        <div className={styles.info}>Клиент: {shipment.clientName}</div>
        <Table columns={columns} data={shipment.resources} />
      </div>
    )
  }
  else {
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
    if (!shipment) return <div>Загрузка...</div>;
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
        <WarehouseButton color="#fc6e6eff" onClick={Del} margin='20px 20px'>Удалить</WarehouseButton>
        <FieldInput label='Номер' initialValue={number} onChange={setNumber} />
        <SingleItemSelector title='Клиент' items={clients} selectedId={selectedClientId as UUID} onSelectionChange={setSelectedClientId} />
        <div className={styles.date}>
          <div className={styles.dateTitle}>Дата</div>
          <DateSelect selectedDate={date} onDateChange={setDate} />
        </div>
        <Table columns={columns} data={balance} />
      </div>
    );
  }
};