import React from "react";
import { Navigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const PrivateRoute = ({ element, ...rest }) => {
    const token = localStorage.getItem("token");

    if (token) {
        const decodedToken = jwtDecode(token);
        const currentTime = Date.now() / 1000;

        if (decodedToken.exp < currentTime) {
            localStorage.removeItem("token");
            return <Navigate to="/login" replace />;
        }
    }

    return token ? element : <Navigate to="/login" replace />;
};

export default PrivateRoute;