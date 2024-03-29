﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMatomoTracking(this IServiceCollection services, Action<MatomoOptions>? setupAction = null)
        {
            services.AddHttpClient<IMatomoTracker, MatomoTracker>();
            services.AddOptions<MatomoOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                if(setupAction!=null)   setupAction(options);
                configuration.GetSection("Matomo").Bind(options);
            });
            services.AddSingleton<IMatomoTracker,MatomoTracker>();

            return services;
        }

    }
}
