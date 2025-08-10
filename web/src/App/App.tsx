import React from 'react';
import './App.css';
import { Sidebar } from './Ui/Sidebar';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { BalancePage } from '../Pages/BalncePage/BalancePage';
import { ClientPage } from '../Pages/ClientPage/ClientPage';
import { IncomePage } from '../Pages/IncomePage/IncomePage';
import { AddClientPage } from '../Pages/AddClientPage/AddClientPage';
import { UpdateClientPage } from '../Pages/UpdateClientPage/UpdateClientPage';
import { UnitPage } from '../Pages/UnitPage/UnitPage';
import { UpdateUnitPage } from '../Pages/UpdateUnitPage/UpdateUnitPage';
import { AddUnitPage } from '../Pages/AddUnitPage/AddUnitPage';
import { ResourcePage } from '../Pages/ResourcePage/ResourcePage';
import { UpdateResourcePage } from '../Pages/UpdateResourcePage/UpdateResourcePage';
import { AddResourcePage } from '../Pages/AddResourcePage/AddResourcePage';
import { AddIncomePage } from '../Pages/AddIncomePage/AddIncomePage';
import { UpdateIncomePage } from '../Pages/UpdateIncomePage/UpdateIncomePage';
import { ShipmentPage } from '../Pages/ShipmentPage/ShipmentPage';
import { AddShipmentPage } from '../Pages/AddShipmentPage/AddShipmentPage';
import { UpdateShipmentPage } from '../Pages/UpdateShipmentPage/UpdateShipmentPage';

function App() {
  return (
    <BrowserRouter>
      <div className="app-container">
        <Sidebar />
        <div className="content">
          <Routes>
            <Route path="/balance" element={<BalancePage />} />

            <Route path="/clients" element={<ClientPage />} />
            <Route path="/client-add" element={<AddClientPage />} />
            <Route path="/client-update/:clientIdParam" element={<UpdateClientPage />} />

            <Route path="/units" element={<UnitPage />} />
            <Route path="/unit-add" element={<AddUnitPage />} />
            <Route path="/unit-update/:unitIdParam" element={<UpdateUnitPage />} />

            <Route path="/resources" element={<ResourcePage />} />
            <Route path="/resource-add" element={<AddResourcePage />} />
            <Route path="/resource-update/:resourceIdParam" element={<UpdateResourcePage />} />

            <Route path="/incomes" element={<IncomePage />} />
            <Route path="/income-add" element={<AddIncomePage />} />
            <Route path="/income-update/:incomeIdParam" element={<UpdateIncomePage />} />

            <Route path="/shipments" element={<ShipmentPage />} />
            <Route path="/shipment-add" element={<AddShipmentPage />} />
            <Route path="/shipment-update/:shipmentIdParam" element={<UpdateShipmentPage />} />

            <Route path="*" element={<Navigate to="/balance" replace />} />
          </Routes>
        </div>
      </div>
    </BrowserRouter>
  );

}

export default App;
