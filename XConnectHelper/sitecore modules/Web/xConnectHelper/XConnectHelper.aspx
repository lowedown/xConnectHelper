<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XConnectHelper.aspx.cs" Inherits="Sitecore.SharedSource.XConnectHelper.sitecore_modules.Web.xConnect.XConnectHelper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xConnect Helper</title>
	<link rel="Stylesheet" type="text/css" href="bootstrap-grid.min.css">
    <link rel="Stylesheet" type="text/css" href="xConnectHelper.css">
	
</head>
<body>
    <form id="form1" runat="server">	
		
        <div class="container">
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
			
			<div class="row">
				
				<div class="col"> 
					<h2>Personal Information</h2>
					
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
				
				<div class="col"> 
					<h2>Engagement</h2>
				
					<table>
						<tr>
							<td><label>Engagement Value:</label></td>
							<td>♥<%= SessionData.EngagementValue %></td>
						</tr>
						<tr>
							<td><label>Page views in this visit:</label></td>
							<td><%= SessionData.PagesInCurrentVisit %></td>
						</tr>
						<tr>
							<td><label>Visits to the site:</label></td>
							<td><%= SessionData.Visits %></td>
						</tr>
					</table>
				
				</div>
				
				
			</div>
			
			<div class="row">
				<div class="col"> 
					<h2>Device</h2>
					
					<table>
						<tr><td>Geo City & Country:</td><td><%= SessionData.GeoCity %> / <%= SessionData.GeoCountry %></td></tr>
						<tr><td>Device</td><td><%= SessionData.Device %></td></tr>
						<tr><td>Browser</td><td><%= SessionData.Browser %></td></tr>
						<tr><td>Robot Classification</td><td><%= SessionData.RobotDetection %></td></tr>
					</table>
				</div>
				
				<div class="col"> 
					<h2>Referral</h2>
					
					<table>
						<tr><td><label>Channel:</label></td><td><%= SessionData.Channel %></td></tr>
						<tr><td><label>Campaign:</label></td><td><%= SessionData.CampaignId %></td></tr>
						<tr><td><label>Referrer:</label></td><td><%= SessionData.Referrer %></td></tr>
					</table>
				</div>
			</div>
			
			<div class="row">
				<div class="col"> 
					<h2>Events</h2>
					
					
					<h3>Goals (<%= SessionData.GoalsCount %>)</h3>
					
					<% if(SessionData.Goals != null) { %>
					<table>		
						<% foreach (var pageEvent in SessionData.Goals)
							{ %>						
							<tr>
								<td><%= pageEvent.Timestamp.ToShortTimeString() %>/td>
								<td><%= pageEvent.Title %></td>
								<td>♥<%= pageEvent.EngagementValue %></td>
							</tr>
						<%} %>
					</table>
					<% } %>
					
					<h3>Page Events (<%= SessionData.PageEventsCount %>)</h3>
					
					<% if(SessionData.PageEvents != null) { %>
					<table>		
						<% foreach (var pageEvent in SessionData.PageEvents)
							{ %>						
							<tr>
								<td><%= pageEvent.Timestamp.ToShortDateString() %> <%= pageEvent.Timestamp.ToShortTimeString() %></td>
								<td><%= pageEvent.Title %></td>
								<td>♥<%= pageEvent.EngagementValue %></td>
							</tr>
						<%} %>
					</table>
					<% } %>
					
				</div>
				<div class="col"> 
					<h2>Profiling</h2>
							
						<% if (SessionData.ProfileData != null && SessionData.ProfileData.Any()) { %>
                            
                            
                            <% foreach (var profile in SessionData.ProfileData)
                               { %>
                                
                                <h3><%= profile.Name %></h3>
                                
                                Pattern Match: <%= profile.Pattern == "()" ? "no match" : profile.Pattern %>
                            
                                <table>
                                    <% foreach (var val in profile.Values)
                                       { %>
                                    
                                        <tr>
                                            <td><%= val.Key %></td>
                                            <td><%= val.Value %></td>
                                        </tr>

                                    <% } %>
                                </table>
                                
                            <%} %>
						<% } %>
					
				</div>
				
				
			</div>
			<div class="row">
				<div class="col"> 
					<h2>Service Status</h2>
					<p>Check the status of the connected xConnect service (Certificates, Connection Strings etc.)</p>
					
					<asp:Button runat="server" ID="CheckStatus" OnClick="CheckStatus_OnClick" Text="Check status"/>

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
				
				<div class="col"> 
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
			</div>
			
			<div class="row">
				
				

				<div class="col"> 
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
				<div class="col"> 
					<h2>Flush Session</h2>
					<p>This will trigger processing immediately.</p>
					<asp:Button runat="server" ID="FlushSession" OnClick="FlushSession_Click" Text="Flush current session" />
				</div>
			</div>   
            
        </div>
    </form>
</body>
</html>
