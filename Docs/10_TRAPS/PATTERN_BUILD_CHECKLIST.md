## Build Rule (Unity)
All trap prefabs are placed via Drag & Drop into the scene and then:
Right Click → Prefab → Unpack Completely
Do not edit the original prefab assets while building levels.

# Pattern Build Checklist

Per-pattern checklist for building levels from **PATTERNS.json** and **TRAP_LIBRARY.md**. Use for fast, mistake-free placement.

---

## P-001 — Fake Bridge

**Required prefabs/systems:** FakePlatform, DeathZone, Goal

**Placement rules:** Intro/mid/end. Place FakePlatform over pit; pit must be fully covered by DeathZone. Goal on far side; optional alternate (longer) route.

**Critical settings:** None (no AttemptTrigger, no Delay). Ensure DeathZone covers entire pit area.

**Common mistakes**
- [ ] Pit not fully covered by DeathZone
- [ ] No death below — player lands on another surface
- [ ] FakePlatform not over a lethal drop

**Fast build steps**
1. Place Ground and define pit gap.
2. Place DeathZone under pit; scale to cover full gap.
3. Place FakePlatform spanning the pit.
4. Place Goal on far side.
5. Test: step on platform → should fade and kill.
6. Optional: add alternate safe route.

---

## P-002 — Double Bluff Path

**Required prefabs/systems:** AttemptTrigger, SpikeTrap (or AmbushTrap-PopUp), Goal. Two distinct routes to goal.

**Placement rules:** Mid/end. One route safe attempt 1; trap on that route from attempt 2. Other route always safe (or no trap).

**Critical settings:** AttemptTrigger Condition **GreaterOrEqual 2**. On Condition Met → SpikeTrap/AmbushTrap-PopUp **Activate()**. **AttemptTrigger.Check()** must be called on level load (bootstrap/Start).

**Common mistakes**
- [ ] Check() never called — trap never activates
- [ ] Wrong condition (e.g. Equal 2 only) if you want “from attempt 2 onward”
- [ ] Only one route — no “other path” to take
- [ ] Trap visible or active on attempt 1

**Fast build steps**
1. Build two routes to Goal (e.g. left and right).
2. Place SpikeTrap or AmbushTrap-PopUp on “betrayal” route; leave disabled or inactive until attempt 2.
3. Add AttemptTrigger; set Condition GreaterOrEqual, Target 2.
4. Wire On Condition Met → trap Activate() (or enable GameObject).
5. Ensure level bootstrap calls AttemptTrigger.Check() on load.
6. Test attempt 1 (safe path) and attempt 2 (trap active).

---

## P-003 — Safe-Looking Kill

**Required prefabs/systems:** SpikeTrap (or KillPlayer, or FakePlatform + DeathZone), Goal

**Placement rules:** Intro/mid. Corridor or platform that looks safe; no obvious hazard until it triggers.

**Critical settings:** If SpikeTrap/KillPlayer: TrapBase **Activation Chance 50** (or desired %). If FakePlatform: place over DeathZone.

**Common mistakes**
- [ ] Activation Chance 100 — no “sometimes safe” feel, or 0 — never kills
- [ ] FakePlatform with no DeathZone below
- [ ] Kill direction wrong for spike variant

**Fast build steps**
1. Place corridor or platform section.
2. Add SpikeTrap or KillPlayer in/near path; set Activation Chance (e.g. 50).
3. Or: place FakePlatform over DeathZone.
4. Place Goal past the hazard.
5. Test multiple runs to confirm RNG or fake-platform behavior.
6. Verify kill direction if using spikes.

---

## P-004 — Delayed Death Platform

**Required prefabs/systems:** DeathZone, DisappearingPlatform or BreakingPlatform, Goal

**Placement rules:** Mid/end. Platform over pit; delay gives false sense of safety.

**Critical settings:** **Delay** (DisappearingPlatform) or **Time Before Break** (BreakingPlatform). Set so player can cross if they move; death if they hesitate.

**Common mistakes**
- [ ] No DeathZone under platform
- [ ] Delay too short — no teachable moment
- [ ] Delay too long — no tension
- [ ] Respawn on if you want one-time only

**Fast build steps**
1. Place pit and DeathZone below.
2. Place DisappearingPlatform or BreakingPlatform over pit.
3. Set Delay or Time Before Break (e.g. 1–2 s).
4. Place Goal on far side.
5. Test: hesitate on platform → die; cross quickly → survive.
6. Disable Respawn if platform should not return.

---

## P-005 — Attempt Gate

**Required prefabs/systems:** AttemptTrigger, FakeWall, Goal

**Placement rules:** Mid/end. Wall blocks path at start; from attempt 2 wall opens (or closes). Clear “blocked” vs “open” state.

