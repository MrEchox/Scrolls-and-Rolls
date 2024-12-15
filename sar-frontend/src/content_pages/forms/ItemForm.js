import React, { useState } from "react";

import { createItem } from "../../utils/ApiService";
import { useAuth } from "../../utils/AuthContext";

const ItemForm = ({ onSuccess, sessionId }) => {
    const [loading, setLoading] = useState(false);
    const { userToken } = useAuth();

    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [modifiedDexterity, setModifiedDexterity] = useState(0);
    const [modifiedStrength, setModifiedStrength] = useState(0);
    const [modifiedConstitution, setModifiedConstitution] = useState(0);
    const [modifiedIntelligence, setModifiedIntelligence] = useState(0);
    const [modifiedWisdom, setModifiedWisdom] = useState(0);
    const [modifiedCharisma, setModifiedCharisma] = useState(0);

    const addItem = async (newItem) => {
            try {
                setLoading(true);
                
                const itemData = {
                    name : newItem.name,
                    description : newItem.description,
                    modifiedDexterity : newItem.modifiedDexterity,
                    modifiedStrength : newItem.modifiedStrength,
                    modifiedConstitution : newItem.modifiedConstitution,
                    modifiedIntelligence : newItem.modifiedIntelligence,
                    modifiedWisdom : newItem.modifiedWisdom,
                    modifiedCharisma : newItem.modifiedCharisma,
                };

                console.log("Adding item:", newItem);
                await createItem(userToken, sessionId, itemData);
                onSuccess();
            } catch (error) {
                console.error("Failed to add item:", error);
            } finally {
                setLoading(false);
            }
        };

    const handleSubmit = (e) => {
        e.preventDefault();

        const newItem = {
            name,
            description,
            modifiedDexterity,
            modifiedStrength,
            modifiedConstitution,
            modifiedIntelligence,
            modifiedWisdom,
            modifiedCharisma,
        };

        addItem(newItem);
    };

    return (
        <div>
            <h4>Add item</h4>
            {loading ? (
                <p>Adding item...</p>
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
                        <label>Description:</label>
                        <input
                        type="text"
                        value={description}
                        minLength={3}
                        maxLength={500}
                        onChange={(e) => setDescription(e.target.value)}
                        required
                        />
                    </div>
                    <div>
                        <label>Modified Dexterity:</label>
                        <input
                        type="number"
                        value={modifiedDexterity}
                        onChange={(e) => setModifiedDexterity(Number(e.target.value))}
                        min="0"
                        max="10"
                        />
                    </div>
                    <div>
                        <label>Modified Strength:</label>
                        <input
                        type="number"
                        value={modifiedStrength}
                        onChange={(e) => setModifiedStrength(Number(e.target.value))}
                        min="0"
                        max="10"
                        />
                    </div>
                    <div>
                        <label>Modified Constitution:</label>
                        <input
                        type="number"
                        value={modifiedConstitution}
                        onChange={(e) => setModifiedConstitution(Number(e.target.value))}
                        min="0"
                        max="10"
                        />
                    </div>
                    <div>
                        <label>Modified Intelligence:</label>
                        <input
                        type="number"
                        value={modifiedIntelligence}
                        onChange={(e) => setModifiedIntelligence(Number(e.target.value))}
                        min="0"
                        max="10"
                        />
                    </div>
                    <div>
                        <label>Modified Wisdom:</label>
                        <input
                        type="number"
                        value={modifiedWisdom}
                        onChange={(e) => setModifiedWisdom(Number(e.target.value))}
                        min="0"
                        max="10"
                        />
                    </div>
                    <div>
                        <label>Modified Charisma:</label>
                        <input
                        type="number"
                        value={modifiedCharisma}
                        onChange={(e) => setModifiedCharisma(Number(e.target.value))}
                        min="0"
                        max="10"
                        />
                    </div>
                    <button type="submit">Add item</button>
                </form>
            )}
        </div>
    );
};

export default ItemForm;
