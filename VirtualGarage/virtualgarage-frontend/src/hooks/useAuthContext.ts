import { useAuth } from "react-oidc-context";

export const useAuthContext = () => {
  const auth = useAuth();

  const isAuthenticated = auth.isAuthenticated || false;
  const user = auth.user;
  const accessToken = user?.access_token;

  return {
    auth,
    isAuthenticated,
    user,
    accessToken,
    isLoading: auth.isLoading,
    error: auth.error,
  };
};
