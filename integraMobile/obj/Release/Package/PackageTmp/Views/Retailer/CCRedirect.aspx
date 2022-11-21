<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>CCRedirect</title>
</head>
<body onload="ccForm.submit();">

	<form name="ccForm" id="ccForm" action="<%:ViewData["ekashu_form_url"]%>" method="post"> 
		<input type="hidden" name="ekashu_seller_id" value="<%:ViewData["ekashu_seller_id"]%>"> 
		<input type="hidden" name="ekashu_seller_key" value="<%:ViewData["ekashu_seller_key"]%>"> 
		<input type="hidden" name="ekashu_amount" value="<%:ViewData["ekashu_amount"]%>"> 
		<input type="hidden" name="ekashu_currency" value="<%:ViewData["ekashu_currency"]%>"> 
		<input type="hidden" name="ekashu_auto_confirm" value="False">
		<input type="hidden" name="ekashu_card_address_verify" value="False">
		<input type="hidden" name="ekashu_card_zip_code_verify" value="False"> 
		<input type="hidden" name="ekashu_duplicate_check" value="resend"> 
		<input type="hidden" name="ekashu_reference" value="<%:ViewData["ekashu_reference"]%>">
		<input type="hidden" name="ekashu_description " value="<%:ViewData["ekashu_description"]%>">
		<input type="hidden" name="ekashu_seller_name" value="<%:ViewData["ekashu_seller_name"]%>">
		<input type="hidden" name="ekashu_failure_url" value="<%:ViewData["ekashu_failure_url"]%>">
		<input type="hidden" name="ekashu_return_url" value="<%:ViewData["ekashu_return_url"]%>">
		<input type="hidden" name="ekashu_success_url" value="<%:ViewData["ekashu_success_url"]%>">
	<% if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CREDIT_CALL_HASH_KEY"])) { %>
		<input type="hidden" name="ekashu_hash_code" value="<%:ViewData["ekashu_hash_code"]%>">
		<input type="hidden" name="ekashu_hash_code_format" value="base64">
		<input type="hidden" name="ekashu_hash_code_type" value="SHA1">
		<input type="hidden" name="ekashu_hash_code_version" value="1.0.0">
	<% } %>
	</form> 

</body>
</html>
