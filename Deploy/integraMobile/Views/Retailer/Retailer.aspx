<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RetailerCouponsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RetailerCoupons_Title%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.RetailerCoupons_SubTitle2%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div class="row">
    <div id="paper-top">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.RetailerCoupons_SubTitle%>
                </span>
            </h2>

        </div>
    </div>
</div>
--%>
<%--
<div id="breadcrumb-wrapper" class="row">
    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Index", "Home") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.RetailerCoupons_Title%>
        </li>
    </ul>
</div>
--%>
<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">

            <%
                foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                {
                    foreach (ModelError modelError in keyValuePair.Value.Errors) 
                    {
                        %>
                            <div class="alert alert-bky-danger">
                                <button data-dismiss="alert" class="close" type="button">×</button>
                                <span class="bky-cancel"></span>
                                &nbsp;
                                <%= Html.Encode(modelError.ErrorMessage) %>
                            </div>
                        <%
                    } // foreach ModelError
                } // foreach ModelState
            %>

            <div class="alert alert-bky-info">
                <span class="bky-info"></span>
                &nbsp;
                <%=Resources.RetailerCoupons_Details%>
            </div>
        </div>
    </div>
<% 
    using (Html.BeginForm("Retailer", "Retailer", FormMethod.Post, new {@class="form-horizontal", @role="form"}))
    { 
        %>
            <div class="row">

                <%-- COL LEFT // --%>   
                <div class="col-lg-6 col-block">

                    <h3><%=Resources.RetailerCoupons_Data%></h3>

                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.Email, new { @class= "col-lg-4 control-label" })%>
                        <div class="col-lg-7">
                        <%= Html.TextBoxFor(ret => ret.Email, new {
                            @placeholder = Resources.RetailerCouponsModel_WriteYourEmail,
                            @required="required",
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_Email)+"');", 
                            @oninput="this.setCustomValidity('');",
                            @type="email",
                            @class="form-control" })
                        %>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.ConfirmEmail, new { @class= "col-lg-4 control-label" })%>
                        <div class="col-lg-7">
                        <%= Html.TextBoxFor(ret => ret.ConfirmEmail, new {
                            @placeholder = Resources.RetailerCouponsModel_WriteYourConfirmEmail,
                            @required="required",
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_ConfirmEmail)+"');", 
                            @oninput="this.setCustomValidity('');",
                            @type="email",
                            @class="form-control" })
                        %>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.Name, new { @class= "col-lg-4 control-label" })%>
                        <div class="col-lg-7">
                        <%= Html.TextBoxFor(ret => ret.Name, new {
                            @placeholder = Resources.RetailerCouponsModel_WriteYourName,
                            @required="required",
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_Name)+"');", 
                            @oninput="this.setCustomValidity('');",
                            @class="form-control" })
                        %>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.Address, new { @class= "col-lg-4 control-label" })%>
                        <div class="col-lg-7">
                        <%= Html.TextBoxFor(ret => ret.Address, new {
                            @placeholder = Resources.RetailerCouponsModel_WriteYourAddress,
                            @required="required", 
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_Address)+"');", 
                            @oninput="this.setCustomValidity('');",
                            @class="form-control" })
                        %>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.DocId, new { @class= "col-lg-4 control-label" })%>
                        <div class="col-lg-7">
                        <%= Html.TextBoxFor(ret => ret.DocId, new {
                            @placeholder = Resources.RetailerCouponsModel_WriteYourDocID,
                            @required="required", 
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_DocID)+"');", 
                            @oninput="this.setCustomValidity('');",
                            @class="form-control" })
                        %>
                        </div>
                    </div>
                </div><!--// col-left.col-block-->
                <%-- // COL LEFT --%>   

                <%-- COL RIGHT // --%>   
                <div class="col-lg-6 col-block">

                    <h3><%=Resources.RetailerCoupons_BuyResume%></h3>

                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.Coupons, new { @class= "col-lg-5 control-label" }) %>
                        <div class="col-lg-6">
                            <select name="Coupons" id="Coupons" class="form-control" onchange="CalculateTotals();">
                                <option value="0">Selecciona</option>
                                <option value="25">25</option>
                                <option value="50">50</option>
                                <option value="100">100</option>                
                                <option value="200">200</option>
                                <option value="500">500</option>
                                <option value="1000">1000</option>
                                <option value="5000">5000</option>
                            </select>
                        </div>
                    </div>

                    <div class="form-group">
                        <%=Html.LabelFor(ret => ret.CouponAmount, new { @class= "col-lg-5 control-label" }) %>
                        <div class="col-lg-6">
                            <select name="CouponAmount" id="CouponAmount" class="form-control" onchange="CalculateTotals();">
                                <option value="0" selected="">Selecciona</option>
                                <option value="0.01">0.01 MXN</option>
                                <option value="1">1 MXN</option>
                                <option value="2">2 MXN</option>
                                <option value="3">3 MXN</option>
                                <option value="4">4 MXN</option>
                                <option value="5">5 MXN</option>
                                <option value="10">10 MXN</option>
                                <option value="20">20 MXN</option>
                                <option value="20.40">20.40 MXN</option>
                                <option value="25">25 MXN</option>
                                <option value="40.80">40.80 MXN</option>
                                <option value="50">50 MXN</option>
                                <option value="75">75 MXN</option>
                                <option value="100">100 MXN</option>
                                <option value="200">200 MXN</option>
                                <option value="250">250 MXN</option>
                                <option value="500">500 MXN</option>
                                <option value="750">750 MXN</option>
                                <option value="1000">1000 MXN</option>
                            </select>
                        </div>
                    </div>

                    <div class="table-responsive abonos-table">
                        <table class="table">
                            <tr id="contTotalAmount">
                                <th><%= Html.DisplayNameFor(ret => ret.TotalAmount) %></th>
                                <td id="TotalAmount"><%= Html.ValueFor(ret => ret.TotalAmountString) %></td>
                            </tr>
                            <tr id="contTotalServiceFEE">
                                <th><%= Html.DisplayNameFor(ret => ret.TotalServiceFEE) %></th>
                                <td id="TotalServiceFEE"><%= Html.ValueFor(ret => ret.TotalServiceFEEString) %></td>
                            </tr>
                            <tr id="contTotalVat">
                                <th><%=String.Format(Html.DisplayNameFor(ret => ret.TotalVat).ToHtmlString(), Model.VatString)%></th>
                                <td id="TotalVat"><%= Html.ValueFor(ret => ret.TotalVatString) %></td>
                            </tr>
                            <tr id="contTotal">
                                <th><%= Html.DisplayNameFor(ret => ret.Total) %></th>
                                <td id="Total"><%: Html.ValueFor(ret => ret.TotalString) %></td>
                            </tr>
                        </table>
                    </div>

                    <div class="row-buttons">
                        <button class="btn btn-bky-primary" type="submit"><%: Resources.RetailerCoupons_BuyButton %></button>
                    </div>

                </div><!--//.col-right.col-block-->
                <%-- // COL RIGHT --%>   

            </div>
        <% 
    } 
