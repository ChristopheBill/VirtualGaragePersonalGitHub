import { useAuth } from "react-oidc-context";
import { useEffect } from "react";

export default function CallbackPage() {
  const auth = useAuth();

  useEffect(() => {
    if (auth.isAuthenticated) {
      window.location.href = "/";
    }
  }, [auth.isAuthenticated]);

  if (auth.isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 dark:border-blue-500 mx-auto mb-4"></div>
          <p className="text-neutral-600 dark:text-neutral-400">Logging in...</p>
        </div>
      </div>
    );
  }

  if (auth.error) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
        <div className="text-center">
          <p className="text-red-600 dark:text-red-400 mb-4">Authentication failed</p>
          <p className="text-neutral-600 dark:text-neutral-400">{auth.error.message}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
      <p className="text-neutral-600 dark:text-neutral-400">Redirecting...</p>
    </div>
  );
}
