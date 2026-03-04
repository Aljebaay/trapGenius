# Devilbait Project Documentation

## 1. Overview
**Devilbait** is a precision platformer game built in Unity. The core mechanic involves levels that change and evolve based on the number of player attempts. Traps can appear, disappear, or change behavior as the player dies and retries, creating a dynamic and "troll-like" experience.

**Target Platform:** Android (Mobile) & PC
**Engine Version:** Unity 6 (Recommended)

---

## 2. Project Folder Structure

The project is organized to separate logic, art, and level design tools.

- **`Assets/Scripts/`**: Contains all C# source code.
  - **`Core/`**: Base classes for Traps, Projectiles, and Triggers.
  - **`Player/`**: Player movement, animation, and input logic.
  - **`Managers/`**: Global systems (GameManager, AudioManager, Inventory).
  - **`Trap Types/`**: Specific trap implementations (Spikes, Saws, etc.).
  - **`Interactables/`**: Buttons, Switches, Teleporters.
- **`Assets/Prefabs/`**: Ready-to-use game objects.
  - **`Level Builder Tools/`**: **CRITICAL FOR DESIGNERS**. Contains all drag-and-drop traps and walls.
  - **`Interactables/`**: Keys, Doors, Buttons.
  - **`Pickups/`**: Coins, Collectibles.
- **`Assets/Scenes/`**:
  - `Level 1`: The main gameplay loop example.
  - `Traps showcase`: A gallery of all available traps for testing.
  - `Scratch Level`: A sandbox for experimenting.
- **`Assets/Resources/`**: Configuration files.
  - `PlayerData`: Global settings for player physics (Speed, Jump Height).

---

## 3. Quick Start (Build a Level in 10 Minutes)

1. **Create a Scene**: Duplicate `Assets/Scenes/Scratch Level` to start with a clean slate.
2. **Add Ground**: Drag `Ground` prefabs from `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/`.
3. **Add Player**: Ensure the `Player` prefab is in the scene (usually at position 0,0,0).
4. **Add a Trap**:
   - Go to `Assets/Prefabs/Level Builder Tools/Traps/`.
   - Drag a `Spike Trap` or `Moving Saw` into the scene.
5. **Make it "Troll"**:
   - Select the Trap.
   - In the Inspector, find the `TrapBase` component.
   - Set **Activation Chance** to `50` (50% chance to kill).
   - OR add an **AttemptTrigger** (see Section 7) to make it appear only after 3 deaths.
6. **Add a Goal**: Drag `Goal` from `Assets/Prefabs/Interactables TileMap Version/`.
7. **Play**: Press Play in Unity.

---

## 4. Core Architecture

The game relies on a few singleton managers and a modular trap system.

### Key Systems
- **`GameManager`** (`Assets/Scripts/Managers/GameManager.cs`):
  - Tracks **Attempts** (how many times the player died in this level).
  - Tracks **Coins**.
  - Handles Level Loading and Resets.
- **`TrapBase`** (`Assets/Scripts/Core/TrapBase.cs`):
  - The parent class for ALL traps.
  - Handles **RNG** (`Activation Chance`).
  - Handles **Mutations** (changing player speed/size on hit).
  - Handles **Kill Direction** (e.g., "Only Top" means safe to touch sides).

### Flow Diagram
```ascii
[Scene Start] -> [GameManager: Increment Attempts]
      |
      v
[Player Spawns] -> [Input: Move/Jump]
      |
      +---> [Trigger: AttemptTrigger] -> (Check Attempts) -> [Activate Trap]
      |
      +---> [Trap Collision] -> [Player Death] -> [GameManager: Restart Level]
      |
      +---> [Goal Trigger] -> [Level Complete] -> [Load Next Scene]
```

---

## 5. Player Controller
**Script:** `Assets/Scripts/Player/PlayerController.cs`

The player uses a custom physics controller (Rigidbody2D) for tight platforming feel.

### Features
- **Coyote Time**: Allows jumping shortly after walking off a ledge.
- **Jump Buffer**: Remembers jump inputs pressed slightly before hitting the ground.
- **Variable Jump Height**: Hold jump longer to go higher.
- **Mutations**: The player's stats can be modified at runtime by traps.
  - *Example*: A "Giant Potion" trap can change `PlayerScale` to 2.0.
  - *Example*: A "Heavy Zone" can change `GravityMultiplier`.

