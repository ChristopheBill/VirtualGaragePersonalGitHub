import type { Vehicle } from "../../types/vehicle";

type Props = {
  vehicle: Vehicle;
  onEdit: (vehicle: Vehicle) => void;
  onDelete: (id: string) => void;
};

export default function VehicleCard({ vehicle, onEdit, onDelete }: Props) {
  return (
    <div className="rounded-xl border border-neutral-200 dark:border-neutral-700 bg-white dark:bg-neutral-800 p-6 hover:shadow-lg transition">
      <h3 className="text-lg font-semibold text-neutral-900 dark:text-white">
        {vehicle.brand} {vehicle.model}
      </h3>

      <p className="text-sm text-neutral-600 dark:text-neutral-400 mt-1">
        Built in {new Date(vehicle.manufactureDate).getFullYear()}
      </p>

      <div className="mt-4 flex flex-col sm:flex-row gap-2">
        <button
          onClick={() => onEdit(vehicle)}
          className="px-4 py-2 text-sm rounded-lg font-medium bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 transition-colors"
        >
          Edit
        </button>

        <button
          onClick={() => onDelete(vehicle.id)}
          className="px-4 py-2 text-sm rounded-lg font-medium bg-neutral-200 text-red-600 hover:bg-red-50 dark:bg-neutral-700 dark:text-red-400 dark:hover:bg-red-900/20 transition-colors"
        >
          Delete
        </button>
      </div>
    </div>
  );
}