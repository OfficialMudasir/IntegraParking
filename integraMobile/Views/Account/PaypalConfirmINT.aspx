<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
                <h3><%=Resources.Account_RechargeConfirmTitle%></h3>
                    <div class="alert alert-bky-success">
                        <span class="bky-ok"></span>
                        &nbsp;
                        <%=Resources.Account_RechargeConfirm_1%> 
                        &nbsp;
                        <%=Resources.Account_RechargeConfirm_2%> 
                        &nbsp;
                        <strong><%= ViewData["PayerQuantity"]%> <%= ViewData["PayerCurrencyISOCode"]%></strong>
                    </div>

                    <div class="alert alert-bky-info">
                        <%
                            if (Convert.ToDouble(ViewData["UserBalance"])<=0) 
                            { 
                                %>
                                    <span class="bky-info"></span> 
                                    &nbsp;
                                    <strong> <%=Resources.Account_RechargeConfirm_3%> </strong>
                                    <hr>
                                    <p><%=Resources.Account_RechargeConfirm_4%></p>
                                <% 
                            } 
                            else 
                            { 
                                %>
                                    <span class="bky-info"></span> 
                                    &nbsp;
                                    <strong> <%=Resources.Account_RechargeConfirm_4%> </strong>
                                <% 
                            } // if else UserBalance
                        %>
                    </div>

                    <div class="row-buttons">
                        <a class="btn btn-bky-primary" href="<%= Url.Action("PaypalSuccessINT", "Account") %>"><%=Resources.Account_RechargeButton_Confirm%></a>
                        &nbsp;
                        <a class="btn btn-bky-sec-default" href="PaypalCancelINT"><%=Resources.Account_RechargeButton_Cancel%></a>
                    </div><!--//.row-buttons-->
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

</asp:Content>