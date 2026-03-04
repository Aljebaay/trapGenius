# Level 014 – Double Cross

## Target Expectation
E-02 – Path I used last run is still safe

## Patterns Used
Primary: P-002 – Double Bluff Path
Support: (none)

## Player Expectation
The path I used last time is still safe.

## Betrayal
From attempt 2, that path has a trap; only the other route is safe.

## Expected First Death
On attempt 2, player reuses the previously safe path and is killed by the newly active SpikeTrap or AmbushTrap-PopUp.

## Implementation Notes
- AttemptTrigger
- SpikeTrap or AmbushTrap-PopUp
- Goal
- Two routes. Condition: GreaterOrEqual 2. Call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; two routes (left and right) to goal.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (13,0)
- AttemptTrigger: Condition GreaterOrEqual 2 → trap Activate(). Call Check() on load.
- SpikeTrap or AmbushTrap-PopUp: on one route (e.g. left); activated by AttemptTrigger

<!-- log-sync: level=LEVEL_014 primary=P-002 expectation=E-02 updated=2026-03-04 -->
