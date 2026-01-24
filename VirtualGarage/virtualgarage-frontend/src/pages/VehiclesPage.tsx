import { useEffect, useState } from "react";
import { getMyVehicles, deleteVehicle } from "../api/vehicles";
import { getVehicleSpecsPdf } from "../api/vehicleSpecs";
import type { Vehicle } from "../types/vehicle";
import VehicleCard from "../components/vehicles/VehicleCard";
import EditVehicleModal from "../components/vehicles/EditVehicleModal";
import AddVehicleModal from "../components/vehicles/AddVehicleModal";
import DeleteConfirmModal from "../components/vehicles/DeleteConfirmModal";
import PdfViewerModal from "../components/vehicles/PdfViewerModal";
import Spinner from "../components/Spinner";


export default function VehiclesPage() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [loading, setLoading] = useState(true);
  const [isAdding, setIsAdding] = useState(false);
  const [selectedVehicle, setSelectedVehicle] = useState<Vehicle | null>(null);
  const [vehicleToDelete, setVehicleToDelete] = useState<Vehicle | null>(null);
  const [pdfUrl, setPdfUrl] = useState<string | null>(null);
  const [pdfLoading, setPdfLoading] = useState(false);


  function openDelete(vehicle: Vehicle) {
  setVehicleToDelete(vehicle);
}

function closeDelete() {
  setVehicleToDelete(null);
}

async function confirmDelete() {
  if (!vehicleToDelete) return;

  await deleteVehicle(vehicleToDelete.id);

  setVehicles(vs =>
    vs.filter(v => v.id !== vehicleToDelete.id)
  );

  closeDelete();
}

function openEdit(vehicle: Vehicle) {
  setSelectedVehicle(vehicle);
}

function closeEdit() {
  setSelectedVehicle(null);
}

async function openPdfViewer(vehicle: Vehicle) {
  setPdfLoading(true);
  try {
    const year = new Date(vehicle.manufactureDate).getFullYear();
    const blob = await getVehicleSpecsPdf(vehicle.brand, vehicle.model, year);
    const url = URL.createObjectURL(blob);
    setPdfUrl(url);
  } catch (error) {
    console.error("Failed to load PDF:", error);
    alert("Failed to load vehicle specifications PDF");
  } finally {
    setPdfLoading(false);
  }
}

function closePdfViewer() {
  if (pdfUrl) {
    URL.revokeObjectURL(pdfUrl);
  }
  setPdfUrl(null);
}

  useEffect(() => {
    getMyVehicles().then(v => {
      setVehicles(v);
      setLoading(false);
    });
  }, []);

  const baseButton =
    "inline-flex items-center justify-center px-4 py-2 rounded-lg font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2";

  const primaryButton =
    `${baseButton} bg-blue-600 text-white hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600 focus:ring-blue-500`;

  return (
  <div className="w-full max-w-6xl mx-auto">
    <div className="text-center mb-6 sm:mb-8">
      <h1 className="text-2xl sm:text-3xl font-bold mb-4 sm:mb-6 text-neutral-900 dark:text-white">My Vehicles</h1>
      <button
        onClick={() => setIsAdding(true)}
        className={primaryButton}
      >
        + Add Vehicle
      </button>
    </div>

    {loading ? (
      <div className="flex justify-center py-12">
        <Spinner />
      </div>
    ) : vehicles.length === 0 ? (
      <p className="text-neutral-600 dark:text-neutral-400 text-center py-8">No vehicles yet.</p>
    ) : (
      <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 place-items-stretch">
        {vehicles.map(v => (
         <VehicleCard 
          key={v.id} 
          vehicle={v} 
          onEdit={openEdit} 
          onDelete={() => openDelete(v)}
          onViewPdf={openPdfViewer}
          />
        ))}
      </div>
    )}

    {isAdding && (
      <AddVehicleModal
      onClose={() => setIsAdding(false)}
      onCreated={(v) => {
      setVehicles(vs => [...vs, v]);
      setIsAdding(false);
    }}
  />
)}

    {selectedVehicle && (
      <EditVehicleModal
        vehicle={selectedVehicle}
        onClose={closeEdit}
        onSaved={(updated) => {
          setVehicles(vs =>
            vs.map(v => v.id === updated.id ? updated : v)
          );
          closeEdit();
        }}
      />
    )}

    {vehicleToDelete && (
      <DeleteConfirmModal
        vehicle={vehicleToDelete}
        onClose={closeDelete}
        onDeleted={confirmDelete}
      />
    )}

    {pdfUrl && (
      <PdfViewerModal
        pdfUrl={pdfUrl}
        onClose={closePdfViewer}
        title="Vehicle Specifications"
      />
    )}

    {pdfLoading && (
      <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 dark:border-blue-500 mx-auto mb-4"></div>
          <p className="text-white">Loading PDF...</p>
        </div>
      </div>
    )}
  </div>
);
}
