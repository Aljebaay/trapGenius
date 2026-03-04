# Pattern Combinations

This document describes how trap patterns from **TRAP_PATTERNS.md** can be combined to create more advanced level design. Focus is on **psychological deception** and **expectation reversal** while keeping levels short (5–10 second solved run) and fail-fast.

---

## Purpose

- **Scale difficulty** by layering patterns instead of lengthening levels.
- **Deepen deception** by teaching one rule then subverting it with a second pattern.
- **Preserve clarity** so each death still teaches a single, attributable lesson where possible.

---

## Design Rules for Combining

1. **One primary betrayal per micro-level** — the “main” trick should be clear; secondary patterns support or extend it.
2. **Order matters** — often: teach (first pattern) → reverse (second pattern). Example: safe path on attempt 1, then that path becomes lethal (double bluff) and a rhythm gate appears on the alternate route.
3. **Avoid same-tag overload** — don’t stack two pure “bait” patterns in the same 5–10 second run unless the second is a deliberate twist on the first.
4. **Keep implementation realistic** — use only systems and prefabs from **TRAP_LIBRARY.md** (AttemptTrigger, TrapSequencer, FakePlatform, ArrowTrap, etc.).
5. **Test attempt progression** — verify attempt 1, 2, 3+ each have a clear intended outcome (e.g. attempt 1: learn bait; attempt 2: learn reversal; attempt 3: execute).

---

## Combination Catalog

Each combination lists: **Patterns used**, **Psychological goal**, **Setup summary**, and **Implementation notes**. Pattern IDs refer to **TRAP_PATTERNS.md**.

---

### COMBO-01 — Bait Then Gate

| Field | Content |
|-------|---------|
| **Patterns** | P-001 (Fake Bridge) + P-005 (Attempt Gate) |
| **Psychological goal** | Player learns the fake bridge is deadly, then discovers the “blocked” route opens on a later attempt. |
| **Setup** | Fake Platform over pit as obvious shortcut; Fake Wall or blocked path as alternate route. Attempt Trigger (e.g. Equal 2) → Fake Wall BecomeOpen(). First run: player takes bait and dies. Second run: they avoid the bridge and find the gate open. |
| **Implementation** | Fake Platform over Death Zone. Fake Wall (Solid) with Attempt Trigger (Equal, 2) → On Condition Met → BecomeOpen(). Goal beyond the gate. Ensure AttemptTrigger.Check() on level load. |

---

### COMBO-02 — Double Bluff + Rhythm Gate

| Field | Content |
|-------|---------|
| **Patterns** | P-002 (Double Bluff Path) + P-006 (Rhythm Corridor) |
| **Psychological goal** | First run: one path is safe. Second run: that path is lethal; the only escape is through a timing challenge the player hasn’t learned yet. |
| **Setup** | Two routes. Route A safe attempt 1; Attempt Trigger (GreaterOrEqual, 2) activates trap on Route A. Route B contains a Moving Trap or Arrow Trap (rhythm). Player must switch route and then learn the rhythm. |
| **Implementation** | Attempt Trigger (GreaterOrEqual, 2) → Spike or Ambush on Route A. Route B: MovingTrap or ArrowTrap, Auto Start + Loop. Goal at end of Route B. |

---

### COMBO-03 — Delayed Death + One-Time Safe

| Field | Content |
|-------|---------|
| **Patterns** | P-004 (Delayed Death Platform) + P-007 (One-Time Safe) |
| **Psychological goal** | Player crosses a platform that disappears after a delay, then enters a “safe” corridor that is only safe if they keep moving; sequencer triggers ambush behind them. |
| **Setup** | Disappearing Platform over pit (must cross quickly). Beyond it, Area Trigger starts Trap Sequencer that activates an Ambush (e.g. falling block) after a delay. First pass: they run through and are safe. Retry: they may hesitate on the platform and die, or re-enter and get hit by the delayed ambush. |
| **Implementation** | Disappearing Platform (delay ~1–1.5 s) over Death Zone. Area Trigger → Trap Sequencer (Delay → AmbushTrap-Falling Activate()). Place Ambush so it blocks or kills in the corridor. |

---

### COMBO-04 — Betrayal Platform + Pop-Up Surprise

