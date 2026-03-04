# Level 021 – Fork in the Road

## Target Expectation
E-02 – Path I used last run is still safe

## Patterns Used
Primary: P-002 – Double Bluff Path
Support: P-012 – Arrow Rhythm Gate

## Player Expectation
The path I used last time is still safe.

## Betrayal
From attempt 2, that path has a trap; only the other route is safe.

## Expected First Death
Attempt 2: player takes the same path and is killed by the newly active trap. Or they mistime the arrow rhythm on the alternate route.

## Implementation Notes
- AttemptTrigger
- SpikeTrap or AmbushTrap-PopUp
- ArrowTrap (support: Auto Start, Loop)
- Goal
- Two routes; one gets trap from attempt 2. Other route has arrow rhythm. Condition: GreaterOrEqual 2. Call AttemptTrigger.Check() on level load.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_021 | synced: 2026-03-04 -->
