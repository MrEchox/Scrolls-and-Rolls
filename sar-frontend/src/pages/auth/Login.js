import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../utils/AuthContext";
import { TailSpin } from "react-loader-spinner";

const Login = () => {
    const { login } = useAuth();
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const handleLogin = async (event) => {
        event.preventDefault();
        try {
            setLoading(true);
            await login(email, password); // Call login from context
            console.log("Login successful!");
            setLoading(false);
            navigate("/home"); // Navigate after successful login
        } catch (error) {
            setError("Login failed. Please check your email and password.");
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Login</h2>
            {error && <p style={{ color: "red", marginTop: "10px" }}>{error}</p>}
            <form onSubmit={handleLogin}>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        onChange={(e) => setEmail(e.target.value)}
                        value={email}
                        required
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                        type="password"
                        onChange={(e) => setPassword(e.target.value)}
                        value={password}
                        required
                    />
                </div>
                <button type="submit" disabled={loading}>
                {loading ? "Loading..." : "Login"}
                </button>
                {loading && <TailSpin color="#333" height={30} width={30} />}
            </form>
        </div>
    );
};

export default Login;
