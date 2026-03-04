# Fail Fast Design

## Purpose
Codify layout and interaction rules that keep Devilbait deaths immediate, retries frictionless, and learning momentum high.

## Design Rules
- A failed attempt should end quickly and restart quickly.
- Place lethal outcomes close to trap triggers so cause and effect are obvious.
- Minimize downtime:
  - short distance from spawn to first decision,
  - no long non-interactive travel,
  - no waiting-heavy setups before first hazard.
- Use short encounter chains that preserve tension for 5-10 seconds.
- Keep checkpoint logic simple; most micro-levels should rely on full instant retry.
- Avoid soft-fail states (stuck but not dead). If the run is invalid, kill fast and reset.
- Use clear readability cues for post-death learning (where betrayal occurred, what state changed, what timing was wrong).

## Examples
- **Good fail-fast:** fake platform directly above a pit with instant death zone and immediate restart.
- **Bad fail-fast:** hidden trap after 20 seconds of traversal with no learning signal.
- **Good retry loop:** attempt-triggered spike appears near spawn, player tests adaptation within seconds.
- **Good invalid-state handling:** crusher corridor kills on missed timing instead of trapping player indefinitely.

## Implementation Notes
- Align with the flow in `Docs/00_OVERVIEW/PROJECT_OVERVIEW.md` (death -> GameOver -> restart).
- Use trap behaviors from `Docs/10_TRAPS/TRAP_LIBRARY.md` that naturally support quick resolution:
  - Death Zone / Kill Player for definitive failure states.
  - Disappearing or collapsing floors for immediate consequence.
  - Sequenced traps with short delays to preserve tempo.
- QA target: from death to next controllable attempt should feel near-instant on target platforms.
