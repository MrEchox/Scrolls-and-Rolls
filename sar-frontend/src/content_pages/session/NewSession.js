import React, {useState} from "react";
import { useNavigate} from "react-router-dom";

const NewSession = () => {
    const [session_name, setSessionName] = React.useState("");
    const navigate = useNavigate();

    const handleInputChange = (event) => {
        setSessionName(event.target.value);
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        navigate('/session/create-session', { state: { session_name: session_name } });
        // API call here
    }


    return (
        <div>
            <h2>Create a new game session</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Session name:</label>
                    <input
                    type="text"
                    name="session_name"
                    onChange={handleInputChange}
                    value={session_name}
                    required
                    />
                </div>
                <button type='submit'>Continue</button>
            </form>
        </div>
    );
}

export default NewSession;