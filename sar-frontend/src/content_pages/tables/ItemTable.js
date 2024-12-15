import React, { useState, useEffect } from "react";
import Modal from "react-modal";
import { TailSpin } from "react-loader-spinner";
import ItemForm from "../forms/ItemForm";

import {getSessionItems, deleteItem} from "../../utils/ApiService";
import { useAuth } from "../../utils/AuthContext";

const ItemTable = ({ sessionId, sessionName }) => {
    const {userToken} = useAuth();
        const [items, setItems] = useState([]);
        const [loading, setLoading] = useState(true);
        const [showModal, setShowModal] = React.useState(false);
        const [selectedItemId, setSelectedItemId] = useState(null);


    const fetchItems = async () => {
            if (!sessionId) {
                console.error("Session ID is required to fetch items.");
                return;
            }
            try {
                console.log("Fetching items for session ID:", sessionId);
                setLoading(true);
                const items = await getSessionItems(userToken, sessionId);
                setItems(items);
            } catch (error) {
                console.error("Failed to fetch items:", error);
            } finally {
                setLoading(false);
            }
        };

    useEffect(() => {
            fetchItems();
        }, [sessionId]);

    const removeItem = async (itemId) => {
            try {
                setLoading(true);
                await deleteItem(userToken, sessionId, itemId);
                fetchItems();
            } catch (error) {
                console.error("Failed to delete item:", error);
            } finally {
                setLoading(false);
                setShowModal(false);
            }
        };
    
    const handleRemoveClick = (itemId) => {
        setSelectedItemId(itemId);  
        setShowModal(true);
    };

    return (
        <div>
            <h3>Items for session: {sessionName}</h3>
            {loading ? (
                <TailSpin color="#333" height={30} width={30} />
            ) : (
                <>
                    <ItemForm onSuccess={fetchItems} sessionId={sessionId}/>
                    <p>Current items:</p>
                    <table>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Modified Stats</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            {items.map((item) => (
                                <tr key={item.itemId}>
                                    <td>{item.name}</td>
                                    <td>{item.description}</td>
                                    <td>
                                        {`Dex: +${item.modifiedDexterity}, Str: +${item.modifiedStrength}, Con: +${item.modifiedConstitution}, Int: +${item.modifiedIntelligence}, Wis: +${item.modifiedWisdom}, Cha: +${item.modifiedCharisma}`}
                                    </td>
                                    <td>
                                        <button onClick={() => handleRemoveClick(item.itemId)}>Remove</button>
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
                contentLabel="Item Modal"
            >
                <h3>Are you sure you wish to delete this item?</h3>
                <button onClick={() => removeItem(selectedItemId)}>Yes</button>
                <button onClick={() => setShowModal(false)}>No</button>
            </Modal>
        </div>
    );
};

export default ItemTable;
