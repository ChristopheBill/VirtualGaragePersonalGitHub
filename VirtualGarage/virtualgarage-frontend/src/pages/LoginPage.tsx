import RegisterButton from "../components/auth/RegisterButton";
import { useState } from "react";
import { useAuth } from "../providers/AuthProvider";
import { useNavigate } from "react-router-dom";
import { HOME } from "../routes";
import ThemeToggle from "../components/common/ThemeToggle";

export default function LoginPage() {
  const auth = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError("");
    try {
      await auth.login(email, password);
      navigate(HOME);
    } catch (loginError) {
      setError(loginError instanceof Error ? loginError.message : "Login failed");
    }
  };

  return (
    <div className="relative flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
      <div className="absolute right-4 top-4">
        <ThemeToggle />
      </div>
      <div className="text-center space-y-6">
        <div>
          <h1 className="text-4xl font-bold text-neutral-900 dark:text-white mb-2">
            Welcome to Virtual Garage
          </h1>
          <p className="text-neutral-600 dark:text-neutral-400 text-lg">
            Manage your vehicles with ease
          </p>
        </div>
        <form onSubmit={handleSubmit} className="pt-4 space-y-3">
          {error && <p className="text-sm text-red-600">{error}</p>}
          <input className="w-full rounded-lg border p-3" type="email" placeholder="Email" value={email} onChange={(event) => setEmail(event.target.value)} required />
          <div className="relative">
            <input className="w-full rounded-lg border p-3 pr-20" type={showPassword ? "text" : "password"} placeholder="Password" value={password} onChange={(event) => setPassword(event.target.value)} required />
            <button type="button" onClick={() => setShowPassword((visible) => !visible)} className="absolute right-2 top-1/2 -translate-y-1/2 px-2 py-1 text-sm text-neutral-600 hover:text-neutral-900" aria-label={showPassword ? "Hide password" : "Show password"}>
              {showPassword ? "Hide" : "Show"}
            </button>
          </div>
          <button className="w-full px-4 py-2 rounded-lg bg-blue-600 text-white" type="submit" disabled={auth.isLoading}>Login</button>
          <div className="relative">
            <div className="absolute inset-0 flex items-center">
              <div className="w-full border-t border-neutral-300 dark:border-neutral-600"></div>
            </div>
            <div className="relative flex justify-center text-sm">
              <span className="px-2 bg-white dark:bg-neutral-900 text-neutral-600 dark:text-neutral-400">
                or
              </span>
            </div>
          </div>
          <RegisterButton />
        </form>
      </div>
    </div>
  );
}
