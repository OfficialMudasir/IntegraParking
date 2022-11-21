<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RechargeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Recharge_BuyCredit %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
    <%=Resources.Account_Recharge_BuyCredit%>
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
    <h6><%=Resources.Account_Recharge_BuyCredit %> - <%= Resources.Account_RechargeValue_Label %></h6>
</div>
--%>
<% 
    using (Html.BeginForm("Recharge", "Account", FormMethod.Post, new { @id = "FormRecharge", @name = "FormRecharge", @role = "form" }))
    { 
        %>
            <div class="content-wrap">
                <%-- ALERTS CONTENT //  --%>
                <div class="col-md-12">
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
                        } // ModelState
                    %>

                    <% 
                        if(Convert.ToDouble(ViewData["UserBalance"])<=0)
                        {
                            %>
                            <div class="alert alert-bky-danger">
                                <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                <span class="bky-cancel"></span> 
                                &nbsp; 
                                <%=Resources.Account_RechargeMessage_1%> <%=Resources.Account_RechargeMessage_2%>
                            </div>
                            <% 
                        } 
                        else if (Convert.ToBoolean(Session["OVERWRITE_CARD"])) 
                        { 
                            %>
                            <div class="alert alert-bky-danger">
                                <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                <span class="bky-cancel"></span> 
                                &nbsp; 
                                <%=Resources.Account_RechargeMessage_3%>
                            </div>
                            <% 
                        } // else if UserBalance
                    %>            
                </div>

                <div class="row">
                    <div class="col-md-6 col-block">
                        <!-- Account_RechargeValue -->
                        <h3><%= Resources.Account_RechargeValue_Label %></h3>
                        <div class="form-group">
                            <select id="RechargeQuantity" name="RechargeQuantity" class="form-control">
                                <%  
                                    var oRechargeValues = ViewData["RechargeValues"] as List<string>;
                                    var oRechargeValuesBase = ViewData["RechargeValuesBase"] as List<string>;
                                    var oRechargeValuesFEE = ViewData["RechargeValuesFEE"] as List<string>;
                                    var oRechargeValuesVAT = ViewData["RechargeValuesVAT"] as List<string>;
                                    for(int i=0; i<(ViewData["RechargeValues"] as List<string>).Count; i++) 
                                    {
                                        %>
                                        <option 
                                            data-service-base="<%= string.Format(Resources.Account_RechargeValue, oRechargeValuesBase[i], ViewData["CurrencyISOCode"]) %>" 
                                            data-service-fee="<%= string.Format(Resources.Account_RechargeValue, oRechargeValuesFEE[i], ViewData["CurrencyISOCode"]) %>"
                                            data-service-vat="<%= string.Format(Resources.Account_RechargeValue, oRechargeValuesVAT[i], ViewData["CurrencyISOCode"]) %>"
                                            data-service-total="<%= string.Format(Resources.Account_RechargeValue, oRechargeValues[i], ViewData["CurrencyISOCode"]) %>"
                                            value="<%= oRechargeValuesBase[i] %>" 
                                            <%
                                                if (i == Convert.ToInt32(ViewData["RechargeDefaultValueIndex"])) 
                                                { 
                                                    %> selected="selected" <%
                                                } 
                                            %> >
                                                <%= string.Format(Resources.Account_RechargeValue, oRechargeValuesBase[i], ViewData["CurrencyISOCode"])%> 
                                        </option>
                                        <% 
                                    } // for  RechargeValues
                                %>
                            </select>
                            <p class="help-block"><%=Resources.Account_Recharge_SelectAnAmount%></p>
                        </div>                
                        <!-- SelectPayMethod -->
                        <h3><%=Resources.SelectPayMethod_IntroText3%></h3>
                        <div class="form-group">
                            <p><%=Resources.SelectPayMethod_IntroText4%></p>
                            <input id="AutomaticRecharge" name="AutomaticRecharge" type="checkbox" value="<%= Convert.ToString((bool)ViewData["AutomaticRecharge"]).ToLower()%>" onclick="toggleautorecarga()">
                            <%=Html.LabelFor(cust => cust.AutomaticRecharge)%>
                        </div><!--//form-group -->
                        <div id="AutoRecharge">
                            <% 
                                if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) 
                                { 
                                    %>
                                    <div class="form-group">
                                        <%=Html.LabelFor(cust => cust.PaypalID)%>
                                        <%= Html.TextBoxFor(cust => cust.PaypalID, new {
                                            @placeholder = Resources.CustomerInscriptionModel_AutomaticRechargeWritePaypalID, 
                                            @class = "form-control" 
                                        })%>
                                    </div>
                                    <% 
                                } 
                            %>
                            
                            <div class="form-group">
                                <%=Html.LabelFor(cust => cust.AutomaticRechargeQuantity)%>
                                <select id="AutomaticRechargeQuantity" name="AutomaticRechargeQuantity" class="form-control">
                                    <option value="1000" <%= (1000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>10 <%= ViewData["CurrencyISOCode"]%></option>
                                    <option value="2000" <%= (2000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>20 <%= ViewData["CurrencyISOCode"]%></option>
                                    <option value="3000" <%= (3000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>30 <%= ViewData["CurrencyISOCode"]%></option>
                                    <option value="4000" <%= (4000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>40 <%= ViewData["CurrencyISOCode"]%></option>
                                    <option value="5000" <%= (5000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>50 <%= ViewData["CurrencyISOCode"]%></option>
                                    <option value="10000" <%= (10000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>100 <%= ViewData["CurrencyISOCode"]%></option>
                                    <option value="20000" <%= (20000==Convert.ToInt32(ViewData["SelectedQuantity"]))?"selected":"" %>>200 <%= ViewData["CurrencyISOCode"]%></option>
                                </select>
                            </div><!--//.form-group -->
                            <div class="form-group">
                                <%=Html.LabelFor(cust => cust.AutomaticRechargeWhenBelowQuantity)%>
                                <select id="AutomaticRechargeWhenBelowQuantity" name="AutomaticRechargeWhenBelowQuantity" class="form-control">
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
                            </div><!--//.form-group -->
                        </div><!--// #AutoRecharge -->

                    </div><!--// .col-md-6.col-block -->
                    <div class="col-md-6 col-block">
                                        
                        <% 
                            if (Model.PercVAT1 != 0 || Model.PercFEE != 0 || Model.FixedFEE != 0) 
                            { 
                                %>

                                <h3><%= Resources.Account_RechargeValueOrder_Label %></h3>

                                <table class="table large-only">
                                    <tbody>
                                        <tr>
                                            <th><%= Resources.Account_RechargeValueBase_Label %></th>
                                            <td id="ServiceBase"></td>                                                 
                                        </tr>
                                        <tr>
                                            <th><%= Resources.Account_RechargeValueFEE_Label %></th>
                                            <td id="ServiceFEE"></td>                                                 
                                        </tr>
                                        <tr>
                                            <th><%= string.Format(Resources.Account_RechargeValueVAT_Label, Model.PercVAT2*100) %></th>
                                            <td id="ServiceVAT"></td>                                                  
                                        </tr>
                                        <tr>
                                            <th><%= Resources.Account_RechargeValueTotal_Label %></th>
                                            <td id="ServiceTotal"></td>                                                
                                        </tr>
                                    </tbody>
                                </table>
                                <% 
                            } 
                        %>
                        <p class="row-buttons">
                            <% 
                                if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) 
                                { 
                                    %>
                                    <button class="btn btn-bky-warning" type="submit"><img src="https://www.paypal.com/<%=ViewData["Culture"]%>/i/btn/btn_dg_pay_w_paypal.gif" alt=""></button>
                                    <% 
                                } 
                                else 
                                { 
                                    %>
                                    <button class="btn btn-bky-primary" type="submit"><%=Resources.Account_Recharge_BuyNow %></button>
                                    <% 
                                } // if else PaymentType
                            %>
                        </p>
                        <%--
                            <p class="lead"><%=Resources.Account_Recharge_Billing_Address%></p>

                            <p>
                            <% if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) { %>
                                <img src="<%= Url.Content("~/Content/img/credit/paypal2.png") %>" alt="<%=Resources.SelectPayMethod_Paypal %>">
                            <% } else { %>
                                <img src="<%= Url.Content("~/Content/img/credit/visa.png") %>" alt="<%=Resources.SelectPayMethod_CreditCard %>">
                                <img src="<%= Url.Content("~/Content/img/credit/mastercard.png") %>" alt="<%=Resources.SelectPayMethod_CreditCard %>">
                            <% } %> 
                            </p>

                            <address>
                                <%=ViewData["Name"]%>
                                <br><%=ViewData["Address"]%>
                                <br><%=ViewData["City"]%>
                                <br><%=ViewData["State"]%>
                                <br><%=ViewData["ZipCode"]%>
                                <br><%=ViewData["Country"]%>
                            </address>
                        --%>

                    </div><!--// .col-md-6.col-block -->
                </div><!--// .row -->
            </div>
            <input type="hidden" name="PercVAT1" value="<%= Model.PercVAT1 %>">
            <input type="hidden" name="PercVAT2" value="<%= Model.PercVAT2 %>">
            <input type="hidden" name="PercFEE" value="<%= Model.PercFEE %>">
            <input type="hidden" name="PercFEETopped" value="<%= Model.PercFEETopped %>">
            <input type="hidden" name="FixedFEE" value="<%= Model.FixedFEE %>">
        <% 
    } // Html.BeginForm
%>

<script type="text/javascript">

    <%if (!((bool)ViewData["AutomaticRecharge"])) { %>
        document.getElementById("AutoRecharge").style.display = 'none';
        document.FormRecharge.AutomaticRecharge.checked=false;
        document.FormRecharge.AutomaticRecharge.value=false;
        if (document.FormRecharge.PaypalID != null) {
            document.FormRecharge.PaypalID.value = "";
        }
    <% }else{ %>
        document.getElementById("AutoRecharge").style.display = 'block';
        document.FormRecharge.AutomaticRecharge.checked=true;
        document.FormRecharge.AutomaticRecharge.value=true;

    <%} %>
        function toggleautorecarga() {
            if (document.FormRecharge.AutomaticRecharge.checked == true) {
                document.FormRecharge.AutomaticRecharge.value=true;
                document.getElementById("AutoRecharge").style.display = 'block';
            }
            else {
                document.FormRecharge.AutomaticRecharge.value=false;
                document.getElementById("AutoRecharge").style.display = 'none';
                if (document.FormRecharge.PaypalID != null) {
                    document.FormRecharge.PaypalID.value = "";
                }
            }

        }

    <% if (Model.PercVAT1 != 0 || Model.PercFEE != 0 || Model.FixedFEE != 0) { %>

    $(document).ready(function() {

        var CurrentServiceBase = $('select#RechargeQuantity option:selected').data("service-base"),
            CurrentServiceFEE = $('select#RechargeQuantity option:selected').data("service-fee"),
            CurrentServiceVAT = $('select#RechargeQuantity option:selected').data("service-vat"),
            CurrentServiceTotal = $('select#RechargeQuantity option:selected').data("service-total");

        $('td#ServiceBase').text(CurrentServiceBase);
        $('td#ServiceFEE').text(CurrentServiceFEE);
        $('td#ServiceVAT').text(CurrentServiceVAT);
        $('td#ServiceTotal').text(CurrentServiceTotal);

        $('select#RechargeQuantity').on('change',function(){
            var UpdatedServiceBase = $('select#RechargeQuantity option:selected').data("service-base"),
                UpdatedServiceFEE = $('select#RechargeQuantity option:selected').data("service-fee"),
                UpdatedServiceVAT = $('select#RechargeQuantity option:selected').data("service-vat"),
                UpdatedServiceTotal = $('select#RechargeQuantity option:selected').data("service-total");
            $('td#ServiceBase').text(UpdatedServiceBase);
            $('td#ServiceFEE').text(UpdatedServiceFEE);
            $('td#ServiceVAT').text(UpdatedServiceVAT);
            $('td#ServiceTotal').text(UpdatedServiceTotal);
        });
    });

    <% } %>

</script>

</asp:Content>