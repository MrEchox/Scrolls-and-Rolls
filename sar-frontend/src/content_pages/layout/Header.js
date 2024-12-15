import React, { useState, useEffect } from "react";
import { NavLink } from "react-router-dom";
import { useAuth } from "../../utils/AuthContext";
import Logout from "../../pages/auth/Logout";

const Header = () => {
  const { isAuthed, userName, userRole } = useAuth();
  const [menuOpen, setMenuOpen] = useState(false);

  const toggleMenu = () => {
    setMenuOpen((prev) => !prev);
  };

  // Reset menuOpen when screen resizes
  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth > 1000) {
        setMenuOpen(false); // Close the menu when screen size is larger than 1000px
      }
    };

    window.addEventListener("resize", handleResize);

    // Cleanup event listener on unmount
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  return (
    <header className="header">
      

      <div className="logo">
        <img src="/scroll.svg" alt="Scroll Icon" className="icon" />
        <h1>Scrolls & Rolls</h1>
      </div>

      {/* Navigation Links */}
      <nav className={`nav-links ${menuOpen ? "open" : ""}`}>
        {isAuthed ? (
          <>
            <p className="user-info">
              Logged in as: <strong>{userName}</strong> ({userRole})
            </p>
            <ul className="menu-info">
              <li className="menu-navlink">
                <NavLink to="/home">Home</NavLink>
              </li>
              <li className="menu-navlink">
                <Logout />
              </li>
            </ul>
          </>
        ) : (
          <ul className="menu-info">
            <li className="menu-navlink">
              <NavLink to="/login">Login</NavLink>
            </li>
            <li className="menu-navlink">
              <NavLink to="/register">Register</NavLink>
            </li>
          </ul>
        )}
      </nav>
      {/* Hamburger Menu Button */}
      <button className="hamburger" onClick={toggleMenu} aria-label="Toggle Menu">
        <div className="hamburger-line"></div>
        <div className="hamburger-line"></div>
        <div className="hamburger-line"></div>
      </button>
    </header>
  );
};

export default Header;
