<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
                                    <strong><span class="bky-info"></span> <%=Resources.Account_RechargeConfirm_3%></strong>
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
                        <a class="btn btn-bky-primary" href="<%= Url.Action("PaypalSuccess", "Account") %>"><%=Resources.Account_RechargeButton_Confirm%></a>
                        &nbsp;
                        <a class="btn btn-bky-sec-default" href="PaypalCancel"><%=Resources.Account_RechargeButton_Cancel%></a>
                    </div><!--//.row-buttons-->
            
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

</asp:Content>