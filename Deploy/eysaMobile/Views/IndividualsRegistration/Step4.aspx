<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.CustomerInscriptionModelStep4>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Step4
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

    <!--
  
    var previousPlateValue = '';
    var platePattern = /^[A-Z0-9]+$/;
    var maxPlateLength =50;

    function validateInputPlate(text, id) {

        var newValue = text;
        newValue=newValue.toUpperCase();


        if (newValue.length>maxPlateLength)
        {
            document.getElementById(id).value = previousPlateValue;
        }
        else
        {
            if ((newValue.match(platePattern)) || (newValue.length==0)) {
                // Valid input; update previousValue:
                previousPlateValue = newValue;
                document.getElementById(id).value = newValue;
            } else {
                // Invalid input; reset field value:
                document.getElementById(id).value = previousPlateValue;
            }
        }

     
    }
    //-->

</script>
<div class="div100Sup1"><img src="../Content/img/Step4-<%=((CultureInfo)Session["Culture"]).Name%>.png"/></div>

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
 <% using (Html.BeginForm("Step4", "IndividualsRegistration", FormMethod.Post)) { %>  
 <p>&nbsp;</p>
<h3><%=Resources.CustomerInscriptionModel_Step4UserInformation%></h3>
 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>
 <p><%= Html.ValidationMessageFor(cust => cust.Username)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Password)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.ConfirmPassword)%></p>
 <p><%= Html.ValidationMessageFor(cust => cust.Plate)%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  
<fieldset>  


<div class="div50-center">
 <% if (ViewData["UsernameEqualsEmail"].ToString()=="1"){ %>
<p><%=Html.LabelFor(cust => cust.Email) %> </p>
      <%= Html.TextBoxFor(cust => cust.Email, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourUsername,4,50),@readonly="true" })%>
      <%= Html.HiddenFor(cust => cust.Username)%>
<%}else{ %>
<p><%=Html.LabelFor(cust => cust.Username) %> </p>
  <%= Html.TextBoxFor(cust => cust.Username, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourUsername,4,50),@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Username)+"');", 
            @oninput="this.setCustomValidity('');" })%>
      <%= Html.HiddenFor(cust => cust.Email)%>
<%} %>
  <!-- Username -->

</div> 
<div class="div50-center">
<p><%=Html.LabelFor(cust => cust.Password) %> </p>
  <%= Html.PasswordFor(cust => cust.Password, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourPassword,4,50),@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Password)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Password -->
</div>
<div class="div50-center">
 <p><%=Html.LabelFor(cust => cust.ConfirmPassword) %> </p>
  <%= Html.PasswordFor(cust => cust.ConfirmPassword, new { @placeholder = string.Format(Resources.CustomerInscriptionModel_WriteYourConfirmPassword,4,50),@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_ConfirmPassword)+"');", 
            @oninput="this.setCustomValidity('');" })%>
  <!--  Repeat Password -->
</div>
<div class="div50-center">
<p><%=Resources.CustomerInscriptionModel_Step4SecurityAdvice %></p>
</div>
<p>&nbsp;</p>
<div class="div50-center">
<p>&nbsp;</p>
<h3><%=Resources.CustomerInscriptionModel_Step4PreferredPlate%></h3>
<p><%=Html.LabelFor(cust => cust.Plate) %> </p>
  <%= Html.TextBoxFor(cust => cust.Plate, new { @placeholder = Resources.CustomerInscriptionModel_WriteYourPlate,@required="true", 
            @oninvalid="this.setCustomValidity('"+string.Format(Resources.ErrorsMsg_RequiredField,Resources.CustomerInscriptionModel_Plate)+"');", 
            @oninput="this.setCustomValidity('');", onkeyup="validateInputPlate(this.value, 'Plate')" })%>
  <!--  Plate -->
</div>
<p>&nbsp;</p>

<div class="div50-center">
<div class="div5-left"><%: Html.CheckBoxFor(m => m.ConfirmServiceCondictions, new { @style = "margin-bottom:0;margin-left:0px;display:inline;" })%>
</div>
<div><%=Resources.CustomerInscriptionModel_Step4IHaveReadAndAccept%>
    <a href="gCond_<%=ViewData["lang_for_gCond"]%>" title="Conditions"  target="_blank"><%=Resources.CustomerInscriptionModel_Step4UseConditions%></a>
</div>
</div>


<p>&nbsp;</p>
<br/>
<div class="greenhr"><hr /></div> 
<input type="submit" value="<%=Resources.CustomerInscriptionModel_StepBttnFinish %>" class="botonverde" />
<input type="button" value="<%=Resources.Button_Back %>" class="botongris" 
    onclick="location.href='<%=Url.Action("Step3", "IndividualsRegistration", null)%>'" />
<% } %>
</fieldset>

</div>

</asp:Content>
