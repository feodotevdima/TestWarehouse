import Client from "../../../Entities/Client";
import axios from "axios";

const GetArchiveClients = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Client/archive`;
    const response = await axios.get(
        query,
    );
    let json: Client[] = await response.data;
    return json;
}

export default GetArchiveClients;