// SessionTable.js
import React from "react";
import Modal from "react-modal";
import { useNavigate } from "react-router-dom";

import { useAuth } from "../../utils/AuthContext";
import { deleteSession } from "../../utils/ApiService";

import GameSession from "../../pages/session/GameSession";
import NPCTable from "./NPCTable";
import ItemTable from "./ItemTable";

const SessionTable = ({ sessions, onRemoveSession }) => {
    const navigate = useNavigate();

    const { userToken } = useAuth();

    const [showModal, setShowModal] = React.useState(false);
    const [modalContent, setModalContent] = React.useState(null);

    const handleRemoveSession = async (sessionId) => {
        try {
            await deleteSession(userToken, sessionId);
            onRemoveSession();
        } catch (error) {
            console.error("Failed to delete session:", error);
        }
    };

    const handleJoinSession = (sessionId) => {
        navigate(`/session/game/${sessionId}`);
    };

    const openModal = (content) => {
        setModalContent(content);
        setShowModal(true);
    };

    const closeModal = () => {
        setShowModal(false);
        setModalContent(null);
    }

    return (
        <div>
            <h2>Your Game Sessions</h2>
            {sessions.length === 0 ? (
                <p>You do not have any sessions.</p>
            ) : (
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
                                    <button onClick={() => handleJoinSession(session.sessionId)}>
                                        Join
                                    </button>
                                </td>
                                <td>
                                    <button onClick={() => openModal(<NPCTable sessionId={session.sessionId} sessionName={session.sessionName} />)}>
                                        Edit NPC List
                                    </button>
                                </td>
                                <td>
                                    <button onClick={() => openModal(<ItemTable sessionId={session.sessionId} sessionName={session.sessionName}/>)}>
                                        Edit Item List
                                    </button>
                                </td>
                                <td>
                                    <button onClick={() => handleRemoveSession(session.sessionId)}>
                                        Remove
                                    </button>
                                </td>
                                <td>
                                    <p>Session ID: {session.sessionId}</p>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
            <Modal
                isOpen={showModal}
                onRequestClose={closeModal}
                contentLabel="Edit Modal"
                ariaHideApp={false} // Set to true for accessibility in production
            >
                <button onClick={closeModal} style={{ float: "right" }}>Close</button>
                {modalContent}
            </Modal>
        </div>
    );
};

export default SessionTable;
