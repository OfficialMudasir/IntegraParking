<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="ZendeskSSO.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post" action="SSO.aspx">
    <div>
        <input type="hidden" name="return_to" id="return_to" value="fdasdfasdfasd" />
        <input type="hidden" name="user_token" id="user_token" value="fasdfasdfasda" />

        <input type="submit" />
    </div>
    </form>
</body>
</html>
