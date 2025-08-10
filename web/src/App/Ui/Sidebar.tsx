import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import styles from './Sidebar.module.css';

export const Sidebar: React.FC = () => {
  const location = useLocation();

const isActiveGroup = (basePath: string) => {
    const singularPath = basePath.endsWith('s') 
      ? basePath.slice(0, -1) 
      : basePath;
    
    return (
      location.pathname === basePath ||
      location.pathname.startsWith(`${basePath}/`) ||
      location.pathname.startsWith(`${singularPath}-`)
    );
  };

  return (
    <aside className={styles.sidebar}>
      <h2 className={styles.title}>Управление складом</h2>
      
      <nav>
        <div className={styles.section}>
          <h3 className={styles.sectionTitle}>Склад</h3>
          <ul className={styles.menu}>
            <li>
              <NavLink 
                to="/balance" 
                className={({ isActive }) => 
                  isActive ? `${styles.link} ${styles.active}` : styles.link
                }
              >
                Баланс
              </NavLink>
            </li>
            <li>
              <NavLink 
                to="/incomes" 
                className={isActiveGroup('/incomes') ? `${styles.link} ${styles.active}` : styles.link}
              >
                Поступления
              </NavLink>
            </li>
            <li>
              <NavLink 
                to="/shipments" 
                className={isActiveGroup('/shipments') ? `${styles.link} ${styles.active}` : styles.link}
              >
                Отгрузки
              </NavLink>
            </li>
          </ul>
        </div>
        
        <div className={styles.section}>
          <h3 className={styles.sectionTitle}>Справочники</h3>
          <ul className={styles.menu}>
            <li>
              <NavLink 
                to="/clients" 
                className={isActiveGroup('/clients') ? `${styles.link} ${styles.active}` : styles.link}
              >
                Клиенты
              </NavLink>
            </li>
            <li>
              <NavLink 
                to="/resources" 
                className={isActiveGroup('/resources') ? `${styles.link} ${styles.active}` : styles.link}
              >
                Ресурсы
              </NavLink>
            </li>
            <li>
              <NavLink 
                to="/units" 
                className={isActiveGroup('/units') ? `${styles.link} ${styles.active}` : styles.link}
              >
                Единицы измерения
              </NavLink>
            </li>
          </ul>
        </div>
      </nav>
    </aside>
  );
};