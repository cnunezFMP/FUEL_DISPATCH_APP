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
    public class DispatchValidator : AbstractValidator<WareHouseMovement>
    {
        public DispatchValidator(IWareHouseMovementServices wareHouseMovementServices)
        {
            RuleFor(x => x.Qty).Must((qty, _) =>
            {
                return wareHouseMovementServices.QtyCantBeZero(qty);
            }).WithMessage("{PropertyName} no puede ser cero. ");
            RuleFor(x => x.Odometer).Must((odometer, _) =>
            {
                return wareHouseMovementServices.CheckPreviousDispatch(odometer);
            }).WithMessage("{PropertyName} no puede ser menor o igual al anterior de el vehiculo especificado. Se ingreso {PropertyValue}");
        }
    }
}
