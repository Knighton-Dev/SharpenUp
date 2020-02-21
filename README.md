![CircleCI](https://img.shields.io/circleci/build/github/IanKnighton/SharpenUp?label=Circle%20CI&style=for-the-badge&logo=CircleCI)
![Coveralls](https://img.shields.io/coveralls/github/IanKnighton/SharpenUp?style=for-the-badge)
[![NuGet](https://img.shields.io/nuget/v/SharpenUp?color=pink&logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/SharpenUp/)
[![DotNet](https://img.shields.io/static/v1?label=Standard&message=2.1&color=orange&style=for-the-badge&logo=.NET)](https://github.com/dotnet/standard)
[![Licnese](https://img.shields.io/github/license/IanKnighton/SharpenUp?color=blue&style=for-the-badge)](/LICENSE)
[![Twitter](https://img.shields.io/twitter/follow/ProbablyNotIan?style=for-the-badge)](https://twitter.com/ProbablyNotIan)

# Sharpen Up

This is a .Net Standard 2.1 library for working with the [Uptime Robot API](https://uptimerobot.com/api). I had looked into writing my own uptime monitoring tool, but I found this was an easier jumping off point. 

This is very much a **WORK IN PROGRESS**. I appreciate being made aware of any issues you find once this hits NuGet, but please refrain from creating issues before that point. 

## Usage

As mentioned, this is currently in development so if you're interested in using it you'll need to clone/download the repo and link the `.csproj` file in your project. 

I moved away from using an `appsettings.json` file since it made it very difficult to work with in CircleCI. Was this the correct decision? I'm not sure, but it's what I'm doing for the time being. 

To run tests, there needs to be some environment variables set on the host machine. I put together a bash script to do it *somewhat* quickly between reboots. You could also set them in a myriad of other ways, I'm not your dad.

Here are the needed environment variables. 

```bash
export GOOD_API_KEY="YOUR_API_KEY"
export ACCOUNT_EMAIL="YOUR_ACCOUNT_EMAIL"
export GOOD_MONITOR_ID_1=A_VALID_ID
export GOOD_MONITOR_ID_2=A_VALID_ID
export GOOD_MONITOR_1_FRIENDLY_NAME="MATCHING_NAME_1"
export GOOD_MONITOR_2_FRIENDLY_NAME="MATCHING_NAME_2"
```
