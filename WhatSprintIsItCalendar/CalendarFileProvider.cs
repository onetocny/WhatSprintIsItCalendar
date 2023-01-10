using System.Text;
using Ical.Net.Serialization;

namespace WhatSprintItIsCalendar
{
    public class CalendarFileProvider : ICalendarFileProvider
    {
        private readonly ICalendarFactory _calendarFactory;
        private readonly IStringSerializer _serializer;

        private static readonly Encoding Encoding = Encoding.UTF8;

        public CalendarFileProvider(ICalendarFactory calendarFactory, IStringSerializer serializer)
        {
            _calendarFactory = calendarFactory;
            _serializer = serializer;
        }

        public byte[] GetCalendar()
        {
            var calendar = _calendarFactory.Create(); 
            var ical = _serializer.SerializeToString(calendar);
            return Encoding.GetBytes(ical);
        }
    }
}
