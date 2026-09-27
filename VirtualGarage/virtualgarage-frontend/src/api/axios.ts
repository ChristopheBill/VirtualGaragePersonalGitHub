import axios from "axios";

declare global {
  interface Window {
    __auth_context__?: {
      user?: {
        access_token?: string;
      };
    };
  }
}

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:3000/api",
  headers: {
    "Content-Type": "application/json",
  }
});

// Interceptor to add authorization token to all requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("virtualgarage.token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;