import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();

  // Form verileri
  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    password: "",
  });

  // Hata & loading
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      await register(form);
      navigate("/coming", { replace: true }); // başarılı → home
    } catch (err: any) {
      setError(err.response?.data?.message || "Registration failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-white p-8 rounded-xl shadow-md w-96">
        <h2 className="text-2xl font-bold mb-6 text-center">Register</h2>

        {error && <div className="text-red-600 text-sm mb-4">{error}</div>}

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">

          {/* First Name */}
          <div>
            <label className="block text-sm mb-1">First Name</label>
            <input
              name="firstName"
              value={form.firstName}
              onChange={handleChange}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          {/* Last Name */}
          <div>
            <label className="block text-sm mb-1">Last Name</label>
            <input
              name="lastName"
              value={form.lastName}
              onChange={handleChange}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          {/* Username */}
          <div>
            <label className="block text-sm mb-1">Username</label>
            <input
              name="userName"
              value={form.userName}
              onChange={handleChange}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          {/* Email */}
          <div>
            <label className="block text-sm mb-1">Email</label>
            <input
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          {/* Password */}
          <div>
            <label className="block text-sm mb-1">Password</label>
            <input
              type="password"
              name="password"
              value={form.password}
              onChange={handleChange}
              className="border p-2 rounded w-full focus:outline-blue-500"
              required
            />
          </div>

          {/* Submit button */}
          <button
            type="submit"
            disabled={loading}
            className={`bg-green-600 text-white py-2 rounded hover:bg-green-700 transition ${
              loading ? "opacity-70 cursor-not-allowed" : ""
            }`}
          >
            {loading ? "Registering..." : "Register"}
          </button>

        </form>
      </div>
    </div>
  );
}
