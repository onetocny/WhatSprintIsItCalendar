using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LazyCache;

[assembly: FunctionsStartup(typeof(WhatSprintIsItCalendar.Startup))]

namespace WhatSprintIsItCalendar
{    
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAppCache>(_ => new CachingService());
        }
    }
}
