
using CloudMiddleService.Services;
using Microsoft.Extensions.Hosting;

namespace CloudMiddleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Đăng ký MySettings để đọc từ appsettings.json
            var configuration = builder.Configuration;

            //Strong-typed appsettings
            builder.Services.Configure<Settings>(configuration.GetSection("AppSettings"));
            var settings = configuration.GetSection("AppSettings").Get<Settings>();

            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddHttpClient();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //Add background service            
            AddCustomServices(builder.Services);
            

            //CORS config
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            if (settings?.HttpsForce == true)
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors();

            app.Run();
        }


        /// <summary>
        /// Custom service registration
        /// </summary>
        /// <param name="provider"></param>
        static void AddCustomServices(IServiceCollection provider)
        {
            provider.AddHostedService<SimpleServices>();

            //Add your custom service below
           // provider.AddHostedService<ServiceA>();
        }
    }
}
