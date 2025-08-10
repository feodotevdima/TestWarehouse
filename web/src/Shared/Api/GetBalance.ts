import { UUID } from "crypto";
import Balance from "../../Entities/Balance";
import axios from "axios";

interface Filters {
  resourcesIds?: UUID[];
  unitsIds?: UUID[];
}

const GetBalance = async ({resourcesIds, unitsIds}: Filters = {}) =>{
    let query = `${process.env.REACT_APP_API_URL}Balance?`;

    if(resourcesIds != null && resourcesIds.length > 0)
    {
        resourcesIds.map((item) => {
            query += `resource_id=${item}&`
        })
    }

    if(unitsIds != null && unitsIds.length > 0)
    {
        unitsIds.map((item) => {
            query += `unit_id=${item}&`
        })
    }

    const response = await axios.get(
        query,
    );
    const json: Balance[] = await response.data;
    return json;
}

export default GetBalance;