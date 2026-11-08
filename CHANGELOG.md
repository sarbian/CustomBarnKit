# Changes

## Unreleased

- Project: an SDK-style .NET Framework 4.8 project, references through `KSPDIR`, portable pdb, nothing copied without `KSPDIR`.
- GitHub Actions: `build.yml` builds every push and PR (zip as artifact and PR comment), `create-release` builds, bumps, tags and publishes a release with the zip and the KSP-AVC version file; the version file points at GitHub releases and declares KSP 1.12.0 to 1.13.99.

## 1.1.22 (2022-01-04)

- Fix facilities getting reset to level 1 in Sandbox and Science games (#11).
