<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RegistrationModelSignUpStep1>" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RegistrationForm %> - <%=Resources.SignUp1_Emailmsg %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
     <%-- <%=Resources.Home_AddNewUser %> --%>
     <%=Resources.RegistrationForm %>
     
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
        <li><%=Resources.Home_AddNewUser%>
        </li>
    </ul>
</div>
--%>
<%--
<div class="title-alt">
    <h6>
        <%=Resources.RegistrationForm %> - <%=Resources.SignUp1_Emailmsg %>
    </h6>
</div>
--%>
<% 
    using (Html.BeginForm("Step1", "Registration", FormMethod.Post, new { @role = "form" })) 
    {
        %> 
        <div class="content-wrap">
            <%--    ALERTS  --%>
            <div class="row">
                <div class="col-xs-12 col-block">
                    <%
                        foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                        {
                            foreach (ModelError modelError in keyValuePair.Value.Errors) 
                            {
                                %>
                                <div class="alert alert-bky-danger">
                                    <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                    <span class="bky-cancel"></span> 
                                    &nbsp; 
                                    <%= Html.Encode(modelError.ErrorMessage) %>
                                </div>
                                <%
                            } // ModelError
                        } // ModelState
                    %>
                </div><!--// .col-xs-12.col-block -->
            </div><!-- // .row ALERTS -->
            <%--    STEPS   --%>
            <div class="row">
                <div class="col-xs-12 col-block">
                    <ul class="steps-dots">
                        <li class="step current">
                            <span class="number">1.</span>
                            <span class="step-desc"> <%=Resources.SignUp1_Emailmsg %></span>
                        </li>
                        <!-- .step.step-arrow -->
                        <li class="step step-arrow">
                            <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                        </li>
                        <li class="step disabled">
                            <span class="number">2.</span>
                            <span class="step-desc"> <%=Resources.SignUp3_Passwmsg%></span>
                        </li>
                    </ul>
                </div>
            </div><!--//.row STEPS -->
            <%--    FORM    --%>
            <div class="row">
                <div id="basicClose" class="col-sm-8 col-sm-offset-2 col-xs-12 col-block">
                    <%
                        if (String.IsNullOrEmpty(Model.Message))
                        {
                            %>
                            <div class="form-group">
                                <%=Html.LabelFor(register => register.Email)%>
                                <%= Html.TextBoxFor(register => register.Email, new { 
                                    @placeholder = Resources.SignUp1_Emailtype, 
                                    @required="required", 
                                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
                                    @oninput="this.setCustomValidity('');",
                                    @type = "email",
                                    @class="form-control" })
                                %>
                            <p class="help-block"><%=Resources.SignUp1_Emailtip %></p>
                            </div>
                            <p>
                                <button class="btn btn-bky-primary" type="submit"><%=Resources.Button_Next %></button>
                            </p>
                            <% 
                        }
                        else
                        {
                            %>
                            <div class="alert alert-bky-info " role="alert">
                                <span class="entypo-thumbs-up"></span>
                                <%=Model.Message %>
                            </div>
                            <%  
                        } // if Model.Message
                    %>
                </div><!--// #basicClose.col-xs-12.col-block" -->
            </div><!--// .row  FORM -->
        </div>
        <%
    } //Html.BeginForm
%>
</asp:Content>