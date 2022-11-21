<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.CustomerInscriptionModelStep3>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step3Address%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step3Address%> --%>
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
        <%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step3Address%>
    </h6>
</div>    
--%>
<% 
    using (Html.BeginForm("Step3", "IndividualsRegistration", FormMethod.Post, new { @role="form"}))
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

                    </div><!--// .col-xs-12.col-block -->
                </div><!-- // .row -->

                <%-- ROW STEPS // --%>
                <div class="row">
                    <div class="col-xs-12 col-block">
                        <ul class="steps-dots">
                            <li class="step done">
                                <span class="number">1.</span>  
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

                            <li class="step current">
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

                <%-- ROW FORM --%>
                <div class="row">
                    <div class="col-sm-8 col-sm-offset-2 col-xs-12 col-block">
                        <h3><%=Resources.CustomerInscriptionModel_Step3Address%></h3>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.StreetName) %>
                            <%= Html.TextBoxFor(cust => cust.StreetName, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetName,
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetName)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control"
                            })%>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.StreetNumber) %>
                            <%= Html.TextBoxFor(cust => cust.StreetNumber, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetNumber,
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetNumber)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control",
                                @type="text"
                            })%>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.Country) %>
                            <select name="Country" id="ChooseCountry" class="form-control">
                                <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "", (String)ViewData["SelectedCountry"])%>
                            </select>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.State) %>
                            <%= Html.TextBoxFor(cust => cust.State, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourState,
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_State)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control"
                            })%>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.City) %>
                            <%= Html.TextBoxFor(cust => cust.City, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourCity,
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_City)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control" 
                            })%>
                        </div>
                        <div class="form-group">
                            <%= Html.LabelFor(cust => cust.ZipCode) %>
                            <%= Html.TextBoxFor(cust => cust.ZipCode, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourZipCode,
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_ZipCode)+"');", 
                                @oninput="this.setCustomValidity('');",
                                @class="form-control",
                                @type="number"
                            })%>
                        </div>

                        <div class="row-buttons">

                            <input name="LevelInStreetNumber" id="LevelInStreetNumber" type="hidden" value="">
                            <input name="DoorInStreetNumber" id="DoorInStreetNumber" type="hidden" value="">
                            <input name="LetterInStreetNumber" id="LetterInStreetNumber" type="hidden" value="">
                            <input name="StairInStreetNumber" id="StairInStreetNumber" type="hidden" value="">

                            <button class="btn btn-bky-primary" type="submit"><%=Resources.Button_Next %></button>
                        </div><!--//.row-buttons-->
                        
                    </div><!--//.col-block-->
                </div><!--//.row-->
            </div><!--//.content-wrap-->
        <%
    } // Html.BeginForm Step3
%>

</asp:Content>