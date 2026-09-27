import { useState } from "react";
import { createVehicle } from "../../api/vehicles";
import type { Vehicle } from "../../types/vehicle";
import Loading from "../common/Loading";

type Props = {
  onClose: () => void;
  onCreated: (vehicle: Vehicle) => void;
};

export default function AddVehicleModal({ onClose, onCreated }: Props) {
  const [brand, setBrand] = useState("");
  const [model, setModel] = useState("");
  const [manufactureDate, setManufactureDate] = useState("");
  const [loading, setLoading] = useState(false);

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);

    try {
      const created = await createVehicle({
        brand,
        model,
        manufactureDate,
      });

      onCreated(created);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="fixed inset-0 bg-black/30 backdrop-blur-sm flex items-end sm:items-center justify-center z-50 animate-fade-in">
      <div className="bg-white dark:bg-neutral-800 p-4 sm:p-6 rounded-t-2xl sm:rounded-2xl w-full sm:w-full sm:max-w-md sm:max-h-[90vh] overflow-y-auto shadow-2xl sm:animate-slide-up animate-slide-up-mobile">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-lg sm:text-2xl font-bold text-neutral-900 dark:text-white">Add Vehicle</h2>
          <button
            onClick={onClose}
            className="text-neutral-600 dark:text-neutral-400 hover:text-neutral-900 dark:hover:text-neutral-300 text-2xl leading-none"
          >
            ×
          </button>
        </div>

        <form onSubmit={submit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-200 mb-2">Brand</label>
            <input
              className="w-full border border-neutral-300 dark:border-neutral-600 p-3 rounded-lg text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="e.g., Toyota"
              value={brand}
              onChange={e => setBrand(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-200 mb-2">Model</label>
            <input
              className="w-full border border-neutral-300 dark:border-neutral-600 p-3 rounded-lg text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="e.g., Camry"
              value={model}
              onChange={e => setModel(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-200 mb-2">Manufacture Date</label>
            <input
              type="date"
              className="w-full border border-neutral-300 dark:border-neutral-600 p-3 rounded-lg text-base bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              value={manufactureDate}
              onChange={e => setManufactureDate(e.target.value)}
              required
            />
          </div>

          <div className="flex flex-col-reverse sm:flex-row justify-end gap-3 pt-6 border-t border-neutral-200 dark:border-neutral-700">
            <button
              type="button"
              onClick={onClose}
              className="w-full sm:w-auto px-4 py-3 sm:py-2 border border-neutral-300 dark:border-neutral-600 rounded-lg hover:bg-neutral-100 dark:hover:bg-neutral-700 transition-colors font-medium text-neutral-700 dark:text-neutral-300"
            >
              Cancel
            </button>

            <button
              type="submit"
              disabled={loading}
              className="w-full sm:w-auto px-4 py-3 sm:py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 transition-colors font-medium shadow-md hover:shadow-lg disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <Loading size="sm" className="mr-2" /> Adding...
                </>
              ) : (
                "Add Vehicle"
              )}
            </button>
          </div>
        </form>
      </div>

      <style>{`
        @keyframes fadeIn {
          from {
            opacity: 0;
          }
          to {
            opacity: 1;
          }
        }

        @keyframes slideUp {
          from {
            transform: translateY(100px);
            opacity: 0;
          }
          to {
            transform: translateY(0);
            opacity: 1;
          }
        }

        @keyframes slideUpMobile {
          from {
            transform: translateY(100%);
          }
          to {
            transform: translateY(0);
          }
        }

        .animate-fade-in {
          animation: fadeIn 0.3s ease-out;
        }

        .animate-slide-up {
          animation: slideUp 0.4s cubic-bezier(0.34, 1.56, 0.64, 1);
        }

        .animate-slide-up-mobile {
          animation: slideUpMobile 0.5s cubic-bezier(0.34, 1.56, 0.64, 1);
        }
      `}</style>
    </div>
  );
}
