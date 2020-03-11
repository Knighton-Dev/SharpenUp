![CircleCI](https://img.shields.io/circleci/build/github/IanKnighton/SharpenUp?label=Circle%20CI&style=for-the-badge&logo=CircleCI)
[![Coveralls](https://img.shields.io/coveralls/github/IanKnighton/SharpenUp?style=for-the-badge)](https://coveralls.io/github/IanKnighton/SharpenUp)
[![NuGet](https://img.shields.io/nuget/v/SharpenUp?color=pink&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/SharpenUp/)
[![Twitter](https://img.shields.io/twitter/follow/ProbablyNotIan?style=for-the-badge)](https://twitter.com/ProbablyNotIan)

# Sharpen Up

*A .Net Standard library for working with the Uptime Robot API.*

## About

This is a library for working with the [Uptime Robot API](https://uptimerobot.com/api). It allows you to easily integrate the metrics and tooling provided by Uptime Robot into your own applications.

This project adheres to the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md).

## Usage

You can either clone this repo and add the `.csproj` file to your project or add it via NuGet using the command:

```console
dotnet add package SharpenUp
```

### Usage Examples

- [Checking Monitor Status](UsageExamples/CheckMonitorStatus.md)
- [Checking SSL Expiration](UsageExamples/CheckSSL.md)
- [Monthly Report](UsageExamples/ShareHolderReport.md)

## Development

For information on the setting up the environment, see the [Environment Setup](UsageExamples/EnvironmentSetup.md) document.

## Acknowldegements 

- [Uptime Robot](https://uptimerobot.com/) - Really, this library doesn't make sense without their service. 
- [RestSharp](http://restsharp.org/) - Making REST calls isn't terribly difficult in C#, but RestSharp makes it even easier. Especially when you're trying to stuff queries into the request body.
- [Shields.io](https://shields.io/) - Having matching badges that aren't a nightmare to work with is nice and I appreciate that there's a service to make it work. 
- [Coveralls](https://coveralls.io/) - This is my first time really messing with code coverage. It was pretty easy to get my foot in the door with Coveralls. 
- [CircleCI](https://circleci.com/) - Aside from dealing with .Net issues, CircleCI provides an easy way to get CI into your project. 