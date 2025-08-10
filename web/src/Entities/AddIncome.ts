import { UUID } from "crypto";

interface AddIncome {
    id?: UUID;
    number: string;
    date?: Date | null;
    resources: AddIncomeResource[]
}

interface AddIncomeResource {
    id: UUID;
    incomeId?: UUID;
    resourceId: UUID;
    unitId: UUID;
    quantity: Number
}

export type {AddIncome, AddIncomeResource};