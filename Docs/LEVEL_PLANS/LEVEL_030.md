# Level 030 – Timer and Arrows

## Target Expectation
E-04 – I have time to cross / platform holds one more step

## Patterns Used
Primary: P-004 – Delayed Death Platform
Support: P-012 – Arrow Rhythm Gate

## Player Expectation
I have time to cross or the platform will hold for one more step.

## Betrayal
The platform vanishes or collapses after a short delay; hesitation causes death.

## Expected First Death
Player hesitates on the platform and falls into the DeathZone. Or they mistime the arrow rhythm in the support section after crossing.

## Implementation Notes
- DeathZone
- DisappearingPlatform or BreakingPlatform
- ArrowTrap (support: Auto Start, Loop)
- Goal
- Set Delay or Time Before Break. Support: arrow corridor after platform section.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..4 and x=8..16; gap x=5..7. After platform, arrow corridor.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (14,0)
- DisappearingPlatform or BreakingPlatform: (5,0) over pit; Delay or Time Before Break. DeathZone below
- ArrowTrap (support): after platform section; Auto Start On, Loop On

<!-- log-sync: level=LEVEL_030 primary=P-004 expectation=E-04 updated=2026-03-04 -->
