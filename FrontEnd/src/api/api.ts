import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5121/api",
});

export default api;
