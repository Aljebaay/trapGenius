# Level 027 – Pop and Drop

## Target Expectation
E-10 – This tile / spot is safe

## Patterns Used
Primary: P-010 – Pop-Up Surprise
Support: P-004 – Delayed Death Platform

## Player Expectation
This tile is safe.

## Betrayal
The spot is only safe when the ambush is not triggered; from attempt 2 or after crossing a line it is deadly.

## Expected First Death
Player triggers the AmbushTrap-PopUp on the tile or line. Or they hesitate on a delayed-death platform later and fall.

## Implementation Notes
- AmbushTrap-PopUp
- AreaTrigger or AttemptTrigger
- DeathZone
- DisappearingPlatform or BreakingPlatform (support)
- Goal
- Primary: pop-up on tile/line. Support: platform over pit with delay.
- Support is mechanical only (timing/movement), not a second deception.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..16; tile/choke for pop-up then gap with delayed platform.
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (15,0)
- AmbushTrap-PopUp: at choke; AreaTrigger or AttemptTrigger → Activate()
- DeathZone + DisappearingPlatform or BreakingPlatform (support): over pit; Delay or Time Before Break

<!-- log-sync: level=LEVEL_027 primary=P-010 expectation=E-10 updated=2026-03-04 -->
