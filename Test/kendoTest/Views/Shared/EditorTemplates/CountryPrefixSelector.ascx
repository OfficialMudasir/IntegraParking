<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<kendoTest.Models.CountryDataModel>" %>
<%@ Import Namespace="kendoTest.Models" %>

<%= Html.Kendo().DropDownListFor(m => m)        
        .Name("Country")
        .DataValueField("CountryID")
        .DataTextField("Description").Template("#=TelPrefix# - #=Description#")
        .BindTo((System.Collections.IEnumerable)ViewData["countries"])        
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