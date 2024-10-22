import React, { useState } from "react";
import { NavLink } from "react-router-dom";

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleInputChange = (event) => {
        const { name, value } = event.target;

        // Update state based on input field
        if (name === "email") {
            setEmail(value);
        } else if (name === "password") {
            setPassword(value);
        }
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        // API call here
        console.log("Email: ", email);
        console.log("Password: ", password);
    };

    return (
        <div>
            <h2>Login</h2>
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
