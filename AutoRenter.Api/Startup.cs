using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using AutoRenter.Api.Authentication;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Services.Commands;
using AutoRenter.Domain.Validation;

namespace AutoRenter.Api
{
    public class Startup
    {
        private const string CorsPolicyName = "Secure";
        private bool useInMemoryProvider = true;
        private bool isDevelopment = false;

        public Startup(IHostingEnvironment env)
        {
            isDevelopment = env.IsDevelopment();

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
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            ConfigureData(services);
            ConfigureCompression(services);
            ConfigureCors(services);
            ConfigureMvc(services);
            ConfigureDI(services);
            ConfigureSwagger(services);
        }

        private void ConfigureData(IServiceCollection services)
        {
            useInMemoryProvider = bool.Parse(Configuration["AppSettings:InMemoryProvider"]);

            services.AddDbContext<AutoRenterContext>(options =>
            {
                if (useInMemoryProvider)
                {
                    options.UseInMemoryDatabase();
                }
            });
        }

        private static void ConfigureCompression(IServiceCollection services)
        {
            services.AddResponseCompression();
        }

        private static void ConfigureDI(IServiceCollection services)
        {
            ConfigureDIForApi(services);
            ConfigureDIForDomainServices(services);
            ConfigureDIForFactories(services);
            ConfigureDIForAutoMapper(services);
        }

        private static void ConfigureDIForApi(IServiceCollection services)
        {
            services.AddTransient<IAuthenticateUser, AuthenticateUser>();
            services.AddTransient<IDataStructureConverter, DataStructureConverter>();
            services.AddTransient<IErrorCodeConverter, ErrorCodeConverter>();
            services.AddTransient<ITokenManager, TokenManager>();
        }

        private static void ConfigureDIForDomainServices(IServiceCollection services)
        {
            Assembly ass = Assembly.Load(new AssemblyName("AutoRenter.Domain.Services"));
            foreach (TypeInfo ti in ass.DefinedTypes
                .Where(x => x.ImplementedInterfaces.Contains(typeof(IDomainService))))
            {
                var interfaceType = ti.ImplementedInterfaces.FirstOrDefault(x => x.Name == $"I{ti.Name}");
                var serviceType = ass.GetType(ti.FullName);
                services.AddTransient(interfaceType, serviceType);
            }
        }

        private static void ConfigureDIForFactories(IServiceCollection services)
        {
            services.AddTransient(typeof(ICommandFactory<>), typeof(CommandFactory<>));
            services.AddTransient<IValidatorFactory, ValidatorFactory>();
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(a =>
            {
                a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                a.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            var origins = GetCorsOrigins();
            var corsProvider = new CorsProvider(isDevelopment, origins);

            services.AddCors(options => 
            {
                options.AddPolicy(CorsPolicyName, corsProvider.GetCorsPolicy());
            });
        }

        private string[] GetCorsOrigins()
        {
            var config = Configuration["AppSettings:CorsOrigins"];

            if (string.IsNullOrEmpty(config))
            {
                return new string[0];
            }

            return config
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToArray();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "AutoRenterAPI", 
                    new Swashbuckle.AspNetCore.Swagger.Info {
                        Title = "AutoRenter API",
                        Version = "v1",
                        Description = "ASP.NET Core implementation of the RESTful AutoRenter API."
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseResponseCompression();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(CorsPolicyName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });
            }

            app.UseStatusCodePages();
            ValidateTokens(app);
            AutoRenterDbInitializer.Initialize(app.ApplicationServices);
            app.UseMvc();
            app.UseSwagger(options =>
            {   
                options.RouteTemplate = "docs/api/{documentName}/swagger.json";                
            });
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs/api";
                options.SwaggerEndpoint(
                    "/docs/api/AutoRenterAPI/swagger.json", 
                    "AutoRenter API V1"
                );
                options.DocExpansion("none");
            });
        }

        private void ValidateTokens(IApplicationBuilder app)
        {
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = GetTokenValidationParameters()
            });
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Configuration["AppSettings:TokenSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = Configuration["AppSettings:TokenSettings:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:TokenSettings:Secret"])),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        private static void ConfigureDIForAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<Vehicle, VehicleModel>()
                    .ForMember(dest => dest.Make,
                        opts => opts.MapFrom(src => src.Make.Name))
                    .ForMember(dest => dest.Model,
                        opts => opts.MapFrom(src => src.Model.Name));

                x.CreateMap<VehicleModel, Vehicle>()
                    .ForMember(dest => dest.Make,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Model,
                        opts => opts.Ignore());

                x.CreateMap<Make, MakeModel>()
                    .ForMember(dest => dest.Id,
                                opts => opts.MapFrom(src => src.ExternalId));

                x.CreateMap<MakeModel, Make>();

                x.CreateMap<Model, ModelModel>()
                    .ForMember(dest => dest.Id,
                        opts => opts.MapFrom(src => src.ExternalId));

                x.CreateMap<ModelModel, Model>();

                x.CreateMap<LocationModel, Location>();

                x.CreateMap<Location, LocationModel>()
                    .ForMember(dest => dest.VehicleCount,
                        opts => opts.ResolveUsing(src => src.Vehicles.Count));

                x.CreateMap<SkuModel, Sku>();
                x.CreateMap<Sku, SkuModel>();

                x.CreateMap<LogEntry, LogEntryModel>();
                x.CreateMap<LogEntryModel, LogEntry>();
            });

            services.AddSingleton(typeof(IMapper), new Mapper(config));
        }
    }
}