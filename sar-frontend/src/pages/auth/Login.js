import React, { useState } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import '../styles/Auth.css'; // Import the shared CSS

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const response = await axios.post('https://your-backend-api.com/login', { email, password });
      const { token, user } = response.data;
      localStorage.setItem('token', token);
      console.log('Logged in as:', user);
    } catch (err) {
      setError('Login failed. Please check email and password.');
    }
  };

  return (
    <div className="form-container">
        <h1>Scrolls and Rolls</h1>
      <h3>Login</h3>
      {error && <p>{error}</p>}
      <form onSubmit={handleLogin}>
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
        <button type="submit">Login</button>
      </form>
      <br></br>
      <p>Don't have an account?</p>
      <br></br>
      <nav style={{ marginBottom: '20px', textAlign: 'center' }}>
            <Link to="/register" style={navLinkStyle}>Register</Link>
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

export default Login;
