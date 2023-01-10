using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using LazyCache;
using Microsoft.Extensions.Configuration;
using WhatSprintItIsCalendar;
using Ical.Net.Serialization;

[assembly: FunctionsStartup(typeof(WhatSprintIsItCalendar.Startup))]

namespace WhatSprintIsItCalendar
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAppCache>(_ => new CachingService());

            builder.Services
                .AddOptions<CalendarOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(CalendarOptions)).Bind(settings);
                });

            builder.Services.AddSingleton<IDateTimeProvider, UTCProvider>();

            builder.Services.AddSingleton<IStringSerializer>(_ => new ComponentSerializer());

            builder.Services.AddSingleton<CalendarFileProvider>();

            builder.Services.AddSingleton<ICalendarFileProvider>(p =>
            {
                var calendarProvider = p.GetRequiredService<CalendarFileProvider>();
                var cache = p.GetRequiredService<IAppCache>();
                var dateTimeProvider = p.GetRequiredService<IDateTimeProvider>();
                return new CalendarFileProviderCache(calendarProvider, cache, dateTimeProvider);
            });

            builder.Services.AddSingleton<ICalendarFactory, CalendarFactory>();
        }
    }
}
