import React from "react";
import { Link } from "react-router-dom";
import Notifications from "./Notifications";
import { useState, useEffect, useRef } from "react";
import {
  getNotifications,
  markAllNotificationsRead,
} from "../services/NotificationsServices";
import { getProfile } from "../services/ProfileServices";
import "../styles/header.css";

export default function Header() {
  const [showNotifications, setShowNotifications] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [user, setUser] = useState(null);
  const ref = useRef(null);
  const hasUnread = notifications.some((n) => n.isNew);

  useEffect(() => {
    getProfile()
      .then((data) => setUser(data))
      .catch(() => setUser(null));
  }, []);
  const fetchNotificationsAgain = async () => {
    try {
      const data = await getNotifications();
      setNotifications(data);
    } catch (err) {
      console.error("Помилка оновлення сповіщень:", err.message);
    }
  };

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
      <img className="logo" src="/logo.svg" />
      <nav className="links">
        <Link to="/tasks">Навірії</Link>
        <Link to="/assistant">Помічник</Link>
        <Link to="/achievements">Досягнення</Link>
        <Link to="/statistics">Статистика</Link>
        <Link to="/friends">Ком'юніті</Link>
      </nav>
      <div className="actions">
        <button className="notifications" onClick={handleToggleNotifications}>
          <img
            src={hasUnread ? "/new-notif.svg" : "/bell.svg"}
            alt="notifications"
          />
        </button>
        {showNotifications && (
          <Notifications
            notifications={notifications}
            onMarkRead={fetchNotificationsAgain}
          />
        )}

        <Link to="/profile">
          <img
            className="avatar"
            src={user && user.photo ? user.photo : "Ellipse.svg"}
            alt={user ? user.nickname : "avatar"}
          />
        </Link>
      </div>
    </header>
  );
}
