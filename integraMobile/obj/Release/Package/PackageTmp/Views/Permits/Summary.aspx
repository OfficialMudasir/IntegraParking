<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<PermitsModel>" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="integraMobile.Models" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>
    <%= resBundle.GetString("Permits_Summary") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content("~/Content/Permits.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentTitle" runat="server">
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>
    <%= resBundle.GetString("Permits_Summary") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>
    <div id="loader"></div>
    <div class="content-wrap">
        <div id="summary-steps" class="row no-print">
            <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12">
                <%-- OLD SUMMARY STEPS --%>
                <%--
                <div class="decoration-little-little">
                    <div>
                        <div class="bubble bubble-little" id="bubble1"><img src="<%= Url.Content("~/Content/img/Permits/check.png")%>" /></div>
                        <div class="p-arrow p-arrow-light" id="arrow1"><img src="<%= Url.Content("~/Content/img/Permits/arrow.png")%>" /></div>
                        <div class="bubble" id="bubble2"><img src="<%= Url.Content("~/Content/img/Permits/2-blue.png")%>" /></div>
                    </div>
                    <div>
                        <div class="bubble-label label-gray"><%= resBundle.GetString("Permits_PayForPermit")%></div>
                        <div class="bubble-label label-blue"><%= resBundle.GetString("Permits_Summary")%></div>
                    </div>
                </div>
                --%>

                <ul class="steps-dots">
                    <li class="step done">
                        <span class="number">1.</span>
                        <span class="step-desc"><%= resBundle.GetString("Permits_PayForPermit")%></span>
                    </li>
                    <!-- .step.step-arrow -->
                    <li class="step step-arrow">
                        <span class="glyphicon glyphicon-play" aria-hidden="true"><!--bootstrap 3 icon --></span>
                    </li>
                    <li class="step current">
                        <span class="number">2.</span>
                        <span class="step-desc"><%= resBundle.GetString("Permits_Summary")%></span>
                    </li>
                </ul><!--//.steps-dots-->
            </div>
        </div><!--//.#summary-steps.row-->
        <div id="summary-content" class="row">
            <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12 col-block">

            <% 
                if (!string.IsNullOrEmpty(Model.Error))
                {
                    %>
                        <div class="alert alert-bky-danger notice">

                            <p style="display:inline-block;"><span class="bky-cancel"></span></p>
                            <p style="display:inline-block;"><%= Model.Error%></p>
                        
                        </div><!--//.notice.alert-->
                        <div class="row-buttons">
                            <a class="btn btn-bky-primary" href="<%= Url.Action("PayForPermit", "Permits") %>"><%= resBundle.GetString("Permits_Retry")%></a>
                        </div>
                    <%
                }
                else 
                { 
                    %>
                    <h3 class="content-header">
                        <span>2.</span> <%= resBundle.GetString("Permits_Summary")%>
                    </h3>
                    <hr>
                    <div>
                        <label><% =resBundle.GetString("PermitsDataModel_User") %></label>
                        <div class="summary-text"><%= Model.UserNameForSummary %></div>
                    </div>
                    <div>
                        <label><% =resBundle.GetString("PermitsDataModel_City") %></label>
                        <div class="summary-text"><%= Model.Cities[Model.City] %></div>
                    </div>
                    <div>
                        <label><% =resBundle.GetString("PermitsDataModel_Zone") %></label>
                        <div class="summary-text"><%= Model.Zones[Model.Zone] %></div>
                    </div>
                    <%
                        for (int i = 1; i <= Model.PlateCollection.Count; i++)
                        {
                            %>
                            <div>
                                <label><% =resBundle.GetString("Permits_Permit") %> <% =i %></label>
                            </div>
                            <div>
                                <label><% =resBundle.GetString("PermitsDataModel_Plates") %></label>
                                <div class="summary-text">
                                    <% = Model.PlateCollection[i-1].Replace(",", ", ") %>
                                </div>
                            </div>
                            <% 
                        } // for Model.PlateCollection.Count
                    %>
                    <div>
                        <label><% =resBundle.GetString("PermitsDataModel_Tariff") %></label>
                        <div class="summary-text"><%= Model.Tariffs[Model.Tariff] %></div>
                    </div>
                    <div>
                        <label><% =resBundle.GetString("PermitsDataModel_TimeStep") %></label>
                        <div class="summary-text"><%= Model.TimeSteps[Model.TimeStep].text %></div>
                    </div>
                    <hr>
                    <div class="row-buttons">
                        <a id="logout-button" class="btn btn-bky-primary" href="<%= Url.Action("PayForPermit", "Permits") %>"><%= resBundle.GetString("Permits_Start")%></a>
                    </div>            
                    <%                
                } // if else Model.Error
            %>
            </div><!--//.col-block-->
        </div><!--//.#summary-content.row-->
    </div>
</asp:Content>