import { useState } from "react";
import { listBlobs, downloadBlob, type BlobInfo } from "../api/blobs";

interface ParsedBlobInfo extends BlobInfo {
  brand?: string;
  model?: string;
  year?: number;
}

function capitalize(str: string): string {
  return str
    .split(/[\s-]+/)
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(" ");
}

function parseBlobName(name: string): ParsedBlobInfo {
  // Try to parse format: "Brand-Model-Year.pdf" or similar
  const match = name.match(/^(.+?)-(.+?)-(\d{4})\.pdf$/i);
  
  if (match) {
    return {
      name,
      url: "",
      brand: capitalize(match[1]),
      model: capitalize(match[2]),
      year: parseInt(match[3]),
    };
  }
  
  return { name, url: "" };
}

export default function VehicleSearchPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [results, setResults] = useState<ParsedBlobInfo[]>([]);
  const [hasSearched, setHasSearched] = useState(false);

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    setHasSearched(true);

    try {
      const blobs = await listBlobs(searchTerm);
      const parsed = blobs.map((blob) => ({
        ...parseBlobName(blob.name),
        url: blob.url,
        createdOn: blob.createdOn,
        sizeInBytes: blob.sizeInBytes,
      }));
      setResults(parsed);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to search blobs");
    } finally {
      setLoading(false);
    }
  };

  const handleViewPdf = async (fileName: string) => {
    try {
      const blob = await downloadBlob(fileName);
      const url = URL.createObjectURL(blob);
      window.open(url, "_blank");
    } catch (error) {
      alert("Failed to load PDF");
    }
  };

  const formatFileSize = (bytes?: number) => {
    if (!bytes) return "N/A";
    const kb = bytes / 1024;
    const mb = kb / 1024;
    return mb >= 1 ? `${mb.toFixed(2)} MB` : `${kb.toFixed(2)} KB`;
  };

  return (
    <div className="max-w-6xl mx-auto space-y-8">
      {/* Header */}
      <div>
        <h1 className="text-3xl font-bold text-neutral-900 dark:text-white">
          Vehicle PDF Search
        </h1>
        <p className="text-neutral-600 dark:text-neutral-400 mt-2">
          Search through stored vehicle specification PDFs
        </p>
      </div>

      {/* Search Form */}
      <div className="bg-white dark:bg-neutral-800 rounded-xl shadow-md p-6">
        <form onSubmit={handleSearch} className="space-y-4">
          <div className="flex gap-4">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search by brand, model, or year (e.g., Volvo, V60, 2019)"
              className="flex-1 px-4 py-3 rounded-lg border border-neutral-200 dark:border-neutral-600 bg-white dark:bg-neutral-700 text-neutral-900 dark:text-white placeholder-neutral-400 dark:placeholder-neutral-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-3 bg-blue-600 hover:bg-blue-700 disabled:bg-neutral-400 text-white font-bold rounded-lg transition-all shadow-md hover:shadow-lg disabled:cursor-not-allowed whitespace-nowrap"
            >
              {loading ? (
                <span className="flex items-center">
                  <span className="animate-spin rounded-full h-5 w-5 border-b-2 border-white mr-2"></span>
                  Searching...
                </span>
              ) : (
                "🔍 Search"
              )}
            </button>
          </div>
          <p className="text-sm text-neutral-500 dark:text-neutral-400">
            Leave empty to view all PDFs
          </p>
        </form>
      </div>

      {/* Error Message */}
      {error && (
        <div className="bg-red-50 dark:bg-red-900/30 border border-red-200 dark:border-red-700 text-red-800 dark:text-red-200 p-4 rounded-lg">
          {error}
        </div>
      )}

      {/* Results */}
      {hasSearched && !loading && (
        <div className="bg-white dark:bg-neutral-800 rounded-xl shadow-md overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <h2 className="text-xl font-bold text-neutral-900 dark:text-white">
              {results.length === 0
                ? "No PDFs found"
                : `${results.length} PDF${results.length !== 1 ? "s" : ""} found`}
            </h2>
          </div>

          {results.length > 0 && (
            <div className="divide-y divide-neutral-200 dark:divide-neutral-700">
              {results.map((blob, index) => (
                <div
                  key={index}
                  className="p-6 hover:bg-neutral-50 dark:hover:bg-neutral-700/50 transition-colors"
                >
                  <div className="flex items-start justify-between gap-4">
                    <div className="flex-1 min-w-0">
                      {blob.brand && blob.model && blob.year ? (
                        <>
                          <h3 className="text-lg font-bold text-neutral-900 dark:text-white mb-2">
                            {blob.brand} {blob.model} ({blob.year})
                          </h3>
                          <div className="flex flex-wrap gap-2 mb-2">
                            <span className="px-3 py-1 bg-blue-100 dark:bg-blue-900/40 text-blue-800 dark:text-blue-200 rounded-full text-sm font-semibold">
                              {blob.brand}
                            </span>
                            <span className="px-3 py-1 bg-green-100 dark:bg-green-900/40 text-green-800 dark:text-green-200 rounded-full text-sm font-semibold">
                              {blob.model}
                            </span>
                            <span className="px-3 py-1 bg-purple-100 dark:bg-purple-900/40 text-purple-800 dark:text-purple-200 rounded-full text-sm font-semibold">
                              {blob.year}
                            </span>
                          </div>
                        </>
                      ) : (
                        <h3 className="text-lg font-semibold text-neutral-900 dark:text-white mb-2">
                          {blob.name}
                        </h3>
                      )}
                      <div className="flex flex-wrap gap-4 text-sm text-neutral-600 dark:text-neutral-400">
                        <span>📄 {blob.name}</span>
                        <span>💾 {formatFileSize(blob.sizeInBytes)}</span>
                        {blob.createdOn && (
                          <span>
                            📅 {new Date(blob.createdOn).toLocaleDateString()}
                          </span>
                        )}
                      </div>
                    </div>
                    <button
                      onClick={() => handleViewPdf(blob.name)}
                      className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-lg transition-colors whitespace-nowrap"
                    >
                      View PDF
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}

          {results.length === 0 && (
            <div className="p-12 text-center">
              <div className="text-6xl mb-4">📭</div>
              <h3 className="text-xl font-semibold text-neutral-700 dark:text-neutral-300 mb-2">
                No Results
              </h3>
              <p className="text-neutral-500 dark:text-neutral-400">
                Try a different search term or leave it empty to see all PDFs
              </p>
            </div>
          )}
        </div>
      )}

      {/* Empty State */}
      {!hasSearched && !loading && (
        <div className="text-center py-12">
          <div className="text-6xl mb-4">🔍</div>
          <h3 className="text-xl font-semibold text-neutral-700 dark:text-neutral-300 mb-2">
            Start Your Search
          </h3>
          <p className="text-neutral-500 dark:text-neutral-400">
            Enter search terms or leave empty to browse all vehicle PDFs
          </p>
        </div>
      )}
    </div>
  );
}
