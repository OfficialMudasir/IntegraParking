<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RechargeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Recharge
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="formulario">
<h1> <%=Resources.Account_Recharge_BuyCredit %></h1> 
<h2></h2>

<%if(Convert.ToDouble(ViewData["UserBalance"])<=0)
{%>
<div class="cajagris">
<div class="redtext">
<%=Resources.Account_RechargeMessage_1%>
<br />
<%=Resources.Account_RechargeMessage_2%>
</div>
</div>
<%} else if (Convert.ToBoolean(Session["OVERWRITE_CARD"])) {%>
<div class="cajagris">
<div class="redtext">
<%=Resources.Account_RechargeMessage_3%>
</div>
</div>
<%} %>

<h3><%=Resources.Account_Recharge_SelectAnAmount%></h3>
<!--<div class=cajagris>
<div class=div25-right>
	<p>
    <% if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) { %>
        <img src="../Content/img/paypal.jpg" >
    <% } else { %>
        <img src="../Content/img/visa.jpg"/>
    <% } %>       
    </p>
</div>
<div class=div50-right>
	<p><strong><%=Resources.Account_Recharge_Billing_Address%></strong></p>
	<p><%=ViewData["Name"]%></p>
	<p><%=ViewData["Address"]%></p>
	<p><%=ViewData["City"]%></p>
	<p><%=ViewData["State"]%></p>
	<p><%=ViewData["ZipCode"]%></p>
	<p><%=ViewData["Country"]%></p>
</p>
</div>	
</div>-->  
 <% using (Html.BeginForm("Recharge", "Account", FormMethod.Post,
        new { @id = "FormRecharge", @name = "FormRecharge", @style = "style=\"position:relative,\"" }))
        { %>

 
<div class=cajagris>
    <table border="0" style="width:100%;">
        <tr>
            <td style="width:50%;">
                <h4><%= Resources.Account_RechargeValue_Label %></h4><br />
  <% var oRechargeValues = ViewData["RechargeValues"] as List<string>;
     var oRechargeValuesBase = ViewData["RechargeValuesBase"] as List<string>;
     var oRechargeValuesFEE = ViewData["RechargeValuesFEE"] as List<string>;
     var oRechargeValuesVAT = ViewData["RechargeValuesVAT"] as List<string>;
     for(int i=0; i<(ViewData["RechargeValues"] as List<string>).Count; i++) { %>
  <div class="div-leftbuycredit">
    <p><input type="radio" name="RechargeQuantity" value="<%= oRechargeValuesBase[i] %>" 
                                                   <% if (i == Convert.ToInt32(ViewData["RechargeDefaultValueIndex"])) { %> checked <% } %>
                           onclick="SetServiceOrder('<%= i %>');" />
        <%= string.Format(Resources.Account_RechargeValue, oRechargeValuesBase[i], ViewData["CurrencyISOCode"])%>
        <%= (i.ToString() == ViewData["RechargeMinValueIndex"].ToString() ? Resources.Account_RechargeMinImportRemark : "") %>
        <input type="hidden" id="hdnServiceBase<%= i %>" value="<%= string.Format(Resources.Account_RechargeValue, oRechargeValuesBase[i], ViewData["CurrencyISOCode"]) %>" />
        <input type="hidden" id="hdnServiceFEE<%= i %>" value="<%= string.Format(Resources.Account_RechargeValue, oRechargeValuesFEE[i], ViewData["CurrencyISOCode"]) %>" />
        <input type="hidden" id="hdnServiceVAT<%= i %>" value="<%= string.Format(Resources.Account_RechargeValue, oRechargeValuesVAT[i], ViewData["CurrencyISOCode"]) %>" />
        <input type="hidden" id="hdnServiceTotal<%= i %>" value="<%= string.Format(Resources.Account_RechargeValue, oRechargeValues[i], ViewData["CurrencyISOCode"]) %>" />
  </div>
  <% } %>
            </td>
            <td style="<% if (Model.PercVAT1 == 0 && Model.PercFEE == 0 && Model.FixedFEE == 0) { %> display:none; <% } %>">
                <h4><%= Resources.Account_RechargeValueOrder_Label %></h4><br/>
                <table border="0" style="padding-left: 10px; width:70%;">
                    <tr>
                        <td style="padding-left: 20px;"><%= Resources.Account_RechargeValueBase_Label %></td>
                        <td style="text-align:right;" id="tdCreditBase"></td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px;"><%= Resources.Account_RechargeValueFEE_Label %></td>
                        <td style="text-align:right;" id="tdCreditFEE"></td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px;"><%= Resources.Account_RechargeValueVAT_Label %></td>
                        <td style="text-align:right;" id="tdCreditVAT"></td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px; font-weight: bold;"><%= Resources.Account_RechargeValueTotal_Label %></td>
                        <td style="text-align:right; font-weight: bold;" id="tdCreditTotal"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" name="PercVAT1" value="<%= Model.PercVAT1 %>" />
    <input type="hidden" name="PercVAT2" value="<%= Model.PercVAT2 %>" />
    <input type="hidden" name="PercFEE" value="<%= Model.PercFEE %>" />
    <input type="hidden" name="PercFEETopped" value="<%= Model.PercFEETopped %>" />
    <input type="hidden" name="FixedFEE" value="<%= Model.FixedFEE %>" />

</div>
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRecharge)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRechargeQuantity)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AutomaticRechargeWhenBelowQuantity)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.PaypalID)%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div> 
<div class=cajagris>
  <div class="div100" style="text-align:left;">
  <input type="checkbox" id="AutomaticRecharge" name="AutomaticRecharge" value="<%= Convert.ToString((bool)ViewData["AutomaticRecharge"]).ToLower()%>" onClick="toggleautorecarga()"/>
