<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title><%=Resources.ForgotPasswordConfirmation_Title%> || BILBAOPARK</title>
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
  <section class="content passSuccess">
    	<img src="../Content/img/check-icon.png" alt="Success" class="imgPassSuccess" width="200" />
     <h2> <%=Resources.ForgotPasswordConfirmation_Message1%></h2>
        <div class="formContainer">
        <form action="#" id="formPasswordReset" name="passwordReset" method="post">

            <br />
            <button type="button" title="Cerrar formulario"><%=Resources.Button_Close%></button>
            
        
        </form>
        </div>
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
<%-- end .container --%></div>
</body>
</html>
