<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCtype html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>MonerisRedirect</title>
</head>
<body onload="monForm.submit();">

<form name="monForm" action="<%:ViewData["moneris_form_url"]%>" method="post">
    <!-- Store Settings-->
    <input type="hidden" name="hpp_id" value="<%:ViewData["moneris_hpp_id"]%>" />
    <input type="hidden" name="ticket" value="<%:ViewData["moneris_ticket"]%>" />
    <input type="hidden" name="hpp_preload" />
</form>
</body>
</html>
