import { api } from "./axios";

export async function getVehicleSpecsPdf(brand: string, model: string, year: number): Promise<Blob> {
  const response = await api.get("/vehiclespecs/pdf", {
    params: { brand, model, year },
    responseType: "blob",
  });
  return response.data;
}
