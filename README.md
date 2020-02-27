![CircleCI](https://img.shields.io/circleci/build/github/IanKnighton/SharpenUp?label=Circle%20CI&style=for-the-badge&logo=CircleCI)
![Coveralls](https://img.shields.io/coveralls/github/IanKnighton/SharpenUp?style=for-the-badge)
[![NuGet](https://img.shields.io/nuget/v/SharpenUp?color=pink&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/SharpenUp/)
[![DotNet](https://img.shields.io/static/v1?label=Standard&message=2.1&color=orange&style=for-the-badge&logo=.NET)](https://github.com/dotnet/standard)
[![Licnese](https://img.shields.io/github/license/IanKnighton/SharpenUp?color=blue&style=for-the-badge)](/LICENSE)
[![Twitter](https://img.shields.io/twitter/follow/ProbablyNotIan?style=for-the-badge)](https://twitter.com/ProbablyNotIan)

# Sharpen Up

*A .Net Standard library for working with the Uptime Robot API.*

## About

This is a .Net Standard 2.1 library for working with the [Uptime Robot API](https://uptimerobot.com/api). I had looked into writing my own uptime monitoring tool, but I found this was an easier jumping off point. 

## Usage

There are two options.

1) Clone the repo and add the `SharpenUp.csproj` to your solution file. 
```
git clone https://github.com/IanKnighton/SharpenUp.git
```
2) Use the latest version of the NuGet package.
```
dotnet add package SharpenUp
```

Once you have the reference in your project, you can instantiate the library using the `IUptimeManager` interface.

Like this:

```csharp
using SharpenUp.Client;
using SharpenUp.Common;

namespace Application
{
    class Program
    {
        static async Task Main( string[] args )
        {
            IUptimeManager manager = new UptimeManager( "YOUR_API_KEY" );
        }
    }
}
```

From there, you can reference the methods exposed in the `IUptimeManager` interface just like you would any other library. 

```csharp
using SharpenUp.Client;
using SharpenUp.Common;
using SharpenUp.Common.Models.Monitors;

namespace Application
{
    class Program
    {
        static async Task Main( string[] args )
        {
            IUptimeManager manager = new UptimeManager( "YOUR_API_KEY" );
            MonitorsResult result = await manager.GetMonitorsAsync();
        }
    }
}
```


## Development

To run tests, there needs to be some environment variables set on the host machine. I put together a bash script to do it *somewhat* quickly between reboots. You could also set them in a myriad of other ways, I'm not your dad.

Here are the needed environment variables. 

```bash
export GOOD_API_KEY="YOUR_API_KEY"
export ACCOUNT_EMAIL="YOUR_ACCOUNT_EMAIL"
export GOOD_MONITOR_ID_1=A_VALID_ID
export GOOD_MONITOR_ID_2=A_VALID_ID
export GOOD_MONITOR_1_FRIENDLY_NAME="MATCHING_NAME_1"
export GOOD_MONITOR_2_FRIENDLY_NAME="MATCHING_NAME_2"
export PSP_NAME_1="PUBLICE_STATUS_PAGE_1_NAME"
export PSP_NAME_2="PUBLICE_STATUS_PAGE_2_NAME"
export PSP_URL_1="PUBLICE_STATUS_PAGE_1_URL"
export PSP_URL_2="PUBLICE_STATUS_PAGE_2_URL"
```

## Endpoints Exposed

This is based off of a list on the Uptime Robot reference.

- ~~`POST getAccountDetails`~~ *Complete as of 0.0.3*
- `POST getMonitors` *Mostly complete as of version 0.0.3*
- `POST newMonitor`
- `POST editMonitor`
- `POST deleteMonitor`
- `POST resetMonitor`
- ~~`POST getAlertContacts`~~ *Complete as of 0.0.3*
- `POST newAlertContact`
- `POST editAlertContact`
- `POST deleteAlertContact`
- `POST getMWindows` *Mostly complete, but untested, as of 0.0.6 (I don't have a premium account at the moment.*
- `POST newMWindow`
- `POST editMWindow`
- `POST deleteMWindow`
- ~~`POST getPSPs`~~ *Complete as of 0.0.5*
- `POST newPSP`
- `POST editPSP`
- `POST deletePSP`

## Acknowldegements 

- [Uptime Robot](https://uptimerobot.com/) - Really, this library doesn't make sense without their service. 
- [Shields.io](https://shields.io/) - Having matching badges that aren't a nightmare to work with is nice and I appreciate that there's a service to make it work. 
