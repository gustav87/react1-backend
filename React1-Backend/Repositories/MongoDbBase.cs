using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace React1_Backend.Repositories;

public class MongoDbBase(MongoClient mongoClient, string databaseName = "React1-Backend")
{
    private readonly MongoClient _mongoClient = mongoClient;
    private readonly string databaseName = databaseName;

    protected IMongoDatabase CreateClient()
    {
        var database = _mongoClient.GetDatabase(databaseName);
        return database;
    }

    protected Task<T> LoadFirst<T>(Expression<Func<T, bool>> expression)
    {
        IMongoCollection<T> mongoCollection = CreateCollection<T>();
        return mongoCollection.FindSync(expression).FirstOrDefaultAsync();
    }

    protected Task<T> LoadFirst<T>(string id) where T : IIdentifiable
    {
        return LoadFirst<T>(x => x.Id == id);
    }

    protected Task Insert<T>(T element)
    {
        IMongoCollection<T> mongoCollection = CreateCollection<T>();
        return mongoCollection.InsertOneAsync(element);
    }

    protected async Task<List<T>> LoadAll<T>()
    {
        IMongoCollection<T> mongoCollection = CreateCollection<T>();
        return await mongoCollection.Find(_ => true).ToListAsync();
    }

    protected IMongoCollection<T> CreateCollection<T>()
    {
        IMongoDatabase mongoDatabase = CreateClient();
        IMongoCollection<T> mongoCollection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        return mongoCollection;
    }

    protected async Task Delete<T>(Expression<Func<T, bool>> deleteQuery)
    {
        var mongoDatabase = CreateClient();
        var mongoCollection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        await mongoCollection.DeleteOneAsync<T>(deleteQuery);
    }

    protected Task Delete<T>(string id) where T : IIdentifiable
    {
        return Delete<T>(x => x.Id == id);
    }
}

public interface IIdentifiable
{
    public string Id { get; }
}
