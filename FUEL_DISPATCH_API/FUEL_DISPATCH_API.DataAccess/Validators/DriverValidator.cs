using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class DriverValidator : AbstractValidator<Driver>
    {
         
        public DriverValidator(IDriversServices driversServices)
        {
            RuleFor(x => x.Identification)
                .NotNull()
                .NotEmpty()
                .MinimumLength(13)
                .MaximumLength(13)
                .Must((identification, _) =>
            {
                return driversServices.CheckIfIdIsUnique(identification);
            }).WithMessage("Ya existe un registro con {PropertyName}. Se ingreso {PropertyValue}. ");
            RuleFor(x => x.Email)
                .EmailAddress()
                .Must((driverEmail, _) =>
            {
                return driversServices.IsEmailUnique(driverEmail);
            }).WithMessage("El {PropertyName} ya esta registrado. Se ingreso {PropertyValue}. ")
              .When(x => x.Email is not null);

            RuleFor(x => x.BirthDate)
                 .Must(date => date != DateTime.Today
                 && date <= DateTime.Today.AddYears(-18))
                 .WithMessage("The date of birth cannot be today and must correspond to a person over 18 years of age. ");
            // DONE: Agregar validacion para la fecha de expiracion de la licencia.
            RuleFor(x => x.LicenceExpDate).Must(date => date > DateTime.Today)
                 .WithMessage("The date of birth cannot be today and must correspond to a person over 18 years of age. ");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .NotEmpty();
        }
    }
}
