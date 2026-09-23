using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HttpClientDemo.Data;
using HttpClientDemo.Extensions;

namespace HttpClientDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            
            builder.Services.AddDbContext<UniversityContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("UniversityContext") ?? throw new InvalidOperationException("Connection string 'UniversityContext' not found.")));


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.EnableAnnotations();
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });
            
            //builder.Services.AddOpenApi();

            var app = builder.Build();





            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
