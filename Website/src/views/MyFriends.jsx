import React from "react";
// import { Link, useNavigate } from "react-router-dom";
// import { useEffect, useState } from "react";
import "../styles/friends.css";

export function MyFriends() {
  return (
    <div className="discover-list">
      <div className="item">
        <img className="avatar" src="Ellipse 19.svg" />
        <div className="info">
          <div className="name-lvl">
            <p className="name">Ім'я</p>
            <p className="level">43 lvl</p>
          </div>
          <p className="desc">
            Body text for whatever you'd like to say. Add main takeaway points,
            quotes, anecdotes, or even a very very short story.
          </p>
          <button className="support">Підтримати</button>
          <button className="remove">Видалити</button>
        </div>
      </div>
    </div>
  );
}
