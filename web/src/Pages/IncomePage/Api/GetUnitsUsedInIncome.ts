import axios from "axios";
import Unit from "../../../Entities/Unit";

const GetUnitsUsedInIncome = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Unit/use-in-incomes`;
    const response = await axios.get(
        query,
    );
    const json: Unit[] = await response.data;
    return json;
}

export default GetUnitsUsedInIncome;