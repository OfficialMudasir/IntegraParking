<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetPaymentOptionReceipt.aspx.cs" Inherits="ASPNET_SDK_Samples.Samples.GetPaymentOptionReceipt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Payments - Get Payment Options</title>
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
			<center><h3>Adaptive Payments - Get Payment Options Response</h3></center><br/>
			<form id="Form1" method="post" runat="server">	
				<table width="400" align="center">
					 <tr>
		                 <td> Email Header Image URL:</td>
		                 <td><%=GetPaymentResponse.displayOptions != null ? GetPaymentResponse.displayOptions.emailHeaderImageUrl : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> Email Marketing Image URL:</td>
		                 <td><%=GetPaymentResponse.displayOptions != null ? GetPaymentResponse.displayOptions.emailHeaderImageUrl : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> InstitutionId :</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.institutionId : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> Institution CustomerId:</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.institutionCustomerId : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> Email :</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.email : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> First Name:</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.firstName : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> Last Name:</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.lastName : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> Country Code:</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.countryCode : ""%></td>
	                 </tr>
	                 <tr>
		                 <td> Display Name:</td>
		                 <td><%=GetPaymentResponse.initiatingEntity != null ? GetPaymentResponse.initiatingEntity.institutionCustomer.displayName : ""%></td>
	                 </tr>						
				</table>
			</form>
		</div>
	</div>
</body>
</HTML>
