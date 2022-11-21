<%@ Page language="c#" Codebehind="APIError.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.APIError" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Payments - API Error</title>
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

		<div id="response_form">

			<div id="divAPIError" runat="server">			
				<center><h3>The PayPal API has returned an error</h3></center><br/>						
				<table width="450" align="center">
					<tr>
						<td>Ack:</td>
						<td><%= this.TransactionEx.Ack %></td>
					</tr>
					<tr>
						<td>Correlation ID:</td>
						<td><%= this.TransactionEx.CorrelationID %></td>
					</tr>
					<tr>
						<td>Version:</td>
						<td><%= this.TransactionEx.Build %></td>
					</tr>
					<tr>
						<td colspan="2">   
							<br/><br/>
							<%= GetErrorDetails(this.TransactionEx.ErrorDetails) %>
						</td>
					</tr>
				</table>
			</div>
			<div id="divFATALError" runat="server">
				<center><h3>PayPal Fatal Error</h3></center><br/>
				<table width="450" align="center">
					<tr>
						<td>Error Message:</td>
						<td><%= FATALEx.FATALExceptionMessage %></td>
					</tr>
					<tr>
						<td>Long Error Message</td>
						<td><%= FATALEx.FATALExceptionLongMessage %></td>
					</tr>
				</table>
			</div>
		</div>
	</div>
</body>
</HTML>
