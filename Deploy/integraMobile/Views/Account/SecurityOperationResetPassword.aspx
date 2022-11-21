<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteWithoutHeader.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ResetPasswordModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
  <%=Resources.ResetPassword_Title%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
    <%=Resources.ResetPassword_Title%>
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
        <li> <%=Resources.ResetPassword_Title%>
        </li>
    </ul>
</div>
--%>
<%-- 
<div class="title-alt">
    <h6>
        <%=Resources.ResetPassword_Title%>
    </h6>
</div>
 --%>
<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
            <% 
                if ((!Convert.ToBoolean(ViewData["CodeExpired"])) &&
                (!Convert.ToBoolean(ViewData["CodeAlreadyUsed"])) &&
                (!Convert.ToBoolean(ViewData["ConfirmationCodeError"]))) 
                { 
                    using (Html.BeginForm("SecurityOperationResetPassword", "Account", FormMethod.Post, new {@role = "form"}))
                    { %>
                        <div class="form_center">           
                            <%
                                foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
                                    foreach (ModelError modelError in keyValuePair.Value.Errors) {
                                    %>
                                    <div class="alert alert-bky-danger">
                                        <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button>
                                        <span class="bky-cancel"></span> 
                                        &nbsp; 
                                        <%= Html.Encode(modelError.ErrorMessage) %>
                                    </div>
                                    <%
                                    } // foreach ModelError
                                } // foreach (KeyValuePair
                            %>
                            <div class="form-block">
                                <p><%=String.Format(Resources.ResetPassword_Message1, ("<b>" + (String)ViewData["username"]) + "</b>") %></p>
                                <p><%=Resources.ResetPassword_Message2%></p>
                                <div class="form-group">
                                    <%=Html.LabelFor(cust => cust.Password) %> 
                                    <%= Html.PasswordFor(cust => cust.Password, new { 
                                        @placeholder = string.Format(Resources.ResetPassword_WriteYourPassword,4,50),
                                        @class = "form-control",
                                        @required="required"
                                    })%>            
                                </div><!--// form-group -->
                                <div class="form-group">
                                    <%=Html.LabelFor(cust => cust.ConfirmPassword) %> 
                                    <%= Html.PasswordFor(cust => cust.ConfirmPassword, new { 
                                        @placeholder = string.Format(Resources.ResetPassword_WriteYourConfirmPassword,4,50),
                                        @class = "form-control",
                                        @required="required"
                                    })%>
                                </div><!--// form-group -->
                                                        
                            </div><!--// .form-block-->
                            <div class="row-buttons">
                                <input name="type" type="hidden" value="confirmcode">
                                <button class="btn btn-bky-success" type="submit"><%=Resources.Button_Validate%></button>
                            </div><!--//.row-buttons-->
                        </div><!--// .form_center-->
                    <% 
                    } //Html.BeginForm
                } 
                else 
                { 
                    %>
                        <div class="alert alert-bky-danger">
                            <p>
                                <span class="bky-cancel"></span> 
                                &nbsp; 
                                <%= Html.ValidationMessage("CodeExpired")%> 
                            </p>
                            <hr>
                            <p> 
                                <span class="bky-cancel"></span> 
                                &nbsp; 
                                <%= Html.ValidationMessage("CodeAlreadyUsed")%> 
                            </p>
                            <hr>
                            <p>
                                <span class="bky-cancel"></span> 
                                &nbsp; 
                                <%= Html.ValidationMessage("ConfirmationCodeError")%>
                            </p>
                        </div>
                    <% 
                } // if else CodeExpired && CodeAlreadyUsed &&ConfirmationCodeError
            %>
           

        </div>
    </div>
</div>

</asp:Content>