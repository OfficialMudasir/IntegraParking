<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.RegistrationForm %> - [[Summary]] --%>
    <%=Resources.RegistrationForm %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div class="row">
    <div id="Div1">
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
    <li>Summary
    </li>
</ul>
</div>
--%>
<%--
<div class="title-alt">
    <h6>
        <%=string.Format(Resources.CustomerInscriptionModel_Welcome, ViewData["UserNameAndSurname"])%>!
    </h6>
</div>
--%>

<div class="content-wrap">

    <%--    ALERTS  --%>
    <div class="row">
        <div class="col-xs-12 col-block">
            <div class="alert alert-bky-success">
                <span class="bky-done"></span>
                &nbsp;
                <%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_1%> 
                &nbsp;
                <b><%=string.Format(Resources.CustomerInscriptionModel_SuccessCreatingAccount_3,ViewData["email"]) %></b>
            </div>
        </div><!--// .col-xs-12 -->
    </div><!--// .row ALERTS -->
            
    <!-- ROW FORM -->
    <div class="row">
        <div class="col-sm-8 col-sm-offset-2 col-xs-12 col-block">
                <h3><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_4%></h3>

                <ul>
                    <li><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_5%></li>
                    <li><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_6%></li>
                    <li><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_7%></li>
                    <li><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_8%></li>
                </ul>
                
                <p><strong><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_9%></strong></p>

                <div class="row-buttons">
                    <a href="<%= Url.Action("Index", "Home") %>" class="btn btn-bky-primary"><%=Resources.CustomerInscriptionModel_Summary_Button_Login%></a>
                </div>

        </div><!--//.col-block-->
    </div><!--//.row FORM-->

</div><!--//.content-wrap-->
</asp:Content>