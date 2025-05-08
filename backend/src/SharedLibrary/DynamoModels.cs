using Amazon.DynamoDBv2.DataModel;

namespace SharedLibrary;

[DynamoDBTable("Simulation")]
public class Simulation
{
    [DynamoDBHashKey]
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string FileName { get; init; }
    public int Downloads { get; set; }
}
