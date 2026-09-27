import { useNavigate } from "react-router-dom";
import { REGISTER } from "../../routes";

export default function RegisterButton() {
  const navigate = useNavigate();

  const handleRegister = () => {
    navigate(REGISTER);
  };

  return (
    <button
      onClick={handleRegister}
      className="px-4 py-2 rounded-lg bg-green-600 text-white hover:bg-green-700 dark:bg-green-500 dark:hover:bg-green-600 transition font-medium"
    >
      Create Account
    </button>
  );
}
