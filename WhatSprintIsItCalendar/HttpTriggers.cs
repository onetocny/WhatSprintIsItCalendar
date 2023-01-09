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
using Ical.Net;

namespace WhatSprintItIsCalendar
{
    public class HttpTriggers
    {
        private readonly IAppCache _cache;
        private readonly ILogger<HttpTriggers> _log;

        private static readonly string Zone = TimeZoneInfo.Utc.Id;
        private static readonly DateTime FirstSprintStartDate = DateTime.Parse("2010-08-14T00:00:00.000Z");
        private static readonly TimeSpan OneWeek = TimeSpan.FromDays(7);
        private static readonly MediaTypeHeaderValue CalendarHeaderType = new MediaTypeHeaderValue("text/calendar");

        private const int WeeksPerSprint = 3;
        private const string RefreshDuration = "P1W"; // one week, see https://www.rfc-editor.org/rfc/rfc2445#section-4.3.6
        private const string CalendarFileName = "whatsprintisit.ics";
        private const string ProductId = "-//github.com/onetocny/WhatSprintIsItCalendar//EN";
        private const string Version2_0 = "2.0";
        private const string PublishMethod = "PUBLISH";
        private const string CalendarName = "What Sprint Is It";

        public HttpTriggers(IAppCache cache, ILogger<HttpTriggers> log)
        {
            _cache = cache;
            _log = log;
        }

        [FunctionName(nameof(GetCalendar))]
        public IActionResult GetCalendar([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "calendar")] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var bytes = GetCalendarBytesFromCache();

            return new FileContentResult(bytes, CalendarHeaderType)
            {
                FileDownloadName = CalendarFileName,
            };
        }

        private byte[] GetCalendarBytesFromCache()
        {
            return _cache.GetOrAdd(nameof(GetCalendarBytes), GetCalendarBytes, DateTimeOffset.UtcNow + OneWeek);
        }

        private static byte[] GetCalendarBytes()
        {
            var serializer = new ComponentSerializer();
            var ical = serializer.SerializeToString(GetCalendar());
            return Encoding.UTF8.GetBytes(ical);
        }

        private static Calendar GetCalendar()
        {
            var calendar = new Calendar
            {
                Method = PublishMethod,
                ProductId = ProductId,
                Version = Version2_0,
                Properties =
                {
                    new CalendarProperty("X-PUBLISHED-TTL", RefreshDuration),
                    new CalendarProperty("X-WR-CALNAME", CalendarName),
                    new CalendarProperty("REFRESH-INTERVAL;VALUE=DURATION", RefreshDuration)
                }
            };

            var events = GetEvents();
            calendar.Events.AddRange(events);

            return calendar;
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
                        Uid = $"{sprint:D3}_{week}",
                        Start = new CalDateTime(start, Zone),
                        Duration = OneWeek,
                        IsAllDay = true
                    };
                    events.Add(e);
                    start = start + OneWeek;
                }

                sprint++;
            }

            return events;
        }
    }
}
