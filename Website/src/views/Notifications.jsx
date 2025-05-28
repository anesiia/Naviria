import React from "react";
import "../styles/notifications.css";
import { markAllNotificationsRead } from "../services/NotificationsServices";

export default function Notifications({ notifications, onMarkRead }) {
  if (!notifications.length) {
    return (
      <div className="notifications-popup">
        <p className="no-notifications">Немає нових повідомлень</p>
      </div>
    );
  }

  const handleMarkAllRead = async () => {
    await markAllNotificationsRead();
    onMarkRead(); // викликає перезавантаження списку з батьківського компонента
  };

  const formatTime = (isoString) => {
    const date = new Date(isoString);
    return date.toLocaleString("uk-UA", {
      hour: "2-digit",
      minute: "2-digit",
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    });
  };

  return (
    <div className="notifications-popup">
      <div className="notifications-header">
        <h3 className="notifications-title">Повідомлення</h3>
        <button className="mark-read-btn" onClick={handleMarkAllRead}>
          Прочитати всі
        </button>
      </div>
      {[...notifications]
        .sort((a, b) => new Date(b.recievedAt) - new Date(a.recievedAt))
        .map((notif) => (
          <div
            key={notif.id}
            className={`notification-item ${notif.isNew ? "unread" : "read"}`}
          >
            <p className="notification-text">{notif.text}</p>
            <span className="notification-time">
              {formatTime(notif.recievedAt)}
            </span>
          </div>
        ))}
    </div>
  );
}
