<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="contactTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Contact - My ASP.NET MVC Application
</asp:Content>

<asp:Content ID="contactContent" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1>Contact.</h1>
        <h2><%: ViewBag.Message %></h2>
    </hgroup>

    <h3>Telerik UI for ASP.NET MVC TabStrip</h3>
    <br />

    <%    Html.Kendo().TabStrip()
        .Name("ContactsTabStrip")
        .Items(items =>
        {
            items.Add()
                .Text("Phone")
                .Selected(true)
                .Content(() =>
                {
                %>
                    <p>
                        <span class="label">Main:</span>
                        <span>425.555.0100</span>
                    </p>
                    <p>
                        <span class="label">After Hours:</span>
                        <span>425.555.0199</span>
                    </p>
                <%
                })
                .ContentHtmlAttributes(new { style = "min-height: 200px" });
            items.Add()
                .Text("Email")
                .Content(() =>
                {
                %>
                    <p>
                        <span class="label">Support:</span>
                        <span><a href="mailto:Support@example.com">Support@example.com</a></span>
                    </p>
                    <p>
                        <span class="label">Marketing:</span>
                        <span><a href="mailto:Marketing@example.com">Marketing@example.com</a></span>
                    </p>
                    <p>
                        <span class="label">General:</span>
                        <span><a href="mailto:General@example.com">General@example.com</a></span>
                    </p>
                <%
                })
                .ContentHtmlAttributes(new { style = "min-height: 200px" });
            items.Add()
                .Text("Address")
                .Content(() =>
                {
                %>
                    <p>
                        One Microsoft Way<br />
                        Redmond, WA 98052-6399
                    </p>
                <%
                })
                .ContentHtmlAttributes(new { style = "min-height: 200px" });
        }).Render();
    %>
</asp:Content>
