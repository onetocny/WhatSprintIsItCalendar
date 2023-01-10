using System;

namespace WhatSprintItIsCalendar
{
    public class UTCProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
