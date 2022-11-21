<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Tree
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Tree</h2>

<% Html.Kendo().Splitter()
      .Name("horizontal")
      .Orientation(SplitterOrientation.Horizontal).HtmlAttributes(new { style = "height: 400px;" })
      .Panes(verticalPanes =>
      {
          verticalPanes.Add()
              .HtmlAttributes(new { id = "left-pane" })
              .Size("300px")
              .Scrollable(false)
              .Collapsible(false)
              .Content(() =>
              {
                %>

<div class="treeview-back">
    <h3>Usuarios y grupos</h3>

    <%= Html.Kendo().TreeView()
        .Name("treeUsers")
        .Items(treeview =>
        {
            treeview.Add().Text("Administradores")
                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/users16.png"))
                .Expanded(true)
                .Items(user =>
                {
                    user.Add().Text("admin")
                        .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/user16.png"));
                });

            treeview.Add().Text("Mantenedores")
                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/users16.png"))
                .Expanded(true)
                .Items(user =>
                {
                    user.Add().Text("Técnicos 1")
                        .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/users16.png"))
                        .Expanded(true)
                        .Items(tec =>
                        {
                            tec.Add().Text("Técnico1")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/user16.png"));
                            tec.Add().Text("Técnico2")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/user16.png"));
                        });
                    user.Add().Text("Técnicos 2")
                        .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/users16.png"))
                        .Expanded(true)
                        .Items(tec =>
                        {
                            tec.Add().Text("Técnico3")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/user16.png"));
                        });
                    user.Add().Text("Usuario1")
                        .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/user16.png"));
                    user.Add().Text("Usuario2")
                        .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/user16.png"));
                });

        })
    %>

</div>

<% });

          verticalPanes.Add()
              .HtmlAttributes(new { id = "right-pane" })
              .Collapsible(false)
              .Content(() =>
              { %>

<div class="treeview-back" style="width: 225px; height: 300px;">
    <h3>Acceso a las Instalaciones</h3>

<script id="treeInstallations-template" type="text/kendo-ui-template">
    #: item.text #    
    # if (!item.items) { #
        <a class='features-link' href='\#' title='Configurar permisos para la instalación' onclick='alert("a");'></a>
    # } else { #
        <a class='features-link' href='\#' title='Configurar permisos para todas las instalaciones'></a>
    # } #
</script>

    <%= Html.Kendo().TreeView()
        .Name("treeInstallations")
        .TemplateId("treeInstallations-template")
        .Checkboxes(checkboxes => checkboxes
            .Name("checkedFiles")
            .CheckChildren(true)
        )
        .Items(treeview =>
        {
            treeview.Add().Text("Todas las instalaciones")
                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/installations16.png"))
                .Expanded(true)
                .Items(installation =>
                {
                    installation.Add().Text("Barcelona")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/installation16.png"));

                    installation.Add().Text("Madrid")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/installation16.png"));

                    installation.Add().Text("Bilbao")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/installation16.png"));

                });

        })
    %>
</div>

<div class="treeview-back" style="width: 300px; height: 400px;">
    <h3>Permisos de la Instalación</h3>
    
<script id="treeFeatures-template" type="text/kendo-ui-template">

    <!--<img alt="image" class="k-image" src="/Plugins/MaintenancePlugin/Content/images/tree/admin12.png">
    <img alt="image" class="k-image" src="/Plugins/MaintenancePlugin/Content/images/tree/write12.png">    
    <img alt="image" class="k-image" src="/Plugins/MaintenancePlugin/Content/images/tree/read12.png">    -->
    <button type="button" class="k-button k-button-icon"><span class="k-icon admin-icon"></span></button>
    <button type="button" class="k-button k-button-icon"><span class="k-icon write-icon"></span></button>
    <button type="button" class="k-button k-button-icon k-state-selected"><span class="k-icon read-icon"></span></button>
</script>

    <%= Html.Kendo().TreeView()
        .Name("treeFeatures")
        .Checkboxes(checkboxes => checkboxes
            .Name("checkedFiles")
            .CheckChildren(true)
            //.Template("<img alt=\"image\" class=\"k-image\" src=\"/Plugins/MaintenancePlugin/Content/images/tree/admin16.png\">")
            .TemplateId("treeFeatures-template")
        )
        .Items(treeview =>
        {
            treeview.Add().Text("Barcelona")
                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/config216.png"))
                .Expanded(true)
                .Items(feature =>
                {
                    feature.Add().Text("Producción")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"))
                                .Expanded(true)
                                .Items(feature1 =>
                                {
                                    feature1.Add().Text("Mapa").Id("btMapa")
                                            .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                    feature1.Add().Text("Dashboard")
                                            .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                    feature1.Add().Text("Alarmas")
                                            .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"))
                                            .Expanded(true)
                                            .Items(feature1_1 =>
                                            {
                                                feature1_1.Add().Text("Estado de Alarmas")
                                                          .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                                feature1_1.Add().Text("Histórico de Alarmas")
                                                          .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                            });
                                });
                    feature.Add().Text("Finanzas")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"))
                                .Expanded(true)
                                .Items(feature1 =>
                                {
                                    feature1.Add().Text("Estadísticas")
                                            .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                });
                    feature.Add().Text("Configuración")
                                .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"))
                                .Expanded(true)
                                .Items(feature1 =>
                                {
                                    feature1.Add().Text("Usuarios")
                                            .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                    feature1.Add().Text("Parámetros")
                                            .ImageUrl(Url.Content(RouteConfig.BasePath + "Content/images/tree/feature16.png"));
                                });

                });

        })
    %>
</div>

<% });
      })
      .Render();
