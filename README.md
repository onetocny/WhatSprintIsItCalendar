# What Sprint Is It Calendar

## Overview

This tool provides an ics file that contains definitions of Azure DevOps sprints. As [WhatSprintIs.It](https://whatsprintis.it/) provides nice view of current sprint as well as its visual representation, WhatSprintIsItCalendar generates a calendar containing those information. Such calendar might be imported to your Outlook, Google Calendar etc. App is currently living [here](https://whatsprintisitcalendar.azurewebsites.net/api/calendar).

![Screenshot 2023-01-09 101822](https://user-images.githubusercontent.com/5574525/211275110-00ceaf00-56c0-4840-ab58-370a43ec170e.png)

## How import it to Outlook

1. Open calendar view in Outlook and right click on the calendar group where you would like to add sprint calendar. Select Add calendar -> From Internet. The dialog asking for URL should show up. Fill in the URL of calendar https://whatsprintisitcalendar.azurewebsites.net/api/calendar.

    ![Screenshot 2023-01-09 112259](https://user-images.githubusercontent.com/5574525/211287096-c36960c2-b108-41c2-b2e0-3cc1487af655.png)

1. Once you have the calendar imported. You can overlay it with other calendar. Select all calendar that you want to overlay. Then right click on them and select overlay.

    ![Screenshot 2023-01-09 112321](https://user-images.githubusercontent.com/5574525/211287299-209ac2fd-f4c8-49ac-9fe4-fe50bb1f385b.png)

1. All calendars should be displayed in single calendar grid now. You can distinguish between particular calendar events by color.

    ![Screenshot 2023-01-09 112811](https://user-images.githubusercontent.com/5574525/211292433-3e4c98d4-206d-4633-8d84-34f015369dcc.png)



## TODO
- Clean up repo
- Load settings (such as first sprint date, sprint length) from configuration,
- Generate ics file recurently, store it in blob and then provide it through API
- Setup CI/CD in GitHub Actions
- Support request parameters to specify the time range of result file
