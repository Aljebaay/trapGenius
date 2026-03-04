# Level 025 – Betrayal and Flow

## Target Expectation
E-08 – The platform will hold again (on retry)

## Patterns Used
Primary: P-008 – Betrayal Platform (Attempt)
Support: P-006 – Rhythm Corridor

## Player Expectation
The platform will hold again.

## Betrayal
From attempt 2 the platform betrays on contact; must skip or use a different route.

## Expected First Death
Attempt 2: player steps on the betrayal platform and falls. Or they mistime the MovingTrap on the alternate route.

## Implementation Notes
- FakePlatform
- AttemptTrigger
- DeathZone
- MovingTrap (support: Auto Move, ping-pong)
- Goal
- AttemptTrigger (GreaterOrEqual 2) → FakePlatform Activate(). Alternate route has rhythm corridor. Call AttemptTrigger.Check() on level load.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_025 | synced: 2026-03-04 -->
