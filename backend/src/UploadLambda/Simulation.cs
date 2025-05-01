using Amazon.DynamoDBv2.DataModel;

namespace UploadLambda;

[DynamoDBTable("Simulation")]
public class Simulation
{
    [DynamoDBHashKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public required string FileName { get; set; }
    public int Downloads { get; set; }
}
