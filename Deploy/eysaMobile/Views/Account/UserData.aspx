<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.UserDataModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    UserData
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script src="../Content/dropdown/js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../Content/dropdown/js/jquery.dd.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="../Content/dropdown/dd.css" />
<script  type="text/javascript">
    $(document).ready(function (e) {
        try {
            $("body select").msDropDown();
        } catch (e) {
            //alert(e.message);
        }
    });
    
    <!--
  
    var previousPlateValue = '';
    var platePattern = /^[A-Z0-9]+$/;
    var maxPlateLength =50;



    function removeOptionSelected()
    {
        var elSel = document.getElementById('Plates');
        var i;
        for (i = elSel.length - 1; i>=0; i--) {
            if (elSel.options[i].selected) {
                document.getElementById('platesChanges').value = document.getElementById('platesChanges').value + '#D:' + elSel.options[i].text;
                elSel.remove(i);

            }
        }
    }

    function appendOptionLast(plate)
    {
        var elOptNew = document.createElement('option');
        elOptNew.text = plate;
        elOptNew.value = -1;
        var elSel = document.getElementById('Plates');
        var bPlateExist=false;

        for (i = elSel.length - 1; i>=0; i--) {
            if (elSel.options[i].text==plate) {
                bPlateExist=true;
                break;
            }
        }


        if (bPlateExist==false)
        {
            try {
                elSel.add(elOptNew, null); // standards compliant; doesn't work in IE
            }
            catch(ex) {
                elSel.add(elOptNew); // IE only
            }

            document.getElementById('platesChanges').value = document.getElementById('platesChanges').value + '#I:' + plate;
        }

    }


    function enableAddPlate()
    {
        changeVisibilityAddElements('visible');
        var elBtn = document.getElementById('btnAddPlate');
        elBtn.style.visibility='hidden';
        var elInput = document.getElementById('newPlate');
        elInput.value='';
        previousPlateValue = '';
        elInput.onkeyup = validateInputPlate;
    }

    function confirmAdd()
    {
        var elInput = document.getElementById('newPlate');
        if (elInput.value.length>0)
        {
            appendOptionLast(elInput.value);
            changeVisibilityAddElements('hidden');
            var elBtn = document.getElementById('btnAddPlate');
            elBtn.style.visibility='visible';

        }

    }

    function cancelAdd()
    {
        changeVisibilityAddElements('hidden');
        var elBtn = document.getElementById('btnAddPlate');
        elBtn.style.visibility='visible';
    }


    function changeVisibilityAddElements(visibility)
    {
        var elInput = document.getElementById('newPlate');
        elInput.style.visibility=visibility;
        var elBtn = document.getElementById('btnConfirmAdd');
        elBtn.style.visibility=visibility;
        var elBtn2 = document.getElementById('btnCancelAdd');
        elBtn2.style.visibility=visibility;
    }




    function validateInputPlate(event) {

        event = event || window.event;
        var newValue = event.target.value || '';
        newValue=newValue.toUpperCase();

        if (newValue.length>maxPlateLength)
        {
            event.target.value = previousPlateValue;
        }
        else
        {
            if ((newValue.match(platePattern)) || (newValue.length==0)) {
                // Valid input; update previousValue:
                previousPlateValue = newValue;
                event.target.value = newValue;
            } else {
                // Invalid input; reset field value:
                event.target.value = previousPlateValue;
            }
        }

     
    }
    //-->

</script>



<div id="formulario">
<div class="greenhr"><hr /></div>
<div class=div33-left>
    <h2><%=Resources.UserData_PayMeansTitle %></h2>
	<p>
    <% if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentMeanType.pmtPaypal) { %>
        <img src="../Content/img/paypal.jpg" >
    <% } else { %>
        <img src="../Content/img/visa.jpg"/>
    <% } %>       
<input type="button" value="<%=Resources.UserData_ChangePaymentMean%>" onclick="location.href='<%=Url.Action("SelectPayMethod", "Account",new {bForceChange = true})%>'" class="botonverdecenter" />

    </p>
    <div class="error">
<p><%=Resources.UserData_PayMeansRemark%> 
    <% if ( Convert.ToInt32(ViewData["PaymentType"])==(int)PaymentSuscryptionType.pstPrepay) { %>
    <%=Resources.UserData_PayMeansRemark_Prepay%>
    <%} %></p><br /></div>
       
</div>
<div class=div33-left>
    <h2><%=Resources.UserData_ChangeTelOrEmailTitle %></h2>
	<p>
        <img src="../Content/img/tel_email.jpg" >
        <input type="button" value="<%=Resources.UserData_InitProcedure%>" onclick="location.href='<%=Url.Action("ChangeEmailOrMobile", "Account", null)%>'" class="botonverdecenter" />
    </p>
<br />
       
</div>
<div class=div33-right>
    <h2><%=Resources.UserData_RemoveUserAccount %></h2>
	<p>
        <img src="../Content/img/del_user.png" >
        <input type="button" value="<%=Resources.UserData_InitProcedure%>" onclick="location.href='<%=Url.Action("DeleteUser", "Account", null)%>'" class="botonverdecenter" />
    </p>
