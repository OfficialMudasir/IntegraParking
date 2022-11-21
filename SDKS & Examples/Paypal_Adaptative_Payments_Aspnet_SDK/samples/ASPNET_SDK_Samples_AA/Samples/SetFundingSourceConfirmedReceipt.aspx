<%@ Page language="c#" Codebehind="SetFundingSourceConfirmedReceipt.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.SetFundingSourceConfirmedReceipt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<title>PayPal SDK - Adaptive Accounts - Confirm Funding Source</title>
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

		<div id="response_form">
			<center><h3>Adaptive Accounts - Confirm Funding Source Response</h3></center><br/>

			<form id="Form1" method="post" runat="server">

				<table width="400" align="center">
					<tr>
						<td>Status:</td>
						<td><%= this.SetFundingSourceConfirmedResponse.responseEnvelope.ack%></td>
					</tr>
					<tr>
						<td>Timestamp:</td>
						<td><%= this.SetFundingSourceConfirmedResponse.responseEnvelope.timestamp%></td>
					</tr>
					<tr>
						<td>Correlation Id:</td>
						<td><%= this.SetFundingSourceConfirmedResponse.responseEnvelope.correlationId%></td>
					</tr>
					<tr>
						<td>Build:</td>
						<td><%= this.SetFundingSourceConfirmedResponse.responseEnvelope.build%></td>
					</tr>															
				</table>
			</form>
		</div>
	</div>
</body>
</html>
