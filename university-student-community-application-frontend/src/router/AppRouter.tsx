import { Routes, Route } from "react-router-dom";
import ComingSoonPage from "../pages/ComingSoonPage";

const AppRouter = () => {
  return (
    <Routes>
        <Route path="/" element={<ComingSoonPage />} />
    </Routes>
  );
};

export default AppRouter;

