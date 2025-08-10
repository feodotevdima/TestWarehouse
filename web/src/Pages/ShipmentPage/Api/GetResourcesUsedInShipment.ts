import axios from "axios";
import Resource from "../../../Entities/Resource";

const GetResourcesUsedInShipment = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Resource/use-in-shipments`;
    const response = await axios.get(
        query,
    );
    const json: Resource[] = await response.data;
    return json;
}

export default GetResourcesUsedInShipment;