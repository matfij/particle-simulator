using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SimulatorUI.Api;

public interface IApiManager
{
    Task<IEnumerable<SimulationPreview>> DownloadSimulationsPreview();
    Task UploadSimulation(string simulationName, string simulationData);
    Task<Stream> DownloadSimulation(string simulationId);
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

    public async Task<IEnumerable<SimulationPreview>> DownloadSimulationsPreview()
    {
        var response = await _httpClient.GetAsync($"{_apiUrl}/v1/preview");
        var data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<ApiError>(data);
            throw new HttpRequestException(error?.Message);
        }

        var simulationsResponse =
            JsonSerializer.Deserialize<SimulationsPreviewResponse>(data)
            ?? throw new HttpRequestException("Invalid data format");

        return simulationsResponse.Simulations;
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
            ?? throw new HttpRequestException("Invalid data format");

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

    public async Task<Stream> DownloadSimulation(string simulationId)
    {
        var simulationDownloadRequest = new SimulationDownloadRequest { SimulationId = simulationId };
        var body = new StringContent(JsonSerializer.Serialize(simulationDownloadRequest));

        var response = await _httpClient.PostAsync($"{_apiUrl}/v1/download", body);
        var data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<ApiError>(data);
            throw new HttpRequestException(error?.Message);
        }

        var downloadResponse =
            JsonSerializer.Deserialize<SimulationDownloadResponse>(data)
            ?? throw new HttpRequestException("Invalid data format");

        return await DownloadSimulationData(downloadResponse.DownloadUrl);
    }

    private async Task<Stream> DownloadSimulationData(string targetUrl)
    {
        var response = await _httpClient.GetAsync(targetUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"S3 download failed: {response.StatusCode}, Error: {error}");
        }

        return await response.Content.ReadAsStreamAsync();
    }
}
