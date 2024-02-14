using System.ComponentModel.DataAnnotations;

namespace DataEntity.Validation
{
    public class ValidPropertyValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                return new ValidationResult("test validasi");
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
