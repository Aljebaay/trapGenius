# Generator Rules — Psychological Trap Engine

Non-random, psychologically-smart level plans using **PATTERNS.json**. One primary betrayal per level; teach before betray.

---

## Purpose

- Output **level plans** (pattern IDs + placement) that a builder or generator can implement without touching code.
- Enforce **one primary betrayal**, **5–10 s solved run**, and **fairness/readability** so each death teaches one clear lesson.
- Use **expectation state** (see **EXPECTATION_STATE.md**) to pick which expectation to teach and betray.

---

## Core Constraints

1. **Solved run:** 5–10 seconds from spawn to goal when the player knows the solution.
2. **One primary betrayal:** Exactly one main deception per level; support patterns only aid it.
3. **Fairness/readability:** Every death attributable after one retry (player can name what killed them and why).
4. **Pattern source:** Patterns from **PATTERNS.json** (P-001–P-012) only; prefabs/systems from **TRAP_LIBRARY.md**.

---

## Selection Algorithm

Run these steps in order to produce one level plan.

### Step A — Choose target expectation to teach (and betray)

- From **EXPECTATION_STATE.md**, pick one expectation (E-01–E-12) that:
  - Has been **taught** in prior levels or can be taught in this level’s setup, and
  - You will **betray** with the primary pattern.
- For early levels (L1–10), prefer expectations that need minimal prior teaching (e.g. E-01 solid platforms, E-03 corridor safe).
- For mid/late, use expectations already established (e.g. E-02 path still safe, E-05 path blocked/open).

### Step B — Choose pattern to betray it

- From **PATTERNS.json**, select one pattern whose **expectation.betray** matches the chosen expectation.
- Use **primary_tag** and **segment_fit** to match level index (intro early, mid/end later).
- This pattern is the **primary pattern**; its ID is the main pattern for the level.

### Step C — Choose one support pattern (timing/movement) if needed

- Optional. Add at most one **support** pattern that is timing or movement only (e.g. P-006 Rhythm Corridor, P-009 Crusher Run, P-012 Arrow Rhythm Gate) to add pressure **without** a second betrayal.
- Support must not introduce a new primary psychological twist (no second bait, reversal, or betrayal).
- Skip support for L1–10; consider for L11+ when difficulty allows.

### Step D — Validate cooldown rules

- **Pattern cooldown:** Primary pattern ID must not appear in the **last 5 levels**. If it does, pick another pattern or skip this level slot.
- **Primary psychological cooldown:** The primary pattern’s **primary_tag** (if psychological: bait, betrayal, expectation-reversal, psychological, rng) must not repeat in the **last 2 levels**. If it does, pick another primary pattern.

### Step E — Output Level Plan

- Emit:
  - **Level title** (short, descriptive).
  - **Pattern IDs** in order: [primary] then [support] if any.
  - **Target expectation** (E-XX).
  - **Expected first death point** (where and why).
  - **Implementation notes** (which prefabs/triggers; reference TRAP_LIBRARY.md).

---

## Anti-Repetition

| Rule | Value |
|------|--------|
| Same pattern (by ID) as primary | Do not repeat within **last 5 levels**. |
| Same primary psychological concept (primary_tag) | Do not repeat within **last 2 levels**. Psychological concepts: bait, betrayal, expectation-reversal, psychological, rng. |

---

## Difficulty Scaling by Level Index

| Level range | Patterns | Notes |
|-------------|----------|--------|
| **L1–10** | 1 pattern | Low difficulty (1–2). Single primary pattern only. Intro/mid segment_fit. Teach one expectation, betray once. |
| **L11–30** | 2 patterns | Primary + optional support (timing/movement). Add attempt-based (P-002, P-005, P-008, P-010) sometimes. Still 1 primary betrayal. |
| **L31+** | 2–3 patterns | Combos allowed (e.g. primary + 1–2 support). Support can be timing/rhythm/ambush. Still **exactly 1 primary betrayal** per level. |

---

## Example Generated Level Plans

Each uses P-001–P-012 and expectations E-01–E-12. Prefabs/triggers from **TRAP_LIBRARY.md**.

---

### Example 1 — First Steps

| Field | Content |
|-------|---------|
| **Level title** | First Steps |
| **Pattern IDs** | [P-001] |
| **Target expectation** | E-01 (solid platforms hold) |
| **Expected first death point** | Player steps on fake platform over pit; platform fades, fall into DeathZone. |
| **Implementation notes** | FakePlatform over DeathZone. Goal on far side. No AttemptTrigger. Pit fully covered by DeathZone. |

---

### Example 2 — Wrong Turn

| Field | Content |
|-------|---------|
| **Level title** | Wrong Turn |
| **Pattern IDs** | [P-002] |
| **Target expectation** | E-02 (path I used last run is still safe) |
| **Expected first death point** | Attempt 2: player takes same (left) path; SpikeTrap or AmbushTrap-PopUp now active, kills. |
| **Implementation notes** | Two routes to goal. AttemptTrigger (GreaterOrEqual, 2) → On Condition Met → SpikeTrap or AmbushTrap-PopUp Activate() on left route. Call AttemptTrigger.Check() on level load. Right route always safe. |

---

### Example 3 — Trust Fall

| Field | Content |
|-------|---------|
| **Level title** | Trust Fall |
| **Pattern IDs** | [P-004] |
| **Target expectation** | E-04 (I have time to cross / platform holds one more step) |
| **Expected first death point** | Player stands on disappearing or collapsing platform; delay ends, platform goes away, fall into DeathZone. |
| **Implementation notes** | DisappearingPlatform or BreakingPlatform over DeathZone. Set Delay or Time Before Break. Prefabs in Floor (10). |

---

### Example 4 — Gate and Run

| Field | Content |
|-------|---------|
| **Level title** | Gate and Run |
| **Pattern IDs** | [P-005, P-006] |
| **Target expectation** | E-05 (path blocked/open) + support: E-06 (run through anytime) |
| **Expected first death point** | Attempt 1: wall blocks, player may hit MovingTrap if wrong timing. Attempt 2+: gate open but rhythm corridor can kill if mistimed. |
| **Implementation notes** | AttemptTrigger (GreaterOrEqual, 2) → FakeWall BecomeOpen(). Beyond gate, MovingTrap (Auto Move, ping-pong) as support. One primary betrayal (gate); rhythm is support only. |

---

### Example 5 — Ceiling Drop

| Field | Content |
|-------|---------|
| **Level title** | Ceiling Drop |
| **Pattern IDs** | [P-011] |
| **Target expectation** | E-11 (safe under ceiling / in this zone) |
| **Expected first death point** | Player enters corridor and lingers; AreaTrigger → TrapSequencer → after delay Ceiling AmbushTrap-Falling Activate(), crush. |
| **Implementation notes** | AreaTrigger at corridor start → TrapSequencer StartSequence(). Step: Delay ~1.0 s → AmbushTrap-Falling Activate(). Goal past corridor. Clear zone quickly to survive. |

---

## Reference

- **PATTERNS.json** — pattern IDs, primary_tag, expectation, requires/optional.
- **EXPECTATION_STATE.md** — E-01–E-12, teach/betray/reinforce, update rules.
- **TRAP_LIBRARY.md** — prefabs and triggers.
