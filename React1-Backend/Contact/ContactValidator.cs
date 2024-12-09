using FluentValidation;

namespace React1_Backend.Contact
{
    public class ContactValidator : AbstractValidator<ContactData>
    {
        public ContactValidator()
        {
            RuleFor(contactData => contactData.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name exceeded maximum length of max 50 characters.");

            RuleFor(contactData => contactData.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email.")
            .MaximumLength(100)
            .WithMessage("Email exceeded maximum length of 100 characters.");

            RuleFor(contactData => contactData.Message)
            .NotEmpty()
            .WithMessage("Message is required.")
            .Length(10, 400)
            .WithMessage("Message must be between 10 and 400 characters.");
        }
    }
}
