import React, { useState, useEffect, useRef } from "react";
import { sendMessage, getMessages } from '../../utils/ApiService';
import { useAuth } from '../../utils/AuthContext';
import "./Chat.css";

const PlayerChat = ({ sessionId }) => {
    const chatEndRef = useRef(null);
    const { userId, userToken, userRole } = useAuth();
    const [messages, setMessages] = useState([]);
    const [inputValue, setInputValue] = useState("");

    useEffect(() => {
        const fetchMessagesFromApi = async () => {
            try {
            const messagesData = await getMessages(userToken, sessionId, "Chat");
            setMessages(messagesData.map(msg => ({
                characterName: msg.characterName,
                messageContent: msg.messageContent,
                timestamp: msg.timeStamp
            })).sort((a, b) => new Date(b.timestamp) - new Date(a.timestamp)));
            } catch (err) {
            console.error('Error fetching messages:', err);
            }
        };
        fetchMessagesFromApi(); // Fetch messages initially
        const intervalId = setInterval(fetchMessagesFromApi, 5000);
        return () => clearInterval(intervalId); // Clear the interval when the component unmounts
    }, [sessionId, userToken]);

    const handleSendMessage = async () => {
        if (inputValue.trim() !== "") {
            try {
                await sendMessage(userToken, sessionId, userId, inputValue, "Chat");
                setInputValue(""); 
            } catch (err) {
                console.error("Error sending message:", err);
            }
        }
    };

    return (
        <div className="player-chat-box">
            <h2>Player Chat</h2>
            <div className="chat-messages">
                {messages.map((msg, index) => (
                    <div key={index} className="player-message">
                        {msg.timestamp && <span className="timestamp">{new Date(msg.timestamp).toLocaleTimeString()}</span>}
                        <span style={{ marginLeft: '10px' }}><strong>{msg.characterName}: </strong></span>
                        {msg.messageContent}
                    </div>
                ))}
            </div>
            {userRole === "Player" && (
                <div className="input-container">
                    <input
                        type="text"
                        placeholder="Type a message..."
                        value={inputValue}
                        onChange={(e) => setInputValue(e.target.value)}
                        onKeyDown={(e) => {
                            if (e.key === "Enter") {
                                handleSendMessage();
                            }
                        }}
                    />
                    <button onClick={handleSendMessage}>Send</button>
                </div>
            )}
        </div>
    );
};

export default PlayerChat;
