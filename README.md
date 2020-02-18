[![CircleCI](https://circleci.com/gh/IanKnighton/SharpenUp.svg?style=svg)](https://github.com/IanKnighton/SharpenUp)


# Sharpen Up

This is a .Net Standard library for working with the [Uptime Robot API](https://uptimerobot.com/api). I had looked into writing my own uptime monitoring tool, but I found this was an easier jumping off point. 

It's currently a WIP. Once I've worked out all of the available endpoints in the API, I'll make it available via NuGet package. 

## Usage

As mentioned, this is currently in development so if you're interested in using it you'll need to clone/download the repo and link the `.csproj` file in your project. 

It is setup for Depenedency Injection, but the only real parameter it needs is your API Key. You can add this in the `appsettings.json` file. I've included an example version of the file where you can update the key and account email. 

You can look at the tests if you need further examples on how to use it. 
