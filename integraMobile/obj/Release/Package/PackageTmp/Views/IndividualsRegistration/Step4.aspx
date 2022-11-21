<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.CustomerInscriptionModelStep4>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step4UserInformation%>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step4UserInformation%> --%>
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
        <%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step4UserInformation%>
    </h6>
</div>
--%>

<% 
    using (Html.BeginForm("Step4", "IndividualsRegistration", FormMethod.Post, new { @role="form"})) 
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
                                                        <button data-dismiss="alert" aria-label="Close" class="close" type="button"><span class="bky-delete"></span></button>
                                                        <span class="bky-cancel"></span>
                                                        &nbsp;
                                                        <%= Html.Encode(modelError.ErrorMessage) %>
                                                    </div>
                                                <%
                                            } // foreach ModelError
                                        } // foreach ModelState
                                    %>
                    </div><!--// .col-xs-12 -->
                </div><!--// .row -->

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

                            <li class="step done">
                                <span class="number">3.</span> 
                                <span class="step-desc"><%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step done">
                                <span class="number">4.</span> 
                                <span class="step-desc"><%=Resources.CustomerInscriptionModel_Step3Address%></span>
                            </li>

                            <!-- .step.step-arrow -->
                            <li class="step step-arrow">
                                <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                            </li>                

                            <li class="step current">
                                <span class="number">5.</span> 
                                <span class="step-desc"><%=Resources.CustomerInscriptionModel_Step4UseConditions%></span>
                            </li>
                        </ul>
                    </div><!--//.col-block STEPS -->
                </div><!--//.row STEPS -->

                <!-- ROW FORM -->
                <div class="row">
                    <div class="col-sm-12">
                        <h3><%=Resources.CustomerInscriptionModel_Step4UserInformation%></h3>

                        <div class="form-group">
                            <% 
                                if (ViewData["UsernameEqualsEmail"].ToString()=="1")
                                { 
                                    %>
                                        <%= Html.LabelFor(cust => cust.Email) %>
                                        <%= Html.TextBoxFor(cust => cust.Email, new {
                                            @value = string.Format(Resources.CustomerInscriptionModel_WriteYourUsername,4,50),
                                            @readonly="true",
                                            @class="form-control" })%>
                                        <p class="help-block"><%=Resources.CustomerInscriptionModel_Step1RememberUsernameEqualsEmail %></p>
                                        <%= Html.HiddenFor(cust => cust.Username)%>
                                    <% 
                                }
                                else
                                { 
                                    %>
                                        <%= Html.LabelFor(cust => cust.Username) %>
                                        <%= Html.TextBoxFor(cust => cust.Username, new {
                                            @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourUsername,4,50),
                                            @required="required",
                                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Username)+"');", 
                                            @oninput="this.setCustomValidity('');",
                                            @class="form-control" })%>
                                        <%= Html.HiddenFor(cust => cust.Email)%>
                                    <% 
                                } 
                            %>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.Password) %>
                            <%= Html.PasswordFor(cust => cust.Password, new {
                                @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourPassword,4,50),
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Password)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control" })%>
                            <p class="help-block"><%=Resources.CustomerInscriptionModel_Step4SecurityAdvice %></p>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.ConfirmPassword) %>
                            <%= Html.PasswordFor(cust => cust.ConfirmPassword, new { 
                                @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourConfirmPassword,4,50),
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_ConfirmPassword)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control" })%>
                        </div>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Plate) %>
                            <%= Html.TextBoxFor(cust => cust.Plate, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourPlate,@required="true", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Plate)+"');", 
                                @oninput="this.setCustomValidity('');", 
                                @onkeyup="validateInputPlate(this.value, 'Plate')",
                                @class="form-control" })%>
                            <p class="help-block"><%=Resources.CustomerInscriptionModel_Step4PreferredPlate%></p>
                        </div>
                        <div class="checkbox">
                            <label>
                                <%: Html.CheckBoxFor(m => m.ConfirmServiceCondictions)%><%=Resources.CustomerInscriptionModel_Step4IHaveReadAndAccept%> <a href="gCond_<%=ViewData["lang_for_gCond"]%>" target="_blank"><%=Resources.CustomerInscriptionModel_Step4UseConditions%></a>
                            </label>
                        </div>

                        <div class="row-buttons">
                            <a class="btn btn-bky-sec-primary" href="<%=Url.Action("Step3", "IndividualsRegistration", null)%>" ><%=Resources.Button_Back %></a>
                            &nbsp;
                            <button class="btn btn-bky-primary" type="submit"><%=Resources.CustomerInscriptionModel_StepBttnFinish %></button>
                        </div><!--//.row-buttons-->
                        
                    </div><!--//.col-block-->
                </div><!--//.row FORM-->

            </div><!--//.content-wrap-->
        <% 
    } // Html.BeginForm Step4
%>

<script  type="text/javascript">  

    var previousPlateValue = '';
    var platePattern = /^[A-Z0-9]+$/;
    var maxPlateLength =50;

    function validateInputPlate(text, id) {

        var newValue = text;
        newValue=newValue.toUpperCase();


        if (newValue.length>maxPlateLength)
        {
            document.getElementById(id).value = previousPlateValue;
        }
        else
        {
            if ((newValue.match(platePattern)) || (newValue.length==0)) {
                // Valid input; update previousValue:
                previousPlateValue = newValue;
                document.getElementById(id).value = newValue;
            } else {
                // Invalid input; reset field value:
                document.getElementById(id).value = previousPlateValue;
            }
        }

     
    }
</script>

</asp:Content>