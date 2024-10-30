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
  - `POST /api/auth/register`: Register a new user. **Authorization**: None.
  - `POST /api/auth/login`: Authenticate a user and return a JWT. **Authorization**: None.

#### User Management
- **Endpoints**:
  - `GET /api/users`: Retrieve all users. **Authorization**: Admin only.
  - `GET /api/users/{id}`: Retrieve a specific user by ID. **Authorization**: Admin only.
  - `PUT /api/users/{id}`: Update user details. **Authorization**: Admin only.
  - `DELETE /api/users/{id}`: Delete a user. **Authorization**: Admin only.

#### Game Sessions
- **Endpoints**:
  - `POST /api/sessions`: Create a new game session. **Authorization**: Game Master only.
  - `GET /api/sessions`: Retrieve all game sessions. **Authorization**: All authenticated users.
  - `GET /api/sessions/{id}`: Retrieve a specific session by ID. **Authorization**: All authenticated users.
  - `PUT /api/sessions/{id}`: Update a game session. **Authorization**: Game Master only.
  - `DELETE /api/sessions/{id}`: Delete a game session. **Authorization**: Game Master only.

#### Item Management
- **Endpoints**:
  - `POST /api/items`: Create a new item. **Authorization**: Admin or Game Master.
  - `GET /api/items`: Retrieve all items. **Authorization**: All authenticated users.
  - `GET /api/items/{id}`: Retrieve a specific item by ID. **Authorization**: All authenticated users.
  - `PUT /api/items/{id}`: Update an item. **Authorization**: Admin or Game Master.
  - `DELETE /api/items/{id}`: Delete an item. **Authorization**: Admin only.

#### Character Management
- **Endpoints**:
  - `POST /api/characters`: Create a new character. **Authorization**: Player.
  - `GET /api/characters`: Retrieve all characters. **Authorization**: Game Master only.
  - `GET /api/characters/{id}`: Retrieve a specific character by ID. **Authorization**: All authenticated users.
  - `PUT /api/characters/{id}`: Update a character. **Authorization**: Player or Game Master.
  - `DELETE /api/characters/{id}`: Delete a character. **Authorization**: Player or Game Master.

#### Game Actions
- **Endpoints**:
  - `POST /api/sessions/{id}/roll`: Roll dice for a specific session. **Authorization**: Player.
  - `POST /api/sessions/{id}/perform-action`: Perform an action in a session. **Authorization**: Player.

#### Error Handling
Responses will include appropriate HTTP status codes and messages for error handling.

#### Notes
Ensure to include the JWT in the Authorization header for protected routes:
`Authorization: Bearer {token}`

