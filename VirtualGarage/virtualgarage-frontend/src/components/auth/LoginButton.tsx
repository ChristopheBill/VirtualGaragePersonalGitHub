import { useAuth } from "react-oidc-context";

export default function LoginButton() {
  const auth = useAuth();

  const handleLogin = async () => {
    await auth.signinRedirect();
  };

  return (
    <button
      onClick={handleLogin}
      disabled={auth.isLoading}
      className="px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 transition disabled:opacity-50 disabled:cursor-not-allowed font-medium"
    >
      {auth.isLoading ? "Logging in..." : "Login"}
    </button>
  );
}
