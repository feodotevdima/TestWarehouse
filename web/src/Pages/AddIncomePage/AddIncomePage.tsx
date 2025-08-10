import React, { useState, useEffect } from 'react';
import styles from './AddIncomePage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import PostIncome from './Api/PostIncome';
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

export const AddIncomePage: React.FC = () => {
  const navigate = useNavigate();

  const [number, setNumber] = useState<string>('');
  const [date, setDate] = useState<Date| null>(new Date());
  const [incomeResources, setIncomeResources] = useState<AddIncomeResource[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);

  useEffect(() => {
    const fetchBalance = async () => {
      const dataResources = await GetActiveResources();
      setResources(dataResources);
      const dataUnits = await GetActiveUnits();
      setUnits(dataUnits);
    };

    fetchBalance();
  }, []);


  const SaveIncome = async () => {
    try {
      const income: AddIncome = {number: number, date: date, resources: incomeResources}
      await PostIncome(income);
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
    cellRenderer: ({ id, value }: { id: UUID; value: UUID }) => (
      <SingleItemSelector<Resource>
        items={resources}
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
    )
  },
  {
    header: 'Единица измерения',
    accessor: 'unitId',
    cellRenderer: ({ id, value }: { id: UUID; value: UUID }) => (
      <SingleItemSelector<Unit>
        items={units}
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
      />
    )
  },
  {
    header: 'Количество',
    accessor: 'quantity',
    cellRenderer: ({ id, value }: { id: UUID; value: number }) => (
      <FieldInput 
        initialValue={value.toString()}
        onChange={(newValue: string) => {
          const num = Number(newValue)
          if (!isNaN(num)) {
            setIncomeResources(prev => 
              prev.map(item => 
                item.id === id ? { ...item, quantity: newValue === '' ? 0 : num } : item
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
      <h1>Поступление</h1>
      <FieldInput label='Номер' initialValue={number} onChange={setNumber} />
      <div className={styles.date}>
        <div className={styles.dateTitle}>Дата</div>
        <DateSelect selectedDate={date} onDateChange={setDate}/>
      </div>
      <Table columns={columns} data={incomeResources} />
      <WarehouseButton color="#5a97ecff" onClick={SaveIncome} margin='20px 0'>Сохранить</WarehouseButton>
    </div>
  );
};