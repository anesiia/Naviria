import { useNavigate } from "react-router-dom";
import { registration } from "../services/AuthServices";

export function useRegistration() {
  const navigate = useNavigate();

  return async (fullName, nickname, gender, birthDate, email, password) => {
    await registration(fullName, nickname, gender, birthDate, email, password);
    navigate("/profile");
  };
}
