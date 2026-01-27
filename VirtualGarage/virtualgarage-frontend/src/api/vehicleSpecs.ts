import axios from "axios";
import { api } from "./axios";

export interface VehicleSpecs {
  brand: string;
  model: string;
  year: number;
  engine?: string;
  horsepower?: string;
  transmission?: string;
  fuelType?: string;
  [key: string]: any;
}

export async function getVehicleSpecsPdf(brand: string, model: string, year: number): Promise<Blob> {
  try {
    const response = await api.get("/vehiclespecs/pdf", {
      params: { brand, model, year },
      responseType: "blob",
    });
    return response.data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      const data = err.response?.data as any;
      let message = "Failed to load vehicle specifications PDF";
      if (data) {
        if (typeof data === "string") {
          message = data;
        } else if (typeof data.detail === "string") {
          message = data.detail;
        } else if (typeof data.message === "string") {
          message = data.message;
        }
      }
      throw new Error(message);
    }
    throw err as Error;
  }
}

export async function getVehicleSpecs(brand: string, model: string, year: number): Promise<VehicleSpecs> {
  try {
    const response = await api.get<VehicleSpecs>("/vehiclespecs", {
      params: { brand, model, year },
    });
    return response.data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      const data = err.response?.data as any;
      let message = "Vehicle specifications not found";
      if (data?.detail) {
        message = data.detail;
      }
      throw new Error(message);
    }
    throw err as Error;
  }
}
