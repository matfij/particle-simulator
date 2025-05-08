namespace SharedLibrary;

public readonly struct SimulationPreview
{
    public List<Simulation> Simulations { get; init; }
    public string PaginationToken { get; init; }
}

public readonly struct FileUploadRequest
{
    public required string Name { get; init; }
    public required string ContentType { get; init; }
}

public readonly struct FileUploadResponse
{
    public required string UploadUrl { get; init; }
    public required string FileKey { get; init; }
}

public readonly struct ApiError
{
    public required string Message { get; init; }
}
