using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutoRenter.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = BuildConfiguration(args);
            var webHost = BuildWebHost(config);
            webHost.Run();
        }

        private static IWebHost BuildWebHost(IConfigurationRoot config)
        {
            var envUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
            if (!string.IsNullOrWhiteSpace(envUrls))
            {
                return BuildHostWithUrls(config, envUrls);
            }

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                return BuildHostWithUrls(config, "http://*:3000");
            }

            return BuildHost(config);
        }

        private static IConfigurationRoot BuildConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables("ASPNETCORE_")
                .Build();
        }

        private static IWebHost BuildHostWithUrls(IConfigurationRoot config, string urls)
        {
            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(urls)
                .Build();
        }

        private static IWebHost BuildHost(IConfigurationRoot config)
        {
            return new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
        }
    }
}