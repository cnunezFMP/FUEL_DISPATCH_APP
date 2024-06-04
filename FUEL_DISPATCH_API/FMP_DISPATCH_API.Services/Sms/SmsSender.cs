using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Chat.V1.Service.Channel;
using Twilio.Types;

namespace FMP_DISPATCH_API.Services.Sms
{
    public class SmsSender : ISmsSender
    {
        public void SendSms(string from, string to, string body)
        {
            TwilioClient.Init(
                Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
                Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN"));

            Twilio.Rest.Api.V2010.Account.MessageResource.Create(to: new PhoneNumber("+18297620954"), from: new PhoneNumber("18498506282"));
        }
    }
}
