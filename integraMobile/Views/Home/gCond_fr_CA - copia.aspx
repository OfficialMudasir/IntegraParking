<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Condiciones del servicio</title>
    <link rel="stylesheet" href="../../Content/CSS/reset.css" type="text/css" />
    <link rel="stylesheet" href="../../Content/CSS/estilooo.css" type="text/css" />
    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro:400,700,600,400italic,700italic' rel='stylesheet' type='text/css'>
</head>

<body>

<div id="cuerpoConditions">
<div class="header" id="header">
    <div id="menucontainer">
    <div class="left">
    <ul>
        <li><%= Html.ActionLink("English", "gCond_en_US", "Home")%></li>
        <li><%= Html.ActionLink("Español", "gCond_es_ES", "Home")%></li> 
        <li><%= Html.ActionLink("Français", "gCond_fr_CA",  "Home")%></li> 
    </ul>
 
    </div>
    <div class="right">
    <ul>
        <li><%: Html.ActionLink(Resources.SiteMaster_Home, "Index", "Home")%></li>
        <li><a href=""><%=Resources.SiteMaster_Contact %></a></li>
    </ul>
    </div>
    </div>
</div>

<div class="cajaConditions">

<div class="div100-right">
<div class="bluetext">Aviso Legal</div>
<p>&nbsp;</p>
 En cumplimiento con lo establecido en el art&iacute;culo 10 de la Ley 34/2002, de 11 de julio, de Servicios de la Sociedad de la Informaci&oacute;n y de Comercio Electr&oacute;nico, les informamos que el dominio <strong>www.eysamobile.com</strong> es un dominio de la titularidad de ESTACIONAMIENTOS Y SERVICIOS, S.A.U, (en adelanta tambi&eacute;n EYSA) con CIF A28385458 y domicilio en:

<p>&nbsp;</p>
<p><strong>ESTACIONAMIENTOS Y SERVICIOS, S.A.U.</strong></p>
<p>C/Cardenal Marcelo Sp&iacute;nola, 50-52</p>
<p>28016 Madrid</p>
<p>Tel.: +34 912308164</p>
<p><a href="mailto:info@eysamobile.com">info@eysamobile.com</a></p>
<p>&nbsp;</p>
ESTACIONAMIENTOS Y SERVICIOS, S.A.U., sociedad inscrita en el Registro Mercantil de Madrid, Folio 40, Tomo 3.724, General 2.976, de la Secci&oacute;n 3ª del Libro de Sociedades Hoja 28.373, informa a los Usuarios de la p&aacute;gina web
<strong>www.eysamobile.com</strong>, de la que es titular, de las Condiciones de Acceso y Uso de la misma.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Condiciones de uso</div>
<p>&nbsp;</p>
El simple acceso a la Web y a cualquiera de sus p&aacute;ginas y aplicaciones iPhone, Android, Blackberry, Windows Mobile e IVR (en adelante, las "Aplicaciones") atribuye la condici&oacute;n de usuario de la misma (en adelante, el "Usuario") e implica la aceptaci&oacute;n plena y sin reservas de todas y cada una de las condiciones incluidas en este Aviso Legal en la versi&oacute;n publicada por EYSA en el momento mismo en que el Usuario acceda a las Aplicaciones.

La utilizaci&oacute;n de ciertos servicios ofrecidos a los Usuarios a trav&eacute;s de la Web puede encontrarse sometida a condiciones particulares propias (en adelante, las "Condiciones Particulares") que, según los casos, sustituir&aacute;n, completar&aacute;n y/o modificar&aacute;n el presente Aviso Legal. Por lo tanto, con anterioridad a la utilizaci&oacute;n de dichos servicios, el Usuario tambi&eacute;n ha de leer atentamente las correspondientes Condiciones Particulares.

Estacionamientos y Servicios S.A.U. podr&aacute; modificar unilateralmente, en cualquier momento y sin necesidad de aviso previo la presentaci&oacute;n de la Web. En caso de modificaci&oacute;n unilateral del contenido y condiciones de acceso y disfrute de los Servicios por parte de EYSA, &eacute;sta deber&aacute; avisar de dichos cambios, a trav&eacute;s de la Web, con una antelaci&oacute;n m&iacute;nima de 15 d&iacute;as naturales a la fecha de efectividad de los mismos.

Estacionamientos y Servicios S.A.U. se reserva la posibilidad de cancelar unilateralmente, de forma general o particular, el acceso a la Web; al Contenido; a los servicios a los que el Usuario pueda acceder mediante los enlaces o hiperv&iacute;nculos publicados en el dominio de Estacionamientos y Servicios S.A.U. o a aquellos nuevos contenidos, servicios o facilidades adicionales que, en un futuro, pudieran ofrecerse al Usuario.

