import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";

const AddResource = async (resourceName: string) =>{
    const query = `${process.env.REACT_APP_API_URL}Resource`;
    try {
        const response = await axios.post(
            query,
            {
                'name': resourceName
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

export default AddResource;