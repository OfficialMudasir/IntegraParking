<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="MaintenancePlugin" %>
<%@ Import Namespace="MaintenancePlugin.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%
        var resBundle = ResourceBundle.GetInstance(Model.MaintenanceData.Definition.ResourcesAssemblyName);
    %>
    <%= resBundle.GetString("Maintenance", string.Format("Fields.{0}", Model.MaintenanceData.Name)) %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    
    <br />
    <div style="height: 100%;">
    <%
        //Html.RenderPartial("../../Plugins/MaintenancePlugin/Views/Shared/Maintenance", Model);
        Html.RenderPartial(RouteConfig.BasePath + "Views/Shared/Maintenance.ascx", Model);
    %>
    </div>

    <script type="text/javascript">

        $(window).resize(function () {
            try {
                if (ipsGrid<%: Model.ModelId %> != null)
                    //setTimeout(function () {
                    ipsGrid<%: Model.ModelId %>.ResizeGrid($(window).height() - $("#" + ipsGrid<%: Model.ModelId %>.gridId).offset().top - 10);
                //}, 500);
            }
            catch (ex) { }
        });

    </script>

</asp:Content>

