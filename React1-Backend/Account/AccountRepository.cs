using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace React1_Backend.Account;

public class AccountRepository
{
    private readonly IMongoCollection<Account> _accountCollection;

    public AccountRepository()
    {
        string mongoConnectionString = Environment.GetEnvironmentVariable("mongoConnectionString") ?? "mongodb://localhost:27017";

        MongoClientSettings mongoClientSettings = MongoClientSettings.FromConnectionString(mongoConnectionString);
        mongoClientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(5);
        var mongoClient = new MongoClient(mongoClientSettings);
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase("React1-Backend");
        _accountCollection = mongoDatabase.GetCollection<Account>("Account");
    }

    public async Task<List<Account>> GetAsync() =>
      await _accountCollection.Find(_ => true).ToListAsync();

    public async Task<Account> GetAsync(string id) =>
        await _accountCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Account> GetAsyncByUsername(string username) =>
        await _accountCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

    public async Task CreateAsync(Account account) =>
        await _accountCollection.InsertOneAsync(account);

    public async Task UpdateAsync(string id, Account updatedAccount) =>
        await _accountCollection.ReplaceOneAsync(x => x.Id == id, updatedAccount);

    public async Task RemoveAsync(string id) =>
        await _accountCollection.DeleteOneAsync(x => x.Id == id);
}
