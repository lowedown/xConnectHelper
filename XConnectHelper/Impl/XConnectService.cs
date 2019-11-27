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
using Sitecore.Analytics.Core;
using Sitecore.CES.DeviceDetection;

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
                    Identifiers = Tracker.Current.Contact.Identifiers.Select(i => new KeyValuePair<string, string>(i.Source, i.Identifier)),
                };

                var xConnectFacets = Tracker.Current.Contact.GetFacet<IXConnectFacets>("XConnectFacets");
                if (xConnectFacets.Facets != null && xConnectFacets.Facets.ContainsKey(PersonalInformation.DefaultFacetKey))
                {
                    var personalInfoXConnect = xConnectFacets.Facets[PersonalInformation.DefaultFacetKey] as PersonalInformation;
                    contact.Firstname = personalInfoXConnect.FirstName;
                    contact.Lastname = personalInfoXConnect.LastName;
                }

                contact.ContactId = "New. (Not persisted to XDB yet)";
                if (!Tracker.Current.Contact.IsNew)
                {
                    var repository = new XConnectContactRepository();
                    Contact xConnectContact = null;

                    try
                    {
                        xConnectContact = repository.GetCurrentContact(EmailAddressList.DefaultFacetKey);
                    }
                    catch (XdbCollectionUnavailableException ex)
                    {
                        
                    }

                    if (xConnectContact != null)
                    {
                        contact.ContactId = xConnectContact?.Id.ToString();

                        // The emails facet is not loaded into session by default. Therefore we load it from xConnect
                        var emails =
                            repository.GetFacet<EmailAddressList>(xConnectContact, EmailAddressList.DefaultFacetKey);

                        if (emails != null)
                        {
                            contact.Emails = emails.Others.Select((k, v) => $"{k} ({v})");
                            contact.PreferredEmail = emails.PreferredEmail.SmtpAddress;
                        }
                    }
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

                var profileData = new List<Profile>();
                foreach (var profileName in Tracker.Current.Interaction.Profiles.GetProfileNames())
                {
                    var profile = Tracker.Current.Interaction.Profiles[profileName];

                    profileData.Add(new Profile()
                    {
                        Name = profile.ProfileName,
                        Pattern = $"{profile.PatternLabel}",
                        Values = profile.Select(p => new KeyValuePair<string, double>(p.Key, p.Value))
                    });
                }

                var currentPage = Tracker.Current.Interaction.CurrentPage;

                var data = new SessionData()
                {
                    Channel = Tracker.Current.Session.Interaction.ChannelId.ToString(),
                    EngagementValue = Tracker.Current.Session.Interaction.Value.ToString(),
                    GeoCity = Tracker.Current.Interaction.GeoData?.City,
                    GeoCountry = Tracker.Current.Interaction.GeoData?.Country,
                    ProfileData = profileData,
                    RobotDetection =  GetBotTypeString(),
                    CampaignId = Tracker.Current.Interaction.CampaignId.ToString(),
                    Visits = Tracker.Current.Interaction.ContactVisitIndex,
                    PagesInCurrentVisit = Tracker.Current.Interaction.PageCount,
                    Referrer = Tracker.Current.Interaction.Referrer,
                    GoalsCount = currentPage?.PageEvents.Count(e => e.IsGoal) ?? 0,
                    PageEventsCount = currentPage?.PageEvents.Count(e => !e.IsGoal) ?? 0,
                    Goals = currentPage?.PageEvents.Where(e => e.IsGoal).Take(10).Select(e => new EventEntry() { EngagementValue = e.Value, Timestamp = e.DateTime, Title = e.Name}),
                    PageEvents = currentPage?.PageEvents.Where(e => !e.IsGoal).Take(10).Select(e => new EventEntry() { EngagementValue = e.Value, Timestamp = e.DateTime, Title = e.Name })
                };
                
                if (DeviceDetectionManager.IsEnabled && DeviceDetectionManager.IsReady && !string.IsNullOrEmpty(Tracker.Current.Interaction.UserAgent))
                {
                    var deviceData = DeviceDetectionManager.GetDeviceInformation(Tracker.Current.Interaction.UserAgent);

                    data.Device = deviceData.DeviceModelName;
                    data.Browser = deviceData.Browser;
                };

                return data;
            }
        }

        private string GetBotTypeString()
        {
            var classification = Tracker.Current.Contact.System.Classification;
            if (ContactClassification.IsMaliciousRobot(classification))
            {
                return $"Malicious robot ({classification})";
            }

            if (ContactClassification.IsAutoDetectedRobot(classification))
            {
                return $"Auto detected robot ({classification})";
            }

            if (ContactClassification.IsRobot(classification))
            {
                return $"Robot ({classification})";
            }

            if (ContactClassification.IsHuman(classification))
            {
                return $"Human ({classification})";
            }

            return string.Empty;
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
