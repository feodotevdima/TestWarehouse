import React, { useEffect, useState } from 'react';
import { Table } from '../../Shared/Ui/Table/table';
import GetIncomes from './Api/GetIncomes';
import { Income } from '../../Entities/Income';
import styles from './IncomePage.module.css';
import { UUID } from 'crypto';
import DateSelect from '../../Shared/Ui/DateSelect/DateSelect';
import Unit from '../../Entities/Unit';
import Resource from '../../Entities/Resource';
import GetResourcesUsedInIncome from './Api/GetResourcesUsedInIncome';
import GetUnitsUsedInIncome from './Api/GetUnitsUsedInIncome';
import GetIncomeNumbers from './Api/GetIncomeNumbers';
import ItemSelector from '../../Shared/Ui/ItemSelector/ItemSelector';
import StringSelector from '../../Shared/Ui/ItemSelector/StringSelector';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';

export const IncomePage: React.FC = () => {
  const navigate = useNavigate();

  const [income, setIncome] = useState<Income[]>([]);
  const [loading, setLoading] = useState(true);
  const [resources, setResources] = useState<Resource[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [numbers, setNumbers] = useState<string[]>([]);
  const [start, setStart] = useState<Date | null>(null);
  const [end, setEnd] = useState<Date | null>(null);
  const [selectResources, setSelectResources] = useState<UUID[]>([]);
  const [selectUnits, setSelectUnits] = useState<UUID[]>([]);
  const [selectNumbers, setSelectNumbers] = useState<string[]>([]);

  useEffect(() => {
    const fetchIncomes = async () => {
      const dataResources = await GetResourcesUsedInIncome();
      setResources(dataResources);
      const dataUnits = await GetUnitsUsedInIncome();
      setUnits(dataUnits);
      const dataNumbers = await GetIncomeNumbers();
      setNumbers(dataNumbers);
    };
    fetchIncomes();
  }, []);

  useEffect(() => {
    const fetchIncomes = async () => {
      try {
        const data = await GetIncomes({numbers: selectNumbers, start: start, end: end, resourcesIds: selectResources, unitsIds: selectUnits});
        setIncome(data);
      } catch (error) {
        console.error('Ошибка при загрузке баланса:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchIncomes();
  }, [selectUnits, selectResources, selectNumbers, start, end]);

  const OnRowClick = (id: UUID) => {navigate(`/income-update/${id}`)}

  const columns = [
    { header: 'Номер', accessor: 'number' },
    { header: 'Дата', accessor: 'date' },
    { header: 'Ресурс', accessor: 'resources.resourceName', isResource: true },
    { header: 'Единица измерения', accessor: 'resources.unitName', isResource: true },
    { header: 'Колличество', accessor: 'resources.quantity', isResource: true }
  ];

  return (
    <div className={styles.incomePage}>
      <h1>Поступления</h1>

      <div className={styles.filterConteyner} >
        <div className={styles.dateConteyner} >

          <div className={styles.periodTitle}>Период</div>
          <DateSelect selectedDate={start} onDateChange={setStart} />
          <DateSelect selectedDate={end} onDateChange={setEnd} />

        </div>

        <StringSelector
          items = {numbers}
          selectedIds = {selectNumbers}
          onSelectionChange = {setSelectNumbers}
          title='Номер поступления'
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

      <WarehouseButton color="#5a97ecff" onClick = {() =>{navigate('/income-add')}} margin='0 0 40px 0' >Добавить</WarehouseButton>

      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <Table 
          columns={columns} 
          data={income} 
          emptyMessage="Нет данных о поступлениях"
          onRowClick={OnRowClick}
        />
      )}
    </div>
  );
};