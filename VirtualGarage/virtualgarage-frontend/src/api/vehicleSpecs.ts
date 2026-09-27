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
}

type ErrorResponseData = {
  detail?: string;
  message?: string;
} | string | null | undefined;

function getErrorMessage(data: ErrorResponseData, fallback: string): string {
  if (!data) {
    return fallback;
  }

  if (typeof data === "string") {
    return data;
  }

  if (typeof data.detail === "string") {
    return data.detail;
  }

  if (typeof data.message === "string") {
    return data.message;
  }

  return fallback;
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
      throw new Error(
        getErrorMessage(err.response?.data as ErrorResponseData, "Failed to load vehicle specifications PDF")
      );
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
      throw new Error(
        getErrorMessage(err.response?.data as ErrorResponseData, "Vehicle specifications not found")
      );
    }
    throw err as Error;
  }
}
