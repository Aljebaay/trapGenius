# Player Learning Loop

## Purpose
Define the intended fail-fast learning cycle so Devilbait levels feel punishing but teachable, with immediate retries and rapid mastery.

## Design Rules
- Design every level around this loop: **observe -> fail -> infer -> retry -> adapt -> clear**.
- Keep failure-to-retry time extremely short; minimize dead time between death and next attempt.
- Deliver one clear lesson per death whenever possible.
- Ensure lethal cause is attributable (player can identify what killed them and why).
- Reward adaptation immediately; once the player applies the lesson, success chance should increase sharply.
- Use attempt progression to scaffold learning:
  - early attempts reveal rules,
  - later attempts test whether the rule was internalized.
- Keep completion targets aligned with micro-level pacing (5-10 second solved run).

## Examples
- **Observation death:** player trusts a fake bridge and falls.
- **Inference retry:** player notices fade timing and chooses an alternate jump on the next attempt.
- **Adaptation check:** an attempt-triggered trap appears later; player pauses, reads timing, then passes.
- **Mastery clear:** player chains moves with confidence and clears without hesitation.

## Implementation Notes
- Use the attempt-based architecture described in `Docs/00_OVERVIEW/PROJECT_OVERVIEW.md` to support iterative learning.
- Use trap combinations from `Docs/10_TRAPS/TRAP_LIBRARY.md` to create teach-test loops:
  - bait trap (teaches),
  - timing trap (tests),
  - sequenced trap (confirms mastery under pressure).
- QA should verify that first death teaches, second attempt adapts, and solved runs remain within 5-10 seconds.