El Usuario se compromete a no realizar un uso que impida el normal y correcto funcionamiento de las Aplicaciones.

El Usuario declara realizar el uso de las Aplicaciones y del contenido expuesto en las mismas bajo su única y exclusiva responsabilidad y se compromete a realizar un uso correcto de la Aplicaciones y los Servicios de conformidad con las presentes Condiciones Generales de Uso, la ley, la moral y las buenas costumbres generalmente aceptadas y el orden público.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext"> Retirada y suspensi&oacute;n de los servicios</div></p>
<p>&nbsp;</p>
EYSA podr&aacute; retirar o suspender en cualquier momento y sin necesidad de aviso previo la prestaci&oacute;n de los Servicios a aquellos Usuarios que incumplan lo establecido en el presente Aviso Legal.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Legislaci&oacute;n aplicable</div></p>
<p>&nbsp;</p>
El presente Aviso Legal y las Condiciones de uso se rigen en todos y cada uno de sus extremos por la ley española. Para la resoluci&oacute;n de cualquier controversia que pudiera surgir entre el Usuario y EYSA en la interpretaci&oacute;n de las presentes Condiciones de uso, ser&aacute;n competentes los Tribunales de la Ciudad de Madrid, con renuncia expresa a cualquier otro fuero que pudiera corresponderles.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Sujeto</div></p>
<p>&nbsp;</p>
EYSA ha desarrollado un servicio que facilita la compra de tickets electr&oacute;nicos de estacionamiento mediante Aplicaciones y lo pone a disposici&oacute;n de los ayuntamientos que deseen implantarlo. Dicho servicio se podr&aacute; utilizar en cualquier ciudad donde est&eacute;
 implantado nuestro servicio de cobro. El usuario puede utilizar el servicio de forma inmediata sin o con previo registro, de acuerdo con las condiciones publicadas en <strong>www.eysamobile.com</strong>. El importe de los tickets electr&oacute;nicos y, en su caso, el coste del servicio se carga en la factura m&oacute;vil del usuario o contra el saldo de su cuenta virtual de la plataforma Eysa@Mobile. El usuario, al estacionar en la v&iacute;a publica y al utilizar el servicio de EYSA, se obliga a respetar las ordenanzas municipales. Los vigilantes de la zona pueden verificar la existencia de un ticket electr&oacute;nico v&aacute;lido mediante terminales introduciendo la matr&iacute;cula. EYSA es responsable de abonar los importes pagados a las empresas concesionarias. La disponibilidad del servicio depende de la disponibilidad de las redes de telecomunicaciones. EYSA no se responsabiliza de los daños y perjuicios que pudieran derivarse del uso de las Aplicaciones.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Cierre de Contrato</div></p>
