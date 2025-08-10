import axios, { AxiosError } from "axios";
import ApiErrorResponse from "../../../Entities/ApiErrorResponse";
import { AddIncome } from "../../../Entities/AddIncome";
import { AddIncomeResource } from "../../../Entities/AddIncome";
import { format } from 'date-fns';

const PostIncome = async (income: AddIncome) =>{
    const query = `${process.env.REACT_APP_API_URL}Income`;
    try {
        const response = await axios.put(
            query,
            {
                id: income.id,
                number: income.number,
                date: income.date ? format(income.date, 'yyyy-MM-dd') : '', 
                resources: income.resources.map((item: AddIncomeResource) => ({
                    incomeId: income.id,
                    resourceId: item.resourceId,
                    unitId: item.unitId,
                    quantity: item.quantity
            }))
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

export default PostIncome;