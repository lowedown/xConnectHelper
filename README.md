# xConnectHelper

![Screenshot of xConnectHelper](doc/logo.png?raw=true "xConnect Helper")

*Your Swiss Army Knife for debugging xConnect.*

## How does it help? 

This helper page provides the following tools:

- Insight into current tracking session data
- Show goals and events that were triggered on last page view
- Status checker that validates your xConnect setup (connections, certificates, settings)
- Set identifiers and basic contact data in the current session
- Flush current session for immediate processing

Screenshot 1: Tracking data of current session:

![Screenshot of xConnectHelper](doc/screenshot-1.PNG?raw=true "xConnect Helper")

Screenshot 2: Status checker result:

![Screenshot of xConnectHelper](doc/screenshot-2.PNG?raw=true "xConnect Helper")

## How to install
1. Download [Release ZIP](https://github.com/lowedown/xConnectHelper/releases/latest) and integrate into your build
2. Set a hard-to guess key in App_Config/Modules/xConnectHelper/xConnectHelper.config
3. Access your page through */sitecore%20modules/Web/xConnectHelper/xConnectHelper.aspx?key=<yourkey>*

**Important Note**: Because this tool is made to debug current contacts and interactions in-session, it needs to be accessible on *Content Delivery* instances. The helper page is protected by a key that you need to define during installation. Make sure this is sufficcient for your organisation and if not, you need to meet appropriate measures to prevent access to this page i.E. IP-blocking through web.config.

## How to use

Once installed, you can access the helper page through:


    /sitecore%20modules/Web/xConnectHelper/xConnectHelper.aspx?key=<yourkey>

### Some possible use cases:
##### I want to check the xConnect connection
If there is a connection problem, this will be immediately shown on the *xConnectHelper* page. Also, if the tracker is not active becaues of config or license issues.
xConnect helper validates:
- connection strings
- https access to Services
- Server certificate validity
- Client certificate validity and read rights to private key
- Warning if client certificate expires soon

##### I want to check if data is written to the collection and reporting db:
Interact with your website and then hit **flush current session**. Processing of your contact and interactions will immediately start and data should be visible in the databases within seconds.
##### I want to test contact search / experience profile:
The experience profile manager only lists identified contacts. You can set an identifier for your contact, then flush session. Your contact should show in the list within a few seconds.



## Supported Sitecore versions
This release was tested with Sitecore *9.0 update-2*, *9.1*, *9.2*,*9.3*-*10.2*. It will not support any lower releases because of API changes.


---
Made in Switzerland
