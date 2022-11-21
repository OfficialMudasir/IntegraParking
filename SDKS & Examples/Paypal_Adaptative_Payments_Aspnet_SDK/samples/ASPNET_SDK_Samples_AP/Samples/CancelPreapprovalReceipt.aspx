<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelPreapprovalReceipt.aspx.cs" Inherits="ASPNET_SDK_Samples.Samples.CancelPreapprovalReceipt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
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

		<div id="response_form">
			<center>
				<h3>Adaptive Payments - Cancel Preapproval</h3><br/>
				<h4>Preapproval Cancelled</h4>
			</center>
			<table width=300 align="center">
				<tr>
					<td>Whole Transaction Status:</td>
					<td><%= this.CancelPreapprovalResponse.responseEnvelope.ack.ToString() %></td>
				</tr>
				<tr>
					<td>Correlation Id:</td>
					<td><%= this.CancelPreapprovalResponse.responseEnvelope.correlationId %></td>
				</tr>
				
			</table>
		</div>
	</div>
</body>
</html>
