<%@ Page Language="C#" MasterPageFile="~/Plugins/PBPPlugin/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SecurityPlugin.Models.LoginModel>" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="PBPPlugin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">            
    <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
    <%= resBundle.GetString("Security", "LoginView.Title", "Login") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/Account/Login.css") %>" rel="stylesheet" type="text/css" />    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
   <% using (Html.BeginForm("Login", "Account", FormMethod.Post, new { plugin = "PBPPlugin", autocomplete = "off" }))
      { %>  

        <div class="login-container">
            
            <!-- header login !-->
            <% var resBundleBackOffice = ResourceBundle.GetInstance("backOffice"); %>
            <div id="viaLogin">
                <img src="<%= Url.Content(RouteConfig.BasePath + "Content/Account/logo.png") %>" /> 
                
            </div>
            <br />               
            <!-- header login !-->

            <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
            <div class="login-label">
                <h4><%= resBundle.GetString("Security", "LoginView.Language", "Language") %></h4>
            </div>
            <div class="login-field">
                <%= Html.Kendo().DropDownList()
                            .Name("ddlLanguages")
                            .DataTextField("Description")
                            .DataValueField("Culture")
                            .Events(e => e.Change("login_ChangeLanguage").DataBound("login_DataBoundLanguages") )
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("LanguagesLogin_Read", "Security", new { plugin = "SecurityPlugin" });
                                }); 
                            })
                            /*.BindTo(new System.Collections.Generic.List<dynamic>() {
                                new {Culture = "en-US", Description = "English", Flag = "US" },
                                new {Culture = "es-ES", Description = "Español", Flag = "ES" },
                                new {Culture = "ca-ES", Description = "Català", Flag = "CATALONIA" }
                            })*/
                            
                            .Value(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
                    //.Height(300)
                            .Template("<img src=\"" + RouteConfig.GetContentUrl("~/Content/img/flags") + "#=Flag#.gif\"/> #=Description#")                      
                %>
            </div>            

            <div class="login-label">
                <h4><%= Html.LabelFor(m => m.UserName) %></h4>                            
            </div>
            <div class="login-field">
                <%: /*Html.TextBoxFor(m => m.UserName, new { autocomplete = "off", tabindex = 1, @class = "k-input k-textbox" })*/
                    Html.TextBoxFor(m => m.UserName, new Dictionary<string, Object> { { "autocomplete", "off" }, { "tabindex", "1" }, { "class", "k-input k-textbox" }, { "data-val-required", resBundle.GetString("Common", "Default.Field.Required2", "Field required") } })
                    %>
                <%: Html.ValidationMessageFor(m => m.UserName) %>
            </div>

            <div class="login-label">
                <h4><%: Html.LabelFor(m => m.Password) %></h4>
            </div>
            <div class="login-field">
                <%: Html.PasswordFor(m => m.Password, new Dictionary<string, Object> { { "autocomplete", "off" }, { "tabindex", "2" }, { "class", "k-input k-textbox" }, { "data-val-required", resBundle.GetString("Common", "Default.Field.Required2", "Field required") } }) %>
                <%: Html.ValidationMessageFor(m => m.Password) %>
            </div>

            <div class="login-submit">
                <button type="submit" class="k-button k-button-icon"                 
                        title="<%= resBundle.GetString("Security", "LoginView.SubmitButton.Title", "Login") %>">
                    <span class="k-icon k-update"></span><%= resBundle.GetString("Security", "LoginView.SubmitButton", "Login") %>
                </button>
            </div>

            <div class="login-errors">
                <%: Html.ValidationSummary(true, "") %>
            </div>

            <!-- footer ciutat !-->
            <div id="footerCiutat">&nbsp;</div>
            <!-- footer ciutat !-->

        </div>
        <!--<input type="submit" value="<%=resBundle.GetString("Security", "Login.SubmitButton", "Login")%>" />-->

    <% } %>

    <script>

        $(document).ready(function () {

            $("form").kendoValidator();

            //login_SetFlag();

        });

        function login_ChangeLanguage(e) {
            if (e.sender.selectedIndex >= 0) {
                var flag = e.sender.dataSource.data()[e.sender.selectedIndex].Flag;
                login_SetFlag(flag);
            }
            var value = $("#ddlLanguages").val();            
            var url = "<%= Url.Action("ChangeCulture", "Account", new { plugin = "SecurityPlugin", lang = "LANG", returnUrl = this.Request.RawUrl }) %>";
            url = url.replace("LANG", value);
            window.location = url;
        }

        function login_SetFlag(flag) {

            var ddlLanguages = $("#ddlLanguages").data("kendoDropDownList");

            if (flag == null) {
                if (ddlLanguages.selectedIndex >= 0) {
                    flag = ddlLanguages.dataSource.data()[ddlLanguages.selectedIndex].Flag;
                }
            }

            if (flag != null) {
                /*ddlLanguages.span[0].style.backgroundRepeat = "no-repeat";
                ddlLanguages.span[0].style.backgroundPosition = "5px 7px";
                ddlLanguages.span[0].style.paddingLeft = "20px";
                ddlLanguages.span[0].style.backgroundImage = "Url(<%= RouteConfig.GetContentUrl("~/Content/img/flags") %>" + flag + ".gif)";*/

                ddlLanguages.span[0].style.paddingLeft = "5px"; // 20px
            }

        }

        function login_DataBoundLanguages(e) {
            login_SetFlag();
        }

    </script>

</asp:Content>

