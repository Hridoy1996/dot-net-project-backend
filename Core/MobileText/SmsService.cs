using Contract;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Core.SmsService
{
    public class SmsService : ISmsService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _config;

        public SmsService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<bool> SendTextMessageAsync(string message, string phoneNumber)
        {
            try
            {
                if(string.IsNullOrEmpty(message) || string.IsNullOrEmpty(phoneNumber))
                {
                    return false;
                }

                var values = new List<KeyValuePair<string, string>>();

                values.Add(new KeyValuePair<string, string>("token", _config["SmsProviderToken"])); 
                values.Add(new KeyValuePair<string, string>("to", phoneNumber)); 
                values.Add(new KeyValuePair<string, string>("message", message)); 

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://api.greenweb.com.bd/api.php?", content);

                var responseString = await response.Content.ReadAsStringAsync();

                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());

                return false;
            }
        }
    }
}
