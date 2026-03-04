# Level 028 – One Chance Hall

## Target Expectation
E-07 – This section is safe after I passed through once

## Patterns Used
Primary: P-007 – One-Time Safe
Support: P-012 – Arrow Rhythm Gate

## Player Expectation
This section is safe after I passed through once.

## Betrayal
The section is only safe for a short window; lingering or re-entering gets you killed.

## Expected First Death
Player lingers or re-enters; TrapSequencer activates the ambush. Or they mistime the arrow rhythm in the support section.

## Implementation Notes
- AreaTrigger
- TrapSequencer
- AmbushTrap-Falling or AmbushTrap-PopUp or SpikeTrap
- ArrowTrap (support: Auto Start, Loop)
- Goal
- AreaTrigger → TrapSequencer StartSequence(); step Delay → Ambush/Spike Activate().
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_028 | synced: 2026-03-04 -->
