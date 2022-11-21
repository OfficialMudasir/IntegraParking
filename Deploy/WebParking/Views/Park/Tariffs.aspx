<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<WebParking.WS.Data.WSTariff>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.TariffsView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <link href="<%= Url.Content("~/Content/TariffEM2.css") %>" rel="stylesheet" type="text/css" />    


    <style>

    </style>

    <div class="mainContainer">


        <header class="op-parking">
            <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <span class="valign"></span>
            <h2><%= Resources.TariffsView_Title %></h2>
        </header>

        <script type="text/x-kendo-tmpl" id="tariff-template">
            <div class="tariffItemButton tariffItemType#:Id#">
                # if (itemAlt) {#
                    <div class="licity li-alt">
                # } else { #
                    <div class="licity">
                #}
                    itemAlt = !itemAlt;
                #
                        <div class="li-left tariffItem ">
                        <a href="<%= Url.Action("QueryParking", "Park") %>?groupId=<%= ViewData["GroupId"] %>&tariffId=#:Id#" onclick="$('.loading').show();">
                            <%/*<img alt="image" class="k-image" src="<%= Url.Content("~/Content/images/TariffIcon.png") %->"/>*/%>
                            <span>#: Description #</span>
                        </a>
                        </div>
                    
                </div>
            </div>
        </script>
    
        <% Html.Kendo().ListView<WebParking.WS.Data.WSTariff>(Model)
            .Name("lstTariffs")
            .TagName("div")
            .ClientTemplateId("tariff-template")
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
