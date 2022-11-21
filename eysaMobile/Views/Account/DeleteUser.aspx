<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.DeleteUserModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    DeleteUSer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script src="../Content/dropdown/js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../Content/dropdown/js/jquery.dd.js" type="text/javascript"></script>

<div id="formulario">
<div class="greenhr"><hr /></div>
    <h2><%=Resources.UserData_DeleteUserTitle %></h2>

  <% using (Html.BeginForm("DeleteUser", "Account", FormMethod.Post))
     { %>   
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.CurrentPassword)%></p>
<p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  

<fieldset> 


<div class="div40-left">
    <p class="aclaracion"><%=Resources.UserData_DeleteUserRemark %></p>
    <br />
    <div class="div5-left"><%= Html.CheckBoxFor(m => m.ConfirmDeletion)%></div>
    <div class="div50-right">
    <p> <%=Resources.UserData_DeleteUser_UnderstandDeletion%>    
    </p></div>
    
</div>


<div class="div33-right">
<p><%=Html.LabelFor(cust => cust.CurrentPassword)%> </p>
  <%= Html.PasswordFor(cust => cust.CurrentPassword, new { @placeholder = string.Format(Resources.UserData_WriteYourCurrentPassword, 4, 50), @class = "inputelementsw100",
            @required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.UserData_CurrentPassword)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Password -->
</div>


<div class="greenhr"><hr /></div>
<p><br /></p>
<input type="submit" value="<%=Resources.UserData_ButtonConfirm%>" class="botonverde" />
<input type="button" value="<%=Resources.UserData_ButtonCancel%>" class="botongris" 
    onclick="location.href='<%=Url.Action("UserData", "Account", null)%>'" />

</fieldset>
<% } %>

</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