**Critical settings:** AttemptTrigger Condition **GreaterOrEqual 2**. On Condition Met → FakeWall **BecomeOpen()** or **BecomeGhost()**. **AttemptTrigger.Check()** on level load.

**Common mistakes**
- [ ] Check() not called — gate never opens
- [ ] Wrong initial state on FakeWall (e.g. open when should be solid)
- [ ] No alternate route on attempt 1 if gate is only path
- [ ] Forgetting to call BecomeOpen() from event

**Fast build steps**
1. Place FakeWall blocking desired path; set Initial State Solid.
2. Add AttemptTrigger; Condition GreaterOrEqual, Target 2.
3. Wire On Condition Met → FakeWall BecomeOpen() (or BecomeGhost()).
4. Ensure Check() called on level load.
5. Place Goal beyond gate (or provide attempt-1 route).
6. Test attempt 1 (blocked) and attempt 2+ (open).

---

## P-006 — Rhythm Corridor

**Required prefabs/systems:** MovingTrap, Goal

**Placement rules:** Mid/end. Narrow passage; hazard moves in a fixed cycle. Spacing so player must wait or run in window.

**Critical settings:** **Auto Move** On. **Is Stop On Reaching Destination** Off (ping-pong). **Local Target Offset 1/2** and **Speed** set. Or ArrowTrap: **Auto Start** On, **Loop** On, **Cycle Interval** set.

**Common mistakes**
- [ ] Both offsets zero — no movement
- [ ] Stop On Reaching Destination On — moves once then stops
- [ ] Corridor too wide — no timing challenge
- [ ] MovingTrap not lethal (Is Lethal off)

**Fast build steps**
1. Place narrow corridor (floor/ceiling/wall as needed).
2. Place MovingTrap (Floor/Ceiling/Wall); set Local Target Offset 1 and 2, Speed.
3. Enable Auto Move; disable Stop On Reaching Destination for ping-pong.
4. Set TrapBase Is Lethal, Kill Direction as needed.
5. Place Goal past corridor.
6. Test: wrong timing kills; correct timing allows pass.

---

## P-007 — One-Time Safe

**Required prefabs/systems:** AreaTrigger, TrapSequencer, AmbushTrap-Falling or AmbushTrap-PopUp or SpikeTrap, Goal

**Placement rules:** Mid/end. Section that is safe only if player crosses quickly; re-entry or lingering triggers trap.

**Critical settings:** AreaTrigger → **TrapSequencer.StartSequence()**. Sequencer: one step with **Delay** (e.g. 1–1.5 s) → Ambush or Spike **Activate()**. Trap placed so it blocks or kills in the zone.

**Common mistakes**
- [ ] StartSequence() never called — trap never fires
- [ ] Delay too short — impossible to pass
- [ ] Delay too long — always safe
- [ ] Trap not in position to kill/block when activated

**Fast build steps**
1. Place corridor or zone; add AreaTrigger at section start.
2. Add TrapSequencer; add one step: Delay (e.g. 1.0 s), Action = AmbushTrap-Falling/PopUp or SpikeTrap Activate().
3. Wire AreaTrigger On Trigger Enter → TrapSequencer StartSequence().
4. Place ambush/spike so it affects the zone (ceiling drop, pop-up, etc.).
5. Place Goal past the section.
6. Test: run through quickly (safe); linger or re-enter (trap kills).

---

## P-008 — Betrayal Platform (Attempt)

**Required prefabs/systems:** FakePlatform, AttemptTrigger, DeathZone, Goal

**Placement rules:** Mid/end. Platform in jump sequence over pit. Attempt 1: platform holds. Attempt 2+: platform pre-triggered, fades on contact.

**Critical settings:** AttemptTrigger Condition **GreaterOrEqual 2**. On Condition Met → **FakePlatform.Activate()** (pre-triggers so next touch fades). **AttemptTrigger.Check()** on level load. FakePlatform over DeathZone.

**Common mistakes**
- [ ] Check() not called — platform never pre-triggers
- [ ] No DeathZone under platform
- [ ] Wrong condition — platform should betray from attempt 2 onward
- [ ] Activate() not called on FakePlatform (must pre-trigger it)

**Fast build steps**
1. Place jump sequence with gap; DeathZone under gap.
2. Place FakePlatform in middle of sequence over DeathZone.
3. Add AttemptTrigger; Condition GreaterOrEqual, Target 2.
4. Wire On Condition Met → FakePlatform Activate().
5. Ensure Check() on level load.
6. Test: attempt 1 platform holds; attempt 2 touch fades and kills.

---

## P-009 — Crusher Run

**Required prefabs/systems:** CrushingWall, solid geometry (ground/wall) on opposite side, Goal

