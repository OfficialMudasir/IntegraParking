<%@ Page language="c#" Codebehind="AddPaymentCard.aspx.cs" AutoEventWireup="true" Inherits="ASPNET_SDK_Samples.Samples.AddPaymentCard" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD runat="server">
		<title>PayPal SDK - Adaptive Accounts - Add Payment Card</title>
		<link href="style.css" rel="stylesheet" type="text/css">
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<script type="text/javascript">

	function GenerateCreditCardNumber(){
		var cc_number = new Array(16);
		var cc_len = 16;
		var start = 0;
		var rand_number = Math.random();

		switch(document.AddPaymentCardForm.creditCardType.value)
        {
			case "Visa":
				cc_number[start++] = 4;
				break;
			case "Discover":
				cc_number[start++] = 6;
				cc_number[start++] = 0;
				cc_number[start++] = 1;
				cc_number[start++] = 1;
				break;
			case "MasterCard":
				cc_number[start++] = 5;
				cc_number[start++] = Math.floor(Math.random() * 5) + 1;
				break;
			case "Amex":
				cc_number[start++] = 3;
				cc_number[start++] = Math.round(Math.random()) ? 7 : 4 ;
				cc_len = 15;
				break;
        }

        for (var i = start; i < (cc_len - 1); i++) {
			cc_number[i] = Math.floor(Math.random() * 10);
        }

		var sum = 0;
		for (var j = 0; j < (cc_len - 1); j++) {
			var digit = cc_number[j];
			if ((j & 1) == (cc_len & 1)) digit *= 2;
			if (digit > 9) digit -= 9;
			sum += digit;
		}

		var check_digit = new Array(0, 9, 8, 7, 6, 5, 4, 3, 2, 1);
		cc_number[cc_len - 1] = check_digit[sum % 10];

		document.AddPaymentCardForm.creditCardNumber.value = "";
		for (var k = 0; k < cc_len; k++) {
			document.AddPaymentCardForm.creditCardNumber.value += cc_number[k];
		}
	}

</script>

<body onload ="javascript:GenerateCreditCardNumber(); return false;">
	<div id="wrapper">
		<div id="header">
            <div id="logo">
			    <span id="loginbox">Login to <a href="https://developer.paypal.com" target="_blank">PayPal sandbox</a></span>
			    <a title="Paypal X Home" href="#" id="web_pageheader_anchor_index"><img alt="Paypal X" src="paypal_x_logo.gif"/></a>
		    </div>
        </div>

		<% Server.Execute("ApiList.aspx"); %>  

		<div id="request_form">
			<center><h3>Adaptive Accounts - Add Payment Card</h3></center><br/>

			<form id="AddPaymentCardForm" method="post" runat="server">	
				<table align="center">	<tr>
        <td class="label">Email Id</td>
        <td><input type="text" size="30" maxlength="50" name="emailid" id="emailid" runat="server" value="platfo@paypal.com" /></td>
    </tr>
    <tr>
        <td class="label">Card Type</td>
        <td>	
            <select   name="creditCardType" id="creditCardType" runat="server" onchange="javascript:GenerateCreditCardNumber(); return false;">        
                <option  value="Visa" selected="true">Visa</option>
                <option  value="MasterCard">MasterCard</option>
                <option  value="Discover">Discover</option>
                <option  value="Amex">American Express</option>                    
            </select>
	    </td>
    </tr>
    <tr>
        <td class="label">Credit Card Number</td>
        <td><input type="text" size="19" maxlength="50" ID="creditCardNumber" runat="server" value="4753221149290804" /></td>
    </tr>
     <tr>
        <td class="label">First Name</td>

        <td><input type="text" size="30" maxlength="50" name="firstName"
            value="John" id="firstName" runat="server" /></td>
    </tr>
   
    <tr>
        <td class="label">Last Name</td>
        <td><input type="text" size="30" maxlength="50"
            name="lastName" value="Deo" id="lastName" runat="server" /></td>
    </tr>
	<tr>
	<td class="label">Expiration Date</td>
	<td>
            <select name="expDateMonth" id="expDateMonth" runat="server" class="smallfield">        
                <option  value="01">01</option>
                <option  value="02">02</option>
                <option  value="03">03</option>
                <option  value="04">04</option>
                <option  value="05">05</option>
                <option  value="06">06</option>
                <option  value="07">07</option>
                <option  value="08">08</option>
                <option  value="09">09</option>
                <option  value="10">10</option>
                <option  value="11">11</option>
                <option  value="12">12</option>                     
            </select>
            <select name="expDateYear" id="expDateYear" runat="server" class="smallfield">        
                <option  value="2011">2011</option>
                <option  value="2012" selected="true">2012</option>
                <option  value="2013">2013</option>
                <option  value="2014">2014</option>
                <option  value="2015">2015</option>                   
                <option  value="2016">2016</option>                   
                <option  value="2017">2017</option>                   
                <option  value="2018">2018</option>                   
                <option  value="2019">2019</option>                   
            </select>            
	</td>
</tr>
    <tr>
            <td class="label">Confirmation Type</td>
            <td>
                <select   name="confirmationtype" id="confirmationtype" runat="server">        
                    <option  value="WEB">WEB</option>
                    <option  value="MOBILE">MOBILE</option>
                </select>
            </td>
    </tr>
    <tr>
        <td class="label note"><b>Billing Address</b></td>
    </tr>
    <tr>
        <td class="label">Address1</td>

        <td><input type="text" size="30" maxlength="50" name="address1"
            value="1 Main St" id="address1" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">Address2</td>

        <td><input type="text" size="30" maxlength="50" name="address2"
            value="2nd cross" id="address2" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">City</td>

        <td><input type="text" size="30" maxlength="50" name="city"
            value="Austin" id="city" runat="server" /></td>
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
        <td class="label">Zip Code</td>
        <td><input type="text" size="30" maxlength="50" name="zipCode"
            value="78750" id="zipCode" runat="server" /></td>
   </tr>
    <tr>
        <td class="label">Country</td>
        <td><input type="text" size="30" maxlength="50" name="country"
            value="US" id="country" runat="server" /></td>
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
