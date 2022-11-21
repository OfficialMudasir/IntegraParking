<%@ Page language="c#" Codebehind="CreateAccountBusiness.aspx.cs" AutoEventWireup="false" Inherits="ASPNET_SDK_Samples.Samples.CreateAccountBusiness" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
<HEAD>
	<title>PayPal SDK - Adaptive Accounts - Create Account (Business)</title>
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
			<center><h3>Adaptive Accounts - Create Account (Business)</h3></center>						
			<br/>
			<form id="Form1" method="post" runat="server">	
				<table align="center">	
					<tr>
						<td colspan="2" class="label note"><b>Personal Info</b></td>
					</tr>
					<tr>
						<td class="label">Account Type</td>
						<td><select name="accountType" id="accountType" runat="server">
								<option value="BUSINESS" selected>BUSINESS</option>								
							</select></td>
					</tr>
					<tr>
						<td class="label">Salutation</td>
						<td><select name="name.salutation" id="nameSalutation" runat="server">
								<option value="Dr." selected>Dr.</option>
								<option value="Mr.">Mr.</option>
								<option value="Mrs.">Mrs.</option>
							</select></td>
					</tr>
					<tr>
						<td class="label">First Name</td>
						<td><input type="text" maxlength="100" name="name.firstName" id="nameFirstName" runat="server"
								value="Bonzop"></td>
					</tr>
					<tr>
						<td class="label">Middle Name</td>
						<td><input type="text" maxlength="100" name="name.middleName" id="nameMiddleName"
								runat="server" value="Simore"></td>
					</tr>
					<tr>
						<td class="label">Last Name</td>
						<td><input type="text" maxlength="100" name="name.lastName" id="nameLastName" runat="server"
								value="Zaius"></td>
					</tr>
					<tr>
						<td class="label">DOB</td>
						<td><input type="text" maxlength="50" name="dateOfBirth" id="dateOfBirth" runat="server"
								value="1968-01-01Z"></td>
					</tr>
					<tr>
						<td class="label">Address 1</td>
						<td><input type="text" maxlength="100" name="address.line1" id="addressLine1" runat="server"
								value="1968 Ape Way"></td>
					</tr>
					<tr>
						<td class="label">Address 2</td>
						<td><input type="text" maxlength="100" name="address.line2" id="addressLine2" runat="server"
								value="Apt 123"></td>
					</tr>
					<tr>
						<td class="label">City</td>
						<td><input type="text" maxlength="40" name="address.city" id="addressCity" runat="server"
								value="Austin"></td>
					</tr>
					<tr>
						<td class="label">State</td>
						<td><select name="address.state" id="addressState" runat="server">
								<option></option>
								<option value="AK">AK</option>
								<option value="AL">AL</option>
								<option value="AR">AR</option>
								<option value="AZ">AZ</option>
								<option value="CA">CA</option>
								<option value="CO">CO</option>
								<option value="CT">CT</option>
								<option value="DC">DC</option>
								<option value="DE">DE</option>
								<option value="FL">FL</option>
								<option value="GA">GA</option>
								<option value="HI">HI</option>
								<option value="IA">IA</option>
								<option value="ID">ID</option>
								<option value="IL">IL</option>
								<option value="IN">IN</option>
								<option value="KS">KS</option>
								<option value="KY">KY</option>
								<option value="LA">LA</option>
								<option value="MA">MA</option>
								<option value="MD">MD</option>
								<option value="ME">ME</option>
								<option value="MI">MI</option>
								<option value="MN">MN</option>
								<option value="MO">MO</option>
								<option value="MS">MS</option>
								<option value="MT">MT</option>
								<option value="NC">NC</option>
								<option value="ND">ND</option>
								<option value="NE">NE</option>
								<option value="NH">NH</option>
								<option value="NJ">NJ</option>
								<option value="NM">NM</option>
								<option value="NV">NV</option>
								<option value="NY">NY</option>
								<option value="OH">OH</option>
								<option value="OK">OK</option>
								<option value="OR">OR</option>
								<option value="PA">PA</option>
								<option value="RI">RI</option>
								<option value="SC">SC</option>
								<option value="SD">SD</option>
								<option value="TN">TN</option>
								<option value="TX" selected>TX</option>
								<option value="UT">UT</option>
								<option value="VA">VA</option>
								<option value="VT">VT</option>
								<option value="WA">WA</option>
								<option value="WI">WI</option>
								<option value="WV">WV</option>
								<option value="WY">WY</option>
								<option value="AA">AA</option>
								<option value="AE">AE</option>
								<option value="AP">AP</option>
								<option value="AS">AS</option>
								<option value="FM">FM</option>
								<option value="GU">GU</option>
								<option value="MH">MH</option>
								<option value="MP">MP</option>
								<option value="PR">PR</option>
								<option value="PW">PW</option>
								<option value="VI">VI</option>
							</select></td>
					</tr>
					<tr>
						<td class="label">ZIP Code</td>
						<td><input type="text" maxlength="10" name="address.postalCode" id="addressPostalCode"
								runat="server" value="78750"><span class="greybox"> (5 or 9 digits)</span></td>
					</tr>
					<tr>
						<td class="label">Country</td>
						<td><input type="text" maxlength="30" name="address.countryCode" id="addressCountryCode"
								runat="server" value="US"></td>
					</tr>
					<tr>
						<td class="label">Citizenship Country Code</td>
						<td><input type="text" maxlength="30" name="citizenshipCountryCode" id="citizenshipCountryCode"
								runat="server" value="US"></td>
					</tr>
					<tr>
						<td class="label">Contact Phone Number</td>
						<td><input type="text" maxlength="50" name="contactPhoneNumber" id="contactPhoneNumber"
								runat="server" value="512-691-4160"></td>
					</tr>
					<tr>
						<td class="label">Notification URL</td>
						<td><input type="text" maxlength="100" name="notificationURL" id="notificationURL"
								runat="server" value="http://stranger.paypal.com/cgi-bin/ipntest.cgi"></td>
					</tr>
					<tr>
						<td class="label">Partner Field 1</td>
						<td><input type="text" maxlength="50" name="partnerField1" id="partnerField1" runat="server"
								value="p1"></td>
					</tr>
					<tr>
						<td class="label">Partner Field 2</td>
						<td><input type="text" maxlength="50" name="partnerField2" id="partnerField2" runat="server"
								value="p2"></td>
					</tr>
					<tr>
						<td class="label">Partner Field 3</td>
						<td><input type="text" maxlength="50" name="partnerField3" id="partnerField3" runat="server"
								value="p3"></td>
					</tr>
					<tr>
						<td class="label">Partner Field 4</td>
						<td><input type="text" maxlength="50" name="partnerField4" id="partnerField4" runat="server"
								value="p4"></td>
					</tr>
					<tr>
						<td class="label">Partner Field 5</td>
						<td><input type="text" maxlength="50" name="partnerField5" id="partnerField5" runat="server"
								value="p5"></td>
					</tr>
					<tr>
						<td class="label">currencyCode</td>
						<td><select name="currencyCode" id="currencyCode" runat="server">
								<option value="USD" selected>USD</option>
								<option value="GBP">GBP</option>
								<option value="EUR">EUR</option>
								<option value="JPY">JPY</option>
								<option value="CAD">CAD</option>
								<option value="AUD">AUD</option>
							</select></td>
					</tr>
					<tr>
						<td class="label0">Developer Network Email <div class="greybox"> (Applicable only for Sandbox)</div></td>
						<td><input type="text" maxlength="100" name="sandboxDeveloperEmail" id="sandboxDeveloperEmail" value="platform.sdk.seller@gmail.com" runat="server"
								></td>
					</tr>
					<tr>
						<td class="label">Email</td>
						<td><input type="text" maxlength="100" name="sandboxEmail" id="sandboxEmail" value="platfo_1255612361_per@gmail.com" runat="server"
								></td>
					</tr>
					
	   <tr>
	            <td class="label note"><b>Business Info</b></td>
	   </tr>
	   
				
	<tr>
	<td class="label">Business Name</td>
	 <td><input type="text" maxlength="100"
            name="biz_firstName" value="Bonzop" id="biz_firstName" runat="server" /></td>
	</tr>

        <tr>
	<td class="label">Biz Address Line1</td>

	 <td><input type="text" maxlength="100" name="biz_address_line1"
            value="1968 Ape Way" id="biz_address_line1" runat="server" /></td>
