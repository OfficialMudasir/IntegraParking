<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ChangeEmailOrMobileEnd
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="../Content/dropdown/js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../Content/dropdown/js/jquery.dd.js" type="text/javascript"></script>

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
 <p>&nbsp;</p>
<h3><%=Resources.UserData_ChangeTelOrEmailTitle%></h3>


<div class="div50-center">
<p class="aclaracion"><%=Resources.UserData_ChangeEmailOrMobileEnd_Remark%></p>
</div> 

<p>&nbsp;</p>
<br/>
<div class="greenhr"><hr /></div> 
<input type="button" value="<%=Resources.Button_Next %>" class="botonverde" 
    onclick="location.href='<%=Url.Action("UserData", "Account", null)%>'" />
</div>

</asp:Content>
