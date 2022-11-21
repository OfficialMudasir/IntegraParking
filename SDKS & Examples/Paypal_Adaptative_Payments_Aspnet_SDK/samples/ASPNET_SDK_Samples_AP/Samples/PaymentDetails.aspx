<%@ Page language="c#" Codebehind="PaymentDetails.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.PaymentDetails" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
	<title>PayPal SDK - Adaptive Payments - Payment Details</title>
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
			<center><h3>Adaptive Payments - Get Payment Details Response</h3></center><br/>
			<form id="Form1" method="post" runat="server">	
				<table width="400" align="center">
					<tr>
						<td>Transaction Status:</td>
						<td><%= this.PaymentDetailsResponse.status %></td>
					</tr>
					<tr>
						<td>Transaction Id:</td>
						<td><%= PaymentDetailsResponse.paymentInfoList[0].transactionId %></td>
					</tr>
					<tr>
						<td>Pay Key:</td>
						<td><%= this.PaymentDetailsResponse.payKey %></td>
					</tr>
					<tr>
						<td>Sender Email:</td>
						<td><%= this.PaymentDetailsResponse.senderEmail %></td>
					</tr>
					<tr>
						<td>Action Type:</td>
						<td><%= this.PaymentDetailsResponse.actionType %></td>
					</tr>
					<tr>
						<td>Fees Payer:</td>
						<td><%= this.PaymentDetailsResponse.feesPayer %></td>
					</tr>
                     <tr>
						<td>Currency :</td>
						<td><%= this.PaymentDetailsResponse.currencyCode %></td>
					</tr>
					<tr>
						<td>Preapproval Key:</td>
						<td><%= GetPreapprovalKey() %></td>
					</tr>
				</table>
			</form>
		</div>
	</div>
</body>
</html>
