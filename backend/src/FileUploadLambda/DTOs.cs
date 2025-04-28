namespace FileUploadLambda;

public class FileUploadRequest
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

public class FileUploadResponse
{
    public string UploadUrl { get; set; } = string.Empty;
    public string FileKey { get; set; } = string.Empty;
}
