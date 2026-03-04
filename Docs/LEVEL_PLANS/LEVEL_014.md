# Level 014 – Double Cross

## Target Expectation
E-02 – Path I used last run is still safe

## Patterns Used
Primary: P-002 – Double Bluff Path
Support: (none)

## Player Expectation
The path I used last time is still safe.

## Betrayal
From attempt 2, that path has a trap; only the other route is safe.

## Expected First Death
On attempt 2, player reuses the previously safe path and is killed by the newly active SpikeTrap or AmbushTrap-PopUp.

## Implementation Notes
- AttemptTrigger
- SpikeTrap or AmbushTrap-PopUp
- Goal
- Two routes. Condition: GreaterOrEqual 2. Call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_014 | synced: 2026-03-04 -->
