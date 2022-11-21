<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Recharge Confirm
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="formulario">
<h1><%=Resources.Account_Recharge_BuyCredit %> </h1> 
<h2></h2>
<h3><%=Resources.Account_RechargeConfirmTitle%></h3>
<% using (Html.BeginForm("PaypalSuccessINT", "Account", FormMethod.Post))
   { %>
<div class="cajagris">
	<div class=div25-right>
	<p><img src="../Content/img/Carro.jpg"/></p>
	</div>

	<div class="div75-right">
	<div class="greentext"><%=Resources.Account_RechargeConfirm_1%> <br/>
    <%=Resources.Account_RechargeConfirm_2%> <br/>
    <p>&nbsp;</p>
    <div class="importe"> <%= ViewData["PayerQuantity"]%> <%= ViewData["PayerCurrencyISOCode"]%> </div>
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
<strong><%=Resources.Account_RechargeConfirm_3%></strong><br/>
<p>&nbsp;</p>
<%=Resources.Account_RechargeConfirm_4%>
<%}else { %>
<strong><%=Resources.Account_RechargeConfirm_4%></strong><br/>
 <%} %>
</div>
</div>

<br/>
<div class="greenhr"><hr/></div>
<br/>
<input type="submit" value="<%=Resources.Account_RechargeButton_Confirm%>" class="botonverde" />
<input type="button" value="<%=Resources.Account_RechargeButton_Cancel%>" class="botongris" onclick="location.href='PaypalCancelINT';"/>
<% } %>
</br>
</div>

</asp:Content>
