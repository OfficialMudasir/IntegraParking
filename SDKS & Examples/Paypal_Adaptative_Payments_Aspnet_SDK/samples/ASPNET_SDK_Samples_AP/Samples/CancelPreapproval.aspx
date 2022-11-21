<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelPreapproval.aspx.cs" Inherits="ASPNET_SDK_Samples.Samples.CancelPreapproval" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>PayPal SDK - Adaptive Payments - Cancel Preapproval</title>
		<link href="style.css" rel="stylesheet" type="text/css">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>
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
			<center><h3>Adaptive Payments - Cancel Preapproval</h3></center><br/>
		    <form id="form1" runat="server">
				<table align="center">
				 <tr></tr>
					<tr>
						<td class="label">
							Preapproval Key **
						</td>
						<td align="left">
							<input type="text" name="preapprovalkey" id="preapprovalkey" runat="server"/>
						</td>
					</tr>
					<tr>
						<td colspan="2" align="right" class="note">** <i>Required field</i></td>
					</tr>
					<tr align="center">                    
						<td colspan="2" class="submit" align="center">
							<span class="pop-button primary" >
								<span><input type="submit" value="Submit" id="Submit" name="Submit" runat="server" class="button"/></span>
							</span>
						</td>
					</tr>
				</table>
			</form>
		</div>
	</div>
</body>
</html>
