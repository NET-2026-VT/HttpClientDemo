using HttpClientDemo.Client.Models;
using HttpClientDemo.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HttpClientDemo.Client.Controllers
{
    public class HomeController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string json = "application/json"; 

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7045/");

            //Kan rent praktiskt sätta den här om man vill att den ska sättas på alla requests.
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json)); 
        }

        public async Task<IActionResult> Index()
        {
            var res = await SimpleGet();
            var res2 = await GetWithRequestMessage(); 
            return View();
        }

        private async Task<IEnumerable<StudentDto>> GetWithRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/students");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var response = await _httpClient.SendAsync(request); 
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var students = JsonSerializer.Deserialize<IEnumerable<StudentDto>>(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return students;

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
