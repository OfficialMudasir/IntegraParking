<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Step2
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="div100Sup1"><img src="../Content/img/Step2-<%=((CultureInfo)Session["Culture"]).Name%>.png"/></div>

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step2%></h2>
<br />
<div class="greenhr"><hr /></div>
<% if ((!Convert.ToBoolean(ViewData["CodeExpired"])) && (!Convert.ToBoolean(ViewData["CodeAlreadyUsed"])))
   { %>
<br/>
<% using (Html.BeginForm("Step2", "IndividualsRegistration", FormMethod.Post)) { %>  
<p><%=String.Format(Resources.CustomerInscriptionModel_Step2WriteYourCode1,
   ConfigurationManager.AppSettings["NumCharactersActivationSMS"],
   (String)ViewData["EndSufixMainPhone"],
   (int)ViewData["NumMinutesTimeoutActivationSMS"])%>  
   </p>
 <div class="error">
 <p><%= Html.ValidationMessage("ConfirmationCodeError")%></p>
 </div>
<div class="div100" style="text-align:right">

   
 <%=Resources.CustomerInscriptionModel_Step2WriteYourCode%>
  <input type="text" autofocus="true" placeholder="" name="confirmationcode" style="display:inline;width:auto;" required="true"
            oninvalid="this.setCustomValidity('<%=string.Format(Resources.ErrorsMsg_RequiredField,"")%>');", 
            oninput="this.setCustomValidity('');" />
 <input type="hidden" name="type" value="confirmcode"/>
 <input type="submit" value=" <%=Resources.Button_Validate%>" name="submitButton" class="botonverde" />


</div>
<p>&nbsp;</p>

<p>&nbsp;</p>
<div class="div100">
<div class="bluetext"><%=String.Format(Resources.CustomerInscriptionModel_Step2SendSMS, 
                            (String)ViewData["EndSufixMainPhone"])%></div>
</div>
<p>&nbsp;</p>
 <% } %>
<div class="greenhr"><hr /></div>
 <% using (Html.BeginForm("Step2", "IndividualsRegistration", FormMethod.Post)) { %>   
<div class="div60-left">
<h4><%=(String)ViewData["ActivationRetries"] %></h4>

<div class="div40-right">
<input type="hidden" name="type" value="resendsms"> 
<input type="submit" value="<%=Resources.Button_ResendSMS%>" name="submitButton"  class="botonverde" />
 <% } %>
 <% } else { %>
  <div class="error">
 <p><%= Html.ValidationMessage("CodeExpired")%></p>
 <p><%= Html.ValidationMessage("CodeAlreadyUsed")%></p>
 </div>
  <div class="div100" style="text-align:right">
 <% using (Html.BeginForm("Step1", "IndividualsRegistration", FormMethod.Get)) { %> 
 <input type="submit" value="<%=Resources.Button_ReInitRegistration%>"  class="botonverde" />
  <% } %>
 <% } %>

</div>
</div>



</asp:Content>
