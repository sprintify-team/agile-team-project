import './App.css'
import { BrowserRouter } from "react-router-dom";
import AppRouter from "./router/AppRouter";
import { AuthProvider } from "./context/AuthContext";

function App() {
  return (
      <AuthProvider>
      <BrowserRouter>
        <AppRouter />
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App