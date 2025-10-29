import { useEffect, useState } from "react";
import apiClient from "../services/apiClient";

interface SystemMessage {
  id: number;
  code: string;
  message: string;
}

const ComingSoon = () => {
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    const fetchMessage = async () => {
      try {
        const response = await apiClient.get<SystemMessage>("/SystemMessage/COMING_SOON");
        setMessage(response.data.message);
      } catch (err) {
        setError("Mesaj alınırken bir hata oluştu.");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchMessage();
  }, []);

  if (loading) return <p>Yükleniyor...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <div style={{ textAlign: "center", marginTop: "100px" }}>
      <h1>{message}</h1>
    </div>
  );
};

export default ComingSoon;
