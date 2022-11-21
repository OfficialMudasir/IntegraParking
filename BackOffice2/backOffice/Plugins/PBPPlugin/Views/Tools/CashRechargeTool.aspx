<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<backOffice.Models.CashRechargeDataModel>" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>

<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Infrastructure.Security" %>
<%@ Import Namespace="PBPPlugin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>
    <%= resBundle.GetString("PBPPlugin", "CashRecharge.Title", "Cash Recharge") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/cashRecharge.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

    <br />

    <div class="editor-form">
        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.InstallationId)%>
            </div>
            <div class="editor-field">
                <%=
                    Html.Kendo().DropDownList()
                               .Name("ddlInstallations")
                               .DataTextField("Description")
                               .DataValueField("InstallationId")
                               .AutoBind(true)                               
                               .DataSource(ds =>
                               {                                   
                                   ds.Read("InstallationsCashRecharge_Read", "Tools", new { plugin = "PBPPlugin" });
                               })
                               .Events(events => events.DataBound("cashRecharge_OnInstallationsDataBound")
                                                       .Change("cashRecharge_OnInstallationsChange"))
                    
                %>
                <%= Html.ValidationMessageFor(m => m.InstallationId) %>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.Email)%>
            </div>
            <div class="editor-field">
                <%=
                    Html.TextBoxFor(m => m.Email, new { id="Email", @class = "k-input k-textbox", style = "width:100%;" })
                %>
                <%= Html.ValidationMessageFor(m => m.Email) %>
            </div>
        </div>
        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.EmailRepeat)%>
            </div>
            <div class="editor-field">
                <%=
                    Html.TextBoxFor(m => m.EmailRepeat, new { id="EmailRepeat", @class = "k-input k-textbox", style = "width:100%;" })
                %>
                <%= Html.ValidationMessageFor(m => m.EmailRepeat) %>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.TotalAmount) %>
            </div>
            <div class="editor-field">
                <%=
                    Html.Kendo().NumericTextBoxFor(m => m.TotalAmount).Format("n2").Decimals(2)                                                    
                %>
                <%= Html.ValidationMessageFor(m => m.TotalAmount) %>
            </div>
        </div>


        <div class="buttons-container">
            <div class="buttonsleft-container">
            </div>
            <div class="buttonsright-container">
                <button type="button" id="btnTransfer" class="k-button k-button-icon"
                    onclick="cashRecharge_Recharge(); return false;"
                    title="<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>">
                    <span class="k-icon recharge-icon"></span><%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge", "Recharge") %>
                </button>
            </div>

        </div>

    </div>

    <div id="dlgRechargeSummary" style="display:none;">

        <div class="k-edit-form-container rechargeSummary-content" >

            <div class="editor-container summary-title">                
                <%= Html.Label("SummaryInfo", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Info", "Resumen de la recarga"), new { id = "lblSummaryInfo" } ) %>
                <%= Html.Label("SummarySuccess", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Success", "!! La operación de recarga se realizó satisfactoriamente !!"), new { id = "lblSummarySuccess" } ) %>
            </div>

            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("SummaryEmail", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Email", "User e-mail:")) %>
                </div>
                <div class="editor-field">
                    <input id="txtSummaryEmail" class="k-textbox input-summary" readonly="readonly"></input>
                </div>
            </div>

            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("SummaryTotalAmount", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.TotalAmount", "Importe abonado:")) %>
                </div>
                <div class="editor-field">
                    <input id="txtSummaryTotalAmount" class="k-textbox input-summary" readonly="readonly"></input>
                </div>
            </div>

            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("SummaryFEE", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.FEE", "Coste del Servicio:")) %>
                </div>
                <div class="editor-field">
                    <input id="txtSummaryFEE" class="k-textbox input-summary" readonly="readonly"></input>
                </div>
            </div>

            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("SummaryVAT", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.VAT", "IVA asociado:")) %>
                </div>
                <div class="editor-field">
                    <input id="txtSummaryVAT" class="k-textbox input-summary" readonly="readonly"></input>
                </div>
            </div>

            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("SummaryAmount", resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Amount", "Importe a recargar:")) %>
                </div>
                <div class="editor-field">
                    <input id="txtSummaryAmount" class="k-textbox input-summary" readonly="readonly"></input>
                </div>
            </div>

        </div>

        <div id="divSumaryButtonsConfirm" class="rechargeSummary-buttons">
            <button id="btnSummaryCancel" type="button" class="k-button k-button-icon" 
                    onclick="$('#dlgRechargeSummary').data('kendoWindow').save = false; $('#dlgRechargeSummary').data('kendoWindow').close(); return false;"
                    title="<%= resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Cancel.Button.Title", "< Atrás") %>">
                <span class="k-icon k-cancel"></span><%= resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Cancel.Button", "< Atrás") %>
            </button>
            <button id="btnSummaryRecharge" type="button" class="k-button k-button-icon" 
                    onclick="$('#dlgRechargeSummary').data('kendoWindow').save = true; $('#dlgRechargeSummary').data('kendoWindow').close(); return false;"
                    title="<%= resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Recharge.Button.Title", "Recargar") %>">
                <span class="k-icon k-update"></span><%= resBundle.GetString("PBPPlugin", "CashRecharge.Summary.Recharge.Button", "Recargar") %>
            </button>
        </div>
        <div id="divSumaryButtonsAccept" class="rechargeSummary-buttons">
            <button id="btnSummaryAccept" type="button" class="k-button k-button-icon" 
                    onclick="$('#dlgRechargeSummary').data('kendoWindow').save = false; $('#dlgRechargeSummary').data('kendoWindow').close(); return false;"
                    title="<%= resBundle.GetString("PBPPlugin", "CashRecharge.Accept.Button.Title", "Aceptar") %>">
                <span class="k-icon k-update"></span><%= resBundle.GetString("PBPPlugin", "CashRecharge.Accept.Button", "Aceptar") %>
            </button>
        </div>

    </div>

    <script>

        var crInstallationId = <%: (int) Model.InstallationId %>;

        $(document).ready(function () {

            var dlg = $("#dlgRechargeSummary");
            if (!dlg.data("kendoWindow")) {
                dlg.kendoWindow({
                    title: "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Cash Recharge") %>",
                    modal: true,
                    width: "500px",
                    height: "300px",
                    visible: false,
                    resizable: false,
                    actions: [
                        //"Pin",
                        //"Refresh",
                        //"Minimize",
                        //"Maximize",
                        "Close"
                    ],
                    open: function () {
                        this.save = false;
                    },
                    close: function (e) {
                        //commandsModel.cancelInProgress();
                        if (e.userTriggered) this.save = false;
                        if (this.save && !cashRecharge_Save()) {
                            e.preventDefault();
                        }
                    },
                    resize: cashRecharge_ResizeAll,
                    open: function (e) {
                        setTimeout(cashRecharge_ResizeAll, 1000);
                    }

                });
            }

            $(".editor-form").kendoValidator();

        });

        function cashRecharge_OnInstallationsDataBound(e) {

            var ddl = $("#ddlInstallations").data("kendoDropDownList");            
            ddl.select(function (dataItem) {
                return dataItem.id === <%: (int) Model.InstallationId %>;
            });

        }
        
        function cashRecharge_OnInstallationsChange() {
            var value = this.value();
            crInstallationId = value;
        }

        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }


        function cashRecharge_Recharge() {
            var validator = $(".editor-form").kendoValidator().data("kendoValidator");
            if (validator.validate()) {
                cashRecharge_Recharge_Confirm();
            }
        }
        function cashRecharge_Recharge_Confirm() {
                           
            var sEmail = $("#Email").val();
            var sEmailRepeat = $("#EmailRepeat").val();                
            var sTotalAmount = $("#TotalAmount").val();
            var fTotalAmount = parseFloat(sTotalAmount.replace(",", ".")).toFixed(2);                

            if (crInstallationId > 0 && sEmail != "" && sEmail == sEmailRepeat && fTotalAmount > 0) {

                kendo.ui.progress($(".editor-form"), true);

                $.ajax({
                    type: 'GET',
                    url: "<%= Url.Action("CashRechargeSummary", "Tools" )%>",
                    data: { plugin: 'PBPPlugin', installationId: crInstallationId, email: sEmail, totalAmount: fTotalAmount },
                    success: function (response) {

                        try {
                            kendo.ui.progress($(".editor-form"), false);

                            if (response.Result == true) {
                                
                                $("#txtSummaryEmail").val(response.Data.Email);
                                $("#txtSummaryAmount").val(response.Data.Amount);
                                $("#txtSummaryFEE").val(response.Data.FEE);
                                $("#txtSummaryVAT").val(response.Data.VAT);
                                $("#txtSummaryTotalAmount").val(response.Data.TotalAmount);
                                //$("#divSumaryButtonsConfirm").show();
                                //$("#divSumaryButtonsAccept").hide();
                                $("#divSumaryButtonsConfirm").css("display", "block");
                                $("#divSumaryButtonsAccept").css("display", "none");
                                $("#lblSummaryInfo").show();
                                $("#lblSummarySuccess").hide();


                                var dlg = $("#dlgRechargeSummary").data("kendoWindow");
                                dlg.center();
                                dlg.open();

                            }
                            else {
                                msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Error", "Recharge Error") %>: " + response.ErrorInfo, "warning");
                            
                        }

                    } catch (ex) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Error", "Recharge Error") %>");
                        kendo.ui.progress($(".editor-form"), false);
                    }
                },
                    error: function (xhr) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Error", "Recharge Error") %>");
                    kendo.ui.progress($(".editor-form"), false);
                }
                });
            }
            else {
                if (crInstallationId <= 0)
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "CashRechargeDataModel.Installation", "Installation")) %>", "warning");
                else if (sEmail == "")
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "CashRechargeDataModel.Email", "Email")) %>", "warning");
                else if (sEmail != sEmailRepeat)
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.DifferentEmails", "Different emails") %>", "warning");
                else
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_InvalidField", "InvalidField"), resBundle.GetString("PBPPlugin", "CashRechargeDataModel.TotalAmount", "Amount")) %>", "warning");
            }
        }

        function cashRecharge_Save() {

            var sEmail = $("#Email").val();
            var sEmailRepeat = $("#EmailRepeat").val();                
            var sTotalAmount = $("#TotalAmount").val();
            var fTotalAmount = parseFloat(sTotalAmount.replace(",", ".")).toFixed(2);                

            if (crInstallationId > 0 && sEmail != "" && sEmail == sEmailRepeat && fTotalAmount > 0) {

                kendo.ui.progress($(".editor-form"), true);

                $.ajax({
                    type: 'GET',
                    url: "<%= Url.Action("CashRechargeSave", "Tools" )%>",
                    data: { plugin: 'PBPPlugin', installationId: crInstallationId, email: sEmail, totalAmount: fTotalAmount },
                    success: function (response) {

                        try {
                            kendo.ui.progress($(".editor-form"), false);

                            if (response.Result == true) {
                                
                                $("#txtSummaryEmail").val(response.Data.Email);
                                $("#txtSummaryAmount").val(response.Data.Amount);
                                $("#txtSummaryFEE").val(response.Data.FEE);
                                $("#txtSummaryVAT").val(response.Data.VAT);
                                $("#txtSummaryTotalAmount").val(response.Data.TotalAmount);
                                $("#divSumaryButtonsConfirm").hide();                                
                                $("#divSumaryButtonsAccept").show();
                                $("#lblSummaryInfo").hide();
                                $("#lblSummarySuccess").show();

                                $("#Email").val("");
                                $("#EmailRepeat").val("");
                                $("#TotalAmount").data("kendoNumericTextBox").value(0);
                                                                
                                var dlg = $("#dlgRechargeSummary").data("kendoWindow");
                                dlg.center();
                                dlg.save = false;
                                dlg.open();
                            }
                            else {
                                msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Error", "Recharge Error") %>: " + response.ErrorInfo, "warning");                            
                            }

                        } catch (ex) {
                            msgboxError("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Error", "Recharge Error") %>");
                            kendo.ui.progress($(".editor-form"), false);
                        }
                    },
                    error: function (xhr) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Error", "Recharge Error") %>");
                        kendo.ui.progress($(".editor-form"), false);
                    }
                });

                return true;
            }
            else {
                if (crInstallationId <= 0)
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "CashRechargeDataModel.Installation", "Installation")) %>", "warning");
                else if (sEmail == "")
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "CashRechargeDataModel.Email", "Email")) %>", "warning");
                else if (sEmail != sEmailRepeat)
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.DifferentEmails", "Different emails") %>", "warning");
                else
                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "CashRecharge.Recharge.Title", "Recharge") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_InvalidField", "InvalidField"), resBundle.GetString("PBPPlugin", "CashRechargeDataModel.TotalAmount", "Amount")) %>", "warning");

                return false;
            }
            
        }

        function cashRecharge_ResizeAll() {

        }

    </script>

</asp:Content>
