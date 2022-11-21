<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.DeleteUserModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.UserData_RemoveUserAccount %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.Account_Main_BttnUserData%> --%>
    <%=Resources.UserData_RemoveUserAccount %>
    <%-- <%=Resources.UserData_DeleteUserTitle%> --%>
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
        <li><%=Resources.UserData_RemoveUserAccount %>
        </li>
    </ul>
--%>
<%--
<div class="title-alt">
    <h6><%=Resources.UserData_DeleteUserTitle %></h6>
</div>
--%>

<% using (Html.BeginForm("DeleteUser", "Account", FormMethod.Post, new { @role="form"}))
{ 
    %>

        <div class="content-wrap">
            <div id="content-alerts" class="row">
                <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2">
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
                        <hr class="separator-block-line">
                    <%
                                }
                        }
                    %>
                    <div class="alert alert-bky-warning text-center">
                        <%-- <button data-dismiss="alert" class="close" type="button"><span class="bky-delete"></span></button> --%>
                        <p><span class="bky-info big-icon"></span></p>
                        <p><%=Resources.UserData_DeleteUserRemark %></p>
                    </div>      
                        
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-block">
                    <h3><%=Resources.UserData_DeleteUserTitle %></h3>


                    <div class="form-group">
                        <%=Html.LabelFor(cust => cust.CurrentPassword,new{@style="white-space:initial"})%>
                        <%= Html.PasswordFor(cust => cust.CurrentPassword, new {
                            @placeholder = string.Format(Resources.UserData_WriteYourCurrentPassword, 4, 50), 
                            @class = "form-control",
                            @required="required", 
                            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.UserData_CurrentPassword)+"');", 
                            @oninput="this.setCustomValidity('');" 
                        })%>
                        <%-- <p class="help-block"><%=Resources.UserData_DeleteUserRemark %></p> --%>
                    </div>
                    <div class="checkbox">
                        <label>
                            <%= Html.CheckBoxFor(m => m.ConfirmDeletion)%>
                            &nbsp;
                            <%=Resources.UserData_DeleteUser_UnderstandDeletion%>
                        </label>
                    </div>
                    <div class="row-buttons">
                        <button class="btn btn-bky-danger" type="submit"><span class="bky-cancel"></span> &nbsp; <%=Resources.UserData_InitProcedure%></button>
                    </div>
                </div>
            </div>
        </div>

    <% 
} // Html.BeginForm
%>
</asp:Content>