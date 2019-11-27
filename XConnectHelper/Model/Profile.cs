using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.XConnectHelper.Model
{
    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }
        public IEnumerable<KeyValuePair<string, double>> Values { get; set; }
    }
}