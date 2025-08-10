import Unit from "../../../Entities/Unit";
import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";

const UpdateUnit = async (unit: Unit) =>{
    const query = `${process.env.REACT_APP_API_URL}Unit`;
    try {
        const response = await axios.put(
            query,
            {
                'id': unit.id,
                'name': unit.name,
                'isActive': unit.isActive
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

export default UpdateUnit;