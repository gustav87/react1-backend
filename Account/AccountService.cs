using React1_backend.Contracts;

using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace React1_backend.Account;

public class AccountService
{
  private readonly IMongoCollection<Account> _accountCollection;
  private readonly string adminToken = Environment.GetEnvironmentVariable("adminToken") ?? "";
  private readonly string userToken = Environment.GetEnvironmentVariable("userToken") ?? "";

  public AccountService(
    IOptions<AccountDatabaseSettings> accountDatabaseSettings)
  {
    string mongoConnectionString = Environment.GetEnvironmentVariable("mongoConnectionString")  ?? "mongodb://localhost:27017";

    MongoClientSettings mongoClientSettings = MongoClientSettings.FromConnectionString(mongoConnectionString);
    mongoClientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
    mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(5);
    var mongoClient = new MongoClient(mongoClientSettings);
    var mongoDatabase = mongoClient.GetDatabase("react1-backend");
    _accountCollection = mongoDatabase.GetCollection<Account>("Account");
  }

  public async Task<string> LogIn(Account req)
  {
    Console.WriteLine($"Log in attempt from {req.Username}.");
    Account? res = await GetAsyncByUsername(req.Username);
    if (res != null && SecretHasher.Verify(req.Password, res.Password))
    {
      return res.Admin ? adminToken : userToken;
    }
    return "";
  }

  public async Task CreateAccount(Account req)
  {
    Account hashedAccount = new Account() { Username = req.Username, Password = SecretHasher.Hash(req.Password)};
    await _accountCollection.InsertOneAsync(hashedAccount);
  }

  public async Task<List<Account>> GetAsync() =>
    await _accountCollection.Find(_ => true).ToListAsync();

  public async Task<Account?> GetAsync(string id) =>
    await _accountCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

  public async Task<Account?> GetAsyncByUsername(string username) =>
    await _accountCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

  public async Task CreateAsync(Account account) =>
    await _accountCollection.InsertOneAsync(account);

  public async Task UpdateAsync(string id, Account updatedAccount) =>
    await _accountCollection.ReplaceOneAsync(x => x.Id == id, updatedAccount);

  public async Task RemoveAsync(string id) =>
    await _accountCollection.DeleteOneAsync(x => x.Id == id);

  // private static Account ConvertAccountToHashed(Account input)
  // {
  //         using SHA256 sha = SHA256.Create();
  //         byte[] asBytes = Encoding.UTF8.GetBytes(input.Password);
  //         string hashed = Convert.ToBase64String(sha.ComputeHash(asBytes));
  //         Account hashedAccount = new Account() { Username = input.Username, Password = hashed };
  //         return hashedAccount;
  // }
}
