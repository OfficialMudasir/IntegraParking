<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.WS.Data.WSCities>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <%= Resources.CitiesView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= Url.Content("~/Content/CitiesEM2.css") %>" rel="stylesheet" type="text/css" />    
    <style>

    </style>

    <div class="mainContainer">

        <header class="op-parking">
            <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <span class="valign"></span>
            <h2><%= Resources.CitiesView_Title %></h2>
        </header>
        
        <script type="text/x-kendo-tmpl" id="city-template">
            <div style="display:inline;" >
                # if (itemAlt) {#
                    <div class="licity li-alt">
                # } else { #
                    <div class="licity">
                #}
                    itemAlt = !itemAlt;
                #
                    <div class="li-left cityItem">
                        <a href="<%= Url.Action("StartCity", "Park") %>/#:Id#?name=#:Description#" onclick="$('.loading').show();">
                            <!--<img alt="image" class="k-image" src="<%= Url.Content("/Content/EysaMobv2/location-ico.svg") %>">-->
                            #: Description #
                        </a>
                    </div>
                </div>
            </div>
        </script>


        <% Html.Kendo().ListView<WebParking.WS.Data.WSCity>(Model.GetCities())
            .Name("lstCities")
            .TagName("div")
            .ClientTemplateId("city-template")
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
