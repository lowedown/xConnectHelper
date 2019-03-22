using System.Collections.Generic;

namespace Sitecore.SharedSource.XConnectHelper.Model
{
    public class ServiceStatus
    {
        public ServiceStatus()
        {
            Messages = new List<string>();
        }

        public bool Error { get; set; }
        public string ServiceName { get; set; }
        public IList<string> Messages { get; set; }
    }
}