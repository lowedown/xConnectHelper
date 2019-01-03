# xConnectHelper
This helper page allows you to test if your xConnect functionality is working as expected.

## How does it help? 
After installing, you can access the page through 

    /sitecore%20modules/Web/xConnectHelper/xConnectHelper.aspx?key=<yourkey>

On this page you can...

- View status information on the current Tracker config and collection service connection
- View basic contact facets
- Set one or multiple identifiers for the current contact
- Set basic contact facets
- Flush current session which immediately triggers processing

Some possible use cases:
**I want to check the xConnect connection**:
If there is a connection problem, this will be immediately shown on the *xConnectHelper* page. Also, if the tracker is not active becaues of config or license issues.
**I want to check if data is written to the collection and reporting db**:
Interact with your website and then hit **flush current session**. Processing of your contact and interactions will immediately start and data should be visible in the databases within seconds.
**I want to test contact search / experience profile**:
The experience profile manager only lists identified contacts. You can set an identifier for your contact, then flush session. Your contact should show in the list within a few seconds.

## How to install
1. Download Release-Zip and integrate into your build
2. Set a hard-to guess key in App_Config/Modules/xConnectHelper/xConnectHelper.config
3. Access your page through */sitecore%20modules/Web/xConnectHelper/xConnectHelper.aspx?key=<yourkey>*

**Important Note**: Because this tool is made to debug current contacts and interactions in-session, it can not be used as a regular Sitecore admin page and is therefore publicly accessible (if your access key is known). You are responsible for setting appropriate measures to regulate access to this page.

## Supported Sitecore versions
This release was tested with Sitecore *9.0 update-2*. It will not support any lower releases because of API changes. It should be easy to adopt to 9.1.