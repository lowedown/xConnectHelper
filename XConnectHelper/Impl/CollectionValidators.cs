using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using System;
using System.Linq;

namespace Sitecore.SharedSource.XConnectHelper.Impl
{
    public class CollectionValidator : XConnectValidator
    {
        public CollectionValidator(string serviceName, string connectionStringName) : base(serviceName, connectionStringName)
        {
        }

        public new void Validate()
        {
            base.Validate();
            if (Error) return;
            ValidateReadWrite();
        }

        public void ValidateReadWrite()
        {
            try
            {
                using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
                {
                    if (Tracker.Current.Contact.IsNew)
                    {
                        var manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;

                        Tracker.Current.Contact.ContactSaveMode = ContactSaveMode.AlwaysSave;
                        manager.SaveContactToCollectionDb(Tracker.Current.Contact);
                        manager.RemoveFromSession(Tracker.Current.Contact.ContactId);
                        Tracker.Current.Session.Contact = manager.LoadContact(Tracker.Current.Contact.ContactId);
                    }
                    else
                    {
                        var id = Tracker.Current.Contact.Identifiers.FirstOrDefault();
                        client.Get<Contact>(new IdentifiedContactReference(id.Source, id.Identifier), new ContactExpandOptions());
                    }

                    Messages.Add("Status: OK");
                    Error = false;
                }
            }
            catch (XdbCollectionUnavailableException ex)
            {
                Messages.Add($"NOT AVAILABLE: {ex.Message}");
                Error = true;
            }
            catch (Exception ex)
            {
                Messages.Add($"FAILED: {ex.Message}");
                Error = true;
            }
        }        
    }
}