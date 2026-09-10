# Elemental Tower Defence — Revival & Personal Repo

**Date:** 2026-09-10
**Author:** Claude, with John Minseok Kim (`kakwak123`)
**Status:** Approved for scope; phases 2–4 pending spec review

## Purpose

Take the COMP30019 Project 2 group submission ("Elemental Tower Defence", Unity
2019.4.3f1) and turn it into a personal, portfolio-quality repository under the
GitHub account `kakwak123`, while fixing real defects in the gameplay code and
improving its presentation, visuals, and structure.

## Constraints

These are binding and shape every decision below.

1. **No Unity Editor, no `dotnet`, no `msbuild` on the development machine.**
   Verified absent. C# and shader changes therefore **cannot be compiled, run,
   or playtested** here. Every code change ships unverified and must be
   confirmed by the user opening the project in Unity 2019.4.3f1.
2. **Repository is large.** 602 MiB packed; largest single file is
   `Assets/Plugins/FMOD/lib/ios/libfmodstudiounitypluginL.a` at 42 MB. Under
   GitHub's 100 MB per-file hard limit, so a push will succeed, but slowly.
3. **The work is a 4-person group's coursework.** Contributors: John Minseok
   Kim, Jiawei Xu, Yanshuo Wang, Zi Chen Li. Attribution must survive into the
   new repository even though git history is being discarded.
4. **The existing group repository must not be damaged.** All work happens in a
   new directory; `Graphics-and-Interaction-COMP30019/project-2-project2_group_29`
   is read-only for this project.

## Success criteria

- A private repository `kakwak123/elemental-tower-defence` exists, contains the
  full playable project, and was pushed successfully.
- Its README stands alone as a portfolio piece: what the game is, how to run
  it, how it was built, who built it.
- The two confirmed defects (below) are fixed, with the reasoning recorded so
  the user can verify each in the Editor.
- Nothing claimed as "working" that was not actually verified.

## Decisions

| Decision | Choice | Rationale |
|---|---|---|
| Repo name | `elemental-tower-defence` | The game's actual name, not `Project2` |
| Visibility | **Private** | Course may still set this project; avoids being a plagiarism source |
| History | **Fresh, single commit** | User's choice; ~600 MB smaller, no co-author history |
| Attribution | README credits table | Required, since git history is discarded |
| Licence | None by default | Group-owned coursework; not the user's alone to relicense |

## Confirmed defects

Both were found by reading the source and are stated with evidence. Neither has
been reproduced in the Editor, because there is no Editor.

### D1 — The laser turret's slow effect never applies

`LaserTurret.Laser()` calls `targetEnemy.Slow(slowAmount)` every frame
(`Assets/Scripts/LaserTurret.cs:38`). `Enemy.Update()` contains:

```csharp
if (bulletSlowTimer >= bulletSlowDuration) { speed = startSpeed; }
```

(`Assets/Scripts/Enemy.cs:88`). `bulletSlowTimer` and `bulletSlowDuration` both
default to `0`, so the guard is `0 >= 0` — true on every frame of every enemy's
lifetime, including enemies never hit by an ice bullet. The reset therefore
fires continuously and overwrites the laser's slow. Which one is visible in a
given frame depends on Unity's script execution order between `LaserTurret` and
`Enemy`, making the behaviour order-dependent rather than merely absent.

Ice bullets are unaffected, because `IceBulletSlow()` sets a real
`bulletSlowDuration`.

**Fix direction:** model slows as an explicit, expiring effect with a remaining
duration, rather than as a raw `speed` write plus a separate reset guard. The
laser applies a short refreshing slow each frame; the reset only runs when a
slow is actually active and has expired.

### D2 — Flame VFX leaks when a burning enemy dies to another source

`Enemy.Die()` (`Assets/Scripts/Enemy.cs:104`) destroys `onFireEffect` only
inside the burn-damage branch of `Update()`. An enemy that is on fire but is
killed by a bullet, laser, or explosion runs `Die()` without that cleanup,
orphaning the instantiated flame particle object in the scene permanently.

**Fix direction:** clean up `onFireEffect` inside `Die()` itself, so every death
path releases it regardless of what dealt the killing blow.

## Phases

Ordered by how much risk can actually be retired before handing over.

### Phase 0 — Repository (verifiable)

Create `kakwak123/elemental-tower-defence`, private. Build a fresh working
directory containing the current tree with no `.git`, initialise, single
commit, push. The existing group clone is never mutated.

**Verification:** the push either succeeds or fails; `gh repo view` confirms.

### Phase 1 — Portfolio polish (verifiable)

Rewrite `README.md` for a reader who has never taken COMP30019: what the game
is, how to play, how to run it, architecture, the team. Carry across existing
media from `Gifs/`. Remove coursework scaffolding (submission deadlines, tick
boxes, the LMS commit-ID instruction). Preserve the third-party asset
attributions and the team credits.

**Verification:** files exist, links resolve, images referenced actually exist
on disk.

### Phase 2 — Defects and game feel (NOT verifiable here)

Fix D1 and D2. Then targeted feel work: wave pacing, turret balance, and
death/hit feedback. Small, separately-committed, individually-reviewable diffs
so a single bad change can be reverted without losing the rest.

**Verification:** user opens Unity 2019.4.3f1, confirms compilation, plays a
level. Until then these changes are labelled unverified in the commit messages.

### Phase 3 — Shaders and graphics (NOT verifiable here)

`Assets/CustomShader/fog.shader` and `shining.shader`, plus lighting and
post-processing. Purely visual, so nothing about the result can be assessed
without rendering it.

**Risk:** a shader that fails to compile breaks the material it is on. Every
shader edit keeps the original preserved so it can be restored.

### Phase 4 — Architecture (NOT verifiable here, highest risk)

The codebase leans on mutable global state: `PlayerStats.Money` / `.Lives` /
`.Rounds`, `WaveSpawner.EnemiesAlive`, and `Waypoints.points` are all `static`
and reset in `Start()`, which makes scene reloads order-dependent. `PlayerStats.Update()`
also rebuilds two UI strings every frame, allocating garbage continuously.

**Position on record:** refactoring 34 working scripts with no compiler is the
worst risk/reward trade in this plan. It is included because the user
explicitly asked for it after the risk was stated. It runs **last**, so that if
it goes wrong, phases 0–3 are already committed and safe.

## Out of scope

- Relicensing or open-sourcing the group's work publicly
- Upgrading the Unity version (2019.4.3f1 → newer) — large, breaking, unverifiable
- Replacing vendored asset packs
- Adding automated tests (Unity Test Framework needs the Editor to run)

## Open question

Whether a playable build (WebGL or Windows) can be included. It cannot be
produced here — no Unity. If the user wants one in the repo, they must build it
in the Editor and it can be attached as a GitHub Release rather than committed,
to avoid inflating an already-large repository.
