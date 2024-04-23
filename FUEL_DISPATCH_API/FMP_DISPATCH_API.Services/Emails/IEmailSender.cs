using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMP_DISPATCH_API.Services.Emails
{
    public interface IEmailSender
    {
        void SendEmailAsync(/*string from,*/ string to, string subject, string message);
    }
}
