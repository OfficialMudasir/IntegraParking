<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Recharge Failure
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div id="formulario">
<h1><%=Resources.Account_Register_PaymentMean %> </h1> 
<h2></h2>
<h3><%=Resources.Account_Register_PaymentMeanError%></h3>
<div class="cajagris">
<div class=div25-right>
	<p><img src="../Content/img/Carro.jpg"/></p>
	</div>
<%  NumberFormatInfo provider = new NumberFormatInfo();
    provider.NumberDecimalSeparator = ".";
%>

	<div class="div75-right">
	<div class="greentext"><%=string.Format(Resources.Account_Register_PaymentMeanMessageError, string.Format(provider, "{0:0.00}", 
                        Convert.ToDouble( ViewData["PayerQuantity"].ToString())/100),ViewData["PayerCurrencyISOCode"]) %> <br/>
    <p>&nbsp;</p>


	</div>
</div>
</div>

<div class=cajagris>
<div class=div25-right>
	<p><img src="../Content/img/visa.jpg"/></p>
</div>

<div class="div75-right">
<span class="SelectUser"><%=Resources.Account_Register_PaymentMean_ChangeToPrepay%></span></p>
        <p>&nbsp;</p>	
        <p><%=Resources.SelectSuscriptionType_Prepay2%>
           <%=string.Format(Resources.SelectSuscriptionType_Prepay3,ViewData["DiscountValue"],ViewData["DiscountCurrency"])%></p>
        <p><%=Resources.SelectSuscriptionType_Prepay4%></p>
        <p>&nbsp;</p>
                <div> <img src="../Content/img/SelectUserOf.png"/> <%=Resources.SelectSuscriptionType_Prepay5%></div>
                <p>&nbsp;</p>
                <div> <img src="../Content/img/SelectUserOc.png"/> <%=Resources.SelectSuscriptionType_Prepay6%></div>
                <p>&nbsp;</p>
                <div><img src="../Content/img/SelectUserMe.png"/> <%=Resources.SelectSuscriptionType_Prepay7%></div>     

</div>
</div>

<br/>
<div class="greenhr"><hr/></div>
<br/>
<input type="button" value="<%=Resources.Account_Register_PaymentMeanRetry%>" class="botonverde" onclick="location.href = 'SelectPayMethod';" />
</br>
</div>



</asp:Content>
