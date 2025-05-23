import { authHeaders } from "./AuthServices";

const API_URL = "http://localhost:5186";

export async function fetchChatHistory() {
  const id = localStorage.getItem("id");
  const res = await fetch(`${API_URL}/api/AssistantChat/user/${id}`, {
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося отримати повідомлення");
  }

  return data.map((msg) => ({
    from: msg.userId === id ? "user" : "assistant",
    text: msg.message,
  }));
}

export async function sendMessageToAssistant(message, isTaskRequest) {
  const id = localStorage.getItem("id");

  const res = await fetch(`${API_URL}/api/AssistantChat/ask`, {
    method: "POST",
    headers: {
      ...authHeaders(),
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      userId: id,
      message,
      isTaskRequest,
    }),
  });

  const data = await res.json();

  if (!res.ok) {
    throw new Error(data.message || "Не вдалося надіслати повідомлення");
  }

  return data;
}
