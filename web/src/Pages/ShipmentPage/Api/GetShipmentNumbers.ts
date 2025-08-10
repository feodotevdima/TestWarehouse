import axios from "axios";

const GetShipmentNumbers = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Shipment/numbers`;
    const response = await axios.get(
        query,
    );
    const json: string[] = await response.data;
    return json;
}

export default GetShipmentNumbers;