import { UUID } from "crypto";
import Client from "../../../Entities/Client";
import axios from "axios";

const GetClientById = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Client/id/${id}`;
    const response = await axios.get(
        query,
    );
    const json: Client = await response.data;
    return json;
}

export default GetClientById;