<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
	<%-- <%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%> --%>
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
    <h6><%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%>
    </h6>
</div>
--%>

<div class="content-wrap">

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

                <li class="step done">
                    <span class="number">2.</span> 
                    <span class="step-desc"><%=Resources.RegistrationForm_Step1End_IdentityValidation_Step1%></span>
                </li>

                <!-- .step.step-arrow -->
                <li class="step step-arrow">
                    <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                </li>                

                <li class="step current">
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
        </div><!--//.col-block STEPS -->
    </div><!--//.row STEPS -->

    <%-- ROW ALERTS & FORMS // --%>
    <div class="row">
        <div class="col-sm-12 col-block">
            
            <h3><%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%></h3>

            <% 
                if ((!Convert.ToBoolean(ViewData["CodeExpired"])) && (!Convert.ToBoolean(ViewData["CodeAlreadyUsed"])))
                {
                    if (string.IsNullOrEmpty((String)ViewData["ActivationRetries"])) 
                    {%><%}else{
                        %>
                            <div class="alert alert-bky-info">
                                <button data-dismiss="alert" aria-label="Close" class="close" type="button"><span class="bky-delete"></span></button>
                                <span class="bky-info"></span>
                                &nbsp;
                                <%=(String)ViewData["ActivationRetries"] %>
                            </div><!--//.alert.alert-bky-info -->
                        <%
                    } // if else ActivationRetries
                    %>

                        <div class="alert alert-bky-warning">

                            <% 
                                using (Html.BeginForm("Step2", "IndividualsRegistration", FormMethod.Post, new { @role="form"}))
                                { 
                                    %>
                                        <div class="form-group">
                                            <span class="bky-info"></span> 
                                            &nbsp;
                                            <%=String.Format(Resources.CustomerInscriptionModel_Step2SendSMS, 
                                            (String)ViewData["EndSufixMainPhone"])%>
                                        </div>
                                        <div class="row-buttons">
                                            <button class="btn btn-bky-warning" type="submit"><%=Resources.Button_ResendSMS%></button>
                                            <input type="hidden" name="type" value="resendsms">
                                        </div><!--//.row-buttons-->
                                    <% 
                                } // Html.BeginForm
                            %>
                        </div><!--//.alert.alert-warning-->

                    <% 
                        using (Html.BeginForm("Step2", "IndividualsRegistration", FormMethod.Post, new { @role="form"})) 
                        {
                            %>
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
                                    } // foreach Errors
                                } // foreach ModelState
                            %>
                                <div class="form-group">
                                    <label for="confirmationcode" class="sr-only"><%=Resources.CustomerInscriptionModel_Step2WriteYourCode%></label>
                                    <input type="number" oninvalid="this.setCustomValidity('<%=string.Format(Resources.ErrorsMsg_RequiredField,"")%>');", oninput="this.setCustomValidity('');" required="required" id="confirmationcode" name="confirmationcode" class="form-control">
                                    <p class="help-block"><%=String.Format(Resources.CustomerInscriptionModel_Step2WriteYourCode1,ConfigurationManager.AppSettings["NumCharactersActivationSMS"], (String)ViewData["EndSufixMainPhone"], (int)ViewData["NumMinutesTimeoutActivationSMS"])%></p>
                                </div><!--//.form-group-->
                                <div class="row-buttons">
                                    <button class="btn btn-bky-primary" type="submit"><%=Resources.Button_Validate%></button>
                                    <input type="hidden" name="type" value="confirmcode">
                                </div><!--//.row-buttons-->
                            <% 
                        } // Html.BeginForm Step2
                    %>
                    <%
                } 
                else 
                { 
                    %>

                        <div class="alert alert-bky-danger">
                            <% 
                                using (Html.BeginForm("Step1", "IndividualsRegistration", FormMethod.Get, new {@role="form"})) 
                                { 
                                    %>
                                        <div class="form-group">
                                            <span class="bky-cancel"></span> 
                                            &nbsp;
                                            <%= Html.ValidationMessage("CodeExpired")%> 
                                            &nbsp; 
                                            <%= Html.ValidationMessage("CodeAlreadyUsed")%>
                                        </div>
                                        <div class="row-buttons">
                                            <button class="btn btn-bky-success" type="submit"><%=Resources.Button_ReInitRegistration%></button>
                                        </div>
                                    <% 
                                }  // Html.BeginForm Step1
                            %>
                        </div><!--//.alert.alert-bky-danger-->
                    <% 
                } // if else CodeExpired && CodeAlreadyUsed
            %>
        </div><!--//.col-block FORMS -->
    </div><!--//.row ALERTS & FORMS-->

</div><!--//.content-wrap-->

</asp:Content>