**Placement rules:** Mid/end. Corridor with crusher; player must pass in safe window. Optional SlipperyFloor to reduce control.

**Critical settings:** **Move Offset** (travel vector), **Crush Layer** (layers to raycast for squish), **Auto Start** On. **Ping Pong** On for repeat cycle. **Speed** / **Wait Time** as needed.

**Common mistakes**
- [ ] Crush Layer empty or wrong — squish never detected
- [ ] Squish Check Distance too small
- [ ] No opposing geometry — nothing to crush against
- [ ] Auto Start Off and nothing calls Activate()

**Fast build steps**
1. Place corridor with solid wall/ground on one side.
2. Place CrushingWall (Wall/Ceiling/Floor variant); set Move Offset toward opposing geometry.
3. Set Crush Layer to ground/wall layers; Auto Start On; Ping Pong On.
4. Place Goal past crusher zone.
5. Optional: add SlipperyFloor in run.
6. Test: wrong timing = squish; correct timing = pass.

---

## P-010 — Pop-Up Surprise

**Required prefabs/systems:** AmbushTrap-PopUp, AreaTrigger or AttemptTrigger, Goal

**Placement rules:** Mid/end. Safe-looking tile or landing spot; ambush triggers on line cross or from attempt 2.

**Critical settings:** **Pop Offset** and **Pop Speed** on AmbushTrap. AreaTrigger or AttemptTrigger → **Activate()**. If attempt-based: Condition **GreaterOrEqual 2**; **Check()** on level load.

**Common mistakes**
- [ ] Pop Offset zero or wrong axis — trap doesn’t move visibly
- [ ] No trigger wired to Activate()
- [ ] Check() not called if using AttemptTrigger
- [ ] Trigger zone too large/small — fires at wrong moment

**Fast build steps**
1. Place tile or choke point where player will land or pass.
2. Place AmbushTrap-PopUp (Floor/Ceiling/Wall); set Pop Offset and Pop Speed.
3. Add AreaTrigger at line (or AttemptTrigger with GreaterOrEqual 2).
4. Wire trigger → AmbushTrap-PopUp Activate().
5. If AttemptTrigger, ensure Check() on level load.
6. Test: crossing line or attempt 2 → ambush kills.

---

## P-011 — Warning Then Drop

**Required prefabs/systems:** AreaTrigger, TrapSequencer, AmbushTrap-Falling, Goal

**Placement rules:** Mid/end. Corridor; entering triggers delayed ceiling drop. Goal beyond so player must clear zone quickly.

**Critical settings:** AreaTrigger → **TrapSequencer.StartSequence()**. One step: **Delay** ~1.0 s → **AmbushTrap-Falling Activate()**. Ceiling Ambush placed above corridor.

**Common mistakes**
- [ ] StartSequence() not wired to AreaTrigger
- [ ] AmbushTrap not above kill zone
- [ ] Delay too short — unavoidable
- [ ] Delay too long — no pressure
- [ ] Is Lethal off on AmbushTrap

**Fast build steps**
1. Place corridor; add AreaTrigger at start of zone.
2. Place Ceiling AmbushTrap-Falling above corridor.
3. Add TrapSequencer; one step: Delay 1.0 s → AmbushTrap-Falling Activate().
4. Wire AreaTrigger On Trigger Enter → TrapSequencer StartSequence().
5. Place Goal past corridor.
6. Test: linger → drop kills; clear quickly → survive.

---

## P-012 — Arrow Rhythm Gate

**Required prefabs/systems:** ArrowTrap, Goal

**Placement rules:** Mid/end. Corridor; arrows cross path on fixed cycle. Narrow enough that timing matters.

**Critical settings:** **Auto Start** On, **Loop** On. **Cycle Interval** (and **Arrows Per Burst** / **Fire Rate** as needed). **Kill Player** On on ArrowTrap. Rotate trap so projectiles cross path.

**Common mistakes**
- [ ] Wrong rotation — arrows don’t cross path
- [ ] Kill Player off — arrows don’t kill
- [ ] Loop off — single burst only
- [ ] No Arrow Prefab assigned in prefab
- [ ] Cycle too fast/slow for readable rhythm

**Fast build steps**
1. Place corridor; position ArrowTrap (FreeZone or Ceiling/Wall) so red axis points across path.
2. Set Auto Start On, Loop On, Cycle Interval (e.g. 2 s).
3. Enable Kill Player on ArrowTrap.
4. Place Goal past corridor.
5. Test: wrong timing = hit; correct timing = pass.
6. Tune Cycle Interval for 5–10 s solve feel.

---

## Reference

- **PATTERNS.json** — requires, optional, difficulty, segment_fit, attempt_logic.
- **TRAP_LIBRARY.md** — prefab paths, Inspector details, mini recipes.
