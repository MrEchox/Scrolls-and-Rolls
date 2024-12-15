import React, { useState } from "react";
import { createCharacter } from "../../utils/ApiService";  // Adjust the path as needed
import { useAuth } from "../../utils/AuthContext";  // For user token and id
import { useNavigate } from "react-router-dom";
import { useParams } from "react-router-dom";  // For getting sessionId from URL

const PCForm = () => {
    const { userToken, userId } = useAuth();  // Get userToken and userId from context
    const { sessionId } = useParams();  // Get sessionId from URL
    const navigate = useNavigate();  // For navigation

    // Define state variables for character attributes
    const [name, setName] = useState("");
    const [biography, setBiography] = useState("");
    const [dexterity, setDexterity] = useState(1);
    const [strength, setStrength] = useState(1);
    const [constitution, setConstitution] = useState(1);
    const [intelligence, setIntelligence] = useState(1);
    const [wisdom, setWisdom] = useState(1);
    const [charisma, setCharisma] = useState(1);
    const [loading, setLoading] = useState(false);  // Track loading state

    // Function to add character to session
    const addPC = async (newPC) => {
        try {
            setLoading(true);

            const characterData = {
                name: newPC.name,
                biography: newPC.biography,
                isNpc: false,  // Mark as Player Character
                dexterity: newPC.dexterity,
                strength: newPC.strength,
                constitution: newPC.constitution,
                intelligence: newPC.intelligence,
                wisdom: newPC.wisdom,
                charisma: newPC.charisma,
                userId: userId,  // Associate with current user
            };

            console.log("Adding Player Character:", newPC);
            await createCharacter(userToken, sessionId, characterData);  // Call API to add character
            navigate(`/home`);  // Redirect to game session
        } catch (error) {
            console.error("Failed to add Player Character:", error);
        } finally {
            setLoading(false);  // Reset loading state
        }
    };

    // Handle form submission
    const handleSubmit = (e) => {
        e.preventDefault();  // Prevent page refresh on form submit

        const newPC = {
            name,
            biography,
            dexterity,
            strength,
            constitution,
            intelligence,
            wisdom,
            charisma,
        };

        addPC(newPC);  // Call function to add the new character
    };

    return (
        <div>
            <h4>Create Player Character</h4>
            {loading ? (
                <p>Creating character...</p>
            ) : (
                <form onSubmit={handleSubmit}>
                    <div>
                        <label>Name:</label>
                        <input
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            minLength={3}
                            maxLength={20}
                            required
                        />
                    </div>
                    <div>
                        <label>Biography:</label>
                        <input
                            type="text"
                            value={biography}
                            onChange={(e) => setBiography(e.target.value)}
                            minLength={3}
                            maxLength={500}
                            required
                        />
                    </div>
                    <div>
                        <label>Dexterity:</label>
                        <input
                            type="number"
                            value={dexterity}
                            onChange={(e) => setDexterity(Number(e.target.value))}
                            min="1"
                            max="20"
                        />
                    </div>
                    <div>
                        <label>Strength:</label>
                        <input
                            type="number"
                            value={strength}
                            onChange={(e) => setStrength(Number(e.target.value))}
                            min="1"
                            max="20"
                        />
                    </div>
                    <div>
                        <label>Constitution:</label>
                        <input
                            type="number"
                            value={constitution}
                            onChange={(e) => setConstitution(Number(e.target.value))}
                            min="1"
                            max="20"
                        />
                    </div>
                    <div>
                        <label>Intelligence:</label>
                        <input
                            type="number"
                            value={intelligence}
                            onChange={(e) => setIntelligence(Number(e.target.value))}
                            min="1"
                            max="20"
                        />
                    </div>
                    <div>
                        <label>Wisdom:</label>
                        <input
                            type="number"
                            value={wisdom}
                            onChange={(e) => setWisdom(Number(e.target.value))}
                            min="1"
                            max="20"
                        />
                    </div>
                    <div>
                        <label>Charisma:</label>
                        <input
                            type="number"
                            value={charisma}
                            onChange={(e) => setCharisma(Number(e.target.value))}
                            min="1"
                            max="20"
                        />
                    </div>
                    <button type="submit">Create Character</button>
                </form>
            )}
        </div>
    );
};

export default PCForm;
