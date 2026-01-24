import { useNavigate, useLocation } from "react-router-dom";
import { useDarkMode } from "../../providers/DarkModeProvider";
import AuthStatus from "../auth/AuthStatus";

export default function UserHeader() {
  const navigate = useNavigate();
  const location = useLocation();
  const { darkMode, toggleDarkMode } = useDarkMode();

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
          <div className="flex items-center gap-3">
            <button
              onClick={() => navigate("/vehicles")}
              className={`px-3 py-2 rounded-lg transition hover:cursor-pointer ${
                location.pathname === "/vehicles"
                  ? "bg-blue-600 text-white dark:bg-blue-500"
                  : "bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-700 dark:text-white dark:hover:bg-neutral-600"
              }`}
            >
              Vehicles
            </button>

            <button
              onClick={() => navigate("/users")}
              className={`px-3 py-2 rounded-lg transition hover:cursor-pointer ${
                location.pathname === "/users"
                  ? "bg-blue-600 text-white dark:bg-blue-500"
                  : "bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-700 dark:text-white dark:hover:bg-neutral-600"
              }`}
            >
              Users
            </button>

            <button
              onClick={toggleDarkMode}
              className="px-3 py-2 rounded-lg border border-neutral-300 dark:border-neutral-600 hover:bg-neutral-200 dark:hover:bg-neutral-700 transition hover:cursor-pointer text-neutral-900 dark:text-white"
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
