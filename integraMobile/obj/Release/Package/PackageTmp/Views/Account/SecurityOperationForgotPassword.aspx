<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  <%=Resources.SecurityOperationForgotPassword_IdentityValidation%>
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
        <li><a href="<%= Url.Action("Index", "Home") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.SecurityOperationForgotPassword_IdentityValidation%>
        </li>
    </ul>
</div>
--%>
<%-- 
<div class="title-alt">
    <h6>
    <%=Resources.SecurityOperationForgotPassword_IdentityValidation%>
    </h6>
</div>
 --%>
<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
            <% 
                if ((!Convert.ToBoolean(ViewData["CodeExpired"])) && (!Convert.ToBoolean(ViewData["CodeAlreadyUsed"]))) 
                { 
                    %>
                        
                        <div class="form_center">
                            <% 
                                if (string.IsNullOrEmpty((String)ViewData["ActivationRetries"])) 
                                {
                                    %>
                                    <%
                                }
                                else
                                {
                                    %>
                                        <div class="alert alert-bky-info">
                                            <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                            <span class="bky-info"></span> &nbsp; <%=(String)ViewData["ActivationRetries"] %>.
                                        </div>
                                    <% 
                                } %>

                            <% 
                                using (Html.BeginForm("SecurityOperationForgotPassword", "Account", FormMethod.Post, new {@role = "form"}))
                                { 
                                    %>
                                        <div class="alert alert-bky-warning">
                                            <div class="form-group">
                                                <span class="bky-info"></span> 
                                                &nbsp; 
                                                <%=String.Format(Resources.CustomerInscriptionModel_Step2SendSMS, (String)ViewData["EndSufixMainPhone"])%>
                                            </div>
                                            <input name="type" type="hidden" value="resendsms"> 
                                            <button class="btn btn-sm btn-bky-danger" type="submit"><%=Resources.Button_ResendSMS%></button>
                                        </div>
                                    <% 
                                }  // using Html.BeginForm 
                            %>

                            <% 
                                using (Html.BeginForm("SecurityOperationForgotPassword", "Account", FormMethod.Post, new {@role = "form"}))
                                { 
                                    foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
                                        foreach (ModelError modelError in keyValuePair.Value.Errors) {
                                            %>
                                            <div class="alert alert-bky-danger">
                                                <button data-dismiss="alert" class="close" type="button">×</button>
                                                <span class="bky-cancel"></span>
                                                &nbsp;
                                                <%= Html.Encode(modelError.ErrorMessage) %>
                                            </div>
                                            <%
                                        } // foreach ModelError
                                    } // foreach KeyValuePair

                                    %>
                                        <div class="form-group">
                                            <input name="confirmationcode" class="form-control" placeholder="<%=Resources.SecurityOperationForgotPassword_Step2WriteYourCode2%>" oninvalid="this.setCustomValidity('<%=string.Format(Resources.ErrorsMsg_RequiredField,"")%>');" oninput="this.setCustomValidity('');" type="number" required="required">
                                            <p class="help-block"><%=String.Format(Resources.SecurityOperationForgotPassword_Step2WriteYourCode1, ConfigurationManager.AppSettings["NumCharactersActivationSMS"], (String)ViewData["EndSufixMainPhone"], (int)ViewData["NumMinutesTimeoutActivationSMS"])%></p>
                                        </div>
                                        <div class="row-buttons">
                                            <input name="type" type="hidden" value="confirmcode">
                                            <button class="btn btn-bky-success" type="submit"><%=Resources.Button_Validate%></button>
                                        </div>
                                    <% 

                                } // using Html.BeginForm
                            %>
                        </div><!--// .form_center--> 

                    <% 
                } 
                else 
                { 
                    %>
                        <div class="alert alert-bky-danger">
                            <span class="bky-cancel"></span> &nbsp; <%= Html.ValidationMessage("CodeExpired")%> <%= Html.ValidationMessage("CodeAlreadyUsed")%>
                        </div><!--//.alert-bky-danger-->
                    <% 
                }  // if else CodeExpired && CodeAlreadyUsed
            %>
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

</asp:Content>