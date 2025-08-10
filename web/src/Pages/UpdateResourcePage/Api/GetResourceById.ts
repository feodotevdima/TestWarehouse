import { UUID } from "crypto";
import Resource from "../../../Entities/Resource";
import axios from "axios";

const GetResourceById = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Resource/id/${id}`;
    const response = await axios.get(
        query,
    );
    const json: Resource = await response.data;
    return json;
}

export default GetResourceById;