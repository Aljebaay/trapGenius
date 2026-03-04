# Level 026 – Risky Path and Crusher

## Target Expectation
E-03 – This corridor / platform is safe (no visible hazard)

## Patterns Used
Primary: P-003 – Safe-Looking Kill
Support: P-009 – Crusher Run

## Player Expectation
This path is safe or I made it last time.

## Betrayal
The path is never fully trustworthy; RNG or fake platform kills.

## Expected First Death
Player is killed by SpikeTrap/KillPlayer/FakePlatform on the safe-looking path, or they are squished by the CrushingWall in the support section.

## Implementation Notes
- SpikeTrap or KillPlayer or FakePlatform
- DeathZone (if FakePlatform)
- CrushingWall (support)
- Goal
- Primary: unreliable corridor/platform. Support: crusher run section.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; safe-looking corridor then crusher run.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- SpikeTrap or KillPlayer or FakePlatform: in corridor; Activation Chance 50 or over DeathZone
- CrushingWall (support): later section; Move Offset, Crush Layer, Auto Start On

<!-- log-sync: level=LEVEL_026 primary=P-003 expectation=E-03 updated=2026-03-04 -->
