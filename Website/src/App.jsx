import { Routes, Route } from "react-router-dom";
import Header from "./views/Header";
import { HideHeader } from "./hooks/HideHeader";
// import Footer from "./views/Footer";
import { Login } from "./views/Login";
import { Profile } from "./views/Profile";
import { Registration } from "./views/Registration";
import { Friends } from "./views/Friends";

function App() {
  const hideHeader = HideHeader();
  return (
    <div className="layout">
      {!hideHeader && <Header />}
      <div className="page-content">
        <Routes>
          <Route path="/profile" element={<Profile />} />
          <Route path="/friends" element={<Friends />} />
          <Route path="/login" element={<Login />} />
          <Route path="/registration" element={<Registration />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
