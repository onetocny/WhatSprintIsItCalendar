using System;

namespace WhatSprintItIsCalendar
{
    public class CalendarOptions
    {
        public string CalendarName { get; set; }
        public string RefreshDuration { get; set; }
        public string CalendarFileName { get; set; }
        public int WeeksPerSprint { get; set; }
        public string CalendarTimeZone { get; set; }
        public DateTime FirstSprintStartDate { get; set; }
    }
}
