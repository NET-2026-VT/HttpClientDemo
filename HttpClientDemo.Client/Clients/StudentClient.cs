using HttpClientDemo.Models.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HttpClientDemo.Client.Clients
{
    public class StudentClient : IStudentClient
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

        public async Task<T> GetAsync<T>(string path, string contentType = json)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(json));

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            return JsonSerializer.Deserialize<T>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


        }
    }
}
