using HttpClientDemo.Client.Clients;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStudentClient _studenClient; 
        private const string json = "application/json"; 

        public HomeController(HttpClient httpClient, IHttpClientFactory httpClientFactory, IStudentClient studentClient)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7045/");
            _httpClientFactory = httpClientFactory;
            _studenClient = studentClient; 


            //Kan rent praktiskt sätta den här om man vill att den ska sättas på alla requests.
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json)); 
        }

        public async Task<IActionResult> Index()
        {
            //var res = await SimpleGet();
            //var res = await GetWithRequestMessage(); 
            ////var res = await CreateStudent();
            //var res2 = await _studenClient.GetWithRequestMessage(); 

            var res = await _studenClient.GetAsync<IEnumerable<StudentDto>>("api/students");
            var res2 = await _studenClient.GetAsync<StudentDto>("api/students/2");
            return View();
        }

        private async Task<StudentDto> CreateStudent()
        {
            //Ändrar så att den använder vår factory med name
            var httpClient = _httpClientFactory.CreateClient("StudentsClient"); 

            var request = new HttpRequestMessage(HttpMethod.Post, "api/students");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var student = new CreateStudentDto("Kalle", "Anka", "N/A", "Gatan", "12345", "Staden");

            var serializedStudent = JsonSerializer.Serialize(student);

            request.Content = new StringContent(serializedStudent);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(json);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var studentDto = JsonSerializer.Deserialize<StudentDto>(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return studentDto; 
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
