import Client from "../../Entities/Client";
import axios from "axios";

const GetActiveClients = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Client/active`;
    const response = await axios.get(
        query,
    );
    const json: Client[] = await response.data;
    return json;
}

export default GetActiveClients;