<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<kendoTest.Models.LogOnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Integra Mobile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1> <%=Resources.ServiceName %> </h1> 
<p><%=Resources.CustomerInscriptionModel_Step1IntroText1%></p>
<p><%=Resources.CustomerInscriptionModel_Step1IntroText2%></p> 
<p><%=Resources.CustomerInscriptionModel_Step1IntroText3%></p>
<br />
    <div id="divformularioacceso">
  <div id="divlogin">
  <div id="divloginmargen">
   <% using (Html.BeginForm("LogOn", "Home", FormMethod.Post, new { autocomplete = "off" }))
      { %>  
   <fieldset>
   <%: Html.ValidationSummary(true, "") %>
    <h4><%=Resources.Home_Username%></h4>
     <%: Html.TextBoxFor(m => m.UserName, new  {@style="width:100%;margin-bottom:0;", autocomplete = "off", tabindex = 1}  ) %>
     <%: Html.ValidationMessageFor(m => m.UserName) %>
    <p>&nbsp;</p>
    <h4><%=Resources.Home_Password%></h4>
   <%: Html.PasswordFor(m => m.Password, new  {@style="width:100%;margin-bottom:0;", autocomplete = "off",  tabindex = 2}) %>
   <%: Html.ValidationMessageFor(m => m.Password) %>
  
    <div class="aclaracion"><%= Html.ActionLink(Resources.Home_ForgotPassword,"ForgotPassword","Account")%><br/>
<!--     <%: Html.CheckBoxFor(m => m.RememberMe, new { @style = "width:5%;margin-bottom:0;margin-left:0px;display:inline;",tabindex = 3 })%>
     <%=Resources.RememberMeCheck%><br/>-->
    </div>
    <input type ="hidden" value="" id="utcoffset" name="utcoffset"/>
    <input type="submit" value="<%=Resources.Button_Logon%>" class="botonverde" />
   
</fieldset>
<% } %>
  </div>
  </div>
  <div id="divregistro">
  <div id="divregistromargen">
     <h4><%=Resources.Home_AddNewUser%></h4>
     <p>&nbsp;</p><p>&nbsp;</p>
     <% using (Html.BeginForm("Step1", "IndividualsRegistration", FormMethod.Get)) { %>
     <input type="submit" onclick="" value="<%=Resources.Home_Individuals%>" class="botonverde" style="width:100%"/>
	 <% } %>
    <p>&nbsp;</p><p>&nbsp;</p>
     <input type="button" disabled onclick="" value="<%=Resources.Home_Companies%>" class="botonazul" style="width:100%"/>
     <p>&nbsp;</p>
     <p>&nbsp;</p>
  </div>
  </div>
</div>

<div><%= Html.ActionLink(Resources.Home_Service_Conditions, "gCond_"+ViewData["lang_for_gCond"], "Home")%>
<p>&nbsp;</p>
</div>
<div class="greenhr"><hr /></div>
</asp:Content>