<br />
       
</div>
<div class="greenhr"><hr /></div>
    <h2><%=Resources.UserData_UserDataTittle %></h2>

  <% using (Html.BeginForm("UserData", "Account", FormMethod.Post))
     { %>   
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.Username)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Email)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.CurrentPassword)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.NewPassword)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.ConfirmNewPassword)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Name)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Surname1)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Surname2)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.DocId)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.MainPhoneNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AlternativePhoneNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.StreetName)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.StreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.LevelInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.DoorInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.LetterInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.StairInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.State)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.City)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.ZipCode)%></p>
<p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  

<fieldset> 

<p><br /></p>

<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Email)%></p>
  <%= Html.TextBoxFor(cust => cust.Email, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourEmail, @class = "inputelementsw100", @readonly = "readonly" })%>
</div>
<div class="div50-right">

<% if (ViewData["UsernameEqualsEmail"].ToString()!="1"){ %>
<p><%=Html.LabelFor(cust => cust.Username)%> </p>
  <%= Html.TextBoxFor(cust => cust.Username, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourUsername, 4, 50), @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Username)+"');", 
            @oninput="this.setCustomValidity('');" })%>
<%}else{ %>
    <p><br /><br /><br /></p>
    <%= Html.HiddenFor(cust => cust.Username)%>
<%} %>
  <!-- Username -->
</div> 


<div class="div33-left">
<p><%=Html.LabelFor(cust => cust.CurrentPassword)%> </p>
  <%= Html.PasswordFor(cust => cust.CurrentPassword, new { @placeholder = string.Format(Resources.UserData_WriteYourCurrentPassword, 4, 50), @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.UserData_CurrentPassword)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Password -->
</div>
<div class="div33-left">
<p><%=Html.LabelFor(cust => cust.NewPassword)%> </p>
  <%= Html.PasswordFor(cust => cust.NewPassword, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourPassword, 4, 50), @class = "inputelementsw100" })%>
  <!--  Password -->
 <p class="aclaracion"><%=Resources.UserData_PasswordRemark%></p> 
</div>
<div class="div33-right">
 <p><%=Html.LabelFor(cust => cust.ConfirmNewPassword)%> </p>
  <%= Html.PasswordFor(cust => cust.ConfirmNewPassword, new { @placeholder = string.Format(Resources.UserData_WriteYourConfirmPassword, 4, 50), @class = "inputelementsw100" })%>
  <!--  Repeat Password -->
<br />
</div>

<div class="greenhr"><hr /></div>
<p><br /></p>

<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Name)%> </p>
  <%= Html.TextBoxFor(cust => cust.Name, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourName, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Name)+"');", 
            @oninput="this.setCustomValidity('');" })%>
</div>
<div class="div50-right">
 <p><%=Html.LabelFor(cust => cust.Surname1)%></p>
  <%= Html.TextBoxFor(cust => cust.Surname1, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourFirstSurname, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_FirstSurname)+"');", 
            @oninput="this.setCustomValidity('');" })%>
 
</div>
<div class="div50-left">
   <p><%=Html.LabelFor(cust => cust.Surname2)%></p>
  <%= Html.TextBoxFor(cust => cust.Surname2, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourSecondSurname, @class = "inputelementsw100" })%>
 
</div>
<div class="div50-right">
   <p><%=Html.LabelFor(cust => cust.DocId)%></p>
  <%= Html.TextBoxFor(cust => cust.DocId, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourDocID, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_DocID)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  
</div>
<div class="div15-left">
   <p><%=Html.LabelFor(cust => cust.MainPhoneNumber)%></p>
  <select id="MainPhoneNumberPrefixDis" size="1"  onChange="showValue(this.value)" name="MainPhoneNumberPrefixDis"  class ="inputelementsw100" disabled >
   <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "../Content/img/banderas/", (String)ViewData["SelectedMainPhoneNumberPrefix"])%>
  </select>
  <input type="hidden"  id="MainPhoneNumberPrefix"  name="MainPhoneNumberPrefix" value="<%=(String)ViewData["SelectedMainPhoneNumberPrefix"] %>" />
</div>
<div class="div35-left">
  <p>&nbsp</p>
  <%= Html.TextBoxFor(cust => cust.MainPhoneNumber, new
{
    @placeholder = Resources.CustomerInscriptionModel_WriteYourMainPhoneNumber,
    @class = "inputelementsw100",
    @readonly = "readonly"
})%>
</div>
<div class="div15-left">
  <p><%=Html.LabelFor(cust => cust.AlternativePhoneNumber)%></p>
  <select id="AlternativePhoneNumberPrefix" size="1"  onChange="showValue(this.value)" name="AlternativePhoneNumberPrefix"  class ="inputelementsw100" >
    <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "../Content/img/banderas/", (String)ViewData["SelectedAlternativePhoneNumberPrefix"])%>				
  </select>
