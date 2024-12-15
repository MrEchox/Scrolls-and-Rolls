import React from "react";
import { Navigate } from "react-router-dom"; 
import { useAuth } from "./AuthContext"; 

const PrivateRoute = ({ element, roles, ...rest }) => {
    const { isAuthed, userRole } = useAuth();

    if (!isAuthed) {
        return <Navigate to="/login" />;
    }

    if (roles && !roles.includes(userRole)) {
        return <Navigate to="/home" />;
    }

    return element; 
};

export default PrivateRoute;
