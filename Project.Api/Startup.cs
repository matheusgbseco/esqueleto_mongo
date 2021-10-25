using Common.API;
using Common.API.Extensions;
using Common.Domain.Base;
using Common.Domain.Interfaces;
using Common.Domain.Model;
using Common.Domain.Security;
using Common.Orm;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.Core.Api.Config;
using Project.Core.Services.Config;
using Project.Data.Context;
using System;
using System.Globalization;
using System.Text.Json;

namespace Project.Core.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfigurationRoot Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.WriteIndented = false;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.Converters.Add(new StringJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new DatimeJsonConverter());
            });

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            services.Configure<DbContextMongoDb>(Configuration.GetSection("ConnStringMongoDB"));
            services.AddSingleton<IContextMongoDb>(sp => sp.GetRequiredService<IOptions<DbContextMongoDb>>().Value);

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton(new EnviromentInfo
            {
                RootPath = this._env.ContentRootPath
            });


            AutoMapperConfigCore.RegisterMappings(services);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(
                        "http://localhost:8081"
                    )
                    .AllowCredentials());
            });

            services.Configure<ConfigEmailBase>(Configuration.GetSection("ConfigEmail"));
            services.Configure<ConfigConnectionBase>(Configuration.GetSection("EFCoreConnStrings"));
            services.Configure<ConfigSettingsBase>(Configuration.GetSection("ConfigSettings"));


            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = false;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateAudience = false;
                paramsValidation.ValidateIssuer = false;
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddSignalR();

            services.AddHangfire(options => options.UseMemoryStorage());

            ConfigContainerCore.Config(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("pt-BR"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<WorkItemHub>("/workitem-hub");
            //});

            if (Convert.ToBoolean(Configuration.GetSection("Hangfire")["Enabled"]))
            {
                app.UseHangfireServer(new BackgroundJobServerOptions
                {
                    HeartbeatInterval = new System.TimeSpan(0, 1, 0),
                    ServerCheckInterval = new System.TimeSpan(0, 1, 0),
                    SchedulePollingInterval = new System.TimeSpan(0, 1, 0)
                });

                app.UseHangfireDashboard("/tasks-services", new DashboardOptions
                {
                    Authorization = new[] { new HangfireAuthorizationFilter() }
                });

                GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
                ConfigHangfire.RegisterTasks(Configuration.GetSection("Hangfire"));
            }

            app.UseCors("AllowAnyOrigin");
            app.AddTokenMiddleware();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(_ => _.MapControllers());

        }
    }
}


public class HangfireActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public HangfireActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override object ActivateJob(Type type)
    {
        return _serviceProvider.GetService(type);
    }
}

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
