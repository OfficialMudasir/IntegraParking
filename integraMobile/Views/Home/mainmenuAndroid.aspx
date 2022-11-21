<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<script language=javascript>
    var redirectToApp = function () {
        setTimeout(function appNotInstalled() {
            window.location.replace("<%=ViewData["AndroidGooglePlayURL"].ToString()%>");
        }, 100);
        window.location.replace("<%=ViewData["AndroidDeepLink"].ToString()%>");
    };
    window.onload = redirectToApp;
</script>
</head>
<body></body>
</html>