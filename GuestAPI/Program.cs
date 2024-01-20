using GuestAPI.ApiMiddlewareAuthentication;
using GuestAPI.Authentication;
using GuestAPI.Data;
using GuestAPI.Handlers.CommandHandlers;
using GuestAPI.Handlers.ICommandHandlers;
using GuestAPI.Handlers.IQueryHandlers;
using GuestAPI.Handlers.QueryHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace GuestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Creating a Serilog logger configuration to write log events to the console.
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GuestAPI", Version = "v1" });

                // Add security definition for API key
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Description = "API key needed for authentication"
                });

                // Add security requirement for API key
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                  Type = ReferenceType.SecurityScheme,
                                    Id = "ApiKey"
                            }
                        },
                          Array.Empty<string>()
                    }
                });
            });

            //Configures authentication for the API using the "ApiKey" scheme.
            builder.Services.AddAuthentication("ApiKey")
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", configureOptions: null);

            //Adds the application's database context, configured to use an in-memory database.
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDatabase"));

            // Configuring Serilog as the logger for the application.
            builder.Host.UseSerilog();

            builder.Services.AddScoped<IAddGuestCommandHandler,AddGuestCommandHandler>();
            builder.Services.AddScoped<IUpdateGuestCommandHandler, UpdateGuestCommandHandler>();
            builder.Services.AddScoped<IAddPhoneCommandHandler, AddPhoneCommandHandler>();
            builder.Services.AddScoped<IGetAllGuestsQueryHandler, GetAllGuestsQueryHandler>();
            builder.Services.AddScoped<IGetGuestQueryHandler, GetGuestQueryHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Adding the custom API key middleware to the application pipeline.
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSerilogRequestLogging();


            app.MapControllers();

            app.Run();
        }
    }
}