<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%-- /* Refactoring behavior to show icon flag */ --%>
<%-- 
<li class="f16"><%= Html.ActionLink("English", "ChangeCulture", "Home",  
     new { lang = "en-US", returnUrl = this.Request.RawUrl }, new { @class = "flag gb" })%></li>
<li class="f16"><%= Html.ActionLink("Español", "ChangeCulture", "Home",  
     new { lang = "es-ES", returnUrl = this.Request.RawUrl }, new { @class = "flag es" })%></li>
<li class="f16"><%= Html.ActionLink("Français", "ChangeCulture", "Home",  
     new { lang = "fr-CA", returnUrl = this.Request.RawUrl }, new { @class = "flag ca" })%></li>	 
<li class="f16"><%= Html.ActionLink("Català", "ChangeCulture", "Home",  
     new { lang = "ca-ES", returnUrl = this.Request.RawUrl }, new { @class = "flag cat" })%></li>	 
--%>

<%--<li class="f16">
    <a href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "en-US", returnUrl = this.Request.RawUrl } ) %>" >
        <span class="flag-icon us"></span>  English
    </a>
</li>
<li class="f16">
    <a href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "es-ES", returnUrl = this.Request.RawUrl }) %>">
            <span class="flag-icon es"></span>  Español
    </a>
</li>
<li class="f16">
    <a href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "fr-CA", returnUrl = this.Request.RawUrl }) %>">
        <span class="flag-icon ca"></span>  Français
    </a>
</li>
<li class="f16">
    <a href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "ca-ES", returnUrl = this.Request.RawUrl }) %>">
        <span class="flag-icon cat"></span>  Català
    </a>
</li>--%>

<%--New changes--%>
<li class="f16">
    <a class="changeURLName" data-lang="English" href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "en-US", returnUrl = this.Request.RawUrl } ) %>" >
        <span class="flag-icon us"></span>  English
    </a>
</li>
<li class="f16">
    <a class="changeURLName" data-lang="Español" href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "es-ES", returnUrl = this.Request.RawUrl }) %>">
            <span class="flag-icon es"></span>  Español
    </a>
</li>
<li class="f16">
    <a class="changeURLName" data-lang="Français" href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "fr-CA", returnUrl = this.Request.RawUrl }) %>">
        <span class="flag-icon ca"></span>  Français
    </a>
</li>
<li class="f16">
    <a class="changeURLName" data-lang="Català" href="<%= Url.Action("ChangeCulture", "Home",  new { lang = "ca-ES", returnUrl = this.Request.RawUrl }) %>">
        <span class="flag-icon cat"></span>  Català
    </a>
</li>
<script type="text/javascript">

    $(document).ready(function () {        
        var _loadcommonurl = sessionStorage.getItem("languageUrlName");      
        if (_loadcommonurl !== null && _loadcommonurl !== undefined && _loadcommonurl !== "") {
            $("#selectedidlang").text(_loadcommonurl);
        }
        else {
            $("#selectedidlang").text("English");
        }
       
    });
        
    $(".changeURLName").click(function (e) {
        debugger;
        sessionStorage.removeItem("languageUrlName");
        var _self = this;
        var _url = $(_self).attr("data-lang");
        sessionStorage.setItem("languageUrlName", _url);
    });
</script>

<%--End changes--%>