import React, {useState} from "react";
import {NavLink} from "react-router-dom";

const Register = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [re_password, setRePassword] = useState("");
    const [error, setError] = useState("");


    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setError(""); // Reset error message

        switch(name) {
            case "email":
                setEmail(value);
                break;
            case "password":
                setPassword(value);
                break;
            case "re_password":
                setRePassword(value);
                break;
            default:
                break;
        }
    }

    const handleSubmit = (event) => {
        event.preventDefault();

        // Check if passwords match
        if (password !== re_password) { 
            setError("Passwords do not match");
            return;
        }

        // API call here
        console("Email: ", email);
        console("Password: ", password);
    }


    return (
        <div>
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                {/* Validation errors here */}
                {error && <p style={{ color: 'red' }}>{error}</p>}
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
                <button type='submit'>Register</button>
                <p>Already have an account? <NavLink to="/login">Login</NavLink></p>
            </form>
        </div>
    );
}

export default Register;