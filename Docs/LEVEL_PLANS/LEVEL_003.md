# Level 003 – Wrong Turn

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
On attempt 2, player takes the same (e.g. left) path; SpikeTrap or AmbushTrap-PopUp is now active and kills them.

## Implementation Notes
- AttemptTrigger
- SpikeTrap (or AmbushTrap-PopUp)
- Goal
- Two routes to goal; call AttemptTrigger.Check() on level load. Condition: GreaterOrEqual 2.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_003 | synced: 2026-03-04 -->
