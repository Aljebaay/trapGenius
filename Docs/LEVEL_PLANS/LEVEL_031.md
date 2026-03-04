# Level 031 – Squeeze Play

## Target Expectation
E-09 – I have time / I can wait (in a corridor)

## Patterns Used
Primary: P-009 – Crusher Run
Support: (none)

## Player Expectation
I have time or I can wait in the corridor.

## Betrayal
The crusher cycle is fixed; only passing in the safe window works.

## Expected First Death
Player hesitates or times the crusher wrong and is squished by the CrushingWall between the moving hazard and the opposing geometry.

## Implementation Notes
- CrushingWall
- Goal
- Set Move Offset, Crush Layer, Auto Start On. Optional: SlipperyFloor. Solid geometry (ground/wall) on opposite side for squish detection.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; corridor x=5..10 with solid wall or ceiling for crusher to close against.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- CrushingWall: in corridor (e.g. ceiling or wall at 7,1); Move Offset toward opposing geometry, Crush Layer set, Auto Start On, Ping Pong On

<!-- log-sync: level=LEVEL_031 primary=P-009 expectation=E-09 updated=2026-03-04 -->
