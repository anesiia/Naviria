import React from "react";
import "../styles/notifications.css";

export default function Notifications({ notifications }) {
  if (!notifications.length) {
    return (
      <div className="notifications-popup">
        <p className="no-notifications">Немає нових повідомлень</p>
      </div>
    );
  }

  return (
    <div className="notifications-popup">
      {notifications.map((notif) => (
        <div
          key={notif.id}
          className={`notification-item ${notif.isRead ? "read" : "unread"}`}
        >
          <p className="notification-text">{notif.text}</p>
          <span className="notification-time">{notif.receivedAt}</span>
        </div>
      ))}
    </div>
  );
}
