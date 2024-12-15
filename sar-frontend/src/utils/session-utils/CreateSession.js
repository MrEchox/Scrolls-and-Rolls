import React, { useState } from "react";
import { useAuth } from "../AuthContext";
import { createSession } from "../ApiService";

const CreateSession = ({ onSuccess }) => {
    const [sessionName, setSessionName] = useState("");
    const [resultMessage, setResultMessage] = useState("");
    const [loading, setLoading] = useState(false);
    const [sessionCreated, setSessionCreated] = useState(false); // New state to track session creation
    const { userId, userToken } = useAuth();

    const handleCreateSession = async (event) => {
        event.preventDefault();
        setLoading(true);
        setResultMessage("");

        try {
            await createSession(userToken, userId, sessionName);
            setLoading(false);
            setSessionCreated(true); // Indicate success
            onSuccess(); // Call parent component's onSuccess function
        } catch (error) {
            setLoading(false);
            console.log(error);
            setResultMessage("Failed to create session. Please try again.");
        }
    };

    // Success message component
    const SuccessMessage = () => (
        <div>
            <h3>Session "{sessionName}" created successfully!</h3>
            <button onClick={() => setSessionCreated(false)}>Create another session</button>
        </div>
    );

    return (
        <div>
            <h2>Create Session</h2>
            {sessionCreated ? (
                <SuccessMessage /> // Show success message if session was created
            ) : (
                <form onSubmit={handleCreateSession}>
                    {resultMessage && (
                        <p style={{ color: "red", marginTop: "10px" }}>{resultMessage}</p>
                    )}
                    <div>
                        <label>Session Name:</label>
                        <input
                            type="text"
                            value={sessionName}
                            onChange={(e) => setSessionName(e.target.value)}
                            minLength={3}
                            maxLength={20}
                            required
                        />
                    </div>
                    <button type="submit" disabled={loading}>
                        {loading ? "Creating..." : "Create Session"}
                    </button>
                </form>
            )}
        </div>
    );
};

export default CreateSession;
