import { Routes, Route } from "react-router-dom";
import { Login } from "./views/Login";
import { Registration } from "./views/Registration";

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/registration" element={<Registration />} />
    </Routes>
  );
}

export default App;
