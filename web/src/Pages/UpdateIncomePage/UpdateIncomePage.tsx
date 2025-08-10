import React, { useState, useEffect } from 'react';
import styles from './UpdateIncomePage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import UpdateIncome from './Api/UpdateIncome';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';
import DateSelect from '../../Shared/Ui/DateSelect/DateSelect';
import { Table } from '../../Shared/Ui/Table/table';
import { AddIncomeResource } from '../../Entities/AddIncome';
import { UUID } from 'crypto';
import Unit from '../../Entities/Unit';
import Resource from '../../Entities/Resource';
import GetActiveResources from '../../Shared/Api/GetActiveResources';
import GetActiveUnits from '../../Shared/Api/GetActiveUnits';
import SingleItemSelector from '../../Shared/Ui/ItemSelector/SingleItemSelector';
import { AddIncome } from '../../Entities/AddIncome';
import { useParams } from 'react-router-dom';
import GetIncomeById from './Api/GetIncomeById';
import GetArchiveResources from '../ResourcePage/Api/GetArchiveResources';
import GetArchiveUnits from '../UnitPage/Api/GetArchiveUnits';
import DeleteIncome from './Api/DeleteIncome';

export const UpdateIncomePage: React.FC = () => {
  const navigate = useNavigate();
  const { incomeIdParam } = useParams();

  const [loading, setLoading] = useState(true);
  const [id, setId] = useState<UUID>();
  const [number, setNumber] = useState<string>('');
  const [date, setDate] = useState<Date| null>(new Date());
  const [incomeResources, setIncomeResources] = useState<AddIncomeResource[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [archiveResources, setArchiveResources] = useState<Resource[]>([]);
  const [archiveUnits, setArchiveUnits] = useState<Unit[]>([]);

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const data = await GetIncomeById(incomeIdParam as UUID);
        setId(data.id)
        setNumber(data.number);
        setDate(new Date(data.date));
        setIncomeResources(data.resources);

        const dataResources = await GetActiveResources();
        setResources(dataResources);
        const dataUnits = await GetActiveUnits();
        setUnits(dataUnits);

        const dataArchiveResources = await GetArchiveResources();
        setArchiveResources(dataArchiveResources);
        const dataArchiveUnits = await GetArchiveUnits();
        setArchiveUnits(dataArchiveUnits);
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

    fetchBalance();
  }, []);


  const SaveIncome = async () => {
    try {
      const income: AddIncome = {id: id, number: number, date: date, resources: incomeResources}
      await UpdateIncome(income);
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

  const DelIncome  = async () => {
    try {
      if(id)
      {
        await DeleteIncome(id);
        navigate(-1);
      }
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

  const addNewResource = () => {
    const newResource: AddIncomeResource = {
      id: crypto.randomUUID() as UUID, 
      resourceId: resources[0].id,
      unitId: units[0].id,
      quantity: 0
    };
    setIncomeResources([...incomeResources, newResource]);
  };

  const removeResource = (id: UUID) => {
    setIncomeResources(incomeResources.filter(resource => resource.id !== id));
  };


  const columns = [
  {
    header: <button 
              onClick={addNewResource}
              style={{
                backgroundColor: '#5a97ecff',
                border: 'none',
                borderRadius: '4px',
                padding: '15px 18px',
                cursor: 'pointer',
                fontSize: '20px'
              }}
            >
              +
            </button>,
    accessor: '',
    cellRenderer: ({ id }: { value: string; id: UUID }) => (
      <button 
        onClick={() => removeResource(id)}
        style={{
          backgroundColor: '#fc6e6eff',
          border: 'none',
          borderRadius: '4px',
          padding: '15px 18px',
          cursor: 'pointer',
          fontSize: '20px'
        }}
      >
        -
      </button>
    )
  },
  {
  header: 'Ресурс',
  accessor: 'resourceId',
  cellRenderer: ({ id, value }: { id: UUID; value: UUID }) => {

  const resourceExists = resources.some(res => res.id === value);

  let itemsToRender = resources;

  if (!resourceExists) {
    const foundItem = archiveResources.find(item => item.id === value);
    if(foundItem)
      itemsToRender = [...itemsToRender, foundItem]
  }

  return (
    <SingleItemSelector<Resource>
      items={itemsToRender}
      selectedId={value}
      onSelectionChange={(selectedId) => {
      if (selectedId) {
        setIncomeResources(prev => 
          prev.map(item => 
            item.id === id ? { ...item, resourceId: selectedId } : item
          )
        );
      }
    }}
  />
  );
}
},
  {
    header: 'Единица измерения',
    accessor: 'unitId',
    cellRenderer: ({ id, value }: { id: UUID; value: UUID }) => {

      const unitExists = units.some(res => res.id === value);

      let itemsToRender = units;

      if (!unitExists) {
        const foundItem = archiveUnits.find(item => item.id === value);
        if(foundItem)
          itemsToRender = [...itemsToRender, foundItem]
      }

      return(
      <SingleItemSelector<Unit>
        items={itemsToRender}
        selectedId={value}
        onSelectionChange={(selectedId) => {
          if (selectedId) {
            setIncomeResources(prev => 
              prev.map(item => 
                item.id === id ? { ...item, unitId: selectedId } : item
              )
            );
          }
        }}
      />)
    }
  },
  {
    header: 'Количество',
    accessor: 'quantity',
    cellRenderer: ({ id, value }: { id: UUID; value: number }) => (
      <FieldInput 
        initialValue={value.toString()}
        onChange={(newValue: string) => {
          if (newValue === '' || /^\d+$/.test(newValue)) {
            setIncomeResources(prev => 
              prev.map(item => 
                item.id === id ? { ...item, quantity: newValue === '' ? 0 : Number(newValue) } : item
              )
            );
          }
        }}
        onlyNumbers={true}
      />
    )
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
            {loading ? (
        <div>Загрузка...</div>
      ) : (
      <>
        <h1>Поступление</h1>
        <FieldInput label='Номер' initialValue={number} onChange={setNumber} />
        <div className={styles.date}>
          <div className={styles.dateTitle}>Дата</div>
          <DateSelect selectedDate={date} onDateChange={setDate}/>
        </div>
        <Table columns={columns} data={incomeResources} />
        <WarehouseButton color="#5a97ecff" onClick={SaveIncome} margin='40px 0'>Сохранить</WarehouseButton>
        <WarehouseButton color="#fc6e6eff" onClick={DelIncome} margin='40px 20px'>Удалить</WarehouseButton>
      </>)}
    </div>
   );
};