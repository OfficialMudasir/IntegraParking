<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RetailerCouponsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RetailerCoupons_Title%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%= Url.Content("~/Content/dropdown/js/jquery-1.3.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Content/dropdown/js/jquery.dd.js") %>" type="text/javascript"></script>
    <link href="<%= Url.Content("~/Content/dropdown/dd2.css") %>" rel="stylesheet" type="text/css" />

    <script  type="text/javascript">

        <%
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;            
        %>

        $(document).ready(function (e) {
            try {
                $("body select").msDropDown();
            } catch (e) {
                //alert(e.message);
            }

            CalculateTotals();
        });

        var retailer_calculateTotalsUrl = '<%= Url.Action("CalculateTotals") %>';

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

    </script>

    <div id="formulario">
        <h1><%=Resources.RetailerCoupons_SubTitle%></h1>
        <p>&nbsp;</p>
        <h3><%=Resources.RetailerCoupons_SubTitle2%></h3>
        <b>
            <p><%=Resources.RetailerCoupons_Details%></p>
            <p>&nbsp;</p>
        </b>
    </div>

    <% using (Html.BeginForm("Retailer", "Retailer", FormMethod.Post))
       { %>   

     <div class="error">
        <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
        <p><%= Html.ValidationMessageFor(retailer => retailer.Name)%></p>
        <p><%= Html.ValidationMessageFor(retailer => retailer.Email)%></p>
        <p><%= Html.ValidationMessageFor(retailer => retailer.ConfirmEmail)%></p>
        <p><%= Html.ValidationMessageFor(retailer => retailer.Address)%></p>
        <p><%= Html.ValidationMessageFor(retailer => retailer.DocId)%></p>
        <p><%= Html.ValidationMessageFor(retailer => retailer.Coupons)%></p>
        <p><%= Html.ValidationMessageFor(retailer => retailer.CouponAmount)%></p>
        <p><%= Html.ValidationMessage("customersDomainError")%></p>
    </div>  

	<div class="cajazul">
		<div class="div50-left">			
		    <span style="color:#0071b6"><%=Resources.RetailerCoupons_Data%></span>
		    <p>&nbsp;</p>

            <p><%=Html.LabelFor(ret => ret.Name)%></p>
            <%= Html.TextBoxFor(ret => ret.Name, new { @placeholder = Resources.RetailerCouponsModel_WriteYourName, @class = "inputelementsw100",
                    @required="true", 
                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_Name)+"');", 
                    @oninput="this.setCustomValidity('');" })%>

            <p><%=Html.LabelFor(ret => ret.Address)%></p>
            <%= Html.TextBoxFor(ret => ret.Address, new { @placeholder = Resources.RetailerCouponsModel_WriteYourAddress, @class = "inputelementsw100",
                    @required="true", 
                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_Address)+"');", 
                    @oninput="this.setCustomValidity('');" })%>

            <p><%=Html.LabelFor(ret => ret.DocId)%></p>
            <%= Html.TextBoxFor(ret => ret.DocId, new { @placeholder = Resources.RetailerCouponsModel_WriteYourDocID, @class = "inputelementsw100",
                    @required="true", 
                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_DocID)+"');", 
                    @oninput="this.setCustomValidity('');" })%>

        </div>  
        <div class="div50-right">   
    	    <p>&nbsp;</p>
    	    <p>&nbsp;</p>
            <p><%=Html.LabelFor(ret => ret.Email)%></p>
            <%= Html.TextBoxFor(ret => ret.Email, new { @placeholder = Resources.RetailerCouponsModel_WriteYourEmail,
                    @required="true", 
                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_Email)+"');", 
                    @oninput="this.setCustomValidity('');" })%>

            <p><%=Html.LabelFor(ret => ret.ConfirmEmail)%></p>
            <%= Html.TextBoxFor(ret => ret.ConfirmEmail, new { @placeholder = Resources.RetailerCouponsModel_WriteYourConfirmEmail,
                    @required="true", 
                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.RetailerCouponsModel_ConfirmEmail)+"');", 
                    @oninput="this.setCustomValidity('');" })%>

        </div>
    </div>

    <p>&nbsp;</p>
    <p>&nbsp;</p>
    <p>&nbsp;</p>

    <h3><%=Resources.RetailerCoupons_BuyResume%></h3>

    <div class="div50-left">
        <input type="hidden" id="NumDecimals" name="NumDecimals" value="2" />
      	<span style="color:#cddf37"><%=Resources.RetailerCoupons_Import%></span>
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
            <span style="color:#0071b6"><%=Html.DisplayNameFor(ret => ret.Coupons) %></span>
            <select id="Coupons" class="dd2" size="1" value="dd2" onchange="CalculateTotals();" onhaschange="showValue(this.value);" name="Coupons" style="width:100%">
                <option value="0" class="dd2" <%= (0==Model.Coupons)?"selected":"" %>>Selecciona</option>
                <option value="25" class="" <%= (25==Model.Coupons)?"selected":"" %>>25</option>
                <option value="50" class="" <%= (50==Model.Coupons)?"selected":"" %>>50</option>
                <option value="100" class="" <%= (100==Model.Coupons)?"selected":"" %>>100</option>                
                <option value="200" class="" <%= (200==Model.Coupons)?"selected":"" %>>200</option>
            </select>
            <p>&nbsp;</p> 
    
            <span style="color:#0071b6"><%=Html.DisplayNameFor(ret => ret.CouponAmount) %></span>
            <select id="CouponAmount" name="CouponAmount" onchange="CalculateTotals();" onhaschange="showValue(this.value)"  style="width:100%">
                <option value="0" class="dd2" <%= (0==Model.CouponAmount)?"selected":"" %>>Selecciona</option>
                <option value="0.5" class="" <%= (0.5==(double)Model.CouponAmount)?"selected":"" %>>0.5 €</option>
                <option value="1" class="" <%= (1==(double)Model.CouponAmount)?"selected":"" %>>1 €</option>
                <option value="1.5" class="" <%= (1.5==(double)Model.CouponAmount)?"selected":"" %>>1.5 €</option>
                <option value="2" class="" <%= (2==(double)Model.CouponAmount)?"selected":"" %>>2 €</option>
            </select>        
        </div>
    </div>

    <div class="greenhr"><hr /></div>
    <p>&nbsp;</p>
    <input type="submit" value="<%: Resources.RetailerCoupons_BuyButton %>" class="botonverde" />

    <% } %>
     
</asp:Content>
