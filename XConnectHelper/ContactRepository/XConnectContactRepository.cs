using System;
using System.Linq;
using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;

namespace Sitecore.SharedSource.XConnectHelper.ContactRepository
{
    internal class XConnectContactRepository : IContactRepository
    {
        private Sitecore.XConnect.Contact GetContact(string source, string identifier, string FacetKey)
        {
            using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(FacetKey))
                    {
                        return client.Get<Sitecore.XConnect.Contact>(new IdentifiedContactReference(source, identifier), new ContactExpandOptions());
                    }

                    return client.Get<Sitecore.XConnect.Contact>(new IdentifiedContactReference(source, identifier), new ContactExpandOptions(FacetKey));
                }
                catch (XdbExecutionException ex)
                {
                    Log.Error($"XConnectContactRepository: Unable to get contact for ID '{identifier}', Source: '{source}', FacetKey: '{FacetKey}'", ex, this);
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error("XConnectContactRepository: Error communication with the xConnect colllection service", ex, this);
                    return null;
                }
            }
        }

        public bool SaveFacet<T>(Sitecore.XConnect.Contact contact, string FacetKey, T Facet) where T : Facet
        {
            using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    client.SetFacet(contact, FacetKey, Facet);
                    client.Submit();
                    return true;
                }
                catch (XdbExecutionException ex)
                {
                    Log.Error($"XConnectContactRepository: Error saving contact data to xConnect. Facet: '{Facet}' Contact: '{contact.Id}'", ex, this);
                    return false;
                }
                catch (Exception ex)
                {
                    Log.Error("XConnectContactRepository: Error communication with the xConnect colllection service", ex, this);
                    return false;
                }
            }
        }

        public bool ReloadCurrentContact()
        {
            try
            {
                var manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;

                if (manager == null)
                {
                    Log.Error("XConnectContactRepository:ReloadCurrentContact(): Unable to instantiate ContactManager", this);
                    return false;
                }

                manager.RemoveFromSession(Tracker.Current.Contact.ContactId);
                Tracker.Current.Session.Contact = manager.LoadContact(Tracker.Current.Contact.ContactId);
            }
            catch (XdbExecutionException ex)
            {
                Log.Error($"XConnectContactRepository: Error reloading current contact with ID '{Tracker.Current.Contact.ContactId}'", ex, this);
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("XConnectContactRepository: Error communication with the xConnect colllection service", ex, this);
                return false;
            }

            return true;
        }

        public Sitecore.XConnect.Contact GetCurrentContact(string facetKey)
        {
            var manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;

            if (manager == null)
            {
                Log.Error("XConnectContactRepository: Unable to instantiate ContactManager", this);
                return null;
            }

            if (!IsContactIdentified(Tracker.Current.Contact))
            {
                // Save contact first
                Tracker.Current.Contact.ContactSaveMode = ContactSaveMode.AlwaysSave;
                manager.SaveContactToCollectionDb(Tracker.Current.Contact);
                this.ReloadCurrentContact();

                return GetContact(Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource, Tracker.Current.Contact.ContactId.ToString("N"), facetKey);
            }
         
            var id = Tracker.Current.Contact?.Identifiers.FirstOrDefault();
            return id != null ? GetContact(id.Source, id.Identifier, facetKey) : null;
        }

        private bool IsContactIdentified(Sitecore.Analytics.Tracking.Contact trackingContact)
        {
            return !trackingContact.IsNew && trackingContact.Identifiers.Any();
        }

        public T GetFacet<T>(Sitecore.XConnect.Contact contact, string FacetKey) where T : Facet
        {
            using (XConnectClient client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    return contact.GetFacet<T>(FacetKey);
                }
                catch (XdbExecutionException ex)
                {
                    Log.Error($"XConnectContactRepository: Error reading facet '{FacetKey}' from contact '{contact.Id}'", ex, this);
                    return null;
                }
                catch (Exception ex)
                {
                    Log.Error("XConnectContactRepository: Error communication with the xConnect colllection service", ex, this);
                    return null;
                }
            }
        }
    }
}