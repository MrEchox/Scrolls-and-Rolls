import React from "react";
import { Route, HashRouter, Routes } from "react-router-dom";

import { AuthProvider } from "./utils/AuthContext";
import PrivateRoute from "./utils/PrivateRoute";

import SessionManager from "./pages/session/SessionManager";
import GameSession from "./pages/session/GameSession";
import Header from "./content_pages/layout/Header";
import Footer from "./content_pages/layout/Footer";
import PCForm from "content_pages/forms/PCForm";
import Login from "./pages/auth/Login";
import Register from "./pages/auth/Register";
import Home from "./pages/Home";
import "./App.css";

const App = () => {
  return (
    <AuthProvider>
      <HashRouter>
          <Header />
          <div className="App">
            <Routes>
              {/* Public Routes */}
              <Route path="/" element={<Login />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              {/* End Public Routes */}

              {/* Private Routes */}
              <Route 
                path="/home"
                element={<PrivateRoute element={<Home />} />}
              />
              <Route 
                path="/session/create-character/:sessionId"
                element={<PrivateRoute element={<PCForm />} roles={["Player"]} />}
              />
              <Route 
                path="/session/session-manager" 
                element={<PrivateRoute element={<SessionManager />} roles={["GameMaster"]} />} 
              />
              <Route 
                path="/session/game/:sessionId" 
                element={<PrivateRoute element={<GameSession />} roles={["GameMaster", "Player"]} />} 
              />
              {/* End Private Routes */}
            </Routes>
          </div>
        <Footer />
      </HashRouter>
    </AuthProvider>
  );
};

export default App;
