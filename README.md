# What Sprint It Is Calendar

An ics file provider of Azure DevOps sprints. As [WhatSprintIs.It](https://whatsprintis.it/) provides nice view of current sprint, this repo generates a calendar containing such information. Such calendar might be imported to your Outlook, Google Calendar etc. App is currently living [here](https://whatsprintitis.azurewebsites.net/api/calendar).

![Screenshot 2023-01-09 092507](https://user-images.githubusercontent.com/5574525/211266173-9777d384-83e5-4f5e-81bc-9386d59c0630.png)


**TODO**
- Clean up repo
- Load settings (such as first sprint date, sprint length) from configuration,
- Generate ics file recurently, store it in blob and then provide it through API
