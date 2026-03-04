# Level 006 – The Other Door

## Target Expectation
E-05 – This path is blocked / this path is open

## Patterns Used
Primary: P-005 – Attempt Gate
Support: (none)

## Player Expectation
This path is blocked or this path is open.

## Betrayal
From attempt 2 the blocked path opens (or the open path closes).

## Expected First Death
Attempt 1: player may hit the wall or take the long route. Attempt 2+: they learn the gate opens and can pass (or must avoid the now-closed route).

## Implementation Notes
- AttemptTrigger
- FakeWall
- Goal
- On Condition Met → FakeWall BecomeOpen() or BecomeGhost(). Condition: GreaterOrEqual 2. Call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14. Wall or blocked path at e.g. x=6 until attempt 2.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- AttemptTrigger: Condition GreaterOrEqual 2; On Condition Met → FakeWall BecomeOpen(). Call Check() on load.
- FakeWall: block path at (6,0); Initial State Solid

<!-- log-sync: level=LEVEL_006 primary=P-005 expectation=E-05 updated=2026-03-04 -->
