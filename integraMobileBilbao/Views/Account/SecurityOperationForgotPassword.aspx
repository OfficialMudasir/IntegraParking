<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ForgotPasswordModel>" %>
<%@ Import Namespace="System.Globalization" %>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title><%=Resources.ForgotPassword_Title%> || BILBAOPARK</title>
<link href="../Content/CSS/password_reset.css" rel="stylesheet" type="text/css"><!--[if lt IE 9]>
<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
</head>

<body>

<div class="containerBody">
  <header> <img src="../Content/img/logo_Bilbao_160x80.png" alt="BILBAOPARK" name="logoBILBAOPARK" width="160" height="80" id="logoBILBAOPARK" class="imgLogo" />
  <h1><%=Resources.ForgotPassword_Title%></h1>
  </header>
  
  <%-- CONTENT --%>
  <section class="content">
    
  

    <% if ((!Convert.ToBoolean(ViewData["CodeExpired"])) && 
           (!Convert.ToBoolean(ViewData["CodeAlreadyUsed"])) && 
           (!Convert.ToBoolean(ViewData["ConfirmationCodeError"])))
    { %>
        <div class="error">
         <%: Html.ValidationSummary(true, "")%>
         <p><%= Html.ValidationMessageFor(cust => cust.Password)%></p>
         <p><%= Html.ValidationMessageFor(cust => cust.ConfirmPassword)%></p>
         <p><%= Html.ValidationMessage("CodeExpired")%></p>
         <p><%= Html.ValidationMessage("CodeAlreadyUsed")%></p>
        </div> 
        <p><%=String.Format(Resources.ForgotPassword_Message1,(String)ViewData["username"])%></p>
        <div class="formContainer">
        <% using (Html.BeginForm("SecurityOperationForgotPassword", "Account", FormMethod.Post))
        { %>  
            <div class="messageContainer">
                <p><%=Resources.ForgotPassword_Message2%></p>
            </div>
            <div id="fieldNewPassword" class="fieldContainer">
                <%=Html.LabelFor(cust => cust.Password) %> 
                <%= Html.PasswordFor(cust => cust.Password, new { @placeholder = string.Format(Resources.ForgotPassword_WriteYourPassword,5,50) })%>            
            </div>
            <div id="fieldConfirmNewPassword" class="fieldContainer">
                <%=Html.LabelFor(cust => cust.ConfirmPassword) %> 
                <%= Html.PasswordFor(cust => cust.ConfirmPassword, new { @placeholder = string.Format(Resources.ForgotPassword_WriteYourConfirmPassword,5,50) })%>
            </div>
                <br />
                <button type="submit" title="Enviar formulario"><%=Resources.Button_Send%></button>           
            <% } %>
        </div>
    <% } else { %>
      <div class="error">
        <%: Html.ValidationSummary(true, "")%>
        <p><%= Html.ValidationMessage("CodeExpired")%></p>
        <p><%= Html.ValidationMessage("CodeAlreadyUsed")%></p>
        <p><%= Html.ValidationMessage("ConfirmationCodeError")%></p>
        </div>
    <% } %>

    <%-- end .content --%></section>
  <footer class="pie">
<div id="enlacespie">
		<div class="cont-enlacespie">
			<ul lang="es">
			  <li><a accesskey="u" href="#">Condiciones de uso</a></li>
              <li><a accesskey="p" href="#">Aviso de privacidad</a></li>
              <li><a accesskey="c" href="#">Política de cookies</a></li>
              <li><a accesskey="s" href="#">Baja del servicio</a></li>
            </ul>
		</div>
		<div id="direccionpie">
			<p class="inv"><strong>Address:</strong></p>
			<p><span lang="eu">Bilboko Udala</span> / <span lang="es">Ayuntamiento de Bilbao</span> / <span lang="en">Bilbao Council</span></p>
       		<address>
       		 Plaza Ernesto Erkoreka <abbr title="Número">nº</abbr>1. 48007 Bilbao. Teléfono: <a href="tel:+34944204200">944 204 200</a> - <a href="http://www.bilbao.eus">www.bilbao.eus</a>
            </address>
  		</div>	
	</div>
  </footer>
</div>
</body>
</html>

