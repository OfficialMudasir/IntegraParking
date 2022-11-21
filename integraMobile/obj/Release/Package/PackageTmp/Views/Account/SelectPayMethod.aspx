<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectPayMethodModel>"%>
<%@ Import Namespace="integraMobile.Domain.Abstract"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Resources.SelectPayMethod_IntroText1 %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
    <%=Resources.SelectPayMethod_IntroText1%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%-- 
<div class="row">
    <div id="paper-top">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.ServiceName %>
                </span>
            </h2>

        </div>
    </div>
</div>
 --%>
<%-- 
<div class="title-alt">
    <h6><%=Resources.SelectPayMethod_IntroText1+" ("+ViewData["SuscriptionTypeString"]+")"%></h6>
</div>
 --%>
<%
    using (Html.BeginForm("SelectPayMethod", "Account", FormMethod.Post, new { @id="FormSelectPayMethod", @name="FormSelectPayMethod", @role="form" }))
    {
        %>  
            <div class="content-wrap">
                <%-- ROW ALERTS // --%>
                <div class="row">
                    <div class="col-md-12">
                        <%
                            foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                            {
                                foreach (ModelError modelError in keyValuePair.Value.Errors) 
                                {   
                                    %>
                                    <div class="alert alert-bky-danger">
                                        <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                        <span class="bky-cancel"></span> &nbsp; <%= Html.Encode(modelError.ErrorMessage) %>
                                    </div>
                                    <hr class="separator-block-line">
                                    <%
                                } //ModelError
                            } //KeyValuePair
                            

                        %>
                    </div><!-- // col-md-12 -->
                </div><!--// .row / 1 col -->
                <%--// ROW ALERTS --%>

                <%-- ROW CONTENT //--%>
                <div class="row">
                
                    <%-- COL LEFT //--%>
                    <div class="col-md-6 col-block">
                        <p class="lead"><%=Resources.SelectPayMethod_IntroText2%></p>

                        <div class="form-group">
                            <%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtDebitCreditCard, new {
                                @id = "CardType",
                                @onclick = "DisableOverWriteCreditCardCheck("+ Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard).ToString()+");" 
                                })%>
                            <label for="CardType"><img src="<%= Url.Content("~/Content/img/credit/visa.png") %>" alt="Visa"> <img src="<%= Url.Content("~/Content/img/credit/mastercard.png") %>" alt="Mastercard"></label>
                            <p class="help-block"><%=Resources.SelectPayMethod_CreditCard%></p>
                        </div><!--// form-group --> 
                        <%
                            if (Convert.ToBoolean(ViewData["ShowCheckCreditCard"]))
                            { %>
                                <div class="form-group">
                                    <input type="checkbox" id="OverWriteCreditCard" name="OverWriteCreditCard" value="<%=(((bool)ViewData["OverWriteCreditCardValue"]) ? "true" : "false")%>" onclick="toggleOverWriteCreditCard()"> &nbsp;
                                    <label for="OverWriteCreditCard"><%=string.Format(Resources.SelectPayMethod_SubstituteCurrentCard,ViewData["CurrentPaymentType_PAN"])%></label>
                                </div>
                                <% 
                            } // if ShowCheckCreditCard
                        %>
                        <div class="form-group">
                            <%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtPaypal, new {
                                    @id = "PayPalType",
                                    @onclick = "DisableOverWriteCreditCardCheck(" + Convert.ToInt32(PaymentMeanType.pmtPaypal).ToString() + ");",
                                    @disabled = "disabled"
                                })%> &nbsp; 
                            <label for="PayPalType"><img src="<%= Url.Content("~/Content/img/credit/paypal2.png") %>" alt="PayPal"></label>
                            <p class="help-block"><%=Resources.SelectPayMethod_Paypal%></p>
                        </div><!--// form-group -->                                            
                    </div><!--// .col-md-6 -->

                    <%-- COL RIGHT //--%>
                    <div class="col-md-6 col-block">
                        <%
                            if (Convert.ToInt32(ViewData["SuscriptionType"])==(int)PaymentSuscryptionType.pstPrepay)
                            { 
                                %>
                                <p class="lead"><%=Resources.SelectPayMethod_IntroText3%></p>
                                <div class="form-group">
                                    <p><%=Resources.SelectPayMethod_IntroText4%></p>
                                    <input type="checkbox" id="AutomaticRecharge" name="AutomaticRecharge" value="<%= Convert.ToString((bool)ViewData["AutomaticRecharge"]).ToLower()%>" onclick="toggleautorecarga()">
                                    <%=Html.LabelFor(cust => cust.AutomaticRecharge)%>
                                </div><!--// .form-group-->
                                <div id="AutoRecharge">
                                    <div class="form-group">
                                        <%=Html.LabelFor(cust => cust.AutomaticRechargeQuantity)%>
                                        <select name="AutomaticRechargeQuantity" id="AutomaticRechargeQuantity" class="form-control">
                                            <option value="1000" <%= (1000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>10 <%= ViewData["CurrencyISOCode"]%></option>
                                            <option value="2000" <%= (2000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>20 <%= ViewData["CurrencyISOCode"]%></option>
                                            <option value="3000" <%= (3000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>30 <%= ViewData["CurrencyISOCode"]%></option>
                                            <option value="4000" <%= (4000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>40 <%= ViewData["CurrencyISOCode"]%></option>
                                            <option value="5000" <%= (5000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>50 <%= ViewData["CurrencyISOCode"]%></option>
                                            <option value="10000" <%= (10000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>100 <%= ViewData["CurrencyISOCode"]%></option>
                                            <option value="20000" <%= (20000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>200 <%= ViewData["CurrencyISOCode"]%></option>
                                        </select>
                                    </div><!--// .form-group-->
                                    <div class="form-group">
                                        <%=Html.LabelFor(cust => cust.AutomaticRechargeWhenBelowQuantity)%>
                                        <select name="AutomaticRechargeWhenBelowQuantity" id="AutomaticRechargeWhenBelowQuantity" class="form-control">
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
                                    </div><!--// .form-group-->
                                </div><!--// AutoRecharge-->
                                <%
                            } else { 
                                %>
                                <div id="ChargeMessage">
                                    <p class="lead">&nbsp;</p>
                                    <div class="form-group">
                                        <p class="help-block"><%=string.Format(Resources.SelectPayMethod_PerTransactionMessage,ViewData["ChargeValue"],ViewData["ChargeCurrency"])%></p>
                                        <input type="checkbox" id="AcceptCharge" name="AcceptCharge" value="<%= Convert.ToString((bool)ViewData["AcceptChargeValue"]).ToLower()%>" onclick="toggleacceptcharge()">
                                        <label for="AcceptCharge"><%=Resources.SelectPayMethod_PerTransactionCheckBoxMessage%></label>
                                    </div>
                                </div>
                                <% 
                            } // else SuscriptionType
                        %>

                        <%-- ROW BUTTONS--%>
                        <div class="row-buttons">
                            <input type="hidden" name="bForceChange" id="bForceChange" value="<%=Convert.ToBoolean(ViewData["ForceChange"])%>">
                            <button class="btn btn-bky-primary" type="submit">Next</button>
                            <%
                                if (string.IsNullOrEmpty(ViewData["SuscriptionTypeConf"].ToString())) 
                                {
                                    %>
                                        &nbsp;
                                        <a href="SelectSuscriptionType" class="btn btn-bky-sec-default"><%=Resources.SelectPayMethod_ChangeSuscriptionType %></a>
                                    <% 
                                } // if SuscriptionTypeConf
                            %>
                        </div><!--//.row-buttons-->

                    </div><!--// .col-md-6 -->
                
                </div><!--// .row / 2 col -->
                <%--// ROW CONTENT --%>

            </div><!--//.content-wrap-->
        <% 
    } //Html.BeginForm 
%>
<script type="text/javascript">


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
        document.getElementById("AutoRecharge").style.display = 'none';
        document.FormSelectPayMethod.AutomaticRecharge.checked = false;
        document.FormSelectPayMethod.AutomaticRecharge.value = false;
    <% }
        else
        { %>
        document.getElementById("AutoRecharge").style.display = 'block';
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

        document.getElementById("ChargeMessage").style.display = 'block';

    <%}%>

    function toggleautorecarga() {
        if (document.FormSelectPayMethod.AutomaticRecharge.checked == true) {
            document.FormSelectPayMethod.AutomaticRecharge.value=true;
            document.getElementById("AutoRecharge").style.display = 'block';
        }
        else {
            document.FormSelectPayMethod.AutomaticRecharge.value=false;
            document.getElementById("AutoRecharge").style.display = 'none';
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

            if (document.getElementById("ChargeMessage"))
                document.getElementById("ChargeMessage").style.display = 'block';
            document.FormSelectPayMethod.OverWriteCreditCard.value = true;
        }
        else {
            if (document.getElementById("ChargeMessage"))
                document.getElementById("ChargeMessage").style.display = 'none';
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

</asp:Content>