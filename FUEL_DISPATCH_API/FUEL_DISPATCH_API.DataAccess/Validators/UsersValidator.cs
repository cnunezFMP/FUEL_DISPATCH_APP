using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;

using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class UsersValidator : AbstractValidator<User>
    {
        public UsersValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull();
            RuleFor(x => x.Username).MinimumLength(8).MaximumLength(15).NotEmpty().NotNull();
        }
    }
}
