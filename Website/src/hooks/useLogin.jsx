import { useNavigate } from "react-router-dom";
import { login } from "../services/LoginServices";

export function useLogin() {
  const navigate = useNavigate();

  return async (email, password) => {
    await login(email, password);
    navigate("/profile");
  };
}
