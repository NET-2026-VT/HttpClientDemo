using HttpClientDemo.Models.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HttpClientDemo.Client.Clients
{
    public class StudentClient
    {
        private readonly HttpClient _httpClient;
        private const string json = "application/json";

        public StudentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7045/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(json));
        }

        public async Task<IEnumerable<StudentDto>> GetWithRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/students");

            //Behövs inte om vi vill köra standard, eftersom den redan är satt som default i constructorn. 
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var students = JsonSerializer.Deserialize<IEnumerable<StudentDto>>(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return students; 
        }
    }
}
