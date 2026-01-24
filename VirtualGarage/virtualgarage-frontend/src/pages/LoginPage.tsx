import LoginButton from "../components/auth/LoginButton";

export default function LoginPage() {
  return (
    <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
      <div className="text-center space-y-6">
        <div>
          <h1 className="text-4xl font-bold text-neutral-900 dark:text-white mb-2">
            Welcome to Virtual Garage
          </h1>
          <p className="text-neutral-600 dark:text-neutral-400 text-lg">
            Please sign in to continue
          </p>
        </div>
        <div className="pt-4">
          <LoginButton />
        </div>
      </div>
    </div>
  );
}
