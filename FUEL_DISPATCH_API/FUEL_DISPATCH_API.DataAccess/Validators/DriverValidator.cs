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
                .WithMessage("Debe ingresar su cedula. ");

            RuleFor(x => x.Email)
                .EmailAddress();

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