| Field | Content |
|-------|---------|
| **Patterns** | P-008 (Betrayal Platform) + P-010 (Pop-Up Surprise) |
| **Psychological goal** | First run: middle platform holds, player reaches the far side. Second run: that platform betrays; if they try to skip it, a pop-up blocks or kills the alternate path. |
| **Setup** | Jump sequence: platform A (real), platform B (Fake, attempt-betrayal), platform C (real). Attempt Trigger (GreaterOrEqual, 2) → Fake Platform B Activate(). Near the “skip” path (e.g. one-tile gap), Floor or Wall AmbushTrap-PopUp triggered by Area Trigger or Attempt Trigger so the “safe” skip becomes lethal on attempt 2. |
| **Implementation** | Fake Platform B over pit; Attempt Trigger (GreaterOrEqual, 2) → Fake Platform Activate(). AmbushTrap-PopUp with Attempt Trigger (Equal, 2) or Area Trigger → Activate(). |

---

### COMBO-05 — Safe-Looking Kill + Attempt Gate

| Field | Content |
|-------|---------|
| **Patterns** | P-003 (Safe-Looking Kill) + P-005 (Attempt Gate) |
| **Psychological goal** | One path is probabilistically lethal (RNG or fake platform); the other is blocked. On a later attempt the blocked path opens, so the player must switch and trust the “new” route. |
| **Setup** | Corridor A: Spike Trap Activation Chance 50 or Fake Platform over pit. Corridor B: Fake Wall (Solid). Attempt Trigger (Equal, 2) → Fake Wall BecomeOpen(). Goal past B. Player learns A is unreliable, then discovers B opens. |
| **Implementation** | Spike Trap or Fake Platform + Death Zone on Route A. Fake Wall + Attempt Trigger (Equal, 2) → BecomeOpen() on Route B. |

---

### COMBO-06 — Crusher Run + Slippery Floor

| Field | Content |
|-------|---------|
| **Patterns** | P-009 (Crusher Run) + timing/sliding (Slippery Floor from TRAP_LIBRARY) |
| **Psychological goal** | Player expects to control their run through the crusher; the slippery floor removes fine control and forces commitment to the rhythm. |
| **Setup** | Slippery Floor leading into or inside a Crushing Wall/Ceiling corridor. Crusher Auto Start, ping-pong. Player must time the approach and not overshoot or get stuck. |
| **Implementation** | Slippery Floor prefab before/inside crusher zone. Crushing Wall or Crushing Ceiling with Move Offset, Crush Layer, Auto Start On. No new pattern ID; uses P-009 plus environmental tuning. |

---

### COMBO-07 — Warning Then Drop + Double Bluff

| Field | Content |
|-------|---------|
| **Patterns** | P-011 (Warning Then Drop) + P-002 (Double Bluff Path) |
| **Psychological goal** | First run: path is safe. Second run: the “safe” path gets a delayed drop when the player enters; they must take the other path or run through before the drop. |
| **Setup** | Two routes. Route A: Area Trigger → Trap Sequencer → Ceiling AmbushTrap-Falling (only from attempt 2). Use Attempt Trigger (GreaterOrEqual, 2) to enable the sequencer or the trap. Route B: safe but perhaps longer or less obvious. |
| **Implementation** | Attempt Trigger (GreaterOrEqual, 2) → enable a GameObject that contains Area Trigger + Trap Sequencer, or wire Attempt Trigger to StartSequence() on a sequencer that runs Delay → Ambush Activate(). Route A = path under the ambush. |

---

### COMBO-08 — Arrow Rhythm Gate + Fake Bridge

| Field | Content |
|-------|---------|
| **Patterns** | P-012 (Arrow Rhythm Gate) + P-001 (Fake Bridge) |
| **Psychological goal** | Obvious shortcut is a fake bridge; the “hard” route is a rhythm corridor. Player learns to avoid the bait and then master the rhythm. |
| **Setup** | Fake Platform over pit in front of goal; Death Zone below. Alternate path: Arrow Trap with Loop. Player tries shortcut, dies; then must time the arrows. |
| **Implementation** | Fake Platform over Death Zone. Arrow Trap (Auto Start, Loop) covering the only other path to the goal. |

---

### COMBO-09 — One-Time Safe + Delayed Death

