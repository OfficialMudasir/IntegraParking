<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<PermitsModel>" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="integraMobile.Models" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>
    <%= resBundle.GetString("Permits_SignUp") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if (top.location != this.location) {
                $("html, body").css("display", "none");
                top.location = this.location;
            }
            else {
                $("html, body").css("display", "block");
            }
        });
    </script>
    <link href="<%= Url.Content("~/Content/Permits.css?rnd=1") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>    
<div id="loader"></div>        
    <div class="content-wrap">

        <%-- ROW STEPS // --%>
        <div id="Payment-Steps" class="row">
            <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12">
                <%-- OLD STEP LIST --%>
                <%--
                <div class="decoration-little">
                    <div>
                        <div class="bubble" id="Div3"><img src="<%= Url.Content("~/Content/img/Permits/1-blue.png")%>" /></div>
                        <div class="p-arrow p-arrow-light" id="Div4"><img src="<%= Url.Content("~/Content/img/Permits/arrow.png")%>" /></div>
                        <div class="bubble bubble-little" id="Div1"><img src="<%= Url.Content("~/Content/img/Permits/2-gray.png")%>" /></div>
                        <div class="p-arrow p-arrow-light" id="Div2"><img src="<%= Url.Content("~/Content/img/Permits/arrow.png")%>" /></div>
                        <div class="bubble bubble-little" id="Div5"><img src="<%= Url.Content("~/Content/img/Permits/3-gray.png")%>" /></div>
                    </div>
                    <div>
                        <div class="bubble-label label-blue"><%= resBundle.GetString("Permits_PaymentMethod")%></div>
                        <div class="bubble-label label-gray"><%= resBundle.GetString("Permits_PayForPermit")%></div>
                        <div class="bubble-label label-gray"><%= resBundle.GetString("Permits_Summary")%></div>
                    </div>
                </div>
                --%>            
                <div class="steps-dots">
                    <div class="step current">
                        <span class="number">1.</span>
                        <span class="step-desc"><%= resBundle.GetString("Permits_PaymentMethod")%></span>
                    </div>
                    <!-- .step.step-arrow -->
                    <div class="step step-arrow">
                        <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                    </div>
                    <div class="step disabled">
                        <span class="number">2.</span>
                        <span class="step-desc"><%= resBundle.GetString("Permits_PayForPermit")%></span>
                    </div>
                    <!-- .step.step-arrow -->
                    <div class="step step-arrow">
                        <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                    </div>                    
                    <div class="step disabled">
                        <span class="number">3.</span>
                        <span class="step-desc"><%= resBundle.GetString("Permits_Summary")%></span>
                    </div>
                </div><!--//.steps-->
            </div><!--//.col-lg-8.col-block-->
        </div><!--//#Payment-Steps.row-->
        <%-- // ROW STEPS --%>

        <%-- ROW CONTENTS //--%>
        <div class="row">
            <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12 col-block">
                <h3><span>1.</span> <%= resBundle.GetString("Permits_PaymentMethod")%></h3>
                <div class="alert alert-bky-warning notice">
                    <span class="bky-info"></span>
                    &nbsp;
                    <%= resBundle.GetString("Permits_Notice2")%>
                </div><!--//.alert.notice-->
                <div class="row-buttons">
                    <a class="btn btn-bky-primary" href="<%= Url.Action("SelectPayMethod", "Account", new { bForceChange = "True", ReturnToPermits = "True", InstallationId = ViewData["InstallationID"] }) %>">
                        <%= resBundle.GetString("Permits_Continue")%>
                        </a>
                </div>
            </div><!--//.col-lg-8.col-block-->
        </div><!--//.row-->
        <%-- // ROW CONTENTS --%>

    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            var installationId = getParameterByName("InstallationId");
            if (installationId != null) {
                $(".submit-button-little > a").prop("href", $(".submit-button-little > a").prop("href") + "&InstallationId=" + installationId);
            }
        });

        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

    </script>

</asp:Content>