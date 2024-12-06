using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace React1_Backend.Attributes;

public class FavoriteColorValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var colorName = value as string;
        var validColors = new List<string>
        {
            "red",
            "green",
            "blue",
            "yellow",
            "orange",
            "purple",
            "pink",
            "cyan",
            "black",
            "white",
        };

        if (!validColors.Contains(colorName.ToLower()))
            return new ValidationResult("Not a valid color.", new[] { validationContext.MemberName });

        return ValidationResult.Success;
    }
}
