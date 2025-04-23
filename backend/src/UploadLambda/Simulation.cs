namespace UploadLambda;

public class Simulation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = String.Empty;
    public string FileName { get; set; } = String.Empty;
    public int Downloads { get; set; }
}
