# What Sprint Is It Calendar

An ics file provider of Azure DevOps sprints. As [WhatSprintIs.It](https://whatsprintis.it/) provides nice view of current sprint, this repo generates a calendar containing such information. Such calendar might be imported to your Outlook, Google Calendar etc. App is currently living [here](https://whatsprintisitcalendar.azurewebsites.net/api/calendar).

![Screenshot 2023-01-09 101822](https://user-images.githubusercontent.com/5574525/211275110-00ceaf00-56c0-4840-ab58-370a43ec170e.png)


**TODO**
- Clean up repo
- Load settings (such as first sprint date, sprint length) from configuration,
- Generate ics file recurently, store it in blob and then provide it through API
- Setup CI/CD in GitHub Actions
