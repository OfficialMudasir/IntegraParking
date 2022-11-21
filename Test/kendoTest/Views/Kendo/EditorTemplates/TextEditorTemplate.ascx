<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>

<%: Html.TextBoxFor(m => m, new {@class = "k-input k-textbox" })%>
