using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SimulatorUI.Api;

public interface IApiManager
{
    Task UploadSimulation(string simulationName, string simulationData);
}

public class ApiManager : IApiManager
{
    private readonly string _contentType = "text/plain";
    private readonly HttpClient _httpClient = new();

    public ApiManager(IConfiguration configuration)
    {
        var apiUrl = configuration["ApiUrl"] ?? throw new InvalidDataException("ApiUrl missing");
        var apiKey = configuration["ApiKey"] ?? throw new InvalidDataException("ApiKey missing");
        _httpClient.BaseAddress = new Uri(apiUrl);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
    }

    public async Task UploadSimulation(string simulationName, string simulationData)
    {
        var simulationUploadRequest = new SimulationUploadRequest 
        { 
            Name = simulationName, 
            ContentType = _contentType, 
        };
        var body = new StringContent(JsonSerializer.Serialize(simulationUploadRequest));

        var response = await _httpClient.PostAsync("/v1/upload", body);
        var data = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<ApiError>(data);
            throw new HttpRequestException(error?.Message);
        }

        var simulationUploadResult = JsonSerializer.Deserialize<SimulationUploadResponse>(data);

        // upload to S3 from simulationUploadResult and simulationData
    }
}
