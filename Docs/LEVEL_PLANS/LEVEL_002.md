# Level 002 – Hold Your Nerve

## Target Expectation
E-04 – I have time to cross / platform holds one more step

## Patterns Used
Primary: P-004 – Delayed Death Platform
Support: (none)

## Player Expectation
I have time to cross or the platform will hold for one more step.

## Betrayal
The platform vanishes or collapses after a short delay; hesitation causes death.

## Expected First Death
Player stands or hesitates on the disappearing or collapsing platform; when the delay ends they fall into the DeathZone.

## Implementation Notes
- DeathZone
- DisappearingPlatform or BreakingPlatform
- Goal

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14 with gap x=5..7; pit below.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- DisappearingPlatform or BreakingPlatform: (5,0) over pit; set Delay or Time Before Break
- DeathZone: under pit x=5..7

<!-- log-sync: level=LEVEL_002 primary=P-004 expectation=E-04 updated=2026-03-04 -->
