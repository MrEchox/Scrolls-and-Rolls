import React, { useEffect, useState } from "react";
import { Route, NavLink, HashRouter, Routes } from "react-router-dom";
import Logout from "./auth/Logout";

const Header = () => {
    const [isAuthed, setIsAuthed] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            setIsAuthed(true);
        }
    }, []);

    return (
        <div className="header">
            <ul>
                {isAuthed ? (
                    <>
                        <li><NavLink to="/home">Home</NavLink></li>
                        <li><NavLink to="/session/new-session">New Session</NavLink></li>
                        <li><Logout /></li>
                    </>
                ) : (
                    <>
                        <li><NavLink to="/login">Login</NavLink></li>
                        <li><NavLink to="/register">Register</NavLink></li>
                    </>
                )}
            </ul>
        </div>
    );
};

export default Header;