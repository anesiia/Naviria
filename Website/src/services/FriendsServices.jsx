import { authHeaders } from "./AuthServices";
const API_URL = "http://localhost:5186";

export async function getUserFriends() {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/Friends/${id}/`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });
  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати профіль");
  }

  return data;
}

export async function getFriendRequests() {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/FriendRequest/incoming/${id}`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });
  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати профіль");
  }

  return data;
}

export async function getDiscoverUsers() {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/Friends/${id}/potential-friends`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });
  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати профіль");
  }

  return data;
}

export async function sendFriendRequest(toUserId) {
  const fromUserId = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/FriendRequest`, {
    method: "POST",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ fromUserId, toUserId }),
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося надіслати запит");
  }

  return data;
}

export async function updateFriendRequest(requestId, status) {
  const res = await fetch(`${API_URL}/api/FriendRequest/${requestId}`, {
    method: "PUT",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ status }),
  });

  if (!res.ok) {
    throw new Error("Не вдалося оновити запит");
  }
}

export async function deleteFriend(friendId) {
  const fromUserId = localStorage.getItem("id");

  const res = await fetch(
    `${API_URL}/api/Friends/${fromUserId}/to/${friendId}`,
    {
      method: "DELETE",
      headers: {
        ...authHeaders(),
      },
    }
  );

  if (!res.ok) {
    throw new Error("Не вдалося видалити друга");
  }
}

export async function searchFriends(query) {
  const id = localStorage.getItem("id");

  const res = await fetch(
    `${API_URL}/api/Friends/${id}/search-friends?query=${encodeURIComponent(
      query
    )}`,
    {
      headers: {
        ...authHeaders(),
        "Content-Type": "application/json",
      },
    }
  );

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося знайти друзів");
  }

  return data;
}

export async function searchMyFriends(query, categoryId) {
  const id = localStorage.getItem("id");
  let url = `${API_URL}/api/Friends/${id}/search-friends?query=${encodeURIComponent(
    query
  )}`;
  if (categoryId) {
    url += `&categoryId=${categoryId}`;
  }

  const res = await fetch(url, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося знайти друзів");
  }

  return data;
}
