namespace HttpClientDemo.Client.Clients
{
    public interface IStudentClient
    {
        Task<T> GetAsync<T>(string path, string contentType = "application/json");
    }
}