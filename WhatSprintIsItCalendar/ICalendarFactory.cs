using Calendar = Ical.Net.Calendar;

namespace WhatSprintItIsCalendar
{
    public interface ICalendarFactory
    {
        Calendar Create();
    }
}
