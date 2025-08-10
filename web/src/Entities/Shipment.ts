import { UUID } from "crypto";

interface Shipment {
    id: UUID;
    number: string;
    clientId: UUID;
    clientName: UUID;
    date: Date;
    isSigned: boolean
    resources: ShipmentResource[]
}

interface ShipmentResource {
    id: UUID;
    shipmentId: UUID;
    resourceId: UUID;
    resourceName: string;
    unitId: UUID;
    unitName: string;
    quantity: number
}

export type {Shipment, ShipmentResource};