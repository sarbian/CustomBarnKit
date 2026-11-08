# CustomBarnKit

## Building

A .NET Framework 4.8 class library built with the .NET SDK. Set the `KSPDIR` environment variable to the KSP install (or pass `-p:KSPDIR=<path>`): the references are read from its `KSP_x64_Data\Managed`, and a build copies the DLL and its pdb into `GameData\CustomBarnKit`. With `KSPDIR` empty nothing is copied.

```
dotnet build -c Debug
```

## Releases

GitHub Actions builds every push and pull request (`build.yml`, the zip is a workflow artifact and a comment on the PR). A release is made by running the `create-release` workflow with the version (`major.minor.patch.build`): it builds that version, commits the bump to `Properties/AssemblyInfo.cs`, tags it, and publishes the release with `CustomBarnKit-<version>.zip` and the KSP-AVC `CustomBarnKit.version` next to it. The version file is generated from `CustomBarnKit/CustomBarnKit.version.in` with the KSP range set in `build.yml` (1.12.0 to 1.13.99, built against 1.12.5).