<p>&nbsp;</p>
El contrato se cierra al registrarse el usuario en <strong>www.eysamobile.com</strong>. Se establece un contrato para cada número de m&oacute;vil. Un cliente tambi&eacute;n puede cerrar un contrato para varios m&oacute;viles.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Cambios</div></p>
<p>&nbsp;</p>
El usuario se obliga a actualizar los datos en su &aacute;rea de administraci&oacute;n o a comunicar los cambios a EYSA. Especialmente se refiere esta cl&aacute;usula a cambios en los siguientes datos: domicilio, matr&iacute;cula, número de m&oacute;vil, datos bancarios, tarjeta de cr&eacute;dito/d&eacute;bito.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Obligaci&oacute;n de pago</div></p>
<p>&nbsp;</p>
<ul>
<li>El cliente es responsable del pago de los gastos incurridos por el uso del servicio, los gastos para los tickets electr&oacute;nicos de estacionamiento comprados y los gastos del servicio prestado por EYSA. La responsabilidad se extiende tambi&eacute;n al posible uso del servicio por terceros autorizados o no autorizados por el cliente.</li>
<p>&nbsp;</p>
<li>El cliente podr&aacute; recibir mensualmente documento justificativo del pago con desglose del importe de los tickets electr&oacute;nicos y, en su caso, del coste de servicio. Dicho documento identificar&aacute; todas las transacciones con los siguientes datos: número m&oacute;vil, zona de estacionamiento, tiempo aparcado y el importe.</li>
<p>&nbsp;</p>
<li>En caso de no cumplir con la obligaci&oacute;n de pago EYSA suspender&aacute; de forma inmediata el servicio. Antes de reactivar el servicio el usuario ha de realizar un pago v&iacute;a transferencia bancaria o registr&aacute;ndose con una tarjeta de cr&eacute;dito/d&eacute;bito v&aacute;lida. En el caso de no ser posible cobrar el importe EYSA tomar&aacute; las medidas legales oportunas.</li>
<p>&nbsp;</p>
<li>Al optar por el m&eacute;todo de pago domiciliaci&oacute;n bancaria el usuario se obliga a mantener el saldo de su cuenta bancaria positivo para que no se produzcan devoluciones innecesarias. Por cada devoluci&oacute;n que se produce EYSA puede facturar 10 EUR en concepto de gesti&oacute;n de la devoluci&oacute;n.</li>
</ul>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Forma de pago</div></p>
<p>&nbsp;</p>
<p><strong>Sin registro:</strong></p>
El servicio est&aacute; disponible exclusivamente para usuarios de movistar que tienen activado el servicio pagos movistar. Al inicio del estacionamiento el Usuario manda un SMS al número 22022, en el formato indicado en los parqu&iacute;metros de cada zona, o puede usar las aplicaciones para smartphones que sintetizan y mandan el SMS al 22022 al confirmar la compra. El coste del SMS de 0,15 EUR + IVA, m&aacute;s el coste de servicio y el coste de notificaci&oacute;n v&iacute;a SMS (0,1 EUR+IVA) se cobra en el momento de solicitar el ticket, directamente contra la factura m&oacute;vil o contra el saldo del m&oacute;vil prepago del usuario. El usuario ha de asegurarse, que recibe un SMS del sistema que le informa, que tiene un ticket vigente, con el tiempo de inicio/final, coste, municipio, zona y matr&iacute;cula. Mientras que el usuario no haya recibido este SMS no tiene un ticket vigente.
<p>&nbsp;</p>
<p><strong>Con registro:</strong></p>
<p>Los usuarios pueden elegir entre las siguientes formas de pago:</p>
<ul>
<li> Prepago – tarjeta de cr&eacute;dito</li>
<li> Pospago – tarjeta de cr&eacute;dito</li>
<li> Prepago – domiciliaci&oacute;n bancaria</li>
<li> Pospago – domiciliaci&oacute;n bancaria</li>
</ul>
<p>&nbsp;</p>
Al registrarse con una tarjeta de cr&eacute;dito/d&eacute;bito y cada vez que se cambia o actualiza la informaci&oacute;n de la tarjeta se realiza un cargo de 1 EUR que se abona a la cuenta virtual de EYSA. Con esta transacci&oacute;n se verifica si los datos de la tarjeta son correctos y que la tarjeta es activa. El cargo de 1 EUR reducir&aacute; el cargo en la tarjeta en la siguiente liquidaci&oacute;n. EYSA puede deshabilitar el uso de la forma de pago con domiciliaci&oacute;n bancaria a usuarios que no cumplen con sus obligaciones y en consecuencia se producen devoluciones de los recibos.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext"> Precios</div></p>
<p>&nbsp;</p>
El usuario paga la tasa o precio público de estacionamiento de acuerdo con las tarifas vigentes reguladas en las ordenanzas del ayuntamiento para cada zona. Dependiendo de la ciudad puede que el usuario tambi&eacute;n pague la tarifa de servicio de EYSA que se publica en la p&aacute;gina web en el apartado "Localidades" para cada ciudad, en las aplicaciones y/o en los medios de comunicaci&oacute;n (vinilos, tr&iacute;pticos).
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Responsibilidades</div></p>
<p>&nbsp;</p>
<p>1.- La prestaci&oacute;n del servicio depende de la disponibilidad de la red de telecomunicaci&oacute;n en el momento de iniciar el estacionamiento. La misma condici&oacute;n se aplica en el momento de terminar el estacionamiento al usar el mecanismo "start/stop" o la opci&oacute;n "desaparcar", en el caso de que las mismas estuvieran implantadas. Al no estar disponible el servicio el usuario est&aacute; obligado a utilizar un modo de pago alternativo ofrecido por la Empresa Concesionaria. El usuario tiene la obligaci&oacute;n de comprobar, si las transacciones han finalizado correctamente. En el caso de usar el canal de llamada recibir&aacute; la confirmaci&oacute;n del sistema interactivo v&iacute;a voz y en el canal www o aplicaciones iPhone/Android/Blackberry/Windows Mobile ser&aacute; redirigido a una p&aacute;gina que confirma la finalizaci&oacute;n exitosa de la transacci&oacute;n.
 </p>
 <p>&nbsp;</p>
