using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VotingGuide.Objects
{
    /// <summary>
    /// Raw data representing each row from party_answers
    /// </summary>
    public class Parties
    {
        public string question_id { get; set; }
        public string answer { get; set; }
        public string party_id { get; set; }
        public string party_name { get; set; }

        private Parties()
        {

        }
        /// <summary>
        /// Transforms the raw party_answer table data into an array of Party instances.
        /// </summary>
        /// <param name="objectList"></param>
        /// <returns></returns>
        public static Party[] LoadParties(JEnumerable<JObject> objectList)
        {
            List<Parties> partyList = new List<Parties>();
            List<Party> results = new List<Party>();
            foreach (JObject obj in objectList)
            {
                partyList.Add(obj.ToObject<Parties>());
            }
            //x Vai pa onde com essa ideia de iterar dado multiplicado em batch? 
            //! guess ill partyList to do it
            var ids = partyList.GroupBy(c => c.party_id).Select(g => g.First()).Select(x=> x.party_id).ToList();
            foreach (string id in ids)
            {
                var party = new Party();
                var singlePartyAnswers = partyList.Where(x => x.party_id == id).ToArray();
                party.party_id = singlePartyAnswers[0].party_id;
                party.party_name = singlePartyAnswers[0].party_name;
                foreach (Parties parties in singlePartyAnswers)
                {
                    party.Answers.Add(parties.question_id, parties.answer);
                }
                results.Add(party);
            }
            return results.ToArray();
        }

    }
    public class Party
    {
        public string party_id { get; set; }
        public string party_name { get; set; }
        /// <summary>
        /// Dictionary where the Key is the question id and the Value is the answer value (0=agrees 1=disagrees)
        /// </summary>
        public Dictionary<string, string> Answers = new Dictionary<string, string>();
        private Dictionary<string,int> point = new Dictionary<string, int>(); //know the specific question the point counted, good for rollbacking
        public int points 
        { 
            get
            {
                return point.Values.ToArray().Sum();
            } 
        }

        //linq shortcut
        public void AddPoint(string current)
        {
            point.Add(current, 1);
        }
        public void RemovePoint(string current)
        {
            if(point.ContainsKey(current))
                point.Remove(current);
        }
    }
}
