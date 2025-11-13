import { useNavigate } from "react-router-dom";

const HomePage = () => {
  const navigate = useNavigate();

  return (
    <div
      style={{
        height: "100vh",
        display: "flex",
        flexDirection: "column",
        gap: "20px",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <h1 style={{ marginBottom: "20px" }}>Welcome</h1>

      <button
        style={{
          padding: "12px 24px",
          fontSize: "18px",
          cursor: "pointer",
        }}
        onClick={() => navigate("/login")}
      >
        Login
      </button>

      <button
        style={{
          padding: "12px 24px",
          fontSize: "18px",
          cursor: "pointer",
        }}
        onClick={() => navigate("/register")}
      >
        Register
      </button>
    </div>
  );
};

export default HomePage;
