using System;

namespace WhatSprintItIsCalendar
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
