# Trap Design Patterns

This document defines reusable psychological trap patterns for Devilbait. Each pattern is built from systems and prefabs described in **TRAP_LIBRARY.md**. Use **PATTERN_TAGS.md** for tagging and **PATTERN_COMBINATIONS.md** for combining patterns.

---

## Pattern Template

Use this structure when defining or documenting a new pattern:

| Field | Description |
|-------|-------------|
| **Pattern ID** | `P-###` (unique identifier) |
| **Name** | Short, memorable pattern name |
| **Goal** | What the designer wants the player to experience |
| **Player Expectation** | What the player believes before the betrayal |
| **Betrayal Mechanism** | How that expectation is subverted |
| **Setup** | Level geometry and trap placement |
| **Truth** | What is actually true (the rule after learning) |
| **Implementation** | Prefabs, scripts, and wiring (from TRAP_LIBRARY) |
| **Tags** | See PATTERN_TAGS.md |

---

## Initial Patterns (P-001 – P-012)

---

### P-001 — Fake Bridge

| Field | Content |
|-------|---------|
| **Pattern ID** | P-001 |
| **Name** | Fake Bridge |
| **Goal** | Player trusts a visible shortcut over a gap and dies when it fails. |
| **Player Expectation** | The platform is solid and will hold them. |
| **Betrayal Mechanism** | Platform fades on contact; player falls into death zone. |
| **Setup** | One or more Fake Platforms over a pit; Death Zone below. Goal reachable via longer “safe” route or after learning. |
| **Truth** | The shortcut platform is never safe; only the alternate path or timed crossing works. |
| **Implementation** | **FakePlatform** over **Death Zone**. Fake Platform prefab: `Assets/Prefabs/Level Builder Tools/Traps/Floor (10)/Fake Platform.prefab`. Ensure pit is covered by Death Zone. |
| **Tags** | psychological, bait, betrayal, environmental |

---

### P-002 — Double Bluff Path

| Field | Content |
|-------|---------|
| **Pattern ID** | P-002 |
| **Name** | Double Bluff Path |
| **Goal** | First run: one path is safe. Later runs: that path becomes lethal. |
| **Player Expectation** | “The path I used last time is still safe.” |
| **Betrayal Mechanism** | Attempt Trigger activates a trap on the previously safe route from attempt 2 onward. |
| **Setup** | Two routes to goal. Route A safe on attempt 1. Attempt Trigger (Equal 2 or GreaterOrEqual 2) → On Condition Met → enable Spike Trap or Ambush on Route A. |
| **Truth** | From attempt 2, only Route B is safe; Route A is deadly. |
| **Implementation** | **AttemptTrigger** (Condition Equal or GreaterOrEqual, Target 2). On Condition Met → **Spike Trap** or **AmbushTrap-PopUp** Activate(), or enable trap GameObject. Ensure **Check()** is called on level load. |
| **Tags** | psychological, expectation-reversal, attempt-based, sequence |

---

### P-003 — Safe-Looking Kill (RNG)

| Field | Content |
|-------|---------|
| **Pattern ID** | P-003 |
| **Name** | Safe-Looking Kill |
| **Goal** | A corridor or platform looks safe but sometimes kills. |
| **Player Expectation** | “This path is safe” or “I made it last time.” |
| **Betrayal Mechanism** | Trap has Activation Chance &lt; 100; sometimes it kills, sometimes it doesn’t. Or Fake Platform over pit. |
| **Setup** | Narrow path with Spike Trap / Kill Player (Activation Chance 50) or Fake Platform over Death Zone. |
| **Truth** | The path is never fully trustworthy; player must treat it as risky or avoid it. |
| **Implementation** | **Spike Trap** or **Kill Player** with **Activation Chance** 50 (TrapBase). Or **Fake Platform** over **Death Zone**. |
| **Tags** | psychological, bait, rng, direct-lethal |

---

### P-004 — Delayed Death Platform

| Field | Content |
|-------|---------|
| **Pattern ID** | P-004 |
| **Name** | Delayed Death Platform |
| **Goal** | Death occurs after a delay (platform disappears or collapses), not on first touch. |
| **Player Expectation** | “I have time to cross” or “It will hold for one more step.” |
| **Betrayal Mechanism** | Platform vanishes or collapses after a short delay; player falls into pit if still on it. |
| **Setup** | Disappearing Platform or Gradually Collapsing Floor over Death Zone. Optional Trap Sequencer to trigger collapse. |
| **Truth** | The platform is time-limited; hesitation or backtracking causes death. |
| **Implementation** | **DisappearingPlatform** (Delayed or Instantly Disappearing) or **BreakingPlatform** (Gradually Collapsing floor) over **Death Zone**. Set Delay / Time Before Break. Prefabs in `Floor (10)/`. |
| **Tags** | timing, delayed-death, environmental, movement |

