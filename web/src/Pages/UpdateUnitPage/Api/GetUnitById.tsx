import { UUID } from "crypto";
import Unit from "../../../Entities/Unit";
import axios from "axios";

const GetUnitById = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Unit/id/${id}`;
    const response = await axios.get(
        query,
    );
    const json: Unit = await response.data;
    return json;
}

export default GetUnitById;