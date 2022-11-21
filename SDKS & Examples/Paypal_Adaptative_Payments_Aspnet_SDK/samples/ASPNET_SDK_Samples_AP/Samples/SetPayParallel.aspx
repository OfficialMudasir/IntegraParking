<%@ Page language="c#" Codebehind="SetPayParallel.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.SetPayParallel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Payments - Pay Parallel</title>
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
			<center><h3>Adaptive Payments - Pay Parallel</h3></center><br/>
			<form id="Form1" method="post" runat="server">	
				<table align="center">
					<tr>
						<td class="label">Sender's Email</td>
						<td align="left">
							<input type="text" id="email" runat="server" maxlength="80" name="email" value="platfo_1255077030_biz@gmail.com">
						</td>
					</tr>
					<tr>
						<td class="label">Memo</td>
						<td><input type="text" maxlength="32" name="memo" id="memo" runat="server" value="parallel"></td>
					</tr>
					<tr>
						<td class="label">Fees Payer</td>
						<td><input type="text" maxlength="32" name="feesPayer" id="feesPayer" runat="server"
								value="SENDER"></td>
					</tr>
					<tr>
						<td class="label">Currency Code</td>
						<td><select name="currencyCode" id="currencyCode" runat="server">
								<option value="USD" selected>
									USD
								</option>
								<option value="GBP">
									GBP
								</option>
								<option value="EUR">
									EUR
								</option>
								<option value="JPY">
									JPY
								</option>
								<option value="CAD">
									CAD
								</option>
								<option value="AUD">
									AUD
								</option>
							</select></td>
					</tr>
					<tr>
						<td class="label note" colspan="3">
								<b>Receiver Details</b>
						</td>
					</tr>
					<tr>
						<td>Payee</td>
						<td>ReceiverEmail  **</td>
						<td>Amount **</td>
					</tr>
					<tr>
						<td>
							<p align="center">1</p>
						</td>
						<td><input type="text" name="receiveremail[0]" id="receiveremail_0" value="platfo_1255612361_per@gmail.com"
								runat="server"></td>
						<td><input type="text" name="amount[0]" id="amount_0" maxlength="7" value="1.00" runat="server" class="smallfield"></td>
					</tr>
					<tr>
						<td>
							<p align="center">2</p>
						</td>
						<td><input type="text" name="receiveremail[1]" value="platfo_1255611349_biz@gmail.com"
								id="receiveremail_1" runat="server"></td>
						<td><input type="text" name="amount[1]" id="amount_1" maxlength="7" value="1.00" runat="server" class="smallfield"></td>
					</tr>
					<tr>
						<td colspan="3" align="right" class="note">** <i>Required field</i></td>
					</tr>
					<tr align="center">                    
						<td colspan="2" class="submit" align="center">
							<span class="pop-button primary" >
								<span><input type="submit" value="Submit" id="Submit" runat="server" NAME="Submit" class="button"></span>
							</span>
						</td>
					</TR>
				</table>
			</form>
		</div>
	</div>
</body>
</HTML>