</tr>

        <tr>
	<td class="label">Biz Address Line2</td>
	 <td><input type="text" maxlength="100" name="biz_address_line2" value="Apt 123" id="biz_address_line2" runat="server"/></td>
</tr>

        <tr>
	<td class="label">City</td>

 <td><input type="text" maxlength="50" name="biz_address_city"
            value="Austin" id="biz_address_city" runat="server" /></td>
	
</tr>

        <tr>
	<td class="label">State</td>
	  <td><select name="biz_address_state" id="biz_address_state" runat="server">
            <option></option>
            <option value="AK">AK</option>

            <option value="AL">AL</option>
            <option value="AR">AR</option>
            <option value="AZ">AZ</option>
            <option value="CA">CA</option>
            <option value="CO">CO</option>
            <option value="CT">CT</option>

            <option value="DC">DC</option>
            <option value="DE">DE</option>
            <option value="FL">FL</option>
            <option value="GA">GA</option>
            <option value="HI">HI</option>
            <option value="IA">IA</option>

            <option value="ID">ID</option>
            <option value="IL">IL</option>
            <option value="IN">IN</option>
            <option value="KS">KS</option>
            <option value="KY">KY</option>
            <option value="LA">LA</option>

            <option value="MA">MA</option>
            <option value="MD">MD</option>
            <option value="ME">ME</option>
            <option value="MI">MI</option>
            <option value="MN">MN</option>
            <option value="MO">MO</option>

            <option value="MS">MS</option>
            <option value="MT">MT</option>
            <option value="NC">NC</option>
            <option value="ND">ND</option>
            <option value="NE">NE</option>
            <option value="NH">NH</option>

            <option value="NJ">NJ</option>
            <option value="NM">NM</option>
            <option value="NV">NV</option>
            <option value="NY">NY</option>
            <option value="OH">OH</option>
            <option value="OK">OK</option>

            <option value="OR">OR</option>
            <option value="PA">PA</option>
            <option value="RI">RI</option>
            <option value="SC">SC</option>
            <option value="SD">SD</option>
            <option value="TN">TN</option>

            <option value="TX" selected>TX</option>
            <option value="UT">UT</option>
            <option value="VA">VA</option>
            <option value="VT">VT</option>
            <option value="WA">WA</option>
            <option value="WI">WI</option>

            <option value="WV">WV</option>
            <option value="WY">WY</option>
            <option value="AA">AA</option>
            <option value="AE">AE</option>
            <option value="AP">AP</option>
            <option value="AS">AS</option>

            <option value="FM">FM</option>
            <option value="GU">GU</option>
            <option value="MH">MH</option>
            <option value="MP">MP</option>
            <option value="PR">PR</option>
            <option value="PW">PW</option>

            <option value="VI">VI</option>
        </select></td>
