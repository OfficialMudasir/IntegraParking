<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_RechargeSuccessTitle%>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div class="row">
    <div id="Div1">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.ServiceName %></span>
            </h2>

        </div>
    </div>
</div>
--%>
<%-- TGA@BKY -- Eliminated breadcrumb 
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
                    <div class="col-md-8 col-md-offset-2 col-sm-12 col-block">
                        <h3><%=Resources.Account_RechargeSuccessTitle%></h3>
                    </div><!-- // .col-sm-12.title-content -->
                </div><!--// .row -->


                <%-- ALERTS CONTENT //  --%>
                <div id="content-alerts" class="row">
                    <div class="col-md-8 col-md-offset-2 col-sm-12 col-block">

                                <%  NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = "."; %>

                                <div class="alert alert-bky-success">
                                    <span class="bky-done"></span>
                                    &nbsp;
                                    <%=Resources.Account_RechargeSuccess_1%>
                                    &nbsp;
                                    <strong><%=string.Format(provider, "{0:0.00} {1}", Convert.ToDouble(ViewData["UserBalance"]) / 100.0, ViewData["PayerCurrencyISOCode"])%></strong>
                                </div>

                                <div class="alert alert-bky-info">
                                    <%if(Convert.ToDouble(ViewData["UserBalance"])<=0) { %>
                                        <p>
                                            <span class="bky-info"></span> 
                                            &nbsp; 
                                            <%=Resources.Account_RechargeSuccess_2%>
                                        </p>
                                        <p><%=Resources.Account_RechargeSuccess_3%></p>
                                    <% } else { %>
                                        <p>
                                            <span class="bky-info"></span> 
                                            &nbsp;
                                            <%=Resources.Account_RechargeSuccess_3%>
                                        </p>
                                    <% } %>
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
                                            }
                                        }
                                    %>
                                </div><!--//.row-buttons-->
                    </div><!-- #content-alerts col-md-8 -->
                </div><!-- //   #content-alerts.row -->
                <%-- // ALERTS CONTENT--%>


            </div><!-- #content-wrap -->

        <%        
    }
    else 
    { 
        %>
            <div class="content-wrap">
                
                <div class="row"  >
                    <div class="col-md-8 col-md-offset-2 col-sm-12 col-block">
                        <h3><%=Resources.Account_RechargeFailureTitle%></h3>
                    </div>
                </div>

                <%-- ALERTS CONTENT //  --%>
                <div id="content-alerts" class="row">
                    <div class="col-md-8 col-md-offset-2 col-block">
                        <div class="alert alert-bky-danger">
                            <p>
                                <span class="bky-cancel"></span> 
                                &nbsp; 
                                <%=Resources.Account_RechargeFailure_1%>
                            </p>
                            <%
                                if (Convert.ToDouble(ViewData["UserBalance"])<=0) 
                                { 
                                    %> 
                                        <p>
                                            <%=Resources.Account_RechargeFailure_2%> 
                                            &nbsp; 
                                            <%=Resources.Account_RechargeFailure_3%>
                                        </p>

                                    <% 
                                } // if UserBalance
                            %>
                            <hr>
                            <p>
                                <span class="bky-info"></span> 
                                &nbsp; 
                                <%=j.errorMessage%>
                                </strong>
                            </p>
                        </div>

                        <div class="alert alert-bky-info">
                            <%
                                if (Convert.ToDouble(ViewData["UserBalance"])<=0) 
                                { 
                                    %>
                                        <p>
                                            <span class="bky-info"></span> 
                                            &nbsp; 
                                            <%=Resources.Account_RechargeFailure_4%>
                                        </p>
                                        <p><%=Resources.Account_RechargeFailure_5%></p>
                                    <% 
                                } 
                                else 
                                { 
                                    %>
                                        <p>
                                            <span class="bky-info"></span> 
                                            &nbsp; 
                                            <%=Resources.Account_RechargeFailure_5%>
                                        </p>
                                    <% 
                                } // if else UserBalance
                            %>
                        </div>

                    </div><!-- #content-alerts col-md-8 -->
                </div><!-- //   #content-alerts.row -->
                <%-- // ALERTS CONTENT--%>
                
                <!-- ROW BUTTONS -->
                <div class="row">
                    <div class="col-md-8 col-md-offset-2 row-buttons col-block">
                        <a class="btn btn-bky-primary" href="Recharge"><%=Resources.Account_RechargeButton_Recharge%></a>
                        <a class="btn btn-bky-sec-primary" href="Main"><%=Resources.Account_RechargeButton_MainMenu%></a>
                    </div><!--// .col-md-12 -->
                </div><!--// .row -->
            </div>
        <%        
    } // if else j.result
%>
</asp:Content>