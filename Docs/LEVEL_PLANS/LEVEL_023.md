# Level 023 – Crush and Collapse

## Target Expectation
E-09 – I have time / I can wait (in a corridor)

## Patterns Used
Primary: P-009 – Crusher Run
Support: P-004 – Delayed Death Platform

## Player Expectation
I have time or I can wait in the corridor.

## Betrayal
The crusher cycle is fixed; only passing in the safe window works.

## Expected First Death
Player is squished by the CrushingWall or, after the crusher, hesitates on a delayed-death platform and falls into the DeathZone.

## Implementation Notes
- CrushingWall
- DeathZone
- DisappearingPlatform or BreakingPlatform (support)
- Goal
- CrushingWall: Move Offset, Crush Layer, Auto Start On. Support section: platform over pit with delay.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; crusher corridor x=5..9; then gap with pit for delayed platform.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- CrushingWall: corridor; Move Offset, Crush Layer, Auto Start On
- DeathZone + DisappearingPlatform or BreakingPlatform (support): after crusher; Delay or Time Before Break

<!-- log-sync: level=LEVEL_023 primary=P-009 expectation=E-09 updated=2026-03-04 -->
