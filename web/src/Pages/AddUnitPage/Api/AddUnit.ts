import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";

const AddUnit = async (unitName: string) =>{
    const query = `${process.env.REACT_APP_API_URL}Unit`;
    try {
        const response = await axios.post(
            query,
            {
                'name': unitName
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

export default AddUnit;