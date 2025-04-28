import React from "react";
import { Link } from "react-router-dom";
import Notifications from "./Notifications";
import { useState, useEffect, useRef } from "react";
import {
  getNotifications,
  markAllNotificationsRead,
} from "../services/NotificationsServices";

import "../styles/header.css";

export default function Header() {
  const [showNotifications, setShowNotifications] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const ref = useRef(null);

  const handleToggleNotifications = async () => {
    if (!showNotifications) {
      try {
        const data = await getNotifications();
        setNotifications(data);
        await markAllNotificationsRead();
      } catch (err) {
        console.error("Помилка завантаження повідомлень:", err.message);
      }
    }
    setShowNotifications(!showNotifications);
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (ref.current && !ref.current.contains(event.target)) {
        setShowNotifications(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <header className="site-header">
      <img className="logo" src="logo.svg" />
      <nav className="links">
        <Link to="/tasks">Навірії</Link>
        <Link to="/helper">Помічник</Link>
        <Link to="/achievements">Досягнення</Link>
        <Link to="/statistics">Статистика</Link>
        <Link to="/friends">Ком'юніті</Link>
      </nav>
      <div className="actions">
        <button className="notifications" onClick={handleToggleNotifications}>
          <img src="bell.svg" alt="notifications" />
        </button>
        {showNotifications && <Notifications notifications={notifications} />}
        <Link to="/profile">
          <img className="avatar" src="Ellipse.svg" alt="avatar" />
        </Link>
      </div>
    </header>
  );
}
