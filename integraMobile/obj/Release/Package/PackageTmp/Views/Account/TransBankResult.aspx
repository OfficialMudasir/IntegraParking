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
                    <div class="col-md-12 col-block">
                        <h3><%=Resources.Account_RechargeSuccessTitle%></h3>
                        <%  NumberFormatInfo provider = new NumberFormatInfo(); provider.NumberDecimalSeparator = "."; %>
                        
                        <div class="alert alert-bky-success">
                            <span class="bky-done"></span> 
                            &nbsp;  
                            <%=Resources.Account_RechargeSuccess_1%> 
                            &nbsp; 
                            <strong><%=string.Format(provider, "{0:0.00} {1}", Convert.ToDouble(ViewData["UserBalance"]) / 100.0, ViewData["PayerCurrencyISOCode"])%></strong>
                        </div>

                        <div class="alert alert-bky-info">
                            <%
                                if(Convert.ToDouble(ViewData["UserBalance"])<=0) 
                                { 
                                    %>
                                        <span class="bky-info"></span> 
                                        &nbsp;
                                        <strong><%=Resources.Account_RechargeSuccess_2%></strong>
                                        <hr>
                                        <p><%=Resources.Account_RechargeSuccess_3%></p>
                                    <% 
                                } 
                                else 
                                {
                                    %>
                                        <span class="bky-info"></span> 
                                        &nbsp;
                                        <strong><%=Resources.Account_RechargeSuccess_3%></strong>
                                    <% 
                                }  // if else UserBalance
                            %>
                        </div>
                        
                        <%-- BUTTONS --%>
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
                                    } // if (bool)Session["ReturnToPermits"]
                                } // if Session["ReturnToPermits"] 
                            %>
                        </div>
                
                    </div><!--// .col-sm-12.col-block -->
                </div><!--// .row -->
            </div><!--// .content-wrap / IF j.result -->
        <%        
    }
    else 
    {
        %>
            <div class="content-wrap">
                <div class="row">
                    <div class="col-md-12 col-block">
                        <h3><%=Resources.Account_RechargeFailureTitle%></h3>
                        
                        <div class="alert alert-bky-danger">
                            <span class="bky-cancel"></span> 
                            &nbsp;
                            <%=Resources.Account_RechargeFailure_1%>
                            <%
                                if(Convert.ToDouble(ViewData["UserBalance"])<=0) 
                                { 
                                    %> 
                                        <hr>
                                        <%=Resources.Account_RechargeFailure_2%>
                                        &nbsp;
                                        <%=Resources.Account_RechargeFailure_3%>
                                    <% 
                                } 
                            %>
                        </div><!--//.alert.alert-bky-danger -->

                        <div class="alert alert-bky-danger">
                            <span class="bky-info"></span> &nbsp; <strong> <%=j.errorMessage%> </strong>
                        </div>

                        <div class="alert alert-bky-info">
                            <%
                                if(Convert.ToDouble(ViewData["UserBalance"])<=0) 
                                { 
                                    %>
                                        <span class="bky-info"></span> 
                                        &nbsp; 
                                        <strong><%=Resources.Account_RechargeFailure_4%></strong>
                                        <hr>
                                        <p><%=Resources.Account_RechargeFailure_5%></p>
                                    <% 
                                } 
                                else 
                                { 
                                    %>
                                        <span class="bky-info"></span> 
                                        &nbsp;
                                        <strong><%=Resources.Account_RechargeFailure_5%></strong>
                                    <% 
                                } // if UserBalance
                            %>
                        </div>

                        <%-- BUTTONS --%>
                        <div class="row-buttons">
                            <a class="btn btn-bky-primary" href="Main"><%=Resources.Account_RechargeButton_MainMenu%></a>
                            &nbsp;
                            <a class="btn btn-bky-sec-info" href="Recharge"><%=Resources.Account_RechargeButton_Recharge%></a>
                        </div><!--//.row-buttons-->

                    </div><!--//.col-block / else -->
                </div><!--// .row / Else -->
            </div><!--// .content-wrap / ELSE j.result -->
        <%        
    } // if else j.result
%>
</asp:Content>