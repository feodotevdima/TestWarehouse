import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";
import { UUID } from "crypto";

const DeleteIncome = async (id: UUID) =>{
    const query = `${process.env.REACT_APP_API_URL}Income/${id}`;
    try {
        const response = await axios.delete(query);
        return response.status;
    } catch (error) {
        const axiosError = error as AxiosError<ApiErrorResponse>;
        if (axiosError.response && axiosError.response.status != 200) {
            throw new Error(axiosError.response.data.detail);
        }
    }
}

export default DeleteIncome;