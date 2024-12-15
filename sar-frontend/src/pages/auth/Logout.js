import React from "react";
import { useNavigate } from "react-router-dom";

import { useAuth } from "../../utils/AuthContext";

const Logout = () => {
    const { logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/login"); // Redirect to login page
        console.log("Logged out");
    };

    return (
        <button onClick={handleLogout}>Logout</button>
    );
};

export default Logout;
