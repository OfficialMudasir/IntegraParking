<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Preapproval Success
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>&nbsp;</p>
<div id="formulario">
<h1><%=Resources.Account_Recharge_PaymentsWithPreapproval%> </h1> 
<h2></h2>
<h3><%=Resources.Account_Recharge_PaymentsWithPreaprovalTitle%></h3>
<% using (Html.BeginForm("PaypalPreapprovalPayRedirect", "Account", FormMethod.Post))
   { %>
<div class="cajagris">
	<div class=div25-right>
	<p><img src="../Content/img/Carro.jpg"/></p>
	</div>

	<div class="div75-right">
	<div class="greentext"><%=Resources.Account_Recharge_PaymentsWithPreaproval_1%> <br/>
    <p>&nbsp;</p>
<%  NumberFormatInfo provider = new NumberFormatInfo();
    provider.NumberDecimalSeparator = ".";
%>
    <div class="importe"> 
    <%=string.Format(provider, "{0:0.00} {1}", Convert.ToDouble(ViewData["PayerQuantity"]) / 100.0, ViewData["PayerCurrencyISOCode"])%>
    </div>
	</div>
    </div>
</div>

<div class=cajagris>
<div class=div25-right>
	<p><img src="../Content/img/paypal.jpg"/></p>
</div>

<div class="div75-right">
<%if(Convert.ToDouble(ViewData["UserBalance"])<=0)
{%>
<strong><%=Resources.Account_Recharge_PaymentsWithPreaproval_2%></strong><br/>
<p>&nbsp;</p>
<%=Resources.Account_RechargeSuccess_3%>
<%}else { %>
<strong><%=Resources.Account_RechargeSuccess_3%></strong><br/>
 <%} %>
</div>
</div>

<br/>
<div class="greenhr"><hr/></div>
<br/>
<%if(Convert.ToDouble(ViewData["UserBalance"])>0)
{%>
<input type="button" value="<%=Resources.Account_RechargeButton_MainMenu%>" class="botongris" onclick="location.href = 'Main';"/>
<%} %>
<input type="submit" value="<%=Resources.Account_RechargeButton_Recharge%>" class="botonverde" />

<% } %>
</br>
</div>

</asp:Content>
