<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<PermitsModel>" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="integraMobile.Models" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>
    <%= resBundle.GetString("Permits_SignUp") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if (top.location != this.location) {
                $("html, body").css("display", "none");
                top.location = this.location;
            }
            else {
                $("html, body").css("display", "block");
            }
        });
    </script>
    <link href="<%= Url.Content("~/Content/Permits.css?rnd=1") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentTitle" runat="server">
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>
    <%= resBundle.GetString("Permits_SignUp") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>    
    <div id="loader"></div>        
    <div class="content-wrap">
        <div class="row">
            <div class="col-md-12-col-block">
                <h3><%= resBundle.GetString("Permits_Manage")%></h3>

                    <div class="alert alert-bky-warning notice">
                        <span class="bky-cancel"></span> 
                        &nbsp;  
                        <p><% = Model.Error %></p>
                    </div>

                    <div class="row-buttons">
                        <a class="btn btn-bky-primary" href="<%= Url.Action("ActivePermits", "Permits") %>" >
                            <%= resBundle.GetString("Account_Main_BttnActivePermits")%>
                        </a>
                    </div>
            </div><!--//.col-block-->
        </div><!--//.row-->
    </div><!--//content-wrap-->

</asp:Content>