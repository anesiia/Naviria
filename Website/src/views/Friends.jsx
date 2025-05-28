import React from "react";
// import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import {
  getUserFriends,
  getFriendRequests,
  getDiscoverUsers,
  sendFriendRequest,
  updateFriendRequest,
  deleteFriend,
  searchFriends,
  searchMyFriends,
} from "../services/FriendsServices";
import { fetchCategories } from "../services/TasksServices";
import "../styles/friends.css";

export function Friends() {
  const [activeTab, setActiveTab] = useState("discover"); // "my", "discover", "requests"
  const [friends, setFriends] = useState([]);
  const [requests, setRequests] = useState([]);
  const [discover, setDiscover] = useState([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(""); // id категорії або ""

  useEffect(() => {
    const fetchAllCategories = async () => {
      try {
        const data = await fetchCategories(); // імпортуй з TasksServices.jsx
        setCategories(data);
      } catch (e) {
        console.error("Не вдалося отримати категорії:", e.message);
      }
    };
    fetchAllCategories();
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (activeTab === "my") {
          const data = await getUserFriends();
          setFriends(data);
        } else if (activeTab === "requests") {
          const data = await getFriendRequests();
          setRequests(data);
        } else if (activeTab === "discover") {
          const data = await getDiscoverUsers();
          setDiscover(data);
        }
      } catch (e) {
        console.error("Помилка при завантаженні:", e.message);
      }
    };
    fetchData();
  }, [activeTab]);

  useEffect(() => {
    const fetchFriends = async () => {
      try {
        const data = await getUserFriends();
        setFriends(data);
      } catch (e) {
        console.error("Помилка при завантаженні друзів:", e.message);
      }
    };
    fetchFriends();
  }, []);
  const handleUpdateRequest = async (requestId, status) => {
    try {
      await updateFriendRequest(requestId, status);

      // Оновлюємо всі списки паралельно
      const [updatedFriends, updatedRequests, updatedDiscover] =
        await Promise.all([
          getUserFriends(),
          getFriendRequests(),
          getDiscoverUsers(),
        ]);
      setFriends(updatedFriends);
      setRequests(updatedRequests);
      setDiscover(updatedDiscover);
    } catch (e) {
      console.error("Не вдалося оновити статус запиту:", e.message);
    }
  };

  const renderActions = (friend, tab = activeTab) => {
    switch (tab) {
      case "my":
        return (
          <div className="actions">
            <button
              className="remove"
              onClick={() => handleRemoveFriend(friend.id)}
            >
              Видалити
            </button>
            <button className="support">Підтримати</button>
          </div>
        );

      case "discover":
      default:
        return (
          <div className="actions">
            <button
              className="add-friend"
              onClick={() => handleSendRequest(friend.id)}
            >
              Додати друга
            </button>
          </div>
        );
    }
  };
  const renderList = () => {
    const list =
      activeTab === "my"
        ? friends
        : activeTab === "requests"
        ? requests
        : discover;

    return (
      <div className="discover-list">
        {list.map((friend) => {
          if (activeTab === "requests") {
            const { request, sender } = friend;
            return (
              <div className="item" key={request.id}>
                <img
                  className="avatar"
                  src={sender.photo || "Ellipse 4.svg"}
                  alt={sender.nickname || "avatar"}
                />
                <div className="info">
                  <div className="name-lvl">
                    <p className="name">{sender.nickname}</p>
                    <p className="level">
                      {sender.levelInfo?.level ?? 0} рівень
                    </p>
                  </div>
                  <p className="desc">
                    {sender.description || "Опис відсутній"}
                  </p>
                  <div className="actions">
                    <button
                      className="reject"
                      onClick={() =>
                        handleUpdateRequest(request.id, "rejected")
                      }
                    >
                      Відхилити
                    </button>
                    <button
                      className="accept"
                      onClick={() =>
                        handleUpdateRequest(request.id, "accepted")
                      }
                    >
                      Прийняти
                    </button>
                  </div>
                </div>
              </div>
            );
          }

          // Для вкладок "my" та "discover" item — це просто користувач
          return (
            <div className="item" key={friend.id}>
              <img
                className="avatar"
                src={friend.photo || "Ellipse 4.svg"}
                alt={friend.nickname || "avatar"}
              />
              <div className="info">
                <div className="name-lvl">
                  <p className="name">{friend.nickname}</p>
                  <p className="level">{friend.levelInfo?.level ?? 0} рівень</p>
                </div>
                <p className="desc">{friend.description || "Опис відсутній"}</p>
                {renderActions(friend)}
              </div>
            </div>
          );
        })}
      </div>
    );
  };

  const handleSendRequest = async (toUserId) => {
    try {
      await sendFriendRequest(toUserId);
      setDiscover((prev) => prev.filter((u) => u.id !== toUserId));
    } catch (e) {
      console.error("Не вдалося надіслати запит:", e.message);
    }
  };

  const handleRemoveFriend = async (friendId) => {
    try {
      await deleteFriend(friendId);
      setFriends((prev) => prev.filter((f) => f.id !== friendId));
    } catch (e) {
      console.error("Не вдалося видалити друга:", e.message);
    }
  };

  // const handleSearchDiscover = async () => {
  //   if (activeTab !== "discover") return; // ❗ Нічого не робити, якщо вкладка не discover
  //   if (!searchQuery.trim()) {
  //     try {
  //       const data = await getDiscoverUsers();
  //       setDiscover(data);
  //     } catch (e) {
  //       console.error("Помилка завантаження discover:", e.message);
  //     }
  //     return;
  //   }
  //   try {
  //     const data = await searchFriends(searchQuery);
  //     setDiscover(data);
  //   } catch (e) {
  //     console.error("Помилка пошуку:", e.message);
  //   }
  // };

  // const handleSearchMyFriends = async () => {
  //   if (!friendSearchQuery.trim() && !selectedCategory) {
  //     const all = await getUserFriends();
  //     setFriends(all);
  //     return;
  //   }

  //   try {
  //     // якщо обрана категорія - додаємо до запиту
  //     const data = await searchMyFriends(friendSearchQuery, selectedCategory);
  //     setFriends(data);
  //   } catch (e) {
  //     console.error("Не вдалося знайти друзів:", e.message);
  //   }
  // };

  const handleSearch = async () => {
    if (activeTab === "my") {
      // Пошук серед друзів з урахуванням фільтру
      try {
        // Якщо пустий пошук і пустий фільтр — просто завантажуємо всіх друзів
        if (!searchQuery.trim() && !selectedCategory) {
          const data = await getUserFriends();
          setFriends(data);
          return;
        }

        // Якщо пустий пошук, але є фільтр — підставляємо пробіл у query (щоб API прийняв)
        const queryForApi = searchQuery.trim() ? searchQuery.trim() : " ";
        const data = await searchMyFriends(queryForApi, selectedCategory);
        setFriends(data);
      } catch (e) {
        console.error("Помилка пошуку друзів:", e.message);
      }
    } else if (activeTab === "discover") {
      // Пошук у discover (як раніше)
      try {
        if (!searchQuery.trim()) {
          const data = await getDiscoverUsers();
          setDiscover(data);
          return;
        }
        const data = await searchFriends(searchQuery);
        setDiscover(data);
      } catch (e) {
        console.error("Помилка пошуку у discover:", e.message);
      }
    }
  };

  const renderMyFriendsList = () => {
    return (
      <div className="friend-list">
        {friends.map((friend, index) => (
          <div className="friend" key={index}>
            <div className="info">
              <img
                className="avatar"
                src={friend && friend.photo ? friend.photo : "Ellipse 4.svg"}
                alt={friend ? friend.nickname : "avatar"}
              />
              <div className="name">{friend.nickname}</div>
            </div>
            {renderActions(friend, "my")}
          </div>
        ))}
      </div>
    );
  };

  return (
    <div className="friends-page">
      <aside className="side-bar">
        <div className="title">
          <img className="icon" src="friends-icon.svg" />
          <h2 className="title-text">Мої друзі</h2>
        </div>
        <div className="search-box">
          <input type="text" placeholder="Пошук" />
          <button className="search-btn">
            <img src="search.svg" alt="search" />
          </button>
        </div>
        {renderMyFriendsList()}
      </aside>
      <main className="content">
        <div className="tabs">
          <button
            className={activeTab === "my" ? "active" : ""}
            onClick={() => setActiveTab("my")}
          >
            Мої друзі
          </button>
          <button
            className={activeTab === "discover" ? "active" : ""}
            onClick={() => setActiveTab("discover")}
          >
            Знайти друзів
          </button>
          <button
            className={activeTab === "requests" ? "active" : ""}
            onClick={() => setActiveTab("requests")}
          >
            Запити
          </button>
        </div>
        <div className="search">
          <div className="search-box">
            <input
              type="text"
              placeholder={activeTab === "my" ? "Пошук серед друзів" : "Пошук"}
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
            <button className="search-btn" onClick={handleSearch}>
              <img src="search.svg" alt="search" />
            </button>
          </div>
          {activeTab === "my" && (
            <select
              className="options"
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
            >
              <option value="">Без фільтра</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
          )}
        </div>

        {renderList()}
      </main>
    </div>
  );
}
