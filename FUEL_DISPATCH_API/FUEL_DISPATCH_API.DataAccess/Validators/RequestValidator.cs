using Azure.Core;
using FluentValidation;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator() { }   
    }
}
