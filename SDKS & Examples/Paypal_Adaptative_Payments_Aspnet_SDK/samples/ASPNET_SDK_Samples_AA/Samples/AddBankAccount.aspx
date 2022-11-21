<%@ Page language="c#" Codebehind="AddBankAccount.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.AddBankAccount" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Accounts - Add Bank Account</title>
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
			<center><h3>Adaptive Accounts - Add Bank Account</h3></center>						
			<br/>
			<form id="Form1" method="post" runat="server">	
				<table align="center">
					  <tr>
        <td class="label">Email Id</td>
        <td><input type="text" size="30" maxlength="50" name="emailid" id="emailid" runat="server" value="platfo@paypal.com" /></td>
    </tr>
     <tr>
        <td class="label">Bank Country Code</td>

        <td><input type="text" size="30" maxlength="50" name="bankCountryCode"
            value="US" id="bankCountryCode" runat="server" /></td>
    </tr>
   
    <tr>
        <td class="label">Bank Name</td>
        <td><input type="text" size="30" maxlength="50"
            name="bankName" value="Huntington Bank" id="bankName" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">Routing Number</td>

        <td><input type="text" size="30" maxlength="50" name="routingNumber"
            value="021473030" id="routingNumber" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">Bank Account Number</td>
        <td><input type="text" size="30" maxlength="50" name="bankAccountNumber"
            value="162951" id="bankAccountNumber" runat="server" /></td>
     </tr>
     <tr>
        <td class="label">Account Type</td>

        <td>
        <select   name="accounttype" id="accounttype" runat="server">        
        <option  value="CHECKING">CHECKING</option>
         <option  value="SAVINGS">SAVINGS</option>
         <option  value="BUSINESS_CHECKING">BUSINESS_CHECKING</option>
         <option  value="BUSINESS_SAVINGS">BUSINESS_SAVINGS</option>
         <option  value="NORMAL">NORMAL</option>
         <option  value="UNKNOWN">UNKNOWN</option>
          </select>
</td>
     </tr>
      <tr>
        <td class="label">Confirmation Type</td>
        <td>
        <select name="confirmationType" id="confirmationType" runat="server">
        <option  value="WEB">WEB</option>
         <option  value="MOBILE">MOBILE</option>
          </select>
        </td>
     </tr>

  					<tr align="center">                    
						<td colspan="2" class="submit" align="center">
							<span class="pop-button primary" >
								<span>
									<input type="submit" value="Submit" id="Submit" runat="server" class="button">
								</span>
							</span>
						</td>
					</tr>
				</table>
			</form>
		</div>
	</div>
</body>
</HTML>
