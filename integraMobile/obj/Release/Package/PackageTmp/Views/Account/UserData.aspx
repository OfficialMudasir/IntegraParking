<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.UserDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="integraMobile.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%-- <%=Resources.PersonalData %> --%>
    <%=Resources.Account_Main_BttnUserData%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<link href="<%= Url.Content("~/Content/css/profile.css") %>" rel="stylesheet" type="text/css">--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_Main_BttnUserData%>
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
        <li><%=Resources.PersonalData %>
        </li>
    </ul>
</div>


<div class="title-alt">
    <h6><%=Resources.UserData_UserDataTittle %></h6>
</div>
--%>


<%  
    USER oUser = (USER)ViewData["oUser"];
    NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = ".";
%>

<% 
using (Html.BeginForm("UserData", "Account", FormMethod.Post, new {@class="form-user"}))
{ 
    %>  
        <div style="overflow:hidden; height: 0;background: transparent;" data-description="dummyPanel for Chrome auto-fill issue">
                <input type="text" style="height:0;background: transparent; color: transparent;border: none;" data-description="my-username" id="my-username" name="my-username" placeholder="Write your username" />
                <input type="password" style="height:0;background: transparent; color: transparent;border: none;" data-description="Current Password" id="MyCurrentPassword" name="MyCurrentPassword" placeholder="Write your Password" />
        </div>
        <div class="content-wrap">

            <%-- ROW ALERTS //  --%>
            <div id="content-alerts" class="row">
                <div class="col-md-8 col-md-offset-2 col-sm-12">
                    <%
                        foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                        {
                            foreach (ModelError modelError in keyValuePair.Value.Errors) 
                            {
                                %>
                                <div class="alert alert-bky-warning">
                                    <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                    <span class="bky-cancel"></span> &nbsp; <%= Html.Encode(modelError.ErrorMessage) %>
                                </div>
                                <%
                            } // ModelError
                        } // ModelState
                    %>
                </div><!--// .col-block -->
            </div><!-- //   #content-alerts.row -->
            <%-- // ROW ALERTS --%>

            
            
            <%-- ROW CONTENT //  --%>
            <div class="row">
                <div class="col-md-6 col-block">

                    <table class="table">
                        <thead>
                            <tr>
                                <th class="text-left lead" colspan="2">
                                    <span class="bky-profile"></span> &nbsp; <%= Resources.Account_Profile %>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row" class="text-left">
                                    <%= Resources.Account_CurrentBalance %>
                                </th>
                                <td class="text-right">
                                    <strong class="text-success"><%=string.Format(provider, "{0:0.00} {1}", Convert.ToDouble(oUser.USR_BALANCE)/100.0,ViewData["CurrencyISOCode"]) %></strong>
                                </td>
                            </tr>
                            <tr>
                                <th scope="row" class="text-left">
                                    <%= Resources.CustomerInscriptionModel_SuscriptionType %>
                                </th>
                                <td class="text-right">
                                    <%
                                        if (Convert.ToInt32(ViewData["SuscriptionType"])==(int)PaymentSuscryptionType.pstPrepay) {
                                    %>
                                        <%=Resources.SelectPayMethod_SuscriptionType_Prepay %>
                                    <% } else { %>
                                        <%=Resources.SelectPayMethod_SuscriptionType_PerTransaction %>
                                    <% } %>                    
                                </td>
                            </tr>
                            <tr>
                                <th class="text-left"><%= Resources.CustomerInscriptionModel_PaymentMean %></th>
                                <td class="text-right">
                                    <%
                                        if (Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtDebitCreditCard) {
                                    %>
                                        <%=Resources.SelectPayMethod_CreditCard %>
                                    <% } else { %>
                                        <%=Resources.SelectPayMethod_Paypal %>
                                    <% } %>                    
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div><!-- // .col-md-6 profile-settings -->
                <!-- profile name col   -->
                <div class="col-md-6 profile-name col-block">
                    <table class="table">
                        <thead>
                            <tr>
                                <th colspan="2" class="text-right lead">
                                    <%=ViewData["UserNameAndSurname"]%>&nbsp;
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row" class="text-left"><%=Resources.CustomerInscriptionModel_Username %></th>
                                <td class="text-right">
                                    <% if (ViewData["UsernameEqualsEmail"].ToString()!="1") { %>
                                        <%= Html.DisplayFor(cust => cust.Username) %>
                                    <% } else { %>
                                        <%= Html.DisplayFor(cust => cust.Email) %>
                                    <% } %>                
                                </td>
                            </tr>
                            <tr>
                                <th scope="row" class="text-left"><%= Html.DisplayNameFor(cust => cust.Email) %></th>
                                <td class="text-right"><%= Html.DisplayFor(cust => cust.Email) %></td>
                            </tr>
                            <tr>
                                <th scope="row" class="text-left"></th>
                                <td class="text-right"></td>
                            </tr>
                        </tbody>
                    </table>    	
                </div><!-- // .col-md-6 -->
            </div><!--// row -->   
            <%-- // ROW CONTENT  --%>

            <%-- // HIDDEN OPTIONS // ---
            <div class="row">
                <div class="col-md-12 divider text-center">

                    <div class="col-md-4 emphasis">
                        <h2><span class="entypo-credit-card"></span> </h2>    
                        <p><%=Resources.UserData_PayMeansTitle %></p>
                        <a href="<%=Url.Action("SelectPayMethod", "Account",new {bForceChange = true})%>" class="btn btn-success">
                        <%=Resources.UserData_InitProcedure %></a>
                    </div>
                    <div class="col-md-4 emphasis">
                        <h2><span class="icon icon-mobile-portrait"></span></h2> 
                        <p><%=Resources.UserData_ChangeTelOrEmailTitle %></p>
                        <a href="<%=Url.Action("ChangeEmailOrMobile", "Account", null)%>" class="btn btn-info">
                        <%=Resources.UserData_InitProcedure %></a>
                    </div>
                    <div class="col-md-4 emphasis">
                        <h2><span class="bky-cancel"></span> </h2> 
                        <p><%=Resources.UserData_RemoveUserAccount %></p>
                        <a href="<%=Url.Action("DeleteUser", "Account", null)%>" class="btn btn-default"> 
                        <%=Resources.UserData_InitProcedure %></a>
                    </div>
                </div>
            </div>
            /// HIDDEN OPTIONS  //  --%>


            <!-- ROW DATOS USUARIO && CAMBIO CONTRASEÑA -->
            <div class="row">
                
                <!-- COL LEFT - USER DATA .col -->
                <div class="col-md-6 col-block">

                    <p class="lead underline">
                        <span class="bky-profile"></span> &nbsp; <%=Resources.UserData_UserDataTittle %>
                    </p>

                    <div id="name-user">
                        <% if (ViewData["UsernameEqualsEmail"].ToString()!="1") { %>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Username)%>
                            <%= Html.TextBoxFor(cust => cust.Username, new {
                                @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourUsername, 4, 50), 
                                @class = "form-control",
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Username)+"');", 
                                @oninput="this.setCustomValidity('');" 
                            })%>
                        </div>
                        <% } else { %>
                        <%= Html.HiddenFor(cust => cust.Username)%>
                        <% } %>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Name)%>
                            <%= Html.TextBoxFor(cust => cust.Name, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourName, 
                                @class = "form-control",
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Name)+"');", 
                                @oninput="this.setCustomValidity('');" 
                            })%>
                        </div>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Surname1)%>
                            <%= Html.TextBoxFor(cust => cust.Surname1, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourFirstSurname, 
                                @class = "form-control",
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_FirstSurname)+"');", 
                                @oninput="this.setCustomValidity('');" 
                            })%>
                        </div>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Surname2)%>
                            <%= Html.TextBoxFor(cust => cust.Surname2, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourSecondSurname, 
                                @class = "form-control" 
                            })%>
                        </div>
                        <div class="form-group col-sm-4 col-xs-12">
                            <%=Html.LabelFor(cust => cust.AlternativePhoneNumber)%>
                            <select name="AlternativePhoneNumberPrefix" id="AlternativePhoneNumberPrefix" class="form-control">
                                <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "", (String)ViewData["SelectedAlternativePhoneNumberPrefix"])%>           
                            </select>
                        </div>
                        <div class="form-group col-sm-8 col-xs-12">
                            <%=Html.LabelFor(cust => cust.AlternativePhoneNumber, new {@style="visibility: hidden;"})%>
                            <%= Html.TextBoxFor(cust => cust.AlternativePhoneNumber, new {
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourAlternativePhoneNumber,
                                @class = "form-control",
                                @type = "tel"
                            })%>
                        </div>
                        <p class="help-block"><%=Resources.CustomerInscriptionModel_Remark_ivr%></p>
                    </div><!-- // #name-user -->
                </div><!-- USER DATA .col-block -->
                
                <!-- COL RIGHT - PASSWORD CHANGE .col -->
                <div class="col-md-6 col-block">
                    
                    <p class="lead underline">
                        <span class="bky-password"></span> &nbsp; <%= Resources.UserData_ChangePassword%>
                    </p>

                    <div id="change-password">
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.NewPassword)%>
                            <%= Html.PasswordFor(cust => cust.NewPassword, new { 
                                @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourPassword, 4, 50), 
                                @class = "form-control" 
                            })%>
                            <div class="help-block"><%=Resources.UserData_PasswordRemark%></div>
                        </div>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.ConfirmNewPassword)%>
                            <%= Html.PasswordFor(cust => cust.ConfirmNewPassword, new { 
                                @placeholder = string.Format(Resources.UserData_WriteYourConfirmPassword, 4, 50), 
                                @class = "form-control" 
                            })%>
                        </div>
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
                    </div><!--// #change-password   -->

                </div><!-- PASSWORD CHANGE .col-block -->

            </div>
            <!--// ROW DATOS USUARIO && CAMBIO CONTRASEÑA -->

            <%-- ROW INVOICE & OTHER DATA // --%>
            <div class="row">

                <%-- COL LEFT - INVOICE DATA .col --%>
                <div class="col-md-6 col-block">
                    <p class="lead underline">
                        <span class="bky-billing"></span> &nbsp; <%= Resources.UserData_Billing%>
                    </p>
                    <div id="invoice-data">
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.DocId)%>
                            <%= Html.TextBoxFor(cust => cust.DocId, new { 
                                @placeholder = Resources.CustomerInscriptionModel_WriteYourDocID, 
                                @class = "form-control",
                                @required="required", 
                                @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_DocID)+"');", 
                                @oninput="this.setCustomValidity('');" 
                            })%>
                        </div>
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Country)%>
                            <select name="Country" id="Country" class="form-control">
                                <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "", (String)ViewData["SelectedCountry"])%>
                            </select>
                        </div>

                        <%----%>

                        <!--    CITY DATA // -->
                        <div id="city-data" class="row">
                            <div class="form-group col-lg-4 col-md-12">
                                <%=Html.LabelFor(cust => cust.State)%>
                                <%= Html.TextBoxFor(cust => cust.State, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourState, 
                                    @class = "form-control",
                                    @required="required", 
                                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_State)+"');", 
                                    @oninput="this.setCustomValidity('');"
                                })%>
                            </div>
                            <div class="form-group col-lg-4 col-md-12">
                                <%=Html.LabelFor(cust => cust.City)%>
                                <%= Html.TextBoxFor(cust => cust.City, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourCity, 
                                    @class = "form-control" ,
                                    @required="required", 
                                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_City)+"');", 
                                    @oninput="this.setCustomValidity('');"
                                })%>
                            </div>
                            <div class="form-group col-lg-4 col-md-12">
                                <%=Html.LabelFor(cust => cust.ZipCode)%>
                                <%= Html.TextBoxFor(cust => cust.ZipCode, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourZipCode, 
                                    @class = "form-control",
                                    @required="required", 
                                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_ZipCode)+"');", 
                                    @oninput="this.setCustomValidity('');" 
                                })%>
                            </div>
                        </div><!--// CITY DATA #city-data.row -->
                        
                        <!-- #STREET-DATA -->
                        <div id="street-data" class="row">
                            <div class="form-group col-md-9 col-sm-12">
                                <%=Html.LabelFor(cust => cust.StreetName)%>
                                <%= Html.TextBoxFor(cust => cust.StreetName, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetName, 
                                    @class = "form-control",
                                    @required="required", 
                                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetName)+"');", 
                                    @oninput="this.setCustomValidity('');" 
                                })%>
                            </div>
                            <div class="form-group col-md-3 col-sm-12">
                                <%=Html.LabelFor(cust => cust.StreetNumber)%>
                                <%= Html.TextBoxFor(cust => cust.StreetNumber, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetNumber, 
                                    @class = "form-control",
                                    @required="required", 
                                    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetNumber)+"');", 
                                    @oninput="this.setCustomValidity('');" 
                                })%>
                            </div>
                        </div><!--// #street-data.row -->
                        
                        <!-- street-location-data-->
                        <div id="street-location-data" class="row">
                            <div class="form-group col-lg-3 col-sm-6">
                                <%=Html.LabelFor(cust => cust.LetterInStreetNumber)%>
                                <%= Html.TextBoxFor(cust => cust.LetterInStreetNumber, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourLetterInStreetNumber, 
                                    @class = "form-control" 
                                })%>
                            </div>
                            <div class="form-group col-lg-3 col-sm-6">
                                <%=Html.LabelFor(cust => cust.LevelInStreetNumber)%>
                                <%= Html.TextBoxFor(cust => cust.LevelInStreetNumber, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourLevelInStreetNumber, 
                                    @class = "form-control" 
                                })%>
                            </div>
                            <div class="form-group col-lg-3 col-sm-6">
                                <%=Html.LabelFor(cust => cust.DoorInStreetNumber)%>
                                <%= Html.TextBoxFor(cust => cust.DoorInStreetNumber, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourDoorInStreetNumber, 
                                    @class = "form-control" 
                                })%>
                            </div>
                            <div class="form-group col-lg-3 col-sm-6">
                                <%=Html.LabelFor(cust => cust.StairInStreetNumber)%>
                                <%= Html.TextBoxFor(cust => cust.StairInStreetNumber, new { 
                                    @placeholder = Resources.CustomerInscriptionModel_WriteYourStairInStreetNumber, 
                                    @class = "form-control" 
                                })%>
                            </div>
                        </div><!--//    #street-location-data.row -->

                    </div><!--// #invoice-data -->

                </div><!--// col-block -->

                <%-- COL RIGHT - OTHER DATA .col --%>
                <div class="col-md-6 col-block">

                    <p class="lead underline">
                        <span class="bky-config"></span> &nbsp; <%= Resources.UserData_MyPlates%>
                    </p>

                    <div id="config-data">
                        <div class="form-group">
                            <%=Html.LabelFor(cust => cust.Plates)%>
                            <%=Html.ListBoxFor(cust => cust.Plates, Model.Plates, new {
                                @class = "form-control",
                                @style = "height: auto !important;" 
                            })%>
                        </div>
                        <div id="LicensePlate" class="form-group" style="display: none;">
                            <input id="LicensePlateInput" class="form-control" placeholder="<%= Resources.CustomerInscriptionModel_WriteYourPlate%>" type="text" value="">
                            <p class="help-block"><%=Resources.UserData_PlatesRemark%></p>
                        </div>
                        <div class="form-group row-buttons">
                            <button id="AddPlate" class="btn btn-sm btn-bky-primary" onclick="EnableAddPlate();" type="button"><span class="bky-car-add"></span> &nbsp; <%=Resources.UserData_InsertNewPlate%></button>
                            <button id="RemovePlate" class="btn btn-sm btn-bky-sec-danger" onclick="removeOptionSelected();" type="button"><span class="bky-car-delete"></span> &nbsp; <%=Resources.UserData_RemovePlate%></button>
                            <button id="ConfirmPlate" class="btn btn-sm btn-bky-success" style="display: none;" onclick="ConfirmAddPlate();" type="button"><span class="bky-car-select"></span> &nbsp; <%=Resources.UserData_ConfirmAdd%></button>
                            <button id="CancelPlate" class="btn btn-sm btn-bky-sec-secondary" style="display: none;" onclick="CancelAddPlate();" type="button"><span class="bky-cancel"></span> &nbsp; <%=Resources.UserData_CancelAdd%></button>
                        </div>
                    </div><!--// #config-data -->        
                </div><!--// .col-block -->

            </div><!--// .row -->

            <hr>

            <%-- ROW BUTTONS // --%>
            <div class="row">
                <div class="col-md-12 col-block">
                    <div class="form-group row-buttons">
                        <button class="btn btn-bky-primary"  type="submit"> <%=Resources.UserData_ButtonConfirm%> </button>
                        &nbsp; 
                        <a href="UserData" class="btn btn-bky-sec-default"><%=Resources.UserData_ButtonCancel%></a>
                    </div>
                </div>
                <!-- hidden fields -->
                <div style="display:none;">
                    <%= Html.HiddenFor(cust => cust.Email)%>
                    <%= Html.HiddenFor(cust => cust.MainPhoneNumber)%>
                    <input type="hidden" value="<%=Model.platesChanges%>"  id="platesChanges"  name="platesChanges">    
                    <input type="hidden"  id="MainPhoneNumberPrefix"  name="MainPhoneNumberPrefix" value="<%=(String)ViewData["SelectedMainPhoneNumberPrefix"] %>">
                </div>
            </div><!--// .row -->
            <%-- // ROW BUTTONS  --%>

        </div>
    <% 
} //  Html.BeginForm
%>
<script type="text/javascript">

    $(document).ready(function (e) {
        try {
            $("body select").msDropDown();
        } catch (e) {
            //alert(e.message);
        }
    });

    var previousPlateValue = '';
    var platePattern = /^[A-Z0-9]+$/;
    var maxPlateLength = 50;

    function EnableAddPlate() {
        var AddPlate = document.getElementById('AddPlate');
        var RemovePlate = document.getElementById('RemovePlate');
        var LicensePlateInput = document.getElementById('LicensePlateInput');
        DisplayAddPlateElements();
        AddPlate.style.display = 'none';
        RemovePlate.style.display = 'none';
        LicensePlateInput.value = '';
        previousPlateValue = '';
        LicensePlate.onkeyup = ValidateInputPlate;
    }

    function ConfirmAddPlate() {
        var LicensePlateInput = document.getElementById('LicensePlateInput');
        var AddPlate = document.getElementById('AddPlate');
        if (LicensePlateInput.value.length > 0) {
            appendOptionLast(LicensePlateInput.value);
            HideAddPlateElements();
            AddPlate.style.display = 'inline-block';
            RemovePlate.style.display = 'inline-block';
            alert('<%=Resources.Account_UserData_Plate_Succesfully_Added%>');
        }

    }

    function CancelAddPlate() {
        var AddPlate = document.getElementById('AddPlate');
        var RemovePlate = document.getElementById('RemovePlate');
        HideAddPlateElements();
        AddPlate.style.display = 'inline-block';
        RemovePlate.style.display = 'inline-block';
    }

    function DisplayAddPlateElements() {
        var LicensePlate = document.getElementById('LicensePlate');
        var ConfirmPlate = document.getElementById('ConfirmPlate');
        var CancelPlate = document.getElementById('CancelPlate');
        LicensePlate.style.display = 'block';
        ConfirmPlate.style.display = 'inline-block';
        CancelPlate.style.display = 'inline-block';
    }
    function HideAddPlateElements() {
        var LicensePlate = document.getElementById('LicensePlate');
        var ConfirmPlate = document.getElementById('ConfirmPlate');
        var CancelPlate = document.getElementById('CancelPlate');
        LicensePlate.style.display = 'none';
        ConfirmPlate.style.display = 'none';
        CancelPlate.style.display = 'none';
    }


    function ValidateInputPlate(event) {

        event = event || window.event;
        var newValue = event.target.value || '';
        newValue = newValue.toUpperCase();

        if (newValue.length > maxPlateLength) {
            event.target.value = previousPlateValue;
        }
        else {
            if ((newValue.match(platePattern)) || (newValue.length == 0)) {
                // Valid input; update previousValue:
                previousPlateValue = newValue;
                event.target.value = newValue;
            } else {
                // Invalid input; reset field value:
                event.target.value = previousPlateValue;
            }
        }
    }

    function removeOptionSelected() {
        var elSel = document.getElementById('Plates');
        var i;
        for (i = elSel.length - 1; i >= 0; i--) {
            if (elSel.options[i].selected) {
                document.getElementById('platesChanges').value = document.getElementById('platesChanges').value + '#D:' + elSel.options[i].text;
                elSel.remove(i);

            }
        }
        // TGA@BKY :: Falta avisar al usuruario para condfirmar formulario !!!!
        alert('[[<%=Resources.UserData_ButtonConfirm%>]]');
    }

    function appendOptionLast(plate) {
        var elOptNew = document.createElement('option');
        elOptNew.text = plate;
        elOptNew.value = -1;
        var elSel = document.getElementById('Plates');
        var bPlateExist = false;

        for (i = elSel.length - 1; i >= 0; i--) {
            if (elSel.options[i].text == plate) {
                bPlateExist = true;
                break;
            }
        }

        if (bPlateExist == false) {
            try {
                elSel.add(elOptNew, null); // standards compliant; doesn't work in IE
            }
            catch (ex) {
                elSel.add(elOptNew); // IE only
            }

            document.getElementById('platesChanges').value = document.getElementById('platesChanges').value + '#I:' + plate;
        }
    }

</script>

</asp:Content>