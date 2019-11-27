<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XConnectHelper.aspx.cs" Inherits="Sitecore.SharedSource.XConnectHelper.sitecore_modules.Web.xConnect.XConnectHelper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>xConnect Helper</title>
	<link rel="Stylesheet" type="text/css" href="bootstrap.min.css">
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
			
			<div class="card-deck">
				
				<div class="mb-3 card box-shadow"> 
				
					<div class="card-header">					
						<h4>Personal Information</h4>
					</div>
					<div class="card-body">					
						<table class="table table-striped table-sm">
							<tr><th>Contact ID (Tracker)</th><td><%= Contact.TrackerContactId %></td></tr>
							<tr><th>Contact ID (XDB)</th><td><%= Contact.ContactId %></td></tr>
							<tr><th>Firstname</th><td><%= Contact.Firstname %></td></tr>
							<tr><th>Lastname</th><td><%= Contact.Lastname %></td></tr>							
							<tr>
								<th>Emails</th>
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
						
						<h5>Identifiers</h5>
						<table class="table table-striped table-sm">
							<% foreach(var id in Contact.Identifiers) { %>
								<tr><th><%= id.Key %></th><td><%= id.Value %></td></tr>
							<% } %>
						</table>
					</div>
				</div>
				
				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Engagement</h4>
					</div>
					<div class="card-body">
				
						<table class="table table-striped table-sm">
							<tr>
								<th>Engagement Value:</th>
								<td>♥<%= SessionData.EngagementValue %></td>
							</tr>
							<tr>
								<th>Page views in this visit:</th>
								<td><%= SessionData.PagesInCurrentVisit %></td>
							</tr>
							<tr>
								<th>Visits to the site:</th>
								<td><%= SessionData.Visits %></td>
							</tr>
						</table>
					</div>
				
				</div>
				
				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Device</h4>
					</div>
					<div class="card-body">					
						<table class="table table-striped table-sm">
							<tr><th>Geo City & Country:</th><td><%= SessionData.GeoCity %> / <%= SessionData.GeoCountry %></td></tr>
							<tr><th>Device</th><td><%= SessionData.Device %></td></tr>
							<tr><th>Browser</th><td><%= SessionData.Browser %></td></tr>
							<tr><th>Robot Classification</th><td><%= SessionData.RobotDetection %></td></tr>
						</table>
					</div>
				</div>
				
				
			</div>
			
			<div class="card-deck">
				
				
				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Referral</h4>
					</div>
					<div class="card-body">					
						<table  class="table table-striped table-sm">
							<tr><td><label>Channel:</label></td><td><%= SessionData.Channel %></td></tr>
							<tr><td><label>Campaign:</label></td><td><%= SessionData.CampaignId %></td></tr>
							<tr><td><label>Referrer:</label></td><td><%= SessionData.Referrer %></td></tr>
						</table>
					</div>
				</div>
		
				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Events</h4>
					</div>
					<div class="card-body">	
					
					
						<strong>Goals (<%= SessionData.GoalsCount %>)</strong>
						
						<% if(SessionData.Goals != null) { %>
						<table class="table table-striped table-sm">		
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
						
						<hr/>
						<strong>Page Events (<%= SessionData.PageEventsCount %>)</strong>
						
						<% if(SessionData.PageEvents != null) { %>
						<table class="table table-striped table-sm">		
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
					
				</div>
				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Profiling</h4>
					</div>
					<div class="card-body">	
							
						<% if (SessionData.ProfileData != null && SessionData.ProfileData.Any()) { %>
                            
                            
                            <% foreach (var profile in SessionData.ProfileData)
                               { %>
                                
                                <h5><%= profile.Name %></h5>
                                
                                Pattern Match: <strong><%= profile.Pattern == "" ? "no match" : profile.Pattern %></strong>
                            
                                <table class="table table-striped table-sm">
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
				
				
			</div>
			<div class="card-deck">
				<div class="mb-3 card box-shadow"> 
				
					<div class="card-header">					
						<h4>Service Status</h4>
					</div>
					<div class="card-body">	
				
					
					<p>Check the status of the connected xConnect service (Certificates, Connection Strings etc.)</p>
					
					<asp:Button runat="server"  CssClass="btn" ID="CheckStatus" OnClick="CheckStatus_OnClick" Text="Check status"/>
                        
                    <br/>

					<table class="table table-striped table-sm">
					<% foreach (var service in Status)
						{ %>
						<tr>
							<th><%= service.ServiceName %></th>
							<td><%= !service.Error ? "OK" : "ERROR" %></td>
							<td>
								<ul>
								<% foreach (var msg in service.Messages)
								{ %>
									<li><%= msg %></li>
								<% } %>
								</ul>
							</td>
						</tr>					
					<% } %>  
					</table>
					
					</div>
				</div>
				
				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Contact identifiers</h4>
					</div>
					<div class="card-body">	
					
					<table class="table table-sm">				
						<tr>
							<td><label for="Identifier">Identifier:</label></td>
							<td><asp:Textbox CssClass="form-control" runat="server" ID="Identifier" /></td>
						</tr>
						<tr>
							<td><label for="IdentifierSource">Identifier Source:</label></td>
							<td><asp:Textbox CssClass="form-control" runat="server" ID="IdentifierSource" Text="xConnectHelper" /></td>
						</tr>
					</table>
					
					<asp:Button runat="server"  CssClass="btn" OnClick="IdentifyContact_Click" Text="Identify current contact" />  
					
					</div>
				</div>
			</div>
			
			<div class="card-deck">		

				<div class="mb-3 card box-shadow"> 
					
					<div class="card-header">					
						<h4>Contact Data</h4>
					</div>
					<div class="card-body">
					
					<table class="table table-sm">				
						<tr>
							<td><label for="Firstname">Firstname</label></td>
							<td><asp:Textbox runat="server" CssClass="form-control" ID="Firstname" /></td>
						</tr>
						<tr>
							<td><label for="Lastname">Lastname</label></td>
							<td><asp:Textbox runat="server" CssClass="form-control" ID="Lastname" /></td>
						</tr>
						<tr>
							<td><label for="EmailAddress">Email</label></td>
							<td><asp:Textbox runat="server" CssClass="form-control" ID="EmailAddress" /></td>
						</tr>
					</table>
					<asp:Button runat="server" CssClass="btn" OnClick="SetContactData_Click" Text="Set contact data" /> 
					
					</div>
				</div>
				<div class="mb-3 card box-shadow"> 
				
					<div class="card-header">					
						<h4>Flush Session</h4>
					</div>
					<div class="card-body">	
				
					<p>This will trigger processing immediately.</p>
					<asp:Button runat="server" ID="FlushSession" CssClass="btn btn-primary" OnClick="FlushSession_Click" Text="Flush current session" />
					
					</div>
				</div>
			</div>   
            
        </div>
    </form>
</body>
</html>
