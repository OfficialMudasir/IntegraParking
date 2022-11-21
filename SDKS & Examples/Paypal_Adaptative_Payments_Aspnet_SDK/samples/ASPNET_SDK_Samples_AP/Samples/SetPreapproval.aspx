<%@ Page language="c#" Codebehind="SetPreapproval.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.SetPreapproval" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Payments - Preapproval</title>
	<link href="style.css" rel="stylesheet" type="text/css">
	<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" Content="C#">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</HEAD>
<body>
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
			<center><h3>Adaptive Payments - Preapproval</h3></center><br/>
			<form id="Form1" method="post" runat="server">
				<table align="center">
					<tr>
						<td class="label">Sender's Email</td>
						<td align="left"><input type="text" maxlength="32" name="senderEmail" id="senderEmail" runat="server"
								value="platfo_1255077030_biz@gmail.com"></td>
					</tr>
					<tr>
						<td class="label">Starting Date</td>
						<td align="left"><input type="text" maxlength="32" name="startingDate" id="startingDate" runat="server"></td>
					</tr>
					<tr>
						<td class="label">Ending Date</td>
						<td><input type="text" maxlength="32" name="endingDate" id="endingDate" runat="server"></td>
					</tr>
					<tr>
						<td class="label0">Maximum Number of Payments</td>
						<td><input type="text" maxlength="32" name="maxNumberOfPayments" id="maxNumberOfPayments"
								runat="server" value="10"></td>
					</tr>
					<tr>
						<td class="label">Maximum Total Amount</td>
						<td><input type="text" maxlength="32" name="maxTotalAmountOfAllPayments" id="maxTotalAmountOfAllPayments"
								runat="server" value="50.00"></td>
					</tr>
					<tr>
						<td class="label">Currency Code</td>
						<td><select name="currencyCode" id="currencyCode" runat="server">
								<option value="USD" selected>USD</option>
								<option value="GBP">GBP</option>
								<option value="EUR">EUR</option>
								<option value="JPY">JPY</option>
								<option value="CAD">CAD</option>
								<option value="AUD">AUD</option>
							</select></td>
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