&nbsp;
<%=Html.LabelFor(cust => cust.AutomaticRecharge)%> 

<div style="display:inline" id="divautorecharge"> 
<br />
<% if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) { %>
<%=Html.LabelFor(cust => cust.PaypalID)%> &nbsp;
<%= Html.TextBoxFor(cust => cust.PaypalID, new { @placeholder = Resources.CustomerInscriptionModel_AutomaticRechargeWritePaypalID, @class = "PaypalID" })%>
<br />
<% } %> 
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
<p>&nbsp;</p>
<div class="div65-right">

<% if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) { %>
<div align="center"><img src="https://www.paypal.com/<%=ViewData["Culture"]%>/i/btn/btn_dg_pay_w_paypal.gif" border="0" onclick="document.forms['FormRecharge'].submit();" style="cursor: pointer;" >
<% } else { %>
<input  type="submit" value="<%=Resources.Account_Recharge_BuyNow %>" class="botonverde" />
<% } %>
</div>
<% } %>
</div>
</div>
</div>
<div class="greenhr"><hr /></div> 
<script language="javascript">

<%if (!((bool)ViewData["AutomaticRecharge"])) { %>
    document.getElementById("divautorecharge").style.visibility = 'hidden';
    document.FormRecharge.AutomaticRecharge.checked=false;
    document.FormRecharge.AutomaticRecharge.value=false;
    if (document.FormRecharge.PaypalID != null) {
        document.FormRecharge.PaypalID.value = "";
    }
<% }else{ %>
    document.getElementById("divautorecharge").style.visibility = 'visible';
    document.FormRecharge.AutomaticRecharge.checked=true;
    document.FormRecharge.AutomaticRecharge.value=true;

<%} %>
    function toggleautorecarga() {
        if (document.FormRecharge.AutomaticRecharge.checked == true) {
            document.FormRecharge.AutomaticRecharge.value=true;
            document.getElementById("divautorecharge").style.visibility = 'visible';
        }
        else {
            document.FormRecharge.AutomaticRecharge.value=false;
            document.getElementById("divautorecharge").style.visibility = 'hidden';
            if (document.FormRecharge.PaypalID != null) {
                document.FormRecharge.PaypalID.value = "";
            }
        }

    }

    function SetServiceOrder(i) {
        document.getElementById("tdCreditBase").innerHTML = document.getElementById("hdnServiceBase" + i).value;
        document.getElementById("tdCreditFEE").innerHTML = document.getElementById("hdnServiceFEE" + i).value;
        document.getElementById("tdCreditVAT").innerHTML = document.getElementById("hdnServiceVAT" + i).value;
        document.getElementById("tdCreditTotal").innerHTML = document.getElementById("hdnServiceTotal" + i).value;
    }

    SetServiceOrder('<%= ViewData["RechargeDefaultValueIndex"].ToString() %>');

</script>

</asp:Content>
