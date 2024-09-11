using ExpenseTracker.Core.Entities;
using ExpenseTracker.Data;
using ExpenseTracker.Service;
using ExpenseTracker.Web.Common;
using ExpenseTracker.Web.Middlewares;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Templates;
using System.Net;

namespace ExpenseTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();
                builder.Services.AddAutoMapper(typeof(Program));

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console(new ExpressionTemplate(
                    // Include trace and span ids when present.
                    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
                .WriteTo.ApplicationInsights(
                  services.GetRequiredService<TelemetryConfiguration>(),
                  TelemetryConverter.Traces));

                Log.Information("Starting the application Expense Tracker Web API...");


                // In production, modify this with the actual domains you want to allow
                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

                // Add services to the container.

                builder.Services.AddControllers();

                // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(options =>

                        {
                            configuration.Bind("AzureAd", options);
                            options.Events = new JwtBearerEvents();

                            options.Events = new JwtBearerEvents
                            {
                                OnTokenValidated = context =>
                                {
                                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                                    // Access the scope claim (scp) directly
                                    var scopeClaim = context.Principal?.Claims.FirstOrDefault(c => c.Type == "scp")?.Value;

                                    if (scopeClaim != null)
                                    {
                                        logger.LogInformation("Scope found in token: {Scope}", scopeClaim);
                                    }
                                    else
                                    {
                                        logger.LogWarning("Scope claim not found in token.");
                                    }

                                    return Task.CompletedTask;
                                },
                                OnAuthenticationFailed = context =>
                                {
                                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                                    logger.LogError("Authentication failed: {Message}", context.Exception.Message);
                                    return Task.CompletedTask;
                                },
                                OnChallenge = context =>
                                {
                                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                                    logger.LogError("Challenge error: {ErrorDescription}", context.ErrorDescription);
                                    return Task.CompletedTask;
                                }
                            };
                        }, options => { configuration.Bind("AzureAd", options); });

                // The following flag can be used to get more descriptive errors in development environments
                IdentityModelEventSource.ShowPII = false;


                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpenseTracker API", Version = "v1" });
                });

                //before this step, install dotnet EF and run commands to pull db entities. E.g. scaffold
                builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
                {
                    options.UseSqlServer(
                    configuration.GetConnectionString("DbContext"),
                    providerOptions => providerOptions.EnableRetryOnFailure()
                    )
                   //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)            
                   .EnableSensitiveDataLogging(); //should not be used in production, only for development purpose
                }
               );

                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IExpenseService, ExpenseService>();
                builder.Services.AddScoped<ICreditCardService, CreditCardService>();
                builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>();

                builder.Services.AddScoped<IUserIncomeRepository, UserIncomeRepository>();
                builder.Services.AddScoped<IUserBudgetRepository, UserBudgetRepository>();

                builder.Services.AddScoped<IUserBudgetService, UserBudgetService>();
                builder.Services.AddScoped<IUserIncomeService, UserIncomeService>();
                builder.Services.AddScoped<IEmailCopyRepository, EmailCopyRepository>();
                builder.Services.AddScoped<IEmailCopyService, EmailCopyService>();

                builder.Services.AddScoped<IFamilyMemberRequestRepository, FamilyMemberRequestRepository>();
                builder.Services.AddScoped<IFamilyMemberRequestService, FamilyMemberRequestService>();

                builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
                builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();

                builder.Services.AddScoped<IUserClaims, UserClaims>();

                var app = builder.Build();

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                        Console.WriteLine(exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });


                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                // Enable our custom middleware
                app.UseMiddleware<RequestBodyLoggingMiddleware>();
                app.UseMiddleware<ResponseBodyLoggingMiddleware>();

                app.UseCors("default");

                // Configure the HTTP request pipeline.
                //if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTracker API v1"));
                }



                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            
        }
    }
}
