import React, { useState } from 'react';
import axios from 'axios';
import '../styles/Auth.css'; // Import the shared CSS
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';

function Register() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [role, setRole] = useState('player');
  const [error, setError] = useState('');

  const handleRegister = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const response = await axios.post('https://your-backend-api.com/register', { email, password, role });
      const { token, user } = response.data;
      localStorage.setItem('token', token);
      console.log('Registered:', user);
    } catch (err) {
      setError('Registration failed. Please try again.');
    }
  };

  return (
    <div className="form-container">
      <h3>Registration</h3>
      {error && <p>{error}</p>}
      <form onSubmit={handleRegister}>
        <div>
          <label>Username:</label>
          <input 
            type="username" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
            required
          />
        </div>
        <div>
          <label>Email:</label>
          <input 
            type="email" 
            value={email} 
            onChange={(e) => setEmail(e.target.value)} 
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required
          />
        </div>
        <div>
          <label>Role:</label>
          <select value={role} onChange={(e) => setRole(e.target.value)}>
            <option value="player">Player</option>
            <option value="gm">Game Master</option>
          </select>
        </div>
        <button type="submit">Register</button>
      </form>
      <br></br>
      <p>Have an existing account?</p>
      <br></br>
      <nav style={{ marginBottom: '20px', textAlign: 'center' }}>
            <Link to="/login" style={navLinkStyle}>Login</Link>
      </nav>
    </div>
  );
}

const navLinkStyle = {
  margin: '0 10px',
  padding: '10px',
  backgroundColor: '#9e2c47',
  color: 'white',
  textDecoration: 'none',
  borderRadius: '5px',
};

export default Register;
