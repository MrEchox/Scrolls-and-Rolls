import React, { useState, useEffect } from 'react';
import './styles/Auth.css';
import SessionList from './SessionList';
import CreateSession from './CreateSession';
import Chatbox from './Chatbox';

const Home = ({ user }) => {
    const [sessions, setSessions] = useState([]);
    const [creatingSession, setCreatingSession] = useState(false);

    useEffect(() => {
        const fetchSessions = async () => {
            const response = await fetch('/api/sessions');
            const data = await response.json();
            setSessions(data);
        };
        fetchSessions();
    }, []);

    const handleCreateSession = () => {
        setCreatingSession(true);
    };

    return (
        <div className="home-page">
            <h1>Welcome, {user.account_type} '{user.name}'!</h1>
            {user.account_type === 'Game Master' ? ( // Game  master UI
                <div>
                    <button onClick={handleCreateSession}>Create New Session</button>
                    {creatingSession && <CreateSession />}
                </div>
            ) : (
                <div className="session-list">
                    <h2>Available Game Sessions</h2>
                    <SessionList sessions={sessions} />
                </div>
            )}
        </div>
    );
};

export default Home;
