import { UUID } from "crypto";

interface Income {
    id: UUID;
    number: string;
    date: Date;
    resources: IncomeResource[]
}

interface IncomeResource {
    id: UUID;
    incomeId: UUID;
    resourceId: UUID;
    resourceName: string;
    unitId: UUID;
    unitName: string;
    quantity: Number
}

export type {Income, IncomeResource};