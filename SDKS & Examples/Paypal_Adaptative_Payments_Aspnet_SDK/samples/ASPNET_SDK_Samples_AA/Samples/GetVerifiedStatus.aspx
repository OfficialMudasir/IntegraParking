<%@ Page language="c#" Codebehind="GetVerifiedStatus.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.GetVerifiedStatus" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Accounts - Get Verified Status</title>
	<link href="style.css" rel="stylesheet" type="text/css">
	<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" Content="C#">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</HEAD>
<body>
	<div id="wrapper">
		<div id="header">
            <div id="logo">
			    <span id="loginbox">Login to <a href="https://developer.paypal.com" target="_blank">PayPal sandbox</a></span>
			    <a title="Paypal X Home" href="#" id="web_pageheader_anchor_index"><img alt="Paypal X" src="paypal_x_logo.gif"/></a>
		    </div>
        </div>

		<% Server.Execute("ApiList.aspx"); %>  

		<div id="request_form">
			<center><h3>Adaptive Accounts - Get Verified Status</h3></center><br/>
			<form id="Form1" method="post" runat="server">	
				<table align="center">
					     <tr>
        <td class="label">Email ID</td>
        <td><input type="text" maxlength="50" name="emailid"
            value="platfo11_per@gmail.com" id="emailid" runat="server" /></td>
    </tr>
   
    <tr>
        <td class="label">First Name</td>

        <td><input type="text" maxlength="50"
            name="firstName" value="Bonzop" id="firstName" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">Last Name</td>
        <td><input type="text" maxlength="50" name="lastname"
            value="Zaius" id="lastname" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">Match Criteria</td>

        <td><input type="text" maxlength="50" name="matchcriteria"
            value="NAME" id="matchcriteria" runat="server" /></td>
     </tr>

    <tr>
        <td class="label" width="52"></td>
        <td colspan="5"></td>
    </tr>

  					<tr align="center">                    
						<td colspan="2" class="submit" align="center">
							<span class="pop-button primary" >
								<span><input type="submit" value="Submit" id="Submit" runat="server" class="button"></span>
							</span>
						</td>
					</tr>
				</table>		
			</form>
		</div>
	</div>	
</body>
</HTML>
