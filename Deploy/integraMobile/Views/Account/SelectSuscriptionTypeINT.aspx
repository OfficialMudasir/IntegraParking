<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectSuscriptionTypeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.SelectPayMethod_ChangeSuscriptionType %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.Account_Main_BttnUserData%> --%>
    <%=Resources.SelectPayMethod_ChangeSuscriptionType %>
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
        <li><a href="UserData" title="<%=Resources.Account_Main_BttnUserData%>"><%=Resources.Account_Main_BttnUserData%></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.SelectPayMethod_ChangeSuscriptionType %>
        </li>
    </ul>
</div>
--%>
<%--
<div class="title-alt">
    <h6><%=Resources.SelectSuscriptionType_IntroText1%></h6>
</div>
--%>
<% using (Html.BeginForm("SelectSuscriptionTypeINT", "Account", FormMethod.Post, new {@id = "FormSelectSuscriptionType", @name = "FormSelectSuscriptionType", @role="form"})) 
{ 
    %>
    <div class="content-wrap">
        <%-- ROW ALERTS // --%>
        <div class="row">
            <div class="col-sm-12">
                <%
                    foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                    {
                        foreach (ModelError modelError in keyValuePair.Value.Errors) 
                        {
                            %>
                            <div class="alert alert-bky-danger">
                                <button data-dismiss="alert" class="close" type="button">×</button>
                                <span class="bky-cancel"></span>
                                <%= Html.Encode(modelError.ErrorMessage) %>
                            </div>
                            <%
                        } // ModelError
                    } // ModelState
                %>
            </div><!--// .col-sm-12 -->
        </div><!--// .row / 1 col -->
        <%-- // ROW ALERTS --%>

        <%-- ROW CONTENT // --%>
        <div class="row">

            <%-- COL LEFT --%>
            <div class="col-lg-6 col-block">
                <p class="h3"><span class="bky-suscriptions"></span> &nbsp; <%=Resources.SelectSuscriptionType_IntroText2%></p>

                <div class="form-group">
                    <%= Html.RadioButtonFor(cust => cust.SuscriptionType, (int)PaymentSuscryptionType.pstPrepay, new {@id="SuscriptionTypePrepay"})%>
                    <label for="SuscriptionTypePrepay"><%=Resources.SelectSuscriptionType_Prepay1%></label>
                </div><!--// .form-group -->
            </div><!--// .col-md-6.col-block -->

            <%-- COL RIGHT --%>
            <div class="col-lg-6 col-block">
                <p class="h3"><span class="bky-card-transaction"></span> &nbsp; <%=Resources.SelectSuscriptionType_IntroText3%></p>
                <div class="form-group">
                    <%= Html.RadioButtonFor(cust => cust.SuscriptionType, (int)PaymentSuscryptionType.pstPerTransaction, new {@id="SuscriptionTypePayment"})%>
                    <label for="SuscriptionTypePayment"><%=Resources.SelectSuscriptionType_PerTransaction1%></label>
                    <div class="help-block">
                        <ul>
                            <li><%=Resources.SelectSuscriptionType_PerTransaction1%></li>
                            <li><%=Resources.SelectSuscriptionType_PerTransaction2%> <%=string.Format(Resources.SelectSuscriptionType_PerTransaction3,ViewData["ChargeValue"],ViewData["ChargeCurrency"])%></li>
                        </ul>
                    </div>

                </div>

            </div><!-- .col-md-6.col-block -->
        </div><!--// .row / 2 col -->  
        <%-- // ROW CONTENT --%>

        <%-- ROW BUTTONS // --%>
        <div class="row">
            <div class="col-md-12 col-block" >
                <p class="row-buttons">
                    <button class="btn btn-bky-primary" type="submit"><%=Resources.UserData_ButtonConfirm%></button>
                </p>
            </div>
        </div>
        <%-- // ROW BUTTONS --%>
    </div>
    <% 
} // Html.BeginForm
%>

</asp:Content>