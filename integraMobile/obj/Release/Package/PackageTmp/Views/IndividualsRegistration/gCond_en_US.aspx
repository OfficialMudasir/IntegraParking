<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8">
    <title>Service Conditions</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>

    <link rel="stylesheet" href="~/Content/css/style.css">
    <link rel="stylesheet" href="~/Content/css/loader-style.css">
    <link rel="stylesheet" href="~/Content/css/bootstrap.min.css">
    <link rel="stylesheet" href="~/Content/css/extra-pages.css">
    <style type="text/css">
        @media (max-width: 767px) {
            body {
                padding: 0 !important;
            }
        }
    </style>

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
        <script src="https://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->
    <!-- Fav and touch icons -->
    <link rel="shortcut icon" href="~/Content/ico/favicon.ico">
</head>

<body>

    <div id="preloader">
        <div id="status">&nbsp;</div>
    </div>

    <nav role="navigation" class="navbar navbar-static-top no-print">
        <div class="container-fluid">

            <div class="navbar-header">
                <button data-target="#bs-example-navbar-collapse-1" data-toggle="collapse" class="navbar-toggle" type="button">
                    <span class="entypo-menu"></span>
                </button>

                <div class="sm-logo">
                    <a href="<%= Url.Action("Index", "Home") %>"><img src="<%= Url.Content("~/Content/img/logo.svg") %>" alt="Integra - Parking Solutions"></a>
                </div>

            </div>

            <div id="bs-example-navbar-collapse-1" class="collapse navbar-collapse">

                <ul style="margin-right:0;" class="nav navbar-nav navbar-right">
                    <li>
                        <a data-toggle="dropdown" class="dropdown-toggle" href="#">
                            <span class="entypo-globe"></span>&#160;&#160;Change Language <b class="caret"></b></a>
                        <ul role="menu" class="dropdown-setting dropdown-menu">
                            <li class="f16"><%= Html.ActionLink("English", "gCond_en_US", "IndividualsRegistration",  new {@class = "flag gb"})%></li>
                            <li class="f16"><%= Html.ActionLink("Español", "gCond_es_ES", "IndividualsRegistration",  new {@class = "flag es"})%></li>
                        </ul>
                    </li>
                </ul>

            </div>
        </div>
    </nav>

    <div class="wrap-fluid">
        <div class="container-fluid paper-wrap bevel tlbr">

            <div class="row">
                <div id="paper-top">
                    <div class="col-sm-12">
                        <h2 class="tittle-content-header">
                            <span>Service Conditions
                            </span>
                        </h2>

                    </div>
                </div>
            </div>

            <ul id="breadcrumb">
                <li>
                    <span class="entypo-home"></span>
                </li>
                <li><i class="fa fa-lg fa-angle-right"></i>
                </li>
                <li><a href="<%= Url.Action("Index", "Home") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
                </li>
                <li><i class="fa fa-lg fa-angle-right"></i>
                </li>
                <li>Service Conditions
                </li>
            </ul>

            <div class="content-wrap">
                <div class="row">


                    <div class="col-sm-12">

                        <div class="nest">
                            <div class="title-alt">
                                <h6>
                                    Legal Terms</h6>

                            </div>

                            <div class="body-nest">
                                <p>In compliance with the provisions of Article 10 of Law 34/2002, of July 11th, Information Society Services and Electronic Commerce, we inform you that the domain <strong> www.eysamobile.com </strong> belongs to Estacionamientos y Servicios, S.A.U, (in forward also EYSA) with CIF A28385458 and address to:</p>
                                <p><strong>ESTACIONAMIENTOS Y SERVICIOS, S.A.U.</strong></p>
                                <ul>
                                    <li>C/Cardenal Marcelo Sp&iacute;nola, 50-52</li>
                                    <li>28016 Madrid</li>
                                    <li>Tel.: +34 912308164</li>
                                    <li><a href="mailto:info@eysamobile.com">info@eysamobile.com</a></li>
                                </ul>
                                <p>Estacionamientos y Servicios, S.A.U, a company registered in the Mercantile Register of Madrid, Page 40, Volume 3724, General 2976, Section 3 of the Companies Book Sheet 28,373, informs Users of the website www.eysamobile.com, of which holds, the Access Conditions and Use of it.</p>
                                <h2>1. Terms of Use</h2>
                                <p>Simple access to the Website and any of its pages and applications such as iPhone, Android, Blackberry, Windows Mobile and IVR (hereinafter, the "Applications") attributes the condition of user thereof (hereinafter, the "User") and implies the full and unreserved acceptance of each and every one of the conditions included in this Legal Notice in the version published by EYSA in the moment when the user accesses the Applications.</p>
                                <p>The use of certain services offered to users through the Web may be subject to specific conditions (hereinafter, the "Terms") which, depending on the case, substitute, complete and / or modify this Legal Notice. Therefore, prior to the use of these services, the user must also carefully read the relevant Conditions.</p>
                                <p>Estacionamientos y Servicios, S.A.U may unilaterally change at any time without prior notice submission of the Web. In case of unilateral modification of the content and conditions of access and enjoyment of the Services by EYSA, it shall notify such changes through the Web, with at least 15 calendar days before the effective date of such.</p>
                                <p>Estacionamientos y Servicios, S.A.U reserves the right to unilaterally cancel, in general or particular, access to the Web, the Content, the services to which the user may access through links or hyperlinks published on the domain of Estacionamientos y Servicios S.A.U. or those new contents, services or additional facilities in the future, may be offered to the user.</p>
                                <p>The User agrees not to make a use that prevents the normal functioning of the Applications.</p>
                                <p>The User declares make use of the applications and content posted on the same under their sole responsibility and undertakes to make proper use of the Applications and Services in accordance with these Terms of Use, the law, the morality and generally accepted good practices and public order.</p>
                                <h2>2. Withdrawal and suspension of services</h2>
                                <p>EYSA may withdraw or suspend at any time without prior notice the provision of the Services to users who violate the provisions of this Legal Notice.</p>
                                <h2>3. Applicable law</h2>
                                <p>This Legal Notice and Terms of Use are governed in each and every one of its clauses by Spanish law. For the resolution of any dispute arising between the User and EYSA in the interpretation of these Terms of Use shall be the Courts of the City of Madrid, expressly waiving any other jurisdiction that may apply.</p>
                                <h2>4. Subject</h2>
                                <p>EYSA has developed a service that makes buying electronic parking tickets by applications and makes it available to municipalities wishing to implement it. This service can be used in any city where our collection service is implemented. The user can use the service immediately with or without prior registration in accordance with the conditions published in www.eysamobile.com. The amount of electronic tickets and, where appropriate, the service cost is charged to the user's mobile bill or against virtual account balance Eysa@mobile platform. The user, when parking on public streets and using EYSA service, is obliged to respect by laws. The guards in the area can check the existence of a valid electronic ticket by entering the registration terminals. EYSA is responsible for paying the amounts paid to concessionaires. Service availability depends on the availability of telecommunication networks. EYSA will not be held responsible for damages that may result from use of the Applications.</p>
                                <h2>5. Closing Contract</h2>
                                <p>The contract is closed when registering the user on www.eysamobile.com. A contract is drawn for each mobile number. A customer can also close a contract for multiple phones.</p>
                                <h2>6. Changes</h2>
                                <p>The user agrees to update the data in their administration area or communicate changes to EYSA. Especially, this clause refers to changes in the following information: address, registration number, mobile number, bank details, credit / debit card.</p>
                                <h2>7. Payment Obligation</h2>
                                <ul>
                                    <li>The customer is responsible for payment of expenses incurred by the use of the service, the costs for electronic parking tickets purchased and service expenses paid by EYSA. The responsibility also extends to the possible use of the service by unauthorized third party or customer.</li>
                                    <li>The customer will receive a monthly payment document from the breakdown of the amount of electronic tickets, if any, of the cost of service. This document will identify all transactions with the following data: mobile number, parking area, parked time and amount.</li>
                                    <li> Failure to meet the payment obligation EYSA will immediately suspend service. Before resetting the service the user has to make a payment via bank transfer or registering with a credit / debit card is required. In case it is not possible to charge the amount EYSA will take legal actions on this matter.</li>
                                    <li> By opting for direct debit payment method the user is obliged to maintain the balance of their bank account positive that there are no unnecessary returns. For each return that occurs Eysa can bill 10 Euros as management fee for every return.</li>
                                </ul>
                                <h2>8. Payment methods</h2>
                                <p>Users can choose from the following payment methods:</p>
                                <ul>
                                    <li>Prepaid - Credit Card</li>
                                    <li>Postpaid - Credit Card</li>
                                    <li>Prepaid - debit</li>
                                    <li>Postpaid - debit</li>
                                </ul>
                                <p>By registering with a credit/debit card and every time you change or update the information in the card a 1 EUR charge is performed which is payable to the virtual EYSA account. With this transaction can be verified if the card data are correct and that the card is active. The charge of 1 EUR reduces the charge on the card in the next settlement. EYSA can disable the use of the payment method to direct debit to users who do not fulfill their obligations and consequently produce returns of receipts.</p>
                                <h2>9. Prices</h2>
                                <p>The user pays the fee or parking retail price according to the current rates regulated by the ordinances of the city council for each area. Depending on the city you may also pay user fee EYSA service that is published on the website under "Locations" for each city, in applications and/or media (vinyl, brochures).</p>
                                <h2>10. Responsibilities</h2>
                                <p>The service depends on the availability of the telecommunication network at the time of initiating the parking lot. The same condition applies when completing the parking lot using the mechanism "start / stop" or the "unparking" in the event that they were implemented. When the service isn´t available the user is forced to use an alternative payment method offered by the Company Dealership. The user is obliged to check whether the transactions have completed successfully. In the case of using the calling channel the user will receive the confirmation through a voice interactive system and channel or applications www.eysamobile.com iPhone / Android / Windows Mobile will be redirected to a page that confirms the successful completion of the transaction.</p>
                                <p>The user is responsible for entering data correctly the area where you want to park, enrollment and departure time. The erroneous introduction of tuition, area and time does not prejudice the right of the Concessionaire Company to issue a fine. EYSA not be held liable for any damages for the misuse of the service.</p>
                                </ol>
                                <h2>11. Privacy Policy</h2>
                                <p>It is necessary for the user to register and provide personal information (among other cases, access to the hiring of mobile payment service, request information, purchase products, ...), the collection and processing of personal data is carried out consistently and in compliance with the principles contained in the Organic Law 15/1999, of 13th December, Data Protection (LOPD).Given the case, the user will be alerted of the need to provide personal data. By providing any email address or other means of electronic communication, the user expressly authorizes the entity to which the medium is used as a means of communication with it, to respond to your request and / or consultation, including providing users with information concerning the Company and inform about any other relevant changes that may occur in the Portal. This treatment of the data is carried out in accordance with those principles and, in particular, subject to the duty of confidentiality and secrecy, having adopted the Entity adequate security measures to prevent any alteration, loss, unauthorized access or damage of personal data and information recorded. The User has the right to access, rectification, cancellation and opposition (ARC) which can be exercised by postal communication with its ​​ID, ​​identified by reference "data protection" to the following address: Estacionamientos y Servicios, S.A.U, c / Cardenal Marcelo Spinola 50-52, 1st. Planta, 28016 Madrid.</p>
                                <h2>12. User Compliance Failure</h2>
                                <p>When the user breaches obligations Estacionamientos y Servicios, S.A.U may unilaterally terminate the contract immediately.</p>
                                <h2>13. Length/Resolution</h2>
                                <ol>
                                    <li>The contract is closed indefinitely to start the first parking lot with the service or register for the service in the Applications or via customer service hotline.</li>
                                    <li>EYSA has the right to suspend or terminate the provision of the service in the following cases:</li>
                                    <li>The user does not have an active and valid payment method (mobile invoice, automatic payment, debit or credit card)</li>
                                    <li>In the case of collections of debts that fall apart or not made ​​within the period prescribed by law. EYSA will disable the service temporarily until the payment is received.</li>
                                    <li>In the event that the user does not accept or breach the terms of use here defined.</li>
                                </ol>
                                <p><em>ESTACIONAMIENTOS Y SERVICIOS, S.A.U. - Todos los derechos reservados - &copy; 2013</em> <a href="mailto:info@eysamobile.com">info@eysamobile.com</a></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

           <div class="footer-space"></div>
            <div id="footer">
                <div class="copyright">
                    &copy; Integra, <%= DateTime.Now.Year.ToString() %>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript" src="<%= Url.Content("~/Content/js/preloader.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/app.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/load.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/js/main.js") %>"></script>

    <% Html.RenderPartial("~/Views/Shared/GoogleAnalytics.ascx"); %>

</body>
</html>