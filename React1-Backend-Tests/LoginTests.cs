using Moq;
using MongoDB.Driver;
using React1_Backend.Account;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace React1_Backend_Tests;

[TestFixture]
public class LoginTests
{
    private AccountService _accountService;
    protected readonly MongoClient MongoClient = new("mongodb://localhost:27017");

    [SetUp]
    public void Setup()
    {
        AccountRepository accountRepository = new();
        _accountService = new AccountService(accountRepository);
    }

    [Test]
    public void AccountIsEmpty()
    {
        Account account = new();
        Assert.That(_accountService.IsEmpty(account), Is.True);
        account.Username = "fish";
        Assert.That(_accountService.IsEmpty(account), Is.True);
        account.Password = "cheese";
        Assert.That(_accountService.IsEmpty(account), Is.False);
    }

    [Test]
    public void SignUpData_Invalid_Color()
    {
        // Arrange
        SignUpData signUpData = new()
        {
            Username = "test",
            Password = "asd",
            FavoriteAnimal = "dog",
            FavoriteColor = "magenta",
            FavoriteNumber = 1,
        };

        // Act
        var results = ValidateModel(signUpData);

        // Assert
        Assert.That(results.Count, Is.GreaterThan(0));
        Assert.That(results, Has.Count.GreaterThan(0));
        Assert.That(results.Any(v => v.MemberNames.Contains("FavoriteColor")), Is.True);
    }

    [Test]
    public void SignUpData_Invalid_Number()
    {
        // Arrange
        SignUpData signUpData = new()
        {
            Username = "test",
            Password = "asd",
            FavoriteAnimal = "dog",
            FavoriteColor = "red",
            FavoriteNumber = -1,
        };

        // Act
        var results = ValidateModel(signUpData);

        // Assert
        Assert.That(results, Has.Count.EqualTo(1));
        Assert.That(results.Any(v => v.MemberNames.Contains("FavoriteNumber")), Is.True);
    }

    [Test]
    public void SignUpData_Invalid()
    {
        // Arrange
        SignUpData signUpData = new()
        {
            Username = "Mike Johnson", // Contains spaces
            Password = "asdfgasdfgasdfgasdfgasdfgasdfgasdfg", // Exceeds 30 characters
            FavoriteAnimal = "", // No animal provided
            FavoriteColor = "redd", // Invalid color
            FavoriteNumber = -1, // Negative integer
        };

        // Act
        var results = ValidateModel(signUpData);

        // Assert
        Assert.That(results, Has.Count.EqualTo(5));

        // https://github.com/nunit/nunit.analyzers/blob/master/documentation/NUnit2045.md
        // Without Assert.Multiple the test will stop executing after the first failure and a second violation
        // won't be detected until the next run when the first test has been fixed.
        Assert.Multiple(() =>
        {
            Assert.That(results.Any(v => v.MemberNames.Contains("Username")), Is.True);
            Assert.That(results.Any(v => v.MemberNames.Contains("Password")), Is.True);
            Assert.That(results.Any(v => v.MemberNames.Contains("FavoriteAnimal")), Is.True);
            Assert.That(results.Any(v => v.MemberNames.Contains("FavoriteColor")), Is.True);
            Assert.That(results.Any(v => v.MemberNames.Contains("FavoriteNumber")), Is.True);
        });
    }

    [Test]
    public void SignUpData_Valid()
    {
        // Arrange
        SignUpData signUpData = new()
        {
            Username = "Mike",
            Password = "asdf",
            FavoriteAnimal = "asdf",
            FavoriteColor = "YELLOw",
            FavoriteNumber = 11,
        };

        // Act
        var results = ValidateModel(signUpData);

        // Assert
        Assert.That(results, Has.Count.EqualTo(0));
    }

    private static List<ValidationResult> ValidateModel<T>(T model)
    {
        var context = new ValidationContext(model);
        var result = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(model, context, result, true);
        return result;
    }

    [Test]
    [TestCase("mike")]
    [TestCase("jane")]
    public async Task Sign_Up_GetAsync(string username)
    {
        // Arrange
        Mock<IAccountService> accountService = new(MockBehavior.Strict);
        Account expected = new() { Username = username, Password = "asdf", Admin = false };

        // Here we're telling the accountService mock that if its GetAsync method is invoked with this specific username, return the 'expected' Account.
        // If it gets invoked with any other username, fail the test immediately (this is what MockBehavior.Strict does).
        accountService.Setup(x => x.GetAsyncByUsername(username)).ReturnsAsync(expected);

        // Act
        Account actual = await accountService.Object.GetAsyncByUsername(username);

        // Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.SameAs(expected));
        Assert.That(actual.Admin, Is.False);
    }

    [Test]
    public async Task Sign_Up_Admin_Property_Changed()
    {
        // Arrange
        Mock<IAccountService> accountServiceMock = new(MockBehavior.Strict);
        string username = "adam";
        Account propertyChanged = Mock.Of<Account>(x => x.Username == username);
        propertyChanged.Admin = true;

        // accountServiceMock.Setup(x => x.GetAsyncByUsername(username)).ReturnsAsync(propertyChanged);
        accountServiceMock.Setup(x => x.GetAsyncByUsername(username).Result).Returns(propertyChanged); // Equivalent to commented out line above.

        // Act
        Account actual = await accountServiceMock.Object.GetAsyncByUsername(username);

        // Assert
        Assert.That(actual.Admin, Is.True);
    }
}
