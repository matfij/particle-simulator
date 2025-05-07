using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SimulatorUI.Api;

public interface IApiManager
{
    Task UploadSimulation(string simulationName, string simulationData);
}

public class ApiManager : IApiManager
{
    private readonly string _apiUrl;
    private readonly string _contentType = "text/plain";
    private readonly HttpClient _httpClient = new();

    public ApiManager(IConfiguration configuration)
    {
        _apiUrl = configuration["ApiUrl"] ?? throw new InvalidDataException("ApiUrl missing");
        var apiKey = configuration["ApiKey"] ?? throw new InvalidDataException("ApiKey missing");
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

        var response = await _httpClient.PostAsync($"{_apiUrl}/v1/upload", body);
        var data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<ApiError>(data);
            throw new HttpRequestException(error?.Message);
        }

        var simulationUploadResponse = 
            JsonSerializer.Deserialize<SimulationUploadResponse>(data) 
            ?? throw new HttpRequestException("Invalid lambda response");

        await UploadSimulationData(simulationUploadResponse.UploadUrl, simulationData);
    }

    private async Task UploadSimulationData(string targetUrl, string simulationData)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(simulationData));
        using var content = new StreamContent(stream);

        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);

        var response = await _httpClient.PutAsync(targetUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"S3 upload failed: {response.StatusCode}, Error: {error}");
        }
    }
}
