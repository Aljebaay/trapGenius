# Level Testing Checklist

## Purpose
Provide a standardized QA checklist for validating Devilbait micro-levels, with emphasis on deception clarity, fail-fast behavior, and attempt-based logic.

## Design Rules
- Test every level in at least three passes:
  - blind first run,
  - informed retry run,
  - consistency/stability run.
- Validate both **player experience** and **technical behavior**.
- Confirm the core loop is preserved: quick death, quick reset, quick relearn.
- Require clarity of betrayal: death should communicate a reason the player can act on next attempt.
- Ensure solved execution fits the 5-10 second design target.
- Verify attempt-based reversals and sequencer timings are deterministic unless randomness is intentional.

## Examples
- **Blind pass:** tester dies to first deception, then can explain what changed.
- **Retry pass:** tester applies learned response and reaches deeper into the level.
- **Consistency pass:** behavior remains stable across multiple attempts (except intended RNG variance).
- **Betrayal quality pass:** fake-safe element causes meaningful surprise but not unexplained failure.

## Implementation Notes
- Use this checklist before marking a level complete:
  - [ ] Player spawn and goal are valid.
  - [ ] All lethal traps kill as intended.
  - [ ] Kill direction is correct on directional hazards.
  - [ ] Attempt Trigger conditions fire on intended attempts.
  - [ ] Trap Sequencer actions fire in order with expected delays.
  - [ ] Fake Platform/Fake Wall behaviors match level intent.
  - [ ] No soft-locks; invalid states resolve by death/reset.
  - [ ] Solved route is consistently 5-10 seconds.
  - [ ] Death-to-retry transition feels immediate.
  - [ ] Mobile/PC parity is acceptable when relevant.
- Base technical expectations on `Docs/10_TRAPS/TRAP_LIBRARY.md` (Testing Checklist + trap behaviors) and project flow in `Docs/00_OVERVIEW/PROJECT_OVERVIEW.md`.
