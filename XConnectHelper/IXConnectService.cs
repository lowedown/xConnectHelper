using Sitecore.SharedSource.XConnectHelper.Model;
using System.Collections.Generic;

namespace Sitecore.SharedSource.XConnectHelper.Helper
{
    internal interface IXConnectService
    {
        ContactData Contact
        {
            get;
        }

        bool IsTrackerActive { get; }
        
        void FlushSession();
        void SetIdentifier(string id, string source);
        ServiceStatus GetStatus();
        IEnumerable<string> ValidateConfig();
        void SetContactData(string firstName, string lastName, string email);
    }
}
