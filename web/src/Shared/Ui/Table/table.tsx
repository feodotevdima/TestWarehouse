import React, { useState } from 'react';
import styles from './Table.module.css';
import { UUID } from 'crypto';

interface TableColumn{
  header: string | React.ReactNode;
  accessor: string;
  isResource?: boolean;
  cellRenderer?: (data: { 
    value: any, 
    id: UUID 
  }) => React.ReactNode;
}

interface TableProps<T extends { id: UUID }> {
  columns: TableColumn[];
  data: T[];
  resourcePath?: string;
  emptyMessage?: React.ReactNode;
  onRowClick?: (id: UUID) => void;
}

export const Table = <T extends { id: UUID }>({
  columns,
  data,
  resourcePath = 'resources',
  emptyMessage = 'Нет данных',
  onRowClick,
}: TableProps<T>) => {
  const [hoveredGroup, setHoveredGroup] = useState<UUID | null>(null);
  
  const getValue = (obj: any, path: string): any => {
    return path.split('.').reduce((acc, part) => acc?.[part], obj);
  };

  const renderCell = (column: TableColumn, value: any, id: UUID) => {
    if (column.cellRenderer) {
      return column.cellRenderer({ value, id });
    }
    return value != null ? String(value) : '-';
  };

  return (
    <div className={styles.tableWrapper}>
      <table className={styles.table}>
        <thead>
          <tr>
            {columns.map((column) => (
              <th key={column.accessor}>{column.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.length > 0 ? (
            data.flatMap((item) => {
              const resources = getValue(item, resourcePath) as Array<any> ?? [];
              const hasResources = resources.length > 0;
              const isGroupHovered = hoveredGroup === item.id;

              if (!hasResources) {
                return (
                  <tr 
                    key={item.id} 
                    className={isGroupHovered ? styles.hoveredRow : ''}
                    onMouseEnter={() => setHoveredGroup(item.id)}
                    onMouseLeave={() => setHoveredGroup(null)}
                    onClick={() => onRowClick?.(item.id)}
                  >
                    {columns.map((column) => {
                      const value = getValue(item, column.accessor);
                      return (
                        <td key={`${item.id}-${column.accessor}`}>
                          {renderCell(column, value, item.id)}
                        </td>
                      );
                    })}
                  </tr>
                );
              }

              const mainRow = (
                <tr 
                  key={item.id} 
                  className={isGroupHovered ? styles.hoveredRow : ''}
                  onMouseEnter={() => setHoveredGroup(item.id)}
                  onMouseLeave={() => setHoveredGroup(null)}
                  onClick={() => onRowClick?.(item.id)}
                >
                  {columns.map((column) => {
                    if (column.isResource) {
                      const firstResource = resources[0];
                      const resourceValue = getValue(firstResource, column.accessor.replace(`${resourcePath}.`, ''));
                      return (
                        <td key={`${item.id}-${column.accessor}-0`}>
                          {renderCell(column, resourceValue, item.id)}
                        </td>
                      );
                    } else {
                      const value = getValue(item, column.accessor);
                      return (
                        <td 
                          key={`${item.id}-${column.accessor}`} 
                          rowSpan={resources.length}
                        >
                          {renderCell(column, value, item.id)}
                        </td>
                      );
                    }
                  })}
                </tr>
              );

              const resourceRows = resources.slice(1).map((resource, index) => (
                <tr 
                  key={`${item.id}-res-${index + 1}`}
                  className={isGroupHovered ? styles.hoveredRow : ''}
                  onMouseEnter={() => setHoveredGroup(item.id)}
                  onMouseLeave={() => setHoveredGroup(null)}
                  onClick={() => onRowClick?.(item.id)}
                >
                  {columns.map((column) => {
                    if (!column.isResource) return null;
                    const value = getValue(resource, column.accessor.replace(`${resourcePath}.`, ''));
                    return (
                      <td key={`${item.id}-${column.accessor}-${index + 1}`}>
                        {renderCell(column, value, item.id)}
                      </td>
                    );
                  })}
                </tr>
              ));

              return [mainRow, ...resourceRows];
            })
          ) : (
            <tr>
              <td colSpan={columns.length} className={styles.emptyCell}>
                {emptyMessage}
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};