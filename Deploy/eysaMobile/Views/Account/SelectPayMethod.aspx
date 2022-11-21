<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectPayMethodModel>"%>
<%@ Import Namespace="integraMobile.Domain.Abstract"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Select Pay Method
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>&nbsp;</p>
<div id="formulario">

<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.SelectPayMethod_IntroText1+" ("+ViewData["SuscriptionTypeString"]+")"%></h2>

 <% using (Html.BeginForm("SelectPayMethod", "Account", FormMethod.Post,
                        new { @id = "FormSelectPayMethod",  @name = "FormSelectPayMethod", @style = "style=\"position:relative,\"" }))
    { %>  
<h3><%=Resources.SelectPayMethod_IntroText2%></h3>
<fieldset>  
<div class="cajagrisStepPay">

 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
  <p><%= Html.ValidationMessageFor(cust => cust.PaymentMean)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRecharge)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRechargeQuantity)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRechargeWhenBelowQuantity)%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  


<div class="div50-leftStep">
 <p><%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtDebitCreditCard, new { @onclick = "DisableOverWriteCreditCardCheck("+
                    Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard).ToString()+");" })%><img src="../Content/img/tarjetas2.jpg"/></p>
  <p class="aclaracion"><%=Resources.SelectPayMethod_CreditCard%></p>
    <%if (Convert.ToBoolean(ViewData["ShowCheckCreditCard"]))
  {%>
     <p class="aclaracion"><input type="checkbox" id="OverWriteCreditCard" name="OverWriteCreditCard" value="<%=(((bool)ViewData["OverWriteCreditCardValue"]) ? "true" : "false")%>" onClick="toggleOverWriteCreditCard()"/>
        <%=string.Format(Resources.SelectPayMethod_SubstituteCurrentCard,ViewData["CurrentPaymentType_PAN"])%></p>
 <% }%>
</div>
<div class="div25-left">
  <p><%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtPaypal, new
                            {
                                @onclick = "DisableOverWriteCreditCardCheck(" +
                                    Convert.ToInt32(PaymentMeanType.pmtPaypal).ToString() + ");",
                                @disabled = "disabled"
                            })%><img src="../Content/img/estadoinactivo/paypal.png"/></p>
   <p class="aclaracion"><%=Resources.SelectPayMethod_Paypal%></p>
</div>
</div>

