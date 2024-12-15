import React, {useState} from "react";
import {NavLink} from "react-router-dom";

import { register } from "../../utils/ApiService";

const Register = () => {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [re_password, setRePassword] = useState("");
    const [role, setRole] = useState("");
    const [error, setError] = useState("");

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setError(""); // Reset error message

        switch(name) {
            case "username":
                setUsername(value);
                break;
            case "email":
                setEmail(value);
                break;
            case "password":
                setPassword(value);
                break;
            case "re_password":
                setRePassword(value);
                break;
            case "role":
                setRole(value);
                break;
            default:
                break;
        }
    }

    const validateForm = () => {
        // Username validation
        if (username.length < 3 || username.length > 20) {
            return "Username must be between 3 and 20 characters";
        }

        // Email validation
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            return "Invalid email format.";
        }

        // Password validation
        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,32}$/;
        if (!passwordRegex.test(password)) {
            return "Password must contain at least one uppercase letter, one lowercase letter, and one number, and be between 8 and 32 characters";
        }

        // Password match validation
        if (password !== re_password) {
            return "Passwords do not match";
        }

        // Role validation (must be either GameMaster or Player)
        if (role !== "Player" && role !== "GameMaster") {
            return "Role must be either 'GameMaster' or 'Player'.";
        }

        return null; // No errors
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        const error = validateForm();
        if (error) {
            setError(error);
            return;
        }
        
        try {
            await register(email, password, role, username);
            console.log("Registration successful!");
            window.location.hash = "/login";
        }
        catch (error) {
            console.error(error);
            setError("Registration failed: " + error.response.data);
        }
    }


    return (
        <div>
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                {/* Validation errors here */}
                {error && <p style={{ color: 'red' }}>{error}</p>}
                <div>
                    <label>Username:</label>
                    <input
                    name="username"
                    onChange={handleInputChange}
                    value={username}
                    required
                    />
                </div>
                <div>
                    <label>Email:</label>
                    <input
                    type="email"
                    name="email"
                    onChange={handleInputChange}
                    value={email}
                    required
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                    type="password"
                    name="password"
                    onChange={handleInputChange}
                    value={password}
                    required
                    />
                </div>
                <div>
                    <label>Re-enter password:</label>
                    <input
                    type="password"
                    name="re_password"
                    onChange={handleInputChange}
                    value={re_password}
                    required
                    />
                </div>
                <div>
                    <label>Select role:   </label>
                    <select className="role-drop" name="role" onChange={handleInputChange} value={role} required>
                    <option value=""></option>
                    <option value="Player">Player</option>
                    <option value="GameMaster">Game Master</option>
                    </select>
                </div>
                <button type='submit'>Register</button>
            </form>
        </div>
    );
}

export default Register;