</tr>

        <tr>
	<td class="label">ZIP Code</td>
        <td><input type="text" maxlength="50" name="biz_address_postalCode"
            value="78750" id="biz_address_postalCode" runat="server" /><span class="greybox"> (5 or 9 digits)</span></td>
</tr>

        <tr>
	  <td class="label">Country</td>
        <td><input type="text" maxlength="50" name="biz_address_countryCode"
            value="US" id="biz_address_countryCode" runat="server" /></td>
</tr>
   
     <tr>
	<td class="label">Work Phone</td>
	 <td><input type="text" maxlength="50" name="biz_contactPhoneNumber"
            value="5126914160" id="biz_contactPhoneNumber" runat="server" /></td>
</tr>

        <tr>
	<td class="label">Category</td>
	 <td><input type="text" maxlength="50" name="biz_CategoryCode"
            value="1001" id="biz_CategoryCode" runat="server" /></td>
</tr>


        <tr>
	<td class="label">Sub Category</td>
	 <td><input type="text" maxlength="50" name="biz_subCategoryCode"
            value="2001" id="biz_subCategoryCode" runat="server" /></td>

</tr>

        <tr>
	<td class="label">Customer Service Phone</td>
<td><input type="text" maxlength="50" name="biz_customerServicePhone"
            value="5126914160" id="biz_customerServicePhone" runat="server" /></td>
	
