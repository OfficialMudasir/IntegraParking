<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.IndividualsRegistration_Signup_Check_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



	<div class="msgcontent">
		<div class="msgcontent__literal"><%=Resources.IndividualsRegistration_Signup1 %></div>
		<div class="msgcontent__resource">
			<img alt="<%=Resources.IndividualsRegistration_Signup_Check_Email %>" src="<%= Url.Content("~/Content/img/signup_error_"+ViewData["culture"]+".png") %>">
		</div>
	</div>

   

</asp:Content>