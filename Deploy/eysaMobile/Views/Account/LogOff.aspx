<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Main Menu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="div65-right">
<input  type="button" value="<%=Resources.Account_Recharge_BuyNow %>" class="botonverde" onclick="location.href='Recharge';"/>
<input  type="button" value="<%=Resources.Account_Logout_Button %>" class="botonverde" onclick="location.href='Logout';"/>
</div>
</asp:Content>
