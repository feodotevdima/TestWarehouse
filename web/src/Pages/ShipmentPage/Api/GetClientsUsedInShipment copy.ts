import axios from "axios";
import Client from "../../../Entities/Client";

const GetClientsUsedInShipment = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Client/use-in-shipments`;
    const response = await axios.get(
        query,
    );
    const json: Client[] = await response.data;
    return json;
}

export default GetClientsUsedInShipment;