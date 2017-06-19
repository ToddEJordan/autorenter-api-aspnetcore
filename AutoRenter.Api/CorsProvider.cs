using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace AutoRenter.Api
{
    public class CorsProvider
    {
        private readonly bool isDevelopment;
        private readonly string[] origins;

        private readonly string[] methods = new List<string>()
        {
            { "GET" },
            { "POST" },
            { "PUT" },
            { "DELETE" },
            { "OPTIONS" }
        }.ToArray();

        private readonly string[] exposedHeaders = new List<string>()
        {
            { "x-total-count" },
        }.ToArray();

        public CorsProvider(bool isDevelopment, string[] origins)
        {
            this.isDevelopment = isDevelopment;
            this.origins = origins;
        }

        public CorsPolicy GetCorsPolicy()
        {
            return isDevelopment ? DevelopmentPolicy() : ProductionPolicy();
        }

        private CorsPolicy DevelopmentPolicy()
        {
            var targetOrigins = origins.Any() 
                ? origins 
                : new List<string>() { "http://localhost:8080", "http://127.0.0.1:8080" }.ToArray();

            return BuildPolicy(targetOrigins, TimeSpan.FromSeconds(5));
        }

        private CorsPolicy ProductionPolicy()
        {
            var targetOrigins = origins.Any()
                ? origins
                : new string[0];

            return BuildPolicy(targetOrigins, TimeSpan.FromMinutes(10));
        }

        private CorsPolicy BuildPolicy(string[] targetOrigins, TimeSpan preflightMaxAge)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.WithOrigins(targetOrigins)
                       .WithMethods(methods)
                       .AllowAnyHeader()
                       .WithExposedHeaders(exposedHeaders)
                       .SetPreflightMaxAge(preflightMaxAge);

            return corsBuilder.Build();
        }
    }
}
