import { useAuth } from "react-oidc-context";
import { useEffect } from "react";
import { useNavigate, Outlet } from "react-router-dom";
import { LOGIN } from "../../routes";
import Loading from "../../components/common/Loading";

interface ProtectedRouteProps {
  children?: React.ReactNode;
}

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!auth.isLoading && !auth.isAuthenticated) {
      navigate(LOGIN);
    }
  }, [auth.isLoading, auth.isAuthenticated, navigate]);

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

  if (!auth.isAuthenticated) {
    return null;
  }

  return children ? <>{children}</> : <Outlet />;
}
