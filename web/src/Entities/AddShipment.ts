import { UUID } from "crypto";
import { DO_NOT_USE_OR_YOU_WILL_BE_FIRED_CALLBACK_REF_RETURN_VALUES } from "react";

interface AddShipment {
    id?: UUID;
    number: string;
    clientId: UUID;
    date: Date | null;
    resources: AddShipmentResource[]
}

interface AddShipmentResource {
    id: UUID;
    shipmentId?: UUID;
    resourceId: UUID;
    unitId: UUID;
    quantity: Number
}

export type {AddShipment, AddShipmentResource};