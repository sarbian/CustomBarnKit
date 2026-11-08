# Changes

## Unreleased

Fixes from the 2026-11-08 review.

- `default.cfg`: `reputationKerbalDeath` is -10 and `reputationKerbalRecovery` 0, the stock values; the shipped 10 and 25 made a Kerbal's death gain 10 reputation and a recovery 25 (#17). `recruitHireBaseCost` is 2000, the stock value; 10000 added 8000 funds to every hire.
- The reputation, recovery, contract and hire-cost values keep their sign in the config. Every negative float used to become `float.MaxValue`, so `reputationKerbalDeath = -10` added nothing (#17). Limits and unlock thresholds keep `-1` = unlimited / never.
- Every value array is checked against its facility's `levels` when the config is read, and a mismatch is logged in every build. It fell back to the stock values silently in release builds (#16).
- The runway's and the SPH's mass and part count limits read their own arrays; they were validated against the launch pad's and the VAB's, a `NullReferenceException` in the editor when only one of the pair was set. The mass limit follows `useBuilding` like the size limit.
- EVA is refused under time warp, as in stock.
- One addon instance for the whole session: the scene-change callbacks are no longer owned by a destroyed object (the KSPCommunityFixes memory-leak report, #15).
- `upgradesVisual` values outside the stock models, a missing array, `levels` below 1, a missing DSN curve node, and a single-level building no longer throw or divide by zero; they log and keep the stock value.
- Removed: `recruitHireFlatRate` and `recruitHireRateModifier` (the stock formula never read them), `EVAChuteOnBuilding` and `unlockedEVAChute` (the detour they relied on has found nothing since KSP 1.11 made the parachute check an instance method). The keys are reported in the log when a config still sets them.
- Project: an SDK-style .NET Framework 4.8 project, references through `KSPDIR`, portable pdb, nothing copied without `KSPDIR`.
- GitHub Actions: `build.yml` builds every push and PR (zip as artifact and PR comment), `create-release` builds, bumps, tags and publishes a release with the zip and the KSP-AVC version file; the version file points at GitHub releases and declares KSP 1.12.0 to 1.13.99.

## 1.1.22 (2022-01-04)

- Fix facilities getting reset to level 1 in Sandbox and Science games (#11).