</div>
<div class="div35-left">
  <p>&nbsp</p>
  <%= Html.TextBoxFor(cust => cust.AlternativePhoneNumber, new
{
    @placeholder = Resources.CustomerInscriptionModel_WriteYourAlternativePhoneNumber,
    @class = "inputelementsw100"
})%>
 <p class="aclaracion"><%=Resources.CustomerInscriptionModel_Remark_ivr%></p>   
</div>
<div class="greenhr"><hr /></div>
<p><br /></p>
<div class="div40-left">
 <p><%=Html.LabelFor(cust => cust.StreetName)%> </p>
  <%= Html.TextBoxFor(cust => cust.StreetName, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetName, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetName)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Nombre de la calle -->
</div>
<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.StreetNumber)%> </p>
  <%= Html.TextBoxFor(cust => cust.StreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetNumber, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetNumber)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Numero  -->
</div>
<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.LevelInStreetNumber)%> </p>
  <%= Html.TextBoxFor(cust => cust.LevelInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourLevelInStreetNumber, @class = "inputelementsw100" })%>
  <!--  Piso  -->
</div>

<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.DoorInStreetNumber)%> </p>
  <%= Html.TextBoxFor(cust => cust.DoorInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourDoorInStreetNumber, @class = "inputelementsw100" })%>
  <!--  Puerta  -->
</div>

<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.LetterInStreetNumber)%> </p>
  <%= Html.TextBoxFor(cust => cust.LetterInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourLetterInStreetNumber, @class = "inputelementsw100" })%>
  <!--  Letra  -->
</div>

<div class="div12-right">
 <p><%=Html.LabelFor(cust => cust.StairInStreetNumber)%> </p>
  <%= Html.TextBoxFor(cust => cust.StairInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourStairInStreetNumber, @class = "inputelementsw100" })%>
  <!--  Escalera  -->
</div>

<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Country)%></p>
  <select id="Country" size="1"  onChange="showValue(this.value)" name="Country"  class="inputelementsw100">
    <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "../Content/img/banderas/", (String)ViewData["SelectedCountry"])%>				
  </select>
</div>
<div class="div50-right">
 <p><%=Html.LabelFor(cust => cust.State)%> </p>
  <%= Html.TextBoxFor(cust => cust.State, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourState, @class = "inputelementsw100" ,
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_State)+"');", 
            @oninput="this.setCustomValidity('');"})%>
  <!--  Provincia  -->
</div>

<div class="div50-left">
 <p><%=Html.LabelFor(cust => cust.City)%> </p>
  <%= Html.TextBoxFor(cust => cust.City, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourCity, @class = "inputelementsw100" ,
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_City)+"');", 
            @oninput="this.setCustomValidity('');"})%>
  <!--  Ciudad  -->
</div>
<div class="div50-right">
 <p><%=Html.LabelFor(cust => cust.ZipCode)%> </p>
  <%= Html.TextBoxFor(cust => cust.ZipCode, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourZipCode, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_ZipCode)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Codigo Postal  -->
 <br />
</div>
<div class="greenhr"><hr /></div>
<div class="div33-left">
    <p><%=Html.LabelFor(cust => cust.Plates)%>:</p>
    <p><%=Html.ListBoxFor(cust => cust.Plates, Model.Plates, new {  @class = "PlatesSelect" })%></p> 
</div>
<div class="div33-left">
 <input type="button" value="<%=Resources.UserData_RemovePlate%>" 
      onclick="removeOptionSelected();"
      class="botonverdeleft" />
    <p><br /><br /></p>
 <input type="button" value="<%=Resources.UserData_InsertNewPlate%>" id="btnAddPlate" 
      onclick="enableAddPlate();"
      class="botonverdeleft" />
 <input class="inputelementsw100" type="text" value=""  id="newPlate" style="visibility:hidden;"
     placeholder="<%= Resources.CustomerInscriptionModel_WriteYourPlate%>"/>

 <input type="hidden" value="<%=Model.platesChanges%>"  id="platesChanges"  name="platesChanges" / >

 <input type="button" value="<%=Resources.UserData_ConfirmAdd%>" id="btnConfirmAdd" style="visibility:hidden;"
      onclick="confirmAdd();"
      class="botonverde" />
 <input type="button" value="<%=Resources.UserData_CancelAdd%>" id="btnCancelAdd" style="visibility:hidden;"
      onclick="cancelAdd();"
      class="botonverde" />
</div>

<div class="greenhr"><hr /></div>
<p class="aclaracion"><%=Resources.UserData_PlatesRemark%></p> 
<p><br /></p>
<input type="submit" value="<%=Resources.UserData_ButtonConfirm%>" class="botonverde" />
<input type="button" value="<%=Resources.UserData_ButtonCancel%>" class="botongris" 
    onclick="location.href='<%=Url.Action("Main", "Account", null)%>'" />

</fieldset>
<% } %>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
