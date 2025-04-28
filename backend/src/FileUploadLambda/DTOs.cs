namespace FileUploadLambda;

public class FileUploadRequest
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
}

public class FileUploadResponse
{
    public required string UploadUrl { get; set; }
    public required string FileKey { get; set; }
}
