using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace SimulatorUI.Api;

public interface IApiManager
{
    Task UploadSimulation(string simulationName);
}

public class ApiManager : IApiManager
{
    private readonly string _apiUrl;
    private readonly string _apiKey;
    private readonly string _contentType = "text/plain";
    private readonly HttpClient _httpClient = new();

    public ApiManager(IConfiguration configuration)
    {
        if (configuration["ApiUrl"] == null || configuration["ApiKey"] == null)
        {
            throw new InvalidDataException("Configuration missing");
        }
        _apiUrl = configuration["ApiUrl"] ?? string.Empty;
        _apiKey = configuration["ApiKey"] ?? string.Empty;
        _httpClient.BaseAddress = new Uri(_apiUrl);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
    }

    public async Task UploadSimulation(string simulationName)
    {
        var body = new SimulationUploadRequest
        {
            Name = simulationName,
            ContentType = _contentType,
        };

        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/upload", body);

        var data = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Success: {data}");
    }
}
