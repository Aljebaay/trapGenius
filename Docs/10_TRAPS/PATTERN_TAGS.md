# Pattern Tags

This document defines the standardized tag system used by trap patterns in Devilbait. Tags are used in **TRAP_PATTERNS.md** and **PATTERN_COMBINATIONS.md** to classify patterns by design intent and to support search, filtering, and combination design.

---

## Purpose

- **Classification:** Quickly identify what kind of challenge or deception a pattern delivers.
- **Combination design:** Choose complementary tags when stacking patterns (e.g. psychological + timing).
- **Difficulty and pacing:** Map tags to tiers (e.g. ambush + sequence = higher complexity).

---

## Tag Categories

Tags are grouped into six categories. Use 1–3 tags per pattern for clarity.

---

### Psychological

| Tag | Description | When to use |
|-----|-------------|-------------|
| **psychological** | Core intent is to deceive or subvert player belief. | Any pattern where the main payoff is “I thought X, but Y.” |
| **bait** | Offers something that looks beneficial or safe but leads to harm. | Fake platforms, coins over pits, “safe” corridors. |
| **betrayal** | Previously safe element becomes unsafe, or trust is broken. | Fake platform on contact, attempt-based swap of safe/unsafe. |
| **expectation-reversal** | The level or path behaves opposite to what the player assumed. | Double bluff path, attempt gate (blocked → open). |
| **rng** | Outcome partly random (e.g. Activation Chance &lt; 100). | Safe-looking kill 50%, unpredictable trap. |

---

### Timing

| Tag | Description | When to use |
|-----|-------------|-------------|
| **timing** | Success depends on when the player acts, not only where. | Rhythm corridors, crusher runs, delayed platforms. |
| **rhythm** | Fixed, repeating cycle the player must read. | Moving traps, arrow loops, rotating saws. |
| **delayed-death** | Death occurs after a delay (platform vanish, collapse, sequencer). | Disappearing platform, warning-then-drop. |

---

### Movement

| Tag | Description | When to use |
|-----|-------------|-------------|
| **movement** | Challenge is primarily about path, jump, or corridor execution. | Narrow passes, crusher run, slippery floor sections. |
| **commitment** | Player commits to a jump or path and cannot easily reverse. | Mid-air betrayal, one-way corridor. |

---

### Ambush

| Tag | Description | When to use |
|-----|-------------|-------------|
| **ambush** | Danger appears or activates from hidden or idle state. | Pop-up spikes, falling block, triggered trap. |
| **surprise** | First encounter is unexpected; learning comes from death. | Pop-up on area trigger, attempt-triggered spawn. |

---

### Environmental

| Tag | Description | When to use |
|-----|-------------|-------------|
| **environmental** | Uses terrain, pits, walls, or zones as core to the trap. | Fake platform over pit, death zone, fake wall. |
| **spatial** | Layout and geometry are the main source of difficulty. | Two routes, narrow gap, crush corridor. |

---

### Sequence

| Tag | Description | When to use |
|-----|-------------|-------------|
| **sequence** | Multiple steps or triggers in order (Trap Sequencer, Attempt Trigger). | Warning then drop, attempt gate, one-time safe. |
| **attempt-based** | Behavior changes with death/attempt count. | Double bluff, attempt gate, betrayal platform (attempt). |
| **multi-step** | Several linked events (delay → activate → delay → activate). | Sequencer chains, button + door + trap. |
| **direct-lethal** | Immediate or obvious lethal contact (spikes, arrows, crusher, death zone). | Spike trap, arrow hit, crush, pit. |

---

## Tag Summary (Alphabetical)

Use this list for quick reference when tagging patterns:

- **ambush** — danger appears from hidden/idle state  
- **attempt-based** — behavior depends on attempt count  
- **bait** — looks safe/beneficial, leads to harm  
- **betrayal** — trust or previous safety is broken  
- **commitment** — player commits and can’t easily reverse  
- **delayed-death** — death after a delay  
- **direct-lethal** — immediate lethal contact  
- **environmental** — terrain/pits/walls central to trap  
- **expectation-reversal** — reality opposes assumption  
- **movement** — path/jump/corridor execution challenge  
- **multi-step** — linked events in order  
- **psychological** — deception or subversion of belief  
- **rng** — random element in outcome  
- **rhythm** — fixed repeating cycle  
- **sequence** — ordered steps or triggers  
- **spatial** — layout/geometry as main difficulty  
- **surprise** — first encounter unexpected  
- **timing** — when the player acts matters  

---

## Usage Notes

- Prefer **2–3 tags** per pattern: one primary (e.g. psychological), one mechanism (e.g. attempt-based), one outcome type (e.g. direct-lethal) if relevant.
- When combining patterns (see **PATTERN_COMBINATIONS.md**), avoid overloading the same tag (e.g. two “bait” patterns in one micro-level unless intentionally stacking).
- Tags are for design and documentation only; they are not implemented in Unity assets or C# code.
