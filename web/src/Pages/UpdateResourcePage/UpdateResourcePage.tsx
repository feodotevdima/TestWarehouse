import React, { useState, useEffect } from 'react';
import styles from './UpdateResourcePage.module.css';
import { FieldInput } from '../../Shared/Ui/FieldInput/FieldInput';
import { WarehouseButton } from '../../Shared/Ui/Buttons/WarehouseButton';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import GetResourceById from './Api/GetResourceById';
import { UUID } from 'crypto';
import UpdateResource from './Api/UpdateResource';
import ChangeResourceStatus from './Api/ChangeResourceStatus';
import DeleteResource from './Api/DeleteResource';
import ErrorNotification from '../../Shared/Ui/ErrorNotification/ErrorNotification';

export const UpdateResourcePage: React.FC = () => {
  const navigate = useNavigate();
  const { resourceIdParam } = useParams();

  const [loading, setLoading] = useState(true);
  const [resourceId, setResourceId] = useState<UUID>();
  const [resourceName, setResourceName] = useState<string>('');
  const [resourceIsActive, setResourceIsActive] = useState<boolean>(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

   useEffect(() => {
    const fetchResources = async () => {
      try {
        const data = await GetResourceById(resourceIdParam as `${string}-${string}-${string}-${string}-${string}`);
        setResourceId(data.id);
        setResourceName(data.name);
        setResourceIsActive(data.isActive);
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

    fetchResources();
  }, []);

  const SaveResource = async () => {
    if(resourceId === undefined)
      return;
    try {
      await UpdateResource({id: resourceId, name: resourceName, isActive: resourceIsActive});
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

  const DelResource = async () => {
    if(resourceId === undefined)
      return;
    try {
      await DeleteResource(resourceId);
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

  const ArchiveResource = async () => {
    if(resourceId === undefined)
      return;
    try {
      await ChangeResourceStatus(resourceId);
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
      <h1>Ресурс</h1>
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <div>
          <FieldInput label='Наименование' initialValue={resourceName} onChange={setResourceName} />
          <WarehouseButton color="#5a97ecff" onClick={SaveResource} margin='20px 0'>Сохранить</WarehouseButton>
          <WarehouseButton color="#fc6e6eff" onClick={DelResource} margin='20px 15px'>Удалить</WarehouseButton>
          <WarehouseButton color="rgba(209, 209, 209, 1)" onClick={ArchiveResource} margin='20px 0'>{resourceIsActive? 'В архив' : 'В работу'}</WarehouseButton>
        </div>)}
    </div>
  );
};