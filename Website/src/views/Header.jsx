import React from "react";
import { Link } from "react-router-dom";
import "../styles/header.css";

export default function Header() {
  return (
    <header className="site-header">
      <img className="logo" src="logo.svg" />
      <nav className="links">
        <Link to="/tasks">Навірії</Link>
        <Link to="/helper">Помічник</Link>
        <Link to="/achievements">Досягнення</Link>
        <Link to="/statistics">Статистика</Link>
        <Link to="/community">Ком'юніті</Link>
      </nav>
      <div className="actions">
        <button className="notifications">
          <img src="bell.svg" alt="notifications" />
        </button>
        <img className="avatar" src="Ellipse.svg" alt="avatar" />
      </div>
    </header>
  );
}
