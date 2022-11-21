<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<OperationExtDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Operations_Title %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/grid.js?v1.2") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //set culture of the Kendo UI
        kendo.culture("<%: System.Threading.Thread.CurrentThread.CurrentCulture  %>");
    </script>

    <%
        var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        var gridDateTimeformat = "{0:" + dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern + "}";
    %>

    <% Html.Kendo().Grid(Model)
        .Name("gridOperationExt")
        .Columns(columns =>
        {
            //columns.Bound(p => p.TypeId).ClientTemplate("#=typeEnumValues[TypeId]#").Width("100px").Hidden(true); //.Groupable(false);
            columns.ForeignKey(p => p.TypeId, (IQueryable)ViewData["chargeOperationTypes"], "ChargeOperationTypeId", "Description").Width("100px");
            columns.Bound(p => p.Username).Width("120px");
            columns.ForeignKey(p => p.MobileOSId, (IQueryable)ViewData["mobileOSs"], "MobileOSId", "Description").Width("100px");
            columns.Bound(p => p.AppVersion).Width("100px");
            columns.Bound(p => p.Installation).Width("150px");
            columns.Bound(p => p.InstallationShortDesc).Width("100px");
            columns.Bound(p => p.DateUTC).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Date).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.DateIni).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.DateEnd).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Amount).ClientTemplate("#=operationsExt_FormatAmount(Amount, AmountCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.ForeignKey(p => p.AmountCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.AmountFinal).ClientTemplate("#=operationsExt_FormatAmount(AmountFinal, BalanceCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.Bound(p => p.Time).Width("75px");
            columns.Bound(p => p.BalanceBefore).ClientTemplate("#=operationsExt_FormatAmount(BalanceBefore, BalanceCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.BalanceCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.ChangeApplied).Format("{0:n6}").Width("75px");
            columns.Bound(p => p.Plate).Width("150px");
            columns.Bound(p => p.TicketNumber).Width("150px");
            columns.Bound(p => p.TicketData).Width("150px").HtmlAttributes(new { title = "#= grid_IsNull(TicketData) #" });
            columns.ForeignKey(p => p.SectorId, (IQueryable)ViewData["groups"], "GroupId", (string)ViewData["groupsDescriptionField"]).Width("150px");
            columns.ForeignKey(p => p.TariffId, (IQueryable)ViewData["tariffs"], "TariffId", (string)ViewData["tariffsDescriptionField"]).Width("150px");

            columns.Bound(p => p.OffstreetEntryDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetNotifyEntryDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetPaymentDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetEndDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetExitLimitDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetExitDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetUTCEntryDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetUTCNotifyEntryDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetUTCPaymentDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetUTCEndDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetUTCExitLimitDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetUTCExitDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.OffstreetLogicalId).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(OffstreetLogicalId) #" });
            columns.Bound(p => p.OffstreetTariff).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(OffstreetTariff) #" });
            columns.Bound(p => p.OffstreetGate).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(OffstreetGate) #" });
            columns.Bound(p => p.OffstreetSpaceDescription).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(OffstreetSpaceDescription) #" });
            
            //columns.Bound(p => p.SuscriptionType).Width("150px");
            columns.ForeignKey(p => p.SuscriptionType, (IList)ViewData["paymentSuscryptionTypes"], "PaymentSuscryptionTypeId", "Description").Width("100px");
            columns.Bound(p => p.InsertionUTCDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");


            columns.Bound(p => p.RechargeDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.RechargeAmount).ClientTemplate("#=operationsExt_FormatAmount(RechargeAmount, RechargeAmountCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.RechargeAmountCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.RechargeBalanceBefore).ClientTemplate("#=operationsExt_FormatAmount(RechargeBalanceBefore, RechargeAmountCurrencyIsoCode)#").Width("100px");
            columns.Bound(p => p.RechargeInsertionUTCDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");

            //public decimal? DiscountId { get; set; }
            columns.Bound(p => p.DiscountDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.DiscountAmount).ClientTemplate("#=operationsExt_FormatAmount(DiscountAmount, DiscountAmountCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.DiscountAmountCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            //public string DiscountAmountCurrencyIsoCode { get; set; }
            columns.Bound(p => p.DiscountAmountFinal).ClientTemplate("#=operationsExt_FormatAmount(DiscountAmountFinal, DiscountAmountCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.DiscountBalanceCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            //public string DiscountBalanceCurrencyIsoCode { get; set; }
            columns.Bound(p => p.DiscountBalanceBefore).ClientTemplate("#=operationsExt_FormatAmount(DiscountBalanceBefore, DiscountBalanceCurrencyIsoCode)#").Width("100px");
            columns.Bound(p => p.DiscountChangeApplied).Format("{0:n6}").Width("150px");
            columns.Bound(p => p.DiscountInsertionUTCDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");

            columns.ForeignKey(p => p.ServiceChargeTypeId, (IQueryable)ViewData["serviceChargeTypes"], "ServiceChargeId", "Description").Width("100px");

            columns.Bound(p => p.OpReference).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(OpReference) #" });
            columns.Bound(p => p.TransactionId).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(TransactionId) #" });
            columns.Bound(p => p.AuthCode).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(AuthCode) #" });
            columns.Bound(p => p.CardHash).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(CardHash) #" });
            columns.Bound(p => p.CardReference).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(CardReference) #" });
            columns.Bound(p => p.CardScheme).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(CardScheme) #" });
            columns.Bound(p => p.MaskedCardNumber).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(MaskedCardNumber) #" });
            columns.Bound(p => p.CardExpirationDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");

            columns.Bound(p => p.ExternalId1).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(ExternalId1) #" });
            columns.Bound(p => p.ExternalId2).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(ExternalId2) #" });
            columns.Bound(p => p.ExternalId3).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(ExternalId3) #" });

            columns.Bound(p => p.Latitude).Format("{0:n6}").Width("150px").HtmlAttributes(new { title = "#= grid_IsNull(Latitude) #" });
            columns.Bound(p => p.Longitude).Format("{0:n6}").Width("150px").HtmlAttributes(new { title = "#= grid_IsNull(Longitude) #" });

            if ((ConfigurationManager.AppSettings["DeleteOperation"] ?? "0") == "1")
                columns.Template(o => o).ClientTemplate("#if (TypeId != 7) {#<a class='k-button k-button-icontext k-grid-Disable' href='\\#' onClick='operationsExt_CmdDelete(#: TypeId #, #: Id #, \"#:Username#\"); return false;' title=\"" + Resources.OperationExt_Delete_Button_Title + "\" ><span class='k-icon k-delete'></span>" + Resources.OperationExt_Delete_Button_Text + "</a>#}#").Width("100px");

        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "OperationExt");
            });
        }
        )
           //.Groupable()
        .Pageable(pager => pager
            .Input(false)
            .Numeric(true)
            .Info(true)
            .PreviousNext(true)
            .Refresh(true)
            .PageSizes(true)
        )
        .Sortable()
        .Scrollable()
           //.Scrollable(s => s.Height("auto"))
        .Filterable(filterable => filterable
            .Extra(true)
           /*.Operators(operators => operators
               .ForString(str => str.Clear()
                   .Contains("Contains")
                   .StartsWith("Starts with")
                   .IsEqualTo("Is equal to")
                   .IsNotEqualTo("Is not equal to")                    
               )
           )*/
        )
        .DataSource(dataSource => dataSource
            .Ajax()
            .PageSize(10)
            .Model(m =>
            {
                m.Id(p => p.Date);
            })
            .Read(read => read.Action("OperationsExt_Read", "Operation", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("DateUTC").Descending())
            .Events(ev =>
                {
                    ev.RequestStart("OperationExt_onRequestStart");
                })
        )
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("OperationExt_onDataBound");
            ev.ColumnHide("OperationExt_onColumnHide");
            ev.ColumnShow("OperationExt_onColumnShow");
            ev.ColumnResize("OperationExt_onColumnResize");
            ev.ColumnReorder("OperationExt_onColumnReorder");            
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        var typeEnumValues = <%= backOffice.Controllers.OperationController.GetChargeOperationsTypeEnum() %>;

        function operationsExt_FormatAmount(amount, currencyIsoCode) {
            if (amount != null) {
                if (currencyIsoCode != null)
                    return kendo.format('{0:0.00} {1}', amount, currencyIsoCode);
                else {
                    if (amount != 0)
                        return kendo.format('{0:0.00}', amount);
                    else
                        return "";
                }
            }
            else
                return "";            
        }

        $(document).ready(function () {

            var defaultFilter = { 
                filters: [ { field: "DateUTC", operator: "", value: null, isDefaultFilter: true} ],
                logic: "and"};

            grid_Ready("OperationExt", defaultFilter);
            
        });

        function operationsExt_CmdDelete(typeId, operationId, username) {

            var msgConfirm = "<%= Resources.OperationExt_Delete_Confirm %>";

            var typeInfo = "";
            if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ParkingOperation %>)
                typeInfo = "<%= Resources.ChargeOperationsType_ParkingOperation.ToLower() %>";
            else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ExtensionOperation %>)
                typeInfo = "<%= Resources.ChargeOperationsType_ExtensionOperation.ToLower() %>";
            else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ParkingRefund %>)
                typeInfo = "<%= Resources.ChargeOperationsType_ParkingRefund.ToLower() %>";
            else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.TicketPayment %>)
                typeInfo = "<%= Resources.ChargeOperationsType_TicketPayment.ToLower() %>";
            else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.BalanceRecharge %>)
                typeInfo = "<%= Resources.ChargeOperationsType_BalanceRecharge.ToLower() %>";
            else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ServiceCharge %>)
                typeInfo = "<%= Resources.ChargeOperationsType_ServiceCharge.ToLower() %>";
            else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.Discount %>)
                typeInfo = "<%= Resources.ChargeOperationsType_Discount.ToLower() %>";

            msgConfirm = msgConfirm.replace("{0}", typeInfo);
            msgConfirm = msgConfirm.replace("{1}", operationId);
            msgConfirm = msgConfirm.replace("{2}", username);
            msgboxConfirm("<%= Resources.OperationExt_Delete_Title %>", msgConfirm, "operationsExt_CmdDeleteConfirm(" + typeId + ", " + operationId + ")");

        }
        function operationsExt_CmdDeleteConfirm(typeId, operationId) {

            kendo.ui.progress($("#gridOperationExt"), true);

            $.ajax({
                type: 'POST',
                url: '<%= Url.Action("OperationExt_Delete", "Operation")%>',
                data: { typeId: typeId, operationId: operationId },
                success: function (response) {
                    
                    try {
                        kendo.ui.progress($("#gridOperationExt"), false);
                        if (response.Result == true) {
                            msgboxAlert("<%= Resources.OperationExt_Delete_Title %>", "<%= Resources.OperationExt_Delete_Success %>", "information");                                                        
                        }
                        else {
                            msgboxAlert("<%= Resources.OperationExt_Delete_Title %>", response.ErrorInfo, "error");                                                        
                        }
                        grid_Refresh("OperationExt");

                    } catch (ex) {
                        msgboxError("<%= Resources.OperationExt_Delete_Title %>", "<%= Resources.OperationExt_Delete_Error %>");
                    }
                },
                error: function (xhr) {
                    kendo.ui.progress($("#gridOperationExt"), false);
                    msgboxError("<%= Resources.OperationExt_Delete_Title %>", "<%= Resources.OperationExt_Delete_Error %>");
                }
            });

        }

    </script>

</asp:Content>

