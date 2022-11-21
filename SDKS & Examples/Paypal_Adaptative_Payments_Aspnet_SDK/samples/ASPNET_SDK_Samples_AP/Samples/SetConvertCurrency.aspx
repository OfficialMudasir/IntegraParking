<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetConvertCurrency.aspx.cs" Inherits="ASPNET_SDK_Samples.Samples.SetConvertCurrency" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
        <title>PayPal SDK - Adaptive Payments - Convert Currency</title>
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

		<div id="request_form">
			<center><h3>Adaptive Payments - Convert Currency</h3></center><br/>
		    <form id="form1" runat="server">
				<table align="center">		   
					<tr>
						<td class="label"><b>Conversion Details</b></td>
						<td>Amount **</td>
						<td>Currency Code **</td>		
					</tr>
					<tr>
						<td>
						<P align="center">1</P>
						</td>
						<td><input type="text" name="fromcode" id="fromcode_0" 
							value="GBP" runat="server"></td>
						<td><input type="text" name="baseamount" id="amount_0"
							value="1.00" runat="server" class="smallfield"></td>							
					</tr>		
					<tr>
						<td>
						<P align="center">2</P>
						</td>				
						<td><input type="text" name="fromcode" id="fromcode_1"  
							value="EUR" runat="server"></td>				
						<td><input type="text" name="baseamount" id="amount_1"
							value="100.00" runat="server" class="smallfield"></td>
						</tr>
					<tr>
						<td class="label0"><b>Convert To Currency List</b></td>		
						<td>Currency Code **</td>		
					</tr>
					<tr>	
					<td>
						<P align="center">1</P>
						</td>	
						<td><input type="text" name="tocode" id="tocode_0"   
							value="USD" runat="server"></td>			
					</tr>
					<tr>	
					   <td>
						<P align="center">2</P>
						</td>	
						<td><input type="text" name="tocode" id="tocode_1"  
							value="CAD" runat="server"></td>			
					</tr>		
					<tr>	
					   <td>
						<P align="center">3</P>
						</td>	
						<td><input type="text" name="tocode" id="tocode_2"  
							value="JPY" runat="server"></td>			
					
					</tr>
					<tr>
						<td colspan="3" align="right" class="note">** <i>Required field</i></td>
					</tr>					
					<tr align="center">                    
						<td colspan="2" class="submit" align="center">
							<span class="pop-button primary" >
								<span><input type="submit" id="Submit" value="Submit" runat="server" class="button"></span>
							</span>
						</td>
					</tr>				
				</table>
			 </form>
		</div>
	</div>
</body>
</html>
