import { UUID } from "crypto";

interface Resource {
    id: UUID;
    name: string;
    isActive: boolean;
}

export default Resource;