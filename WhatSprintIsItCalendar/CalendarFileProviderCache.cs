using LazyCache;

namespace WhatSprintItIsCalendar
{
    public class CalendarFileProviderCache : ICalendarFileProvider
    {
        private readonly ICalendarFileProvider _provider;
        private readonly IAppCache _cache;
        private readonly IDateTimeProvider _dateTimeProvider;

        private static readonly string CacheKey =  nameof(CalendarFileProviderCache);

        public CalendarFileProviderCache(ICalendarFileProvider provider, IAppCache cache, IDateTimeProvider dateTimeProvider)
        {
            _provider = provider;
            _cache = cache;
            _dateTimeProvider = dateTimeProvider;
        }

        public byte[] GetCalendar()
        {
            return _cache.GetOrAdd(CacheKey, _provider.GetCalendar, _dateTimeProvider.Now + TimeConstants.OneWeek);
        }
    }
}
