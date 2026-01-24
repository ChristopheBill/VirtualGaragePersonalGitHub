import axios from "axios";
import type { User } from "../types/user";

const api = axios.create({
  baseURL: "https://localhost:5215/api", // adjust port if needed
});

export const getUsers = async (): Promise<User[]> => {
  const response = await api.get<User[]>("/users");
  return response.data;
};

export const createUser = async (
  data: Omit<User, "id">
): Promise<User> => {
  const response = await api.post<User>("/users", data);
  return response.data;
};
