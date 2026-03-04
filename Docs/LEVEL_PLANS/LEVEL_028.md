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

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; zone section x=5..10 then arrow section.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- AreaTrigger: zone start → TrapSequencer StartSequence(); step Delay → Ambush/Spike Activate()
- ArrowTrap (support): later; Auto Start On, Loop On

<!-- log-sync: level=LEVEL_028 primary=P-007 expectation=E-07 updated=2026-03-04 -->
