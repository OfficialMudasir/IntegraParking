<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8">
    <title>Condiciones del Servicio</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>

    <link rel="stylesheet" href="~/Content/css/style.css">
    <link rel="stylesheet" href="~/Content/css/loader-style.css">
    <link rel="stylesheet" href="~/Content/css/bootstrap.min.css">
    <link rel="stylesheet" href="~/Content/css/extra-pages.css">
    <style type="text/css">
        @media (max-width: 767px) {
            body {
                padding: 0 !important;
            }
        }
    </style>

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
        <script src="https://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->
    <!-- Fav and touch icons -->
    <link rel="shortcut icon" href="~/Content/ico/favicon.ico">
</head>

<body>

    <div id="preloader">
        <div id="status">&nbsp;</div>
    </div>

    <nav role="navigation" class="navbar navbar-static-top no-print">
        <div class="container-fluid">

            <div class="navbar-header">
                <button data-target="#bs-example-navbar-collapse-1" data-toggle="collapse" class="navbar-toggle" type="button">
                    <span class="entypo-menu"></span>
                </button>

                <div class="sm-logo">
                    <a href="<%= Url.Action("Index", "Home") %>"><img src="<%= Url.Content("~/Content/img/logo.svg") %>" alt="Integra - Parking Solutions"></a>
                </div>

            </div>

            <div id="bs-example-navbar-collapse-1" class="collapse navbar-collapse">

                <ul style="margin-right:0;" class="nav navbar-nav navbar-right">
                    <li>
                        <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                            <span class="entypo-globe"></span>&#160;&#160;Change Language <b class="caret"></b></a>
                        <ul role="menu" class="dropdown-setting dropdown-menu">
                            <li class="f16"><%= Html.ActionLink("English", "gCond_en_US", "Home",  new {@class = "flag gb"})%></li>
                            <li class="f16"><%= Html.ActionLink("Español", "gCond_es_ES", "Home",  new {@class = "flag es"})%></li>
                        </ul>
                    </li>
                </ul>

            </div>
        </div>
    </nav>

    <div class="wrap-fluid">
        <div class="container-fluid paper-wrap bevel tlbr">
            <div class="row">
                <div id="paper-top">
                    <div class="col-sm-12">
                        <h2 class="tittle-content-header"> Aviso de Pivacidad</h2>

                    </div>
                </div>
            </div>

            <ul id="breadcrumb">
                <li>
                    <span class="entypo-home"></span>
                </li>
                <li><i class="fa fa-lg fa-angle-right"></i>
                </li>
                <li><a href="<%= Url.Action("Index", "Home") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
                </li>
                <li><i class="fa fa-lg fa-angle-right"></i>
                </li>
                <li><span class="tittle-content-header"> Aviso de Pivacidad</span></li>
            </ul>

            <div class="content-wrap">
                <div class="row">


                    <div class="col-sm-12">

                        <div class="nest">
                            <div class="title-alt">
                                <h6><span class="tittle-content-header"> Aviso de Pivacidad</span></h6>
                            </div>

                            <div class="body-nest">
                              <p>Cuando sea necesario que el Usuario se registre o facilite datos personales  (entre otros supuestos, para acceder a servicios; solicitar información;  adquirir productos; remitir consultas,…) la recolección y el tratamiento de los  datos personales se llevará a cabo de conformidad y en cumplimiento con los  principios de la Ley Federal de Protección de Datos Personales en Posesión de  los Particulares y demás normatividad aplicable (LFPDPPP).</p>
                              <p>                                En su caso, el Usuario será advertido, convenientemente, de la necesidad de  facilitar sus datos personales. En caso de facilitar la dirección de correo  electrónico u otro medio de comunicación electrónica, el Usuario autoriza  expresamente a la Entidad para que dicho medio sea utilizado como medio de  comunicación con el mismo, para dar respuesta a su solicitud y/o consulta, así  como para poderle transmitir información relativa a la Entidad e informarle de  cualesquiera otros cambios relevantes que se produzcan en el Portal.</p>
                              <p>                                Dicho tratamiento de los datos es llevado a cabo de conformidad con los  referidos principios y, en particular, con sujeción al deber de  confidencialidad y secreto, habiendo adoptado la Entidad las medidas de  seguridad adecuadas para evitar cualquier alteración, pérdida, acceso no  autorizado o daño de los datos personales e información registrada.</p>
                              <p>                                El Usuario tiene  los derechos de  acceso, rectificación, cancelación y oposición (A.R.C.O), pudiendo limitar el  uso y divulgación de sus datos, o bien, cuando sea posible, revocar el  consentimiento que haya otorgado para su tratamiento. Para ello, deberá dirigir  una comunicación escrita, acompañada de su identificación, con la indicación:  &ldquo;Protección de Datos&rdquo; y la petición que formula a la siguiente dirección:</p>
                              <p align="center">Integr@  Soluciones de Parking S. de R.L de CV<br>
                                Avda  Gabriel Mancera #1041<br>
Col.  Del Valle Centro 03100<br>
Delegación  Benito Juárez, Distrito Federal<br>
                              <a href="mailto:Blinkay@integraparking.com">Blinkay@integraparking.com</a></p>
<h2>&nbsp;</h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="footer-space"></div>
            <div id="footer">
                <div class="copyright">
                    &copy; Integr@ Parking Solutions Inc., <%= DateTime.Now.Year.ToString() %>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript" src="<%= Url.Content("~/Content/js/preloader.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/app.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/load.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/main.js") %>"></script>

    <% Html.RenderPartial("~/Views/Shared/GoogleAnalytics.ascx"); %>

</body>
</html>