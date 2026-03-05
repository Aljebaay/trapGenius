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

Prefabs (Drag & Drop + Unpack Completely)
- Player: (0,1)
- Goal: (5,1)
- Fake Platform: (3,1)
- Death Zone: (3,-2)

Notes
- Fake Platform should visually cover the gap (x=5..7). Resize/duplicate if needed.
- Death Zone should cover the pit area below the gap. Resize collider to cover x=5..7.

<!-- log-sync: level=LEVEL_001 primary=P-001 expectation=E-01 updated=2026-03-04 -->
