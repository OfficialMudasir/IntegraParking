<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Register_PaymentMean%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="row">
    <div id="paper-top">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.ServiceName %>
                </span>
            </h2>

        </div>
    </div>
</div>

<ul id="breadcrumb">
    <li>
        <span class="entypo-home"></span>
    </li>
    <li><i class="fa fa-lg fa-angle-right"></i>
    </li>
    <li><a href="<%= Url.Action("Main", "Account") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
    </li>
    <li><i class="fa fa-lg fa-angle-right"></i>
    </li>
    <li><%=Resources.Account_Register_PaymentMean%>
    </li>
</ul>

<div class="content-wrap">
    <div class="row">


        <div class="col-sm-12">
            <div class="nest">
                <div class="title-alt">
                    <h6><%=Resources.Account_Register_PaymentMeanFinished%></h6>
                </div>

                <div class="body-nest">

                    <% NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = "."; %>

                    <div class="alert alert-success">
                        <span class="icon icon-checkmark"></span>
                        <%=string.Format(Resources.Account_Register_PaymentMeanMessage, string.Format(provider, "{0:0.00}", 
                        Convert.ToDouble( ViewData["PayerQuantity"].ToString())),ViewData["PayerCurrencyISOCode"]) %>
                    </div>                  
                    
                    <a class="btn btn-primary" href="Main"><%=Resources.Account_RechargeButton_MainMenu%></a>
        <% 
        if (Session["ReturnToPermits"] != null)
        {
            if ((bool)Session["ReturnToPermits"])
            {
                Session["ReturnToPermits"] = false;
                %>
                    <a class="btn btn-primary" href="<%= Url.Action("PayForPermit", "Permits") %>"><%=Resources.Account_Main_BttnPermits%>&nbsp;(<span id="countdown"></span>)</a> 
                    <script type="text/javascript">
                        var countdown_time = 4;
                        var i;
                        document.getElementById("countdown").innerHTML = countdown_time.toString();
                        i = setInterval('secondDown()', 1000);

                        function secondDown() {
                            if (countdown_time > 0) {
                                countdown_time--;
                                $("#countdown").html(countdown_time.toString());
                            }
                            else {
                                clearInterval(i);
                                document.location = "<%= Url.Action("PayForPermit", "Permits") %>";
                            }
                        }
                    </script>                                   
                <%
            }
        }
        %>
                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>