import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";
import { AddShipment } from "../../../Entities/AddShipment";
import { AddShipmentResource } from "../../../Entities/AddShipment";
import { format } from 'date-fns';
import { Shipment } from "../../../Entities/Shipment";

const PostShipment = async (shipment: AddShipment) =>{
    const query = `${process.env.REACT_APP_API_URL}Shipment`;
    try {
        const response = await axios.post(
            query,
            {
                number: shipment.number,
                date: shipment.date ? format(shipment.date, 'yyyy-MM-dd') : '', 
                clientId: shipment.clientId,
                resources: shipment.resources.map((item: AddShipmentResource) => ({
                    resourceId: item.resourceId,
                    unitId: item.unitId,
                    quantity: item.quantity
            }))
            }
        );
        const json: Shipment = await response.data;
        return json;
    } catch (error) {
        const axiosError = error as AxiosError<ApiErrorResponse>;
        if (axiosError.response && axiosError.response.status != 200) {
            throw new Error(axiosError.response.data.detail);
        }
    }
}

export default PostShipment;