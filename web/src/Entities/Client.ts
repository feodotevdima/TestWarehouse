import { UUID } from "crypto";

interface Client {
    id: UUID;
    name: string;
    address: string;
    isActive: boolean;
}

export default Client;