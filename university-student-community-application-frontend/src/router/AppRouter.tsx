import { Routes, Route } from "react-router-dom";
import ComingSoonPage from "../pages/ComingSoonPage";
import HomePage from "../pages/HomePage";
import LoginPage from "../pages/LoginPage";
import RegisterPage from "../pages/RegisterPage";

const AppRouter = () => {
  return (
    <Routes>
      {/* Herkese açık sayfalar */}
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/coming" element={<ComingSoonPage />} />
      
    </Routes>
  );
};

export default AppRouter;

