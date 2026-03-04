# Level 029 – Ceiling and Current

## Target Expectation
E-11 – I am safe under this ceiling / in this zone

## Patterns Used
Primary: P-011 – Warning Then Drop
Support: P-006 – Rhythm Corridor

## Player Expectation
I am safe under this ceiling or in this zone.

## Betrayal
The ceiling is deadly after a fixed delay from entering; must clear the zone quickly.

## Expected First Death
Player lingers under the ceiling and the AmbushTrap-Falling kills them. Or they mistime the MovingTrap in the next section.

## Implementation Notes
- AreaTrigger
- TrapSequencer
- AmbushTrap-Falling
- MovingTrap (support: Auto Move, ping-pong)
- Goal
- AreaTrigger → TrapSequencer; Delay → AmbushTrap-Falling Activate(). Then rhythm corridor.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; ceiling corridor x=5..10 then rhythm section.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- AreaTrigger: corridor start → TrapSequencer; Delay → AmbushTrap-Falling Activate()
- MovingTrap (support): rhythm section; Auto Move On, ping-pong

<!-- log-sync: level=LEVEL_029 primary=P-011 expectation=E-11 updated=2026-03-04 -->
