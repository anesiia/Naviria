using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NaviriaAPI.Tests.helper
{
    public static class ValidationHelper
    {
        public static bool ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    // Log or process the validation errors
                    Console.WriteLine(validationResult.ErrorMessage);
                }
            }

            return isValid;
        }
    }
}
