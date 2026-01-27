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
  baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:5215/api",
  headers: {
    "Content-Type": "application/json",
  }
});

// Interceptor to add authorization token to all requests
api.interceptors.request.use((config) => {
  // Get the OIDC user from sessionStorage
  const oidcUserKey = Object.keys(sessionStorage).find(key => key.startsWith("oidc.user:"));
  if (oidcUserKey) {
    try {
      const oidcUser = JSON.parse(sessionStorage.getItem(oidcUserKey) || "{}");
      if (oidcUser.access_token) {
        config.headers.Authorization = `Bearer ${oidcUser.access_token}`;
      }
    } catch (e) {
      console.error("Failed to parse OIDC user from sessionStorage", e);
    }
  }
  return config;
});

export default api;