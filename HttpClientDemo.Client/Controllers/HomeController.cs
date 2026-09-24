using HttpClientDemo.Client.Models;
using HttpClientDemo.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace HttpClientDemo.Client.Controllers
{
    public class HomeController : Controller
    {

        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7045/");
        }

        public async Task<IActionResult> Index()
        {
            var res = await SimpleGet();
            return View();
        }

        private async Task<IEnumerable<StudentDto>> SimpleGet()
        {
            var response = await _httpClient.GetAsync("api/students");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var students = JsonSerializer.Deserialize<IEnumerable<StudentDto>>(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return students; 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
