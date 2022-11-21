<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.Models.UserInfo>" %>
<%@ Import Namespace="WebParking.WS.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.QueryParkingView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <link href="<%= Url.Content("~/Content/QueryParkingEM2.css") %>" rel="stylesheet" type="text/css" />    


    <style>

    </style>

    <div class="mainContainer">

        <div id="divQueryParking">



        <header class="op-parking">
            <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <span class="valign"></span>
            <h2><%= Resources.QueryParkingView_Title %></h2>
        </header>



        <div id="selParkPlate"><%= Model.Plate %></div>
        <div id="selParkZone"><%= Model.ZoneTar.Zones.GetDescription(Model.GroupId) %> -  <%= Model.ZoneTar.Tariffs.GetDescription(Model.TariffId) %></div>

        <div class="dateTime">
            <span class="icoWatch"></span>
            <div class="dateTime-left">
                <span id="spanTime"><%= Model.QueryParkingOperation.Steps[0].D.ToShortTimeString() %></span>
            </div>
            <div class="dateTime-right">
                <span id="spanDate"><%= Model.QueryParkingOperation.Steps[0].D.ToShortDateString() %></span>
            </div>
        </div>

        <div class="amount">
            <span id="spanAmount"><%= Model.FormatedAmount(Model.QueryParkingOperation.Q1) %></span>
        </div>
        <div id="topvalues"> 
            <span id="valuemin">min</span>
            <span id="valuemax">max</span>
        </div>

        <div class="slider">
            <input id="slider" class="balSlider" value="0" />
        </div>

        <div class="buttonsInc">
            <input type="button" value="<%= Model.QueryParkingOperation.GetBtn1StepLit() %>" onclick="IncButton(<%= Model.QueryParkingOperation.GetBtn1Step() %>);" />
            <input type="button" value="<%= Model.QueryParkingOperation.GetBtn2StepLit() %>" onclick="IncButton(<%= Model.QueryParkingOperation.GetBtn2Step() %>);" />
            <input type="button" value="<%= Model.QueryParkingOperation.GetBtn3StepLit() %>" onclick="IncButton(<%= Model.QueryParkingOperation.GetBtn3Step() %>);" />
        </div>
    
        <% using (Html.BeginForm("ConfirmParking", "Park", FormMethod.Post))
            { %>

            <input id="hdnStepIndex" type="hidden" name="stepindex" value="0" />

        <div class="queryParking-submit">
            <button type="submit" class="k-button k-button-icon"                 
                    title="<%= Resources.QueryParkingView_SubmitButton_Title %>"
                    onclick="$('.loading').show(); return true;">
                <span class="k-icon k-update"></span><%= Resources.QueryParkingView_SubmitButton %>
            </button>
        </div>

        <%  } %>

        </div>

    </div>

    <script>

        var dtEndDateTime;
        var steps = <%= Model.QueryParkingOperation.Steps.ToJson() %>;

        $(document).ready(function () {

            var slider = $("#slider").kendoSlider({
                increaseButtonTitle: "<%= Model.QueryParkingOperation.T2.ToString() + "min" %>",
                decreaseButtonTitle: "<%= Model.QueryParkingOperation.T1.ToString() + "min" %>",
                min: 0,
                max: <%= Model.QueryParkingOperation.Steps.Count - 1 %>,
                smallStep: 1,
                largeStep: 1,
                slide: SliderOnChange,
                change: SliderOnChange,
                tickPlacement: "none"
            }).data("kendoSlider");

            dtEndDateTime = kendo.parseDate("<%= Model.QueryParkingOperation.InitialDate.ToString("dd/MM/yyyy") %>", "dd/MM/yyyy");
        });

        function SliderOnChange(e) {
            var step = steps[e.value];
            var date = kendo.parseDate(step.D);

            $("#hdnStepIndex").val(e.value);
            $("#spanTime").text(kendo.toString(date, "HH:mm"));
            $("#spanDate").text(kendo.toString(date, "dd/MM/yyyy"));
            $("#spanAmount").text(kendo.toString(step.Q / 100, "#0.00 <%= Model.Cur %>"));

        }

        function IncButton(minutes) {
            var curIndex = parseInt($("#hdnStepIndex").val());
            var curStep = steps[curIndex];
            if (curStep != null) {
                var finalT = curStep.T + minutes;
                while (curStep != null && curStep.T < finalT) {
                    if ((curIndex + 1) < steps.length) {
                        curIndex = curIndex + 1;
                        curStep = steps[curIndex];
                    }
                    else
                        curStep = null;
                }
                $("#slider").data("kendoSlider").value(curIndex);
                SliderOnChange({ value: curIndex });
            }
        }

    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
