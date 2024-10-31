# Scrolls and Rolls

## Description

#### System Purpose
The system is an online game based on the principles of **Dungeons and Dragons**. Logged-in players create their characters and use RNG or dice rolls to determine their character's attributes.

The game progresses in "steps," where one "step" represents a scenario, e.g., "you encounter 5 living skeletons in a dungeon." In this step, characters take turns to perform actions based on their items/abilities.

The game length is determined by the Game Master.

#### Functional Requirements
- Creation and suspension of game sessions.
- Creation, description, and attachment of images for items, abilities, scenario locations, and characters.
- Dice rolling.
- Step-by-step gameplay.
- Communication with other players in the session through a message box.

#### Technologies
- **Database**: Azure
- **Front-end**: React
- **Back-end**: .NET

#### Roles
- Administrator
- Game Master
- Player

#### Hierarchy
- Game Session 
  - Characters (including players) 
    - Items/Abilities

#### Specific API
- Assign item to a character.
- Re-assign item from a character to another character.

---

## API Documentation

This API provides functionality for managing user accounts and game sessions in the "Scrolls and Rolls" game. It utilizes **JWT** for authentication and offers a secure, RESTful interface.

#### Authentication
- **Method**: JWT
- **Endpoints**:
  - `POST /auth/register`: Register a new user. **Authorization**: None.
  - `POST /auth/login`: Authenticate a user and return a JWT. **Authorization**: None.
  - `POST /auth/createadmin`: Create a new admin user. **Authorization**: Admin only.

#### User Management
- **Endpoints**:
  - `GET /users`: Retrieve all users. **Authorization**: Admin only.
  - `GET /users/{userId}`: Retrieve a specific user by ID. **Authorization**: Admin only.
  - `PUT /users/{userId}`: Update user details. **Authorization**: Admin only.
  - `DELETE /users/{userId}`: Delete a user. **Authorization**: Admin only.

#### Game Sessions
- **Endpoints**:
  - `POST /sessions`: Create a new game session. **Authorization**: Game Master only.
  - `GET /sessions`: Retrieve all game sessions. **Authorization**: All authenticated users.
  - `GET /sessions/{sessionId}`: Retrieve a specific session by ID. **Authorization**: All authenticated users.
  - `PUT /sessions/{sessionId}`: Update a game session. **Authorization**: Game Master only.
  - `DELETE /sessions/{sessionId}`: Delete a game session. **Authorization**: Game Master only.

#### Item Management
- **Endpoints**:
  - `POST /sessions/{sessionId}/items`: Create a new item in a session. **Authorization**: Admin or Game Master.
  - `GET /sessions/{sessionId}/items`: Retrieve all items in a session. **Authorization**: All authenticated users.
  - `GET /sessions/{sessionId}/items/{itemId}`: Retrieve a specific item by ID. **Authorization**: All authenticated users.
  - `PUT /sessions/{sessionId}/items/{itemId}`: Update an item. **Authorization**: Admin or Game Master.
  - `DELETE /sessions/{sessionId}/items/{itemId}`: Delete an item. **Authorization**: Admin only.
  - `POST /sessions/{sessionId}/items/{itemId}/assign/{characterId}`: Assign an item to a character. **Authorization**: Game Master only.

#### Character Management
- **Endpoints**:
  - `POST /sessions/{sessionId}/characters`: Create a new character in a session. **Authorization**: Player.
  - `GET /sessions/{sessionId}/characters`: Retrieve all characters in a session. **Authorization**: Game Master only.
  - `GET /sessions/{sessionId}/characters/{characterId}`: Retrieve a specific character by ID. **Authorization**: All authenticated users.
  - `PUT /sessions/{sessionId}/characters/{characterId}`: Update a character. **Authorization**: Player or Game Master.
  - `DELETE /sessions/{sessionId}/characters/{characterId}`: Delete a character. **Authorization**: Player or Game Master.

#### Game Actions
- **Endpoints**:
  - `GET /sessions/{sessionId}/messages`: Retrieve all messages in a session. **Authorization**: All authenticated users.
  - `GET /sessions/{sessionId}/messages/{userId}`: Retrieve all messages from a specific user in a session. **Authorization**: All authenticated users.
  - `POST /sessions/{sessionId}/messages`: Create a new message in a session. **Authorization**: All authenticated users.

#### Error Handling
Responses will include appropriate HTTP status codes and messages for error handling.

#### Notes
Ensure to include the JWT in the Authorization header for protected routes:
`Authorization: Bearer {token}`

