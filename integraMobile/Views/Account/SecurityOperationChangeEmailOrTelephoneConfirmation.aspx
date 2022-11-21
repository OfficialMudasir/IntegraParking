<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Resources.SecurityOperationChangeEmailOrTelephone_Confirmation%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
    <%=Resources.SecurityOperationChangeEmailOrTelephone_Confirmation%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%-- 
<div class="row">
    <div id="paper-top">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.ServiceName %>
                </span>
            </h2>
        </div>
    </div>
</div>
 --%>
<%--
<div id="breadcrumb-wrapper" class="row">
    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Index", "Home") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.SecurityOperationChangeEmailOrTelephone_Confirmation%>
        </li>
    </ul>
</div>
--%>
<%-- 
<div class="title-alt">
    <h6>
        <%=Resources.SecurityOperationChangeEmailOrTelephone_Confirmation%>
    </h6>
</div>
 --%>
<div class="content-wrap">
    <div class="row">
        <div class="col-sm-12 col-block">

            <%
                foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
                    foreach (ModelError modelError in keyValuePair.Value.Errors) {
                    %>
                        <div class="alert alert-bky-danger">
                            <span class="bky-cancel"></span>  
                            &nbsp; 
                            <%= Html.Encode(modelError.ErrorMessage) %>
                        </div>
                    <%
                    }
                }
            %>

            <%if (!Convert.ToBoolean(ViewData["ConfirmationError"])) { %>

                <div class="alert alert-bky-success"> 
                        <span class="bky-done"></span> 
                        &nbsp; 
                        <%=Resources.SecurityOperationChangeEmailOrTelephone_ConfirmationText1 %>
                </div>
                <hr class="separator-block-line">
                <div class="lead well">
                    <ul>
                        <li><%=Resources.SecurityOperationChangeEmailOrTelephone_ConfirmationText2 %> <%=ViewData["Email"] %></li>
                        <li><%=Resources.SecurityOperationChangeEmailOrTelephone_ConfirmationText3 %><%=ViewData["Telephone"] %></li>
                    </ul>
                </div>
            <% } %>

            <p class="text-center"><a href="<%=Url.Action("Index", "Home", null)%>" class="btn btn-bky-primary"><%=Resources.Button_Logon %></a></p>
        </div>
    </div>
</div>

</asp:Content>
