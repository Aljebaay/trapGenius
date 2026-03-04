# Level 001 – First Steps

## Target Expectation
E-01 – Solid platforms hold

## Patterns Used
Primary: P-001 – Fake Bridge
Support: (none)

## Player Expectation
The platform over the gap is solid and will hold me.

## Betrayal
The platform fades on contact; only the pit below is real.

## Expected First Death
Player steps onto the fake platform over the pit; it fades immediately and they fall into the DeathZone.

## Implementation Notes
- FakePlatform
- DeathZone
- Goal

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: solid row y=0, x=0..4 and x=8..14; gap x=5..7 (pit).
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- FakePlatform: position (5,0), span over gap
- DeathZone: under gap, cover x=5..7, y&lt;0

<!-- log-sync: level=LEVEL_001 primary=P-001 expectation=E-01 updated=2026-03-04 -->
