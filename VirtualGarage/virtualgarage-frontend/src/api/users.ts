import type { User } from "../types/user";
import { api } from "./axios";

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

export const deleteUser = async (id: string): Promise<void> => {
  await api.delete(`/users/${id}`);
};
