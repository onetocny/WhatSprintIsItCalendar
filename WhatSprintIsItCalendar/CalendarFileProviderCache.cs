using LazyCache;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WhatSprintItIsCalendar
{
    public class CalendarFileProviderCache : ICalendarFileProvider
    {
        private readonly ICalendarFileProvider _provider;
        private readonly IAppCache _cache;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CalendarFileProviderCache> _log;

        private static readonly string CacheKey =  nameof(CalendarFileProviderCache);

        public CalendarFileProviderCache(ICalendarFileProvider provider, IAppCache cache, IDateTimeProvider dateTimeProvider, ILogger<CalendarFileProviderCache> log)
        {
            _provider = provider;
            _cache = cache;
            _dateTimeProvider = dateTimeProvider;
            _log = log;
        }

        public byte[] GetCalendar()
        {
            var expiration = _dateTimeProvider.Now + TimeConstants.OneWeek;
            var cacheHit = true;
            var stopwatch = Stopwatch.StartNew();
            var calendar = _cache.GetOrAdd(CacheKey, () =>
            {
                cacheHit = false;
                return _provider.GetCalendar();
            }, expiration);
            stopwatch.Stop();

            if(cacheHit)
            {
                _log.LogInformation($"Cache hit: retrieved {calendar.Length} bytes from cache in {stopwatch.ElapsedMilliseconds} ms.");
            }
            else
            {
                _log.LogInformation($"Cache miss: retrieved {calendar.Length} bytes from provider in {stopwatch.ElapsedMilliseconds} ms.");
            }

            return calendar;
        }
    }
}
