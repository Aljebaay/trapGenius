# Level 005 – Arrow Alley

## Target Expectation
E-06 – I can run through anytime

## Patterns Used
Primary: P-012 – Arrow Rhythm Gate
Support: (none)

## Player Expectation
I can run through anytime.

## Betrayal
Safe crossing only between bursts; the rhythm must be read.

## Expected First Death
Player runs through the corridor at the wrong time and is hit by an arrow from the ArrowTrap.

## Implementation Notes
- ArrowTrap
- Goal
- Auto Start On, Loop On, Cycle Interval set. Rotate so projectiles cross the path.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; narrow corridor through x=5..8.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- ArrowTrap: position (6,1) or wall variant; rotate so arrows cross path. Auto Start On, Loop On, Cycle Interval set

<!-- log-sync: level=LEVEL_005 primary=P-012 expectation=E-06 updated=2026-03-04 -->
