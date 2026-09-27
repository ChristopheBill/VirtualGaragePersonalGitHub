import { useAuth } from "react-oidc-context";
import { useEffect } from "react";
import { useNavigate, Outlet } from "react-router-dom";
import { LOGIN, HOME } from "../../routes";
import Loading from "../../components/common/Loading";

interface AdminRouteProps {
  children?: React.ReactNode;
}

type ProfileLike = Record<string, unknown> | null | undefined;

function getProfileCandidate(profile: ProfileLike, key: string): unknown {
  return profile ? profile[key] : undefined;
}

function hasAdminRole(profile: ProfileLike): boolean {
  if (!profile) return false;
  const candidates = [
    getProfileCandidate(profile, "role"),
    getProfileCandidate(profile, "roles"),
    getProfileCandidate(profile, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
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
      navigate(LOGIN);
      return;
    }
    if (!auth.isLoading && auth.isAuthenticated && !hasAdminRole(auth.user?.profile)) {
      navigate(HOME);
    }
  }, [auth.isLoading, auth.isAuthenticated, auth.user, navigate]);

  if (auth.isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
        <div className="text-center">
          <Loading />
          <p className="text-neutral-600 dark:text-neutral-400">Loading...</p>
        </div>
      </div>
    );
  }

  if (!auth.isAuthenticated || !hasAdminRole(auth.user?.profile)) {
    return null;
  }

  return children ? <>{children}</> : <Outlet />;
}
