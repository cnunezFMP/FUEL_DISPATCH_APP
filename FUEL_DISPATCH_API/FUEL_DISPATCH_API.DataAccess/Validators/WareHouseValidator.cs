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
    public class WareHouseValidator : AbstractValidator<WareHouse>
    {
        public WareHouseValidator(IWareHouseServices wareHouseServices)
        {
            RuleFor(x => x.Code).NotEmpty().NotNull().Must((wareHouse, _) =>
            {
                return wareHouseServices.WareHouseExists(wareHouse);
            }).WithName("Codigo Existente. ").WithMessage("Al parecer hay un Almacen con este codigo. Se ingreso {PropertyValue}. ");
            RuleFor(x => x.BranchOfficeId).Must((wareHouse, _) =>
            {
                return wareHouseServices.BranchOfficeExist(wareHouse);
            }).WithMessage("No existe la sucursal con el Id proporcionado. Se ingreso {PropertyValue}");
        }
    }
}
