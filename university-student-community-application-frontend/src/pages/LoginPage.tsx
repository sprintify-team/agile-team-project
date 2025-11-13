import { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Eğer kullanıcı başka bir sayfadan yönlendirilmişse (ör: ProtectedRoute)
  const from = (location.state as { from?: Location })?.from?.pathname || "/";

  // local state'ler
  const [userNameOrEmail, setUserNameOrEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      await login({ userNameOrEmail, password });
      navigate(from, { replace: true }); // başarılıysa yönlendir
    } catch (err: any) {
      setError(err.response?.data?.message || "Login failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-white p-8 rounded-xl shadow-md w-96">
        <h2 className="text-2xl font-bold mb-6 text-center">Login</h2>

        {error && <div className="text-red-600 text-sm mb-4">{error}</div>}

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <div>
            <label className="block text-sm mb-1">Email</label>
            <input
              type="text"
              value={userNameOrEmail}
              onChange={(e) => setUserNameOrEmail(e.target.value)}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          <div>
            <label className="block text-sm mb-1">Password</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className={`bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition ${
              loading ? "opacity-70 cursor-not-allowed" : ""
            }`}
          >
            {loading ? "Logging in..." : "Login"}
          </button>
        </form>
      </div>
    </div>
  );
}

