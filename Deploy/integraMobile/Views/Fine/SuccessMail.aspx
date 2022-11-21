<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!doctype html>
<html lang="<%=Session["Culture"].ToString().Substring(0,2)%>">

<head>
	<title></title>
	<!-- Required meta tags -->
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

	<!-- Bootstrap CSS -->
    <%-- TGA@Blinkay :: Change bootstrap version powered by Blinkay  --%>
	<%-- <link rel="stylesheet" href="../Content/css/2020/bootstrap.min.css"> --%>
	<link rel="stylesheet" href="../Content/css/2020/blinkay-bs4.css?v=4.4.1">
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" integrity="sha384-B4dIYHKNBt8Bc12p+WXckhzcICo0wtJAoU8YZTY5qE0Id1GSseTk6S+L3BlXeVIU" crossorigin="anonymous">


	<link rel="stylesheet" href="../Content/css/2020/<%=(Session["Theme"] != null ? Session["Theme"] : "theme-Blinkay")%>.css">
	<link rel="stylesheet" href="../Content/css/2020/styles.css">
	<script src="../Content/js/2020/jquery-3.3.1.slim.min.js"></script>

</head>

<body class="theme-light" style="background-color:white!important;">
	<div id="page-content" style="background-color:white!important;">
		<div class="row justify-content-center">
			<div id="form-win-container" class=" col col-lg-8" style="box-shadow:none!important;">

<% 
if (Session["strRechargeEmailSubject"] != null && Session["strRechargeEmailBody"] != null)
{
    using (Html.BeginForm("SuccessMail", "Fine", FormMethod.Post, new { @class = "form-horizontal", @role = "form" }))
    {
%>
                <div class="row justify-content-center">
	                <div class="col-12 col-lg-8">
		                <p class="lead"><%=Resources.Fine_GetProofPre %></p>
		                <!--<p><%=Resources.Fine_GetProof %>:</p>-->
	                </div><%-- // .col-12 col-lg-6 --%>
                </div><%-- // .row.justify-content-center --%>
                <!--  MAIL FIELDS-->
                <div class="row justify-content-center">
                    <!-- INSERT MAIL-->
	                <div class="col-12 col-lg-8 form-group">
		                <label for="email" class="col-form-label"><% =Resources.FineModel_WriteYourEmail %></label>
		                <input id="email" name="email" class="form-control form-control-md input-text" type="email" minlength="10" placeholder="<% =Resources.FineModel_ValidEmail %>"  required autocomplete="off" title="<% =Resources.FineModel_ValidEmail %>">
	                </div><%-- // .col-12 col-lg-6 form-group --%>
                </div><%-- // .row.justify-content-center --%>
                <%--<div class="row justify-content-center">--%>

                    <!-- REPEAT MAIL-->
	                <%--<div class="col-12 col-lg-8 form-group">--%>
		                <%--<label for="email_confirm" class="col-form-label"><% =Resources.FineModel_WriteYourConfirmEmail %></label>--%>
		                <%--<input id="email_confirm" name="email_confirm" class="form-control form-control-md input-text" type="email" minlength="10" placeholder="<% =Resources.FineModel_ValidEmail %>" pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,10}" required autocomplete="off" title="<% =Resources.FineModel_ValidEmail %>">--%>
	        <%--        </div>--%>
            <%-- // .col-12 col-lg-6 form-group --%>
                <%--</div>--%>
            <%-- //. row justify-content-center --%>
                <!-- FORM SUBMIT BUTTONS	-->
                <div class="row justify-content-center">
	                <div class="col-12 col-lg-6 text-center">
		                <input class="btn btn-primary btn-block p-3" type="submit" value="<%=Resources.Fine_SendMail %>">
	                </div>
                </div><%-- //. row justify-content-center --%>
                <%
            
        if (Session["validEmail"] != null && (Session["validEmail"].ToString() == "2" || Session["validEmail"].ToString() == "3"))
            {
                /* TGA@Blinkay :: Show Resources.Fine_MailSent with style  */
                /* Response.Write(Resources.Fine_MailSent); */
            %>
                <br />
                <div class="alert alert-danger" role="alert">
                    <%= Resources.urs_error_email%>   
                </div>
            <%
            }
    }
}
else {
    /* TGA@Blinkay :: Show Resources.Fine_MailSent with style  */
    /* Response.Write(Resources.Fine_MailSent); */
%>
    <div class="alert alert-info" role="alert">
        <%= Resources.Fine_MailSent%>   
    </div>
<%

}
%>
            </div>
        </div>
    </div>
<script type="text/javascript">
    $("form").on("submit", function () {
        if ($("#email").val().trim() != "" && $("#email").val().trim() == $("#email_confirm").val().trim()) {
            $("input[type=submit]").prop("disabled", "disabled");
            return true;
        }
        else if ($("#email").val().trim() == "") {
            $("#email").css("background-color", "lightpink");
            return false;
        }
        else {
            $("#email_confirm").css("background-color", "lightpink");
            return false;
        }
        

    });
</script>
</body>
</html>
