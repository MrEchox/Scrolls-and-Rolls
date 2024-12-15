import React, { useState, useEffect } from "react";
import Modal from "react-modal";
import { TailSpin } from "react-loader-spinner";
import NPCForm from "../forms/NPCForm";

import {getSessionNPCs, deleteCharacter} from "../../utils/ApiService";
import { useAuth } from "../../utils/AuthContext";

const NPCTable = ({ sessionId, sessionName }) => {
    const {userToken} = useAuth();
    const [NPCs, setNPCs] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = React.useState(false);
    const [selectedCharacterId, setSelectedCharacterId] = useState(null);

    const fetchNPCs = async () => {
        if (!sessionId) {
            console.error("Session ID is required to fetch NPCs.");
            return;
        }
        try {
            console.log("Fetching NPCs for session ID:", sessionId);
            setLoading(true);
            const NPCs = await getSessionNPCs(userToken, sessionId);
            setNPCs(NPCs);
        } catch (error) {
            console.error("Failed to fetch NPCs:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchNPCs();
    }, [sessionId]);

    const removeNPC = async (characterId) => {
        try {
            setLoading(true);
            await deleteCharacter(userToken, sessionId, characterId);
            fetchNPCs();
        } catch (error) {
            console.error("Failed to delete NPC:", error);
        } finally {
            setLoading(false);
            setShowModal(false);
        }
    };

    const handleRemoveClick = (characterId) => {
        setSelectedCharacterId(characterId); 
        setShowModal(true); 
    };


    return (
        <div>
            <h3>NPCs for session: {sessionName}</h3>
            {loading ? (
                <TailSpin color="#333" height={30} width={30} />
            ) : (
                <>
                    <NPCForm onSuccess={fetchNPCs} sessionId={sessionId}/>
                    <p>Current NPCs:</p>
                    <table>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Biography</th>
                                <th>Stats</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {NPCs.map((NPC) => (
                                <tr key={NPC.characterId}>
                                    <td>{NPC.name}</td>
                                    <td>{NPC.biography}</td>
                                    <td>
                                        {`Dex: ${NPC.dexterity}, Str: ${NPC.strength}, Con: ${NPC.constitution}, Int: ${NPC.intelligence}, Wis: ${NPC.wisdom}, Cha: ${NPC.charisma}`}
                                    </td>
                                    <td>
                                        <button onClick={() => handleRemoveClick(NPC.characterId)}>Remove</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </>
            )}
            <Modal
                isOpen={showModal}
                onRequestClose={() => setShowModal(false)}
                contentLabel="NPC Modal"
            >
                <h3>Are you sure you wish to delete this NPC?</h3>
                <button onClick={() => removeNPC(selectedCharacterId)}>Yes</button>
                <button onClick={() => setShowModal(false)}>No</button>
            </Modal>
        </div>
    );
};

export default NPCTable;
