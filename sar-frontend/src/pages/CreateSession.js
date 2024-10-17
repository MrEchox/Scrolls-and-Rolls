import React, { useState } from 'react';
import './styles/Auth.css';

const CreateSession = () => {
    const [sessionName, setSessionName] = useState('');
    const [description, setDescription] = useState('');
    const [npcs, setNpcs] = useState([]);
    const [items, setItems] = useState([]);
    const [modalData, setModalData] = useState({ type: '', index: null, name: '', description: '', effect: '' });
    const [showModal, setShowModal] = useState(false);

    const openModal = (type, index = null) => {
        setModalData({ type, index, name: '', description: '', effect: '' });
        if (type === 'npc') {
            if (index !== null) {
                const npc = npcs[index];
                setModalData({ ...modalData, name: npc.name, description: npc.description });
            }
        } else if (type === 'item') {
            if (index !== null) {
                const item = items[index];
                setModalData({ ...modalData, name: item.name, effect: item.effect });
            }
        }
        setShowModal(true);
    };

    const handleModalChange = (e) => {
        const { name, value } = e.target;
        setModalData({ ...modalData, [name]: value });
    };

    const handleSave = () => {
        if (modalData.type === 'npc') {
            if (modalData.index !== null) {
                const newNpcs = [...npcs];
                newNpcs[modalData.index] = { name: modalData.name, description: modalData.description };
                setNpcs(newNpcs);
            } else {
                setNpcs([...npcs, { name: modalData.name, description: modalData.description }]);
            }
        } else if (modalData.type === 'item') {
            if (modalData.index !== null) {
                const newItems = [...items];
                newItems[modalData.index] = { name: modalData.name, effect: modalData.effect };
                setItems(newItems);
            } else {
                setItems([...items, { name: modalData.name, effect: modalData.effect }]);
            }
        }
        setShowModal(false);
    };

    const handleDelete = () => {
        if (modalData.type === 'npc' && modalData.index !== null) {
            const newNpcs = npcs.filter((_, index) => index !== modalData.index);
            setNpcs(newNpcs); // Update the NPCs state
        } else if (modalData.type === 'item' && modalData.index !== null) {
            const newItems = items.filter((_, index) => index !== modalData.index);
            setItems(newItems); // Update the items state
        }
        setShowModal(false); // Close the modal after deletion
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        // Implement session creation logic here
        console.log('Creating session:', { sessionName, description, npcs, items });
    };

    return (
        <div>
            <form className="create-session" onSubmit={handleSubmit}>
                <h2>Create New Session</h2>
                <input
                    type="text"
                    placeholder="Session Name"
                    value={sessionName}
                    onChange={(e) => setSessionName(e.target.value)}
                    required
                />
                <textarea
                    placeholder="Session Description"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    required
                />

                <h3>NPCs</h3>
                {npcs.map((npc, index) => (
                    <div key={index} style={{ display: 'flex', alignItems: 'center', marginBottom: '8px' }}>
                        <span style={{ flexGrow: 1 }}>{npc.name}</span>
                        <button type="button" onClick={() => openModal('npc', index)} className="edit-button">Edit</button>
                    </div>
                ))}
                <button type="button" onClick={() => openModal('npc')}>Add NPC</button>

                <h3>Items</h3>
                {items.map((item, index) => (
                    <div key={index} style={{ display: 'flex', alignItems: 'center', marginBottom: '8px' }}>
                        <span style={{ flexGrow: 1 }}>{item.name}</span>
                        <button type="button" onClick={() => openModal('item', index)} className="edit-button">Edit</button>
                    </div>
                ))}
                <button type="button" onClick={() => openModal('item')}>Add Item</button>

                <button type="submit">Create Session</button>
            </form>

            {showModal && (
                <div className="modal">
                    <div className="modal-content">
                        <h3>{modalData.type === 'npc' ? 'NPC' : 'Item'} Details</h3>
                        {modalData.type === 'npc' && (
                            <>
                                <input
                                    type="text"
                                    name="name"
                                    placeholder="NPC Name"
                                    value={modalData.name}
                                    onChange={handleModalChange}
                                    required
                                />
                                <input
                                    type="text"
                                    name="description"
                                    placeholder="NPC Description"
                                    value={modalData.description}
                                    onChange={handleModalChange}
                                    required
                                />
                            </>
                        )}
                        {modalData.type === 'item' && (
                            <>
                                <input
                                    type="text"
                                    name="name"
                                    placeholder="Item Name"
                                    value={modalData.name}
                                    onChange={handleModalChange}
                                    required
                                />
                                <input
                                    type="text"
                                    name="effect"
                                    placeholder="Item Effect"
                                    value={modalData.effect}
                                    onChange={handleModalChange}
                                    required
                                />
                            </>
                        )}
                        <button onClick={handleSave}>Save</button>
                        {modalData.index !== null && <button onClick={handleDelete}>Delete</button>}
                        <button onClick={() => setShowModal(false)}>Cancel</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default CreateSession;
