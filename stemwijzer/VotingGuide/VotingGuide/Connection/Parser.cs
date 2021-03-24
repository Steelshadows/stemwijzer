using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VotingGuide.Connection
{
    public class Parser
    {
        public string[] Parse(string url)
        {
            var response = JArray.Parse(Http.Get(url));
            return response.ToObject<string[]>();

        }
        public string GetNoParse(string url)
        {
            return Http.Get(url);
        }

        public JEnumerable<JObject> GetObjectArray(string url)
        {
            JArray array = JArray.Parse(Http.Get(url));
            return array.Children<JObject>();
        }
    }
}
