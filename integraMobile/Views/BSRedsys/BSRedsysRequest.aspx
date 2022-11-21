<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCtype html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>BSRedsys Redirect</title>
</head>
<body onload="bsForm.submit();">

<form name="bsForm" action="<%:ViewData["bsredsys_form_url"]%>" method="post">
    <!-- Store Settings-->
    <input type="hidden" id="Ds_SignatureVersion" name="Ds_SignatureVersion" value="<%:ViewData["bsredsys_signature_version"]%>" />
    <input type="hidden" id="Ds_MerchantParameters" name="Ds_MerchantParameters" value="<%:ViewData["bsredsys_merchant_parameters"]%>" />
    <input type="hidden" id="Ds_Signature" name="Ds_Signature" value="<%:ViewData["bsredsys_signature"]%>" />    
    <input type="hidden" name="email" value="<%:ViewData["email"]%>" />    
</form>
</body>
</html>
