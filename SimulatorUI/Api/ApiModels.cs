namespace SimulatorUI.Api;

public class SimulationPreview
{
    public required IReadOnlyList<Simulation> Simulations { get; init; }
    public required string PaginationToken { get; init; }
}

public class Simulation
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string FileName { get; set; }
    public required int Downloads { get; set; }
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
