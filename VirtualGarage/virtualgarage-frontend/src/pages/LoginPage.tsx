import LoginButton from "../components/auth/LoginButton";
import RegisterButton from "../components/auth/RegisterButton";

export default function LoginPage() {
  return (
    <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
      <div className="text-center space-y-6">
        <div>
          <h1 className="text-4xl font-bold text-neutral-900 dark:text-white mb-2">
            Welcome to Virtual Garage
          </h1>
          <p className="text-neutral-600 dark:text-neutral-400 text-lg">
            Manage your vehicles with ease
          </p>
        </div>
        <div className="pt-4 space-y-3">
          <LoginButton />
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
        </div>
      </div>
    </div>
  );
}
