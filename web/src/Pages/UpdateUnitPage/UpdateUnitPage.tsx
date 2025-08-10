import React, { useState, useEffect } from 'react';
import styles from './UpdateUnitPage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import GetUnitById from './Api/GetUnitById';
import { UUID } from 'crypto';
import UpdateUnit from './Api/UpdateUnit';
import ChangeUnitStatus from './Api/ChangeUnitStatus';
import DeleteUnit from './Api/DeleteUnit';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const UpdateUnitPage: React.FC = () => {
  const navigate = useNavigate();
  const { unitIdParam } = useParams();

  const [loading, setLoading] = useState(true);
  const [unitId, setUnitId] = useState<UUID>();
  const [unitName, setUnitName] = useState<string>('');
  const [unitIsActive, setUnitIsActive] = useState<boolean>(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

   useEffect(() => {
    const fetchUnits = async () => {
      try {
        const data = await GetUnitById(unitIdParam as `${string}-${string}-${string}-${string}-${string}`);
        setUnitId(data.id);
        setUnitName(data.name);
        setUnitIsActive(data.isActive);
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

    fetchUnits();
  }, []);

  const SaveUnit = async () => {
    if(unitId === undefined)
      return;
    try {
      await UpdateUnit({id: unitId, name: unitName, isActive: unitIsActive});
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

  const DelUnit = async () => {
    if(unitId === undefined)
      return;
    try {
      await DeleteUnit(unitId);
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

  const ArchiveUnit = async () => {
    if(unitId === undefined)
      return;
    try {
      await ChangeUnitStatus(unitId);
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

  return (
    <div className={styles.container}>
      {errorMessage && (
        <ErrorNotification 
          message={errorMessage} 
          onClose={() => setErrorMessage(null)} 
        />
      )}
      <h1>Единица измерения</h1>
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <div>
          <FieldInput label='Наименование' initialValue={unitName} onChange={setUnitName} />
          <WarehouseButton color="#5a97ecff" onClick={SaveUnit} margin='20px 0'>Сохранить</WarehouseButton>
          <WarehouseButton color="#fc6e6eff" onClick={DelUnit} margin='20px 15px'>Удалить</WarehouseButton>
          <WarehouseButton color="rgba(209, 209, 209, 1)" onClick={ArchiveUnit} margin='20px 0'>{unitIsActive? 'В архив' : 'В работу'}</WarehouseButton>
        </div>)}
    </div>
  );
};