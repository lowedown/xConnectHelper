<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XConnectHelper.aspx.cs" Inherits="Sitecore.SharedSource.XConnectHelper.sitecore_modules.Web.xConnect.XConnectHelper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
		<h1>xConnect Helper</h1>
		
        <div>
            <div class="messages">
                <td>
					<ul>
						<% foreach(var msg in Messages) { %>
							<li><%= msg %></li>
						<% } %>
					</ul>
				</td>
            </div>
            
            <div class="contactinfo">
				<table>
                    <tr><td>Collection Status</td><td><%= Status.Collection %></td></tr>
					<tr><td>Contact ID</td><td><%= Contact.ContactId %></td></tr>
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
			
			<div class="contact-identify">
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

            <div class="contact-data">
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
            
            <div class="current-session">
				<h2>Flush Session</h2>
				<asp:Button runat="server" ID="FlushSession" OnClick="FlushSession_Click" Text="Flush current session" />
			</div>     
            
        </div>
    </form>
</body>
</html>
