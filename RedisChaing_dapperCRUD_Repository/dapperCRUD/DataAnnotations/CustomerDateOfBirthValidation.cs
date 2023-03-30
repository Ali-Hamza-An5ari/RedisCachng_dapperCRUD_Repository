using System.ComponentModel.DataAnnotations;

namespace dapperCRUD.DataAnnotations
{
    public class CustomerDateOfBirthValidation: ValidationAttribute
    {
        public const string MINIMUM_DATE_OF_BIRTH = "The Customer is younger than 18 years";
        private int minimumAge = 18;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            
            var valueString = value != null ? value.ToString() : null;

            //If dob is empty, show Success
            if(string.IsNullOrWhiteSpace(valueString))
            {
                return ValidationResult.Success;
            }

            //If dob cannot be parsed in Date then it is invalid
            if(!DateTime.TryParse(valueString, out DateTime dob))
            {
                return new ValidationResult("Unable to convert the date of birth to a valid date");

            }

            //Check if age is under minimum age
            var minDateOfBirth = DateTime.Now.Date.AddYears(minimumAge * -1);

            if(dob >  minDateOfBirth)
            {
                return new ValidationResult(MINIMUM_DATE_OF_BIRTH);
            }
            return ValidationResult.Success;
        }
    }
}
