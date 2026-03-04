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

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; corridor x=5..10 under ceiling.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (13,0)
- AreaTrigger: corridor start (e.g. x=5); On Trigger Enter → TrapSequencer StartSequence()
- TrapSequencer: step Delay ~1.0s → AmbushTrap-Falling Activate()
- AmbushTrap-Falling: above corridor

<!-- log-sync: level=LEVEL_015 primary=P-011 expectation=E-11 updated=2026-03-04 -->
