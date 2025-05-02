using Amazon.DynamoDBv2.DataModel;

namespace UploadLambda;

public class FileUploadRequest
{
    public required string Name { get; init; }
    public required string ContentType { get; init; }
}

public class FileUploadResponse
{
    public required string UploadUrl { get; init; }
    public required string FileKey { get; init; }
}

public class ApiError
{
    public required string Message { get; init; }
}

[DynamoDBTable("Simulation")]
public class Simulation
{
    [DynamoDBHashKey]
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string FileName { get; init; }
    public int Downloads { get; set; }
}
