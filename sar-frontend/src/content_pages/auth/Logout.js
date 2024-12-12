import React from "react";

const Logout = () => {
    const handleLogout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        window.location.hash = "/login";
        console.log("Logout successful!");
    };

    return (
        <button onClick={handleLogout}>Logout</button>
    );
};

export default Logout;
