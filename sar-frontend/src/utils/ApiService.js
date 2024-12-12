import axios from 'axios';

const API_BASE_URL = "https://localhost:7224";

export const login = async (email, password) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/auth/login`, {
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
        const response = await axios.post(`${API_BASE_URL}/auth/register`, {
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