<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.RegistrationModelSignUpStep2>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Resources.RegistrationForm %> - <%=Resources.SignUp3_Passwmsg%>
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
    <h6><%=Resources.RegistrationForm %> - <%=Resources.SignUp3_Passwmsg%>
    </h6>
</div>
--%>

<% 
    using (Html.BeginForm("Step2", "Registration", new { ReturnUrl = ViewBag.ReturnUrl }, FormMethod.Post, new { @role = "form" }))
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
                            }
                        }
                    %>
                </div><!--// .col-xs-12 -->
            </div><!--// .row -->
            <%--    STEPS  --%>
            <div class="row">
                <div class="col-xs-12 col-block">
                    <ul class="steps-dots">
                        <li class="step done">
                            <span class="number">1.</span> 
                            <span class="step-desc"><%=Resources.SignUp1_Emailmsg %></span>
                        </li>
                        <!-- .step.step-arrow -->
                        <li class="step step-arrow">
                            <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                        </li>                        
                        <li class="step current">
                            <span class="number">2.</span>
                            <span class="step-desc"><%=Resources.SignUp3_Passwmsg%> </span>
                        </li>
                    </ul>
                </div>
            </div><!--// .row -->
            <!-- FORM -->
            <div class="row">
                <div  id="basicClose" class="col-sm-8 col-sm-offset-2 col-xs-12 col-block">
                    <div class="form-group">
                        <%=Html.LabelFor(register => register.Password)%>
                        <%= Html.PasswordFor(register => register.Password, new {
                            @placeholder = string.Format(Resources.SignUp3_Passwtype,5,50),
                            @class="form-control"})%>
                        <p class="help-block"><%=Resources.SignUp3_Passwtip %></p>
                    </div><!--//.form-group-->
                    <div class="form-group">
                        <%=Html.LabelFor(cust => cust.Country)%>
                        <%= Html.DropDownListFor(x => x.Country, new SelectList(Model.ListCountries, "Id", "Description",Model.Country), new Dictionary<string, Object> { { "class", "form-control" },{"onchange", "this.form.submit()"}})%>
                    </div><!--//.form-group-->
                    <div class="form-group">
                        <table>
                            <%
                                foreach (integraMobile.Models.CheckBoxListQuestionsModel ocheck in Model.ListQuestions) 
                                {
                                    %>
                                    <tr>
                                        <td>
                                            <input type="checkbox" name="SelectedQuestions" value="<%= ocheck.Id %>" > <%= ocheck.QuestionNameHTML %>  
                                        </td>
                                    </tr>
                                    <% 
                                }
                            %>
                        </table>
                    </div><!--//.form-group-->
                    <p>
                        <input id="utcoffset" name="utcoffset" type ="hidden" value="">
                        <button class="btn btn-bky-primary" type="submit" name="button" value="buttonNextStep2" id="buttonNextStep2"><%=Resources.Button_Next%> </button>
                    </p>

                </div>
            </div>
        </div>
        <% 
    } 
%>
</asp:Content>