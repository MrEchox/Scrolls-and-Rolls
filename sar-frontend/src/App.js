import React from "react";
import { Route, HashRouter, Router, Routes } from "react-router-dom";

import PrivateRoute from "./utils/PrivateRoute";
import Login from "./content_pages/auth/Login";
import Register from "./content_pages/auth/Register";
import Home from "./content_pages/Home";
import NewSession from "./content_pages/session/NewSession";
import CreateSession from "./content_pages/session/CreateSession";
import Header from "./content_pages/Header";

const App = () => {
  return (
    <HashRouter>
      <div className="App">
        <h1>Scrolls and Rolls</h1>

        <Header />

        <div className="content">
          <Routes>
            {/* Public Routes */}
            <Route path="/" element={<Login />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            {/* End Public Routes */}

            {/* Private Routes */}
            <Route path="/home"
              element={<PrivateRoute element={<Home />} />}
            />
            {/* End Private Routes */}
          </Routes>
        </div>
      </div>
    </HashRouter>
  );
};

export default App;
