import { useEffect, useState } from "react";

interface PdfViewerModalProps {
  pdfUrl: string;
  onClose: () => void;
  title?: string;
}

export default function PdfViewerModal({ pdfUrl, onClose, title }: PdfViewerModalProps) {
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => setLoading(false), 500);
    return () => clearTimeout(timer);
  }, [pdfUrl]);

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
      <div className="bg-white dark:bg-neutral-800 rounded-xl shadow-2xl w-full max-w-5xl h-[90vh] flex flex-col">
        {/* Header */}
        <div className="flex items-center justify-between p-4 border-b border-neutral-200 dark:border-neutral-700">
          <h2 className="text-xl font-bold text-neutral-900 dark:text-white">
            {title || "Vehicle Specifications"}
          </h2>
          <button
            onClick={onClose}
            className="px-4 py-2 text-sm font-medium rounded-lg bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-700 dark:text-white dark:hover:bg-neutral-600 transition-colors"
          >
            Close
          </button>
        </div>

        {/* PDF Viewer */}
        <div className="flex-1 relative overflow-hidden">
          {loading && (
            <div className="absolute inset-0 flex items-center justify-center bg-neutral-50 dark:bg-neutral-900">
              <div className="text-center">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 dark:border-blue-500 mx-auto mb-4"></div>
                <p className="text-neutral-600 dark:text-neutral-400">Loading PDF...</p>
              </div>
            </div>
          )}
          <iframe
            src={pdfUrl}
            className="w-full h-full border-0"
            title="Vehicle Specifications PDF"
            onLoad={() => setLoading(false)}
          />
        </div>
      </div>
    </div>
  );
}
