<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.WS.Data.WSUserPlates>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.PlatesView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= Url.Content("~/Content/PlatesEM2.css") %>" rel="stylesheet" type="text/css" />    
    <style>
    </style>

    <div class="mainContainer">

        <header class="op-parking">
            <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <span class="valign"></span>
            <h2><%= Resources.PlatesView_Title %></h2>
        </header>

        <script type="text/x-kendo-tmpl" id="plate-template">
            <div style="display:inline;" >
                # if (itemAlt) {#
                    <div class="licity liPlate li-alt">
                # } else { #
                    <div class="licity liPlate">
                #}
                    itemAlt = !itemAlt;
                #
                    <div class="li-left plateItem">
                        <a href="<%= Url.Action("PlateSelected", "Park") %>/#:data#" onclick="$('.loading').show();">
                            <!--<img alt="image" class="k-image" src="<%= Url.Content("Content/img/plate.png") %>">-->
                            #: data #
                        </a>
                    </div>
                </div>
            </div>
        </script>

        <% Html.Kendo().ListView<string>(Model.Plates)
            .Name("lstPlates")
            .TagName("div")
            .ClientTemplateId("plate-template")
            .Selectable()
            .DataSource(dataSource =>
            {
                //dataSource.Read(read => read.Action("Languages_Read", "UnitInformation", new { plugin = "ToolsPlugin" }));
                dataSource.ServerOperation(false);
                //dataSource.PageSize(10);
            })
            //.Pageable()
            .Render();
        %>

    </div>

<script type="text/javascript">

    var itemAlt = false;

</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
