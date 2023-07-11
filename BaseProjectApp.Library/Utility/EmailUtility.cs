using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks; 

namespace BaseProjectApp.Library.Utility
{
    public class EmailUtility
    {
        public static string SendEmail(string To, string Subject, string bodyHtml)
        { 

            NameValueCollection values = new NameValueCollection
            {
                { "apikey", "..." },
                { "from", "From email" },
                { "fromName", "From Name" },
                { "to", To },
                { "subject", Subject },
                { "bodyHtml", bodyHtml },
                { "isTransactional", "true" }
            };
            string address = "Address-URL";

            string response = Send(address, values);

            return response;
        }
         

        static string Send(string address, NameValueCollection values)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    byte[] apiResponse = client.UploadValues(address, values);
                    return Encoding.UTF8.GetString(apiResponse);

                }
                catch (Exception ex)
                {  
                    Console.WriteLine(ex.Message);
                    throw ex; 

                }
            }
        }
    }
}
