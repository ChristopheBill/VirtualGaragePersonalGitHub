import { useState } from "react";
import type { Vehicle } from "../../types/vehicle";
import { updateVehicle } from "../../api/vehicles";
import Loading from "../common/Loading";

type Props = {
  vehicle: Vehicle;
  onClose: () => void;
  onSaved: (vehicle: Vehicle) => void;
};

export default function EditVehicleModal({ vehicle, onClose, onSaved }: Props) {
  const [brand, setBrand] = useState(vehicle.brand);
  const [model, setModel] = useState(vehicle.model);
  const [manufactureDate, setManufactureDate] =
    useState(vehicle.manufactureDate.slice(0, 10));
  const [loading, setLoading] = useState(false);

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);

    try {
      const updated = await updateVehicle(vehicle.id, {
        brand,
        model,
        manufactureDate,
      });

      onSaved(updated);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="fixed inset-0 bg-black/30 backdrop-blur-sm flex items-end sm:items-center justify-center z-50">
      <div className="bg-white dark:bg-neutral-800 rounded-t-2xl sm:rounded-2xl p-4 sm:p-6 w-full sm:w-full sm:max-w-md sm:max-h-[90vh] overflow-y-auto shadow-2xl">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-lg sm:text-xl font-semibold text-neutral-900 dark:text-white">Edit vehicle</h2>
          <button
            onClick={onClose}
            className="text-neutral-600 dark:text-neutral-400 hover:text-neutral-900 dark:hover:text-neutral-300 text-2xl leading-none"
          >
            ×
          </button>
        </div>

        <form onSubmit={submit} className="space-y-4">
          <input
            className="w-full border border-neutral-300 dark:border-neutral-600 rounded-lg p-3 text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
            value={brand}
            onChange={e => setBrand(e.target.value)}
          />

          <input
            className="w-full border border-neutral-300 dark:border-neutral-600 rounded-lg p-3 text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
            value={model}
            onChange={e => setModel(e.target.value)}
          />

          <input
            type="date"
            className="w-full border border-neutral-300 dark:border-neutral-600 rounded-lg p-3 text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            value={manufactureDate}
            onChange={e => setManufactureDate(e.target.value)}
          />

          <div className="flex flex-col-reverse sm:flex-row justify-end gap-2 pt-4 border-t border-neutral-200 dark:border-neutral-700">
            <button
              type="button"
              onClick={onClose}
              disabled={loading}
              className="w-full sm:w-auto px-4 py-3 sm:py-2 rounded-lg bg-neutral-200 hover:bg-neutral-300 dark:bg-neutral-700 dark:hover:bg-neutral-600 transition-colors font-medium text-neutral-900 dark:text-white disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="w-full sm:w-auto px-4 py-3 sm:py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <Loading size="sm" className="mr-2" /> Saving...
                </>
              ) : (
                "Save"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}