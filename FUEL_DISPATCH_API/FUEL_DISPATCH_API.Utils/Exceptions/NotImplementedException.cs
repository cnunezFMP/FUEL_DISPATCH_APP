using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.Utils.Exceptions
{
    public class NotImplementedException : Exception
    {
        public NotImplementedException(string msg) : base(msg) { }
    }
}
