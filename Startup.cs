using AutoRenter.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace AutoRenter.API
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowAll";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCompression(services);
            ConfigureCors(services);
            ConfigureMvc(services);
            ConfigureDI(services);
        }

        private static void ConfigureCompression(IServiceCollection services)
        {
            services.AddResponseCompression();
        }

        private static void ConfigureDI(IServiceCollection services)
        {
            services.AddSingleton<ILocationService, InMemoryLocationService>();
            services.AddSingleton<IVehicleService, InMemoryVehicleService>();
            services.AddTransient<IResponseConverter, ResponseConverter>();
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(
                    a => a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            // TODO: Harden this later
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options => { options.AddPolicy(CorsPolicyName, corsBuilder.Build()); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseResponseCompression();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(CorsPolicyName);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler();

            app.UseStatusCodePages();

            app.UseMvc();
        }
    }
}