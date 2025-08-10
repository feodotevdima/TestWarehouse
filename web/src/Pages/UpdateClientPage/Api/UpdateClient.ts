import Client from "../../../Entities/Client";
import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";

const UpdateClient = async (client: Client) =>{
    const query = `${process.env.REACT_APP_API_URL}Client`;
    try {
        const response = await axios.put(
            query,
            {
                'id': client.id,
                'name': client.name,
                'address': client.address,
                'isActive': client.isActive
            }
        );
        return response.status;
    } catch (error) {
        const axiosError = error as AxiosError<ApiErrorResponse>;
        if (axiosError.response && axiosError.response.status != 200) {
            throw new Error(axiosError.response.data.detail);
        }
    }
}

export default UpdateClient;