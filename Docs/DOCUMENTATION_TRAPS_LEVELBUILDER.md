# Devilbait — Traps & Level Builder Handbook

**Audience:** Level designers (primary), Unity developers (secondary).  
**Use this doc as:** A no-code handbook for building traps and assembling levels quickly.

---

## Table of Contents

1. [Trap & Tool Inventory](#1-trap--tool-inventory)
2. [Categories by Design Purpose](#2-categories-by-design-purpose)
3. [Direct Lethal](#3-direct-lethal)
4. [Ambush / Surprise](#4-ambush--surprise)
5. [Psychological Bait & Betrayals](#5-psychological-bait--betrayals)
6. [Timing & Rhythm](#6-timing--rhythm)
7. [Sequence / Multi-Step](#7-sequence--multi-step)
8. [Environmental / Terrain](#8-environmental--terrain)
9. [Level Patterns](#9-level-patterns)
10. [Testing Checklist](#10-testing-checklist)
11. [How to Create a New Trap Prefab Type (Developers)](#11-how-to-create-a-new-trap-prefab-type-developers)

---

## 1. Trap & Tool Inventory

### Trap prefabs (Level Builder Tools)

| Path | Script (concept) |
|------|------------------|
| `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Spike Trap.prefab` | SpikeTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Kill Player.prefab` | SpikeTrap (invisible zone) |
| `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/ArrowTrap.prefab` | ArrowTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Rotating Saw trap.prefab` | ConstantRotateTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceiling AmbushTrap-Falling.prefab` | AmbushTrap (FallGravity) |
| `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceiling AmbushTrap-PopUp.prefab` | AmbushTrap (PopUp) |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Floor AmbushTrap-PopUp.prefab` | AmbushTrap (PopUp) |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Wall AmbushTrap-PopUp.prefab` | AmbushTrap (PopUp) |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Fake Platform.prefab` | FakePlatform |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Fake Wall.prefab` | Wall (Ghost/Open) |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Delayed Disapearing Platfrom.prefab` | DisappearingPlatform |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Instantly Disapearing Platfrom.prefab` | DisappearingPlatform (delay 0) |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Gradually Collapsing floor.prefab` | BreakingPlatform |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Slippery Floor.prefab` | SlipperyFloor |
| `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Moving Ceiling.prefab` | MovingTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Moving Floor.prefab` | MovingTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Moving Wall.prefab` | MovingTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Walls that reverse direction.prefab` | MovingTrap (ping-pong) |
| `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceilings that shoot projectiles.prefab` | ArrowTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Walls that shoot projectiles.prefab` | ArrowTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Crushing Ceiling.prefab` | CrushingWall |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Crushing Floor.prefab` | CrushingWall |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Crushing Wall.prefab` | CrushingWall |
| `Assets/Prefabs/Level Builder Tools/Traps/Special On trigger traps (3)/TriggeredMove Trap.prefab` | TriggeredMoveTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Special On trigger traps (3)/TriggeredRotate Trap.prefab` | TriggeredRotateTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/Special On trigger traps (3)/TriggeredScale Trap.prefab` | TriggeredScaleTrap |
| `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Attempt Trigger.prefab` | AttemptTrigger |
| `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Trap Sequencer.prefab` | TrapSequencer |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Ground.prefab` | (static terrain) |
| `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Transparent Walkable Floor.prefab` | (walkable surface) |
| `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceiling.prefab` | (static) |
| `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Wall.prefab` | Wall (Solid/Ghost/Open) |

### Interactables & zones (outside Level Builder Tools)

| Path | Purpose |
|------|--------|
| `Assets/Prefabs/Interactables TileMap Version/Death Zone.prefab` | Instant death on enter (DeathZone.cs) |
| `Assets/Prefabs/Interactables TileMap Version/Goal.prefab` | Level complete (GoalTrigger.cs) |
| `Assets/Prefabs/Interactables TileMap Version/Area Trigger.prefab` | One-time event on enter (AreaTrigger.cs) |
| `Assets/Prefabs/Interactables/Buttons.prefab` | PhysicsButton |
| `Assets/Prefabs/Interactables/Switches.prefab` | PhysicsSwitch |
| `Assets/Prefabs/Interactables/Teleporter.prefab` | Teleporter |
| `Assets/Prefabs/Interactables/Locked Door.prefab` | LockedDoor + Key |

### Trap scripts (paths)

- `Assets/Scripts/Core/TrapBase.cs` — Base class for all traps
- `Assets/Scripts/Trap Types/SpikeTrap.cs`
- `Assets/Scripts/Trap Types/ArrowTrap.cs`
- `Assets/Scripts/Trap Types/AmbushTrap.cs`
- `Assets/Scripts/Trap Types/FakePlatform.cs`
- `Assets/Scripts/Trap Types/DisappearingPlatform.cs`
- `Assets/Scripts/Trap Types/BreakingPlatform.cs`
- `Assets/Scripts/Trap Types/SlipperyFloor.cs`
- `Assets/Scripts/Trap Types/MovingTrap.cs`
- `Assets/Scripts/Trap Types/ConstantRotateTrap.cs`
- `Assets/Scripts/Trap Types/CrushingWall.cs`
- `Assets/Scripts/Trap Types/TriggeredMoveTrap.cs`
- `Assets/Scripts/Trap Types/TriggeredRotateTrap.cs`
- `Assets/Scripts/Trap Types/TriggeredScaleTrap.cs`
- `Assets/Scripts/Core/AttemptTrigger.cs`
- `Assets/Scripts/Core/TrapSequencer.cs`
- `Assets/Scripts/Core/DeathZone.cs`
- `Assets/Scripts/Trigger/GoalTrigger.cs`
- `Assets/Scripts/Trigger/AreaTrigger.cs`
- `Assets/Scripts/Core/Wall.cs`

---

## 2. Categories by Design Purpose

| Category | Use when you want… | Prefabs / tools |
|----------|--------------------|------------------|
| **1) Direct lethal** | Instant or obvious death (spikes, pits, kill zone) | Spike Trap, Kill Player, Death Zone, ArrowTrap (projectile kill) |
| **2) Ambush / surprise** | Hidden or delayed danger (falling, pop-up) | Ceiling/Floor/Wall AmbushTrap-Falling, AmbushTrap-PopUp |
| **3) Psychological bait** | Fake safety, coins, betrayals | Fake Platform, Fake Wall, (coins via Pickups) |
| **4) Timing & rhythm** | Moving or rotating hazards | Moving Floor/Ceiling/Wall, Rotating Saw, Crushing Wall/Ceiling/Floor, Slippery Floor |
| **5) Sequence / multi-step** | Linked triggers, attempt-based behavior | Attempt Trigger, Trap Sequencer, TriggeredMove/Rotate/Scale, Buttons, Switches |
| **6) Environmental** | Terrain and structure | Ground, Transparent Walkable Floor, Ceiling, Wall (Solid/Ghost/Open), Goal, Area Trigger |

---

## 3. Direct Lethal

### Spike Trap

- **What it does:** Kills the player on contact (collision or trigger) when **Is Lethal** is on and RNG passes.
- **When to use it:** Classic spikes, obvious danger, or as a lethal trigger zone.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Spike Trap.prefab`

**How to place**

1. Drag **Spike Trap** from the Project window into the Scene.
2. Position it where the hazard should be (e.g. floor, pit edge).
3. Rotate so the “danger” side faces the path (affects **Kill Direction**).
4. Ensure the player has a **Collider2D** and is tagged **Player**.
5. Press Play and walk into the trap to test.

**Inspector (TrapBase + SpikeTrap)**

- **Is Lethal:** On = contact can kill. Off = never kills (e.g. decorative).
- **Kill Direction:** From which side the trap kills.
  - **Any Side** — any contact kills.
  - **Only Top** — only when player hits from above (e.g. landing on spikes).
  - **Only Below** — ceiling spikes; kill when player hits from below.
  - **Only Left / Only Right** — side spikes.
- **Activation Chance (0–100):** Chance the trap “works” each time. 100 = always; 0 = never (safe); 50 = 50% kill.
- **Changes Player Data / Mutations:** Optional; usually leave off for simple spikes.

**Common mistakes**

- Forgetting **Is Lethal** → trap never kills.
- **Kill Direction** wrong (e.g. ceiling spikes with Only Top) → no kill.
- Player not tagged **Player** or no collider → no trigger.

**Test:** Enter once with **Activation Chance** 100 (should die). Set to 0 (should not die). Try from top vs sides with **Only Top** and confirm only top kills.

**Mini recipes**

- **Classic floor spikes:** Spike Trap, Is Lethal On, Kill Direction **Only Top**. Place on floor in a corridor.
- **50% troll:** Same as above, **Activation Chance** 50. Sometimes safe, sometimes not.

---

### Kill Player (invisible zone)

- **What it does:** Invisible trigger zone that kills when the player enters. Uses **SpikeTrap** with sprite alpha 0; enable **Is Lethal** to kill.
- **When to use it:** Invisible death zones, “pit” areas, or custom kill regions without art.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Kill Player.prefab`

**How to place**

1. Drag **Kill Player** into the scene.
2. Scale the Transform (or the BoxCollider2D) to cover the deadly area (e.g. under a gap).
3. In the Inspector, on the **SpikeTrap** component: enable **Is Lethal**.
4. Set **Kill Direction** (usually **Any Side** for a zone).

**Inspector**

- Same as Spike Trap: **Is Lethal**, **Kill Direction**, **Activation Chance**. Leave **Changes Player Data** off unless you want mutations.

**Common mistakes**

- Leaving **Is Lethal** off in the prefab → zone never kills.
- Zone too small or offset so the player never enters.

**Mini recipes**

- **Pit under platform:** Place Kill Player under a platform gap; scale to match. Is Lethal On, Any Side.
- **Invisible corridor:** Long thin Kill Player in a corridor; use with Attempt Trigger so it only activates from attempt 2+.

---

### Death Zone

- **What it does:** Any object with tag **Player** that enters the trigger immediately dies and triggers level restart. No RNG, no kill direction.
- **When to use it:** Bottomless pits, lava, instant death areas. Not a TrapBase trap.
- **Prefab:** `Assets/Prefabs/Interactables TileMap Version/Death Zone.prefab`  
- **Script:** `Assets/Scripts/Core/DeathZone.cs`

**How to place**

1. Drag **Death Zone** into the scene.
2. Resize the Transform so the box covers the deadly area (e.g. below the level).
3. Ensure the GameObject has a **Collider2D** set to **Is Trigger**.
4. Player must be tagged **Player**.

**Inspector**

- No special settings. Scale and position define the zone.

**Common mistakes**

- Collider not set to **Is Trigger** → may not fire as trigger.
- Death Zone too high so player never enters.

**Mini recipe**

- **Pit:** Place Death Zone below the playable area; scale width to level width so any fall kills.

---

### Arrow Trap (projectile kill)

- **What it does:** Spawns arrows (or similar projectiles) that move in the trap’s “right” direction; they can kill the player on hit (if **Kill Player** is on on the ArrowTrap).
- **When to use it:** Ranged hazard, timing challenge, or triggered barrage.
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/ArrowTrap.prefab`  
  - Ceiling: `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceilings that shoot projectiles.prefab`  
  - Wall: `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Walls that shoot projectiles.prefab`  
- **Script:** `Assets/Scripts/Trap Types/ArrowTrap.cs`

**How to place**

1. Drag the Arrow Trap prefab (FreeZone, Ceiling, or Wall) into the scene.
2. Rotate the trap so the **red axis (Right)** points in the direction you want arrows to fly (use Gizmos in Scene view).
3. Optionally assign a **Spawn Point** child Transform for the spawn position.
4. If you want it to start automatically, enable **Auto Start**; otherwise trigger **Activate()** via Attempt Trigger or Trap Sequencer.

**Inspector**

- **Arrow Prefab / Spawn Point:** Set in script; assign if the prefab has a reference slot.
- **Arrow Speed:** How fast projectiles move.
- **Kill Player:** On = arrows kill on contact; Off = no kill.
- **Auto Start:** If On, starts shooting on level load.
- **Loop:** If On, keeps firing in a cycle.
- **Arrows Per Burst / Fire Rate / Cycle Interval:** Control burst size, delay between shots, and time between bursts.
- **TrapBase:** **Is Lethal** applies to the trap *block* itself (touching the launcher). **Activation Chance** applies when **Activate()** is called.

**Common mistakes**

- Wrong rotation → arrows fly the wrong way (check red arrow in Scene view).
- **Kill Player** off on ArrowTrap → arrows don’t kill.
- No **Arrow Prefab** assigned in prefab → no arrows spawn.

**Mini recipes**

- **Single shot on enter:** Arrow Trap, Auto Start Off. Use **Area Trigger** → On Trigger Enter → Arrow Trap’s **Activate()**.
- **Rhythm corridor:** Arrow Trap, Auto Start On, Loop On, Cycle Interval 2. Player must pass between shots.

---

## 4. Ambush / Surprise

### Ambush Trap — Falling (gravity)

- **What it does:** Starts frozen; when **Activate()** is called, it drops with gravity and kills on contact (if lethal).
- **When to use it:** Ceiling blocks that drop on the player (e.g. when they step on a trigger or on attempt N).
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceiling AmbushTrap-Falling.prefab`  
- **Script:** `Assets/Scripts/Trap Types/AmbushTrap.cs` (type **FallGravity**).

**How to place**

1. Drag **Ceiling AmbushTrap-Falling** into the scene.
2. Place it above the path (e.g. over a corridor).
3. Do **not** rely on contact to activate unless you wire it: call **Activate()** from an **Attempt Trigger**, **Area Trigger**, or **Trap Sequencer**.
4. Ensure **Is Lethal** is On if it should kill on impact.

**Inspector**

- **Type:** FallGravity (driven by Rigidbody gravity after activate).
- **TrapBase:** **Is Lethal**, **Kill Direction**, **Activation Chance** (used when **Activate()** is called).

**Common mistakes**

- Expecting it to activate on player touch — it only drops when **Activate()** is invoked by something else.
- Trap placed too low and already overlapping the player at start.

**Mini recipes**

- **Drop on attempt 2:** Attempt Trigger (Condition **Equal**, Target **2**). On Condition Met → Ceiling AmbushTrap-Falling **Activate()**. First run safe, second run drops.
- **Drop when crossing line:** Area Trigger over the path. On Trigger Enter → Ambush **Activate()**.

---

### Ambush Trap — Pop-up

- **What it does:** When **Activate()** is called, the trap moves from its current position by **Pop Offset** at **Pop Speed** (no gravity). Can kill on contact if **Is Lethal**.
- **When to use it:** Spikes or blocks that pop out of floor, wall, or ceiling on trigger or attempt.
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceiling AmbushTrap-PopUp.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Floor AmbushTrap-PopUp.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Wall AmbushTrap-PopUp.prefab`  
- **Script:** `Assets/Scripts/Trap Types/AmbushTrap.cs` (type **PopUp**).

**How to place**

1. Drag the correct PopUp prefab (Ceiling, Floor, or Wall) into the scene.
2. Place it where the hazard should *start* (hidden in wall/floor/ceiling).
3. Set **Pop Offset** (e.g. (0, 1, 0) for floor spikes going up) and **Pop Speed**.
4. Call **Activate()** from Attempt Trigger, Area Trigger, or Trap Sequencer.

**Inspector**

- **Type:** PopUp.
- **Pop Offset:** World offset from start position to end position.
- **Pop Speed:** Movement speed.
- **TrapBase:** **Is Lethal**, **Kill Direction**, **Activation Chance**.

**Common mistakes**

- Pop Offset zero or wrong axis → trap doesn’t move visibly.
- Forgetting to link **Activate()** to a trigger or sequence.

**Mini recipes**

- **Spikes on attempt 3:** Floor AmbushTrap-PopUp at a choke point. Attempt Trigger (Equal, 3) → On Condition Met → **Activate()**.
- **Pop when player passes:** Area Trigger in the middle of the path. On Trigger Enter → Floor AmbushTrap-PopUp **Activate()**.

---

## 5. Psychological Bait & Betrayals

### Fake Platform

- **What it does:** Looks solid; when the player touches it, it fades out (Tilemap alpha to 0) and can kill (e.g. if player falls into a pit). CompositeCollider2D is set to trigger so the platform doesn’t block; the script handles kill by position.
- **When to use it:** “Safe” platforms that disappear, fake bridges, betrayal ledges.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Fake Platform.prefab`  
- **Script:** `Assets/Scripts/Trap Types/FakePlatform.cs`

**How to place**

1. Drag **Fake Platform** into the scene.
2. Place it over a pit or Death Zone so that when it fades, the player falls and dies (or over spikes).
3. Ensure the platform uses a Tilemap and CompositeCollider2D (prefab should have this).

**Inspector**

- **Fade Speed:** How fast the tilemap fades.
- **TrapBase:** **Is Lethal** and **Kill Direction** control whether touching the platform itself kills; often you rely on fall + Death Zone below instead.

**Common mistakes**

- No pit or death below → player just lands on the next surface and doesn’t die.
- Expecting it to kill on first touch — it fades then player falls; death is from the zone below.

**Mini recipes**

- **Fake bridge:** Fake Platform over a Death Zone or spikes. Player crosses once; next time they try, platform is gone (or use two platforms: one real, one fake).
- **First run safe, second run trap:** Use Attempt Trigger (GreaterOrEqual, 2) to **Activate()** a Fake Platform so it’s already “triggered” and fades as soon as they touch it on run 2.

---

### Fake Wall

- **What it does:** A **Wall** (script `Assets/Scripts/Core/Wall.cs`) that can be **Solid**, **Ghost** (pass-through, semi-transparent), or **Open** (no collider). Switch state via **BecomeSolid()**, **BecomeGhost()**, **BecomeOpen()**, or **ToggleOpenSolid()** from events.
- **When to use it:** Secret passages, “solid” walls that become passable on attempt or trigger, or fake walls that close behind the player.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Fake Wall.prefab`

**How to place**

1. Drag **Fake Wall** into the scene.
2. Set **Initial State** (Solid, Ghost, or Open).
3. From an Attempt Trigger or Button, call **BecomeGhost()** or **BecomeOpen()** to let the player through, or **BecomeSolid()** to close.

**Inspector**

- **Initial State:** Solid / Ghost / Open at level start.
- **Ghost Alpha:** Opacity when Ghost (0–1).

**Common mistakes**

- Forgetting to call **BecomeGhost()** or **BecomeOpen()** from an event → wall stays solid.
- Wrong initial state → wall open when you want it closed at start.

**Mini recipe**

- **Open on attempt 2:** Attempt Trigger (Equal, 2) → On Condition Met → Fake Wall **BecomeOpen()**. First run: wall blocks; second run: path opens.

---

## 6. Timing & Rhythm

### Moving Trap (floor / ceiling / wall)

- **What it does:** Moves between two positions (offset from start) and can optionally rotate. Can ping-pong or stop at destination. Can kill on contact if **Is Lethal**.
- **When to use it:** Moving platforms, moving hazards, timing-based corridors.
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Moving Floor.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Moving Ceiling.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Moving Wall.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Walls that reverse direction.prefab`  
- **Script:** `Assets/Scripts/Trap Types/MovingTrap.cs`

**How to place**

1. Drag the Moving Floor, Ceiling, or Wall into the scene.
2. Set **Local Target Offset 1** and **Local Target Offset 2** (relative to start). Often one is (0,0,0) and the other is the travel vector.
3. Set **Speed**. Enable **Auto Move** if it should start on load; otherwise call **Activate()** from a trigger.
4. **Is Stop On Reaching Destination:** On = one-way then stop; Off = ping-pong.

**Inspector**

- **Is Auto Move:** Start moving as soon as the level loads.
- **Is Stop On Reaching Destination:** One-way vs ping-pong.
- **Local Target Offset 1 / 2:** Endpoints relative to initial position.
- **Speed:** Movement speed.
- **Rotation Amount / Revert Rotation Direction:** Optional rotation during move.
- **TrapBase:** **Is Lethal**, **Kill Direction**, **Activation Chance**.

**Common mistakes**

- Both offsets zero → no movement.
- Expecting ping-pong but **Is Stop On Reaching Destination** is On → moves once and stops.

**Mini recipes**

- **Moving platform:** Moving Floor, Auto Move On, ping-pong (Is Stop Off). Safe to stand on; use **Kill Direction** Only Below if you want ceiling variant to kill when player hits from below.
- **Wall that closes after attempt 1:** Moving Wall, Auto Move Off. Attempt Trigger (GreaterOrEqual, 2) → **Activate()** so the wall moves on later attempts.

---

### Rotating Saw

- **What it does:** Spins continuously; contact can kill if **Is Lethal**. Uses **ConstantRotateTrap**.
- **When to use it:** Circular saws, spinning blades, rhythm obstacles.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Rotating Saw trap.prefab`  
- **Script:** `Assets/Scripts/Trap Types/ConstantRotateTrap.cs`

**How to place**

1. Drag **Rotating Saw trap** into the scene.
2. Position and scale as needed. Set **Rotation Speed** and **Direction** (Clockwise / Counter-Clockwise).
3. **Auto Start** On = spins from start; Off = call **Activate()** to start.

**Inspector**

- **Direction:** Clockwise / Counter-Clockwise.
- **Rotation Speed:** Degrees per second.
- **Auto Start:** Start on load vs wait for **Activate()**.
- **TrapBase:** **Is Lethal**, **Kill Direction**, **Activation Chance**.

**Common mistakes**

- **Is Lethal** off → no kill. **Auto Start** off and nothing calls **Activate()** → doesn’t spin.

**Mini recipe**

- **Saw in corridor:** Rotating Saw, Auto Start On, Is Lethal On. Place in a narrow corridor so the player must time the gap.

---

### Crushing Wall / Ceiling / Floor

- **What it does:** Moves along an offset (e.g. horizontal for wall); kills the player if they are **squished** between the crusher and another collider on **Crush Layer** (e.g. ground, wall). Can also kill on direct contact if **Is Lethal** and **Kill Direction** allow.
- **When to use it:** Crushers, closing corridors, “squish” traps.
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Crushing Wall.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Crushing Ceiling.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Crushing Floor.prefab`  
- **Script:** `Assets/Scripts/Trap Types/CrushingWall.cs`

**How to place**

1. Drag the Crushing Wall (or Ceiling/Floor) into the scene.
2. Set **Move Offset** (direction and distance).
3. Set **Crush Layer** to the layers of the geometry that should “squish” the player (e.g. Ground, default).
4. **Auto Start** On = starts on load; Off = call **Activate()**.
5. **Ping Pong** On = move back and forth; Off = one-way then stop.

**Inspector**

- **Move Offset:** Travel vector from start.
- **Speed / Wait Time:** Movement speed and pause at each end.
- **Ping Pong / Auto Start:** As above.
- **Crush Layer:** Layers to raycast against for squish (e.g. ground, walls).
- **Squish Check Distance:** Ray length from player; slightly larger than player width.
- **TrapBase:** **Is Lethal**, **Kill Direction**, **Activation Chance**.

**Common mistakes**

- **Crush Layer** empty or wrong → never detects squish.
- **Squish Check Distance** too small → squish not detected.

**Mini recipe**

- **Closing corridor:** Crushing Wall at one end, solid wall or ground on the other. Crush Layer = Ground. Auto Start On, Ping Pong On. Player must pass before being squished.

---

### Slippery Floor

- **What it does:** While the player is on it, reduces deceleration (slide) and enforces a minimum horizontal speed so they can’t stand still. Can optionally kill on contact (e.g. **Only Below** for ceiling variant) or apply mutations.
- **When to use it:** Ice, oil, sliding sections before pits or hazards.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Slippery Floor.prefab`  
- **Script:** `Assets/Scripts/Trap Types/SlipperyFloor.cs`

**How to place**

1. Drag **Slippery Floor** into the scene (usually as a Tilemap).
2. Place it before a pit, spikes, or narrow platform so the slide is the challenge.
3. Tune **Slippery Deceleration**, **Minimum Speed**, **Nudge Acceleration** to feel.

**Inspector**

- **Slippery Deceleration:** Lower = longer slide (e.g. 3).
- **Minimum Speed:** Speed the player is nudged to while on the floor.
- **Nudge Acceleration:** How quickly that speed is applied.
- **Apply Tint / Floor Tint:** Optional visual (e.g. blue for ice).
- **TrapBase:** **Is Lethal** if you want the floor itself to kill (rare).

**Common mistakes**

- Values too high → no noticeable slide. Too low minimum speed → player stops.

**Mini recipe**

- **Ice before pit:** Slippery Floor leading to a gap; player must jump at the right moment or fall into Death Zone.

---

### Disappearing Platform (delayed or instant)

- **What it does:** On first player contact, waits **Delay** seconds (optional shake), then disappears (collider and renderer off). Optionally **Respawns** after **Respawn Delay**. Can kill if the player is still on it when it vanishes (e.g. fall into pit) or via **Kill Direction**.
- **When to use it:** Timed platforms, “one-time” stepping stones, rhythm jumps.
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Delayed Disapearing Platfrom.prefab` (delay & optional shake)  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Instantly Disapearing Platfrom.prefab` (delay 0)  
- **Script:** `Assets/Scripts/Trap Types/DisappearingPlatform.cs`

**How to place**

1. Drag **Delayed** or **Instantly Disapearing Platfrom** into the scene.
2. Place over pit or hazards so that when it disappears, the player falls and dies (or use **Is Lethal** if the script supports kill on vanish).
3. Set **Delay** (0 = instant), **Shake Before Vanish**, **Respawn** and **Respawn Delay** if needed.

**Inspector**

- **Delay:** Seconds before disappear (0 = instant).
- **Shake Before Vanish / Shake Intensity:** Visual warning.
- **Respawn / Respawn Delay:** Bring platform back after a time.
- **TrapBase:** **Activation Chance**, **Is Lethal**, **Kill Direction**.

**Common mistakes**

- No hazard below → player just lands elsewhere. Expecting respawn but **Respawn** is off.

**Mini recipes**

- **One-time bridge:** Delayed Disappearing Platform over pit, Delay 1.5, no respawn. Player must cross before it vanishes.
- **Rhythm platforms:** Several Instantly Disappearing platforms; player must keep moving.

---

### Gradually Collapsing Floor (Breaking Platform)

- **What it does:** On player contact, shakes for **Time Before Break**, then turns dynamic and falls with gravity; destroyed after **Destroy After Seconds**. Can kill on contact if **Is Lethal**.
- **When to use it:** “Crumble” platforms, run-and-don’t-stop sections.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Gradually Collapsing floor.prefab`  
- **Script:** `Assets/Scripts/Trap Types/BreakingPlatform.cs`

**How to place**

1. Drag **Gradually Collapsing floor** into the scene.
2. Place over a pit or Death Zone so when it falls, the player falls too if they don’t move.
3. Set **Time Before Break** and **Fall Gravity** / **Destroy After Seconds**.

**Inspector**

- **Time Before Break:** Shake duration before it drops.
- **Shake Intensity:** Shake strength.
- **Fall Gravity / Destroy After Seconds:** How it falls and when it’s removed.
- **TrapBase:** **Activation Chance**, **Is Lethal**, **Kill Direction**.

**Common mistakes**

- No pit below → platform falls but player lands on something else. **Destroy After Seconds** too short and platform vanishes before player sees it fall.

**Mini recipe**

- **Crumble bridge:** Several Gradually Collapsing platforms in a row over a pit; player must not stop.

---

## 7. Sequence / Multi-Step

### Attempt Trigger

- **What it does:** Compares current **Attempts** (from GameManager) to a **Target Value** using a **Condition**. Fires **On Condition Met** or **On Condition Failed** (UnityEvents). Does not move or kill by itself — you wire it to traps or other logic.
- **When to use it:** “On death 2, spawn spikes”, “On attempt 3+, close the door”, “Only on first run, open a path”.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Attempt Trigger.prefab`  
- **Script:** `Assets/Scripts/Core/AttemptTrigger.cs`

**How to place**

1. Drag **Attempt Trigger** into the scene (position doesn’t matter for logic; keep it in a logical place in the hierarchy).
2. Set **Condition** and **Target Value** (e.g. Equal + 2 = “exactly attempt 2”).
3. In **On Condition Met**, add a call to the target (e.g. Ambush Trap **Activate()**, or enable a GameObject).
4. **When** to run the check: either call **AttemptTrigger.Check()** from something that runs every time the level loads (e.g. a small script that calls it in Start), or wire **Check()** to **GoalTrigger** / **Area Trigger** / **Trap Sequencer** as needed.  
   - **TODO:** In this repo, nothing automatically calls **AttemptTrigger.Check()** on level load. Designers typically need a component that calls **Check()** in **Start()** (or from a Level Load event) so attempt-based traps fire at the start of each run. Verify in your scene how **Check()** is invoked (e.g. from a “Level Bootstrap” object).

**Inspector**

- **Condition:** Equal, Greater, Less, GreaterOrEqual, Modulo.
- **Target Value:** Number to compare to **Attempts**.
- **On Condition Met / On Condition Failed:** UnityEvents; drag the target object and select the function (e.g. **Activate()**, **SetActive(true)**).

**Common mistakes**

- Never calling **Check()** → condition never runs. Wrong Condition or Target (e.g. Equal 2 when you want “attempt 2 and above” → use GreaterOrEqual 2).

**Mini recipes**

- **Spikes on attempt 2:** Attempt Trigger (Equal, 2). On Condition Met → Spike Trap or Ambush **Activate()**. Ensure **Check()** is called at level start.
- **Permanent change from attempt 3:** Attempt Trigger (GreaterOrEqual, 3). On Condition Met → Moving Wall **Activate()** or Fake Wall **BecomeOpen()**.

---

### Trap Sequencer

- **What it does:** Runs a list of **Steps** in order. Each step has a **Delay** and an **Action** (UnityEvent). Use it to chain: wait → activate trap A → wait → activate trap B.
- **When to use it:** Multi-step sequences, delayed spawns, “warning then danger”.
- **Prefab:** `Assets/Prefabs/Level Builder Tools/Traps/FreeZone (6)/Trap Sequencer.prefab`  
- **Script:** `Assets/Scripts/Core/TrapSequencer.cs`

**How to place**

1. Drag **Trap Sequencer** into the scene.
2. Add steps: for each step set **Delay** (seconds before this step’s action) and **Action** (e.g. call **Activate()** on a trap).
3. Start the sequence by calling **StartSequence()** from an Area Trigger, Attempt Trigger, or Button.

**Inspector**

- **Steps:** List of (Delay, Action). Delay is relative to the previous step.
- **StartSequence()** / **StopSequence():** Public methods to start or stop.

**Common mistakes**

- Forgetting to call **StartSequence()** from somewhere. Delay 0 on first step is fine (runs immediately).

**Mini recipes**

- **Warning then drop:** Step 1: Delay 0, Action = “Warning light On” (optional). Step 2: Delay 1, Action = Ceiling AmbushTrap-Falling **Activate()**. Trigger **StartSequence()** from Area Trigger.
- **Two waves:** Step 1: Delay 0, Arrow Trap **Activate()**. Step 2: Delay 2, second Arrow Trap **Activate()**. Trigger from Area Trigger.

---

### Triggered Move / Rotate / Scale traps

- **What they do:** Start idle; when **Activate()** is called, they move, rotate, or scale once (no ping-pong). Can kill on contact if **Is Lethal**.
- **When to use it:** Doors, gates, platforms or hazards that only move when a button, attempt, or sequence triggers them.
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/Special On trigger traps (3)/TriggeredMove Trap.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Special On trigger traps (3)/TriggeredRotate Trap.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Special On trigger traps (3)/TriggeredScale Trap.prefab`  
- **Scripts:** `TriggeredMoveTrap.cs`, `TriggeredRotateTrap.cs`, `TriggeredScaleTrap.cs`

**How to place**

1. Drag the Triggered Move, Rotate, or Scale prefab into the scene.
2. Set the target (move offset, rotation amount, or scale).
3. Call **Activate()** from a Button, Attempt Trigger, or Trap Sequencer step.
4. **TriggeredMoveTrap:** **Trigger On Contact** = also activate when the player touches it (optional).
5. **TriggeredRotateTrap:** Assign **Parent Rotation Pivot** if the prefab has a child that should rotate.
6. **TriggeredScaleTrap:** Set **Target Scale** and **Scale Anchor** (e.g. shrink from center or from one edge).

**Inspector (short)**

- **TriggeredMoveTrap:** Move Offset, Speed, Trigger On Contact.
- **TriggeredRotateTrap:** Rotation Amount, Speed, Parent Rotation Pivot.
- **TriggeredScaleTrap:** Target Scale, Speed, Anchor (Center, Left, Right, Top, Bottom).
- **TrapBase:** Is Lethal, Kill Direction, Activation Chance.

**Mini recipes**

- **Door opens on button:** TriggeredMove Trap (e.g. move up). Physics Button On Pressed → TriggeredMove Trap **Activate()**.
- **Platform appears on attempt 2:** TriggeredScale Trap (scale from 0 to 1). Attempt Trigger (Equal, 2) → On Condition Met → **Activate()**. Ensure **Check()** is called at level start.

---

### Buttons and Switches

- **What they do:** **PhysicsButton** — press when something on **Contact Layer** is on it; fires **On Pressed** / **On Released**. **PhysicsSwitch** — toggle on/off when triggered; fires **On Turn On** / **On Turn Off**. Both extend **TrapBase**; can be lethal if **Is Lethal** is On (e.g. pressure plate that kills).
- **When to use it:** Open doors, start sequences, turn traps on/off. Use **Is Lethal** Off for normal buttons.
- **Prefabs:** `Assets/Prefabs/Interactables/Buttons.prefab`, `Assets/Prefabs/Interactables/Switches.prefab`  
- **Scripts:** `Assets/Scripts/Interactables/PhysicsButton.cs`, `Assets/Scripts/Interactables/PhysicsSwitch.cs`

**How to place**

1. Drag **Buttons** or **Switches** into the scene.
2. Set **Contact Layer** (e.g. Player, or a layer that includes the player).
3. In **On Pressed** (button) or **On Turn On** (switch), add the target (e.g. TriggeredMove Trap **Activate()**, Trap Sequencer **StartSequence()**).
4. Leave **Is Lethal** Off unless you want the button/switch to kill when touched.

**Mini recipe**

- **Button opens door:** Button → On Pressed → TriggeredMove Trap (door) **Activate()**.

---

## 8. Environmental / Terrain

### Ground, Transparent Walkable Floor, Ceiling, Wall

- **What they do:** Static or mostly static terrain. **Ground** and **Transparent Walkable Floor** are walkable surfaces. **Ceiling** and **Wall** are structural; **Wall** prefab uses **Wall.cs** and can be Solid / Ghost / Open (see Fake Wall).
- **Prefabs:**  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Ground.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Transparent Walkable Floor.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Ceiling (6)/Ceiling.prefab`  
  - `Assets/Prefabs/Level Builder Tools/Traps/Walls (7)/Wall.prefab`

Use these to build the level geometry; no special trap logic unless you use **Wall** with **BecomeGhost** / **BecomeOpen** from events.

---

### Goal

- **What it does:** When the player enters the trigger, the player is frozen and **GameManager.LevelComplete()** is called (next level or loop).
- **Prefab:** `Assets/Prefabs/Interactables TileMap Version/Goal.prefab`  
- **Script:** `Assets/Scripts/Trigger/GoalTrigger.cs`

Place at the end of the level; ensure the Goal has a trigger collider and the player is tagged **Player**.

---

### Area Trigger

- **What it does:** Fires **On Trigger Enter** once when the player first enters. Use it to start sequences, call **Activate()** on traps, or call **AttemptTrigger.Check()** if you want attempt logic when the player reaches a spot.
- **Prefab:** `Assets/Prefabs/Interactables TileMap Version/Area Trigger.prefab`  
- **Script:** `Assets/Scripts/Trigger/AreaTrigger.cs`

Resize the trigger box to cover the desired zone; wire **On Trigger Enter** to your actions.

---

## 9. Level Patterns

Reusable design patterns you can build with the tools above.

1. **Bait:** Place a coin or safe-looking platform (e.g. Fake Platform) that leads into a trap or pit. First run: player takes the bait and dies; later runs: they learn to avoid or use Attempt Trigger to change the bait.
2. **Double bluff:** Safe path on attempt 1; on attempt 2, use Attempt Trigger to activate a trap on the “safe” path so the safe path becomes deadly.
3. **Safe-looking kill:** Spike Trap or Kill Player zone with **Activation Chance** 50 so sometimes the “safe” corridor kills; or use Fake Platform over a pit so it looks like a normal platform.
4. **Delayed death:** Disappearing Platform or Gradually Collapsing Floor over a pit; death happens when the platform goes away or collapses, not on first touch.
5. **Attempt gate:** Use Attempt Trigger (Equal or GreaterOrEqual) to open a Wall (**BecomeOpen**) or activate a Moving Trap only from attempt N, so the level layout changes per run.
6. **Rhythm corridor:** Moving Trap or Arrow Trap with Loop + fixed interval; player must time their run between cycles.
7. **One-time safe:** Area Trigger at the start of a section; On Trigger Enter → Trap Sequencer that activates a trap after a delay. First time through they’re safe if they run; second time the trap is already active or triggers again.
8. **Betrayal platform:** Fake Platform in the middle of a jump sequence; first run they trust it and fall when it fades; or use Attempt Trigger so on run 2 the platform is already “used” and disappears on contact.
9. **Crusher run:** Crushing Wall or Crushing Ceiling with Auto Start; player must pass through before being squished; combine with Slippery Floor to make timing harder.
10. **Pop-up surprise:** Floor or Wall AmbushTrap-PopUp triggered by Area Trigger when the player passes a line; or by Attempt Trigger so on attempt 2 a spike block appears in a previously safe spot.

---

## 10. Testing Checklist

Use this before considering a level “done.”

- [ ] **Player tag & layer:** Player object has tag **Player** and a Collider2D (and Rigidbody2D for movement).
- [ ] **Death:** Every lethal trap (Spike, Kill Player, Death Zone, Arrow kill, Ambush, Crushing, etc.) actually kills and triggers restart when you expect.
- [ ] **Kill direction:** For spikes and crushers, test from top/bottom/left/right; only the intended side kills.
- [ ] **RNG:** For traps with **Activation Chance** &lt; 100, run multiple times and confirm they sometimes kill and sometimes don’t (if that’s the intent).
- [ ] **Attempt-based:** If you use Attempt Trigger, confirm **Check()** is called when the level loads (or from your bootstrap). Test attempt 1 vs 2 vs 3 and confirm behavior matches (e.g. spikes only on attempt 2).
- [ ] **Sequences:** Trap Sequencer steps run in order and with the right delays; no missing **StartSequence()** call.
- [ ] **Buttons/Switches:** Contact Layer includes the player; On Pressed / On Turn On correctly trigger doors or traps.
- [ ] **Goal:** Reaching the Goal freezes the player and loads the next level (or loops).
- [ ] **Pits:** Death Zone (or Kill Player) covers the bottom of pits and kills when the player falls.
- [ ] **Fake platforms/walls:** Fake Platform fades and causes death when intended; Fake Wall opens/closes when events fire.
- [ ] **Mobile:** If building for mobile, test with on-screen controls (MobileMoveButton, MobileJumpButton) and confirm traps and goal work the same.

---

## 11. How to Create a New Trap Prefab Type (Developers)

This section describes the architecture so you can add a new trap type that fits the existing systems.

### Base class: TrapBase

- **Path:** `Assets/Scripts/Core/TrapBase.cs`
- All traps that participate in the shared systems **inherit from TrapBase** and implement **Activate()**.
- **TrapBase** provides:
  - **Lethality:** **isLethal**, **killDirection** (AnySide, OnlyTop, OnlyBelow, OnlyLeft, OnlyRight).
  - **RNG:** **activationChance** (0–100); **ShouldActivate()** returns true/false for use in **Activate()** and collision/trigger handlers.
  - **Mutations:** **changesPlayerData**, **mutations** (list of **PlayerMutation**); **ApplyMutationsToPlayer()** / **RevertMutationsFromPlayer()**.
- Override **OnCollisionEnter2D** / **OnTriggerEnter2D** as needed; call **base** for mutation behavior; use **CanKillFromCollision(collision)** / **CanKillFromTrigger(playerCol, trapCol)** and then **KillPlayer(gameObject)** (implement **KillPlayer** locally: set player inactive, call **GameManager.Instance.GameOver()**).

### Lethality pattern

- **KillPlayer** in each trap: `player.SetActive(false); GameManager.Instance.GameOver();`
- Use **CanKillFromCollision** / **CanKillFromTrigger** so direction and **isLethal** are respected; avoid killing when **!ShouldActivate()** if you want RNG to apply.

### Trigger system

- **AttemptTrigger** holds condition + target value; exposes **onConditionMet** / **onConditionFailed** UnityEvents. Some script or bootstrap must call **AttemptTrigger.Check()** (e.g. on level load or when the player enters a zone).
- **TrapSequencer** runs a list of (delay, UnityEvent); something must call **StartSequence()** (e.g. Area Trigger, Attempt Trigger, or Button).
- **AreaTrigger** and **GoalTrigger** fire once on player enter; wire their events to **Activate()**, **StartSequence()**, or **Check()** as needed.

### Creating a new trap type

1. Add a new script in `Assets/Scripts/Trap Types/` that **inherits from TrapBase**.
2. Implement **public override void Activate()**: call **if (!ShouldActivate()) return;** then run your logic (e.g. start moving, spawn projectile, set state).
3. If the trap can kill on contact, override **OnCollisionEnter2D** and/or **OnTriggerEnter2D**: after **base** (if you want mutations), if **collision.gameObject.CompareTag("Player")** and **CanKillFromCollision(collision)** (or **CanKillFromTrigger**), call your **KillPlayer(player)** (player.SetActive(false); GameManager.Instance.GameOver();).
4. Expose any designer-facing fields (speeds, offsets, delays) with **SerializeField** and **Header**/ **Tooltip**.
5. Create a prefab: add the script, Collider2D (trigger or not as needed), and optional Rigidbody2D. Place the prefab under `Assets/Prefabs/Level Builder Tools/Traps/` in the right category folder.
6. Optional: add a custom editor in `Assets/Scripts/Editor/` (see **TrapBaseEditor.cs**) to draw gizmos or improve the Inspector.

### Interfaces

- There is no formal interface in the repo; the “contract” is **TrapBase** + **Activate()**. Buttons, Attempt Trigger, and Trap Sequencer call **Activate()** on any TrapBase-derived component via UnityEvent (drag the trap GameObject and choose **Activate** from the list).

### ScriptableObjects

- **PlayerData** (`Assets/Resources/PlayerData.asset`) holds default movement/jump stats. Traps that change behavior use **PlayerMutation** (stat type + value + optional duration); **PlayerController** applies/reverts these. You don’t need a ScriptableObject for a new trap unless you want shared config (e.g. arrow prefab, damage profiles).

---

**End of handbook.** For project structure and game flow, see **Docs/DOCUMENTATION_PROJECT.md**.
