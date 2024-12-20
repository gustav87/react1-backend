using System.Threading.Tasks;
using React1_Backend.Account;

namespace React1_Backend_IntegrationTests;

public class AccountTests : IntegrationTestBase
{
    private AccountService _accountService;

    [SetUp]
    public void LocalSetup()
    {
        AccountRepository accountRepository = new(_mongoClient, "IntegrationTests");
        _accountService = new AccountService(accountRepository);
        // IMongoDatabase mongoDatabase = _mongoClient.GetDatabase("IntegrationTests");
        // _accountCollection = mongoDatabase.GetCollection<Account>("Account");
    }

    [Test]
    public void IntegrationTest1()
    {
        Assert.Pass();
    }

    [Test]
    public async Task IntegrationTest2()
    {
        // Arrange
        string username = "Linda";
        SignUpData signUpData = new() { Username = username, Password = "asdf", FavoriteAnimal = "asdf", FavoriteColor = "YELLOw", FavoriteNumber = 11 };

        // Act
        await _accountService.CreateAccount(signUpData);
        Account account = await _accountService.GetAsyncByUsername(username);

        // Assert
        Assert.That(account, Is.Not.Null);
    }
}
