import { useAuth } from "../providers/AuthProvider";

export const useAuthContext = () => {
  const auth = useAuth();
  const roles = auth.user ? [auth.user.role] : [];
  const isAdmin = auth.user?.role === "admin";

  return {
    auth,
    isAuthenticated: auth.isAuthenticated,
    user: auth.user,
    accessToken: auth.accessToken,
    roles,
    isAdmin,
    isLoading: auth.isLoading,
    error: auth.error,
  };
};
