using MongoDB.Bson.Serialization.Attributes;
using React1_Backend.Attributes;
using System.ComponentModel.DataAnnotations;

namespace React1_Backend.Account;

public class Account
{
    [BsonId]
    public string Id => Username;
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Admin { get; set; } = false;
}

public class SignUpData
{
    [Required(ErrorMessage = "{0} is required.")]
    [RegularExpression(@"^[a-zA-Z0-9]{2,30}$", ErrorMessage = "{0} must be between 2 and 30 alphanumeric characters.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must been between {1} and {2} characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [StringLength(30, ErrorMessage = "{0} exceeds maximum length of {1} characters.")]
    [Display(Name = "Favorite animal")]
    public string FavoriteAnimal { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [FavoriteColorValidation]
    [Display(Name = "Favorite color")]
    public string FavoriteColor { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a non-negative integer.")]
    [Display(Name = "Favorite number")]
    public int? FavoriteNumber { get; set; }
}
