using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models.SAP
{
    public class ErrorResponseModel
    {
        public Error? Error { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public Message? Message { get; set; }
    }

    public class Message
    {
        public string? Lang { get; set; }
        public string? Value { get; set; }
    }

}
