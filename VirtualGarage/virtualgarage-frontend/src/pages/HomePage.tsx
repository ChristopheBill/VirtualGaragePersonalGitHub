import { useNavigate } from "react-router-dom";
import { VEHICLES, USERS } from "../routes";
import { useAuthContext } from "../hooks/useAuthContext";

export default function HomePage() {
  const navigate = useNavigate();
  const { isAdmin } = useAuthContext();
  
  const baseButton =
    "inline-flex items-center justify-center px-6 py-3 rounded-lg font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2";

  const primaryButton =
    `${baseButton} bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 focus:ring-blue-500`;

  const secondaryButton =
    `${baseButton} bg-green-600 text-white hover:bg-green-700 dark:bg-green-500 dark:hover:bg-green-600 focus:ring-green-500`;

  return (
    <div className="max-w-4xl mx-auto">
      <div className="text-center py-12">
        <h1 className="text-4xl font-bold mb-4 text-neutral-900 dark:text-white">Virtual Garage</h1>
        <p className="text-neutral-600 dark:text-neutral-400 mb-8 text-lg">Manage your vehicles</p>
        <div className="flex gap-4 justify-center flex-wrap">
          <button onClick={() => navigate(VEHICLES)} className={primaryButton}>
            My Vehicles
          </button>
          {isAdmin && (
            <button onClick={() => navigate(USERS)} className={secondaryButton}>
              Users
            </button>
          )}
        </div>
      </div>
    </div>
  );
}