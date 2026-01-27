import { useEffect } from "react";
import { Routes, Route } from "react-router-dom";
import { useAuth } from "react-oidc-context";
import HomePage from "./pages/HomePage";
import AdminDashboardPage from "./pages/AdminDashboardPage";
import VehiclesPage from "./pages/VehiclesPage";
import VehicleSearchPage from "./pages/VehicleSearchPage";
import DonationPage from "./pages/DonationPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CallbackPage from "./pages/CallbackPage";
import Layout from "./components/layout/Layout";
import ProtectedRoute from "./components/auth/ProtectedRoute";
import AdminRoute from "./components/auth/AdminRoute";
import { DarkModeProvider } from "./providers/DarkModeProvider";

export default function App() {
  const auth = useAuth();

  useEffect(() => {
    (window as any).__auth_context__ = auth;
  }, [auth]);

  // Show loading screen while auth is initializing
  if (auth.isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-white dark:bg-neutral-900">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 dark:border-blue-500 mx-auto mb-4"></div>
          <p className="text-neutral-600 dark:text-neutral-400">Initializing...</p>
        </div>
      </div>
    );
  }

  return (
    <DarkModeProvider>
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/callback" element={<CallbackPage />} />

        {/* Protected Routes */}
        <Route
          path="/*"
          element={
            <ProtectedRoute>
              <Layout>
                <Routes>
                  <Route path="/" element={<HomePage />} />
                  <Route
                    path="/admin"
                    element={
                      <AdminRoute>
                        <AdminDashboardPage />
                      </AdminRoute>
                    }
                  />
                  <Route path="/vehicles" element={<VehiclesPage />} />
                  <Route path="/search" element={<VehicleSearchPage />} />
                  <Route path="/donate" element={<DonationPage />} />
                  <Route path="*" element={
                    <div className="text-center py-12 text-neutral-400 dark:text-neutral-500">
                      Page not found
                    </div>
                  } />
                </Routes>
              </Layout>
            </ProtectedRoute>
          }
        />
      </Routes>
    </DarkModeProvider>
  );
}
