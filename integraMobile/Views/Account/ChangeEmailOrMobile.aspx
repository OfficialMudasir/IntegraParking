<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ChangeEmailOrMobileModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.UserData_ChangeTelOrEmailTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.UserData_ChangeTelOrEmailTitle%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

<%--
<div id="breadcrumb-wrapper" class="row">

    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Main", "Account") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="UserData" title="<%=Resources.Account_Main_BttnUserData%>"><%=Resources.Account_Main_BttnUserData%></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.UserData_ChangeTelOrEmailTitle %>
        </li>
    </ul>
--%>

<%--
    <div class="title-alt">
        <h6>
            <%=Resources.UserData_ChangeTelOrEmailTitle %></h6>

    </div>

--%>
<% using (Html.BeginForm("ChangeEmailOrMobile", "Account", FormMethod.Post, new { @role="form"}))
{ 
%>
<div class="content-wrap">
<!-- ROW ALERTS -->
<div class="row">
    <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2">
        <%
        foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
            foreach (ModelError modelError in keyValuePair.Value.Errors) {
            %>
                <div class="alert alert-bky-danger">
                    <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                    <span class="bky-cancel mx-2"></span> 
                    &nbsp; 
                    <%= Html.Encode(modelError.ErrorMessage) %>
                </div>
            <%
            } //  modelError
        }
        %>    
    </div><!-- // .col ALERTS -->
</div><!-- // .row ALERTS -->
    <div class="row">
        <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-block ">

                    <div class="form-group">
                        <%=Html.LabelFor(cust => cust.Email)%>
                        <%= Html.TextBoxFor(cust => cust.Email, new {
                            @placeholder = Resources.CustomerInscriptionModel_WriteYourEmail, 
                            @class = "form-control",
                            @type = "email",
                            @required="required", 
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
                            @oninput="this.setCustomValidity('');" 
                        })%>
                        <p class="help-block"><%=Resources.ChangeEmailOrMobile_Remarks%></p>
                    </div>
                    <hr class="separator-block">
                    <div class="row"><!-- phone //-->
                        <div class="form-group col-sm-3 col-xs-12">
                            <%=Html.LabelFor(cust => cust.MainPhoneNumber)%>
                            <select name="MainPhoneNumberPrefixDis" class="form-control">
                                <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "", (String)ViewData["SelectedMainPhoneNumberPrefix"])%>
                            </select>
                        </div>
                        <div class="form-group col-sm-9 col-xs-12">
                            <%=Html.LabelFor(cust => cust.MainPhoneNumber, new {@style="visibility: hidden;", @class="hidden-xs"})%>
                            <%= Html.TextBoxFor(cust => cust.MainPhoneNumber, new {
                                @placeholder=Resources.CustomerInscriptionModel_WriteYourMainPhoneNumber,
                                @class="form-control",
                                @type="tel",
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_MainPhoneNumber)+"');", 
                                @oninput="this.setCustomValidity('');"
                            })%>
                        </div>
                    </div><!--// .row phone -->
                    <!-- Add the extra clearfix for only the required viewport -->
                    <div class="clearfix hidden-xs"></div>
                    <hr class="separator-block">
                    <div class="form-group">
                        <%=Html.LabelFor(cust => cust.CurrentPassword)%>
                        <%= Html.PasswordFor(cust => cust.CurrentPassword, new {
                            @placeholder = string.Format(Resources.UserData_WriteYourCurrentPassword, 4, 50), 
                            @class = "form-control",
                            @required="required", 
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.UserData_CurrentPassword)+"');", 
                            @oninput="this.setCustomValidity('');" 
                        })%>
                    </div>
                    
                    <hr class="separator-block">

                    <div class="row-buttons">
                        <button class="btn btn-bky-primary" type="submit"><span class="bky-done"></span> &nbsp; <%=Resources.UserData_ButtonConfirm%></button>
                        <input type="hidden"  id="MainPhoneNumberPrefix"  name="MainPhoneNumberPrefix" value="<%=(String)ViewData["SelectedMainPhoneNumberPrefix"] %>" />
                    </div>





        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->
<% } // FORM Html.BeginForm.ChangeEmailOrMobile %>
</asp:Content>