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
                    driversServices.CheckIfIdIsUnique(identification))
                .WithMessage("Ya existe un registro con esta esta identificacion. Se ingreso {PropertyValue}. ");

            RuleFor(x => x.Email)
                .EmailAddress()
                .Must((driverEmail, _) =>
            driversServices.IsEmailUnique(driverEmail))
                .WithMessage("El {PropertyName} ya esta registrado. Se ingreso {PropertyValue}. ")
              .When(x => x.Email is not null);

            RuleFor(x => x.BirthDate)
                 .Must(date => date != DateTime.Today
                 && date <= DateTime.Today.AddYears(-18))
                 .WithMessage("La fecha de nacimiento no puede ser la actual, y debe corresponder alguien de 18+ ");
            // DONE: Agregar validacion para la fecha de expiracion de la licencia.
            RuleFor(x => x.LicenceExpDate)
                .Must(date => date > DateTime.Today)
                .WithMessage("Tu licencia debe ser válida. Por favor, asegúrate de que la fecha de vencimiento de tu licencia sea posterior a la fecha de hoy. ");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .NotEmpty();
        }
    }
}
