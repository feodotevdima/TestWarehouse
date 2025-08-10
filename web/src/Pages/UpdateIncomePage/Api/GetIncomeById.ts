import { UUID } from "crypto";
import { Income } from "../../../Entities/Income";
import axios from "axios";

const GetIncomeById = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Income/id/${id}`;
    const response = await axios.get(
        query,
    );
    const json: Income = await response.data;
    return json;
}

export default GetIncomeById;