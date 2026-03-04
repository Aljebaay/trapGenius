# Level 004 – Trust No Corridor

## Target Expectation
E-03 – This corridor / platform is safe (no visible hazard)

## Patterns Used
Primary: P-003 – Safe-Looking Kill
Support: (none)

## Player Expectation
This path is safe or I made it last time.

## Betrayal
Activation Chance &lt; 100 or fake platform; the path is never fully trustworthy.

## Expected First Death
Player enters the corridor or steps on the safe-looking platform; SpikeTrap or KillPlayer triggers (or FakePlatform gives way) and they die.

## Implementation Notes
- SpikeTrap (Activation Chance 50) or KillPlayer or FakePlatform
- DeathZone (if using FakePlatform over pit)
- Goal

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; corridor.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- SpikeTrap or KillPlayer: in corridor, e.g. (6,0); Activation Chance 50 (or FakePlatform over DeathZone)

<!-- log-sync: level=LEVEL_004 primary=P-003 expectation=E-03 updated=2026-03-04 -->
