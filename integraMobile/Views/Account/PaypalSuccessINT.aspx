<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.Account_Main_BttnOperations%> --%>
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
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
        <li><%=Resources.Account_Main_BttnOperations%>
        </li>
    </ul>
</div>
--%>
<div class="content-wrap">
    <div class="row">
        <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-block">
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
                        if (Convert.ToDouble(ViewData["UserBalance"])<=0) 
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
                            <% } %>
                </div>

                <div class="row-buttons">
                    <a class="btn btn-primary" href="Main"><%=Resources.Account_RechargeButton_MainMenu%></a>
                </div><!--//.row-buttons-->
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

</asp:Content>