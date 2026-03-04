# Level 025 – Betrayal and Flow

## Target Expectation
E-08 – The platform will hold again (on retry)

## Patterns Used
Primary: P-008 – Betrayal Platform (Attempt)
Support: P-006 – Rhythm Corridor

## Player Expectation
The platform will hold again.

## Betrayal
From attempt 2 the platform betrays on contact; must skip or use a different route.

## Expected First Death
Attempt 2: player steps on the betrayal platform and falls. Or they mistime the MovingTrap on the alternate route.

## Implementation Notes
- FakePlatform
- AttemptTrigger
- DeathZone
- MovingTrap (support: Auto Move, ping-pong)
- Goal
- AttemptTrigger (GreaterOrEqual 2) → FakePlatform Activate(). Alternate route has rhythm corridor. Call AttemptTrigger.Check() on level load.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..4 and x=8..16; gap x=5..7. Alternate route with rhythm section.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (14,0)
- FakePlatform: (5,0) over gap; AttemptTrigger GreaterOrEqual 2 → Activate(); Check() on load. DeathZone below
- MovingTrap (support): alternate route; Auto Move On, ping-pong

<!-- log-sync: level=LEVEL_025 primary=P-008 expectation=E-08 updated=2026-03-04 -->
