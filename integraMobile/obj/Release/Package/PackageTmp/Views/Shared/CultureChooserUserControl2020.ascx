<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% if ((ViewData["Show_en-US"] != null ? ((bool)ViewData["Show_en-US"]) : true))
   {
       using (Html.BeginForm("ChangeCulture", "Fine", FormMethod.Post, new { id = "ChangeCultureEN" }))
       { %>
<input type="hidden" name="submitButton" value="submitButton" />
<input type="hidden" name="Lang" value="en-US" />
<div class="dropdown-item" onclick="document.getElementById('ChangeCultureEN').submit();"><img alt="en-US" src="../Content/img/2020/flags/canada.png" height="19px"> English</div>
<% }
   } %>
<%  if ((ViewData["Show_es-ES"] != null ? ((bool)ViewData["Show_es-ES"]) : true))
    {
        using (Html.BeginForm("ChangeCulture", "Fine", FormMethod.Post, new { id = "ChangeCultureES" }))
        { %>
<input type="hidden" name="submitButton" value="submitButton" />
<input type="hidden" name="Lang" value="es-ES" />
<div class="dropdown-item" onclick="document.getElementById('ChangeCultureES').submit();"><img alt="es-ES" src="../Content/img/2020/flags/spain.png" height="19px"> Español</div>
<% }
    } %>
<%  if ((ViewData["Show_fr-CA"] != null ? ((bool)ViewData["Show_fr-CA"]) : true))
    {
        using (Html.BeginForm("ChangeCulture", "Fine", FormMethod.Post, new { id = "ChangeCultureFR" }))
        { %>
<input type="hidden" name="submitButton" value="submitButton" />
<input type="hidden" name="Lang" value="fr-CA" />
<div class="dropdown-item" onclick="document.getElementById('ChangeCultureFR').submit();"><img alt="fr-CA" src="../Content/img/2020/flags/french-Quebec.png" height="19px"> Français</div>
<% }
    } %>
<%  if ((ViewData["Show_ca-ES"] != null ? ((bool)ViewData["Show_ca-ES"]) : true))
   {
        using (Html.BeginForm("ChangeCulture", "Fine", FormMethod.Post, new { id = "ChangeCultureCA" })) { %>
<input type="hidden" name="submitButton" value="submitButton" />
<input type="hidden" name="Lang" value="ca-ES" />
<div class="dropdown-item" onclick="document.getElementById('ChangeCultureCA').submit();"><img alt="ca-ES" src="../Content/img/2020/flags/catalonia.png" height="19px"> Català</div>
<% } 
   }%>