using System.Collections.Generic;

namespace Sitecore.SharedSource.XConnectHelper.Model
{
    public class ContactData
    {
        public string ContactId { get; set; }
        public string TrackerContactId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PreferredEmail { get; set; }
        public IEnumerable<string> Emails { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Identifiers { get; set; }

        public ContactData()
        {
            Emails = new List<string>();
            Identifiers = new List<KeyValuePair<string, string>>();
        }
    }
}