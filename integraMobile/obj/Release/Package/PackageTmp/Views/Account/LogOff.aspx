<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Logout_Button %>
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
        <li><%=Resources.Account_Logout_Button %>
        </li>
    </ul>
</div>
--%>
<%--
<div class="title-alt">
    <h6><%=Resources.Account_Logout_Button %></h6>
</div>
--%>
<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
            <div class="row-buttons">
                <a href="<%=Url.Action("Recharge", "Account", null)%>" class="btn btn-bky-primary"><%=Resources.Account_Recharge_BuyNow %></a>
                &nbsp;
                <a href="<%=Url.Action("Index", "Home", null)%>" class="btn btn-bky-sec-default"><%=Resources.Account_Logout_Button %></a>
            </div>
        </div>
    </div>
</div>
</asp:Content>