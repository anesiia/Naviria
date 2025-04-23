import { useNavigate } from "react-router-dom";
import { login } from "../services/AuthServices";

export function useLogin() {
  const navigate = useNavigate();

  return async (email, password) => {
    await login(email, password);
    navigate("/profile");
  };
}
