<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--
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
--%>
<%--
<div id="breadcrumb-wrapper" class="row">
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
        <li><%=Resources.Account_Recharge_BuyCredit%>
        </li>
    </ul>
</div>
--%>


<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
            <h3><%=Resources.Account_RechargeSuccessTitle%></h3>

                    <%  NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = "."; %>

                    <div class="alert alert-bky-success">
                        <span class="bky-ok"></span>
                        &nbsp;
                        <%=Resources.Account_RechargeSuccess_1%>
                        &nbsp;
                        <strong><%=string.Format(provider, "{0:0.00} {1}", Convert.ToDouble(ViewData["PayerQuantity"]) / 100.0, ViewData["PayerCurrencyISOCode"])%></strong>
                    </div>

                    <div class="alert alert-bky-info">
                        <%
                            if(Convert.ToDouble(ViewData["UserBalance"])<=0) 
                            { 
                                %>
                                    <span class="bky-info"></span> 
                                    &nbsp;
                                    <strong> <%=Resources.Account_RechargeSuccess_2%> </strong>
                                    <hr>
                                    <p><%=Resources.Account_RechargeSuccess_3%></p>
                                <% 
                            } 
                            else
                            { 
                                %>
                                    <span class="bky-info"></span>
                                    &nbsp;
                                    <strong> <%=Resources.Account_RechargeSuccess_3%> </strong>
                                <% 
                            } // if else UserBalance
                        %>
                    </div>

                    <div class="row-buttons">
                        <a class="btn btn-bky-primary" href="Main"><%=Resources.Account_RechargeButton_MainMenu%></a>
                        <% 
                            if (Session["ReturnToPermits"] != null)
                            {
                                if ((bool)Session["ReturnToPermits"])
                                {
                                    Session["ReturnToPermits"] = false;
                                    %>
                                        &nbsp;
                                        <a class="btn btn-bky-sec-primary" href="<%= Url.Action("PayForPermit", "Permits") %>"><%=Resources.Account_Main_BttnPermits%>&nbsp;(<span id="countdown"></span>)</a> 
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
                                } // if ((bool)Session["ReturnToPermits"])
                            } // if ReturnToPermits
                        %>                    
                    </div><!--//.row-buttons-->
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

</asp:Content>