%>

<style scoped>
    #treeview-sprites .k-sprite {
        background-image: url("<%= Url.Content("~/Content/web/treeview/coloricons-sprite.png")%>");
    }
    
    .rootfolder { background-position: 0 0; }
    .folder { background-position: 0 -16px; }
    .pdf { background-position: 0 -32px; }
    .html { background-position: 0 -48px; }
    .image { background-position: 0 -64px; }
    
    .treeview-back 
    {
        /*float: left;*/
        width: 200px;
        margin: 30px;
        padding: 20px;
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,0.45), inset 0 0 30px rgba(0,0,0,0.07);
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,0.45), inset 0 0 30px rgba(0,0,0,0.07);
        box-shadow: 0 1px 2px rgba(0,0,0,0.45), inner 0 0 30px rgba(0,0,0,0.07);
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
        height: 300px;
    }
    
    .treeview-back h3
    {
        margin: 0 0 10px 0;
        padding: 0;
    }

    .k-checkbox {
        margin-top: 5px;
    }

    .features-link {
        width: 12px;
        height: 12px;
        background: transparent url("<%=Url.Content(RouteConfig.BasePath + "/Content/images/tree/config216.png")%>") no-repeat 50% 50%;
        overflow: hidden;
        display: inline-block;
        font-size: 0;
        line-height: 0;
        vertical-align: top;
        margin: 2px 0 0 3px;
        -webkit-border-radius: 5px;
        -mox-border-radius: 5px;
        border-radius: 5px;
    }

    .admin-icon {
        width: 12px;
        height: 12px;
        background: transparent url("<%=Url.Content(RouteConfig.BasePath + "/Content/images/tree/admin12.png")%>") no-repeat 50% 50%;
        overflow: hidden;
        display: inline-block;
        font-size: 0;
        line-height: 0;
        vertical-align: top;
        margin: 2px 0 0 3px;
        -webkit-border-radius: 5px;
        -mox-border-radius: 5px;
        border-radius: 5px;
    }
    .write-icon {
        width: 12px;
        height: 12px;
        background: transparent url("<%=Url.Content(RouteConfig.BasePath + "/Content/images/tree/write12.png")%>") no-repeat 50% 50%;
        overflow: hidden;
        display: inline-block;
        font-size: 0;
        line-height: 0;
        vertical-align: top;
        margin: 2px 0 0 3px;
        -webkit-border-radius: 5px;
        -mox-border-radius: 5px;
        border-radius: 5px;
    }
    .read-icon {
        width: 12px;
        height: 12px;
        background: transparent url("<%=Url.Content(RouteConfig.BasePath + "/Content/images/tree/read12.png")%>") no-repeat 50% 50%;
        overflow: hidden;
        display: inline-block;
        font-size: 0;
        line-height: 0;
        vertical-align: top;
        margin: 2px 0 0 3px;
        -webkit-border-radius: 5px;
        -mox-border-radius: 5px;
        border-radius: 5px;
    }
    
.k-state-selected,
.k-button:active,
.k-draghandle.k-state-selected:hover {
  background-image: none;
  background-image: none, -webkit-linear-gradient(top, #C0C3C4 0px, #CCD8DF 100%);
  background-image: none, -moz-linear-gradient(top, #C0C3C4 0px, #CCD8DF 100%);
  background-image: none, -o-linear-gradient(top, #C0C3C4 0px, #CCD8DF 100%);  
  background-image: none, linear-gradient(to bottom, #C0C3C4 0px, #CCD8DF 100%);
  border-color: #dbdbde;
}

</style>

</asp:Content>

