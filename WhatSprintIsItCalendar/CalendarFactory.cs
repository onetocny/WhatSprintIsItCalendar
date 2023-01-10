using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Collections.Generic;
using Calendar = Ical.Net.Calendar;
using Ical.Net;
using Microsoft.Extensions.Options;

namespace WhatSprintItIsCalendar
{
    public class CalendarFactory : ICalendarFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly CalendarOptions _options;

        private const string ProductId = "-//github.com/onetocny/WhatSprintIsItCalendar//EN";
        private const string Version2_0 = "2.0";
        private const string PublishMethod = "PUBLISH";

        public CalendarFactory(IOptions<CalendarOptions> options, IDateTimeProvider dateTimeProvider)
        {
            _options = options.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public Calendar Create()
        {
            var calendar = new Calendar
            {
                Method = PublishMethod,
                ProductId = ProductId,
                Version = Version2_0,
                Properties =
                {
                    new CalendarProperty("X-PUBLISHED-TTL", _options.RefreshDuration),
                    new CalendarProperty("X-WR-CALNAME", _options.CalendarName),
                    new CalendarProperty("REFRESH-INTERVAL;VALUE=DURATION", _options.RefreshDuration)
                }
            };

            var events = GetEvents();
            calendar.Events.AddRange(events);

            return calendar;
        }

        private IEnumerable<CalendarEvent> GetEvents()
        {
            var events = new List<CalendarEvent>();
            var start = _options.FirstSprintStartDate;
            var sprint = 1;
            var oneYearFromNow = _dateTimeProvider.Now.AddYears(1);


            while (start < oneYearFromNow)
            {
                for (var week = 1; week <= _options.WeeksPerSprint; week++)
                {
                    var e = new CalendarEvent
                    {
                        Summary = $"Sprint {sprint} Week {week}",
                        Uid = $"{sprint:D3}_{week}",
                        Start = new CalDateTime(start, _options.CalendarTimeZone),
                        Duration = TimeConstants.OneWeek,
                        IsAllDay = true
                    };
                    events.Add(e);
                    start = start + TimeConstants.OneWeek;
                }

                sprint++;
            }

            return events;
        }
    }
}
