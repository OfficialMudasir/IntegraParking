<%@ Page language="c#" Codebehind="SetPaymentOptionsReceipt.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.SetPaymentOptionsReceipt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
	<title>PayPal SDK - Adaptive Payments - Set Payment Options</title>
	<link href="style.css" rel="stylesheet" type="text/css" />
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
			<center><h3>Adaptive Payments - Pay (Set Payment Options Response)</h3></center><br/>
			<form id="Form1" method="post" runat="server">	
				<table width="400" align="center">

					<tr>
						<td>Ack:</td>
						<td><%= PaymentResponse.responseEnvelope.ack %></td>
					</tr>
					<tr>
						<td>Correlation Id:</td>
						<td><%= PaymentResponse.responseEnvelope.correlationId %></td>
					</tr>
					<tr>
						<td>Timestamp:</td>
						<td><%= PaymentResponse.responseEnvelope.timestamp %></td>
					</tr>
					<tr>
						<td>Build:</td>
						<td><%= PaymentResponse.responseEnvelope.build%></td>
					</tr>
					<tr>
						<td colspan="2">
							<br/><font size="2" color="black" face="Verdana"><a id="ExecutePayment" href="ExecutePayment.aspx" name="ExecutePaymentLink">* Execute Payment</a></font><br />
						</td>
					</tr>					
				</table>
			</form>
		</div>
	</div>
</body>
</html>
