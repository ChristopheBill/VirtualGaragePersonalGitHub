import { useAuth } from "react-oidc-context";
import LoginButton from "./LoginButton";
import LogoutButton from "./LogoutButton";

export default function AuthStatus() {
  const auth = useAuth();

  if (auth.isLoading) {
    return (
      <div className="text-neutral-600 dark:text-neutral-400">
        Loading...
      </div>
    );
  }

  if (auth.isAuthenticated) {
    return (
      <div className="flex items-center gap-3">
        <span className="text-sm text-neutral-700 dark:text-neutral-300">
          Welcome, <span className="font-semibold">{auth.user?.profile.name}</span>
        </span>
        <LogoutButton />
      </div>
    );
  }

  return <LoginButton />;
}
