using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace React1_Backend_IntegrationTests;

public class IntegrationTestBase
{
    private readonly string mongoConnectionString = Environment.GetEnvironmentVariable("mongoConnectionString") ?? "mongodb://localhost:27017";
    private readonly string databaseName = "IntegrationTests";
    protected MongoClient _mongoClient;

    [SetUp]
    public async Task Setup()
    {
        MongoClientSettings mongoClientSettings = MongoClientSettings.FromConnectionString($"mongodb://{mongoConnectionString}:27020");
        mongoClientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(5);
        _mongoClient = new MongoClient(mongoClientSettings);
        await _mongoClient.DropDatabaseAsync(databaseName);
    }
}
