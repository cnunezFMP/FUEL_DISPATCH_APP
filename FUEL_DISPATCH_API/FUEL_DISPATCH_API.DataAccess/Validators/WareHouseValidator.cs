using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class WareHouseValidator : AbstractValidator<WareHouse>
    {
        public WareHouseValidator(IWareHouseServices wareHouseServices)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull()
                .Must((wareHouse, _) =>
            {
                return wareHouseServices.WareHouseExists(wareHouse);
            }).WithName("Existe un almacen. ")
              .WithMessage("Ya existe un almacen con el codigo proporcionado. ");

            RuleFor(x => x.BranchOfficeId)
                .Must((wareHouse, _) =>
            {
                return wareHouseServices.BranchOfficeExist(wareHouse);
            }).WithName("No existe la sucursal. ")
              .WithMessage("No existe la sucursal con el Id proporcionado. Se ingreso {PropertyValue}");
        }
    }
}
