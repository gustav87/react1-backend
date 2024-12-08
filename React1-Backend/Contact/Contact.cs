using System.ComponentModel.DataAnnotations;

namespace React1_Backend.Contact;

public class ContactData
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(400, ErrorMessage = "Message exceeds maximum length of 400 characters.")]
    public string Message { get; set; }
}
