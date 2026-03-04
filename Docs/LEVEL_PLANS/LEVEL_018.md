# Level 018 – Used Goods

## Target Expectation
E-08 – The platform will hold again (on retry)

## Patterns Used
Primary: P-008 – Betrayal Platform (Attempt)
Support: (none)

## Player Expectation
The platform will hold again.

## Betrayal
From attempt 2 the platform betrays on contact; must skip or use a different route.

## Expected First Death
Attempt 2: player steps on the platform that held on attempt 1; it fades immediately and they fall into the DeathZone.

## Implementation Notes
- FakePlatform
- AttemptTrigger
- DeathZone
- Goal
- AttemptTrigger (GreaterOrEqual 2) → On Condition Met → FakePlatform Activate(). Call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..4 and x=8..14; gap x=5..7.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- FakePlatform: (5,0) over gap. AttemptTrigger GreaterOrEqual 2 → FakePlatform Activate(); Check() on load.
- DeathZone: under gap

<!-- log-sync: level=LEVEL_018 primary=P-008 expectation=E-08 updated=2026-03-04 -->
