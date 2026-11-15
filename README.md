# BikeRaceSystem

A Windows desktop app for running two-bike races. It has a race-control view, a separate display window, race settings, localization, and SQLite result storage. The sensor layer can be configured independently for each bicycle.

## Requirements

- Windows 10/11
- .NET SDK with the .NET MAUI Windows workload
- Windows 10 SDK 10.0.19041.0 or later

## Build and run

Open `BikeRaceSystem.sln` in Visual Studio and set `BikeRaceSystem.UI.Maui` as the startup project. Or use a terminal from this folder:

```powershell
dotnet restore BikeRaceSystem.sln
dotnet build BikeRaceSystem.sln
dotnet run --project BikeRaceSystem.UI.Maui/BikeRaceSystem.UI.Maui.csproj
```

The app targets Windows only. Generated build output, NuGet intermediates, Visual Studio state, and local database files are excluded from Git. Restoring the solution recreates the package assets needed to build.

## Features

- Race control and a separate spectator display window
- Random/demo and sensor race modes
- Hungarian, English, German, French, and Japanese UI resources
- SQLite race result storage
- Per-bicycle TCP, serial COM, or Bluetooth Classic SPP virtual COM configuration

## Assets

This project contains images generated with AI.

The current sensor input contract is one decimal speed value per line, expressed in distance units per second. A sensor must implement that format or have an adapter for its own protocol.

## Project layout

- `BikeRaceSystem.UI.Maui`: Windows MAUI UI, views, ViewModels, and composition root
- `BikeRaceSystem.Core`: application models and service contracts
- `BikeRaceSystem.Services`: race, sensor transport, configuration, localization, and storage implementations
- `BikeRaceSystem.Localization`: `.resx` resources for supported languages

NuGet package license details are listed in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).
