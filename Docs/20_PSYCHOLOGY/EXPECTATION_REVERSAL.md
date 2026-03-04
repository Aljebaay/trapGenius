# Expectation Reversal

## Purpose
Standardize how to design "gotcha" moments that flip player expectations while staying fair, readable, and fast to retry.

## Design Rules
- Establish a clear expectation first (safe path, stable platform, predictable timing), then reverse it once.
- The reversal should occur after player commitment, not before decision points, to maximize emotional impact.
- Keep reversal windows short: reveal, betray, reset, retry in under 10 seconds.
- Use attempt-indexed logic for controlled reversals:
  - Attempt 1: teach baseline.
  - Attempt 2+: invert baseline.
- Apply reversal to one variable at a time (space, timing, or state), not all at once.
- Use audiovisual clarity before lethal resolution where possible (movement, fade, activation animation, warning rhythm).
- Maintain consistency after reveal: once the player learns the reversal, subsequent attempts should reward adaptation.

## Examples
- **State reversal:** Fake Wall starts solid, then becomes ghost/open on a later attempt, changing the "correct" route.
- **Safety reversal:** a known safe ledge becomes a fake platform only after the first death.
- **Timing reversal:** a moving hazard interval is offset by a sequencer, catching players who reuse previous timing.
- **Goal reversal:** the final jump zone is safe early, then receives a delayed ambush on later attempts.

## Implementation Notes
- Implement reversals with these patterns from `Docs/10_TRAPS/TRAP_LIBRARY.md`:
  - Attempt Trigger (`Equal`, `Greater`, `Modulo`) for state switching.
  - Trap Sequencer for delayed or chained reversals.
  - Fake Platform and Fake Wall for trust-break mechanics.
- Keep reversals deterministic in onboarding content; introduce RNG only after the player understands the pattern language.
- Validate reversal fairness using `Docs/50_QA/LEVEL_TESTING_CHECKLIST.md`.
