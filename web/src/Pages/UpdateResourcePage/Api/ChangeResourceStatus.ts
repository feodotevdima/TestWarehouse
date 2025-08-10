import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";
import { UUID } from "crypto";

const ChangeResourceStatus = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Resource/change-status/${id}`;
    try {
        const response = await axios.put(query);
        return response.status;
    } catch (error) {
        const axiosError = error as AxiosError<ApiErrorResponse>;
        if (axiosError.response && axiosError.response.status != 200) {
            throw new Error(axiosError.response.data.detail);
        }
    }
}

export default ChangeResourceStatus;