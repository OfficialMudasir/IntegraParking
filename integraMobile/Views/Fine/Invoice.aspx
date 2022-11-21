<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RetailerCouponsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RetailerInvoice_Title%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="row no-print">
    <div id="paper-top">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.RetailerCoupons_SubTitle%>
                </span>
            </h2>

        </div>
    </div>
</div>

<ul id="breadcrumb" class="no-print">
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

<div class="content-wrap">
    <div class="page-extra">

        <section class="content invoice">
            <div class="row invoice-header">
                    <div class="col-xs-6">     
                        <img class="invoice-logo" src="<%= Url.Content("~/Content/img/logo_colored.svg") %>" alt="Integra - Parking Solutions">
                    </div>
                    <div class="col-md-6">
                        <img class="invoice-certified pull-right" src="<%= Url.Content("~/Content/img/certified.svg") %>" width="165" height="93" alt="">
                    </div>
            </div>

            <div class="row invoice-info">
                <div class="col-xs-4 invoice-col">
                    From
                    <br>
                    <strong><%= Model.CompanyName %></strong>
                    <br><%= Model.CompanyInfo.Replace("\n","<br>") %>
                </div>
                <div class="col-xs-4 invoice-col">
                    To
                    <br>
                    <strong><%= Html.ValueFor(ret => ret.Name) %></strong>
                    <br><%= Html.ValueFor(ret => ret.Address) %>
                    <br><%=Resources.RetailerInvoice_DocId%> <%= Html.ValueFor(ret => ret.DocId) %>
                </div>
                <div class="col-xs-4 invoice-col">
                    <%=Resources.RetailerInvoice_Date%> <%= Html.ValueFor(ret => ret.PaymentDate) %>
                    <br/>
                    <%=Resources.RetailerInvoice_InvoiceNum%> <strong><%= Html.ValueFor(ret => ret.InvoiceNum) %></strong>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12">
                    <p class="lead"><%=Resources.RetailerInvoice_BuyResume%></p>
                    <div class="table-responsive">
                        <table class="table">
                            <tr>
                                <th style="width:50%"><%= Html.DisplayNameFor(ret => ret.Coupons) %></th>
                                <td id="Coupons"><%= Html.ValueFor(ret => ret.Coupons) %></td>
                            </tr>
                            <tr>
                                <th><%= Html.DisplayNameFor(ret => ret.CouponAmount) %></th>
                                <td id="CouponAmount"><%= Html.ValueFor(ret => ret.CouponAmount) %></td>
                            </tr>
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
                </div>
            </div>

            <div class="row no-print">
                <div class="col-xs-12">
                    <button class="btn btn-primary" onclick="window.print();"><i class="fa fa-print"></i> <%=Resources.RetailerInvoice_Print%></button>
                </div>
            </div>
        </section>

    </div>
</div>

<div class="content-wrap park-coupons">
    <div class="row">
        <% foreach (string sCouponCode in Model.RechargeCoupons)
        {                                
        %>
        <div class="col-xs-6">
            <div class="park-coupon">
                <div class="park-coupon-check">
                    <%=Resources.RetailerInvoice_CouponAvailable%>
                    <div class="park-coupon-checkbox"></div>
                </div>  
                <div class="park-coupone-content">
                    <img class="park-coupon-img" src="<%= Url.Content("~/Tmp/QR" + sCouponCode + ".png") %>" alt="">
                    <div class="park-coupon-value"><%=Resources.RetailerInvoice_CouponCode%> <strong><%= sCouponCode %></strong></div>
                </div>
                <span class="fontawesome-cut"></span> 
            </div>
        </div>
        <%                
        }  
        %>
    </div>
</div>

<script type="text/javascript">

    <%
        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;            
    %>

    $(document).ready(function (e) {
        CalculateTotals();
        setTimeout('DeleteQRTmpFiles()', 5000);
    });

    var retailer_calculateTotalsUrl = '<%= Url.Action("CalculateTotals") %>';
    var retailer_deleteQRTmpFilesUrl = '<%= Url.Action("DeleteQRTmpFiles") %>';

    function CalculateTotals() {

        var separator = "<%=culture.NumberFormat.CurrencyDecimalSeparator%>";

        var couponAmount = $("#CouponAmount").text().replace(".", separator);

        $.ajax({
            type: 'POST',
            url: retailer_calculateTotalsUrl,
            data: { Coupons: $("#Coupons").text(), CouponAmount: couponAmount, NumDecimals: "2" },
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

    function DeleteQRTmpFiles() {

        $.ajax({
            type: 'POST',
            url: retailer_deleteQRTmpFilesUrl,
            data: { RetailerId: "<%= Html.ValueFor(ret => ret.RetailerId) %>" },
            success: function (data) {

            },
            error: function (xhr) {
                alert("error");
            }
        });

    }

</script>

</asp:Content>