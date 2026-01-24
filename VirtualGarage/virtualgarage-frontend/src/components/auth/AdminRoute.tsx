import { useAuth } from "react-oidc-context";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

interface AdminRouteProps {
  children: React.ReactNode;
}

function hasAdminRole(profile: any): boolean {
  if (!profile) return false;
  const candidates = [
    profile.role,
    profile.roles,
    profile["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
  ].filter(Boolean);

  for (const value of candidates) {
    if (Array.isArray(value) && value.includes("Admin")) return true;
    if (typeof value === "string" && value === "Admin") return true;
  }
  return false;
}

export default function AdminRoute({ children }: AdminRouteProps) {
  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!auth.isLoading && !auth.isAuthenticated) {
      navigate("/login");
      return;
    }
    if (!auth.isLoading && auth.isAuthenticated && !hasAdminRole(auth.user?.profile)) {
      navigate("/");
    }
  }, [auth.isLoading, auth.isAuthenticated, auth.user, navigate]);

  if (auth.isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 dark:border-blue-500 mx-auto mb-4"></div>
          <p className="text-neutral-600 dark:text-neutral-400">Loading...</p>
        </div>
      </div>
    );
  }

  if (!auth.isAuthenticated || !hasAdminRole(auth.user?.profile)) {
    return null;
  }

  return <>{children}</>;
}
