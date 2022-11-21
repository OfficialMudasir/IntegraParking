<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<kendoTest.Models.UserAdminDataModel>" %>

<%= Html.HiddenFor(model => model.UserID) %>


<% Html.Kendo().TabStrip()
        .Name("tabstrip")
        .HtmlAttributes(new { style = "height: 100%" })
        .Animation(false)
        .Items(tabstrip =>
        {
            tabstrip.Add().Text("General")
                .Selected(true)                
                .Content(() => 
                { %>
                <div class="editor-form">
                    <div class="editor-container">
                        <div class="editor-label">
                            <%= Html.LabelFor(model => model.Username)%>
                        </div>
                        <div class="editor-field">
                            <%= Html.TextBoxFor(model => model.Username, new {@class = "k-input k-textbox" })%>
                            <%= Html.ValidationMessageFor(model => model.Username)%>
                        </div>
                    </div>
                    <div class="editor-container">
                        <div class="editor-label">
                            <%= Html.LabelFor(model => model.Email)%>
                        </div>
                        <div class="editor-field">
                            <%= Html.TextBoxFor(model => model.Email, new {@class = "k-input k-textbox" })%>
                            <%= Html.ValidationMessageFor(model => model.Email)%>
                        </div>
                    </div>
                    <div class="editor-container">
                        <div class="editor-label">
                            <%= Html.LabelFor(model => model.Country)%>
                        </div>
                        <div class="editor-field">
                            <%= Html.Kendo().DropDownListFor(m => m)        
                                .Name("Country")
                                .DataValueField("CountryID")
                                .DataTextField("Description")
                                .BindTo((System.Collections.IEnumerable)ViewData["countries"])        
                                .Template("<img src=\"../../../Content/img/banderas/#=Code#.gif\"/> #=Description#")
                                .Events(e => {
                                    e.Change("users_DropDownListCountry_OnChange");
                                })
                            %>
                            <script>
                                var dropdownlist = $("#Country").data("kendoDropDownList");
                                dropdownlist.list.width(250);
                            </script>
                        </div>
                    </div>
                </div>
                <% });

            tabstrip.Add().Text("Contact")
                .Content(() => { %>
                <div class="editor-form">
                    <div class="editor-container">
                        <div class="editor-label">
                            <%= Html.LabelFor(model => model.MainPhoneNumber)%>
                        </div>
                        <div class="editor-field">
                            <%= Html.Kendo().DropDownListFor(m => m.MainPhoneCountry)
                                .Name("MainPhoneCountry")
                                .DataValueField("CountryID")
                                .DataTextField("Description")//.Template("#=TelPrefix# - #=Description#")
                                .BindTo((System.Collections.IEnumerable)ViewData["countries"])
                                .Template("<img src=\"../../../Content/img/banderas/#=Code#.gif\"/> #=Description#")
                                .Events(e => {
                                    e.Change("users_DropDownListCountry_OnChange");                                    
                                })
                                .HtmlAttributes(new { id = "MainPhoneCountry", style = "width: 20%;"})
                            %>
                            <script>
                                var dropdownlist = $("#MainPhoneCountry").data("kendoDropDownList");
                                dropdownlist.list.width(250);
                            </script>
                            <%= Html.TextBoxFor(model => model.MainPhoneNumber, new {@class = "k-input k-textbox", stlye = "width: 75%;" })%>
                        </div>
                    </div>
                    <div class="editor-container">
                        <div class="editor-label">
                            <%= Html.LabelFor(model => model.AlternativePhoneNumber)%>
                        </div>
                        <div class="editor-field">
                            <%= Html.Kendo().ComboBoxFor(m=>m.AlternativePhoneCountry)
                                .Name("AlternativePhoneCountry")
                                .DataValueField("CountryID")
                                .DataTextField("Description")//.Template("#=TelPrefix# - #=Description#")
                                .BindTo((System.Collections.IEnumerable)ViewData["countries"])
                                .Template("<img src=\"../../../Content/img/banderas/#=Code#.gif\"/> #=Description#")
                                .Events(e => {
                                    e.Change("users_ComboBoxCountry_OnChange");                                    
                                })
                                .HtmlAttributes(new { id = "AlternativePhoneCountry", style = "width: 40%;"})                                
                            %>
                            <script>
                                var combobox = $("#AlternativePhoneCountry").data("kendoComboBox");
                                combobox.list.width(250);
                            </script>
                            <%= Html.TextBoxFor(model => model.AlternativePhoneNumber, new {@class = "k-input k-textbox", stlye = "width: 50%;" })%>
                        </div>
                    </div>
                </div>
                <% });

            tabstrip.Add().Text("...")
                .Content(() => { %>
                <div class="editor-form">

                </div>
                <% });
        })
        .Render();
%>

<script>

    /*$(document).ready(function () {

        users_PopupReady();

    });*/

</script>

<style>

    .editor-form {
        height: 350px;
        width: 750px;
    }
    .editor-container {
        display: inline-block !important;
        width: 45%;
        min-width: 200px;
        vertical-align: top;
        margin: 5px;
        position: relative;
    }
    .editor-label {
        width: 20% !important;
        padding-top: 8px !important;
        /*float: left;*/
    }
    .editor-field {
        width: 70% !important;
        padding-top: 8px !important;
        vertical-align: middle;
        /*float: right;
        display: block;*/
    }

    .k-edit-form-container {
        width: auto;
    }

</style>