<p>&nbsp;</p>
<%if (Convert.ToInt32(ViewData["SuscriptionType"])==(int)PaymentSuscryptionType.pstPrepay)
  {%>

<h3><%=Resources.SelectPayMethod_IntroText3%></h3>
<div class="cajagris">
<%=Resources.SelectPayMethod_IntroText4%>
<p>&nbsp;</p>
  <div class="div100" style="text-align:left;">
  <input type="checkbox" id="AutomaticRecharge" name="AutomaticRecharge" value="<%= Convert.ToString((bool)ViewData["AutomaticRecharge"]).ToLower()%>" onClick="toggleautorecarga()"/>
&nbsp;
<%=Html.LabelFor(cust => cust.AutomaticRecharge)%> 

<div style="display:inline" id="divautorecharge"> 
<br />
<%=Html.LabelFor(cust => cust.AutomaticRechargeQuantity)%> &nbsp;
  <select name="AutomaticRechargeQuantity" id="AutomaticRechargeQuantity">	
    <option value="1000" <%= (1000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>10 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="2000" <%= (2000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>20 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="3000" <%= (3000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>30 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="4000" <%= (4000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>40 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="5000" <%= (5000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>50 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="10000" <%= (10000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>100 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="20000" <%= (20000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>200 <%= ViewData["CurrencyISOCode"]%></option>

  </select>
    <br />  
<%=Html.LabelFor(cust => cust.AutomaticRechargeWhenBelowQuantity)%> &nbsp;
  <select name="AutomaticRechargeWhenBelowQuantity" id="AutomaticRechargeWhenBelowQuantity">	
    <option value="100" <%= (100==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>1 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="200" <%= (200==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>2 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="300" <%= (300==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>3 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="400" <%= (400==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>4 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="500" <%= (500==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>5 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="1000" <%= (1000==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>10 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="1500" <%= (1500==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>15 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="2000" <%= (2000==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>20 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="3000" <%= (3000==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>30 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="4000" <%= (4000==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>40 <%= ViewData["CurrencyISOCode"]%></option>
    <option value="5000" <%= (5000==Convert.ToInt32(ViewData["SelectedQuantityBelow"]))?"selected":"" %>>50 <%= ViewData["CurrencyISOCode"]%></option>  
    </select>
  </div>
  </div>
</div>
<%}else{ %>

<div style="display:inline" id="divChargeMessage"> 
<div class="cajagris">
 <%=string.Format(Resources.SelectPayMethod_PerTransactionMessage,ViewData["ChargeValue"],ViewData["ChargeCurrency"])%>
<p>&nbsp;</p>
  <div class="div100" style="text-align:left;">
  <input type="checkbox" id="AcceptCharge" name="AcceptCharge" value="<%= Convert.ToString((bool)ViewData["AcceptChargeValue"]).ToLower()%>" onClick="toggleacceptcharge()"/>
&nbsp;
<%=Resources.SelectPayMethod_PerTransactionCheckBoxMessage%>
  </div>
</div>
 </div>

<%} %>
<div class="greenhr"><hr /></div> 
<input type="hidden" name="bForceChange" id="bForceChange" value="<%=Convert.ToBoolean(ViewData["ForceChange"])%>" />
<input type="submit" value="<%=Resources.Button_Next %>" class="botonverde" />
<%if (string.IsNullOrEmpty(ViewData["SuscriptionTypeConf"].ToString())) {  %>
<input type="button" value="<%=Resources.SelectPayMethod_ChangeSuscriptionType %>" class="botongris"  onclick="location.href = 'SelectSuscriptionType';" />
<%} %>

<script language="javascript">


<%if (Convert.ToInt32(ViewData["SuscriptionType"]) == (int)PaymentSuscryptionType.pstPerTransaction)
   {
       if (!((bool)ViewData["AcceptChargeValue"]))
       { %>
    document.FormSelectPayMethod.AcceptCharge.checked = false;
    document.FormSelectPayMethod.AcceptCharge.value = false;
<% }
       else
       { %>
    document.FormSelectPayMethod.AcceptCharge.checked = true;
    document.FormSelectPayMethod.AcceptCharge.value = true;

<%}
   } %>

<%if (Convert.ToInt32(ViewData["SuscriptionType"]) == (int)PaymentSuscryptionType.pstPrepay)
  {
      if (!((bool)ViewData["AutomaticRecharge"]))
      { %>
    document.getElementById("divautorecharge").style.visibility = 'hidden';
    document.FormSelectPayMethod.AutomaticRecharge.checked = false;
    document.FormSelectPayMethod.AutomaticRecharge.value = false;
<% }
      else
      { %>
    document.getElementById("divautorecharge").style.visibility = 'visible';
    document.FormSelectPayMethod.AutomaticRecharge.checked = true;
    document.FormSelectPayMethod.AutomaticRecharge.value = true;

<%}
  } %>

<%if (!((bool)ViewData["OverWriteCreditCardValue"]))
      { %>
    if (document.FormSelectPayMethod.OverWriteCreditCard) {
        document.FormSelectPayMethod.OverWriteCreditCard.checked = false;
        document.FormSelectPayMethod.OverWriteCreditCard.value = false;
    }
<% }else{ %>
    if (document.FormSelectPayMethod.OverWriteCreditCard) {
        document.FormSelectPayMethod.OverWriteCreditCard.checked = true;
        document.FormSelectPayMethod.OverWriteCreditCard.value = true;
    }

<%} %>



    <%if (Convert.ToBoolean(ViewData["ShowCheckCreditCard"]))
    {
          
         if(Convert.ToInt32(ViewData["PaymentType"]) == (int)PaymentMeanType.pmtDebitCreditCard)
         {
             %>document.FormSelectPayMethod.OverWriteCreditCard.disabled = false;<%
               
         }
         else
         {             
             %>document.FormSelectPayMethod.OverWriteCreditCard.disabled = true;<%
         }

    } else {%>

        document.getElementById("divChargeMessage").style.visibility = 'visible';

    <%}%>

    function toggleautorecarga() {
        if (document.FormSelectPayMethod.AutomaticRecharge.checked == true) {
            document.FormSelectPayMethod.AutomaticRecharge.value=true;
            document.getElementById("divautorecharge").style.visibility = 'visible';
        }
        else {
            document.FormSelectPayMethod.AutomaticRecharge.value=false;
            document.getElementById("divautorecharge").style.visibility = 'hidden';
        }

    }

    function toggleacceptcharge() {
        if (document.FormSelectPayMethod.AcceptCharge.checked == true) {
            document.FormSelectPayMethod.AcceptCharge.value = true;
        }
        else {
            document.FormSelectPayMethod.AutomaticRecharge.value = false;
        }

    }



    function toggleOverWriteCreditCard() {
        if (document.FormSelectPayMethod.OverWriteCreditCard.checked == true) {

            if (document.getElementById("divChargeMessage"))
                document.getElementById("divChargeMessage").style.visibility = 'visible';
            document.FormSelectPayMethod.OverWriteCreditCard.value = true;
        }
        else {
            if (document.getElementById("divChargeMessage"))
                document.getElementById("divChargeMessage").style.visibility = 'hidden';
            document.FormSelectPayMethod.OverWriteCreditCard.value = false;           
        }

    }

    function DisableOverWriteCreditCardCheck(i) {
        switch (i) {
            case 1: //CreditCard
                document.FormSelectPayMethod.OverWriteCreditCard.disabled = false;
                break;
            case 2: //Paypal
                document.FormSelectPayMethod.OverWriteCreditCard.disabled = true;
                break;
        }
    }

</script>


</fieldset>  
<% } %>
</div>


</asp:Content>
