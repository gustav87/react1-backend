using React1_Backend.Account;
using React1_Backend.Contact;
using FluentValidation.TestHelper;
namespace React1_Backend_Tests;

[TestFixture]
public class ContactTests
{
    private AccountService _accountService;
    private ContactValidator validator;

    [SetUp]
    public void Setup()
    {
        _accountService = new AccountService();
        validator = new ContactValidator();
    }

    [Test]
    public void ContactData_Name_is_null_and_Email_is_invalid()
    {
        // Arrange
        ContactData model = new()
        {
            Name = null,
            Email = "invalid_email.com",
            Message = "hello there!",
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldNotHaveValidationErrorFor(x => x.Message);
    }
}
