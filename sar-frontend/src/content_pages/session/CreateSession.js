import React from "react";
import { useLocation } from "react-router-dom";

const CreateSession = () => {
    const location = useLocation();
    const sessionName = location.state?.session_name;

    return (
        <div>
            <h2>Creating Session</h2>
            <p>Session Name: {sessionName}</p>
            {/* Add more logic for creating the session */}
        </div>
    );
};

export default CreateSession;
