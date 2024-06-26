using FluentValidation.Results;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public static class ModelStateResult
    {
        public static ModelStateDictionary GetModelStateDic(ValidationResult validationResult)
        {
            var modelstateDictionary = new ModelStateDictionary();
            foreach (ValidationFailure validationFailure in validationResult.Errors)
            {
                modelstateDictionary.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }
            return modelstateDictionary;
        }
    }
}
