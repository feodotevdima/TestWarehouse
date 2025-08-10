import { UUID } from "crypto";

interface Balance {
    id: UUID;
    resourceId: UUID;
    resourceName: string;
    unitId: UUID;
    unitName: string;
    quantity: number;
}

export default Balance;