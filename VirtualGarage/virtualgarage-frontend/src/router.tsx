import React from "react";
import { createBrowserRouter } from "react-router-dom";
import Layout from "./components/layout/Layout";
import ProtectedRoute from "./components/auth/ProtectedRoute";
import AdminRoute from "./components/auth/AdminRoute";
import HomePage from "./pages/HomePage";
import AdminDashboardPage from "./pages/AdminDashboardPage";
import VehiclesPage from "./pages/VehiclesPage";
import VehicleSearchPage from "./pages/VehicleSearchPage";
import DonationPage from "./pages/DonationPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CallbackPage from "./pages/CallbackPage";
import { LOGIN, REGISTER, CALLBACK, ADMIN, VEHICLES, SEARCH, DONATE } from "./routes";

const NotFound = () => (
  <div className="text-center py-12 text-neutral-400 dark:text-neutral-500">Page not found</div>
);

export const router = createBrowserRouter([
  {
    path: LOGIN,
    element: <LoginPage />,
  },
  {
    path: REGISTER,
    element: <RegisterPage />,
  },
  {
    path: CALLBACK,
    element: <CallbackPage />,
  },
  // Protected subtree: Layout + pages
  {
    element: (
      <ProtectedRoute>
        <Layout />
      </ProtectedRoute>
    ),
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: ADMIN.replace(/^\//, ""),
        element: (
          <AdminRoute>
            <AdminDashboardPage />
          </AdminRoute>
        ),
      },
      {
        path: VEHICLES.replace(/^\//, ""),
        element: <VehiclesPage />,
      },
      {
        path: SEARCH.replace(/^\//, ""),
        element: <VehicleSearchPage />,
      },
      {
        path: DONATE.replace(/^\//, ""),
        element: <DonationPage />,
      },
      {
        path: "*",
        element: <NotFound />,
      },
    ],
  },
]);

export default router;
