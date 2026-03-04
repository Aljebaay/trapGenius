# Level Patterns

## Purpose
Provide reusable level blueprints for short, deceptive Devilbait stages that combine psychological bait with reliable technical setup.

## Design Rules
- Each pattern must be completable in **5-10 seconds** after discovery.
- Start with a readable route, then inject one controlled betrayal.
- Use trap combinations that map to known categories: direct lethal, ambush, psychological bait, timing rhythm, sequence.
- Design around commitment points (jump arc, narrow corridor, landing tile, final approach).
- Build patterns to support quick reset and repeated attempts without extra setup.
- Prefer deterministic trigger logic for core patterns; add RNG as a variation layer, not the core identity.

## Examples
- **Bait Bridge**
  - Setup: visible shortcut over pit using Fake Platform.
  - Betrayal: bridge fades on contact and drops player into Death Zone.
  - Adaptation: player routes to a less obvious safe path.
- **Double Bluff Corridor**
  - Setup: left route safe on attempt 1.
  - Betrayal: Attempt Trigger activates spikes on left from attempt 2 onward.
  - Adaptation: player switches to right route.
- **Rhythm Gate**
  - Setup: moving hazard cycles through a narrow opening.
  - Betrayal: sequencer offsets timing after initial death.
  - Adaptation: player waits half-beat before committing.
- **Delayed Relief**
  - Setup: player survives first obstacle and enters "safe" pocket.
  - Betrayal: delayed ambush pops in safe pocket.
  - Adaptation: player keeps momentum and exits before trigger resolves.

## Implementation Notes
- Use pattern references already documented in `Docs/10_TRAPS/TRAP_LIBRARY.md` under Level Patterns.
- Build patterns from approved tools:
  - `AttemptTrigger` for attempt-dependent variants.
  - `TrapSequencer` for timed chain behavior.
  - Fake Platform/Fake Wall for expectation reversal.
- Pattern review checklist:
  - Is the betrayal readable after one death?
  - Is success path consistent once learned?
  - Does solved execution fit 5-10 seconds?
