using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using Maestro.Models;

namespace Maestro.CS
{
    public class ApiClient
    {

        public async static Task<string> CallWebService(Request RequestData)
        {
            try
            {
                var _uri = new Uri(RequestData.Url);

                using (var client = new HttpClient())
                {

                    StringContent content = new StringContent(RequestData.Body);

                    content.Headers.Clear();
                    foreach(Header header in RequestData.Headers)
                    {
                        content.Headers.Add(header.Name, header.Value);
                    }

                    content.Headers.ContentType = MediaTypeHeaderValue.Parse(SetMediaType(RequestData.Format.ToString()));

                    //var response = await client.postasync()
                    using (var response = await client.PostAsync(_uri, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            return response.ReasonPhrase;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return HttpStatusCode.InternalServerError.ToString();
            }
            
        }



        private static string SetMediaType(string format)
        {
            switch (format.ToUpper())
            {
                case "XML": return "text/xml";
                case "JSON": return "application/json";
                default: return "text/plain";
            }

        }
    }



    
}
