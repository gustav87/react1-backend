using MongoDB.Driver;
using React1_Backend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace React1_Backend.Account;

public class AccountRepository(MongoClient mongoClient, string databaseName = "React1-Backend") : MongoDbBase(mongoClient, databaseName)
{
    // public async Task<List<Account>> GetAsync() =>
    //   await _accountCollection.Find(_ => true).ToListAsync();
    public async Task<List<Account>> GetAsync() => await LoadAll<Account>();

    // public async Task<Account> GetAsyncByUsername(string username) =>
    //     await _accountCollection.Find(x => x.Username == username).FirstOrDefaultAsync();
    public async Task<Account> GetAsyncByUsername(string username) =>
        await LoadFirst<Account>(x => x.Username == username);

    // public async Task CreateAsync(Account account) =>
    //     await _accountCollection.InsertOneAsync(account);
    public async Task CreateAsync(Account account) =>
        await Insert<Account>(account);

    // public async Task<Account> GetAsync(string id) =>
    //     await _accountCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // public async Task UpdateAsync(string id, Account updatedAccount) =>
    //     await _accountCollection.ReplaceOneAsync(x => x.Id == id, updatedAccount);

    // public async Task RemoveAsync(string id) =>
    //     await _accountCollection.DeleteOneAsync(x => x.Id == id);
}
