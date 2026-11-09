# CustomBarnKit

A KSP mod that makes the Space Center facilities configurable: the number of levels of each building, their upgrade prices, which stock model each level shows, and every limit or unlock the game derives from a building's level (part count, craft mass and size, crew, contracts, strategies, tracking, R&D, EVA). It also exposes the career tuning values of KSP's `GameVariables` (recovery factors, reputation for a Kerbal's death or recovery, contract reward factors).

Everything is read from one `CUSTOMBARNKIT` node. `GameData/CustomBarnKit/default.cfg` holds the stock values: **do not edit it**, change the values with a Module Manager patch, for example

```
@CUSTOMBARNKIT:FOR[MyMod]
{
    @RESEARCH
    {
        @levels = 5
        @upgrades = 100000, 300000, 600000, 1200000, 2500000
        @upgradesVisual = 1, 1, 2, 2, 3
        @dataToScienceRatio = 0.4, 0.6, 0.8, 0.9, 1
        @scienceCostLimit = 100, 300, 500, 1000, -1
        @unlockedFuelTransfer = 2
        @experimentLevel = 0, 0.5, 0.5, 1, 1
    }
}
```

## Rules

- Every array of a facility node must have exactly `levels` values, one per level, lowest first. An array of the wrong length is **ignored** and the stock value is used; the log says so at load (`[CustomBarnKit] RESEARCH/scienceCostLimit has 3 values but 5 are expected: the stock values are used instead`). When several mods patch the same node, make sure the last one to run sets arrays of the right length (a `:FOR[X]` patch runs after a patch with no pass).
- In a limit array, `-1` means no limit.
- A value named `unlockedSomething = n` unlocks the feature at level `n` (1-based). `-1` never unlocks it.
- `upgradesVisual` says which stock model (1 to 3) each level shows. It is read once per game session.
- The config is read once, when the Space Center is first entered: a change needs a restart of KSP.
- Saves store a facility's level as a fraction of its maximum, so changing `levels` on an existing save remaps the buildings.

## Keys

`VAB` and `SPH`: `levels`, `upgrades` (price of each level, the first one is never paid), `upgradesVisual`, `partCountLimit`, `actionGroupsStockUnlock`, `actionGroupsCustomUnlock`.

`LAUNCHPAD` and `RUNWAY`: `levels`, `upgrades`, `upgradesVisual`, `craftMassLimit` (tons), `craftSizeLimit` (a node with one `size = x, y, z` line per level: **x is the width, y the height, z the length**, the facility text prints height first), `useBuilding` (true: the mass and size limits follow the VAB / SPH level instead of the pad / runway's, and their arrays have the building's level count).

`ASTRONAUTS`: `levels`, `upgrades`, `upgradesVisual`, `recruitHireBaseCost` (stock 2000; the hire price is `base + 80000 · ln(0.25 · activeCrew + 1.12)`, times the difficulty's funds loss multiplier), `recruitHireFixedRate` (true: every hire costs the base), `unlockedEVA`, `unlockedEVAClamber`, `unlockedEVAFlags`, `homebodyEVA` and `homebodyAtmoEVA` (EVA allowed before the unlock when landed on the home body / anywhere around it), `activeCrewLimit`, `crewLevelLimit`.

`MISSION`: `levels`, `upgrades`, `upgradesVisual`, `unlockedFlightPlanning`, `activeContractsLimit`, `scoreSituationHome` and `scoreSituationOther` (8 values: PRELAUNCH, LANDED, SPLASHED, FLYING, SUB_ORBITAL, ORBITING, ESCAPING, DOCKED), `partRecoveryValueFactor`, `resourceRecoveryValueFactor`, `reputationKerbalDeath` (stock -10: negative is a loss), `reputationKerbalRecovery` (stock 0), `contractPrestigeTrivial/Significant/Exceptional`, `contractDestinationWeight`, `contractFundsAdvanceFactor`, `contractFundsCompletionFactor`, `contractFundsFailureFactor`, `contractReputationCompletionFactor`, `contractReputationFailureFactor`, `contractScienceCompletionFactor`.

`TRACKING`: `levels`, `upgrades`, `upgradesVisual`, `unlockedSpaceObjectDiscovery`, `unlockedManeuverTool` (the KSP 1.12 maneuver tool; it also needs flight planning), `orbitDisplayMode` (per level: 0 none, 1 celestial bodies, 2 all orbits, 3 patched conics), `patchesAheadLimit`, `trackedObjectLimit`, `DSNRange` (meters), `DSNRangeCurve`, `DSNPowerCurve`, `DSNScienceCurve` (curve nodes).

`ADMINISTRATION`: `levels`, `upgrades`, `upgradesVisual`, `activeStrategyLimit`, `strategyCommitRange`.

`RESEARCH`: `levels`, `upgrades`, `upgradesVisual`, `dataToScienceRatio`, `scienceCostLimit`, `unlockedFuelTransfer`, `experimentLevel` (per level, compared with each experiment's `requiredExperimentLevel`; stock surface samples need 0.5).

Removed keys (reported in the log if present): `recruitHireFlatRate` and `recruitHireRateModifier` (the stock formula never used them), `EVAChuteOnBuilding` and `unlockedEVAChute` (since KSP 1.11 the EVA parachute is an inventory part gated by the `EVAChuteSkill` trait effect).

## Known stock behaviour

- `scienceCostLimit`: KSP lets you research a node whose cost is **below** the limit from the R&D screen, but the limit check elsewhere allows an equal cost. A node that costs exactly the limit cannot be researched from the screen; use a limit such as `20.1`.
- `levels = 1` works for a pad or a runway (with `useBuilding`), but a single-level building has no meaningful level fraction, so its unlock thresholds are all or nothing.

## Building

A .NET Framework 4.8 class library built with the .NET SDK. Set the `KSPDIR` environment variable to the KSP install (or pass `-p:KSPDIR=<path>`): the references are read from its `KSP_x64_Data\Managed`, and a build copies the DLL and its pdb into `GameData\CustomBarnKit`. With `KSPDIR` empty nothing is copied.

```
dotnet build -c Debug
```

## Releases

GitHub Actions builds every push and pull request (`build.yml`, the zip is a workflow artifact and a comment on the PR). A release is made by running the `create-release` workflow with the version (`major.minor.patch.build`): it builds that version, commits the bump to `Properties/AssemblyInfo.cs`, tags it, and publishes the release with `CustomBarnKit-<version>.zip` and the KSP-AVC `CustomBarnKit.version` next to it. The version file is generated from `CustomBarnKit/CustomBarnKit.version.in` with the KSP range set in `build.yml` (1.12.0 to 1.13.99, built against 1.12.5).
