using React1_Backend.Contracts;
using React1_Backend.Account;
namespace React1_Backend_Tests;

public class UnitTest1
{
    private AccountService _accountService;

    [SetUp]
    public void Setup()
    {
        _accountService = new AccountService();
    }

    [Test]
    public void TestPass()
    {
        Assert.Pass();
    }

    [Test]
    public void TestStartsWithA()
    {
        string[] words = ["Alphabet", "Aebra", "ABC"];
        foreach (string word in words)
        {
            bool result = word.StartsWith('A');
            Assert.IsTrue(result, string.Format("Expected for '{0}': true; Actual: {1}", word, result));
        }
    }

    [Test]
    public void TestCloudFile()
    {
        CloudFile cloudFile = new();
        Assert.IsNotNull(cloudFile);
        Assert.That(cloudFile, Is.Not.Null);
    }

    [Test]
    public void TestLoginIsEmpty()
    {
        Account account = new();
        Assert.That(_accountService.IsEmpty(account), Is.True);
        account.Username = "fish";
        Assert.That(_accountService.IsEmpty(account), Is.True);
        account.Password = "cheese";
        Assert.That(_accountService.IsEmpty(account), Is.False);
    }
}
