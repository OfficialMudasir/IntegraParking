<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPaymentOptions.aspx.cs" Inherits="ASPNET_SDK_Samples.Samples.SetPaymentOptions" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Payments - Set Payment Options</title>
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
			<center><h3>Adaptive Payments - Pay (Set Payment Options)</h3></center><br/>
			<form id="Form1" method="post" runat="server">
				<table align="center">											
					<tr>
						<td class="label">Pay Key</td>
						<td align="left"><input id="paykey" type="text" maxLength="32" value=""
								name="paykey" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label0 note" colspan="3">
							<b>Financial Partner Details (Optional)</b>
						</td>
					</tr>
					<tr>
						<td class="label">Country Code</td>
						<td align="left"><input id="countrycode" type="text" maxLength="32" value=""
								name="countrycode" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">Name </td>
						<td align="left"><input id="name" type="text" maxLength="32" value=""
								name="name" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">Email </td>
						<td align="left"><input id="email" type="text" maxLength="32" value=""
								name="email" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">First Name</td>
						<td align="left"><input id="firstname" type="text" maxLength="32" value=""
								name="firstname" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">Last Name</td>
						<td align="left"><input id="lastname" type="text" maxLength="32" value=""
								name="lastname" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">Customer Id</td>
						<td align="left"><input id="customerid" type="text" maxLength="32" value=""
								name="customerid" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">Institution Id</td>
						<td align="left"><input id="institutionid" type="text" maxLength="32" value=""
								name="institutionid" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label0 note" colspan="3">
							<b>Display Option (Optional)</b>
						</td>
					</tr>
					<tr>
						<td class="label">Email Header Image</td>
						<td align="left"><input id="emailheader" type="text" maxLength="32" value="http://bankone.com/images/emailHeaderImage.jpg"
								name="emailheader" runat="server">
						</td>
					</tr>
					<tr>
						<td class="label">Email Marketing Image</td>
						<td align="left"><input id="emailmarketing" type="text" maxLength="32" value="http://bankone.com/images/emailMarketingImage.jpg"
								name="emailmarketing" runat="server">
						</td>
					</tr>
					<tr align="center">                    
						<td colspan="2" class="submit" align="center">
							<span class="pop-button primary" >
								<span><input type="Submit" value="Submit" id="Submit" name="Submit" runat="server" class="button"/></span>
							</span>
					   </td>
					</TR>
				</table>
			</form>
		</div>
	</div>
</body>
</HTML>
