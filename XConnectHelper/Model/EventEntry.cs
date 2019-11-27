using System;

namespace Sitecore.SharedSource.XConnectHelper.Model
{
    public class EventEntry
    {
        public DateTime Timestamp { get; set; }
        public string Title { get; set; }
        public int EngagementValue { get; set; }
    }
}