import axios from 'axios';

const API_BASE_URL = "https://scrolls-and-rolls-ecedevcmeudga4h7.polandcentral-01.azurewebsites.net";

// Authorization api calls -----------------------------------------------------
export const login = async (email, password) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/api/auth/login`, {
            email : email,
            password : password
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const register = async (email, password, role, username) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/api/auth/register`, {
            email : email,
            password : password,
            role : role,
            username: username
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}

export const refreshToken = async (refreshToken, userId) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/api/auth/refresh`, {
            refreshTokenId: refreshToken,
            userId: userId,
        });
        return response.data; // { token, refreshToken }
    } catch (error) {
        console.error("Failed to refresh token:", error);
        throw error;
    }
};
// ------------------------------------------------------------------------------

// Session api calls -----------------------------------------------------------
export const createSession = async (token, gameMasterId, sessionName) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/api/sessions`,
            {
                gameMasterId: gameMasterId,
                sessionName: sessionName,
            },
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const deleteSession = async (token, sessionId) => {
    try {
        const response = await axios.delete(`${API_BASE_URL}/api/sessions/${sessionId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
            sessionId : sessionId,
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const getSessionsForGameMaster = async (token) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch sessions:", error);
        throw error;
    }
};

export const getSession = async (token, sessionId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch session:", error);
        throw error;
    }
}

export const updateSession = async (token, sessionId, sessionData) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/api/sessions/${sessionId}`,
            sessionData,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        throw error;
    }
};
// ------------------------------------------------------------------------------

// Item api calls --------------------------------------------------------------
export const getSessionItems = async (token, sessionId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}/items`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch items:", error);
        throw error;
    }
};

export const getSessionItem = async (token, sessionId, itemId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}/items/${itemId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch item:", error);
        throw error;
    }
};

export const createItem = async (token, sessionId, itemData) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/api/sessions/${sessionId}/items`, itemData, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const updateItem = async (token, sessionId, itemId, itemData) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/api/sessions/${sessionId}/items/${itemId}`,
            itemData,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const deleteItem = async (token, sessionId, itemId) => {
    try {
        const response = await axios.delete(`${API_BASE_URL}/api/sessions/${sessionId}/items/${itemId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const assignItemToCharacter = async (token, sessionId, itemId, characterId) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/api/sessions/${sessionId}/items/${itemId}/assign/${characterId}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        throw error;
    }
}

// Items for characters --------------------------------------------------------
export const getItemsForCharacter = async (token, sessionId, characterId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}/characters/${characterId}/items`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch items:", error);
        throw error;
    }
};

export const reassignItemToCharacter = async (token, sessionId, itemId, characterId, newCharacterId) => {
    try {
        const response = await axios.put(`${API_BASE_URL}/api/sessions/${sessionId}/characters/${characterId}/items/${itemId}/assign/${newCharacterId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
            }
        );
        return response.data;
    } catch (error) {
        throw error;
    }
};
// ------------------------------------------------------------------------------

// Character api calls ---------------------------------------------------------
export const getSessionNPCs = async (token, sessionId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}/npcs`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch NPCs:", error);
        throw error;
    }
};

export const getAllCharacters = async (token, sessionId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}/characters`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch characters:", error);
        throw error;
    }
};

export const getSessionsByUserId = async (token, userId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/by-user/${userId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch sessions by user ID:", error);
        throw error;
    }
};

export const getSessionPCs = async (token, sessionId) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/sessions/${sessionId}/pcs`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch PCs:", error);
        throw error;
    }
}

export const createCharacter = async (token, sessionId, characterData) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/api/sessions/${sessionId}/characters`, characterData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const deleteCharacter = async (token, sessionId, characterId) => {
    try {
        const response = await axios.delete(`${API_BASE_URL}/api/sessions/${sessionId}/characters/${characterId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error("Failed to delete character:", error);
        throw error;
    }
};

// Chat API calls
export const sendMessage = async (token, sessionId, userId, messageText, channel) => {
    try {
        // Create message object
        const messageData = {
            userId: userId,
            messageContent: messageText,
            sessionId: sessionId,
            channel: channel,
        };
        console.log("Message data:", messageData);

        // Send the message to the API for storing it in the database
        const response = await axios.post(
            `${API_BASE_URL}/api/sessions/${sessionId}/messages`, 
            messageData,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );

        return response.data;
    } catch (error) {
        console.error("Error sending message:", error);
        throw error;
    }
};

export const getMessages = async (token, sessionId, channel) => {
    try {
        const response = await axios.get(
            `${API_BASE_URL}/api/sessions/${sessionId}/messages`,
            {
                params: {
                    channel: channel,
                },
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Error fetching messages:", error);
        throw error;
    }
};