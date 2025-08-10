import { UUID } from "crypto";

export interface BalanceShipmentResources {
    id: UUID;
    resourceId: UUID;
    resourceName: string;
    unitId: UUID;
    unitName: string;
    availableQuantity: number;
    shipmentQuantity: number;
}