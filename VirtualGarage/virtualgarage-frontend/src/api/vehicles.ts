import { api } from "./axios";
import type { Vehicle } from "../types/vehicle";

export const getMyVehicles = async (): Promise<Vehicle[]> => {
  const response = await api.get("/vehicles/mine");
  return response.data;
};

export async function updateVehicle(id: string, data: {
  brand: string;
  model: string;
  manufactureDate: string;
}) {
  const res = await api.put(`/vehicles/${id}`, data);
  return res.data;
}
export async function createVehicle(data: {
  brand: string;
  model: string;
  manufactureDate: string;
}) {
  const res = await api.post("/vehicles", data);
  return res.data;
}
export async function deleteVehicle(id: string) {
  await api.delete(`/vehicles/${id}`);
}