---

### P-005 — Attempt Gate (Wall Opens)

| Field | Content |
|-------|---------|
| **Pattern ID** | P-005 |
| **Name** | Attempt Gate |
| **Goal** | Level layout changes by attempt; a blocked path opens (or closes) on later runs. |
| **Player Expectation** | “This path is blocked” or “This path is open.” |
| **Betrayal Mechanism** | Attempt Trigger opens a Fake Wall or activates a Moving Wall / TriggeredMove so the geometry changes. |
| **Setup** | Fake Wall (Solid at start) or Moving Wall (idle). Attempt Trigger (e.g. Equal 2) → BecomeOpen() or Activate(). |
| **Truth** | From attempt N, the blocked path is open (or the open path is closed). |
| **Implementation** | **AttemptTrigger** → On Condition Met → **Fake Wall** BecomeOpen() / BecomeGhost(), or **MovingTrap** / **TriggeredMoveTrap** Activate(). Fake Wall: `Walls (7)/Fake Wall.prefab`. |
| **Tags** | attempt-based, expectation-reversal, sequence, environmental |

---

### P-006 — Rhythm Corridor

| Field | Content |
|-------|---------|
| **Pattern ID** | P-006 |
| **Name** | Rhythm Corridor |
| **Goal** | Player must time movement between periodic hazards. |
| **Player Expectation** | “I can run through anytime.” |
| **Betrayal Mechanism** | Moving Trap or Arrow Trap with Loop and fixed interval; running at the wrong time kills. |
| **Setup** | Narrow passage with Moving Wall/Floor/Ceiling or Arrow Trap. Auto Start / Loop On, consistent cycle. |
| **Truth** | There is a safe window each cycle; passing requires reading the rhythm. |
| **Implementation** | **MovingTrap** (Moving Floor/Ceiling/Wall) with **Auto Move** On, ping-pong; or **ArrowTrap** with **Auto Start** On, **Loop** On, **Cycle Interval** set. |
| **Tags** | timing, rhythm, movement, direct-lethal |

---

### P-007 — One-Time Safe

| Field | Content |
|-------|---------|
| **Pattern ID** | P-007 |
| **Name** | One-Time Safe |
| **Goal** | First time through a section is safe if the player runs; second time the trap is already active or retriggers. |
| **Player Expectation** | “This section is safe” (after first pass). |
| **Betrayal Mechanism** | Area Trigger starts a Trap Sequencer that activates a trap after a delay. First pass: player exits before it fires. On retry, they may re-enter and the sequence runs again (or trap stays active). |
| **Setup** | Area Trigger at section start → On Trigger Enter → Trap Sequencer StartSequence(). Sequencer: Delay → Ambush or Spike Activate(). |
| **Truth** | The section is only safe for a short window; lingering or re-entering gets you killed. |
| **Implementation** | **Area Trigger** → **Trap Sequencer** StartSequence(). Sequencer steps: e.g. Delay 1.0 → **AmbushTrap-Falling** or **AmbushTrap-PopUp** Activate(). |
| **Tags** | sequence, ambush, timing, expectation-reversal |

---

### P-008 — Betrayal Platform (Attempt)

| Field | Content |
|-------|---------|
| **Pattern ID** | P-008 |
| **Name** | Betrayal Platform (Attempt) |
| **Goal** | First run: platform is solid. Second run: it’s “already used” and fades on contact. |
| **Player Expectation** | “The platform will hold again.” |
| **Betrayal Mechanism** | Attempt Trigger (GreaterOrEqual 2) Activate()s the Fake Platform so it’s pre-triggered and fades as soon as the player touches it on run 2+. |
| **Setup** | Fake Platform in the middle of a jump sequence over a pit. Attempt Trigger (GreaterOrEqual, 2) → On Condition Met → Fake Platform Activate(). |
| **Truth** | From attempt 2, the platform betrays on contact; player must skip it or use a different route. |
| **Implementation** | **FakePlatform** + **AttemptTrigger** (GreaterOrEqual, 2) → On Condition Met → Fake Platform **Activate()**. Fake Platform over **Death Zone**. |
| **Tags** | psychological, bait, betrayal, attempt-based |

---

### P-009 — Crusher Run

