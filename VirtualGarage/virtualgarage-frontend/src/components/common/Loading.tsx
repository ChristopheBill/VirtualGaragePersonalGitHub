type LoadingProps = {
  size?: "sm" | "md" | "lg";
  className?: string;
};

export default function Loading({ size = "lg", className = "" }: LoadingProps) {
  const sizeClass =
    size === "sm" ? "h-5 w-5 border-b-2" : size === "md" ? "h-8 w-8 border-b-2" : "h-12 w-12 border-b-2";

  return (
    <div className={`animate-spin rounded-full ${sizeClass} border-blue-600 dark:border-blue-500 mx-auto ${className}`} />
  );
}
