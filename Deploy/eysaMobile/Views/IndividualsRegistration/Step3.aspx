<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.CustomerInscriptionModelStep3>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Step3
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="../Content/dropdown/js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../Content/dropdown/js/jquery.dd.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="../Content/dropdown/dd.css" />
<script  type="text/javascript">
    $(document).ready(function (e) {
        try {
            $("body select").msDropDown();
        } catch (e) {
            alert(e.message);
        }
    });
</script>
<div class="div100Sup1"><img src="../Content/img/Step3-<%=((CultureInfo)Session["Culture"]).Name%>.png"/></div>

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.RegistrationForm %> - <%=Resources.CustomerInscriptionModel_Step3Address%></h2>
<br />
<div class="greenhr"><hr /></div>
<br />
 <% using (Html.BeginForm("Step3", "IndividualsRegistration", FormMethod.Post)) { %>  

 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.StreetName)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.StreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.LevelInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.DoorInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.LetterInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.StairInStreetNumber)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.State)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.City)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.ZipCode)%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  
<fieldset>  
<div class="div40-left">
 <p><%=Html.LabelFor(cust => cust.StreetName) %> </p>
  <%= Html.TextBoxFor(cust => cust.StreetName, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetName ,@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetName)+"');", 
            @oninput="this.setCustomValidity('');"})%>
  <!--  Nombre de la calle -->
</div>
<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.StreetNumber) %> </p>
  <%= Html.TextBoxFor(cust => cust.StreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourStreetNumber,@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_StreetNumber)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Numero  -->
</div>
<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.LevelInStreetNumber) %> </p>
  <%= Html.TextBoxFor(cust => cust.LevelInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourLevelInStreetNumber })%>
  <!--  Piso  -->
</div>

<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.DoorInStreetNumber) %> </p>
  <%= Html.TextBoxFor(cust => cust.DoorInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourDoorInStreetNumber })%>
  <!--  Puerta  -->
</div>

<div class="div12-center">
 <p><%=Html.LabelFor(cust => cust.LetterInStreetNumber) %> </p>
  <%= Html.TextBoxFor(cust => cust.LetterInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourLetterInStreetNumber })%>
  <!--  Letra  -->
</div>

<div class="div12-right">
 <p><%=Html.LabelFor(cust => cust.StairInStreetNumber) %> </p>
  <%= Html.TextBoxFor(cust => cust.StairInStreetNumber, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourStairInStreetNumber })%>
  <!--  Escalera  -->
</div>

<div class="div50-left">
  <p><%=Html.LabelFor(cust => cust.Country) %></p>
  <select id="Country" size="1"  onChange="showValue(this.value)" name="Country" style="width:100%">
    <%= CountryDropDownHelper.CountryDropDown((Array)ViewData["CountriesOptionList"], "../Content/img/banderas/", (String)ViewData["SelectedCountry"])%>				
  </select>
</div>
<div class="div50-right">
 <p><%=Html.LabelFor(cust => cust.State) %> </p>
  <%= Html.TextBoxFor(cust => cust.State, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourState,@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_State)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Provincia  -->
</div>

<div class="div50-left">
 <p><%=Html.LabelFor(cust => cust.City) %> </p>
  <%= Html.TextBoxFor(cust => cust.City, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourCity,@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_City)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Ciudad  -->
</div>
<div class="div50-right">
 <p><%=Html.LabelFor(cust => cust.ZipCode) %> </p>
  <%= Html.TextBoxFor(cust => cust.ZipCode, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourZipCode,@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_ZipCode)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Codigo Postal  -->
</div>
<p>&nbsp;</p>
<br/>
<div class="greenhr"><hr /></div>
<input type="submit" value="<%=Resources.Button_Next %>" class="botonverde" />
</fieldset>
<% } %>
</div>

</asp:Content>
