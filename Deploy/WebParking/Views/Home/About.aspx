<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1>About.</h1>
        <h2><%: ViewBag.Message %></h2>
    </hgroup>

    <article>
        <p>
            Use this area to provide additional information.
        </p>

        <p>
            Use this area to provide additional information.
        </p>

        <p>
            Use this area to provide additional information.
        </p>
    </article>

    <aside>
        <h3>Telerik UI for ASP.NET MVC TreeView</h3>
        <p>
            Use this area to provide additional information.
        </p>

        <%= Html.Kendo().TreeView()
            .Name("LinksTreeView")
            .Items(items =>
            {
                items.Add()
                    .Text("Home")
                    .Action("Index", "Home");
                items.Add()
                    .Text("About")
                    .Action("About", "Home");
                items.Add()
                    .Text("Contact")
                    .Action("Contact", "Home");
            })
        %>
    </aside>
</asp:Content>
