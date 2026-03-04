# Level 012 – Short Cut

## Target Expectation
E-01 – Solid platforms hold

## Patterns Used
Primary: P-001 – Fake Bridge
Support: P-006 – Rhythm Corridor

## Player Expectation
The platform over the gap is solid and will hold me.

## Betrayal
The platform fades on contact; only the pit below is real.

## Expected First Death
Player takes the shortcut onto the fake platform; it fades and they fall into the DeathZone. Or they mistime the MovingTrap in the alternate route.

## Implementation Notes
- FakePlatform
- DeathZone
- MovingTrap (support: Auto Move, ping-pong)
- Goal
- Alternate route includes rhythm corridor; shortcut is fake bridge.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..4 and x=8..16; gap x=5..7 (shortcut). Alternate route x=5..14 along upper or side path with rhythm section.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (14,0)
- FakePlatform: (5,0) over gap; DeathZone below
- MovingTrap (support): on alternate route; Auto Move On, ping-pong

<!-- log-sync: level=LEVEL_012 primary=P-001 expectation=E-01 updated=2026-03-04 -->
