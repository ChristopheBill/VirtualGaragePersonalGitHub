import { useAuth } from "react-oidc-context";

export const useAuthContext = () => {
  const auth = useAuth();

  const isAuthenticated = auth.isAuthenticated || false;
  const user = auth.user;
  const accessToken = user?.access_token;
  
  // Extract roles from the JWT token's 'role' claim
  // The claim can be either a string (single role) or an array (multiple roles)
  const roles: string[] = (() => {
    if (!user?.profile?.role) {
      return [];
    }
    const roleClaim = user.profile.role;
    return Array.isArray(roleClaim) ? roleClaim : [roleClaim];
  })();

  const isAdmin = roles.includes("Admin");

  return {
    auth,
    isAuthenticated,
    user,
    accessToken,
    roles,
    isAdmin,
    isLoading: auth.isLoading,
    error: auth.error,
  };
};
