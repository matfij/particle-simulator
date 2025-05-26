namespace SimulatorUI.Api;

public class SimulationDownloadRequest
{
    public required string SimulationId;
}

public class SimulationDownloadResponse
{
    public required string DownloadUrl;
}

public class SimulationsPreviewResponse
{
    public required IEnumerable<SimulationPreview> Simulations { get; init; }
    public string? PaginationToken { get; init; }
}

public class SimulationPreview
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string FileName { get; init; }
    public required int Downloads { get; init; }
}

public class SimulationUploadRequest
{
    public required string Name { get; init; }
    public required string ContentType { get; init; }
}

public class SimulationUploadResponse
{
    public required string UploadUrl { get; init; }
    public required string FileKey { get; init; }
}

public class ApiError
{
    public required string Message { get; init; }
}
