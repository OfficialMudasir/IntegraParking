<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.CustomerInscriptionModelStep1>" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Step1
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
            alert(e.message);
        }
    });
</script>

<div class="div100Sup1"><img src="../Content/img/Step1-<%=((CultureInfo)Session["Culture"]).Name%>.png"/></div>

<div id="formulario">
<h1> <%=Resources.ServiceName %></h1> 
<h2><%=Resources.RegistrationForm %> - <%=Resources.PersonalData %></h2>
<p><%=Resources.CustomerInscriptionModel_Step1IntroText4%></p>
<div class="greenhr"><hr /></div>
 <% using (Html.BeginForm("Step1", "IndividualsRegistration", FormMethod.Post)) { %>    
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.Name)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Surname1)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Surname2)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.DocId)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.MainPhoneNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.AlternativePhoneNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Email)%></p>
 <p><%= Html.ValidationMessage("recaptcha")%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  
  <fieldset>  
<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Email) %></p>
  <%= Html.TextBoxFor(cust => cust.Email, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourEmail, @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
            @oninput="this.setCustomValidity('');" })%>
</div>
<div class="div50-right">
 <p>&nbsp;</p>
    <% if (ViewData["UsernameEqualsEmail"].ToString()=="1"){ %>
        <h6><b><%=Resources.CustomerInscriptionModel_Step1RememberUsernameEqualsEmail %></b> </h6> 
    <%} %>
</div>        
<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Name) %> </p>
  <%= Html.TextBoxFor(cust => cust.Name, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourName, @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Name)+"');", 
            @oninput="this.setCustomValidity('');" })%>
</div>
<div class="div50-right">
 <p><%=Html.LabelFor(cust => cust.Surname1) %></p>
  <%= Html.TextBoxFor(cust => cust.Surname1, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourFirstSurname, @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_FirstSurname)+"');", 
            @oninput="this.setCustomValidity('');" })%>
 
</div>
<div class="div50-left">
   <p><%=Html.LabelFor(cust => cust.Surname2) %></p>
  <%= Html.TextBoxFor(cust => cust.Surname2, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourSecondSurname })%>
 
</div>
<div class="div50-right">
   <p><%=Html.LabelFor(cust => cust.DocId) %></p>
  <%= Html.TextBoxFor(cust => cust.DocId, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourDocID, @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_DocID)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  
</div>
<div class="div15-left">
   <p><%=Html.LabelFor(cust => cust.MainPhoneNumber) %></p>
  <select id="MainPhoneNumberPrefix" size="1"  onChange="showValue(this.value)" name="MainPhoneNumberPrefix" style="width:100%">
   <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "../Content/img/banderas/", (String) ViewData["SelectedMainPhoneNumberPrefix"])%>
  </select>
</div>
<div class="div35-left">
  <p>&nbsp</p>
  <%= Html.TextBoxFor(cust => cust.MainPhoneNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourMainPhoneNumber, @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_MainPhoneNumber)+"');", 
            @oninput="this.setCustomValidity('');" })%>
</div>
<div class="div15-left">
  <p><%=Html.LabelFor(cust => cust.AlternativePhoneNumber) %></p>
  <select id="AlternativePhoneNumberPrefix" size="1"  onChange="showValue(this.value)" name="AlternativePhoneNumberPrefix" style="width:100%">
    <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "../Content/img/banderas/", (String)ViewData["SelectedAlternativePhoneNumberPrefix"])%>				
  </select>
</div>
<div class="div35-right">
  <p>&nbsp</p>
  <%= Html.TextBoxFor(cust => cust.AlternativePhoneNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourAlternativePhoneNumber })%>
 <p class="aclaracion"><%=Resources.CustomerInscriptionModel_Remark_ivr%></p>   
</div>
<div class="div100-recap">
      <%= ReCaptchaHelper.ReCAPTCHA(Resources.CustomerInscriptionModel_reCaptchaIdiom)%>
  &nbsp
</div>
<br/>
<div class="greenhr"><hr /></div>
<input type="submit" value="<%=Resources.Button_Next %>" class="botonverde" />
</fieldset>
<% } %>
</div>

</asp:Content>
