
using Microsoft.EntityFrameworkCore;
using Revision_of_Data_Seeding.Models;
using Serilog;

namespace Revision_of_Data_Seeding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Host.UseSerilog((context, services, loggerConfig) =>
            {
                loggerConfig.ReadFrom.Configuration(context.Configuration);
                loggerConfig.ReadFrom.Services(services);
            });



            builder.Services.AddControllers();
            builder.Services.AddDbContext<PesonsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();
            //builder.Logging.AddDebug();
            //builder.Logging.AddEventLog();


            builder.Services.AddHttpLogging(op =>
            {
                op.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
            }
            //app.UseHttpLogging();
            //app.Logger.LogDebug("Debug");
            //app.Logger.LogInformation("Infooo");
            //app.Logger.LogWarning("!!!Warning!!!");
            //app.Logger.LogError("Errorrrrrrrrr");
            //app.Logger.LogCritical("Criticallllllll@$$%^&*");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
