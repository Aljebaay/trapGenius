# Level 010 – Step Lightly

## Target Expectation
E-10 – This tile / spot is safe

## Patterns Used
Primary: P-010 – Pop-Up Surprise
Support: (none)

## Player Expectation
This tile is safe.

## Betrayal
The spot is only safe when the ambush is not triggered; from attempt 2 or after crossing a line it is deadly.

## Expected First Death
Player lands on the tile or crosses the trigger line; AmbushTrap-PopUp activates and kills them.

## Implementation Notes
- AmbushTrap-PopUp
- AreaTrigger or AttemptTrigger
- Goal
- AreaTrigger or AttemptTrigger → Activate(). Set Pop Offset and Pop Speed. Condition if attempt-based: GreaterOrEqual 2; call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

## Build Blueprint (Hybrid)
Terrain (Tilemap)
- Ground: y=0, x=0..14; one tile or choke at e.g. (6,0).
Prefabs (Drag & Drop + Unpack Completely)
- PlayerSpawn: (1,0)
- Goal: (12,0)
- AmbushTrap-PopUp: at (6,0) or choke; AreaTrigger or AttemptTrigger → Activate(). Pop Offset, Pop Speed set. If attempt-based: GreaterOrEqual 2, Check() on load

<!-- log-sync: level=LEVEL_010 primary=P-010 expectation=E-10 updated=2026-03-04 -->
