<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Step1End
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="div100Sup1"><img src="../Content/img/Step1End-<%=((CultureInfo)Session["Culture"]).Name%>.png"/></div>

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.RegistrationForm %> - <%=Resources.RegistrationForm_Step1End_IdentityValidation_Step1%></h2>
 <p>&nbsp;</p>
<h6><%=string.Format(Resources.CustomerInscriptionModel_Step1EndIntroText1,ViewData["maskedTelephone"]) %><br/>
  <p>&nbsp;</p>
  <%=string.Format(Resources.CustomerInscriptionModel_Step1EndIntroText2,ViewData["email"]) %><br/> 
    <p>&nbsp;</p>
    <p><%=Resources.CustomerInscriptionModel_Step1EndIntroText3 %></p></h6>
<p>&nbsp;</p>

<span style="margin-left:5px;">



<p><img src="../Content/img/ok.jpg" width="18px" height="11px" style="vertical-align:baseline"/>
<%=Resources.CustomerInscriptionModel_Step1EndIntroText4%></p> 
<p><img src="../Content/img/ok.jpg" width="18px" height="11px" style="vertical-align:baseline"/>
<%=Resources.CustomerInscriptionModel_Step1EndIntroText5%></p>
<br/>
</div>
 <div class="greenhr"><hr /></div>    
 <h4><%=(String)ViewData["ActivationRetries"] %></h4>
 <% using (Html.BeginForm("Step1End", "IndividualsRegistration", FormMethod.Post)) { %>    
 <input type="submit" name="submitButton" value="<%=Resources.Button_RetrySend %>" class="botonverde" />
 <% } %>
<br/>

</asp:Content>
