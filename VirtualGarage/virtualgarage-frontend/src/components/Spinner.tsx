export default function Spinner() {
  return (
    <div className="flex items-center justify-center">
      <style>{`
        @keyframes spin {
          from {
            transform: rotate(0deg);
          }
          to {
            transform: rotate(360deg);
          }
        }

        .spinner {
          display: inline-block;
          width: 24px;
          height: 24px;
          border: 3px solid rgba(59, 130, 246, 0.1);
          border-top-color: #3b82f6;
          border-radius: 50%;
          animation: spin 0.8s linear infinite;
        }
      `}</style>
      <div className="spinner"></div>
    </div>
  );
}
