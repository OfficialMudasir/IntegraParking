<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<li><%= Html.ActionLink("English", "ChangeCulture", "Home",  
     new { lang = "en-US", returnUrl = this.Request.RawUrl }, null)%></li>
<li><%= Html.ActionLink("Español", "ChangeCulture", "Home",  
     new { lang = "es-ES", returnUrl = this.Request.RawUrl }, null)%></li>
<li><%= Html.ActionLink("Français", "ChangeCulture", "Home",  
     new { lang = "fr-CA", returnUrl = this.Request.RawUrl }, null)%></li>