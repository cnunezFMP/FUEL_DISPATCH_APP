using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;

namespace FUEL_DISPATCH_API.Auth.AuthRepository
{
    public class RegisterValidator : AbstractValidator<User>
    {
        public RegisterValidator(IUsersAuth usersAuthServices)
        {
            RuleFor(x => x.Email).Must((email, _) =>
            {
                return usersAuthServices.IsEmailUnique(email);
            });

            RuleFor(x => x.Email).Must((email, _) =>
            {
                return usersAuthServices.IsEmailUnique(email);
            });

            RuleFor(x => x.Username).MinimumLength(8).MaximumLength(15).NotEmpty().NotNull();
            RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull();
        }
    }
}
