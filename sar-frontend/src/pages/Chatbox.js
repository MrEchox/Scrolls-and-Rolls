// Chatbox.js
import React, { useState } from 'react';
import './styles/Auth.css';

const Chatbox = () => {
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');

    const handleSend = (e) => {
        e.preventDefault();
        if (input.trim()) {
            setMessages([...messages, input]);
            setInput('');
            // Add additional logic to send the message to the server if needed
        }
    };

    return (
        <div className="chatbox">
            <div className="chat-messages">
                {messages.map((msg, index) => (
                    <div key={index} className="chat-message">{msg}</div>
                ))}
            </div>
            <form onSubmit={handleSend}>
                <input
                    type="text"
                    value={input}
                    onChange={(e) => setInput(e.target.value)}
                    placeholder="Type your message..."
                    required
                />
                <button type="submit">Send</button>
            </form>
        </div>
    );
};

export default Chatbox;
