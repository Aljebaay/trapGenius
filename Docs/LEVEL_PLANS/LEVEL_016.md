# Level 016 – Lucky Corridor

## Target Expectation
E-03 – This corridor / platform is safe (no visible hazard)

## Patterns Used
Primary: P-003 – Safe-Looking Kill
Support: (none)

## Player Expectation
This path is safe or I made it last time.

## Betrayal
The path is never fully trustworthy; RNG or fake platform kills.

## Expected First Death
Player crosses the corridor or platform; SpikeTrap or KillPlayer with Activation Chance 50 triggers (or FakePlatform gives way) and they die.

## Implementation Notes
- SpikeTrap or KillPlayer or FakePlatform
- DeathZone (if FakePlatform over pit)
- Goal
- TrapBase Activation Chance 50 for SpikeTrap/KillPlayer.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; corridor.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- SpikeTrap or KillPlayer: in corridor, e.g. (6,0); Activation Chance 50. Or FakePlatform over DeathZone

<!-- log-sync: level=LEVEL_016 primary=P-003 expectation=E-03 updated=2026-03-04 -->
