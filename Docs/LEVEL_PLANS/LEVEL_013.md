# Level 013 – Crush Hour

## Target Expectation
E-09 – I have time / I can wait (in a corridor)

## Patterns Used
Primary: P-009 – Crusher Run
Support: P-012 – Arrow Rhythm Gate

## Player Expectation
I have time or I can wait in the corridor.

## Betrayal
The crusher cycle is fixed; only passing in the safe window works.

## Expected First Death
Player hesitates or times the crusher wrong and is squished by the CrushingWall. Or they are hit by arrows in the support section.

## Implementation Notes
- CrushingWall
- ArrowTrap (support: Auto Start, Loop)
- Goal
- Set Move Offset, Crush Layer, Auto Start On. Optional: SlipperyFloor.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; corridor with crusher run x=5..9; ceiling/floor/wall for crush.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- CrushingWall: in corridor; Move Offset, Crush Layer, Auto Start On. Optional SlipperyFloor
- ArrowTrap (support): later; Auto Start On, Loop On

<!-- log-sync: level=LEVEL_013 primary=P-009 expectation=E-09 updated=2026-03-04 -->
