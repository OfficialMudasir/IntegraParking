<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RetailerCouponsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RetailerInvoice_Title%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script src="<%= Url.Content("~/Content/dropdown/js/jquery-1.3.2.min.js") %>" type="text/javascript"></script>

    <script  type="text/javascript">

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

            var couponAmount = $("#CouponAmount").val().replace(".", separator);

            $.ajax({
                type: 'POST',
                url: retailer_calculateTotalsUrl,
                data: { Coupons: $("#Coupons").val(), CouponAmount: couponAmount, NumDecimals: $("#NumDecimals").val() },
                success: function (data) {

                    try {
                        eval("data = " + data);

                        $("#TotalAmount").text(data.TotalAmountString);                        
                        $("#TotalServiceFEE").text(data.TotalServiceFEEString);
                        $("#TotalVat").text(data.TotalVatString);
                        $("#Total").val($("#hdnTotalLabel").val() + " " + data.TotalString);

                        if (data.TotalServiceFEE != 0) {
                            $("#lblTotalServiceFEE").show();
                            $("#TotalServiceFEE").show();
                        }
                        else {
                            $("#lblTotalServiceFEE").hide();
                            $("#TotalServiceFEE").hide();
                        }
                        if (data.TotalVat != 0) {
                            $("#lblTotalVat").show();
                            $("#TotalVat").show();
                        }
                        else {
                            $("#lblTotalVat").hide();
                            $("#TotalVat").hide();
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
                data: { RetailerId: $("#RetailerId").val() },
                success: function (data) {

                },
                error: function (xhr) {
                    alert("error");
                }
            });

        }

        function Print() {

            $(".header").hide();
            $("#divPrint").hide();
            window.print();
            $("#divPrint").show();
            $(".header").show();

        }

    </script>

    <div id="divPrint">
        <input type="button" onclick="Print();" value="<%=Resources.RetailerInvoice_Print%>" class="botonazulo" style="width:30%"/>
    </div>

    <div id="invoice">

        <div class="div100-top">
            <div class="div25-leftfac"> 
                <img src="<%= Url.Content("~/Content/img/Logo_Eysa.png") %>"/>
            </div>
            <div class="div40-leftfacCent">
                <p><n><%= Model.CompanyName %></n></p>
                <p><%= Model.CompanyInfo.Replace("\n","<br/>") %></p>
            </div>
            <div class="div25-Rightfac">
                <img src="<%= Url.Content("~/Content/img/Logo_certificado.png") %>">
            </div>
        </div>        

        <div class="div100-top">
            <div class="div50-leftfac">
                <p><b><%= Html.ValueFor(ret => ret.Name) %></b></p>
                <p><%= Html.ValueFor(ret => ret.Address) %></p>
                <p><%=Resources.RetailerInvoice_DocId%> <%= Html.ValueFor(ret => ret.DocId) %></p>
            </div>
            <div class="div40-rightfac">
                <div class="div25-left">
                    <p><%=Resources.RetailerInvoice_Date%></p>
                    <p><%=Resources.RetailerInvoice_InvoiceNum%></p>
                </div>
                <div class="div75-right">
                    <p><%= Html.ValueFor(ret => ret.PaymentDate) %></p>
                    <p><b><%= Html.ValueFor(ret => ret.InvoiceNum) %></b></p>
                </div>
            </div>
        </div>

        <h3><%=Resources.RetailerInvoice_BuyResume%></h3>

        <div class="div50-left">
            <input type="hidden" id="NumDecimals" name="NumDecimals" value="2" />
      	    <span style="color:#cddf37"><%=Resources.RetailerInvoice_Import%></span>
            <p>&nbsp;</p>
            <div class="div45-left">
                <p><%=Html.LabelFor(ret => ret.TotalAmount, new { id = "lblTotalAmount" })%></p>                
                <p><%=Html.LabelFor(ret => ret.TotalServiceFEE, new { id = "lblTotalServiceFEE" })%></p>
                <p><span id="lblTotalVat"><%=String.Format(Html.DisplayNameFor(ret => ret.TotalVat).ToHtmlString(), Model.VatString)%></span></p>
            </div>
            <div class="div10-right">
                <p><span id="TotalAmount"><%= Html.ValueFor(ret => ret.TotalAmountString) %></span></p>                
                <p><span id="TotalServiceFEE"><%= Html.ValueFor(ret => ret.TotalServiceFEEString) %></span></p>
                <p><span id="TotalVat"><%= Html.ValueFor(ret => ret.TotalVatString) %></span></p>
                <p>&nbsp;</p>
            </div>
            <input type="hidden" id="hdnTotalLabel" value="<%= Html.DisplayNameFor(ret => ret.Total) %>" />
            <input id="Total" type="button" disabled value="<%= Html.DisplayNameFor(ret => ret.Total) %> <%: Html.ValueFor(ret => ret.TotalString) %>" class="botonimporte" />
            <p>&nbsp;</p>
        </div>

        <div class="div50-right">
            <div class="cajazul">
                <%=Html.LabelFor(ret => ret.Coupons, new { style = "color:#0071b6;" })%>
                <%= Html.TextBoxFor(ret => ret.Coupons, new { disabled = "disabled", style = "width:100%;"}) %>
                <p>&nbsp;</p> 

                <%=Html.LabelFor(ret => ret.CouponAmount, new { style = "color:#0071b6;" })%>
                <%= Html.TextBoxFor(ret => ret.CouponAmount, new { disabled = "disabled", style = "width:100%;"}) %>    
            </div>
        </div>

        <div class="greenhr"><hr /></div>
        <p>&nbsp;</p>

        <%= Html.HiddenFor(ret => ret.RetailerId) %>        

        <%
            bool bAlign = true;
            foreach (string sCouponCode in Model.RechargeCoupons)
            {                                
                %>
                <div class="div100" style="<%:(!bAlign?"text-align:right; vertical-align:middle;":"") %>">
                    <div class="div15-<%: (bAlign?"left":"right") %>" style="text-align:center;">
                        <%=Resources.RetailerInvoice_CouponAvailable%>
                        <div style="border: 1px solid black; width: 40px; height: 40px; margin-left: 43px;"></div>
                    </div>
                    <div class="div85-<%: (bAlign?"right":"left") %>">
                        <div class="div15-<%: (bAlign?"left":"right") %>">
                            <img src="<%= Url.Content("~/Tmp/QR" + sCouponCode + ".png") %>"/>
                        </div>
                        <div class="div85-<%: (bAlign?"right":"left") %>" style="display:table; height:123px;">
                            <span style="display:table-cell; vertical-align:middle;">
                                <%=Resources.RetailerInvoice_CouponCode%>
                                <b><%= sCouponCode %></b>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="div100"> <img src="<%= Url.Content("~/Content/img/Recorta_" + (bAlign?"der":"izq") + ".png") %>"/></div>
                <%                
                bAlign = !bAlign;
            }
            
         %>
    </div>

</asp:Content>