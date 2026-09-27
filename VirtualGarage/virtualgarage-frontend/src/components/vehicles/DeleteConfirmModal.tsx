import { useState } from "react";
import type { Vehicle } from "../../types/vehicle";
import { deleteVehicle } from "../../api/vehicles";
import Loading from "../common/Loading";

type Props = {
  vehicle: Vehicle;
  onClose: () => void;
  onDeleted: (vehicleId: string) => void;
};

export default function DeleteConfirmModal({
  vehicle,
  onClose,
  onDeleted,
}: Props) {
  const [loading, setLoading] = useState(false);

  async function handleDelete() {
    setLoading(true);
    try {
      await deleteVehicle(vehicle.id);
      onDeleted(vehicle.id);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="fixed inset-0 bg-black/30 backdrop-blur-sm flex items-end sm:items-center justify-center z-50">
      <div className="bg-white dark:bg-neutral-800 rounded-t-2xl sm:rounded-2xl p-4 sm:p-6 w-full sm:w-full sm:max-w-md shadow-2xl">
        <h2 className="text-lg sm:text-xl font-semibold mb-4 text-neutral-900 dark:text-white">Delete vehicle?</h2>

        <p className="text-neutral-700 dark:text-neutral-300 mb-6 text-sm sm:text-base">
          Are you sure you want to delete <strong>{vehicle.brand} {vehicle.model}</strong>?
          This action cannot be undone.
        </p>

        <div className="flex flex-col-reverse sm:flex-row gap-3 justify-end border-t border-neutral-200 dark:border-neutral-700 pt-4">
          <button
            onClick={onClose}
            disabled={loading}
            className="w-full sm:w-auto px-4 py-3 sm:py-2 rounded-lg border border-neutral-300 dark:border-neutral-600 hover:bg-neutral-100 dark:hover:bg-neutral-700 font-medium transition-colors text-neutral-900 dark:text-white disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Cancel
          </button>
          <button
            onClick={handleDelete}
            disabled={loading}
            className="w-full sm:w-auto px-4 py-3 sm:py-2 rounded-lg bg-red-600 text-white hover:bg-red-700 dark:bg-red-500 dark:hover:bg-red-600 font-medium transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
          >
            {loading ? (
              <>
                <Loading size="sm" className="mr-2" /> Deleting...
              </>
            ) : (
              "Delete"
            )}
          </button>
        </div>
      </div>
    </div>
  );
}
