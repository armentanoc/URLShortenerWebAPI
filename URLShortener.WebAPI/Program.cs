using URLShortener.Application.Interfaces;
using URLShortener.Infra.Interfaces;
using URLShortener.Infra.Repositories;
using URLShortener.WebAPI.Filters;
namespace URLShortener.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Repositories
            builder.Services.AddScoped<IUrlRepository, UrlRepository>();
            
            //Services
            builder.Services.AddScoped<IUrlService, UrlService>();

            //Controllers
            builder.Services.AddControllers(
                options => options.Filters.Add<LoggingFilter>()
             );
          
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //Custom Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
