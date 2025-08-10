import { UUID } from "crypto";
import { Shipment } from "../../../Entities/Shipment";
import axios from "axios";
import { format } from 'date-fns';

interface Filters {
  numbers?: string[];
  start?: Date | null;
  end?: Date | null;
  resourcesIds?: UUID[];
  unitsIds?: UUID[];
  clientsIds?: UUID[];
}

const GetShipments = async ({numbers, start, end, resourcesIds, unitsIds, clientsIds}: Filters = {}) =>{
    let query = `${process.env.REACT_APP_API_URL}Shipment?`;

    if(start != null)
        query += `start=${format(start, 'MM-dd-yyyy')}&`;

    if(end != null)
        query += `end=${format(end, 'MM-dd-yyyy')}&`;

    if(numbers != null && numbers.length > 0)
    {
        numbers.map((item) => {
            query += `number=${item}&`
        })
    }

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

    if(clientsIds != null && clientsIds.length > 0)
    {
        clientsIds.map((item) => {
            query += `client_id=${item}&`
        })
    }

    const response = await axios.get(
        query,
    );
    const json: Shipment[] = await response.data;
    return json;
}

export default GetShipments;