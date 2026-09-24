using System.Net.Http.Headers;

namespace HttpClientDemo.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //1. Inga konstigheter, skapar en HttpClient och sätta inställningar på.
            builder.Services.AddHttpClient();

            //2. Namnger client, sätter inställningar direkt här istället för att behöva göra det i varje controller. 
            builder.Services.AddHttpClient("StudentsClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7045/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            builder.Services.AddHttpClient("OtherClient", client =>
            {
                client.BaseAddress = new Uri("");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));               
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
