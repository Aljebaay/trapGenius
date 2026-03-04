# Level 024 – Open Sesame

## Target Expectation
E-05 – This path is blocked / this path is open

## Patterns Used
Primary: P-005 – Attempt Gate
Support: P-012 – Arrow Rhythm Gate

## Player Expectation
This path is blocked or this path is open.

## Betrayal
From attempt 2 the blocked path opens; the route beyond may have timing hazards.

## Expected First Death
Attempt 1: wall blocks. Attempt 2+: gate opens; player may be hit by arrows in the support section if they mistime the rhythm.

## Implementation Notes
- AttemptTrigger
- FakeWall
- ArrowTrap (support: Auto Start, Loop)
- Goal
- On Condition Met → FakeWall BecomeOpen(). Condition: GreaterOrEqual 2. Call AttemptTrigger.Check() on level load. Beyond gate: arrow rhythm.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_024 | synced: 2026-03-04 -->
