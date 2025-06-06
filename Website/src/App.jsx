import { Routes, Route } from "react-router-dom";
import Header from "./views/Header";
import { HideHeader } from "./hooks/HideHeader";
import { EditProfile } from "./views/EditProfile";
import { Login } from "./views/Login";
import { Profile } from "./views/Profile";
import { Registration } from "./views/Registration";
import { Friends } from "./views/Friends";
import { Achievements } from "./views/Achievements";
import { Tasks } from "./views/Tasks";
import { AssistantChat } from "./views/AssistantChat";
import { Statistics } from "./views/Statistics";

function App() {
  const hideHeader = HideHeader();
  return (
    <div className="layout">
      {!hideHeader && <Header />}
      <div className="page-content">
        <Routes>
          <Route path="/achievements" element={<Achievements />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/friends" element={<Friends />} />
          <Route path="/login" element={<Login />} />
          <Route path="/registration" element={<Registration />} />
          <Route path="/tasks" element={<Tasks />} />
          <Route path="/assistant" element={<AssistantChat />} />
          <Route path="/statistics" element={<Statistics />} />
          <Route path="/edit-profile" element={<EditProfile />} />
          <Route path="/profile/:id" element={<Profile />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
