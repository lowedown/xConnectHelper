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
        public IEnumerable<Profile> ProfileData { get; set; }
        public string RobotDetection { get; set; }
        public string CampaignId { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public int Visits { get; set; }
        public int PagesInCurrentVisit { get; set; }
        public string Referrer { get; set; }
        public int GoalsCount { get; set; }
        public int PageEventsCount { get; set; }
        public IEnumerable<EventEntry> Goals { get; set; }
        public IEnumerable<EventEntry> PageEvents { get; set; }
    }
}