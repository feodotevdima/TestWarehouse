import axios from "axios";

const GetIncomeNumbers = async () =>{
    const query = `${process.env.REACT_APP_API_URL}Income/numbers`;
    const response = await axios.get(
        query,
    );
    const json: string[] = await response.data;
    return json;
}

export default GetIncomeNumbers;