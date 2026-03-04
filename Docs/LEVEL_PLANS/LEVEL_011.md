# Level 011 – No Return

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
Player lingers in the zone or re-enters; AreaTrigger starts TrapSequencer and the ambush activates. Or they mistime the arrow rhythm in the support section.

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
- Ground: y=0, x=0..16; corridor section x=5..10 (zone).
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- AreaTrigger: at section start (e.g. x=5); On Trigger Enter → TrapSequencer StartSequence()
- TrapSequencer: one step Delay ~1s → AmbushTrap-Falling or AmbushTrap-PopUp or SpikeTrap Activate()
- ArrowTrap (support): later in path; Auto Start On, Loop On

<!-- log-sync: level=LEVEL_011 primary=P-007 expectation=E-07 updated=2026-03-04 -->
