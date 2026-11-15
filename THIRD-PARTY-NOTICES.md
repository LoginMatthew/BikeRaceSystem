# Third-party packages and licenses

Package metadata in the restored dependency graph was checked for the versions listed below. No package with a paid-only or home-use-prohibited license was found. This is a package-license review, not legal advice. Microsoft SDK packages include Microsoft license terms, and redistributed builds must follow those terms.

## Direct package references

| Package | Version | License |
|---|---:|---|
| CommunityToolkit.Maui | 8.0.1 | MIT |
| CommunityToolkit.Mvvm | 8.4.2 | MIT |
| Microsoft.Data.Sqlite | 8.0.27 | MIT |
| Microsoft.Extensions.DependencyInjection | 8.0.1 | MIT |
| Microsoft.Extensions.Logging.Debug | 8.0.1 | MIT |

The MAUI Windows SDK also resolves Microsoft MAUI, Windows App SDK, Windows SDK Build Tools, Win2D, SQLitePCLRaw, and Microsoft.Extensions packages transitively. Their package metadata identifies MIT or Apache-2.0 licenses, or Microsoft SDK terms. SQLitePCLRaw uses Apache-2.0; retain its license/notice when redistributing its binaries.

Microsoft's Windows App SDK terms allow installation and use to develop and test Windows applications. Win2D's current source license is MIT; the resolved package version also carries Microsoft's package license URL, so check the exact package terms again before redistributing a compiled application.
