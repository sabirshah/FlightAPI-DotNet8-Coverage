using FlightInformationAPI.DBContext;
using FlightInformationAPI.Interfaces;
using FlightInformationAPI.Services;
using FlightInformationAPI.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json.Serialization;
using System.Text.Json;
using FlightInformationAPI.Middlewares;
using FlightInformationAPI.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace FlightInformationAPI
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add In-Memory EF Core
            builder.Services.AddDbContext<FlightDbContext>(options =>
                options.UseInMemoryDatabase("FlightInformation"));

                builder.Services.AddControllers()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                  });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Register FlightService for IFlightService
            builder.Services.AddScoped<IFlightService, FlightService>();

            //Register Services related to flight information
            builder.Services.AddTransient<IFlightCsvLoader, FlightCsvLoader>();
            builder.Services.AddTransient<FlightDataSeeder>();

            //Register Validators
            builder.Services.AddValidatorsFromAssemblyContaining<FlightUpdateDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<FlightCreateDtoValidator>();
            builder.Services.AddFluentValidationAutoValidation();
      
            var app = builder.Build();

            //Seed the memory with flight data
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<FlightDataSeeder>();
                seeder.Seed("Data/FlightInformation.csv");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
