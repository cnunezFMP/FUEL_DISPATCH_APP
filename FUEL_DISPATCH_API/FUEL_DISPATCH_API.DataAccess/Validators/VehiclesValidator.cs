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
    public class VehiclesValidator : AbstractValidator<Vehicle>
    {
        public VehiclesValidator(IVehiclesServices vehiclesServices)
        {
            RuleFor(x => x.Plate).MinimumLength(8).MaximumLength(10);
            RuleFor(x => x.Ficha).NotNull().NotNull().Must((vehicleToken, _) =>
            {
                return vehiclesServices.FichaMustBeUnique(vehicleToken);
            }).WithMessage("Ya existe un vehiculo con {PropertyValue}. Esto ocurrio en {PropertyName}. ");
        }
    }
}
