<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ForgotPasswordModel>" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ForgotPassword
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="../Content/dropdown/js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../Content/dropdown/js/jquery.dd.js" type="text/javascript"></script>

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
 <% using (Html.BeginForm("ForgotPassword", "Account", FormMethod.Post)) { %>  
 <p>&nbsp;</p>
<h3><%=Resources.ForgotPassword_Title%></h3>
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.Username)%></p>
 <p><%= Html.ValidationMessage("recaptcha")%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  
<fieldset>  


<div class="div50-center">
<p><%=Resources.CustomerInscriptionModel_Email %> </p>
  <%= Html.TextBoxFor(cust => cust.Username, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourEmail,4,50),
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Email)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!-- Username -->
    <p class="aclaracion"><%=Resources.ForgotPassword_Remarks%></p>
</div> 
    <div class="div50-center">
  <%= ReCaptchaHelper.ReCAPTCHA(Resources.CustomerInscriptionModel_reCaptchaIdiom)%>
  &nbsp
</div>

<p>&nbsp;</p>
<br/>
<div class="greenhr"><hr /></div> 
<input type="submit" value="<%=Resources.Button_Confirm %>" class="botonverde" />
<input type="button" value="<%=Resources.Button_Cancel %>" class="botongris" 
    onclick="location.href='<%=Url.Action("Index", "Home", null)%>'" />
<% } %>
</fieldset>

</div>

</asp:Content>
