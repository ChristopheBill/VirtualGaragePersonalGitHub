import RegisterForm from "../components/auth/RegisterForm";
import { useNavigate } from "react-router-dom";

export default function RegisterPage() {
  const navigate = useNavigate();

  return (
    <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
      <div className="w-full max-w-md space-y-6">
        <div>
          <h1 className="text-4xl font-bold text-neutral-900 dark:text-white mb-2">
            Create Account
          </h1>
          <p className="text-neutral-600 dark:text-neutral-400 text-lg">
            Join Virtual Garage today
          </p>
        </div>

        <div className="pt-4">
          <RegisterForm />
        </div>

        <div className="border-t border-neutral-200 dark:border-neutral-700 pt-4">
          <button
            onClick={() => navigate("/login")}
            className="w-full text-center text-sm text-neutral-600 dark:text-neutral-400 hover:text-neutral-900 dark:hover:text-white transition"
          >
            Back to login
          </button>
        </div>
      </div>
    </div>
  );
}
