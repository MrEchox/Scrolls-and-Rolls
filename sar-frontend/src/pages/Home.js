import React, { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../utils/AuthContext";
import { getSessionsByUserId, getSession } from "../utils/ApiService"; // Function to get player's sessions

const Home = () => {
    const { userName, userRole, userId, userToken } = useAuth();
    const navigate = useNavigate();  // For navigation
    const [sessions, setSessions] = useState([]);
    const [sessionIdToJoin, setSessionIdToJoin] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        if (userRole === "Player") {
            const fetchSessions = async () => {
                try {
                    const playerSessions = await getSessionsByUserId(userToken, userId); // Fetch sessions the player has joined
                    setSessions(playerSessions);
                } catch (err) {
                    console.error("Error fetching player sessions:", err);
                }
            };
            fetchSessions();
        }
    }, [userToken, userId, userRole]);

    const handleJoinSession = async () => {
        if (sessionIdToJoin.trim() === "") {
            setErrorMessage("Session ID cannot be empty.");
            return;
        }

        // Use getSession to validate if the session ID is valid
        try {
            const sessionData = await getSession(userToken, sessionIdToJoin);
            
            // If the session is found, navigate to character creation
            if (sessionData) {
                navigate(`/session/create-character/${sessionIdToJoin}`);
            }
        } catch (error) {
            // If the session doesn't exist or any error occurs
            setErrorMessage("Invalid Session ID.");
        }
    };

    return (
        <div>
            <div>
                <h1>Home</h1>
                <p>Welcome to Scrolls and Rolls, {userName}!</p>
            </div>
            <div>
                {userRole === "GameMaster" && (
                    <NavLink className={"content-navlink"} to="/session/session-manager">Sessions menu</NavLink>
                )}
                {userRole === "Player" && (
                    <>
                        <div>
                            <h3>Join a New Session</h3>
                            <input
                                type="text"
                                placeholder="Enter Session ID"
                                value={sessionIdToJoin}
                                onChange={(e) => setSessionIdToJoin(e.target.value)}
                            />
                            <button onClick={handleJoinSession}>Join Session</button>
                            {errorMessage && <p style={{ color: "red" }}>{errorMessage}</p>}
                        </div>
                        <h3>Your Active Sessions</h3>
                        <table>
                            <thead>
                                <tr>
                                    <th>Session Name</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {sessions.map((session) => (
                                    <tr key={session.sessionId}>
                                        <td>{session.sessionName}</td>
                                        <td>
                                            <NavLink className={"content-navlink"} to={`/session/game/${session.sessionId}`}>Join</NavLink>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </>
                )}
            </div>
        </div>
    );
};

export default Home;
