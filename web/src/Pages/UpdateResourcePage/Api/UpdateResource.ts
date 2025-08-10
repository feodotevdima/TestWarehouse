import Resource from "../../../Entities/Resource";
import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";

const UpdateResource = async (resource: Resource) =>{
    const query = `${process.env.REACT_APP_API_URL}Resource`;
    try {
        const response = await axios.put(
            query,
            {
                'id': resource.id,
                'name': resource.name,
                'isActive': resource.isActive
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

export default UpdateResource;