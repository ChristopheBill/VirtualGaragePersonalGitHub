import api from "./axios";

export interface BlobInfo {
  name: string;
  url: string;
  createdOn?: string;
  sizeInBytes?: number;
}

export async function listBlobs(search?: string): Promise<BlobInfo[]> {
  const response = await api.get<BlobInfo[]>("/blobs", {
    params: search ? { search } : undefined,
  });
  return response.data;
}

export async function downloadBlob(fileName: string): Promise<Blob> {
  const response = await api.get(`/blobs/${encodeURIComponent(fileName)}`, {
    responseType: "blob",
  });
  return response.data;
}
