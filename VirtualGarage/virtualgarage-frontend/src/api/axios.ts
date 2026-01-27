import axios from "axios";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:5215/api",
  headers: {
    "Content-Type": "application/json",
  }
});

// Interceptor to add authorization token to all requests
api.interceptors.request.use((config) => {
  const auth = window.__auth_context__;
  if (auth?.user?.access_token) {
    config.headers.Authorization = `Bearer ${auth.user.access_token}`;
  }
  return config;
});

export default api;