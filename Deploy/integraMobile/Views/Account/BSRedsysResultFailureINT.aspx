<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Recharge_BuyCredit%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_Main_BttnOperations%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

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

<div class="content-wrap">
    <div class="row">


        <div class="col-sm-12">
            <div class="nest">
                <div class="title-alt">
                    <h6><%=Resources.Account_RechargeFailureTitle%></h6>
                </div>

                <div class="body-nest">

                    <div class="alert alert-danger">
                        <span class="entypo-cancel-circled"></span>
                       <%=Resources.Account_RechargeFailure_1%>
                       <%if(Convert.ToDouble(ViewData["UserBalance"])<=0) { %> <%=Resources.Account_RechargeFailure_2%> <%=Resources.Account_RechargeFailure_3%>
                        <% } %>
                    </div>

                    <div class="alert alert-info">
                        <%if(Convert.ToDouble(ViewData["UserBalance"])<=0) { %>
                        <strong><span class="entypo-info-circled"></span> <%=Resources.Account_RechargeFailure_4%></strong>
                        <p><%=Resources.Account_RechargeFailure_5%></p>
                        <% } else { %>
                        <strong><span class="entypo-info-circled"></span> <%=Resources.Account_RechargeFailure_5%></strong>
                        <% } %>
                    </div>

                    <a class="btn btn-primary" href="Main"><%=Resources.Account_RechargeButton_MainMenu%></a>
                    <a class="btn btn-success" href="RechargeINT"><%=Resources.Account_RechargeButton_Recharge%></a>

                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>