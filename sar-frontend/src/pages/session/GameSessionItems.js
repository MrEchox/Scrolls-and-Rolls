import React, { useState, useEffect } from "react";
import Modal from "react-modal";
import { useAuth } from "../../utils/AuthContext";
import {
  getSessionItems,
  getSessionItem,
  assignItemToCharacter,
  reassignItemToCharacter,
  getAllCharacters,
} from "../../utils/ApiService";

const GameSessionItems = ({ sessionId }) => {
  const { userRole, userToken, userId } = useAuth();

  const [items, setItems] = useState([]);
  const [characters, setCharacters] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [selectedItem, setSelectedItem] = useState(null);
  const [selectedCharacterId, setSelectedCharacterId] = useState("");

  // Fetch all session items
  const fetchItems = async () => {
    try {
      setLoading(true);
      const fetchedItems = await getSessionItems(userToken, sessionId);
      setItems(fetchedItems);
    } catch (error) {
      console.error("Failed to fetch items:", error);
    } finally {
      setLoading(false);
    }
  };

  // Fetch all characters in the session
  const fetchCharacters = async () => {
    try {
      const charactersData = await getAllCharacters(userToken, sessionId);
      setCharacters(charactersData);
    } catch (error) {
      console.error("Failed to fetch characters:", error);
    }
  };

  useEffect(() => {
    fetchItems();
    fetchCharacters();
  }, [sessionId]);

  const openModal = async (itemId) => {
    try {
      const itemDetails = await getSessionItem(userToken, sessionId, itemId);
      setSelectedItem(itemDetails);
      setShowModal(true);
    } catch (error) {
      console.error("Failed to fetch item details:", error);
    }
  };

  const closeModal = () => {
    setSelectedItem(null);
    setSelectedCharacterId("");
    setShowModal(false);
  };

  // Handle assigning/re-assigning item to character
  const handleAssignItem = async () => {
    if (!selectedCharacterId) return;
    try {
      if (selectedItem?.characterId) {
        // Reassign if the item already has an owner
        await reassignItemToCharacter(
          userToken,
          sessionId,
          selectedItem.itemId,
          selectedItem.characterId,
          selectedCharacterId
        );
      } else {
        // Assign to a new owner
        await assignItemToCharacter(userToken, sessionId, selectedItem.itemId, selectedCharacterId);
      }
      closeModal();
      fetchItems();
    } catch (error) {
      console.error("Failed to assign item:", error);
    }
  };

  // If user is a Player, filter to show only items owned by them
  const displayItems = userRole === "GameMaster" ? items : items.filter(item => item.characterId === userId);

  // Get character name by characterId
  const getCharacterName = (characterId) => {
    const character = characters.find((char) => char.characterId === characterId);
    return character ? character.name : "Unassigned";
  };

  return (
    <div>
      <h3>Session Items</h3>
      {loading ? (
        <p>Loading items...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Owner</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {displayItems.length === 0 ? (
              <tr>
                <td colSpan="3">No items in this session.</td>
              </tr>
            ) : (
              displayItems.map((item) => (
                <tr key={item.itemId}>
                  <td>{item.name}</td>
                  <td>{getCharacterName(item.characterId) || "Unassigned"}</td>
                  <td>
                    <button onClick={() => openModal(item.itemId)}>Edit</button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      )}

      <Modal
        isOpen={showModal}
        onRequestClose={closeModal}
        contentLabel="Item Modal"
      >
        {selectedItem && (
          <>
            <h4>Item Details</h4>
            <p>
              <strong>Name:</strong> {selectedItem.name}
            </p>
            <p>
              <strong>Description:</strong> {selectedItem.description}
            </p>
            <p>
              <strong>Modifiers:</strong>
            </p>
            <p>
              <strong>Owner:</strong> {getCharacterName(selectedItem.characterId) || "Unassigned"}
            </p>

            {userRole === "GameMaster" ? (
              <>
                <h5>Assign/Reassign Owner</h5>
                <select
                  value={selectedCharacterId}
                  onChange={(e) => setSelectedCharacterId(e.target.value)}
                >
                  <option value="">Select a character</option>
                  {Array.isArray(characters) && characters.length > 0 ? (
                    characters.map((character) => (
                      <option key={character.characterId} value={character.characterId}>
                        {character.name}
                      </option>
                    ))
                  ) : (
                    <option value="">No characters available</option>
                  )}
                </select>
                <button onClick={handleAssignItem}>Assign</button>
              </>
            ) : (
              <p>Only the GameMaster can assign/reassign items.</p>
            )}
            <button onClick={closeModal}>Close</button>
          </>
        )}
      </Modal>
    </div>
  );
};

export default GameSessionItems;
