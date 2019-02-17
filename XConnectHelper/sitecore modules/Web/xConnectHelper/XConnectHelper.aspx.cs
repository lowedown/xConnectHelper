using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.XConnectHelper.Helper;
using Sitecore.SharedSource.XConnectHelper.Model;
using System;
using System.Collections.Generic;
using System.Web;
using Sitecore.SharedSource.XConnectHelper.Impl;

namespace Sitecore.SharedSource.XConnectHelper.sitecore_modules.Web.xConnect
{
    public partial class XConnectHelper : System.Web.UI.Page
    {
        private IXConnectService _helper;
        protected ContactData Contact;
        protected List<string> Messages = new List<string>();
        protected ServiceStatus Status;
        protected SessionData SessionData;

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Settings.GetSetting("xConnectHelper.AccessKey")))
            {
                var message = "xConnectHelper: No 'xConnectHelper.AccessKey' setting has been set in config.";
                Log.Error(message, this);
                throw new UnauthorizedAccessException(message);
            }

            if (Settings.GetSetting("xConnectHelper.AccessKey") != HttpContext.Current.Request.QueryString.Get("key"))
            {
                var message = "xConnectHelper: The provided key doesn't match the 'xConnectHelper.AccessKey' setting.";
                Log.Error(message, this);
                throw new UnauthorizedAccessException(message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPage();      
        }

        protected void InitPage()
        {
            _helper = new XConnectService();
            _helper.DontTrackPageView();

            Status = _helper.GetStatus();
            Contact = new ContactData();
            SessionData = _helper.SessionData;


            if (!_helper.IsTrackerActive)
            {
                Messages.Add("Tracker is not active! Check configuration and license.");                
            }

            if (!Status.CollectionAvailable)
            {
                Messages.Add("Collection service is not available. Check connection string and certificate thumbprint.");
            }

            // Only read contact from xConnect when it is available
            if (Status.CollectionAvailable && _helper.IsTrackerActive)            
            {
                Contact = _helper.Contact;
            }

            if (!IsPostBack)
            {
                Firstname.Text = Contact.Firstname;
                Lastname.Text = Contact.Lastname;
                EmailAddress.Text = Contact.PreferredEmail;
            }

            Messages.AddRange(_helper.ValidateConfig());
        }

        protected void FlushSession_Click(object sender, EventArgs e)
        {
            _helper.FlushSession();
            InitPage();
        }

        protected void IdentifyContact_Click(object sender, EventArgs e)
        {
            _helper.SetIdentifier(Identifier.Text, IdentifierSource.Text);
            InitPage();
        }

        protected void SetContactData_Click(object sender, EventArgs e)
        {
            _helper.SetContactData(Firstname.Text, Lastname.Text, EmailAddress.Text);
            InitPage();
        }
    }
}