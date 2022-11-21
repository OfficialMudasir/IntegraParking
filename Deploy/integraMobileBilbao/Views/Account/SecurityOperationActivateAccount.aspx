<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title><%=Resources.ActivateAccount_Title%> || BILBAOPARK</title>
<link href="../Content/CSS/password_reset.css" rel="stylesheet" type="text/css"><!--[if lt IE 9]>
<script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
</head>

<body>

<div class="containerBody">
  <header> <img src="../Content/img/logo_Bilbao_160x80.png" alt="BILBAOPARK" name="logoBILBAOPARK" width="160" height="80" id="logoBILBAOPARK" class="imgLogo" />
  <h1><%=Resources.ActivateAccount_Title%></h1>
  </header>
  
  <%-- CONTENT --%>
  <section class="content">
    
    <div class="error">
        <%: Html.ValidationSummary(true, "")%>
        <p><%= Html.ValidationMessage("CodeExpired")%></p>
        <p><%= Html.ValidationMessage("CodeAlreadyUsed")%></p>
    <p><%= Html.ValidationMessage("ConfirmationCodeError")%></p>
    </div> 

    <p><%=String.Format(Resources.Activate_Account_Message1,(String)ViewData["username"])%></p>

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

