<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.SelectSuscriptionTypeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Select Pay Method
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<p>&nbsp;</p>
<div id="formulario">

<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.SelectSuscriptionType_IntroText1%></h2>
    <p>&nbsp;</p>
 <% using (Html.BeginForm("SelectSuscriptionType", "Account",FormMethod.Post,
                        new { @id = "FormSelectSuscriptionType", @name = "FormSelectSuscriptionType", @style = "style=\"position:relative,\"" }))
 { %>  
<h3><%=Resources.SelectSuscriptionType_IntroText2%></h3>
<fieldset>  


 <div class="error">
 <%: Html.ValidationSummary(true, Resources.ErrorsMsg_SummaryMessage)%>  
 <p><%= Html.ValidationMessageFor(cust => cust.SuscriptionType)%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
</div>  
<div class="div100">
    <div class="div5-left">
        <p><%= Html.RadioButtonFor(cust => cust.SuscriptionType, (int)PaymentSuscryptionType.pstPrepay)%></p>    
    </div>
    <div class="div75-right">

        <h6><span class="SelectUser"> <%=Resources.SelectSuscriptionType_Prepay1%></span></h6>
        <p>&nbsp;</p>	
        <p><%=Resources.SelectSuscriptionType_Prepay2%>
           <%=string.Format(Resources.SelectSuscriptionType_Prepay3,ViewData["DiscountValue"],ViewData["DiscountCurrency"])%></p>
        <p><%=Resources.SelectSuscriptionType_Prepay4%></p>
        <p>&nbsp;</p>
                 <h6><strong><p><%=Resources.SelectSuscriptionType_Prepay5%></p>
                 <p><%=Resources.SelectSuscriptionType_Prepay6%></p>
                 <p><%=Resources.SelectSuscriptionType_Prepay7%></p>
                <p>&nbsp;</p>
                <p>&nbsp;</p></h6>
                 <div class="img-center"><img src="<%=Url.Content( "~/Content/img/Step8.jpg" )%>"/></div>
                </div>       
          </div>

<p>&nbsp;</p>
<h3><%=Resources.SelectSuscriptionType_IntroText3%></h3>

    <div class="div5-left">        
    <%= Html.RadioButtonFor(cust => cust.SuscriptionType, (int)PaymentSuscryptionType.pstPerTransaction)%></div>
     <h6><span class="SelectUser"><%=Resources.SelectSuscriptionType_PerTransaction1%></span></h6>
         <div class="div100M"><%=Resources.SelectSuscriptionType_PerTransaction2%><br />
         <%=string.Format(Resources.SelectSuscriptionType_PerTransaction3,ViewData["ChargeValue"],ViewData["ChargeCurrency"])%>
     </div>
  </div>
<br/>

<div class="greenhr"><hr /></div> 
<input type="submit" value="<%=Resources.Button_Next %>" class="botonverde" />

</fieldset>  
<% } %>



</asp:Content>
