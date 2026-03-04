# Level 029 – Ceiling and Current

## Target Expectation
E-11 – I am safe under this ceiling / in this zone

## Patterns Used
Primary: P-011 – Warning Then Drop
Support: P-006 – Rhythm Corridor

## Player Expectation
I am safe under this ceiling or in this zone.

## Betrayal
The ceiling is deadly after a fixed delay from entering; must clear the zone quickly.

## Expected First Death
Player lingers under the ceiling and the AmbushTrap-Falling kills them. Or they mistime the MovingTrap in the next section.

## Implementation Notes
- AreaTrigger
- TrapSequencer
- AmbushTrap-Falling
- MovingTrap (support: Auto Move, ping-pong)
- Goal
- AreaTrigger → TrapSequencer; Delay → AmbushTrap-Falling Activate(). Then rhythm corridor.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_029 | synced: 2026-03-04 -->
