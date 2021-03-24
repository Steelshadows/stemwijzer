using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VotingGuide.Connection
{
    public class Http
    {
        static HttpClient web = new HttpClient();
        public static string Get(string url)
        {
            var response = web.GetStringAsync(url);
            response.Wait();
            return response.Result;
        }
    }
}
