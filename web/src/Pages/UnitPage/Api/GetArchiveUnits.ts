import Unit from "../../../Entities/Unit";
import axios from "axios";

const GetArchiveUnits = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Unit/archive`;
    const response = await axios.get(
        query,
    );
    let json: Unit[] = await response.data;
    return json;
}

export default GetArchiveUnits;