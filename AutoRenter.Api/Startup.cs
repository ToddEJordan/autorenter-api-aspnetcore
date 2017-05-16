using System;
using System.Net;
using System.Text;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using AutoRenter.Api;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Data;
using AutoRenter.Api.Authentication;
using AutoRenter.Api.Infrastructure;
using MediatR;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Services;

namespace AutoRenter.Api
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowAll";
        private bool _useInMemoryProvider = true;

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
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            ConfigureData(services);
            ConfigureCompression(services);
            ConfigureCors(services);
            ConfigureAutoMapper(services);
            ConfigureMvc(services);
            ConfigureMediatR(services);
            ConfigureDI(services);
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(Startup));
            //Mapper.AssertConfigurationIsValid();
        }

        private static void ConfigureMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
        }

        private void ConfigureData(IServiceCollection services)
        {
            try
            {
                _useInMemoryProvider = bool.Parse(Configuration["AppSettings:InMemoryProvider"]);
            }
            catch (Exception)
            {
                throw;
            }

            services.AddDbContext<AutoRenterContext>(options =>
            {
                switch (_useInMemoryProvider)
                {
                    case true:
                        options.UseInMemoryDatabase();
                        break;
                    default:
                        break;
                }
            });
        }

        private static void ConfigureCompression(IServiceCollection services)
        {
            services.AddResponseCompression();
        }

        private static void ConfigureDI(IServiceCollection services)
        {
            ConfigureDIForDomainServices(services);

            //services.AddTransient<IResponseConverter, ResponseConverter>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IAuthenticateUser, AuthenticateUser>();
        }

        private static void ConfigureDIForDomainServices(IServiceCollection services)
        {
            Assembly ass = Assembly.Load(new AssemblyName("AutoRenter.Domain.Services"));
            foreach (TypeInfo ti in ass.DefinedTypes
                .Where(x => x.ImplementedInterfaces.Contains(typeof(IDomainService))))
            {
                var interfaceType = ti.ImplementedInterfaces.FirstOrDefault(x => x.Name == $"I{ti.Name}");
                var serviceType = ass.GetType(ti.FullName);
                services.AddScoped(interfaceType, serviceType);
            }
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc(options => { options.Conventions.Add(new FeatureConvention()); })
                .AddJsonOptions(a => 
                {
                    a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    a.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.Requirements.Add(new AdministratorAuthorizationRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
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
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });
            }

            app.UseStatusCodePages();
            ValidateTokens(app);
            AutoRenterDbInitializer.Initialize(app.ApplicationServices);
            app.UseMvc();
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
    }
}