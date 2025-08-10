import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";

interface addClient {
  id?: string; 
  name: string;
  address: string;
}

const AddClient = async (client: addClient) =>{
    const query = `${process.env.REACT_APP_API_URL}Client`;
    try {
        const response = await axios.post(
            query,
            {
                'name': client.name,
                'address': client.address,
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

export default AddClient;