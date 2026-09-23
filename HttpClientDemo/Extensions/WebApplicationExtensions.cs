using HttpClientDemo.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HttpClientDemo.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using(var scope = app.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<UniversityContext>();

            //await context.Database.EnsureDeletedAsync();
            //await context.Database.MigrateAsync();

            try
            {
                await SeedData.InitAsync(context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
