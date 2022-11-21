<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.LogOnModel>" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    
    <%--    manifest.json    --%>
    <%--    https://w3c.github.io/manifest/    --%>
    <%--    https://developer.mozilla.org/en-US/docs/Web/Manifest    --%>
    <%-- <link rel="manifest" type="application/manifest+json" href="~/manifest.webmanifest"> --%>
    <link rel="manifest" type="application/manifest+json" href="~/manifest.json">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no">
    <meta name="mobile-web-app-capable" content="yes">

    <%--  https://developer.mozilla.org/en-US/docs/Web/Manifest#icons      --%>
    <link rel="shortcut icon"   sizes="48x48"     href="launcher-icon-1x.png">
    <link rel="shortcut icon"   sizes="96x96"     href="launcher-icon-2x.png">
    <link rel="shortcut icon"   sizes="128x128"   href="icon-128x128.png">
    <link rel="shortcut icon"   sizes="144x144"   href="ms-touch-icon-144x144-precomposed.png">
    <link rel="shortcut icon"   sizes="152x152"   href="apple-touch-icon.png">
    <link rel="shortcut icon"   sizes="192x192"   href="chrome-touch-icon-192x192.png">
    <link rel="shortcut icon"   sizes="196x196"   href="icon-196x196.png">
    <link rel="shortcut icon"   sizes="256x256"   href="icon-256x256.png">


    <%--   https://developer.apple.com/library/archive/documentation/AppleApplications/Reference/SafariWebContent/ConfiguringWebApplications/ConfiguringWebApplications.html  ---%>
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent">
    <%-- iPhone ICON default --%>
    <link rel="apple-touch-icon" href="~/Content/ico/apple-touch-icon.png"/>
    <%-- iPhone ICON --%>
    <link rel="apple-touch-icon" sizes="57x57" href="~/Content/ico/apple-touch-icon-57x57.png" />
    <%-- iPad ICON--%>
    <link rel="apple-touch-icon" sizes="72x72" href="~/Content/ico/apple-touch-icon-72x72.png" />
    <%-- iPhone (Retina) ICON--%>
    <link rel="apple-touch-icon" sizes="114x114" href="~/Content/ico/apple-touch-icon-114x114.png" />
    <%-- iPad (Retina) ICON--%>
    <link rel="apple-touch-icon" sizes="144x144" href="~/Content/ico/apple-touch-icon-144x144.png" />
    <%-- iOS ICON precomposed--%>
    <link rel="apple-touch-icon-precomposed" sizes="128x128" href="~/Content/ico/apple-touch-icon-precomposed-128x128.png">

    <!-- --->

    <link rel="apple-touch-startup-image" href="apple-touch-startup-image.png">

<%--
    <!-- iPhone SPLASHSCREEN-->
    <link href="apple-touch-startup-image-320x460.png" media="(device-width: 320px)" rel="apple-touch-startup-image">
    <!-- iPhone (Retina) SPLASHSCREEN-->
    <link href="apple-touch-startup-image-640x920.png" media="(device-width: 320px) and (-webkit-device-pixel-ratio: 2)" rel="apple-touch-startup-image">
    <!-- iPad (portrait) SPLASHSCREEN-->
    <link href="apple-touch-startup-image-768x1004.png" media="(device-width: 768px) and (orientation: portrait)" rel="apple-touch-startup-image">
    <!-- iPad (landscape) SPLASHSCREEN-->
    <link href="apple-touch-startup-image-748x1024.png" media="(device-width: 768px) and (orientation: landscape)" rel="apple-touch-startup-image">
    <!-- iPad (Retina, portrait) SPLASHSCREEN-->
    <link href="apple-touch-startup-image-1536x2008.png" media="(device-width: 1536px) and (orientation: portrait) and (-webkit-device-pixel-ratio: 2)" rel="apple-touch-startup-image">
    <!-- iPad (Retina, landscape) SPLASHSCREEN-->
    <link href="apple-touch-startup-image-2048x1496.png" media="(device-width: 1536px)  and (orientation: landscape) and (-webkit-device-pixel-ratio: 2)" rel="apple-touch-startup-image">
    <!-- iPhone 6/7/8 -->
    <link href="/images/favicon/750x1334.png" media="(device-width: 375px) and (-webkit-device-pixel-ratio: 2)" rel="apple-touch-startup-image" />
    <!-- iPhone 6 Plus/7 Plus/8 Plus -->
    <link href="/images/favicon/1242x2208.png" media="(device-width: 414px) and (-webkit-device-pixel-ratio: 3)" rel="apple-touch-startup-image" />
