<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.ServiceName%> --%>
    <%=Resources.Account_Recharge_BuyCredit %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%-- 
<div class="row">
    <div id="Div1">
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
<%
    dynamic j = Json.Decode(ViewData["Result"].ToString());    
    if (j.result == "succeeded")
    {
        %>
            <div class="content-wrap">
                <div class="row">
                    <div class="col-sm-12 col-block">
                        <h3><%=Resources.Account_Register_PaymentMeanFinished%></h3>

                        <% NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = "."; %>

                        <div class="alert alert-bky-success text-center">
                            <p><span class="bky-done big-icon"></span></p>
                            <p><%=string.Format(Resources.Account_Register_PaymentMeanMessage, string.Format(provider, "{0:0.00}", 
                            Convert.ToDouble( ViewData["PayerQuantity"].ToString())/100),ViewData["PayerCurrencyISOCode"]) %></p>
                        </div>
                    
                        <p class="row-buttons">
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
                                    }
                                }
                            %>
                        </p><!-- .row-buttons-->
                    </div><!--//.col-sm-12,col-block-->
                </div><!--//.row-->
            </div><!--//.content-wrap / if j.result --->
        <%        
    }
    else 
    { 
        %>
            <div class="content-wrap">
                <div class="row">
                    <div class="col-sm-12 col-block">
                        <h3><%=Resources.Account_Register_PaymentMeanError%></h3>

                        <div class="alert alert-bky-success">
                            <p><span class="bky-done big-icon"></span></p>
                            <p>
                                <%= string.Format(Resources.Account_Register_PaymentMeanMessageError, string.Format(provider, "{0:0.00}", 
                                    Convert.ToDouble( ViewData["PayerQuantity"].ToString())/100),ViewData["PayerCurrencyISOCode"]) %>
                            </p>
                        </div>
                        
                        <% NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = "."; %>

                        <div class="alert alert-bky-info">
                            <p>
                                <span class="bky-info"></span>
                                &nbsp;
                                <strong><%=Resources.Account_Register_PaymentMean_ChangeToPrepay%></strong>
                            </p>
                            <hr>
                            <ol>
                                <li><%=Resources.SelectSuscriptionType_Prepay2%> - <%=string.Format(Resources.SelectSuscriptionType_Prepay3,ViewData["DiscountValue"],ViewData["DiscountCurrency"])%></li> 
                                <li><%=Resources.SelectSuscriptionType_Prepay4%></li> 
                                <li><%=Resources.SelectSuscriptionType_Prepay5%></li> 
                                <li><%=Resources.SelectSuscriptionType_Prepay6%></li> 
                                <li><%=Resources.SelectSuscriptionType_Prepay7%></li> 
                            </ol>
                        </div>
                        
                        <div class="row-buttons">
                            <a class="btn btn-primary" href="SelectPayMethod"><%=Resources.Account_Register_PaymentMeanRetry%></a>
                        </div>
                    </div><!--//.col-sm-12.col-block -->
                </div><!--//.row-->
            </div><!--//.content-wrap / else j.result -->
        <%        
    } //  if else j.result
%>
</asp:Content>