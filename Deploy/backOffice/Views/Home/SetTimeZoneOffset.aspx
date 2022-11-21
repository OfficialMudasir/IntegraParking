<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title></title>     
    <script language="javascript">
        function SetTimeOffset() {

            var d = new Date()
            var n = d.getTimezoneOffset();

            var url = '<%= Url.Action("SetTimeZoneOffsetAction") %>';
            url += "?utcoffset=" + n + "&redirectUrl=<%: Model %>";

            //var timezone = jstz.determine(); //jstimezonedetect.js
            //alert(timezone.name() + " " + n);

            window.location = url;

        }
    </script>
</head>
<body onload="SetTimeOffset();">
    <div>
        
    </div>
</body>
</html>
