import { useAuth } from "react-oidc-context";

export default function LogoutButton() {
  const auth = useAuth();

  const handleLogout = async () => {
    await auth.removeUser();
  };

  return (
    <button
      onClick={handleLogout}
      disabled={auth.isLoading}
      className="px-4 py-2 rounded-lg bg-red-600 text-white hover:bg-red-700 dark:bg-red-500 dark:hover:bg-red-600 transition disabled:opacity-50 disabled:cursor-not-allowed font-medium"
    >
      {auth.isLoading ? "Logging out..." : "Logout"}
    </button>
  );
}
