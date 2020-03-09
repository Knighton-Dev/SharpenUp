![CircleCI](https://img.shields.io/circleci/build/github/IanKnighton/SharpenUp?label=Circle%20CI&style=for-the-badge&logo=CircleCI)
[![Coveralls](https://img.shields.io/coveralls/github/IanKnighton/SharpenUp?style=for-the-badge)](https://coveralls.io/github/IanKnighton/SharpenUp)
[![NuGet](https://img.shields.io/nuget/v/SharpenUp?color=pink&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/SharpenUp/)
[![Twitter](https://img.shields.io/twitter/follow/ProbablyNotIan?style=for-the-badge)](https://twitter.com/ProbablyNotIan)

# Sharpen Up

*A .Net Standard library for working with the Uptime Robot API.*

## About

This is a .Net Standard 2.1 library for working with the [Uptime Robot API](https://uptimerobot.com/api). I had looked into writing my own uptime monitoring tool, but I found this was an easier jumping off point. 

## Usage

You can either clone this repo and add the `.csproj` file to your project or add it via NuGet using the command:

```console
dotnet add package SharpenUp
```

### Usage Examples

- [Checking Monitor Status](UsageExamples/CheckMonitorStatus.md)
- [Checking SSL Expiration](UsageExamples/CheckSSL.md)

## Development

For information on the setting up the environemt, see the [Environment Setup](UsageExamples/EnvironmentSetup.md) document.

Obviously, the unit tests are written for a very specific use-case (*mine*), but I think I've considered the potential impact for others that may be trying to use the library. 

## Endpoints Exposed

- `getAccountDetails` *Complete as of 1.0.0*
- `getMonitors` *Mostly complete as 1.1.0*
- ~~`newMonitor`~~ *Scaffolded, but not implemented*
- ~~`editMonitor`~~ *Scaffolded, but not implemented*
- `deleteMonitor` *Complete as of 1.3.0*
- `resetMonitor` *Complete as of 1.3.0*
- `getAlertContacts` *Complete as of 1.0.0*
- `newAlertContact` *Complete as of 1.2.0*
- `editAlertContact` *Complete as of 1.2.0*
- `deleteAlertContact` *Complete as of 1.2.0*
- `getMWindows` *Complete as of 1.4.0*
- `newMWindow` *Complete as of 1.4.0*
- `editMWindow` *Complete as of 1.4.0*
- `deleteMWindow` *Complete as of 1.4.0*
- `getPSPs` *Complete as of 1.0.0*
- `newPSP` *Complete as of 1.2.0*
- `editPSP` *Complete as of 1.2.0*
- `deletePSP` *Complete as of 1.2.0*

## [Code of Conduct](CODE_OF_CONDUCT.md)

## Acknowldegements 

- [Uptime Robot](https://uptimerobot.com/) - Really, this library doesn't make sense without their service. 
- [Shields.io](https://shields.io/) - Having matching badges that aren't a nightmare to work with is nice and I appreciate that there's a service to make it work. 
