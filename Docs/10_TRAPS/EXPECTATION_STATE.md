# Expectation State Model

Player expectations (E-01–E-12) for the Devilbait generator. Used to pick **target expectation** and to update state after death/success/repeated deaths. Short, implementable language.

---

## Expectations (E-01–E-12)

For each: **teach** (how player learns it), **betray** (which patterns betray it), **reinforce** (how to keep it learnable).

---

### E-01 — Solid platforms hold

| Field | Content |
|-------|---------|
| **teach** | Early levels with Ground and static platforms; no fake or disappearing platforms. |
| **betray** | P-001 (Fake Bridge), P-008 (Betrayal Platform). Platform fades or betrays on contact/attempt. |
| **reinforce** | Offer clear alternate path or rule: “this platform type is never safe.” Same behavior on retries. |

---

### E-02 — Path I used last run is still safe

| Field | Content |
|-------|---------|
| **teach** | Levels where the same route works multiple times. |
| **betray** | P-002 (Double Bluff Path). AttemptTrigger activates trap on previously safe route. |
| **reinforce** | From attempt 2 onward: “old path deadly; use other path.” No extra surprise on that route. |

---

### E-03 — This corridor / platform is safe (no visible hazard)

| Field | Content |
|-------|---------|
| **teach** | Safe corridors and platforms with no spikes or pits. |
| **betray** | P-003 (Safe-Looking Kill). SpikeTrap/KillPlayer with Activation Chance &lt; 100 or FakePlatform over pit. |
| **reinforce** | “This path is never fully safe” or “avoid it.” RNG: reinforce unreliability, not safety. |

---

### E-04 — I have time to cross / platform holds one more step

| Field | Content |
|-------|---------|
| **teach** | Platforms that don’t disappear or collapse immediately. |
| **betray** | P-004 (Delayed Death Platform). DisappearingPlatform or BreakingPlatform over pit. |
| **reinforce** | “Platform is time-limited; don’t hesitate.” Same delay every retry. |

---

### E-05 — This path is blocked / this path is open

| Field | Content |
|-------|---------|
| **teach** | Walls and gates that don’t change between attempts. |
| **betray** | P-005 (Attempt Gate). FakeWall or MovingTrap/TriggeredMoveTrap change state by attempt. |
| **reinforce** | “From attempt N the layout is X.” No flip-flop on later attempts. |

---

### E-06 — I can run through anytime

| Field | Content |
|-------|---------|
| **teach** | Open corridors with no moving hazards or projectiles. |
| **betray** | P-006 (Rhythm Corridor), P-012 (Arrow Rhythm Gate). MovingTrap or ArrowTrap with Loop. |
| **reinforce** | “Safe window each cycle.” Fixed rhythm on retries. |

---

### E-07 — This section is safe after I passed through once

| Field | Content |
|-------|---------|
| **teach** | Sections that stay safe after first crossing. |
| **betray** | P-007 (One-Time Safe). AreaTrigger → TrapSequencer → ambush after delay. |
| **reinforce** | “Safe only for a short window; don’t linger or re-enter.” Same timing on retries. |

---

### E-08 — The platform will hold again (on retry)

| Field | Content |
|-------|---------|
| **teach** | First run platform holds; player reaches the other side. |
| **betray** | P-008 (Betrayal Platform). AttemptTrigger pre-triggers FakePlatform so it fades on contact from attempt 2. |
| **reinforce** | “From run 2 this platform betrays; skip or use other route.” Stable from attempt 2 onward. |

---

### E-09 — I have time / I can wait (in a corridor)

| Field | Content |
|-------|---------|
| **teach** | Corridors without crushers or closing geometry. |
| **betray** | P-009 (Crusher Run). CrushingWall; hesitation or wrong timing = squish. |
| **reinforce** | “Crusher cycle is fixed; only safe window works.” Same cycle every retry. |

---

### E-10 — This tile / spot is safe

| Field | Content |
|-------|---------|
| **teach** | Tiles and landing spots with no pop-up or falling hazard. |
| **betray** | P-010 (Pop-Up Surprise). AmbushTrap-PopUp via AreaTrigger or AttemptTrigger. |
| **reinforce** | “This spot safe only when ambush not triggered.” Trigger condition consistent on retries. |

---

### E-11 — I am safe under this ceiling / in this zone

| Field | Content |
|-------|---------|
| **teach** | Ceilings and zones that don’t drop or activate traps. |
| **betray** | P-011 (Warning Then Drop). AreaTrigger → TrapSequencer → AmbushTrap-Falling after delay. |
| **reinforce** | “Ceiling kills after fixed delay from entering; clear zone quickly.” Same delay on retries. |

---

### E-12 — Exit / goal is safe to approach

| Field | Content |
|-------|---------|
| **teach** | Goals without last-moment traps. |
| **betray** | P-010 or P-011 near goal; or attempt-based trap on final approach. |
| **reinforce** | “Approach goal with same rules as rest of level.” Trap behavior consistent on retries. |

---

## Update Rules

Apply when updating expectation state (for generator or analytics).

---

### After a death

- **Mark the last expectation** that was violated as **“observed betrayal.”**
- Attribute death to one expectation (E-01–E-12); prefer the one the level was designed to betray.
- Do not remove the expectation globally; narrow it (e.g. “this platform” not “all platforms”).
- Next attempt: level behavior for that segment stays consistent so the player can learn the new rule.

---

### After a successful completion

- **Mark** the expectation that was betrayed and then overcome as **“learned counterplay.”**
- Player has applied the correct rule (e.g. “from attempt 2 use the other path”).
- Generator may reuse or vary that expectation in later levels, respecting cooldowns.

---

### After repeated deaths at same spot

- **Reduce deception:** Prefer simpler or more readable version of the same idea (e.g. longer delay, clearer warning).
- **Add clarity/warning:** Make cause of death obvious (e.g. visible rhythm, clearer alternate path, or stable attempt condition).
- Do not add a second primary betrayal; keep one main lesson per level.

---

## Reference

- **PATTERNS.json** — which pattern betrays which expectation.
- **GENERATOR_RULES.md** — selection algorithm, cooldowns, level plans.
