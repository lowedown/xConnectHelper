using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.XConnectHelper.Model
{
    public class SessionData
    {
        public string GeoCountry { get; set; }
        public string GeoCity { get; set; }
        public string Channel { get; set; }
        public string EngagementValue { get; set; }
        public IEnumerable<string> ProfileData { get; set; }
        public string RobotDetection { get; set; }
        public string CampaignId { get; set; }
    }
}