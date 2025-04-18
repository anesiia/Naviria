import { useLocation } from "react-router-dom";

export function HideHeader() {
  const { pathname } = useLocation();
  const hiddenPaths = ["/login", "/registration"];

  return hiddenPaths.includes(pathname);
}
