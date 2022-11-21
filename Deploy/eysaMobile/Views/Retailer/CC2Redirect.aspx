<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CCRedirect</title>
</head>
<body onload="ConexFlow.submit();">

<form name="ConexFlow" id="ConexFlow" action="<%:ViewData["iecisa_form_url"]%>" method="post"> 

    <input type="hidden" value="<%:ViewData["iecisa_cf_xtntype"]%>"" name="CF_XtnType"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_user"]%>" name="CF_User"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_date"]%>" name="CF_Date"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_time"]%>" name="CF_Time"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_amount"]%>" name="CF_Amount"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_currency"]%>" name="CF_Currency"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_ticketnumber"]%>" name="CF_TicketNumber"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_lang"]%>" name="CF_Lang"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_mac"]%>" name="CF_MAC"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_urlreply"]%>" name="CF_UrlReply"> 
    <input type="hidden" value="<%:ViewData["iecisa_cf_template"]%>" name="CF_Template"> 

 </form>  

</body>
</html>
