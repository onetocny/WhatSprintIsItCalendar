using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Net.Http.Headers;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System.Collections.Generic;
using Calendar = Ical.Net.Calendar;
using LazyCache;

namespace WhatSprintItIsCalendar
{
    public static class WhatSprintItIsCalendar
    {
        private static readonly IAppCache Cache = new CachingService();

        private static readonly string Zone = TimeZoneInfo.Utc.Id;
        private static readonly DateTime FirstSprintStartDate = DateTime.Parse("2010-08-14T00:00:00.000Z");
        private static readonly TimeSpan SprintDuration = TimeSpan.FromDays(7);
        private static readonly MediaTypeHeaderValue calendarHeaderType = new MediaTypeHeaderValue("text/calendar");

        private const int WeeksPerSprint = 3;
        private const string RefreshDuration = "P1W"; // one week, see https://www.rfc-editor.org/rfc/rfc2445#section-4.3.6
        private const string CalendarFileName = "whatsprintitis.ics";

        [FunctionName(nameof(WhatSprintItIsCalendar))]
        public static IActionResult Run
        (
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "calendar")] HttpRequest req,
            ILogger log
        )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var bytes = GetCalendarBytesFromCache();
            var stream = new MemoryStream(bytes);

            return new FileStreamResult(stream, calendarHeaderType)
            {
                FileDownloadName = CalendarFileName
            };
        }

        private static byte[] GetCalendarBytesFromCache()
        {
            return Cache.GetOrAdd(nameof(GetCalendarBytes), GetCalendarBytes, DateTimeOffset.UtcNow + SprintDuration);
        }

        private static byte[] GetCalendarBytes()
        {
            var ical = new CalendarSerializer().SerializeToString(GetCalendar());
            return Encoding.UTF8.GetBytes(ical);
        }

        private static Calendar GetCalendar()
        {
            var events = GetEvents();         

            var cal = new Calendar
            {
                Method = "PUBLISH",
                ProductId = "https://github.com/onetocny/WhatSprintItIsCalendar"
            };

            cal.Events.AddRange(events);
            cal.AddProperty("X-PUBLISHED-TTL", RefreshDuration);
            cal.AddProperty("REFRESH-INTERVAL;VALUE=DURATION", RefreshDuration);
            cal.AddProperty("X-WR-CALNAME", "What Sprint It Is");

            return cal;
        }

        private static IEnumerable<CalendarEvent> GetEvents()
        {
            var events = new List<CalendarEvent>();
            var start = FirstSprintStartDate;
            var sprint = 1;
            var oneYearFromNow = DateTime.UtcNow.AddYears(1);


            while (start < oneYearFromNow)
            {
                for (var week = 1; week <= WeeksPerSprint; week++)
                {
                    var e = new CalendarEvent
                    {
                        Summary = $"Sprint {sprint} Week {week}",
                        Uid = $"M{sprint:D3}_{week}",
                        Start = new CalDateTime(start, Zone),
                        Duration = SprintDuration,
                        IsAllDay = true
                    };
                    events.Add(e);
                    start = start + SprintDuration;
                }

                sprint++;
            }

            return events;
        }
    }
}
