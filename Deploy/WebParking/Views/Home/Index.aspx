<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page - My ASP.NET MVC Application
</asp:Content>
<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Home Page.</h1>
                <h2><%: ViewBag.Message %></h2>
            </hgroup>
            <p>
                To learn more about ASP.NET MVC visit
                <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET MVC.
                If you have any questions about ASP.NET MVC visit
                <a href="http://forums.asp.net/1146.aspx/1?MVC" title="ASP.NET MVC Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Telerik UI for ASP.NET MVC PanelBar</h3>
    <br />

    <% Html.Kendo().PanelBar()
        .Name("IntroPanelBar")
        .Items(items =>
        {
            items.Add()
                .Text("Getting Started")
                .Selected(true)
                .Expanded(true)
                .Content(() =>
                {
                %>
                    <p style="padding:0 1em">ASP.NET MVC gives you a powerful, patterns-based way to build dynamic websites that
                    enables a clean separation of concerns and that gives you full control over markup
                    for enjoyable, agile development. ASP.NET MVC includes many features that enable
                    fast, TDD-friendly development for creating sophisticated applications that use
                    the latest web standards.
                    <a href="http://go.microsoft.com/fwlink/?LinkId=245151">Learn more…</a></p>
                <%
                });
            items.Add()
                .Text("Add NuGet packages and jump-start your coding")
                .Content(() =>
                {
                %>
                    <p style="padding:0 1em">NuGet makes it easy to install and update free libraries and tools.
                    <a href="http://go.microsoft.com/fwlink/?LinkId=245153">Learn more…</a></p>
                <%
                });
            items.Add()
                .Text("Find Web Hosting")
                .Content(() =>
                {
                %>
                    <p style="padding:0 1em">You can easily find a web hosting company that offers the right mix of features
                    and price for your applications.
                    <a href="http://go.microsoft.com/fwlink/?LinkId=245157">Learn more…</a></p>
                <%
                });
        }).Render();
    %>

</asp:Content>
