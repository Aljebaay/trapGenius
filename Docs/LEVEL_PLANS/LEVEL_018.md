# Level 018 – Used Goods

## Target Expectation
E-08 – The platform will hold again (on retry)

## Patterns Used
Primary: P-008 – Betrayal Platform (Attempt)
Support: (none)

## Player Expectation
The platform will hold again.

## Betrayal
From attempt 2 the platform betrays on contact; must skip or use a different route.

## Expected First Death
Attempt 2: player steps on the platform that held on attempt 1; it fades immediately and they fall into the DeathZone.

## Implementation Notes
- FakePlatform
- AttemptTrigger
- DeathZone
- Goal
- AttemptTrigger (GreaterOrEqual 2) → On Condition Met → FakePlatform Activate(). Call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_018 | synced: 2026-03-04 -->
