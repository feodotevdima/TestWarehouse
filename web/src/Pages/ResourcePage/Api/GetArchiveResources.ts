import Resource from "../../../Entities/Resource";
import axios from "axios";

const GetArchiveResources = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Resource/archive`;
    const response = await axios.get(
        query,
    );
    let json: Resource[] = await response.data;
    return json;
}

export default GetArchiveResources;