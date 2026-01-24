import { type AuthProviderProps } from "react-oidc-context";

export const oidcConfig: AuthProviderProps = {
  authority: import.meta.env.VITE_OIDC_AUTHORITY || "https://localhost:5001",
  client_id: import.meta.env.VITE_OIDC_CLIENT_ID || "react-app-client",
  client_secret: import.meta.env.VITE_OIDC_CLIENT_SECRET || "reactapp-secret",
  redirect_uri: import.meta.env.VITE_OIDC_REDIRECT_URI || "http://localhost:5173/callback",
  scope: import.meta.env.VITE_OIDC_SCOPE || "openid profile virtualgarage.api.read virtualgarage.api.write",
  response_type: "code",
  post_logout_redirect_uri: import.meta.env.VITE_OIDC_POST_LOGOUT_REDIRECT_URI || "http://localhost:5173/login",
};
