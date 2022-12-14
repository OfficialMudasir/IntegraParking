<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step1%>
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
        <%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step1%>
    </h6>
</div>
--%>
<% 
    using (Html.BeginForm("Step1End", "IndividualsRegistration", FormMethod.Post, FormMethod.Post, new { @role="form"})) 
    { 
        %>
            <div class="content-wrap">

                <%-- ROW ALERTS // --%>
                <div class="row">
                    <div class="col-xs-12 col-block">
                        <% 
                            if (string.IsNullOrEmpty((String)ViewData["ActivationRetries"])) 
                            {%><%}else{
                                %>
                                    <div class="alert alert-bky-info">
                                        <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                        <span class="bky-info"></span>
                                        &nbsp;
                                        <%=(String)ViewData["ActivationRetries"] %>
                                    </div>
                                <%
                            } // if ActivationRetries
                        %>
                    </div><!--// .col-block -->
                </div><!--//.row ALERTS-->

                <%-- ROW STEPS // --%>
                <div class="row">
                    <div class="col-sm-12 col-block">
                        <ul class="steps-dots">
                            <li class="step done">
                                <span class="number"> 1.</span> 
                                <span class="step-desc"><%=Resources.PersonalData %></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step current">
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
                    </div>
                </div><!--//.row STEPS -->


                <%--    FORM    --%>
                <div class="row" id="basicClose">
                    <div class="col-sm-12 col-block"  id="basic">
                        
                        <h3><%=Resources.RegistrationForm_Step1End_IdentityValidation_Step1%></h3>
                        
                        <div class="alert alert-bky-warning">
                                <div class="form-group">
                                    <span class="entypo-attention"></span> 
                                    &nbsp; 
                                    <%=Resources.CustomerInscriptionModel_Step1EndIntroText4%> 
                                    &nbsp; 
                                    <%=Resources.CustomerInscriptionModel_Step1EndIntroText5%>
                                </div>
                        </div>
                        
                        <hr>
                        
                        <ol>
                            <li><%=string.Format(Resources.CustomerInscriptionModel_Step1EndIntroText1,ViewData["maskedTelephone"]) %></li>
                            <li><%=string.Format(Resources.CustomerInscriptionModel_Step1EndIntroText2,ViewData["email"]) %></li>
                            <li><%=Resources.CustomerInscriptionModel_Step1EndIntroText3 %></li>
                        </ol>
                        
                        <hr>

                        <div class="row-buttons">
                            <button class="btn btn-bky-primary" type="submit"><%=Resources.Button_RetrySend %></button>
                        </div>

                    </div><!--//.col-block-->
                </div><!--//.row FORM -->
                
            </div><!--// .content-wrap -->
        <% 
    } 
%>

</asp:Content>