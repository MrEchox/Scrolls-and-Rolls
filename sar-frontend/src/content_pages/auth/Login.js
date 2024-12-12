import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import { login } from "../../utils/ApiService";

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleInputChange = (event) => {
        const { name, value } = event.target;

        // Update state based on input field
        if (name === "email") {
            setEmail(value);
        } else if (name === "password") {
            setPassword(value);
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            const response = await login(email, password);
            console.log(response);
            localStorage.setItem("token", response.token);
            localStorage.setItem("refreshToken", response.refreshToken);
            console.log("Login successful!");
            window.location.hash = "/home";
        }
        catch (error) {
            console.error(error);
            setError("Login failed. Please check your email and password.");
        }
    };

    return (
        <div>
            <h2>Login</h2>
            {error && <p style={{ color: "red", marginTop: "10px" }}>{error}</p>}
            <form onSubmit={handleSubmit}>
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
                <button type="submit">Login</button>

                <p>Don't have an account? <NavLink to="/register">Register</NavLink></p>
            </form>
        </div>
    );
};

export default Login;
