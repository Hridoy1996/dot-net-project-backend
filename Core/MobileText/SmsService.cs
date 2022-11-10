using Contract;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Core.SmsService
{
    public class SmsService : ISmsService
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<bool> SendTextMessageAsync(string message)
        {
            try
            {
                var values = new List<KeyValuePair<string, string>>();

                values.Add(new KeyValuePair<string, string>("token", "86402347091667929629815a930304ec405be011116e71a7799a")); 
                values.Add(new KeyValuePair<string, string>("to", "01790208848")); 
                values.Add(new KeyValuePair<string, string>("message", "message")); 

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
