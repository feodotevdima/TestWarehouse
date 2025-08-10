import Resource from "../../Entities/Resource";
import axios from "axios";

const GetActiveResources = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Resource/active`;
    const response = await axios.get(
        query,
    );
    const json: Resource[] = await response.data;
    return json;
}

export default GetActiveResources;