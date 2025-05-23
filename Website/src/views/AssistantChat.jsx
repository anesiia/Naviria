import { useEffect, useState } from "react";
import "../styles/assistantChat.css";
import {
  fetchChatHistory,
  sendMessageToAssistant,
} from "../services/AssistantChatServices";

export function AssistantChat() {
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState("");
  const [createTaskEnabled, setCreateTaskEnabled] = useState(false);

  useEffect(() => {
    fetchChatHistory()
      .then(setMessages)
      .catch((err) => console.error("Fetch messages error:", err));
  }, []);

  const handleSend = async () => {
    if (newMessage.trim() === "") return;

    const userMsg = { from: "user", text: newMessage.trim() };
    setMessages((prev) => [...prev, userMsg]);

    try {
      const reply = await sendMessageToAssistant(
        newMessage.trim(),
        createTaskEnabled
      );
      setMessages((prev) => [...prev, { from: "assistant", text: reply.text }]);
    } catch (error) {
      console.error("Send message error:", error);
    }

    setNewMessage("");
  };

  return (
    <div className="assistant-chat-page">
      <div className="header">
        <div className="avatar" />
        <span className="title">Персональний помічник</span>
      </div>

      <div className="messages">
        {messages.map((msg, index) => (
          <div key={index} className={`message ${msg.from}`}>
            {msg.text}
          </div>
        ))}
      </div>

      <div className="input-area">
        <input
          type="text"
          placeholder="Type message"
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleSend()}
        />
        <button onClick={handleSend} className="send-button">
          <img src="PaperPlaneRight.svg" alt="Send" />
        </button>
      </div>

      <div className="task-toggle">
        <input
          type="checkbox"
          id="create-task-toggle"
          checked={createTaskEnabled}
          onChange={() => setCreateTaskEnabled(!createTaskEnabled)}
        />
        <label htmlFor="create-task-toggle">Запит на створення задачі</label>
      </div>
    </div>
  );
}
