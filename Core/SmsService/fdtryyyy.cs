using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Core.SmsService
{
    public static class fdtryyyy
    {
       
        public static void xyz()
        {
            string? accountSid = Environment.GetEnvironmentVariable("ACdef1adf66cfcffcc1f61dc020e6acf64");
            string? authToken = Environment.GetEnvironmentVariable("fb996316a1b0c68786de6e2b7743fa10");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "This is the ship that made the Kessel Run in fourteen parsecs?",
                from: new Twilio.Types.PhoneNumber("+12407302855"),
                to: new Twilio.Types.PhoneNumber("+8801790208848")
            );
            Console.WriteLine(message.Sid);
        }
    }
}
