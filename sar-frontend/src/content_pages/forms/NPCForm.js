import React, { useState } from "react";

import { createCharacter } from "../../utils/ApiService";
import { useAuth } from "../../utils/AuthContext";

const NPCForm = ({ onSuccess, sessionId }) => {
    const [loading, setLoading] = useState(false);
    const { userToken } = useAuth();

    const [name, setName] = useState("");
    const [biography, setBiography] = useState("");
    const [dexterity, setDexterity] = useState(1);
    const [strength, setStrength] = useState(1);
    const [constitution, setConstitution] = useState(1);
    const [intelligence, setIntelligence] = useState(1);
    const [wisdom, setWisdom] = useState(1);
    const [charisma, setCharisma] = useState(1);

    const addNPC = async (newNPC) => {
            try {
                setLoading(true);

                const characterData = {
                    name: newNPC.name,
                    biography: newNPC.biography,
                    isNpc: true,
                    dexterity: newNPC.dexterity,
                    strength: newNPC.strength,
                    constitution: newNPC.constitution,
                    intelligence: newNPC.intelligence,
                    wisdom: newNPC.wisdom,
                    charisma: newNPC.charisma,
                };

                console.log("Adding NPC:", newNPC);
                await createCharacter(userToken, sessionId, characterData);
                onSuccess();
            } catch (error) {
                console.error("Failed to add NPC:", error);
            } finally {
                setLoading(false);
            }
    };

    const handleSubmit = (e) => {
        e.preventDefault(); // Prevent the default form submission behavior

        const newNPC = {
            name,
            biography,
            dexterity,
            strength,
            constitution,
            intelligence,
            wisdom,
            charisma,
        };

        addNPC(newNPC);
    };

    return (
        <div>
            <h4>Add NPC</h4>
            {loading ? (
                <p>Adding NPC...</p>
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
                        minLength={3}
                        maxLength={500}
                        onChange={(e) => setBiography(e.target.value)}
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
                    <button type="submit">Add NPC</button>
                </form>
            )}
        </div>
    );
};

export default NPCForm;
