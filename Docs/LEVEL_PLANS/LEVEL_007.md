# Level 007 – Pulse

## Target Expectation
E-06 – I can run through anytime

## Patterns Used
Primary: P-006 – Rhythm Corridor
Support: (none)

## Player Expectation
I can run through anytime.

## Betrayal
There is a safe window each cycle; wrong timing kills.

## Expected First Death
Player enters the corridor and is hit by the MovingTrap (floor/ceiling/wall) because they did not time the gap.

## Implementation Notes
- MovingTrap
- Goal
- Auto Move On, ping-pong. Narrow passage so player must time the gap.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; narrow corridor x=5..9.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- MovingTrap: in corridor (e.g. floor/ceiling at 6,0); Auto Move On, ping-pong; Local Target Offset 1/2, Speed set

<!-- log-sync: level=LEVEL_007 primary=P-006 expectation=E-06 updated=2026-03-04 -->
