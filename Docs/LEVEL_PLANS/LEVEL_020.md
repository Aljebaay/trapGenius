# Level 020 – Landmine

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
Player lands on the tile or crosses the trigger; AmbushTrap-PopUp activates and kills them.

## Implementation Notes
- AmbushTrap-PopUp
- AreaTrigger or AttemptTrigger
- Goal
- AreaTrigger or AttemptTrigger → Activate(). Set Pop Offset and Pop Speed. If attempt-based: GreaterOrEqual 2; call AttemptTrigger.Check() on level load.

## Estimated Solve Time
5–10 seconds

<!-- level_plan_log: LEVEL_020 | synced: 2026-03-04 -->
