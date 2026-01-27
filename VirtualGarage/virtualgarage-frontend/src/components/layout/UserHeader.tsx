import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "react-oidc-context";
import { useDarkMode } from "../../providers/DarkModeProvider";
import AuthStatus from "../auth/AuthStatus";

export default function UserHeader() {
  const navigate = useNavigate();
  const location = useLocation();
  const { darkMode, toggleDarkMode } = useDarkMode();
  const auth = useAuth();

  const roleCandidates = [
    auth.user?.profile?.role,
    auth.user?.profile?.roles,
    auth.user?.profile?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
  ].filter(Boolean);
  const isAdmin = roleCandidates.some((value) =>
    Array.isArray(value) ? value.includes("Admin") : value === "Admin"
  );

  return (
    <header className="w-full border-b border-neutral-200 dark:border-neutral-700 bg-white dark:bg-neutral-900">
      <div className="px-4 sm:px-6 py-4">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          
          {/* LOGO */}
          <div
            className="flex items-center gap-3 cursor-pointer"
            onClick={() => navigate("/")}
          >
            <span className="text-2xl font-bold text-blue-600 dark:text-blue-500">
              Virtual Garage
            </span>
          </div>

          {/* NAVIGATION & ACTIONS */}
          <div className="flex flex-wrap items-center gap-2 sm:gap-3">
            <button
              onClick={() => navigate("/vehicles")}
              className={`px-2 sm:px-3 py-2 rounded-lg transition hover:cursor-pointer text-sm sm:text-base whitespace-nowrap ${
                location.pathname === "/vehicles"
                  ? "bg-blue-600 text-white dark:bg-blue-500"
                  : "bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-700 dark:text-white dark:hover:bg-neutral-600"
              }`}
            >
              Vehicles
            </button>

            <button
              onClick={() => navigate("/search")}
              className={`px-2 sm:px-3 py-2 rounded-lg transition hover:cursor-pointer text-sm sm:text-base whitespace-nowrap ${
                location.pathname === "/search"
                  ? "bg-blue-600 text-white dark:bg-blue-500"
                  : "bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-700 dark:text-white dark:hover:bg-neutral-600"
              }`}
            >
              🔍 Search
            </button>

            {isAdmin && (
              <button
                onClick={() => navigate("/admin")}
                className={`px-2 sm:px-3 py-2 rounded-lg transition hover:cursor-pointer text-sm sm:text-base whitespace-nowrap ${
                  location.pathname === "/admin"
                    ? "bg-blue-600 text-white dark:bg-blue-500"
                    : "bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-700 dark:text-white dark:hover:bg-neutral-600"
                }`}
              >
                Admin
              </button>
            )}
            <button
              onClick={() => navigate("/donate")}
              className="px-2 sm:px-4 py-2 rounded-lg font-semibold transition hover:cursor-pointer bg-gradient-to-r from-emerald-500 to-teal-600 text-white hover:from-emerald-600 hover:to-teal-700 shadow-md hover:shadow-lg text-sm sm:text-base whitespace-nowrap"
              title="Support Virtual Garage"
            >
              💚 Donate
            </button>


            <button
              onClick={toggleDarkMode}
              className="px-2 sm:px-3 py-2 rounded-lg border border-neutral-300 dark:border-neutral-600 hover:bg-neutral-200 dark:hover:bg-neutral-700 transition hover:cursor-pointer text-neutral-900 dark:text-white"
              title={darkMode ? "Switch to Light Mode" : "Switch to Dark Mode"}
            >
              {darkMode ? "☀️" : "🌙"}
            </button>

            <div className="hidden sm:block border-l border-neutral-300 dark:border-neutral-600 h-8"></div>

            {/* AUTH STATUS */}
            <AuthStatus />
          </div>
        </div>
      </div>
    </header>
  );
}
