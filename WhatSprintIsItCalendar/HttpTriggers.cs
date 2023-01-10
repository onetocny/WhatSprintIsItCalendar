using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace WhatSprintItIsCalendar
{
    public class HttpTriggers
    {
        private static readonly MediaTypeHeaderValue CalendarHeaderType = new MediaTypeHeaderValue("text/calendar");

        private readonly CalendarOptions _options;
        private readonly ICalendarFileProvider _calendarFileProvider;

        public HttpTriggers(IOptions<CalendarOptions> options, ICalendarFileProvider calendarFileProvider)
        {
            _options = options.Value;
            _calendarFileProvider = calendarFileProvider;
        }

        [FunctionName(nameof(GetCalendar))]
        public IActionResult GetCalendar([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "calendar")] HttpRequest req)
        {
            var bytes = _calendarFileProvider.GetCalendar();

            return new FileContentResult(bytes, CalendarHeaderType)
            {
                FileDownloadName = _options.CalendarFileName,
            };
        }
    }
}
