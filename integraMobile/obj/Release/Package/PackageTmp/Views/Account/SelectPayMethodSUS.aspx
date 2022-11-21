<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectPayMethodModel>"%>
<%@ Import Namespace="integraMobile.Domain.Abstract"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.UserData_PayMeansTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.Account_Main_BttnUserData%> --%>
    <%-- <%=Resources.UserData_PayMeansTitle %> --%>
    <%=Resources.SelectPayMethod_IntroText1+" ("+ViewData["SuscriptionTypeString"]+")"%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div id="breadcrumb-wrapper" class="row">
    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Main", "Account") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="UserData" title="<%=Resources.Account_Main_BttnUserData%>"><%=Resources.Account_Main_BttnUserData%></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.UserData_PayMeansTitle %>
        </li>
    </ul>
</div>
--%>
<%-- 
<div class="title-alt">
    <h6><%=Resources.SelectPayMethod_IntroText1+" ("+ViewData["SuscriptionTypeString"]+")"%></h6>
</div>
 --%>
<%
using (Html.BeginForm("SelectPayMethodSUS", "Account", FormMethod.Post, new {@id = "FormSelectPayMethod", @name = "FormSelectPayMethod", @role = "form"}))
{
    %> 
    <div class="content-wrap">

        <%-- ROW ALERTS // --%>
        <div class="row">
            <div class="col-sm-12">
                <%
                    foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                    {
                        foreach (ModelError modelError in keyValuePair.Value.Errors) 
                        {
                            %>
                                <div class="alert alert-bky-danger">
                                    <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                    <span class="bky-cancel"></span> 
                                    &nbsp; 
                                    <%= Html.Encode(modelError.ErrorMessage) %>
                                </div>
                            <%
                        } // ModelError
                    } // KeyValuePair
                %>

            </div>
        </div><!--.row 1 / 1 col-->
        <%-- // ROW ALERTS  --%>
    
        <%-- ROW CONTENT // --%>
        <div class="row">
    
            <%-- COL LEFT // --%>
            <div class="col-md-6  col-block">
                <p class="lead"><%=Resources.SelectPayMethod_Sus_Option2 %></p>

                <div class="form-group">
                    <%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtDebitCreditCard, new {
                        @id = "CardType",
                        @onclick = "DisableOverWriteCreditCardCheck("+Convert.ToInt32(PaymentMeanType.pmtDebitCreditCard).ToString()+");" 
                    })%>
                    &nbsp;
                    <label for="CardType"><img src="<%= Url.Content("~/Content/img/credit/visa.png") %>" alt="Visa"> <img src="<%= Url.Content("~/Content/img/credit/mastercard.png") %>" alt="Mastercard"></label>
                    <p class="help-block"><%=Resources.SelectPayMethod_CreditCard%></p>
                </div><!--//.form-group-->
                <%
                    if (Convert.ToBoolean(ViewData["ShowCheckCreditCard"])) 
                    { 
                        %>
                        <div class="form-group">
                            <input name="OverWriteCreditCard" id="OverWriteCreditCard" type="checkbox" value="<%= (((bool)ViewData["OverWriteCreditCardValue"]) ? "true" : "false")%>" onclick="toggleOverWriteCreditCard()">
                            <label for="OverWriteCreditCard"><%=string.Format(Resources.SelectPayMethod_SubstituteCurrentCard,ViewData["CurrentPaymentType_PAN"])%></label>
                        </div><!--//.form-group-->
                        <% 
                    } // ShowCheckCreditCard
                %>
                <div class="form-group">
                    <%= Html.RadioButtonFor(cust => cust.PaymentMean, (int)PaymentMeanType.pmtPaypal, new {
                        @id = "PayPalType",
                        @onclick = "DisableOverWriteCreditCardCheck(" + Convert.ToInt32(PaymentMeanType.pmtPaypal).ToString() + ");",
                        @disabled = "disabled"
                    })%>
                    &nbsp;
                    <label for="PayPalType"><img src="<%= Url.Content("~/Content/img/credit/paypal2.png") %>" alt="Paypal"></label>
                    <p class="help-block"><%=Resources.SelectPayMethod_Paypal%></p>
                </div><!--//.form-group-->                                      
            </div><!--//.col-md-6-->

            <%-- COL RIGHT // --%>
            <div class="col-md-6  col-block">
                <%
                    if (Convert.ToInt32(ViewData["SuscriptionType"])==(int)PaymentSuscryptionType.pstPrepay) 
                    { 
                        %>
                        <p class="lead"><%=Resources.SelectPayMethod_IntroText3%></p>
                        <div class="form-group">
                            <p class="help-block"><%=Resources.SelectPayMethod_Sus_Option3 %></p>
                            <input name="AutomaticRecharge" id="AutomaticRecharge" type="checkbox" value="<%= Convert.ToString((bool)ViewData["AutomaticRecharge"]).ToLower()%>" onclick="toggleautorecarga()">
                            <%=Html.LabelFor(cust => cust.AutomaticRecharge)%>
                        </div>
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
                            </div>
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
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="alert alert-bky-info">
                                <span class="bky-info"></span>
                                <%=Resources.SelectPayMethod_Sus_Remark1 %>
                                <ul>
                                    <li><%=Resources.SelectPayMethod_Sus_Remark2 %></li>
                                    <li>
                                        <%=Resources.SelectPayMethod_Sus_Remark3 %>
                                        <ol>
                                            <li><%=Resources.SelectPayMethod_Sus_Remark4 %></li>
                                            <li><%=Resources.SelectPayMethod_Sus_Remark5 %></li>
                                        </ol>
                                    </li>
                                </ul>

                                <%=Resources.SelectPayMethod_Sus_Remark6 %>
                            </div>
                        </div>
                        <% 
                    } 
                    else 
                    { 
                        %>
                            <div id="ChargeMessage">
                                <p class="lead">&nbsp;</p>
                                <div class="form-group">
                                    <p class="help-block"><%=string.Format(Resources.SelectPayMethod_PerTransactionMessage,ViewData["ChargeValue"],ViewData["ChargeCurrency"])%></p>
                                    <input name="AcceptCharge" id="AcceptCharge" type="checkbox" value="<%= Convert.ToString((bool)ViewData["AcceptChargeValue"]).ToLower()%>" onclick="toggleacceptcharge()">
                                    <label for="AcceptCharge"><%=Resources.SelectPayMethod_PerTransactionCheckBoxMessage%></label>
                                </div>
                            </div>
                        <% 
                    } // SuscriptionType
                %>

                <div class="row-buttons">    
                    <button class="btn btn-bky-`primary" type="submit"><%=Resources.Button_Next %></button>
                    <%
                        if (string.IsNullOrEmpty(Session["SuscriptionTypeConf"].ToString())) 
                        {
                            %>
                                &nbsp;
                                <a href="SelectSuscriptionTypeINT" class="btn btn-bky-sec-default"><%=Resources.SelectPayMethod_ChangeSuscriptionType %></a>
                            <%
                        } // SuscriptionTypeConf
                    %>
                </div><!--//.row-buttons-->
                
            </div><!--//.col-md-6-->

        </div><!--row 2 / 2 cols -->
        <%-- // ROW CONTENT --%>

        <input name="bForceChange" id="bForceChange" type="hidden" value="<%=Convert.ToBoolean(ViewData["ForceChange"])%>">
    </div>
    <% 
} // Html.BeginForm
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