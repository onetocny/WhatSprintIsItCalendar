using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace WhatSprintItIsCalendar
{
    public class HttpTriggers
    {
        private readonly ILogger<HttpTriggers> _log;
        private readonly CalendarOptions _options;

        private readonly ICalendarFileProvider _calendarFileProvider;

        private static readonly MediaTypeHeaderValue CalendarHeaderType = new MediaTypeHeaderValue("text/calendar");

        public HttpTriggers(ILogger<HttpTriggers> log, IOptions<CalendarOptions> options, ICalendarFileProvider calendarFileProvider)
        {
            _log = log;
            _options = options.Value;
            _calendarFileProvider = calendarFileProvider;
        }

        [FunctionName(nameof(GetCalendar))]
        public IActionResult GetCalendar([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "calendar")] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var bytes = _calendarFileProvider.GetCalendar();

            return new FileContentResult(bytes, CalendarHeaderType)
            {
                FileDownloadName = _options.CalendarFileName,
            };
        }
    }
}
