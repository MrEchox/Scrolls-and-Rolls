import React, { createContext, useContext, useState, useEffect} from "react";
import { jwtDecode } from "jwt-decode";
import { login as loginApiCall} from "./ApiService";

const AuthContext = createContext();

export const useAuth = () => {
    return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
    const [isAuthed, setIsAuthed] = useState(false);

    const [userToken, setUserToken] = useState(null);
    const [userRole, setUserRole] = useState(null);
    const [userName, setUserName] = useState(null);
    const [userId, setUserId] = useState(null);

    useEffect(() => {
        const token = localStorage.getItem("token");
        setUserToken(token);

        if (token) {
            const decodedToken = jwtDecode(token);
            setAuthState(decodedToken);
        } else {
        }
        
    }, []);

    const setAuthState = (decodedToken) => {
        if (decodedToken.exp < Date.now() / 1000) {
            console.warn("Token has expired");
            logout();
            return;
        }
        setIsAuthed(true);
        setUserRole(decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]);
        setUserName(decodedToken.unique_name);
        setUserId(decodedToken.sub);
    }

    // Login function
    const login = async (email, password) => {
        try {
            const response = await loginApiCall(email, password); // Make API call
            const token = response.token;
            localStorage.setItem("token", token);
            setUserToken(token);
            const decodedToken = jwtDecode(token);
            setAuthState(decodedToken);
        } catch (error) {
            console.error("Login failed:", error);
            throw new Error("Invalid credentials");
        }
    };

    // Logout function
    const logout = () => {
        localStorage.removeItem("token");
        setIsAuthed(false);
        setUserRole(null);
        setUserName(null);
        setUserId(null);
    };

    return (
        <AuthContext.Provider value={{ isAuthed, login, logout, userRole, userName, userToken, userId}}>
            {children}
        </AuthContext.Provider>
    );
};