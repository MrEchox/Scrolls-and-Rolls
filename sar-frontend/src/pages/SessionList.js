// SessionList.js
import React from 'react';
import './styles/Auth.css';

const SessionList = ({ sessions }) => {
    return (
        <>
            {sessions.length > 0 ? (
                sessions.map((session) => (
                    <div key={session.id} className="session-item">
                        <span>{session.name}</span>
                        <button onClick={() => joinSession(session.id)}>Join</button>
                    </div>
                ))
            ) : (
                <p>No available sessions at the moment.</p>
            )}
        </>
    );
};

const joinSession = (sessionId) => {
    // Implement join session logic here, possibly redirecting to the session page
    console.log(`Joining session ${sessionId}`);
};

export default SessionList;
