import axios, {AxiosError} from "axios";
import type { AxiosResponse, AxiosRequestConfig } from "axios";
import { authService } from "./authService";

const BASE_URL = "http://localhost:5250/api";

const apiClient = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

/* ----------------------------- REQUEST ----------------------------- */
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("accessToken");
    if (token) {
      config.headers = config.headers ?? {};
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

/* ----------------------------- REFRESH QUEUE ----------------------------- */
let isRefreshing = false;

let failedQueue: Array<{
  resolve: (token: string) => void;
  reject: (err: any) => void;
}> = [];

function processQueue(error: any, token: string | null = null) {
  failedQueue.forEach((prom) => {
    if (error) prom.reject(error);
    else prom.resolve(token!);
  });
  failedQueue = [];
}

/* ----------------------------- RESPONSE ----------------------------- */
apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as AxiosRequestConfig & { _retry?: boolean };

    /* ----------------------------- 401: Access Token Expired ----------------------------- */
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({
            resolve: (newToken: string) => {
              originalRequest.headers = originalRequest.headers ?? {};
              originalRequest.headers.Authorization = `Bearer ${newToken}`;
              resolve(apiClient(originalRequest));
            },
            reject,
          });
        });
      }

      isRefreshing = true;

      try {
        const res = await authService.refresh();
        const newToken = res.token;

        localStorage.setItem("accessToken", newToken);
        localStorage.setItem("refreshToken", res.refreshToken);

        processQueue(null, newToken);

        originalRequest.headers = originalRequest.headers ?? {};
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return apiClient(originalRequest);
      } catch (refreshErr) {
        processQueue(refreshErr, null);

        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");

        console.warn("Session expired. Please login again.");
        window.location.href = "/login";

        return Promise.reject(refreshErr);
      } finally {
        isRefreshing = false;
      }
    }

    /* ----------------------------- 403: Forbidden ----------------------------- */
    if (error.response?.status === 403) {
      console.warn("You are not authorized to perform this action.");
    }

    /* ----------------------------- 500+ Server Errors ----------------------------- */
    if (error.response?.status && error.response.status >= 500) {
      console.error("Server error:", error.response.status);
    }

    return Promise.reject(error);
  }
);

export default apiClient;