</tr>

        <tr>
	<td class="label">Customer Service Email</td>
	   <td><input type="text" maxlength="100" name="biz_customerServiceEmail"
            value="platfo_1255076101_per@gmail.com" id="biz_customerServiceEmail" runat="server" /></td>

</tr>
     <tr>
	<td class="label">Website</td>
	<td><input type="text" maxlength="100" name="biz_webSite"
            value="https://www.x.com" id="biz_webSite" runat="server" /></td>
</tr>

<tr>
	<td class="label">Date Of Establishment</td>
	<td><input type="text" maxlength="50"
            name="biz_dateOfEstablishment" value="2000-01-01" id="biz_dateOfEstablishment" runat="server" /></td>
</tr>

   <tr>
	<td class="label">Business Type</td>
	<td>
		<select name="businessType" id="businessType" runat="server" >
			<option  value="ASSOCIATION">ASSOCIATION</option>
			<option value="CORPORATION">CORPORATION</option>
			<option value="GENERAL_PARTNERSHIP">GENERAL_PARTNERSHIP</option>

			<option value="GOVERNMENT">GOVERNMENT</option>
			<option value="INDIVIDUAL" selected >INDIVIDUAL</option>
			<option value="LIMITED_LIABILITY_PARTNERSHIP">LIMITED_LIABILITY_PARTNERSHIP</option>
			<option value="LIMITED_LIABILITY_PRIVATE_CORPORATION">LIMITED_LIABILITY_PRIVATE_CORPORATION</option>
			<option value="LIMITED_LIABILITY_PROPRIETORS">LIMITED_LIABILITY_PROPRIETORS</option>
			<option value="LIMITED_PARTNERSHIP">LIMITED_PARTNERSHIP</option>

			<option value="LIMITED_PARTNERSHIP_PRIVATE_CORPORATION">LIMITED_PARTNERSHIP_PRIVATE_CORPORATION</option>
			<option value="NONPROFIT">NONPROFIT</option>
			<option value="OTHER_CORPORATE_BODY">OTHER_CORPORATE_BODY</option>
			<option value="PARTNERSHIP">PARTNERSHIP</option>
			<option value="PRIVATE_CORPORATION">PRIVATE_CORPORATION</option>
			<option value="PRIVATE_PARTNERSHIP">PRIVATE_PARTNERSHIP</option>

			<option value="PROPRIETORSHIP">PROPRIETORSHIP</option>
			<option value="PROPRIETORSHIP_CRAFTSMAN">PROPRIETORSHIP_CRAFTSMAN</option>
			<option value="PROPRIETARY_COMPANY">PROPRIETARY_COMPANY</option>
			<option value="PUBLIC_COMPANY">PUBLIC_COMPANY</option>
			<option value="PUBLIC_CORPORATION">PUBLIC_CORPORATION</option>
			<option value="PUBLIC_PARTNERSHIP">PUBLIC_PARTNERSHIP</option>

		</select>
	</td>
</tr>
     <tr>
		<td class="label">Average Price</td>
		<td><input type="text" maxlength="50"
				name="biz_averagePrice" value="1.00" id="biz_averagePrice" runat="server" /></td>
	</tr>
	<tr>
		<td class="label">Average Monthly Volume</td>
		<td><input type="text" maxlength="50"
            name="biz_averageMonthlyVolume" value="100" id="biz_averageMonthlyVolume" runat="server" /></td>
	</tr>

    <tr>
		<td class="label0">Percentage Revenue From Online</td>
		<td><input type="text" maxlength="50"
            name="biz_percentageRevenueFromOnline" value="60" id="biz_percentageRevenueFromOnline" runat="server" /></td>
	</tr>          
	<tr>
		<td class="label">Sales Venue</td>
		<td><input type="text" maxlength="50"
            name="biz_salesVenue" value="WEB" id="biz_salesVenue" runat="server" /></td>
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
