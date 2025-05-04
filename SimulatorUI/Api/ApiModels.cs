namespace SimulatorUI.Api;

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
