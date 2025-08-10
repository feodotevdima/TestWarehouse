import { UUID } from "crypto";
import { Shipment } from "../../../Entities/Shipment";
import axios from "axios";

const GetShipmentById = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Shipment/id/${id}`;
    const response = await axios.get(
        query,
    );
    const json: Shipment = await response.data;
    return json;
}

export default GetShipmentById;