| Field | Content |
|-------|---------|
| **Pattern ID** | P-009 |
| **Name** | Crusher Run |
| **Goal** | Player must pass through a closing space before being squished. |
| **Player Expectation** | “I have time” or “I can wait.” |
| **Betrayal Mechanism** | Crushing Wall/Ceiling/Floor with Auto Start; hesitation or wrong timing = squish. Optional Slippery Floor to reduce control. |
| **Setup** | Crushing Wall or Crushing Ceiling/Floor with solid geometry on the opposite side. Auto Start On. Optional Slippery Floor in the run. |
| **Truth** | The crusher cycle is fixed; only passing in the safe window works. |
| **Implementation** | **CrushingWall** (or Crushing Ceiling/Floor). Set **Move Offset**, **Crush Layer**, **Auto Start** On. Prefabs in `Ceiling (6)/`, `Floor (10)/`, `Walls (7)/`. |
| **Tags** | timing, movement, environmental, direct-lethal |

---

### P-010 — Pop-Up Surprise

| Field | Content |
|-------|---------|
| **Pattern ID** | P-010 |
| **Name** | Pop-Up Surprise |
| **Goal** | A previously safe spot becomes lethal when the player passes a line or on a specific attempt. |
| **Player Expectation** | “This tile is safe.” |
| **Betrayal Mechanism** | Floor/Wall/Ceiling AmbushTrap-PopUp triggered by Area Trigger (pass a line) or Attempt Trigger (attempt 2+). |
| **Setup** | AmbushTrap-PopUp at choke point or landing spot. Area Trigger → Activate() or Attempt Trigger (Equal 2) → Activate(). |
| **Truth** | The spot is only safe when the ambush isn’t triggered; from attempt N or after crossing the line, it’s deadly. |
| **Implementation** | **AmbushTrap-PopUp** (Floor/Ceiling/Wall). **Area Trigger** or **AttemptTrigger** → On Trigger Enter / On Condition Met → **Activate()**. Set Pop Offset and Pop Speed. |
| **Tags** | ambush, surprise, expectation-reversal, attempt-based |

---

### P-011 — Warning Then Drop

| Field | Content |
|-------|---------|
| **Pattern ID** | P-011 |
| **Name** | Warning Then Drop |
| **Goal** | A short warning (or none) then a ceiling block drops. Player who lingers dies. |
| **Player Expectation** | “I’m safe under here.” |
| **Betrayal Mechanism** | Trap Sequencer: delay then Ceiling AmbushTrap-Falling Activate(). Triggered by Area Trigger when player enters zone. |
| **Setup** | Area Trigger over corridor → Trap Sequencer StartSequence(). Step 1: optional “warning” (e.g. light). Step 2: Delay ~1 s → Ceiling AmbushTrap-Falling Activate(). |
| **Truth** | The ceiling is deadly after a fixed delay from entering; must clear the zone quickly. |
| **Implementation** | **Area Trigger** → **Trap Sequencer** StartSequence(). Step: Delay 1.0 → **Ceiling AmbushTrap-Falling** Activate(). AmbushTrap prefab: `Ceiling (6)/Ceiling AmbushTrap-Falling.prefab`. |
| **Tags** | sequence, ambush, timing, delayed-death |

---

### P-012 — Arrow Rhythm Gate

| Field | Content |
|-------|---------|
| **Pattern ID** | P-012 |
| **Name** | Arrow Rhythm Gate |
| **Goal** | Player must cross a corridor between arrow bursts. |
| **Player Expectation** | “I can run through.” |
| **Betrayal Mechanism** | Arrow Trap with Auto Start and Loop; fixed fire interval. Wrong timing = hit. |
| **Setup** | Arrow Trap (FreeZone or Wall/Ceiling) facing the path. Auto Start On, Loop On, Cycle Interval set. Narrow passage so player must time the gap. |
| **Truth** | Safe crossing only between bursts; rhythm must be read. |
| **Implementation** | **ArrowTrap**. **Auto Start** On, **Loop** On, **Arrows Per Burst** / **Cycle Interval** / **Fire Rate** as needed. Rotate so projectiles cross the path. Prefabs: `FreeZone (6)/ArrowTrap.prefab`, or Ceiling/Wall projectile variants. |
| **Tags** | timing, rhythm, direct-lethal, movement |

---

## Reference

- Trap prefabs, scripts, and behaviors: **TRAP_LIBRARY.md**
- Tag definitions and categories: **PATTERN_TAGS.md**
- Combining patterns: **PATTERN_COMBINATIONS.md**
