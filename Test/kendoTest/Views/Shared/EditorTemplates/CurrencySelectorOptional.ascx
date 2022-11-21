<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<kendoTest.Models.CurrencyDataModel>" %>
<%@ Import Namespace="kendoTest.Models" %>

<%= Html.Kendo().DropDownListFor(m => m)    
        .Name("Currency")  
        .DataValueField("CurrencyID")
        .DataTextField("Name")
        .BindTo((System.Collections.IEnumerable)ViewData["currencies"])
        .OptionLabel("--") 
%>

<!-- 
 Html.Kendo().DropDownListFor(m => m)
    //.Name("Currencies")
    .DataTextField("CurrencyName")
    .DataValueField("CurrencyID")
    .DataSource(source => {
        source.Read(read =>
        {
            read.Action("GetCurrencies", "Kendo");
        }); 
    })
    .OptionLabel("--")
   
-->