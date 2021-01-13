# TimeCheck
.Net Console application (net5.0) that converts a time entry to configured time zones. This was developed, built and running on Ubuntu 20.04.1 LTS x86_64.
> Use Case: As a meeting organizer I would like a time to be converted to many time zones so that I can visualize when the meeting will occurr in each time zone

## Prerequiste
[Install .Net on Windows, Linux and macOS](https://docs.microsoft.com/en-us/dotnet/core/install/)

## Instructions
* Add alias to the ~/.bashrc file for quick access
```
alias timecheck="dotnet /[Path to install location]/TimeCheck.dll"
```

* Add time zones to the appsettings.json file. Time zone Ids can be found in TimeZones.md file.

## Example output
```
$ timecheck 8:30am
Central  : 08:30 AM Tue Sep, 08 2020 Zone: (UTC-06:00) Central Standard Time
Bangalore: 07:00 PM Tue Sep, 08 2020 Zone: (UTC+05:30) India Standard Time
Mountain : 07:30 AM Tue Sep, 08 2020 Zone: (UTC-07:00) Mountain Standard Time
Hawaii   : 03:30 AM Tue Sep, 08 2020 Zone: (UTC-10:00) Hawaii-Aleutian Standard Time
```