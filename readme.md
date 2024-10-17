# Scrolls and Rolls


## English
### Description
#### System Purpose
The system is an online game based on the principles of "Dungeons and Dragons".

Logged-in players create their characters and use RNG or dice rolls to determine their character's attributes.

The game progresses in "steps," where one "step" represents a scenario, e.g., "you encounter 5 living skeletons in a dungeon." In this step, characters taketurns to perform actions based on their items/abilities.

The game length is determined by the Game Master.
#### Functional Requirements
- Creation and suspension of game sessions.
- Creation, description, and attachment of images for items, abilities, scenario locations, and characters.
- Dice rolling.
- Step-by-step gameplay.
- Communication with other players in the session through a message box.
#### Technologies
- Database: Azure
- Front-end: React
- Back-end: .NET
#### Roles
- Administrator
- Game Master
- Player
#### Hierarchy
Game Session > Characters (including players) > Items/Abilities
#### Specific API
Assign item to a character.
Re-assign item from a character to another character.



## Lithuanian
### Aprašymas
#### Sistemos paskirtis
Sistema yra internetinis žaidimas, kurio principas panašus į "Dungeons and Dragons".

Prisijungę žaidėjai turi sukurti savo veikėją, naudoti "RNG" arba kauliuko mėtimą nustatyti savo veikėjo atributus.

Žaidimas vyksta "žingsniais" arba "steps", kur vienas "žingsnis" yra scenarijus, pvz. "požėmyje aptinkate 5 gyvus skeletus", šiame žingsnyje veikėjai eitų ratu ir atliktų veiksmus priklausomai nuo jų turimų daiktų/galių.

Žaidimas trunka pagal "Game Master" nuostatą.
#### Funkciniai reikalavimai
- Žaidimo sesijų kūrimas ir sustabdymas.
- Daiktų, galių, scenarijų vietų ir veikėjų sukūrimas, aprašymas ir nuotraukos prikabinimas.
- Kauliukų metimas.
- Žaidimo eiga "žingsniais"
- Komunikacija per žinučių langelį su kitais žaidėjais žaidimo sesijoje.
#### Technologijos
- Duombazė - Azure
- Front-end - React
- Back-end - .NET
#### Rolės
- Administratorius
- Žaidimo prižiūrėtojas (Game Master)
- Žaidėjas (Player)
#### Hierarchija
Žaidimo sesija (Game session) > Veikėjai (Characters) (Įskaitant žaidėjus) > Daiktai/Galios (Items/Abilities)
#### Konkretus API
Priskirti daiktą žaidėjui.
Priskirti žaidėjo turimą daiktą kitam žaidėjui.
