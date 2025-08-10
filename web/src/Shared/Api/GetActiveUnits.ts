import Unit from "../../Entities/Unit";
import axios from "axios";

const GetActiveUnits = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Unit/active`;
    const response = await axios.get(
        query,
    );
    const json: Unit[] = await response.data;
    return json;
}

export default GetActiveUnits;