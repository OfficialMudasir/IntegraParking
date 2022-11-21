<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ChangeEmailOrMobileModel>" %>
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
    
</script>



<div id="formulario">
<div class="greenhr"><hr /></div>
    <h2><%=Resources.UserData_ChangeTelOrEmailTitle %></h2>

  <% using (Html.BeginForm("ChangeEmailOrMobile", "Account", FormMethod.Post))
     { %>   
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.Email)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.MainPhoneNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.CurrentPassword)%></p>
<p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  

<fieldset> 

<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Email)%></p>
  <%= Html.TextBoxFor(cust => cust.Email, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourEmail, @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
            @oninput="this.setCustomValidity('');" })%>
</div>

<div class="div15-left">
   <p><%=Html.LabelFor(cust => cust.MainPhoneNumber)%></p>
  <select id="MainPhoneNumberPrefixDis" size="1"  onChange="showValue(this.value)" name="MainPhoneNumberPrefixDis"  class ="inputelementsw100" >
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
    @required="true", 
    @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_MainPhoneNumber)+"');", 
    @oninput="this.setCustomValidity('');"
})%>
</div>

<p><br /></p>

<div class="div33-left">
<p><%=Html.LabelFor(cust => cust.CurrentPassword)%> </p>
  <%= Html.PasswordFor(cust => cust.CurrentPassword, new { @placeholder = string.Format(Resources.UserData_WriteYourCurrentPassword, 4, 50), @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.UserData_CurrentPassword)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Password -->
</div>

<div class="greenhr"><hr /></div>
<p class="aclaracion"><%=Resources.ChangeEmailOrMobile_Remarks%></p> 
<p><br /></p>
<input type="submit" value="<%=Resources.UserData_ButtonConfirm%>" class="botonverde" />
<input type="button" value="<%=Resources.UserData_ButtonCancel%>" class="botongris" 
    onclick="location.href='<%=Url.Action("UserData", "Account", null)%>'" />

</fieldset>
<% } %>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
