<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%/* TGA @ link original Url.Content("~/Content/Login.css")   */%>
    <link href="<%= Url.Content("~/Content/LoginEM2.css") %>" rel="stylesheet" type="text/css" />    

    <style>
        <%/*-- TGA@ Comment --
        .content-wrapper {
            margin: 0 auto;
            max-width: 900px;
        }
        
        --*/%>

    </style>

<header id="login">
<img id="appicon" src="<%= Url.Content("~/Content/EysaMobv2/EM2-applogo.svg") %>" />
<h2>EYSA <strong>Mobile</strong> <sup>v2</sup></h2>
</header>



   <% using (Html.BeginForm("Login", "Park", FormMethod.Post, new { autocomplete = "off" }))
      { %>  

        <div class="login-container">

            <div class="login-label">
               <%= Html.Label("ddlLanguages", Resources.LoginView_Language) %>                            

                <%/* = Resources.LoginView_Language */%>
            </div>
            <div class="login-field">
                <%= Html.Kendo().DropDownList()
                            .Name("ddlLanguages")
                            .HtmlAttributes(new { tabindex = "5" })
                            .DataTextField("Description")
                            .DataValueField("Culture")
                            .Events(e => e.Change("login_ChangeLanguage").DataBound("login_DataBoundLanguages") )
                            /*.DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("LanguagesLogin_Read", "Account");
                                }); 
                            })*/
                            .BindTo(new System.Collections.Generic.List<dynamic>() {
                                new {Culture = "en-US", Description = "English", Flag = "US" },
                                new {Culture = "es-ES", Description = "Español", Flag = "ES" },
                                new {Culture = "ca-ES", Description = "Català", Flag = "CATALONIA" }
                            })
                            
                            .Value(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
                            //.Height(300)
                            .Template("#=Description#")                      
                %>
            </div>            

            <div class="login-label">
               <%= Html.Label("Username", Resources.LoginView_Username) %>                            
            </div>
            <div class="login-field">
                <%: /*Html.TextBoxFor(m => m.UserName, new { autocomplete = "off", tabindex = 1, @class = "k-input k-textbox" })*/
                    Html.TextBox("Username", null/*"hbusque@integraparking.com"*/, new Dictionary<string, Object> { { "autocomplete", "on" }, { "tabindex", "1" }, { "class", "k-input k-textbox" }, { "data-val-required", Resources.Default_Field_Required2 } , {"placeholder", Resources.LoginView_Username}  })
                    %>
                <%: Html.ValidationMessage("UserName") %>
            </div>

            <div class="login-label">
               <%: Html.Label("Password", Resources.LoginView_Password) %>
            </div>
            <div class="login-field">
                <%: Html.Password("Password", null, new Dictionary<string, Object> { { "autocomplete", "off" }, { "tabindex", "2" }, { "class", "k-input k-textbox" }, { "data-val-required", Resources.Default_Field_Required2 }, {"placeholder", Resources.LoginView_Password}  }) %>
                <%: Html.ValidationMessage("Password") %>
            </div>

            <div class="login-label">
               <%= Html.Label("CityId", Resources.LoginView_City) %>                            

               <%/*= Resources.LoginView_City */%>
            </div>
            <div class="login-field">
                <%= Html.Kendo().DropDownList()
                            .Name("CityId")
                            .HtmlAttributes(new { required = "", tabindex = "3" })
                            .DataTextField("Description")
                            .DataValueField("Id")                            
                            .Events(ev => ev.Change("Login_CityOnChange").DataBound("Login_CityOnDataBound"))
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("Cities_Read", "Park");
                                }); 
                            })                                                        
                            //.Height(300)
                            .Template("#=Description#")                  
                %>
                <span class="k-invalid-msg" data-for="CityId"></span>
                <input type="hidden" id="hdnCityName" name="CityName" value="" />
            </div>        
            
            <div class="login-errors">
                <%: Html.ValidationSummary(true, "") %>
            </div>    

            <div class="login-submit">
                <button type="submit" class="k-button-login k-button k-button-icon" tabindex="4"                 
                        title="<%= Resources.LoginView_SubmitButton_Title %>"
                        onclick="login_Submit();">
                    <span class="k-icon k-update"></span><%= Resources.LoginView_SubmitButton %>
                </button>
            </div>                        



        </div>

    <% } %>

<script>

    $(document).ready(function () {

        $("form").kendoValidator();
        
        //login_SetFlag();

        //$("#CityId").data("kendoDropDownList").select(0);

    });

    function login_ChangeLanguage(e) {
        /*if (e.sender.selectedIndex >= 0) {
            var flag = e.sender.dataSource.data()[e.sender.selectedIndex].Flag;
            login_SetFlag(flag);
        }*/
        var value = $("#ddlLanguages").val();            
        var url = "<%= Url.Action("ChangeCulture", "Account", new { lang = "LANG", returnUrl = this.Request.RawUrl }) %>";
            url = url.replace("LANG", value);
            window.location = url;
    }

    function login_DataBoundLanguages() {

    }

    function Login_CityOnChange(e) {
        if (e.sender.selectedIndex >= 0) {
            var cityDescription = e.sender.dataSource.data()[e.sender.selectedIndex].Description;
            $("#hdnCityName").val(cityDescription);
        }
    }

    function Login_CityOnDataBound(e) {
        Login_CityOnChange(e);
    }

    function login_Submit() {
        var validator = $("form").data("kendoValidator");
        if (validator.validate())
            $(".loading").show();
        return true;
    }

</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
