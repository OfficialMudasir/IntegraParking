<%@ Page language="c#" Codebehind="SetFundingSourceConfirmed.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.SetFundingSourceConfirmed" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>PayPal SDK - Adaptive Accounts - Confirm Funding Source</title>
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
			<center><h3>Adaptive Accounts - Confirm Funding Source</h3></center><br/>
			<form id="Form1" method="post" runat="server">	
				<table align="center">
					  <tr>
        <td class="label">Email Id</td>
        <td><input type="text" maxlength="50" name="emailid" id="emailid" runat="server" value="platfo@paypal.com" /></td>
    </tr>
     <tr>
        <td class="label">Funding Source Key</td>

        <td><input type="text" maxlength="50" name="fundingSourceKey"
            value="" id="fundingSourceKey" runat="server" /></td>
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