%>
</div>

<script type="text/javascript">

    $(document).ready(function (e) {
        try {
            $("body select").msDropDown();
        } catch (e) {
            //alert(e.message);
        }
        CalculateTotals();
    });

    <%
        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;            
    %>

    var retailer_calculateTotalsUrl = '<%= Url.Action("CalculateTotals") %>';

    function CalculateTotals() {

        var separator = "<%=culture.NumberFormat.CurrencyDecimalSeparator%>";

        var couponAmount = $("#CouponAmount").val().replace(".", separator);

        $.ajax({
            type: 'POST',
            url: retailer_calculateTotalsUrl,
            data: { Coupons: $("#Coupons").val(), CouponAmount: couponAmount, NumDecimals: "2" },
            dataType: "html",
            success: function (data) {
                
                try {
                    eval("data = " + data);

                    $("#TotalAmount").text(data.TotalAmountString);
                    $("#TotalServiceFEE").text(data.TotalServiceFEEString);
                    $("#TotalVat").text(data.TotalVatString);
                    $("#Total").text(data.TotalString);

                    if (data.TotalServiceFEE != 0) {
                        $("#contTotalServiceFEE").show();
                    }
                    else {
                        $("#contTotalServiceFEE").hide();
                    }
                    if (data.TotalVat != 0) {
                        $("#contTotalVat").show();
                    }
                    else {
                        $("#contTotalVat").hide();
                    }

                } catch (ex) {
                    alert("error");
                }
            },
            error: function (xhr) {
                alert("error");
            }
        });

    }

</script>

</asp:Content>