# Difficulty Curve

## Purpose
Define how challenge should escalate in Devilbait while preserving fairness, readability, and the short retry loop.

## Design Rules
- Scale difficulty through **deception complexity**, not level length.
- Keep solved-run duration in the 5-10 second target at all tiers.
- Use a staged curve:
  - Tier 1: single betrayal, low timing pressure.
  - Tier 2: betrayal plus timing or route switch.
  - Tier 3: chained betrayals with attempt-conditioned variation.
  - Tier 4: high-density psychological pressure with precise execution.
- Increase one axis at a time:
  - spatial precision,
  - timing strictness,
  - expectation reversal depth,
  - multi-step sequence complexity.
- Ensure every new trap interaction appears first in a low-pressure teaching context.
- Avoid difficulty spikes caused by hidden information; if hidden once, reveal and stabilize on retries.

## Examples
- **Tier 1 example:** fake platform over obvious pit with immediate retry.
- **Tier 2 example:** safe path in run 1, attempt-triggered swap on run 2, same movement skill requirement.
- **Tier 3 example:** moving saw corridor plus delayed ambush exit.
- **Tier 4 example:** double bluff plus rhythm gate plus sequenced final betrayal in one micro-run.

## Implementation Notes
- Use trap taxonomy from `Docs/10_TRAPS/TRAP_LIBRARY.md` to classify each level's challenge axis.
- During production, annotate each level concept with:
  - intended tier,
  - primary deception type,
  - expected first-clear attempt range.
- Validate curve quality in playtests by tracking:
  - average retries per level,
  - first-death clarity,
  - mastery time once pattern is understood.