| Field | Content |
|-------|---------|
| **Patterns** | P-007 (One-Time Safe) + P-004 (Delayed Death Platform) |
| **Psychological goal** | Player crosses a delayed-death platform, then enters a zone that triggers a delayed ambush. They must clear both within time windows; hesitation on either is fatal. |
| **Setup** | Disappearing Platform (short delay) over pit, then Area Trigger → Trap Sequencer (delay → Ambush). If they slow down on the platform they die; if they pass but linger in the zone they get hit. |
| **Implementation** | Disappearing Platform over Death Zone. Area Trigger after it → Trap Sequencer (e.g. Delay 1.5 → AmbushTrap-PopUp or Falling Activate()). |

---

### COMBO-10 — Attempt Gate + Pop-Up Surprise

| Field | Content |
|-------|---------|
| **Patterns** | P-005 (Attempt Gate) + P-010 (Pop-Up Surprise) |
| **Psychological goal** | On attempt 1 the gate is closed and the player sees no danger. On attempt 2 the gate opens but a pop-up appears in the newly opened path, so the “reward” for dying is a new route that is also trapped. |
| **Setup** | Fake Wall (Solid). Attempt Trigger (Equal, 2) → BecomeOpen(). Just beyond the wall, AmbushTrap-PopUp triggered by same Attempt Trigger or by Area Trigger when player passes through. |
| **Implementation** | Fake Wall; Attempt Trigger (Equal, 2) → BecomeOpen(). AmbushTrap-PopUp inside the gate; trigger Activate() from same Attempt Trigger On Condition Met or from Area Trigger placed in the new path. |

---

### COMBO-11 — Double Bluff + Safe-Looking Kill

| Field | Content |
|-------|---------|
| **Patterns** | P-002 (Double Bluff Path) + P-003 (Safe-Looking Kill) |
| **Psychological goal** | First run: path A is safe. Second run: path A gets a trap. The “safe” path B looks safe but uses RNG (Activation Chance 50) or a fake platform so that even the “correct” choice can kill sometimes. |
| **Setup** | Route A: Attempt Trigger (GreaterOrEqual, 2) → activate trap. Route B: Spike Trap Activation Chance 50 or Fake Platform over pit. Goal past B. Player learns to avoid A, then discovers B is not fully reliable. |
| **Implementation** | Attempt Trigger on Route A. Route B: Spike Trap or Kill Player (Activation Chance 50) or Fake Platform over Death Zone. |

---

### COMBO-12 — Betrayal Platform + Rhythm Corridor

| Field | Content |
|-------|---------|
| **Patterns** | P-008 (Betrayal Platform) + P-006 (Rhythm Corridor) |
| **Psychological goal** | First run: player uses the middle platform and succeeds. Second run: that platform betrays; the only alternative is a moving wall or arrow corridor they must time. |
| **Setup** | Three platforms: A (real), B (Fake, attempt-betrayal from attempt 2), C (real). Attempt Trigger (GreaterOrEqual, 2) → Fake Platform B Activate(). Alternate path: Moving Trap or Arrow Trap with Loop. Goal after rhythm section. |
| **Implementation** | Fake Platform B + Attempt Trigger (GreaterOrEqual, 2) → Activate(). Alternate route uses MovingTrap or ArrowTrap (Auto Start, Loop). |

---

## Summary Table

| Combo ID | Primary patterns | Main deception |
|----------|------------------|----------------|
| COMBO-01 | P-001, P-005 | Bait then gate opens |
| COMBO-02 | P-002, P-006 | Safe path becomes lethal; rhythm on alternate |
| COMBO-03 | P-004, P-007 | Delayed platform + delayed ambush in “safe” zone |
| COMBO-04 | P-008, P-010 | Betrayal platform + pop-up on skip route |
| COMBO-05 | P-003, P-005 | RNG/safe-looking path + gate opens later |
| COMBO-06 | P-009 + Slippery | Crusher + loss of control |
| COMBO-07 | P-011, P-002 | Delayed drop on “safe” path from attempt 2 |
| COMBO-08 | P-012, P-001 | Fake bridge + arrow rhythm as only real route |
| COMBO-09 | P-007, P-004 | One-time safe zone + delayed death platform |
| COMBO-10 | P-005, P-010 | Gate opens but pop-up in new path |
| COMBO-11 | P-002, P-003 | Double bluff + RNG on “correct” path |
| COMBO-12 | P-008, P-006 | Betrayal platform + rhythm as alternate |

---

## Reference

- Pattern definitions: **TRAP_PATTERNS.md**
- Tag system: **PATTERN_TAGS.md**
- Trap prefabs and wiring: **TRAP_LIBRARY.md**
