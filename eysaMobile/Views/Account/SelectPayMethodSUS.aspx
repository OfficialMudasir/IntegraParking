<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectPayMethodModel>"%>
<%@ Import Namespace="integraMobile.Domain.Abstract"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Select Pay Method
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>&nbsp;</p>
<div id="formulario">

<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.SelectPayMethod_IntroText1+" ("+ViewData["SuscriptionTypeString"]+")"%></h2>
<div class="bluehr"><hr /></div>
 <% using (Html.BeginForm("SelectPayMethodSUS", "Account", FormMethod.Post,
                        new { @id = "FormSelectPayMethod",  @name = "FormSelectPayMethod", @style = "style=\"position:relative,\"" }))
    { %>  
<fieldset>  

 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
  <p><%= Html.ValidationMessageFor(cust => cust.PaymentMean)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRecharge)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRechargeQuantity)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRechargeWhenBelowQuantity)%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  

 <p>&nbsp;</p>
  <img src="../Content/img/01.jpg"/>
  <span style="font-weight: bold;line-height: 22px; color: rgb(102, 102, 102); font-family: 'Maven Pro',sans-serif;"><%=Resources.SelectPayMethod_Sus_Option1 %></span>
  <p>&nbsp;</p>
  <div class="div5-left">    
      <%if (Convert.ToBoolean(ViewData["ShowCheckCreditCard"]))
    {%>
     <p class="aclaracion"><input type="checkbox" id="OverWriteCreditCard" name="OverWriteCreditCard" value="<%= (((bool)ViewData["OverWriteCreditCardValue"]) ? "true" : "false")%>" onClick="toggleOverWriteCreditCard()"/>
        </p>
 <% }%></div>
    <div style="line-height: 22px; color: rgb(102, 102, 102); font-family: 'Maven Pro',sans-serif;"> <%=string.Format(Resources.SelectPayMethod_SubstituteCurrentCard,ViewData["CurrentPaymentType_PAN"])%></div>
 <p>&nbsp;</p>
 <p>&nbsp;</p>
     <div class="div100">
  <img src="../Content/img/02.jpg"/>
  <span style="font-weight: bold;line-height: 22px; color: rgb(102, 102, 102); font-family: 'Maven Pro',sans-serif;"><%=Resources.SelectPayMethod_Sus_Option2 %></span>
</div>
<div class="cajagrisStepPay">
<div class="div50-left">
 <p><%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtDebitCreditCard, new { @onclick = "DisableOverWriteCreditCardCheck("+
                    Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard).ToString()+");" })%><img src="../Content/img/tarjetas2.jpg"/></p>


</div>
<div class="div50-right">
  <p><%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtPaypal, new
                            {
                                @onclick = "DisableOverWriteCreditCardCheck(" +
                                    Convert.ToInt32(PaymentMeanType.pmtPaypal).ToString() + ");",
                                @disabled = "disabled"
                            })%><img src="../Content/img/estadoinactivo/paypal.png" /></p>
</div>
</div>

<%if (Convert.ToInt32(ViewData["SuscriptionType"])==(int)PaymentSuscryptionType.pstPrepay)
  {%>

  <div class="div100">
  <img src="../Content/img/03.jpg"/>
  <span style="font-weight: bold;line-height: 22px; color: rgb(102, 102, 102); font-family: 'Maven Pro',sans-serif;"><%=Resources.SelectPayMethod_Sus_Option3 %></span>
<p>&nbsp;</p>
<span style="line-height: 22px; color: rgb(102, 102, 102); font-family: 'Maven Pro',sans-serif;"><%=Resources.SelectPayMethod_IntroText4%></span>
</div>
     <p>&nbsp;</p>
  <div class="div100" style="display:inline;text-align:left;line-height: 22px; color: rgb(102, 102, 102); font-family: 'Maven Pro',sans-serif;">
      <div class="div5-left">
        <input type="checkbox" id="AutomaticRecharge" name="AutomaticRecharge" value="<%= Convert.ToString((bool)ViewData["AutomaticRecharge"]).ToLower()%>" onClick="toggleautorecarga()"/>
       </div>
       <div class="div50-right"><%=Html.LabelFor(cust => cust.AutomaticRecharge)%> 

<div style="display:inline" id="divautorecharge"> 
 <p>&nbsp;</p>
<%=Html.LabelFor(cust => cust.AutomaticRechargeQuantity)%> &nbsp;
  <select name="AutomaticRechargeQuantity" id="AutomaticRechargeQuantity" style="width:initial;color:#666;">	
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
  <select name="AutomaticRechargeWhenBelowQuantity" id="AutomaticRechargeWhenBelowQuantity" style="width:initial;color:#666;">	
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

 <br/>
<div class="cajazul2">
  <h4><%=Resources.SelectPayMethod_Sus_Remark1 %></h4>
  <p>&nbsp;</p>
  <div class="div100-blu1">
<ul>
  <li type="disc"><%=Resources.SelectPayMethod_Sus_Remark2 %></li>
  <li type="disc"><%=Resources.SelectPayMethod_Sus_Remark3 %></li>
</ul>
</div>
  <div class="div100-blu2">
    <span style="color: #000"><%=Resources.SelectPayMethod_Sus_Remark4 %></span><p>
    <span style="color: #000"><%=Resources.SelectPayMethod_Sus_Remark5 %></span>
  </div>
  <p>&nbsp;</p>
  <div class="div100">
<span style="color:#000"><%=Resources.SelectPayMethod_Sus_Remark6 %></span>
<p>&nbsp;</p>
</div>
</div>

<div class="bluehr"><hr /></div> 
<input type="hidden" name="bForceChange" id="bForceChange" value="<%=Convert.ToBoolean(ViewData["ForceChange"])%>" />
<input type="submit" value="<%=Resources.Button_Next %>" class="botonverde" />
<%if (string.IsNullOrEmpty(ViewData["SuscriptionTypeConf"].ToString())) {  %>
<input type="button" value="<%=Resources.SelectPayMethod_ChangeSuscriptionType %>" class="botongris"  onclick="location.href = 'SelectSuscriptionTypeINT';" />
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