### Configuration
Edit `Assets/Resources/PlayerData.asset` to change global physics values (Speed, Jump Force) without touching code.

---

## 6. Level Builder Workflow

Level designers should primarily work in the **Scene View** using **Prefabs**.

### Categories (`Assets/Prefabs/Level Builder Tools/`)
1. **Traps**:
   - `Ceiling`: Falling spikes, crushers.
   - `Floor`: Disappearing platforms, slippery ice.
   - `Walls`: Moving walls, shooting walls.
   - `FreeZone`: Rotating saws, projectiles.
2. **Interactables**:
   - `Buttons` / `Switches`: Activate other objects.
   - `Teleporter`: Instant travel.
   - `Locked Door` + `Key`: Progression gating.

### Connecting Logic
To make a Button open a Door:
1. Place a `PhysicsButton` and a `LockedDoor`.
2. On the Button's `OnPressed` event in the Inspector, click `+`.
3. Drag the Door object into the slot.
4. Select `LockedDoor.Open()`.

---

## 7. Triggers & Sequencing

This is the heart of the "Devilbait" mechanic.

### Attempt Trigger (`Assets/Scripts/Core/AttemptTrigger.cs`)
Use this to make things happen based on how many times the player has died.

- **Check Type**:
  - `Equal`: Happens ONLY on a specific attempt (e.g., "Death #3").
  - `Greater`: Happens AFTER attempt X (e.g., "From death #4 onwards").
  - `Modulo`: Happens every X attempts (e.g., "Every 3rd death").
- **Usage**:
  - Add this component to an empty GameObject.
  - Use the `OnConditionMet` event to enable a Trap GameObject.
  - *Result*: The path is safe for the first 2 runs, then a spike appears on run 3.

### Trap Sequencer (`Assets/Scripts/Core/TrapSequencer.cs`)
Use this to create complex timing patterns.
- **Steps**: A list of actions with delays.
- **Example**:
  1. Delay 1.0s -> Warning Light On.
  2. Delay 0.5s -> Spikes Up.
  3. Delay 2.0s -> Spikes Down.

---

## 8. Data & Configuration

### ScriptableObjects
- **`PlayerData`** (`Assets/Resources/PlayerData.asset`):
  - `Move Speed`: Base walking speed.
  - `Jump Height`: Max jump height.
  - `Coyote Time`: Grace period for jumps.
- **`PlayerInventory`** (`Assets/Resources/PlayerInventory.asset`):
  - Tracks collected keys and items.

---

## 9. Performance & Mobile Notes

- **Mobile Controls**: The project includes on-screen controls.
  - `MobileMoveButton.cs` and `MobileJumpButton.cs` handle touch input.
  - Ensure the **Canvas** with these buttons is active for Android builds.
- **Optimization**:
  - Traps check `ActivationChance` (RNG) efficiently.
  - If `ActivationChance` is 0, the logic skips early.
  - Use `Sprite Atlas` (if available) for UI elements to reduce draw calls.

---

## 10. Troubleshooting & Common Errors

| Issue | Cause | Fix |
|-------|-------|-----|
| **Player doesn't move** | `PlayerData` might be missing or zeroed. | Check `Assets/Resources/PlayerData.asset`. |
| **Trap triggers but nothing happens** | `ActivationChance` might be 0. | Set `ActivationChance` to 100 on the Trap component. |
| **Trap kills me when I touch the side** | `KillDirection` is set to `AnySide`. | Change `KillDirection` to `OnlyTop` (for spikes) or `OnlyBelow`. |
| **Level doesn't reload on death** | Scene not in Build Settings. | Go to `File > Build Settings` and add the current scene. |
| **"Missing Reference" in Inspector** | Prefab link broken. | Re-drag the target object (e.g., Door) into the Event slot. |

---

## 11. Glossary

- **Coyote Time**: A game design term allowing players to jump for a few frames after leaving the ground, making controls feel "fairer".
- **Prefab**: A template for a game object. Changing the Prefab changes all copies in the game.
- **Singleton**: A script that only has one copy (like `GameManager`).
- **RNG**: Random Number Generation. Used for probabilistic traps.
- **Mutation**: A temporary or permanent change to the player's physics (speed, gravity) caused by a trap.
