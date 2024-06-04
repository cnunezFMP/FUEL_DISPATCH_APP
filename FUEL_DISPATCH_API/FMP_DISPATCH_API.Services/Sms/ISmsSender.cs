using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMP_DISPATCH_API.Services.Sms
{
    public interface ISmsSender
    {
       void SendSms(string from, string to, string body);
    }
}
