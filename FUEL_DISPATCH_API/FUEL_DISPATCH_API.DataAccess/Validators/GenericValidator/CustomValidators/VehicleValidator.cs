using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators.GenericValidator.CustomValidators
{

    public class VehicleValidator : IGenericValidator<Vehicle>
    {
        public bool Validate(Vehicle entity)
        {
            return entity.Status is not ValidationConstants.InactiveStatus
            && entity.Status is not ValidationConstants.NotAvailableStatus;
        }
    }

}
