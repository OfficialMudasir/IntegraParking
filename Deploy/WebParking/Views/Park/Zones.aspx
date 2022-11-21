<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.WS.Data.WSZoneTar>" %>
<%@ Import Namespace="WebParking.WS.Data" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.ZonesView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <link href="<%= Url.Content("~/Content/ZonesEM2.css") %>" rel="stylesheet" type="text/css" />    


     <style>

    </style>
    
 
        <header class="op-parking">
            <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <span class="valign"></span>
            <h2><%= Resources.ZonesView_Title %></h2>
        </header>


    <% using (Html.BeginForm("ZoneSelected", "Park", FormMethod.Post))
        { %>  

            <script id="treeZones-template" type="text/kendo-ui-template">
                # if (!item.hasChildren) { #
                    <div class="treeZone-template treezone-child">
                # } else { #
                    <div class="treeZone-template treezone-group">
                # } #


                    <div class="treeZone-template-left">
                        <span>#= GetNumDesc(item.text) #</span>
                    </div>
                    <div class="treeZone-template-right">
                        <span>
                        # if (!item.hasChildren) { #
                            <a href="<%= Url.Action("ZoneSelected", "Park") %>?groupId=#:item.id#" onclick="$('.loading').show();">
                                <!--<img alt="image" class="k-image" src="<%= Url.Content("~/Content/images/ZoneNumIcon.png") %>" style="height:16px;">-->
                                #= GetDescription(item.text) #
                            </a>
                        # } else { #
                            <!--<img alt="image" class="k-image" src="<%= Url.Content("~/Content/images/ZoneNumIcon.png") %>" style="height:16px;">-->
                            #= GetDescription(item.text) #
                        # } #
                        </span>
                    </div>
                </div>
            </script>

            <%= Html.Kendo().TreeView()
                            .Name("treeZones")
                            .HtmlAttributes(new { required = ""})
                            .BindTo(Model.Zones.Tree())
                            .TemplateId("treeZones-template")
                            .Events(ev => ev.Select("ZonesView_TreeZonesOnSelect")) %>
            
            <input id="txtGroupId" type="text" name="groupId" value="" style="display:none;" />
            <!--<span class="k-invalid-msg" data-for="groupId"></span>-->
            

            <div class="zones-submit" style="display:none;">
                <button type="submit" class="k-button k-button-icon"                 
                        title="<%= Resources.ZonesView_SubmitButton_Title %>"
                        onclick="if ($('#txtGroupId').val() == '') { alert('error'); return false; } else { return true; } ">
                    <span class="k-icon k-update"></span><%= Resources.ZonesView_SubmitButton %>
                </button>
            </div>

            <div class="zone-errors">
                <%: Html.ValidationSummary(true, "") %>
            </div>
        

    <%  } %>

<script>

    function ZonesView_TreeZonesOnSelect(e) {
        var data = $("#treeZones").data("kendoTreeView").dataItem(e.node);        

        $("#txtGroupId").val(data.id);
    }

    function GetNumDesc(text) {
        return text.split("~")[0];
    }
    function GetDescription(text) {
        return text.split("~")[1];
    }

    /* TGA @ One click Collapse tree view */


    $("#treeZones").on("click", ".k-in", function (e) {
        var tree = $("#treeZones").data('kendoTreeView');
        tree.toggle($(e.target).closest(".k-item"));
    });

</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
