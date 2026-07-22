
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revision_of_Data_Seeding.CustomExcptions;
using Revision_of_Data_Seeding.Identity;
using Revision_of_Data_Seeding.Middlewares;
using Revision_of_Data_Seeding.Models;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Revision_of_Data_Seeding.Interfaces;
using Revision_of_Data_Seeding.Services;

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
            builder.Services.AddExceptionHandler<GlobalExceptionHanlder>();
            builder.Services.AddProblemDetails();

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


            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;

            })
                .AddEntityFrameworkStores<PesonsDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthorization(op =>
            {
                op.AddPolicy("NotAuthorized", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity.IsAuthenticated;
                    });
                });
            });



            var jwtSetting = builder.Configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSetting["Key"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSetting["Issuer"],
                        ValidAudience = jwtSetting["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });


            builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

            //builder.Services.AddCors(opt =>
            //{
            //    opt.AddDefaultPolicy(policyBuilder =>
            //    {
            //        policyBuilder.WithOrigins("http://localhost:4200")
            //              .AllowAnyHeader()
            //              .AllowAnyMethod()
            //              .AllowCredentials();
            //    });
            //});


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("https://localhost:4200") // your actual Angular dev URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();   // required for cookies to work cross-origin
                });
            });

            var app = builder.Build();



            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.MapOpenApi();
            }
            else
            {
                app.UseExceptionHandler();
                app.UseExceptionHandlerMiddleware();
            }
            app.UseStatusCodePages();

            //app.UseHttpLogging();
            //app.Logger.LogDebug("Debug");
            //app.Logger.LogInformation("Infooo");
            //app.Logger.LogWarning("!!!Warning!!!");
            //app.Logger.LogError("Errorrrrrrrrr");
            //app.Logger.LogCritical("Criticallllllll@$$%^&*");

            app.UseCors("AllowAngularApp");
            app.UseHsts();
            app.UseHttpsRedirection();

            //app.UseCors();
            //app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseRouting();
            app.MapControllers();

            app.Run();

        }
    }
}
