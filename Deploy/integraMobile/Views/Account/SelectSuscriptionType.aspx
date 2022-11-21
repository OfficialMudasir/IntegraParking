<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectSuscriptionTypeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.SelectSuscriptionType_IntroText1%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.ServiceName%> --%>
    <%=Resources.SelectSuscriptionType_IntroText1 %>
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
<div class="title-alt">
    <h6><%=Resources.SelectSuscriptionType_IntroText1%></h6>
</div>
 --%>
<%
    using (Html.BeginForm("SelectSuscriptionType", "Account",FormMethod.Post, new {@id="FormSelectSuscriptionType", @name="FormSelectSuscriptionType", @role="form" }))
    {
        %>  
            <div class="content-wrap">

                <%-- ROW ALERTS //--%>
                <div class="row">
                    <div class="col-sm-12">
                        
                        <%
                            foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) 
                            {
                                foreach (ModelError modelError in keyValuePair.Value.Errors) 
                                {
                                    %>
                                    <div class="alert alert-bky-danger">
                                        <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                        <span class="bky-cancel"></span>  
                                        &nbsp; 
                                        <%= Html.Encode(modelError.ErrorMessage) %>
                                    </div>
                                    <%
                                } // foreach ModelError
                            } // foreach KeyValuePair
                        %>
                    </div><!--//.col-sm-12-->
                </div><!--// .row / 1 col -->
                <%-- // ROW ALERTS --%>

                <%-- ROW CONTENT // --%>
                <div class="row">

                            <%-- COL LEFT // --%>
                            <div class="col-lg-6 col-block">
                                <p class="h3"><span class="bky-suscriptions"></span> &nbsp; <%=Resources.SelectSuscriptionType_IntroText2%></p>

                                <div class="form-group">
                                    <%= Html.RadioButtonFor(cust => cust.SuscriptionType, (int)PaymentSuscryptionType.pstPrepay, new {@id="SuscriptionTypePrepay"})%>
                                    <label for="SuscriptionTypePrepay" style="white-space:normal;"><%=Resources.SelectSuscriptionType_Prepay1%></label>                                        
                                </div>
                            </div><!--// .col-md-6.col-block -->

                            <%-- COL RIGHT // --%>
                            <div class="col-lg-6 col-block">
                                <p class="h3"><span class="bky-card-transaction"></span> &nbsp; <%=Resources.SelectSuscriptionType_IntroText3%></p>
                                <div class="form-group">
                                    <%= Html.RadioButtonFor(cust => cust.SuscriptionType, (int)PaymentSuscryptionType.pstPerTransaction, new {@id="SuscriptionTypePayment"})%>
                                    <label for="SuscriptionTypePayment" style="white-space: normal;"><%=Resources.SelectSuscriptionType_PerTransaction1%></label>
                                    <div class="help-block">
                                        <ul>
                                            <li><%=Resources.SelectSuscriptionType_PerTransaction2%></li>
                                            <li><%=string.Format(Resources.SelectSuscriptionType_PerTransaction3,ViewData["ChargeValue"],ViewData["ChargeCurrency"])%></li>
                                        </ul>
                                    </div>

                                </div>

                            </div><!--// .col-md-6.col-block -->

                </div><!--// .row / 2 col -->
                <%-- // ROW CONTENT --%>

                <%-- ROW BUTTONS // --%>
                <div class="row">
                    <div class="col-md-12 col-block" >
                        <p class="row-buttons">
                            <button class="btn btn-bky-primary" type="submit"><%=Resources.Button_Next %></button>
                        </p>
                    </div><!--//.col-block-->
                </div><!--//.row-->
                <%-- // ROW BUTTONS --%>


            </div><!--//.content-wrap-->
        <% 
    } //  Html.BeginForm
%>
</asp:Content>