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
    [Required(ErrorMessage = "Username is required.")]
    [RegularExpression(@"^[a-zA-Z0-9]{2,30}$", ErrorMessage = "Username must be between 2 and 30 alphanumeric characters.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, ErrorMessage = "Password exceeds maximum length of 30 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Favorite animal is required.")]
    [StringLength(30, ErrorMessage = "Favorite animal exceeds maximum length of 30 characters.")]
    public string FavoriteAnimal { get; set; }

    [Required(ErrorMessage = "Favorite color is required.")]
    [FavoriteColorValidation]
    public string FavoriteColor { get; set; }

    [Required(ErrorMessage = "Favorite number is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a non-negative integer.")]
    public int FavoriteNumber { get; set; }
}