--%>

    <title><%=Resources.ServiceName %> | Blinkay</title>

    
    <%-- <link rel="stylesheet" href="~/Content/CSS/bootstrap.min.css"> --%>
    <%-- link rel="stylesheet" href="~/Content/CSS/signin.css" --%>
    <link rel="stylesheet" href="~/Content/CSS/blinkay.css">
    <link rel="stylesheet" href="~/Content/CSS/logon.css">
    <link rel="stylesheet" href="~/Content/CSS/loader-style.css">

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
        <script src="https://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->
    <!-- Fav and touch icons -->
    <link rel="shortcut icon" href="~/Content/ico/favicon.ico">
</head>

<body onload="GetTimeZoneOffset();">
    
    <div id="preloader">
        <div id="status"></div>
    </div>
    <div class="circle-res"><!-- Circle Decoration Resource --></div>

    <div id="login-wrapper">
        

            <div id="login-form" <% if (ViewData.ModelState.Count > 0) Response.Write(" class='wrong-form' ");  %> >

                <div id="logo-login" class="text-center">
                    <%--  <a href="<%= Url.Action("Index", "Home") %>"><img src="<%= Url.Content("~/Content/img/Blinkay-login.svg") %>" alt="Blinkay"></a> --%>
                    <div><a href="/"><img src="<%= Url.Content("~/Content/img/Blinkay-login.svg") %>" alt="Blinkay" ></a></div>
                    <!-- NAVBAR LANGUAGES   //  -->
                    <nav id="language-selector" class="navbar navbar-languages navbar-center">
                        <div class="row text-center">
                            <!-- Brand and toggle get grouped for better mobile display -->
                            <div class="navbar-header">
                                <button type="button"  class="navbar-toggle collapsed" data-toggle="collapse" data-target="#language-list" aria-expanded="false">
                                    <%=Resources.Account_ChangeLanguage%>
                                    <span class="sr-only">Toggle languages</span>
                                </button>
                            </div><!-- /.navbar-header -->

                            <!-- Collect the nav links, forms, and other content for toggling -->
                            <div id="language-list" class="collapse navbar-collapse">
                                <hr class="lang-separator">
                                <ul class="language-select nav nav-pills nav-justified ">
                                    <% Html.RenderPartial("CultureChooserUserControl"); %>
                                </ul>
                                <hr class="lang-separator">

                            </div><!-- /#language-list -->
                        </div><!-- /.container-fluid -->
                    </nav><!-- // navbar-languages  -->
                    <!--    //  NAVBAR LANGUAGES -->

                
                </div><!-- //   logo-login  -->

                <div class="account-box">
                    <% 
                        using (Html.BeginForm("LogOn", "Home", FormMethod.Post, new { autocomplete="off", @class="form", @role="form" })) 
                        { 
                            %>
                        
                            <% 
                                foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                                {
                                    foreach (ModelError modelError in keyValuePair.Value.Errors) 
                                    { 
                                        %>
                                            <div class="alert alert-danger">
                                                <button data-dismiss="alert" class="close" type="button">×</button>
                                                <span class="entypo-attention"></span>
                                                <%= Html.Encode(modelError.ErrorMessage) %>
                                            </div><!-- //   alert danger    -->
                                        <% 
                                    } /* ModelState*/
                                } /* KeyValuePair */ 
                            %>
                        <div class="form-group">
                            <%-- <label for="UserName"><%=Resources.CustomerInscriptionModel_Email%></label> --%>
                            <div class="input-group">
                                <span class="input-group-addon" title="<%= Resources.CustomerInscriptionModel_Email %>"></span>

                                <%: Html.TextBoxFor(m => m.UserName, new {
                                        autocomplete = "off", 
                                        tabindex = 1,
                                        @required="required",
                                        @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.Home_Username)+"');", 
                                        @oninput="this.setCustomValidity('');",
                                        @class="form-control",
                                        @type="email",
                                        placeholder=Resources.CustomerInscriptionModel_Email
                                    }) 
                                %>
                            </div><!--  //  input username  -->
                        </div><!--  //  Username Group  -->
                        <div class="form-group">
                            
                            <%--    <label for="Password"><%=Resources.Home_Password%></label>  --%>
                            <div class="input-group">
                                <span class="input-group-addon" title="<%= Resources.Home_Password %>"></span>
                                <%: Html.PasswordFor(m => m.Password, new  {
                                        autocomplete = "off",  
                                        tabindex = 2,
                                        @required="required", 
                                        @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.Home_Password)+"');", 
                                        @oninput="this.setCustomValidity('');",
                                        @class="form-control",
                                        placeholder=Resources.Home_Password
                                    }) 
                                %>
                                <a class="input-group-addon" onclick="javascript:showpassword(this);" href="#"></a>
                            </div><!-- //   input password  -->
                            <div class="link-helper text-right">
                                <a href="<%= Url.Action("ForgotPassword", "Account") %>" class="link-forgot green-link"><%=Resources.Home_ForgotPassword%></a>
                            </div><!-- //   forgot password  --> 
                        </div><!-- //   Password Group  -->

                        <%-- 
                        <div class="checkbox pull-left">
                            <label>
                                <%: Html.CheckBoxFor(m => m.RememberMe, new { 
                                        tabindex = 3
                                    })
                                %><%=Resources.RememberMeCheck%>
                            </label>
                        </div>
                        --%>
                        <div class="form-group text-center">
                            <input id="utcoffset" name="utcoffset" type ="hidden" value="">
                            <button class="btn btn-bky-primary" type="submit">
                                <%=Resources.Button_Logon%>
                            </button>
                        </div><!--   //  send button/.form-group -->

                            <% 
                        } 
                    %>
                </div>
                <div class="link-helper text-center" >
                    <a href="<%= Url.Action("Step1", "Registration") %>" class="link-new-user green-link">
                        <%=Resources.Home_AddNewUser%>
                    </a>
                </div>


                <div class="btn-sec">


                    <%  //  Add separator if COUPON or PAYTICKET are ACTIVATED
                        if ( (ConfigurationManager.AppSettings["COUPON_ENABLED"] != null && ConfigurationManager.AppSettings["COUPON_ENABLED"].ToString().ToLower() == "true") 
                        || (ConfigurationManager.AppSettings["EXTERNAL_TICKET_PAYMENTS_ENABLED"] != null && ConfigurationManager.AppSettings["EXTERNAL_TICKET_PAYMENTS_ENABLED"].ToString().ToLower() == "true") )
                        {


                            Response.Write("<hr class=\"x\">");
                        }
                    %>
                    <%  //  CUPON ARE ENABLED
                        if (ConfigurationManager.AppSettings["COUPON_ENABLED"] != null && ConfigurationManager.AppSettings["COUPON_ENABLED"].ToString().ToLower() == "true") {    
                    %>
                    <a href="<%= Url.Action("Retailer", "Retailer") %>" class="btn btn-bky-sec-info">
                        <span class="bky-QR"></span> &nbsp; <%=Resources.Home_BuyCoupons%>
                    </a>
                    <% } %>
                    <%  //  PAY TICKET ARE ACTIVATED
                        if (ConfigurationManager.AppSettings["EXTERNAL_TICKET_PAYMENTS_ENABLED"] != null && ConfigurationManager.AppSettings["EXTERNAL_TICKET_PAYMENTS_ENABLED"].ToString().ToLower() == "true")
                        {    
                    %>
                    <a href="<%= Url.Action("Fine", "Fine") %>" class="btn btn-bky-sec-danger">
                        <span class="bky-ticket"></span>  &nbsp; <%=Resources.Fine_Title%>
                    </a>
                    <% } %>
                </div>

            </div><!-- // login-form -->

        <!-- ***    LOGIN FOOTER  *** -->
        <div id="login-footer" class="container">
            <div id="login-disclaimer" class="row" >
                <ul class="bottom-menu">
                    <li><a href="<%= Url.Action("gCond_"+ViewData["lang_for_gCond"], "Home") %>"><%=Resources.Home_Service_Conditions %></a></li>
                    <%--<li><a href="<%=Url.Content( "~/Content/docs/WhyService_"+ViewData["lang_for_gCond"]+".pdf")%>"><%=Resources.Home_WhyService %></a></li>--%>
                    <li><a href="http://www.blinkay.com"><%=Resources.Home_WhyService %></a></li>
                </ul>
            </div><!-- //   login-disclaimer -->

            <div class="row copyright">
                <h6>&copy; Blinkay, <%= DateTime.Now.Year.ToString() %></h6>
            </div>
        </div>

    </div><!--  //#login-wrapper   -->
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <%-- <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.3/jquery-ui.min.js"></script> --%>
    <script type="text/javascript">
        function GetTimeZoneOffset() {
            if (document.getElementById('utcoffset')) {
                document.getElementById('utcoffset').value = 0;
            }

            try
            {
                var d = new Date();
                var n = d.getTimezoneOffset();

                if (document.getElementById('utcoffset')) {
                    document.getElementById('utcoffset').value = n;
                }
            }
            catch(e)
            {
                console.error(e);
            }
        }
    </script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/preloader.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/app.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/load.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/main.js") %>"></script>
    <script>
        //  SHOW PASSWORD
        function showpassword(btn) {
            console.log(btn);
            
            var x = document.getElementById("Password");
            if(x.value.length > 0) 
            {
                console.log('show password');
                if (x.type === "password") {
                    x.type = "text";
                    btn.innerText = ""

                } else {
                    x.type = "password";
                    btn.innerText = ""
                }
            }
        }

        // ERROR SHAKE ALERT
        //$(".alert").effect( "shake", {times:4}, 1000 );
    </script>
    <% Html.RenderPartial("~/Views/Shared/GoogleAnalytics.ascx"); %>
</body>
</html>