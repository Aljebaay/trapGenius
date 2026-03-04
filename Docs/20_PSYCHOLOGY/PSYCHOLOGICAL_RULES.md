# Psychological Rules

## Purpose
Define the core psychological design rules for Devilbait so every level feels deceptive, readable, and replayable within a short fail-fast loop.

## Design Rules
- Build around **trust then betrayal**: show a safe-looking pattern, then subvert it with a trap.
- Keep each micro-level solvable in **5-10 seconds** when the player knows the answer.
- Use **one main deception idea per micro-level**; avoid stacking unrelated tricks in the same 5-10 second window.
- Ensure every betrayal is **learnable**: the player should understand the cause of death after one retry.
- Prefer attempt-based deception over random unfairness; use **Attempt Trigger** for deterministic reversals.
- Use RNG (`Activation Chance`) only when the level still teaches a stable rule (for example, "this corridor is never fully trustworthy").
- Pair psychological bait with clear consequences: fake safety should route into a visible lethal outcome (pit, spikes, crusher, projectile lane).
- Respect rhythm: deception should happen at decisive moments (jump commitment, landing, corner turn, or "goal almost reached").

## Examples
- **Fake Platform betrayal:** a platform appears safe, then fades and drops the player into a Death Zone.
- **Double bluff pathing:** path A is safe on attempt 1, then path A becomes lethal on attempt 2 through Attempt Trigger.
- **False relief:** after surviving one trap, a delayed sequencer triggers a second trap where the player expects recovery.
- **Comfort break trap:** two safe runs establish trust, then an ambush appears on the same route from attempt 3 onward.

## Implementation Notes
- Source trap behaviors from `Docs/10_TRAPS/TRAP_LIBRARY.md` (especially Psychological Bait, Level Patterns, and Testing Checklist).
- Use the project loop in `Docs/00_OVERVIEW/PROJECT_OVERVIEW.md` to align with attempt-based progression.
- Prefer these trap-system building blocks:
  - `TrapBase` for lethality, kill direction, and activation chance.
  - `AttemptTrigger` for expectation reversal by death count.
  - `TrapSequencer` for delayed betrayal and rhythm control.
- During review, ask: "What belief does this level create, and exactly where does it betray that belief?"
