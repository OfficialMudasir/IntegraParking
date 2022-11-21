<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ForgotPasswordModel>" %>

<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>

<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.ForgotPassword_Title%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
    <%=Resources.ForgotPassword_Title%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%-- 
<div class="row">
    <div id="paper-top">
        <div class="col-sm-12 col-block">
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
        <li><%=Resources.ForgotPassword_Title%>
        </li>
    </ul>
</div>
--%>

<%-- 
    <div class="title-alt">
        <h6><%=Resources.ForgotPassword_Title%></h6>
    </div> 
--%>
<% using (Html.BeginForm("ForgotPassword", "Account", FormMethod.Post, new { autocomplete="off", @role="form"}))
    { 
%>
<div class="content-wrap">
    <div class="row">
        <div  id="basicClose" class="col-md-12 col-block">
            <%
                foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
                    foreach (ModelError modelError in keyValuePair.Value.Errors) {
                        %>
                        <div class="alert alert-bky-danger">
                            <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                            <span class="bky-cancel"></span>
                            &nbsp;
                            <%= Html.Encode(modelError.ErrorMessage) %>
                        </div>
                        <%
                    } 
                }
            %>
            <div class="form-group">
                <label for="Username"><%=Resources.CustomerInscriptionModel_Email %></label>
                <%= Html.TextBoxFor(cust => cust.Username, new { 
                    @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourEmail,4,50),
                    @required="required",
                    @type="email", 
                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
                    @oninput="this.setCustomValidity('');",
                    @class="form-control" })
                %>
                <p class="help-block"><%=Resources.ForgotPassword_Remarks%></p>
            </div>
            <div class="row-buttons">                           
                <button class="btn btn-bky-primary" type="submit"><span class="bky-mail-ok"></span> &nbsp; <%=Resources.Button_Confirm %></button>
            </div><!--//.row-buttons-->
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->
<%  
    } // Html.BeginForm
%>
</asp:Content>