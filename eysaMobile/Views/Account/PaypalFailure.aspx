<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Recharge Failure
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="formulario">
<h1><%=Resources.Account_Recharge_BuyCredit %> </h1> 
<h2></h2>
<h3><%=Resources.Account_RechargeFailureTitle%></h3>
<div class="cajagris">
<div class="redtext"><%=Resources.Account_RechargeFailure_1%>
<%if(Convert.ToDouble(ViewData["UserBalance"])<=0)
{%>
<br />
<%=Resources.Account_RechargeFailure_2%>
<%}%>
<br />
<%=Resources.Account_RechargeFailure_3%>
</div>
</div>

<div class=cajagris>
<div class=div25-right>
	<p><img src="../Content/img/paypal.jpg"/></p>
</div>

<div class="div75-right">
<%if(Convert.ToDouble(ViewData["UserBalance"])<=0)
{%>
<strong><%=Resources.Account_RechargeFailure_4%></strong><br/>
<p>&nbsp;</p>
<%=Resources.Account_RechargeFailure_5%>
<%}else { %>
<strong><%=Resources.Account_RechargeFailure_5%></strong><br/>
 <%} %>
</div>
</div>

<br/>
<div class="greenhr"><hr/></div>
<br/>
<input type="button" value="<%=Resources.Account_RechargeButton_Recharge%>" class="botonverde" onclick="location.href='Recharge';" />
<input type="button" value="<%=Resources.Account_RechargeButton_MainMenu%>" class="botongris" onclick="location.href='Main';"/>
</br>
</div>


</asp:Content>