<p> 2.- El usuario es responsable de introducir correctamente los datos de la zona donde quiere estacionar, la matr&iacute;
  cula y la hora de salida. La introducci&oacute;n err&oacute;nea de la matr&iacute;
  cula, de la zona y de la hora no perjudica el derecho de la Empresa Concesionaria de expedir una multa. EYSA no se har&aacute; responsable de posibles daños y perjuicios por el uso indebido del servicio.
</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Pol&iacute;tica de Privacidad</div></p>
<p>&nbsp;</p>
1.- Es necesario que el Usuario se registre y facilite sus datos personales (entre otros supuestos; acceder a la contrataci&oacute;n del servicio de pago por m&oacute;vil; solicitar informaci&oacute;n; adquirir productos;…), la recogida y el tratamiento de los datos personales se llevar&aacute; a cabo de conformidad y en cumplimiento con los principios recogidos en la Ley Org&aacute;nica 15/1999, de 13 de diciembre, de Protecci&oacute;n de Datos (LOPD).

En su caso, el Usuario ser&aacute; advertido, convenientemente, de la necesidad de facilitar sus datos personales. En caso de facilitar la direcci&oacute;n de correo electr&oacute;nico u otro medio de comunicaci&oacute;n electr&oacute;nica, el Usuario autoriza expresamente a la Entidad para que dicho medio sea utilizado como medio de comunicaci&oacute;n con el mismo, para dar respuesta a su solicitud y/o consulta, as&iacute;
 como para poderle transmitirle informaci&oacute;n relativa a la Entidad e informarle de cualesquiera otros cambios relevantes que se produzcan en el Portal.

Dicho tratamiento de los datos es llevado a cabo de conformidad con los referidos principios y, en particular, con sujeci&oacute;n al deber de confidencialidad y secreto, habiendo adoptado la Entidad las medidas de seguridad adecuadas para evitar cualquier alteraci&oacute;n, p&eacute;rdida, acceso no autorizado o daño de los datos personales e informaci&oacute;n registrada.

El Usuario tiene reconocidos los derechos de acceso, rectificaci&oacute;n, cancelaci&oacute;n y oposici&oacute;n (A.R.C.O) los cuales podr&aacute; ejercitar dirigiendo una comunicaci&oacute;n postal, acompañada de su DNI, identificada con la referencia "protecci&oacute;n de datos" a la siguiente direcci&oacute;n: ESTACIONAMIENTOS Y SERVICIOS, S.A.U., c/ Cardenal Marcelo Sp&iacute;nola 50-52, 1ª. Planta, 28016 Madrid.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Incumplimiento por parte del usuario</div></p>
<p>&nbsp;</p>
1.- Al incumplir el usuario las obligaciones Estacionamientos y Servicios S.A.U. puede rescindir el contrato unilateralmente de forma inmediata.
<p>&nbsp;</p>
<p>&nbsp;</p>
<p><div class="bluetext">Duraci&oacute;n/Resoluci&oacute;n</div></p>
<p>&nbsp;</p>
1.- El contrato se cierra de forma indefinida al iniciar el primer estacionamiento con el servicio SMS o al darse de alta al servicio en las Aplicaciones o v&iacute;a  l&iacute;nea de atenci&oacute;n al cliente.
<p>&nbsp;</p>
2.- EYSA tiene el derecho de suspender o finalizar la prestaci&oacute;n del servicio en los siguientes casos:
El usuario no disponga de una forma de pago activa y v&aacute;lida (factura m&oacute;vil, adeudo domiciliado, tarjeta de cr&eacute;dito/d&eacute;bito)
En el caso de que los cobros de adeudos se deshagan o no se realicen dentro del plazo establecido por la ley. EYSA desactivar&aacute; el servicio de forma temporal hasta su aclaraci&oacute;n.
En el caso de que el usuario no acepte o incumpla las condiciones de uso aqu&iacute;
 definidas.
En el caso de rebasar el m&aacute;ximo de compra v&iacute;a SMS por d&iacute;
a de 15 EUR y por mes de 60 EUR,
 EYSA desactiva el servicio v&iacute;a SMS para el resto del periodo. El usuario tiene la posibilidad de continuar utilizando el servicio previo registro.
<p>&nbsp;</p>
ESTACIONAMIENTOS Y SERVICIOS, S.A.U. - Todos los derechos reservados - ©2013 <a href="mailto:info@eysamobile.com">info@eysamobile.com</a>
<p>&nbsp;</p>
</div>
</div>

<br/>
<div class="greenhr"><hr/></div>
<br/>
<input type="button" name="Volver" value="Volver" class="botonverde" onclick="window.location = 'Index';" />
<br/>

</div>

</body>
</html>
