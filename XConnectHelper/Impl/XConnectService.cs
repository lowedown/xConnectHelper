using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.XConnect.Facets;
using Sitecore.Configuration;
using Sitecore.SharedSource.XConnectHelper.ContactRepository;
using Sitecore.SharedSource.XConnectHelper.Helper;
using Sitecore.SharedSource.XConnectHelper.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Sitecore.SharedSource.XConnectHelper.Impl
{
    internal class XConnectService : IXConnectService
    {
        public ContactData Contact
        {
            get
            {
                var contact = new ContactData()
                {
                    TrackerContactId = Tracker.Current.Contact.ContactId.ToString(),
                    Identifiers = Tracker.Current.Contact.Identifiers.Select(i => $"{i.Identifier} ({i.Source})"),
                };

                var xConnectFacets = Tracker.Current.Contact.GetFacet<IXConnectFacets>("XConnectFacets");
                if (xConnectFacets.Facets != null && xConnectFacets.Facets.ContainsKey(PersonalInformation.DefaultFacetKey))
                {
                    var personalInfoXConnect = xConnectFacets.Facets[PersonalInformation.DefaultFacetKey] as PersonalInformation;
                    contact.Firstname = personalInfoXConnect.FirstName;
                    contact.Lastname = personalInfoXConnect.LastName;
                }

                // The emails facet is not loaded into session by default. Therefore we load it from xConnect
                var repository = new XConnectContactRepository();
                var emails = repository.GetFacet<EmailAddressList>(repository.GetCurrentContact(EmailAddressList.DefaultFacetKey), EmailAddressList.DefaultFacetKey);
                
                if (emails != null)
                {
                    contact.Emails = emails.Others.Select((k,v) => $"{k} ({v})");
                    contact.PreferredEmail = emails.PreferredEmail.SmtpAddress;                    
                }

                contact.ContactId = "New. (Not persisted to XDB yet)";
                if (!Tracker.Current.Contact.IsNew)
                {
                    var xConnectContact = repository.GetCurrentContact(EmailAddressList.DefaultFacetKey);
                    contact.ContactId = xConnectContact?.Id.ToString();
                }                

                return contact;
            }
        }

        public SessionData SessionData
        {
            get
            {
                if (!IsTrackerActive)
                {
                    return new SessionData();
                }

                var profileData = new List<string>();
                foreach (var profileName in Tracker.Current.Interaction.Profiles.GetProfileNames())
                {
                    var profile = Tracker.Current.Interaction.Profiles[profileName];
                    profileData.Add($"{profile.ProfileName} Count: {profile.Count} Pattern: {profile.PatternId} {profile.PatternLabel}");
                }

                return new SessionData()
                {
                    Channel = Tracker.Current.Session.Interaction.ChannelId.ToString(),
                    EngagementValue = Tracker.Current.Session.Interaction.Value.ToString(),
                    GeoCity = Tracker.Current.Interaction.GeoData?.City,
                    GeoCountry = Tracker.Current.Interaction.GeoData?.Country,
                    ProfileData = profileData
                };             
            }
        }

        public bool IsTrackerActive
        {
            get
            {
                return Tracker.IsActive;
            }
        }

        public void FlushSession()
        {
            HttpContext.Current.Session.Abandon();
        }

        public void DontTrackPageView()
        {
            if (IsTrackerActive)
            {
                Tracker.Current?.Interaction?.CurrentPage?.Cancel();
                Tracker.Current?.Session?.Interaction?.AcceptModifications();
            }
        }        

        public void SetContactData(string firstName, string lastName, string email)
        {
            IContactRepository repository = new XConnectContactRepository();

            var contact = repository.GetCurrentContact(PersonalInformation.DefaultFacetKey);
            var personalInfo = contact.Personal();
            if (personalInfo == null)
            {
                personalInfo = new PersonalInformation();
            }

            personalInfo.FirstName = firstName;
            personalInfo.LastName = lastName;

            repository.SaveFacet(contact, PersonalInformation.DefaultFacetKey, personalInfo);
            repository.ReloadCurrentContact();

            contact = repository.GetCurrentContact(EmailAddressList.DefaultFacetKey);
            var emails = contact.Emails();
            if (emails == null)
            {
                emails = new EmailAddressList(new EmailAddress(email, true), "default");
            }
            emails.PreferredEmail = new EmailAddress(email, true);
            repository.SaveFacet(contact, EmailAddressList.DefaultFacetKey, emails);
            repository.ReloadCurrentContact();
        }

        public void SetIdentifier(string id, string source)
        {
            Tracker.Current.Session.IdentifyAs(source, id);
        }

        public IEnumerable<string> ValidateConfig()
        {
            var messages = new List<string>();

            if (!Settings.GetBoolSetting("Xdb.Enabled", false))
            {
                messages.Add("Setting 'Xdb.Enabled' is false or not set");
            }

            if (!Settings.GetBoolSetting("Xdb.Tracking.Enabled", false))
            {
                messages.Add("Setting 'Xdb.Tracking.Enabled' is false or not set");
            }

            return messages;
        }
    }
}
