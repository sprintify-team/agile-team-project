import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

type ProtectedRouteProps = {
  roles?: string[]; // opsiyonel roller
};

export default function ProtectedRoute({ roles }: ProtectedRouteProps) {
  const { isAuthenticated, user, loading } = useAuth();
  const location = useLocation();

  if (loading) return <div>Loading...</div>; // ⏳ Auth durumu çözülüyor

  // 1️⃣ Kullanıcı giriş yapmamışsa → login sayfasına yönlendir
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // 2️⃣ Eğer belirli roller belirtilmişse ve kullanıcı bu rollerden hiçbirine sahip değilse
  if (roles && !roles.some((r) => user?.roles.includes(r))) {
    return <Navigate to="/unauthorized" state={{ from: location }} replace />;
  }

  // 3️⃣ Her şey yolundaysa, alt route’u (child route) render et
  return <Outlet />;
}
