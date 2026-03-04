# Level Builder Workflow

## Purpose
Define a repeatable production workflow for building Devilbait micro-levels in Unity without changing gameplay code.

## Design Rules
- Build using prefabs and inspector wiring only; avoid script modifications for normal level production.
- Start from a clear level intent:
  - deception concept,
  - expected betrayal point,
  - solved path duration (5-10 seconds).
- Follow a strict creation order:
  - blockout geometry,
  - place primary bait,
  - place lethal consequence,
  - add attempt/sequence logic,
  - run fail-fast test loop.
- Keep hierarchy readable with grouped objects (layout, traps, triggers, goal, debug helpers).
- Use deterministic first-pass setups before adding optional variation (for example, RNG chance).
- Verify each placed trap has intentional kill direction and trigger behavior.

## Examples
- **Workflow A (simple betrayal):**
  1. Block a single jump route.
  2. Add Fake Platform over pit.
  3. Add Goal after alternate route.
  4. Playtest until first death teaches the trick.
- **Workflow B (attempt reversal):**
  1. Build a safe route for attempt 1.
  2. Add Attempt Trigger (Equal 2) to activate ambush trap.
  3. Ensure run 2 requires a route swap.
  4. Validate consistency on attempts 3+.

## Implementation Notes
- Use `Docs/00_OVERVIEW/PROJECT_OVERVIEW.md` (Level Builder Workflow and Triggers sections) as the baseline process.
- Use `Docs/10_TRAPS/TRAP_LIBRARY.md` as the trap placement and behavior reference.
- Suggested production checklist per level:
  - Deception objective stated in one sentence.
  - One primary betrayal mechanic selected.
  - Failure state is immediate and understandable.
  - Solved run consistently lands in 5-10 seconds.
