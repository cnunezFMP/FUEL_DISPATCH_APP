using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class UsersRolsValidator : AbstractValidator<UsersRols>
    {
        public UsersRolsValidator(IUsersRolesServices usersRolesServices)
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("El usuario es requerido. ");
            RuleFor(x => x.RolId)
                .NotEmpty()
                .WithMessage("El rol es requerido. ");
            RuleFor(x => x)
                .Must(x => usersRolesServices.IsUserRol(x.UserId, x.RolId));
        }
    }
}
