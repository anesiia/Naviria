import React, { useState, useEffect } from "react";
import {
  getProfile,
  updateProfile,
  uploadProfilePhoto,
} from "../services/ProfileServices";
import "../styles/editProfile.css";

export function EditProfile() {
  const [profile, setProfile] = useState(null);
  const [form, setForm] = useState({
    fullName: "",
    nickname: "",
    description: "",
    email: "",
    password: "",
  });
  const [photoFile, setPhotoFile] = useState(null);
  const [photoPreview, setPhotoPreview] = useState(null);

  const [loading, setLoading] = useState(false);
  const [photoLoading, setPhotoLoading] = useState(false);
  const [error, setError] = useState(null);
  const [photoError, setPhotoError] = useState(null);

  useEffect(() => {
    getProfile()
      .then((data) => {
        setProfile(data);
        setForm({
          fullName: data.fullName || "",
          nickname: data.nickname || "",
          description: data.description || "",
          email: data.email || "",
          password: "", // пароль не виводимо
        });
        setPhotoPreview(data.photo || null);
      })
      .catch((e) => setError(e.message));
  }, []);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handlePhotoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setPhotoFile(file);
      setPhotoPreview(URL.createObjectURL(file));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      // Якщо треба оновити пароль - можна перевірити, що поле не пусте
      const updateData = {
        fullName: form.fullName,
        nickname: form.nickname,
        description: form.description,
        email: form.email,
      };
      if (form.password) {
        updateData.password = form.password;
      }
      await updateProfile(updateData);
      alert("Профіль успішно оновлено");
    } catch (e) {
      setError(e.message);
    }
    setLoading(false);
  };

  const handlePhotoUpload = async () => {
    if (!photoFile) return;
    setPhotoLoading(true);
    setPhotoError(null);
    try {
      await uploadProfilePhoto(photoFile);
      alert("Фото профілю оновлено");
      // Можна оновити photoPreview або зробити повторне завантаження профілю
    } catch (e) {
      setPhotoError(e.message);
    }
    setPhotoLoading(false);
  };

  if (!profile) return <p>Завантаження профілю...</p>;

  return (
    <div className="edit-profile-page">
      <h2>Редагування профілю</h2>
      <div className="photo-section">
        {photoPreview ? (
          <img src={photoPreview} alt="Preview" className="photo-preview" />
        ) : (
          <div className="photo-placeholder">Нема фото</div>
        )}
        <input type="file" accept="image/*" onChange={handlePhotoChange} />
        <button
          disabled={!photoFile || photoLoading}
          onClick={handlePhotoUpload}
        >
          {photoLoading ? "Завантаження..." : "Зберегти фото"}
        </button>
        {photoError && <p className="error">{photoError}</p>}
      </div>

      <form onSubmit={handleSubmit} className="profile-form">
        <label>
          Повне ім'я
          <input
            name="fullName"
            value={form.fullName}
            onChange={handleChange}
          />
        </label>
        <label>
          Нікнейм
          <input
            name="nickname"
            value={form.nickname}
            onChange={handleChange}
          />
        </label>
        <label>
          Опис
          <textarea
            name="description"
            value={form.description}
            onChange={handleChange}
          />
        </label>
        <label>
          Email
          <input
            name="email"
            type="email"
            value={form.email}
            onChange={handleChange}
          />
        </label>
        <label>
          Пароль
          <input
            name="password"
            type="password"
            pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"
            value={form.password}
            onChange={handleChange}
          />
        </label>
        <button disabled={loading} type="submit">
          {loading ? "Збереження..." : "Зберегти зміни"}
        </button>
        {error && <p className="error">{error}</p>}
      </form>
    </div>
  );
}
