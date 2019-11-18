<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XConnectHelper.aspx.cs" Inherits="Sitecore.SharedSource.XConnectHelper.sitecore_modules.Web.xConnect.XConnectHelper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xConnect Helper</title>
    <link rel="Stylesheet" type="text/css" href="xConnectHelper.css">
</head>
<body>
    <form id="form1" runat="server">	
		
        <div class="wrapper">
			<h1>xConnect Helper</h1>
		
            <% if (Messages.Count > 0)
                { %>
            <div class="messages">
				<ul>
					<% foreach (var msg in Messages)
                                        { %>
						<li><%= msg %></li>
					<% } %>
				</ul>
            </div>
            <% } %>

            <div class="box contactinfo">             

				<h2>Contact Data</h2>
				<table>
					<tr><td>Contact ID (Tracker)</td><td><%= Contact.TrackerContactId %></td></tr>
                    <tr><td>Contact ID (XDB)</td><td><%= Contact.ContactId %></td></tr>
					<tr><td>Firstname</td><td><%= Contact.Firstname %></td></tr>
					<tr><td>Lastname</td><td><%= Contact.Lastname %></td></tr>
					<tr>
						<td>Identifiers</td>
						<td>
							<ul>
								<% foreach(var id in Contact.Identifiers) { %>
									<li><%= id %></li>
								<% } %>
							</ul>
						</td>
					</tr>
					<tr>
						<td>Emails</td>
						<td>
							<ul>
                                <li><%= Contact.PreferredEmail %> (Preferred)</li>
								<% foreach(var email in Contact.Emails) { %>
							        <li><%= email %></li>
						        <% } %>
							</ul>
						</td>
					</tr>
				</table>
            </div>

            <div class="box session-data">
				<h2>Session Data</h2>
				<table>				
					<tr>
						<td><label>Geo City & Country:</label></td>
						<td><%= SessionData.GeoCity %> / <%= SessionData.GeoCountry %></td>
					</tr>
					<tr>
						<td><label>Channel:</label></td>
						<td><%= SessionData.Channel %></td>
					</tr>
                    <tr>
						<td><label>Engagement Value:</label></td>
						<td><%= SessionData.EngagementValue %></td>
					</tr>
                    <tr>
                        <td><label>Robot Detection:</label></td>
                        <td><%= SessionData.RobotDetection %></td>
                    </tr>
                    <tr>
                        <td><label>Campaign ID:</label></td>
                        <td><%= SessionData.CampaignId %></td>
                    </tr>
                    <% if (SessionData.ProfileData != null && SessionData.ProfileData.Any()) { %>
                    <tr>
                        <td>Profiles:</td>
						<td>
                            <ul>
                            <% foreach (var profile in SessionData.ProfileData)
                                { %>
                                <li><%= profile %></li>
                            <%} %>
                            </ul>
						</td>
                    </tr>
                    <% } %>
				</table>         
			</div>

            <div class="box">
                <h2>Service Status</h2>

                <% foreach (var service in Status)
                    { %>
                    <h3><%= service.ServiceName %><%= !service.Error ? ": OK" : ": ERROR" %></h3>
					<ul>
					<% foreach (var msg in service.Messages)
                    { %>
						<li><%= msg %></li>
					<% } %>
				</ul>
				<% } %>                
            </div>
			
			<div class="box contact-identify">
				<h2>Contact identifiers</h2>
				<table>				
					<tr>
						<td><label for="Identifier">Identifier:</label></td>
						<td><asp:Textbox runat="server" ID="Identifier" /></td>
					</tr>
					<tr>
						<td><label for="IdentifierSource">Identifier Source:</label></td>
						<td><asp:Textbox runat="server" ID="IdentifierSource" Text="xConnectHelper" /></td>
					</tr>
				</table>
				
	            <asp:Button runat="server" OnClick="IdentifyContact_Click" Text="Identify current contact" />          
			</div>

            <div class="box contact-data">
				<h2>Contact Data</h2>
				<table>				
					<tr>
						<td><label for="Firstname">Firstname</label></td>
						<td><asp:Textbox runat="server" ID="Firstname" /></td>
					</tr>
					<tr>
						<td><label for="Lastname">Lastname</label></td>
						<td><asp:Textbox runat="server" ID="Lastname" /></td>
					</tr>
					<tr>
						<td><label for="EmailAddress">Email</label></td>
						<td><asp:Textbox runat="server" ID="EmailAddress" /></td>
					</tr>
				</table>
				<asp:Button runat="server" OnClick="SetContactData_Click" Text="Set contact data" /> 
			</div>
            
            <div class="box current-session">
				<h2>Flush Session</h2>
                <p>This will trigger processing immediately.</p>
				<asp:Button runat="server" ID="FlushSession" OnClick="FlushSession_Click" Text="Flush current session" />
			</div>     
            
        </div>
    </form>
</body>
</html>
