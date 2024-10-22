import React from "react";
import { Route, NavLink, HashRouter, Routes } from "react-router-dom";

import Login from "./content_pages/auth/Login";
import Register from "./content_pages/auth/Register";
import Home from "./content_pages/Home";
import NewSession from "./content_pages/session/NewSession";
import CreateSession from "./content_pages/session/CreateSession";

const App = () => {
  return (
    <HashRouter>
      <div className="App">
        <h1>Scrolls and Rolls</h1>

        <div className="header">
          {/* Navigation Links */}
          <ul>
            <li><NavLink to="/login">Login</NavLink></li>
            <li><NavLink to="/register">Register</NavLink></li>
            <li><NavLink to="/home">Home</NavLink></li>
            <li><NavLink to="/session/new-session">New Session</NavLink></li>
          </ul>
        </div>

        <div className="content">
          {/* Routes for Navigation */}
          <Routes>
            <Route path="/" element={<Login />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/home" element={<Home />} />
            <Route path="/session/new-session" element={<NewSession />} />
            <Route path="/session/create-session" element={<CreateSession />} />
          </Routes>
        </div>
      </div>
    </HashRouter>
  );
};

export default App;
