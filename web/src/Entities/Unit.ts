import { UUID } from "crypto";

interface Unit {
    id: UUID;
    name: string;
    isActive: boolean;
}

export default Unit;