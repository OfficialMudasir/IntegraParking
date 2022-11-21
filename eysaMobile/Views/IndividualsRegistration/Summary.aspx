<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Summary
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="formulario">
<h1> <%=string.Format(Resources.CustomerInscriptionModel_Welcome, ViewData["UserNameAndSurname"])%> </h1> 
<h2><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_1%></h2>
<div class="cajagris">
<p><%=string.Format(Resources.CustomerInscriptionModel_SuccessCreatingAccount_3,ViewData["email"]) %>
<p>&nbsp;</p>
<ul><strong><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_4%></strong></p>
<li><img src="../Content/img/ok.jpg" width="18px" height="11px" style="vertical-align:baseline"/> <%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_5%></li>
<li><img src="../Content/img/ok.jpg" width="18px" height="11px" style="vertical-align:baseline"/> <%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_6%></li>
<li><img src="../Content/img/ok.jpg" width="18px" height="11px" style="vertical-align:baseline"/> <%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_7%></li>
<li><img src="../Content/img/ok.jpg" width="18px" height="11px" style="vertical-align:baseline"/> <%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_8%></li>
<li><p>&nbsp;</p></li>
<li style="font-weight:bold"><%=Resources.CustomerInscriptionModel_SuccessCreatingAccount_9%></li>
</ul>
<p>&nbsp;</p>
<div class="div65-right">
 <% using (Html.BeginForm("LogOn", "Home", FormMethod.Get)) { %>    
<input type="submit" value="<%=Resources.CustomerInscriptionModel_Summary_Button_Login%>" class="botonGrande" />
<% } %>


</div>
</div>

</div>
</asp:Content>
