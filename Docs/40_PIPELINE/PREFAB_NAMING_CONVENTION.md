# Prefab Naming Convention

## Purpose
Establish clear, scalable naming standards for level-design prefabs and scene objects used in Devilbait documentation and production workflows.

## Design Rules
- Use consistent, descriptive names that encode function and behavior.
- Prefer this format for new trap-prefab variants:  
  `TRP_<Category>_<Mechanic>_<Variant>`
- Prefer this format for trigger objects:  
  `TRG_<Type>_<ConditionOrTarget>`
- Prefer this format for sequence controllers:  
  `SEQ_<Intent>_<Index>`
- Use uppercase prefixes to aid hierarchy scanning:
  - `TRP` trap,
  - `TRG` trigger,
  - `SEQ` sequencer,
  - `GEO` structural geometry,
  - `OBJ` interactable objective.
- Avoid ambiguous names such as `Trap1`, `NewPrefab`, `TestObject`.
- Keep names stable after wiring UnityEvents to reduce broken references during iteration.

## Examples
- `TRP_Floor_FakePlatform_MainBridge`
- `TRP_Wall_AmbushPopUp_RightLane`
- `TRG_Attempt_Equal2_LeftRouteSwap`
- `SEQ_ExitBetrayal_01`
- `OBJ_Goal_Final`
- `GEO_Pit_A01`

## Implementation Notes
- Apply naming conventions to new production content; do not rename established legacy assets unless part of a planned migration.
- For design review, verify naming clarity before QA handoff.
- Keep naming aligned with trap categories in `Docs/10_TRAPS/TRAP_LIBRARY.md` and workflow expectations in `Docs/40_PIPELINE/LEVEL_BUILDER_WORKFLOW.md`.
