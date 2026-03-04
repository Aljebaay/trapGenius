# Level 008 – Second Thoughts

## Target Expectation
E-08 – The platform will hold again (on retry)

## Patterns Used
Primary: P-008 – Betrayal Platform (Attempt)
Support: (none)

## Player Expectation
The platform will hold again.

## Betrayal
From attempt 2 the platform betrays on contact; must skip it or use a different route.

## Expected First Death
Attempt 1: platform holds and player reaches the other side. Attempt 2: they step on the same platform and it fades immediately; they fall into the DeathZone.

## Implementation Notes
- FakePlatform
- AttemptTrigger
- DeathZone
- Goal
- AttemptTrigger (GreaterOrEqual 2) → On Condition Met → FakePlatform Activate(). Platform over DeathZone. Call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_008 | synced: 2026-03-04 -->
