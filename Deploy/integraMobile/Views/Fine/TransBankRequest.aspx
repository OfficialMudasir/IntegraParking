<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCtype html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>TransbankRedirect</title>
</head>
<body onload="trbaForm.submit();">

<form name="trbaForm" action="<%:ViewData["transbank_form_url"]%>" method="post">
    <input type="hidden" name="TBK_TOKEN" value="<%:ViewData["token"]%>" />
</form>
</body>
</html>
