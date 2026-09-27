import { useNavigate } from "react-router-dom";
import { LOGIN } from "../../routes";

export default function LoginButton() {
  const navigate = useNavigate();

  return (
    <button
      onClick={() => navigate(LOGIN)}
      className="px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 transition disabled:opacity-50 disabled:cursor-not-allowed font-medium"
    >
      Login
    </button>
  );
}
