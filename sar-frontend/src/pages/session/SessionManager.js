import React, { useEffect, useState } from "react";

import SessionTable from "../../content_pages/tables/SessionTable";
import CreateSession from "../../utils/session-utils/CreateSession";

import { getSessionsForGameMaster } from "../../utils/ApiService";
import { useAuth } from "../../utils/AuthContext";

const SessionManager = () => {
    const { userToken } = useAuth();

    const [sessions, setSessions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [view, setView] = useState("table"); // "table", "create", "edit"

    useEffect(() => {
        setLoading(true);
        fetchSessions();
        setLoading(false);
    }, []);

    const fetchSessions = async () => {
        try {
            setLoading(true);
            const sessionsForGameMaster = await getSessionsForGameMaster(userToken);
            setSessions(sessionsForGameMaster);
        } catch (error) {
            console.error("Failed to fetch sessions:", error);
        } finally {
            setLoading(false);
        }
    }

    return (
        <div>
            {view === "table" && (
                <div>
                    <button onClick={() => setView("create")}>Create New Session</button>
                    <br />
                    <br />
                    {loading ? (
                        <p>Loading sessions...</p>
                    ) : (
                        <SessionTable sessions={sessions} onRemoveSession={fetchSessions} />
                    )}
                </div>
            )}
            {view === "create" && (
                <div>
                    <button onClick={() => setView("table")}>Back to Sessions</button>
                    <CreateSession onSuccess={fetchSessions} />
                </div>
            )}
        </div>
    );
};

export default SessionManager;
