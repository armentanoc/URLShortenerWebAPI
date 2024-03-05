using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using URLShortener.Application.Interfaces;
using URLShortener.Infra.Context;
using URLShortener.Infra.Interfaces;
using URLShortener.Infra.Repositories;
using URLShortener.WebAPI.Controllers;
using URLShortener.WebAPI.Middlewares;
namespace URLShortener.WebAPI
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiName = "URL Shortener";

            var builder = WebApplication.CreateBuilder(args);

            //Cors
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("DevEnvPolicy", policyBuilder =>
                {
                    policyBuilder
                    .WithOrigins("http://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            //Add Logging
            builder.Services.AddLogging();

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite((builder.Configuration.GetConnectionString("URLShortenerSqlite")));
            });

            //Repositories
            builder.Services.AddScoped<IUrlRepository, UrlRepository>();

            //Services
            builder.Services.AddScoped<IUrlService, UrlService>();

            //Controllers
            builder.Services.AddControllers(options =>
                {
                    //Custom Exception Filter
                    options.Filters.Add<ExceptionFilter>();
                }
                );

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
                c.EnableAnnotations();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                //Adds Cors Middleware in Dev Environment
                app.UseCors("DevEnvPolicy");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Custom Logging Middleware
            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
