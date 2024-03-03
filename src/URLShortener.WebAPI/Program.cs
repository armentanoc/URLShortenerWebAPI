using URLShortener.Application.Interfaces;
using URLShortener.Infra.Interfaces;
using URLShortener.Infra.Repositories;
using URLShortener.WebAPI.Middlewares;
namespace URLShortener.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add Logging
            builder.Services.AddLogging();

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
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Custom Logging Middleware
            app.UseMiddleware<LoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
