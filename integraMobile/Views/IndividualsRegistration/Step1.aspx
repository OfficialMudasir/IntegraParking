<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.CustomerInscriptionModelStep1>" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%-- <%=Resources.RegistrationForm %> - <%=Resources.PersonalData %> --%>
    <%=Resources.RegistrationForm %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
     <%-- <%=Resources.PersonalData %> --%>
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
        <%=Resources.RegistrationForm %> - <%=Resources.PersonalData %>
    </h6>
</div>
--%>
<% 
    using (Html.BeginForm("Step1", "IndividualsRegistration", FormMethod.Post, new { @role="form"})) 
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
                </div><!-- // .row -->

                <%-- ROW STEPS // --%>
                <div class="row">
                    <div class="col-xs-12 col-block">
                        <ul class="steps-dots">
                            <li class="step current">
                                <span class="number">1.</span>  
                                <span class="step-desc"><%=Resources.PersonalData %></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step disabled">
                                <span class="number">2.</span> 
                                <span class="step-desc"><%=Resources.RegistrationForm_Step1End_IdentityValidation_Step1%></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step disabled">
                                    <span class="number">3.</span> 
                                    <span class="step-desc"><%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step disabled">
                                <span class="number">4.</span> 
                                <span class="step-desc"><%=Resources.CustomerInscriptionModel_Step3Address%></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step disabled">
                                <span class="number">5.</span> 
                                <span class="step-desc"><%=Resources.CustomerInscriptionModel_Step4UseConditions%></span>
                            </li>
                        </ul>
                    </div><!--//.col-block-->
                </div><!--//.row-->

                <%--    FORM    --%>
                <div class="row" id="basicClose">
                    <div class="col-sm-8 col-sm-offset-2 col-xs-12 col-block" id="basic">

                        <h3><%=Resources.PersonalData %> </h3>

                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Email) %>
                            <%= Html.TextBoxFor(cust => cust.Email, new { 
                                @placeholder = Resources.SignUp1_Emailtype, 
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @type = "email",
                                @class="form-control" })
                            %>
                            <% 
                                if (ViewData["UsernameEqualsEmail"].ToString()=="1")
                                { 
                                    %>
                                        <p class="help-block"><%=Resources.CustomerInscriptionModel_Step1RememberUsernameEqualsEmail %></p>
                                    <% 
                                } 
                            %>
                        </div>

                        <div class="row-buttons">
                            <button class="btn btn-bky-primary" type="submit"><%=Resources.Button_Next %></button>
                        </div>
                        
                    </div><!--//.col-block-->
                </div><!--//.row FORM -->

            </div><!--//.content-wrap-->
        <% 
    } // Html.BeginForm
%>
</asp:Content>