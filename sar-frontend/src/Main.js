// src/Main.js
import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './pages/auth/Login';
import Register from './pages/auth/Register';
import Home from './pages/Home';
import './pages/styles/Auth.css'; // Import shared styles

const Main = () => {
  const [isDarkMode, setIsDarkMode] = useState(true);
  const [user, setUser] = useState({ 
    name: 'Test User', 
    //account_type: 'Player'
    account_type: 'Game Master' 
    //account_type: 'Administrator'
  });

  const toggleTheme = () => {
    setIsDarkMode(!isDarkMode);
  };

  const handleLogout = () => {
    // Logic for logout can be added here (e.g., clearing user state)
    setUser(null); // Clear user state
    // Redirect to login or homepage if needed
  };

  return (
      <div>
        <header style={headerStyle}>
          <h1>Welcome, {user ? user.name : 'Guest'}</h1>
          {user && <button onClick={handleLogout} style={buttonStyle}>Logout</button>}
        </header>
        <Routes>
          <Route path="/" element={<Home user={user} />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Routes>
      </div>
  );
};

// Header style
const headerStyle = {
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  padding: '20px',
  backgroundColor: 'var(--form-background-color)',
  color: 'var(--text-color)',
};

// Button style
const buttonStyle = {
  backgroundColor: 'var(--primary-color)',
  color: 'white',
  border: 'none',
  borderRadius: '5px',
  cursor: 'pointer',
  padding: '10px 15px',
  fontSize: '16px',
};

export default Main;
