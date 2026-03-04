# Level 015 – Look Up

## Target Expectation
E-11 – I am safe under this ceiling / in this zone

## Patterns Used
Primary: P-011 – Warning Then Drop
Support: (none)

## Player Expectation
I am safe under this ceiling or in this zone.

## Betrayal
The ceiling is deadly after a fixed delay from entering; must clear the zone quickly.

## Expected First Death
Player enters the corridor and lingers; AreaTrigger fires TrapSequencer and the ceiling AmbushTrap-Falling drops and kills them.

## Implementation Notes
- AreaTrigger
- TrapSequencer
- AmbushTrap-Falling
- Goal
- AreaTrigger → TrapSequencer StartSequence(); step Delay ~1.0 s → AmbushTrap-Falling Activate().

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_015 | synced: 2026-03-04 -->
