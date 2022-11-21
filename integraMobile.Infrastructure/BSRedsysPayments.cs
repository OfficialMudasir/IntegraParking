using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using integraMobile.Infrastructure.BSRedsysWs;
using integraMobile.Infrastructure.RedsysAPI;


namespace integraMobile.Infrastructure
{
    public class BSRedsysPayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(BSRedsysPayments));

        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

        private const string UNKNOWN_COF_TXT_INI = "999999999999999";

        public enum BSRedsysErrorCode
        {
            SIS0000 = 0,
            SIS0007 = 7,
            SIS0008 = 8,
            SIS0009 = 9,
            SIS0010 = 10,
            SIS0011 = 11,
            SIS0014 = 14,
            SIS0015 = 15,
            SIS0016 = 16,
            SIS0017 = 17,
            SIS0018 = 18,
            SIS0019 = 19,
            SIS0020 = 20,
            SIS0021 = 21,
            SIS0022 = 22,
            SIS0023 = 23,
            SIS0024 = 24,
            SIS0025 = 25,
            SIS0026 = 26,
            SIS0027 = 27,
            SIS0028 = 28,
            SIS0030 = 30,
            SIS0031 = 31,
            SIS0033 = 33,
            SIS0034 = 34,
            SIS0037 = 37,
            SIS0038 = 38,
            SIS0040 = 40,
            SIS0041 = 41,
            SIS0042 = 42,
            SIS0043 = 43,
            SIS0046 = 46,
            SIS0051 = 51,
            SIS0054 = 54,
            SIS0055 = 55,
            SIS0056 = 56,
            SIS0057 = 57,
            SIS0058 = 58,
            SIS0059 = 59,
            SIS0060 = 60,
            SIS0061 = 61,
            SIS0062 = 62,
            SIS0063 = 63,
            SIS0064 = 64,
            SIS0065 = 65,
            SIS0066 = 66,
            SIS0067 = 67,
            SIS0068 = 68,
            SIS0069 = 69,
            SIS0070 = 70,
            SIS0071 = 71,
            SIS0072 = 72,
            SIS0074 = 74,
            SIS0075 = 75,
            SIS0076 = 76,
            SIS0077 = 77,
            SIS0078 = 78,
            SIS0079 = 79,
            SIS0080 = 80,
            SIS0081 = 81,
            SIS0084 = 84,
            SIS0085 = 85,
            SIS0086 = 86,
            SIS0089 = 89,
            SIS0092 = 92,
            SIS0093 = 93,
            SIS0094 = 94,
            SIS0097 = 97,
            SIS0098 = 98,
            SIS0112 = 112,
            SIS0113 = 113,
            SIS0114 = 114,
            SIS0115 = 115,
            SIS0116 = 116,
            SIS0117 = 117,
            SIS0118 = 118,
            SIS0119 = 119,
            SIS0120 = 120,
            SIS0121 = 121,
            SIS0122 = 122,
            SIS0123 = 123,
            SIS0124 = 124,
            SIS0126 = 126,
            SIS0132 = 132,
            SIS0133 = 133,
            SIS0139 = 139,
            SIS0142 = 142,
            SIS0197 = 197,
            SIS0198 = 198,
            SIS0199 = 199,
            SIS0200 = 200,
            SIS0214 = 214,
            SIS0216 = 216,
            SIS0217 = 217,
            SIS0218 = 218,
            SIS0219 = 219,
            SIS0220 = 220,
            SIS0221 = 221,
            SIS0222 = 222,
            SIS0223 = 223,
            SIS0224 = 224,
            SIS0225 = 225,
            SIS0226 = 226,
            SIS0227 = 227,
            SIS0229 = 229,
            SIS0230 = 230,
            SIS0231 = 231,
            SIS0252 = 252,
            SIS0253 = 253,
            SIS0254 = 254,
            SIS0255 = 255,
            SIS0256 = 256,
            SIS0257 = 257,
            SIS0258 = 258,
            SIS0261 = 261,
            SIS0270 = 270,
            SIS0274 = 274,
            SIS0295 = 295,
            SIS0296 = 296,
            SIS0297 = 297,
            SIS0298 = 298,
            SIS0319 = 319,
            SIS0321 = 321,
            SIS0322 = 322,
            SIS0323 = 323,
            SIS0324 = 324,
            SIS0325 = 325,
            SIS0327 = 327,
            SIS0330 = 330,
            SIS0331 = 331,
            SIS0333 = 333,
            SIS0334 = 334,
            SIS0429 = 429,
            SIS0430 = 430,
            SIS0431 = 431,
            SIS0432 = 432,
            SIS0433 = 433,
            SIS0434 = 434,
            SIS0435 = 435,
            SIS0436 = 436,
            SIS0437 = 437,
            SIS0438 = 438,
            SIS0439 = 439,
            SIS0444 = 444,
            SIS0448 = 448,
            SIS0449 = 449,
            SIS0450 = 450,
            SIS0451 = 451,
            SIS0452 = 452,
            SIS0453 = 453,
            SIS0454 = 454,
            SIS0455 = 455,
            SIS0456 = 456,
            SIS0457 = 457,
            SIS0458 = 458,
            SIS0459 = 459,
            SIS0460 = 460,
            SIS0461 = 461,
            SIS0462 = 462,
            SIS0463 = 463,
            SIS0464 = 464,
            SIS0465 = 465,

            _001 = 1,
            _002 = 2,

            _101 = 101,
            _102 = 102,
            _104 = 104,
            _106 = 106,
            _107 = 107,
            _109 = 109,
            _110 = 110,
            //_114 = 114,
            //_116 = 116,
            //_118 = 118,
            _125 = 125,
            _129 = 129,
            _167 = 167,
            _180 = 180,
            _181 = 181,
            _182 = 182,
            _184 = 184,
            _190 = 190,
            _191 = 191,
            _201 = 201,
            _202 = 202,
            _204 = 204,
            _207 = 207,
            _208 = 208,
            _209 = 209,
            _280 = 280,
            _290 = 290,
            _400 = 400,
            _480 = 480,
            _481 = 481,
            _500 = 500,
            _501 = 501,
            _502 = 502,
            _503 = 503,
            _9928 = 9928,
            _9929 = 9929,
            _904 = 904,
            _909 = 909,
            _912 = 912,
            _913 = 913,
            _916 = 916,
            _928 = 928,
            _940 = 940,
            _941 = 941,
            _942 = 942,
            _943 = 943,
            _944 = 944,
            _945 = 945,
            _946 = 946,
            _947 = 947,
            _949 = 949,
            _950 = 950,
            _965 = 965,
            _9064 = 9064,
            _9078 = 9078,
            _9093 = 9093,
            _9094 = 9094,
            _9104 = 9104,
            _9126 = 9126,
            _9142 = 9142,
            _9218 = 9218,
            _9253 = 9253,
            _9256 = 9256,
            _9261 = 9261,
            _9283 = 9283,
            _9281 = 9281,
            _9334 = 9334,
            _9912 = 9912,
            _9913 = 9913,
            _9914 = 9914,
            _9915 = 9915,
            //_9928 = 9928,
            //_9929 = 9929,
            _9997 = 9997,
            _9998 = 9998,
            _9999 = 9999,

            ERROR = -1
        }

        private static Dictionary<BSRedsysErrorCode, string> ErrorMessageDict = new Dictionary<BSRedsysErrorCode, string>()
        {
            {BSRedsysErrorCode.SIS0000, "Correcto"},
            {BSRedsysErrorCode.SIS0007, "Error al desmontar el XML de entrada o error producido al acceder mediante un sistema de firma antiguo teniendo configurado el tipo de clave HMAC SHA256"},
            {BSRedsysErrorCode.SIS0008, "Error falta Ds_Merchant_MerchantCode"},
            {BSRedsysErrorCode.SIS0009, "Error de formato en Ds_Merchant_MerchantCode"},
            {BSRedsysErrorCode.SIS0010, "Error falta Ds_Merchant_Terminal"},
            {BSRedsysErrorCode.SIS0011, "Error de formato en Ds_Merchant_Terminal"},
            {BSRedsysErrorCode.SIS0014, "Error de formato en Ds_Merchant_Order"},
            {BSRedsysErrorCode.SIS0015, "Error falta Ds_Merchant_Currency"},
            {BSRedsysErrorCode.SIS0016, "Error de formato en Ds_Merchant_Currency"},
            {BSRedsysErrorCode.SIS0017, "Error no se admiten operaciones en pesetas"},
            {BSRedsysErrorCode.SIS0018, "Error falta Ds_Merchant_Amount"},
            {BSRedsysErrorCode.SIS0019, "Error de formato en Ds_Merchant_Amount"},
            {BSRedsysErrorCode.SIS0020, "Error falta Ds_Merchant_MerchantSignature"},
            {BSRedsysErrorCode.SIS0021, "Error la Ds_Merchant_MerchantSignature viene vacía"},
            {BSRedsysErrorCode.SIS0022, "Error de formato en Ds_Merchant_TransactionType"},
            {BSRedsysErrorCode.SIS0023, "Error Ds_Merchant_TransactionType desconocido"},
            {BSRedsysErrorCode.SIS0024, "Error Ds_Merchant_ConsumerLanguage tiene mas de 3 posiciones"},
            {BSRedsysErrorCode.SIS0025, "Error de formato en Ds_Merchant_ConsumerLanguage"},
            {BSRedsysErrorCode.SIS0026, "Error No existe el comercio / terminal enviado"},
            {BSRedsysErrorCode.SIS0027, "Error Moneda enviada por el comercio es diferente a la que tiene"},
            {BSRedsysErrorCode.SIS0028, "Error Comercio / terminal está dado de baja"},
            {BSRedsysErrorCode.SIS0030, "Error en un pago con tarjeta ha llegado un tipo de operación que no es ni pago ni preautorización"},
            {BSRedsysErrorCode.SIS0031, "Método de pago no definido"},
            {BSRedsysErrorCode.SIS0033, "Error en un pago con móvil ha llegado un tipo de operación que no es ni pago ni preautorización"},
            {BSRedsysErrorCode.SIS0034, "Error de acceso a la Base de Datos"},
            {BSRedsysErrorCode.SIS0037, "El número de teléfono no es válido"},
            {BSRedsysErrorCode.SIS0038, "Error en java"},
            {BSRedsysErrorCode.SIS0040, "Error el comercio / terminal no tiene ningún método de pago asignado"},
            {BSRedsysErrorCode.SIS0041, "Error en el cálculo de la HASH de datos del comercio."},
            {BSRedsysErrorCode.SIS0042, "La firma enviada no es correcta"},
            {BSRedsysErrorCode.SIS0043, "Error al realizar la notificación on-line"},
            {BSRedsysErrorCode.SIS0046, "El bin de la tarjeta no está dado de alta"},
            {BSRedsysErrorCode.SIS0051, "Error número de pedido repetido"},
            {BSRedsysErrorCode.SIS0054, "Error no existe operación sobre la que realizar la devolución"},
            {BSRedsysErrorCode.SIS0055, "Error existe más de un pago con el mismo número de pedido"},
            {BSRedsysErrorCode.SIS0056, "La operación sobre la que se desea devolver no está autorizada"},
            {BSRedsysErrorCode.SIS0057, "El importe a devolver supera el permitido"},
            {BSRedsysErrorCode.SIS0058, "Inconsistencia de datos, en la validación de una confirmación"},
            {BSRedsysErrorCode.SIS0059, "Error no existe operación sobre la que realizar la confirmación"},
            {BSRedsysErrorCode.SIS0060, "Ya existe una confirmación asociada a la preautorización"},
            {BSRedsysErrorCode.SIS0061, "La preautorización sobre la que se desea confirmar no está autorizada"},
            {BSRedsysErrorCode.SIS0062, "El importe a confirmar supera el permitido"},
            {BSRedsysErrorCode.SIS0063, "Error. Número de tarjeta no disponible"},
            {BSRedsysErrorCode.SIS0064, "Error. El número de tarjeta no puede tener más de 19 posiciones"},
            {BSRedsysErrorCode.SIS0065, "Error. El número de tarjeta no es numérico"},
            {BSRedsysErrorCode.SIS0066, "Error. Mes de caducidad no disponible"},
            {BSRedsysErrorCode.SIS0067, "Error. El mes de la caducidad no es numérico"},
            {BSRedsysErrorCode.SIS0068, "Error. El mes de la caducidad no es válido"},
            {BSRedsysErrorCode.SIS0069, "Error. Año de caducidad no disponible"},
            {BSRedsysErrorCode.SIS0070, "Error. El Año de la caducidad no es numérico"},
            {BSRedsysErrorCode.SIS0071, "Tarjeta caducada"},
            {BSRedsysErrorCode.SIS0072, "Operación no anulable"},
            {BSRedsysErrorCode.SIS0074, "Error falta Ds_Merchant_Order"},
            {BSRedsysErrorCode.SIS0075, "Error el Ds_Merchant_Order tiene menos de 4 posiciones o más de 12"},
            {BSRedsysErrorCode.SIS0076, "Error el Ds_Merchant_Order no tiene las cuatro primeras posiciones numéricas"},
            {BSRedsysErrorCode.SIS0077, "Error el Ds_Merchant_Order no tiene las cuatro primeras posiciones numéricas. No se utiliza"},
            {BSRedsysErrorCode.SIS0078, "Método de pago no disponible"},
            {BSRedsysErrorCode.SIS0079, "Error al realizar el pago con tarjeta"},
            {BSRedsysErrorCode.SIS0080, "Error al tomar los datos de pago con tarjeta desde el XML"},
            {BSRedsysErrorCode.SIS0081, "La sesión es nueva, se han perdido los datos almacenados"},
            {BSRedsysErrorCode.SIS0084, "El valor de Ds_Merchant_Conciliation es nulo"},
            {BSRedsysErrorCode.SIS0085, "El valor de Ds_Merchant_Conciliation no es numérico"},
            {BSRedsysErrorCode.SIS0086, "El valor de Ds_Merchant_Conciliation no ocupa 6 posiciones"},
            {BSRedsysErrorCode.SIS0089, "El valor de Ds_Merchant_ExpiryDate no ocupa 4 posiciones"},
            {BSRedsysErrorCode.SIS0092, "El valor de Ds_Merchant_ExpiryDate es nulo"},
            {BSRedsysErrorCode.SIS0093, "Tarjeta no encontrada en la tabla de rangos"},
            {BSRedsysErrorCode.SIS0094, "La tarjeta no fue autenticada como 3D Secure"},
            {BSRedsysErrorCode.SIS0097, "Valor del campo Ds_Merchant_CComercio no válido"},
            {BSRedsysErrorCode.SIS0098, "Valor del campo Ds_Merchant_CVentana no válido"},
            {BSRedsysErrorCode.SIS0112, "Error El tipo de transacción especificado en"},
            {BSRedsysErrorCode.SIS0113, "Excepción producida en el servlet de operaciones"},
            {BSRedsysErrorCode.SIS0114, "Error, se ha llamado con un GET en lugar de un POST"},
            {BSRedsysErrorCode.SIS0115, "Error no existe operación sobre la que realizar el pago de la cuota"},
            {BSRedsysErrorCode.SIS0116, "La operación sobre la que se desea pagar una cuota no es una operación válida"},
            {BSRedsysErrorCode.SIS0117, "La operación sobre la que se desea pagar una cuota no está autorizada"},
            {BSRedsysErrorCode.SIS0118, "Se ha excedido el importe total de las cuotas"},
            {BSRedsysErrorCode.SIS0119, "Valor del campo Ds_Merchant_DateFrecuency no válido"},
            {BSRedsysErrorCode.SIS0120, "Valor del campo Ds_Merchant_ChargeExpiryDate no válido"},
            {BSRedsysErrorCode.SIS0121, "Valor del campo Ds_Merchant_SumTotal no válido"},
            {BSRedsysErrorCode.SIS0122, "Valor del campo Ds_Merchant_DateFrecuency o no Ds_Merchant_SumTotal tiene formato incorrecto"},
            {BSRedsysErrorCode.SIS0123, "Se ha excedido la fecha tope para realizar transacciones"},
            {BSRedsysErrorCode.SIS0124, "No ha transcurrido la frecuencia mínima en un pago recurrente sucesivo"},
            {BSRedsysErrorCode.SIS0126, "Operación denegada para evitar duplicidades."},
            {BSRedsysErrorCode.SIS0132, "La fecha de Confirmación de Autorización no puede superar en mas de 7 días a la de Preautorización."},
            {BSRedsysErrorCode.SIS0133, "La fecha de Confirmación de Autenticación no puede superar en mas de 45 días a la de Autenticación Previa."},
            {BSRedsysErrorCode.SIS0139, "Error el pago recurrente inicial está duplicado"},
            {BSRedsysErrorCode.SIS0142, "Tiempo excedido para el pago"},
            {BSRedsysErrorCode.SIS0197, "Error al obtener los datos de cesta de la compra en operación tipo pasarela"},
            {BSRedsysErrorCode.SIS0198, "Error el importe supera el límite permitido para el comercio"},
            {BSRedsysErrorCode.SIS0199, "Error el número de operaciones supera el límite permitido para el comercio"},
            {BSRedsysErrorCode.SIS0200, "Error el importe acumulado supera el límite permitido para el comercio"},
            {BSRedsysErrorCode.SIS0214, "El comercio no admite devoluciones"},
            {BSRedsysErrorCode.SIS0216, "Error Ds_Merchant_CVV2 tiene más de 3 posiciones"},
            {BSRedsysErrorCode.SIS0217, "Error de formato en Ds_Merchant_CVV2"},
            {BSRedsysErrorCode.SIS0218, "El comercio no permite operaciones seguras por la entrada /operaciones"},
            {BSRedsysErrorCode.SIS0219, "Error el número de operaciones de la tarjeta supera el límite permitido para el comercio"},
            {BSRedsysErrorCode.SIS0220, "Error el importe acumulado de la tarjeta supera el límite permitido para el comercio"},
            {BSRedsysErrorCode.SIS0221, "Error el CVV2 es obligatorio"},
            {BSRedsysErrorCode.SIS0222, "Ya existe una anulación asociada a la preautorización"},
            {BSRedsysErrorCode.SIS0223, "La preautorización que se desea anular no está autorizada"},
            {BSRedsysErrorCode.SIS0224, "El comercio no permite anulaciones por no tener firma ampliada"},
            {BSRedsysErrorCode.SIS0225, "Error no existe operación sobre la que realizar la anulación"},
            {BSRedsysErrorCode.SIS0226, "Inconsistencia de datos, en la validación de una anulación"},
            {BSRedsysErrorCode.SIS0227, "Valor del campo Ds_Merchant_TransactionDate no válido"},
            {BSRedsysErrorCode.SIS0229, "No existe el código de pago aplazado solicitado"},
            {BSRedsysErrorCode.SIS0230, "El comercio no permite pago fraccionado"},
            {BSRedsysErrorCode.SIS0231, "No hay forma de pago aplicable para el cliente"},
            {BSRedsysErrorCode.SIS0252, "El comercio no permite el envío de tarjeta"},
            {BSRedsysErrorCode.SIS0253, "La tarjeta no cumple el check-digit"},
            {BSRedsysErrorCode.SIS0254, "El número de operaciones de la IP supera el límite permitido por el comercio"},
            {BSRedsysErrorCode.SIS0255, "El importe acumulado por la IP supera el límite permitido por el comercio"},
            {BSRedsysErrorCode.SIS0256, "El comercio no puede realizar preautorizaciones"},
            {BSRedsysErrorCode.SIS0257, "Esta tarjeta no permite operativa de preautorizaciones"},
            {BSRedsysErrorCode.SIS0258, "Inconsistencia de datos, en la validación de una confirmación"},
            {BSRedsysErrorCode.SIS0261, "Operación detenida por superar el control de restricciones en la entrada al SIS"},
            {BSRedsysErrorCode.SIS0270, "El comercio no puede realizar autorizaciones en diferido"},
            {BSRedsysErrorCode.SIS0274, "Tipo de operación desconocida o no permitida por esta entrada al SIS"},
            {BSRedsysErrorCode.SIS0295, "Se ha denegado una operación que fue enviada en el mismo minuto para evitar duplic."},
            {BSRedsysErrorCode.SIS0296, "Error al validar los datos de la operación de Tarjeta en Archivo Inicial."},
            {BSRedsysErrorCode.SIS0297, "Número de operaciones sucesivas de tarjeta en archivo superado."},
            {BSRedsysErrorCode.SIS0298, "El comercio no permite realizar operaciones de Tarjeta en Archivo o Pago por referencia."},
            {BSRedsysErrorCode.SIS0319, "El comercio no pertenece al grupo especificado en Ds_Merchant_Group"},
            {BSRedsysErrorCode.SIS0321, "La referencia indicada en Ds_Merchant_Identifier no está asociada al comercio"},
            {BSRedsysErrorCode.SIS0322, "Error de formato en Ds_Merchant_Group"},
            {BSRedsysErrorCode.SIS0323, "Faltan parámetros CustomerMobile y CustomerMail"},
            {BSRedsysErrorCode.SIS0324, "Imposible enviar enlace al titular"},
            {BSRedsysErrorCode.SIS0325, "Se ha pedido no mostrar pantallas pero no se ha enviado ninguna referencia de tarjeta"},
            {BSRedsysErrorCode.SIS0327, "No se ha indicado teléfono o email en la petición Phone & Sell"},
            {BSRedsysErrorCode.SIS0330, "El enlace para el pago ha caducado."},
            {BSRedsysErrorCode.SIS0331, "La operación no tiene un estado válido o no existe."},
            {BSRedsysErrorCode.SIS0333, "No está configurado el wallet solicitado (V.Me, Master)"},
            {BSRedsysErrorCode.SIS0334, "Operación detenida por superar el control de restricciones de seguridad del TPV Virtual"},
            {BSRedsysErrorCode.SIS0429, "Error en la versión enviada por el comercio en el parámetro Ds_SignatureVersion"},
            {BSRedsysErrorCode.SIS0430, "Error al decodificar el parámetro Ds_MerchantParameters"},
            {BSRedsysErrorCode.SIS0431, "Error del objeto JSON que se envía codificado en el parámetro Ds_MerchantParameters"},
            {BSRedsysErrorCode.SIS0432, "Error FUC del comercio erróneo"},
            {BSRedsysErrorCode.SIS0433, "Error Terminal del comercio erróneo"},
            {BSRedsysErrorCode.SIS0434, "Error ausencia de número de pedido en la operación enviada por el comercio"},
            {BSRedsysErrorCode.SIS0435, "Error en el cálculo de la firma"},
            {BSRedsysErrorCode.SIS0436, "Error en la construcción del elemento padre <REQUEST>"},
            {BSRedsysErrorCode.SIS0437, "Error en la construcción del elemento <DS_SIGNATUREVERSION>"},
            {BSRedsysErrorCode.SIS0438, "Error en la construcción del elemento <DATOSENTRADA>"},
            {BSRedsysErrorCode.SIS0439, "Error en la construcción del elemento <DS_SIGNATURE>"},
            {BSRedsysErrorCode.SIS0444, "Este error se produce cuando el comercio ya ha migrado a la firma HMAC SHA256 y envía una transacción con la firma antigua"},
            {BSRedsysErrorCode.SIS0448, "Se ha realizado una operación con tarjeta DINERS, pero el comercio no tiene habilitado este tipo de tarjeta. Para habilitarla, deberá contactar directamente con Diners Club."},
            {BSRedsysErrorCode.SIS0449, "Se ha enviado el tipo de transacción “A” y el comercio no tiene activado la operatividad con este tipo de transacción."},
            {BSRedsysErrorCode.SIS0450, "Se ha enviado el tipo de transacción “A” con una tarjeta American Express y el comercio no tiene activado la operatividad con este tipo de transacción."},
            {BSRedsysErrorCode.SIS0451, "Se ha enviado el tipo de transacción “A” y el comercio no tiene activado la operatividad con este tipo de transacción."},
            {BSRedsysErrorCode.SIS0452, "Se ha utilizado una tarjeta 4B y el comercio no admite este tipo de tarjeta."},
            {BSRedsysErrorCode.SIS0453, "Se ha utilizado una tarjeta JCB y el comercio no admite este tipo de tarjeta."},
            {BSRedsysErrorCode.SIS0454, "Se ha utilizado una tarjeta American Express y el comercio no admite este tipo de tarjeta."},
            {BSRedsysErrorCode.SIS0455, "Método de pago no disponible"},
            {BSRedsysErrorCode.SIS0456, "Método de pago no seguro (Visa) no disponible"},
            {BSRedsysErrorCode.SIS0457, "Se ha utilizado una tarjeta comercial y el comercio no admite este tipo de tarjeta. Para habilitarlo, deberá contactar con su oficina gestora."},
            {BSRedsysErrorCode.SIS0458, "Se ha utilizado una tarjeta comercial y el comercio no admite este tipo de tarjeta. Para habilitarlo, deberá contactar con su oficina gestora."},
            {BSRedsysErrorCode.SIS0459, "Se ha utilizado una tarjeta JCB y el comercio no admite este tipo de tarjeta."},
            {BSRedsysErrorCode.SIS0460, "Se ha utilizado una tarjeta American Express y el comercio no admite este tipo de tarjeta."},
            {BSRedsysErrorCode.SIS0461, "Se ha utilizado una tarjeta American Express y el comercio no admite este tipo de tarjeta."},
            {BSRedsysErrorCode.SIS0462, "Error, se ha enviado una petición segura a través de Host to Host."},
            {BSRedsysErrorCode.SIS0463, "Método de pago no disponible"},
            {BSRedsysErrorCode.SIS0464, "Se ha utilizado una tarjeta comercial y el comercio no admite este tipo de tarjeta. Para habilitarlo, deberá contactar con su oficina gestora."},
            {BSRedsysErrorCode.SIS0465, "Se ha lanzado una petición de pago no segura y el comercio no admite pagos no seguros."},

            {BSRedsysErrorCode._001, "TRANSACCION APROBADA PREVIA IDENTIFICACION DE TITULAR"},
            {BSRedsysErrorCode._002, "TRANSACCION APROBADA"},

            {BSRedsysErrorCode._101, "TARJETA CADUCADA "},
            {BSRedsysErrorCode._102, "TARJETA BLOQUEDA TRANSITORIAMENTE O BAJO SOSPECHA DE FRAUDE"}, 
            {BSRedsysErrorCode._104, "OPERACIÓN NO PERMITIDA"},
            {BSRedsysErrorCode._106, "NUM. INTENTOS EXCEDIDO"},
            {BSRedsysErrorCode._107, "CONTACTAR CON EL EMISOR"},
            {BSRedsysErrorCode._109, "IDENTIFICACIÓN INVALIDA DEL COMERCIO O TERMINAL"},
            {BSRedsysErrorCode._110, "IMPORTE INVALIDO"},
            //{BSRedsysErrorCode._114, "TARJETA NO SOPORTA EL TIPO DE OPERACIÓN SOLICITADO"},
            //{BSRedsysErrorCode._116, "DISPONIBLE INSUFICIENTE"},
            //{BSRedsysErrorCode._118, "TARJETA NO REGISTRADA"},
            {BSRedsysErrorCode._125, "TARJETA NO EFECTIVA"},
            {BSRedsysErrorCode._129, "ERROR CVV2/CVC2"},
            {BSRedsysErrorCode._167, "CONTACTAR CON EL EMISOR: SOSPECHA DE FRAUDE"},
            {BSRedsysErrorCode._180, "TARJETA AJENA AL SERVICIO"},
            {BSRedsysErrorCode._181, "TARJETA CON RESTRICCIONES DE DEBITO O CREDITO"},
            {BSRedsysErrorCode._182, "TARJETA CON RESTRICCIONES DE DEBITO O CREDITO"},

            {BSRedsysErrorCode._184, "ERROR EN AUTENTICACION"},
            {BSRedsysErrorCode._190, "DENEGACION SIN ESPECIFICAR EL MOTIVO"},
            {BSRedsysErrorCode._191, "FECHA DE CADUCIDAD ERRONEA"},
            {BSRedsysErrorCode._201, "TARJETA CADUCADA"},
            {BSRedsysErrorCode._202, "TARJETA BLOQUEDA TRANSITORIAMENTE O BAJO SOSPECHA DE FRAUDE"},
            {BSRedsysErrorCode._204, "OPERACION NO PERMITIDA"},
            {BSRedsysErrorCode._207, "CONTACTAR CON EL EMISOR"},
            {BSRedsysErrorCode._208, "TARJETA PERDIDA O ROBADA"},
            {BSRedsysErrorCode._209, "TARJETA PERDIDA O ROBADA"},
            {BSRedsysErrorCode._280, "ERROR CVV2/CVC2"},
            {BSRedsysErrorCode._290, "DENEGACION SIN ESPECIFICAR EL MOTIVO"},
            {BSRedsysErrorCode._400, "ANULACION ACEPTADA"},
            {BSRedsysErrorCode._480, "NO SE ENCUENTRA LA OPERACIÓN ORIGINAL O TIME-OUT EXCEDIDO"},
            {BSRedsysErrorCode._481, "ANULACION ACEPTADA"},
            {BSRedsysErrorCode._500, "CONCILIACION ACEPTADA"},
            {BSRedsysErrorCode._501, "NO ENCONTRADA LA OPERACION ORIGINAL O TIME-OUT EXCEDIDO"},
            {BSRedsysErrorCode._502, "NO ENCONTRADA LA OPERACION ORIGINAL O TIME-OUT EXCEDIDO"},
            {BSRedsysErrorCode._503, "NO ENCONTRADA LA OPERACION ORIGINAL O TIME-OUT EXCEDIDO"},
            {BSRedsysErrorCode._9928, "ANULACIÓN DE PREAUTORITZACIÓN REALIZADA POR EL SISTEMA"},
            {BSRedsysErrorCode._9929, "ANULACIÓN DE PREAUTORITZACIÓN REALIZADA POR EL COMERCIO"},
            {BSRedsysErrorCode._904, "COMERCIO NO REGISTRADO EN EL FUC"},
            {BSRedsysErrorCode._909, "ERROR DE SISTEMA"},
            {BSRedsysErrorCode._912, "EMISOR NO DISPONIBLE"},
            {BSRedsysErrorCode._913, "TRANSMISION DUPLICADA"},
            {BSRedsysErrorCode._916, "IMPORTE DEMASIADO PEQUEÑO"},
            {BSRedsysErrorCode._928, "TIME-OUT EXCEDIDO"},
            {BSRedsysErrorCode._940, "TRANSACCION ANULADA ANTERIORMENTE"},
            {BSRedsysErrorCode._941, "TRANSACCION DE AUTORIZACION YA ANULADA POR UNA ANULACION ANTERIOR"},
            {BSRedsysErrorCode._942, "TRANSACCION DE AUTORIZACION ORIGINAL DENEGADA"},
            {BSRedsysErrorCode._943, "DATOS DE LA TRANSACCION ORIGINAL DISTINTOS"},
            {BSRedsysErrorCode._944, "SESION ERRONEA"},
            {BSRedsysErrorCode._945, "TRANSMISION DUPLICADA"},
            {BSRedsysErrorCode._946, "OPERACION A ANULAR EN PROCESO"},
            {BSRedsysErrorCode._947, "TRANSMISION DUPLICADA EN PROCESO"},
            {BSRedsysErrorCode._949, "TERMINAL INOPERATIVO"},
            {BSRedsysErrorCode._950, "DEVOLUCION NO PERMITIDA"},
            {BSRedsysErrorCode._965, "VIOLACIÓN NORMATIVA"},
            {BSRedsysErrorCode._9064, "LONGITUD TARJETA INCORRECTA"},
            {BSRedsysErrorCode._9078, "NO EXISTE METODO DE PAGO"},
            {BSRedsysErrorCode._9093, "TARJETA NO EXISTE"},
            {BSRedsysErrorCode._9094, "DENEGACION DE LOS EMISORES"},
            {BSRedsysErrorCode._9104, "OPER. SEGURA NO ES POSIBLE"},
            {BSRedsysErrorCode._9126, "OPERACIÓN DENEGADA PARA EVITAR DUPLICIDADES"},
            {BSRedsysErrorCode._9142, "TIEMPO LÍMITE DE PAGO SUPERADO"},
            {BSRedsysErrorCode._9218, "NO SE PUEDEN HACER OPERACIONES SEGURAS"},
            {BSRedsysErrorCode._9253, "CHECK-DIGIT ERRONEO"},
            {BSRedsysErrorCode._9256, "PREAUTORITZACIONES NO HABILITADAS"},
            {BSRedsysErrorCode._9261, "LÍMITE OPERATIVO EXCEDIDO"},
            {BSRedsysErrorCode._9283, "SUPERA ALERTAS BLOQUANTES"},
            {BSRedsysErrorCode._9281, "SUPERA ALERTAS BLOQUEANTES"},
            {BSRedsysErrorCode._9334, "DENEGACIÓN POR FILTROS DE SEGURIDAD"},
            {BSRedsysErrorCode._9912, "EMISOR NO DISPONIBLE"},
            {BSRedsysErrorCode._9913, "ERROR EN CONFIRMACION"},
            {BSRedsysErrorCode._9914, "CONFIRMACION “KO”"},
            {BSRedsysErrorCode._9915, "PAGO CANCELADO"},
            //{BSRedsysErrorCode._9928, "AUTORIZACIÓN EN DIFERIDO ANULADA"},
            //{BSRedsysErrorCode._9929, "AUTORIZACIÓN EN DIFERIDO ANULADA"},
            {BSRedsysErrorCode._9997, "TRANSACCIÓN SIMULTÁNEA"},
            {BSRedsysErrorCode._9998, "ESTADO OPERACIÓN: SOLICITADA"},
            {BSRedsysErrorCode._9999, "ESTADO OPERACIÓN: AUTENTICANDO"},

            {BSRedsysErrorCode.ERROR, "Error indefinido."}
        };

        public bool RefundEnabled
        {
            get { return true; }
        }

        public bool PartialRefundEnabled
        {
            get { return true; }
        }

        public static string OrderId()
        {
            DateTime dtUtcNow = DateTime.UtcNow;
            return string.Format("{0:yy}{1:000}{2:00000}{3:00}", dtUtcNow, dtUtcNow.DayOfYear, dtUtcNow.TimeOfDay.TotalSeconds, m_oRandom.Next(0, 99));
        }

        public static BSRedsysErrorCode GetErrorInfo(string sErrorCode, out string sErrorMessage)
        {
            BSRedsysErrorCode eRet = BSRedsysErrorCode.ERROR;
            
            try
            {
                if (string.IsNullOrEmpty(sErrorCode))
                    sErrorCode = "-1";

                var oValues = (int[])Enum.GetValues(typeof(BSRedsysErrorCode));
                if (sErrorCode.StartsWith("SIS0"))
                    sErrorCode = sErrorCode.Substring(4);
                if (oValues.Contains(Convert.ToInt32(sErrorCode)))
                {
                    eRet = (BSRedsysErrorCode)Convert.ToInt32(sErrorCode);
                }
            }
            catch
            {
                eRet = BSRedsysErrorCode.ERROR;
            }

            if (ErrorMessageDict.ContainsKey(eRet))
                sErrorMessage = ErrorMessageDict[eRet];
            else
                sErrorMessage = ErrorMessageDict[BSRedsysErrorCode.ERROR];

            return eRet;
        }

        public static bool IsError(BSRedsysErrorCode eErrorCode)
        {
            return (eErrorCode != BSRedsysErrorCode.SIS0000 &&
                    eErrorCode != BSRedsysErrorCode._001 && 
                    eErrorCode != BSRedsysErrorCode._002);
        }

        public bool StandardPaymentNO3DS(string sWsUrl, int iMerchantCode, string sMerchantSignature, string sMerchantTerminal, int iServiceTimeout,
                                    string sCardReference, int iQuantity, string sCurNumISOCode, string sMerchantGroup, string strCOFTxnID,
                                    out BSRedsysErrorCode eErrorCode, out string sErrorMessage, out string sTransactionId, out string sOpReference, out string sDateTime)
        {
            bool bRet = false;
            eErrorCode = BSRedsysErrorCode.ERROR;
            sErrorMessage = "";
            sTransactionId = "";
            sOpReference = "";
            sDateTime = "";


            AddTLS12Support();

            try
            {
                BSRedsysWs.SerClsWSEntradaService oWs = new BSRedsysWs.SerClsWSEntradaService();

                oWs.Url = sWsUrl;
                oWs.Timeout = iServiceTimeout;

                RedsysAPIWs oAPIWs = new RedsysAPIWs();

                string sTransactionType = "0";
                sOpReference = OrderId();

                if (strCOFTxnID == sCardReference)
                {
                    strCOFTxnID = UNKNOWN_COF_TXT_INI;
                }

                string sInputData = oAPIWs.GenerateDatoEntradaXMLTrataPeticionNO3DS(iQuantity.ToString(), iMerchantCode.ToString(), sCurNumISOCode, sCardReference, sTransactionType, sMerchantTerminal, sOpReference, null, sMerchantGroup, strCOFTxnID, "MIT");

                string sSignature = oAPIWs.createMerchantSignatureHostToHost(sMerchantSignature, sInputData);

                string sRequestXML = oAPIWs.GenerateRequestXML(sInputData, sSignature);

                Logger_AddLogMessage(string.Format("StandardPaymentNO3DS request.url={0}, request.xml={1}", sWsUrl, PrettyXml(sRequestXML)), LogLevels.logINFO);

                string sResult = oWs.trataPeticion(sRequestXML);

                Logger_AddLogMessage(string.Format("StandardPaymentNO3DS response.xml={0}", PrettyXml(sResult)), LogLevels.logINFO);

                oAPIWs.XMLToDiccionary(sResult);

                string sCode = oAPIWs.GetDictionary("CODIGO");
                eErrorCode = GetErrorInfo(sCode, out sErrorMessage);
                if (eErrorCode == BSRedsysErrorCode.SIS0000)
                {
                    string sCadena = oAPIWs.GenerateCadena(sResult);
                    string sSignatureReceived = oAPIWs.GetDictionary("Ds_Signature");

                    string sNumOrder = oAPIWs.GetDictionary("Ds_Order");
                    string sResponseCode = oAPIWs.GetDictionary("Ds_Response");

                    if (!string.IsNullOrEmpty(sResponseCode))
                    {
                        try
                        {
                            int iResponseCode = Convert.ToInt32(sResponseCode);

                            if ((iResponseCode >= 0) && (iResponseCode < 100))
                            {

                                string sSignatureCalculate = oAPIWs.createSignatureResponseHostToHost(sMerchantSignature, sCadena, sNumOrder);

                                // Check if received signature is equal to calculated signature
                                if (sSignatureCalculate != sSignatureReceived)
                                    Logger_AddLogMessage(string.Format("StandardPayment Received signature is not equal to calculated signature"), LogLevels.logERROR);

                                sTransactionId = oAPIWs.GetDictionary("Ds_AuthorisationCode");
                                sOpReference = oAPIWs.GetDictionary("Ds_Order");
                                sDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                                /*string Rcodigo = "CODIGO";
                                string Ramount = "Ds_Amount";
                                string RCurrency = "Ds_Currency";
                                string ROrder = "Ds_Order";
                                string RSignature = "Ds_Signature";
                                string RMerchantCode = "Ds_MerchantCode";
                                string RTerminal = "Ds_Terminal";
                                string RResponse = "Ds_Response";
                                string RAuthoCode = "Ds_AuthorisationCode";
                                string RTransType = "Ds_TransactionType";
                                string RSecure = "Ds_SecurePayment";
                                string RLanguage = "Ds_Language";
                                string RMerchantData = "Ds_MerchantData";
                                string RCountry = "Ds_Card_Country";*/
                                bRet = true;

                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("StandardPaymentNO3DS::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                bRet = false;

                            }

                        }
                        catch
                        {
                            Logger_AddLogMessage(string.Format("StandardPaymentNO3DS::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                            bRet = false;

                        }

                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("StandardPaymentNO3DS::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                        bRet = false;

                    }


                    

                }
                else
                {
                    Logger_AddLogMessage(string.Format("StandardPaymentNO3DS::Response Error (Code={0}({1}), Message='{1}')", sCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                    bRet = false;
                }

            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "StandardPaymentNO3DS::Exception", LogLevels.logERROR);
                bRet = false;
            }

            return bRet;
        }





        public bool StandardPayment3DSStep1(string sWsUrl, int iMerchantCode, string sMerchantSignature, string sMerchantTerminal, int iServiceTimeout,
                                            string sCardReference, int iQuantity, string sCurNumISOCode, string sMerchantGroup,string strCOFTxnID,
                                            string strReturnURL,
                                            out BSRedsysErrorCode eErrorCode, out string sErrorMessage,  out string sOpReference, out string sTransactionId, out string sDateTime, 
                                            out string strInlineForm,out string strMD, out string strPaReq, out string strCreq, out string strProtocolVersion, out string strthreeDSServerTransID)
        {
            bool bRet = false;
            eErrorCode = BSRedsysErrorCode.ERROR;
            sErrorMessage = "";
            sTransactionId = "";
            sOpReference = "";
            sDateTime = "";
            strInlineForm ="";
            strMD="";
            strPaReq = "";
            strCreq = "";
            strProtocolVersion = "";
            strthreeDSServerTransID = "";


            AddTLS12Support();

            try
            {
                BSRedsysWs.SerClsWSEntradaService oWs = new BSRedsysWs.SerClsWSEntradaService();

                oWs.Url = sWsUrl;
                oWs.Timeout = iServiceTimeout;

                RedsysAPIWs oAPIWs = new RedsysAPIWs();

                string sTransactionType = "0";
                sOpReference = OrderId();

                if (strCOFTxnID == sCardReference)
                {
                    strCOFTxnID = UNKNOWN_COF_TXT_INI;
                }

                string sInputData = oAPIWs.GenerateDatoEntradaXML3DS(iQuantity.ToString(), iMerchantCode.ToString(), sCurNumISOCode, sCardReference, sTransactionType, sMerchantTerminal, sOpReference, null, sMerchantGroup, strCOFTxnID, "{'threeDSInfo':'CardData'}");

                string sSignature = oAPIWs.createMerchantSignatureHostToHost(sMerchantSignature, sInputData);

                string sRequestXML = oAPIWs.GenerateRequestXML(sInputData, sSignature);

                Logger_AddLogMessage(string.Format("StandardPayment3DSStep1 request.url={0}, request.xml={1}", sWsUrl, PrettyXml(sRequestXML)), LogLevels.logINFO);

                string sResult = oWs.iniciaPeticion(sRequestXML);

                Logger_AddLogMessage(string.Format("StandardPayment3DSStep1 response.xml={0}", PrettyXml(sResult)), LogLevels.logINFO);

                oAPIWs.XMLToDiccionary(sResult);

                string sCode = oAPIWs.GetDictionary("CODIGO");
                eErrorCode = GetErrorInfo(sCode, out sErrorMessage);
                if (eErrorCode == BSRedsysErrorCode.SIS0000)
                {
                    string sCadena = oAPIWs.GenerateCadena(sResult);
                    string sSignatureReceived = oAPIWs.GetDictionary("Ds_Signature");

                    string sNumOrder = oAPIWs.GetDictionary("Ds_Order");

                    try
                    {


                        string sSignatureCalculate = oAPIWs.createSignatureResponseHostToHost(sMerchantSignature, sCadena, sNumOrder);

                        // Check if received signature is equal to calculated signature
                        if (sSignatureCalculate != sSignatureReceived)
                            Logger_AddLogMessage(string.Format("StandardPayment3DSStep1 Received signature is not equal to calculated signature"), LogLevels.logERROR);

                        string sResponseCode = oAPIWs.GetDictionary("Ds_Response");
                        string Ds_EMV3DS = oAPIWs.GetDictionary("Ds_EMV3DS");
                        if (!string.IsNullOrEmpty(Ds_EMV3DS))
                        {

                            string protocolVersion = null;
                            string threeDSServerTransID = null;
                            string threeDSInfo = null;
                            string threeDSMethodURL = null;


                            dynamic oDSEMV3DS = JsonConvert.DeserializeObject(Ds_EMV3DS);

                            try
                            {
                                protocolVersion = oDSEMV3DS["protocolVersion"];
                            }
                            catch { }
                            try
                            {
                                threeDSServerTransID = oDSEMV3DS["threeDSServerTransID"];
                            }
                            catch { }
                            try
                            {
                                threeDSInfo = oDSEMV3DS["threeDSInfo"];
                            }
                            catch { }
                            try
                            {
                                threeDSMethodURL = oDSEMV3DS["threeDSMethodURL"];
                            }
                            catch { }


                            if (string.IsNullOrEmpty(threeDSMethodURL))
                            {

                                //PARECE QUE ESTO SOLO OCURRE CON TARJETAS 1.0.2 (NO_3DS_V2) PERO NO TENGO FORMA DE COMPROBARLO.
                                protocolVersion = (protocolVersion == "NO_3DS_v2") ? "1.0.2" : protocolVersion;

                                if (protocolVersion != "1.0.2")
                                {
                                    strthreeDSServerTransID = threeDSServerTransID;
                                }
                                
                                bRet = StandardPayment3DSStep2(sWsUrl, iMerchantCode, sMerchantSignature, sMerchantTerminal, iServiceTimeout,
                                                                sCardReference, iQuantity, sCurNumISOCode, sMerchantGroup, strCOFTxnID, sOpReference,
                                                                strReturnURL, protocolVersion, strthreeDSServerTransID, "N",
                                                                out  eErrorCode, out  sErrorMessage, out sTransactionId, out sDateTime,
                                                                out  strInlineForm, out  strMD, out strPaReq, out strCreq);

                            }
                            else
                            {
                                string strJSON = string.Format("{{\"threeDSServerTransID\":\"{0}\"," +
                                                               "\"threeDSMethodNotificationURL\":\"{1}\"}}",
                                                               threeDSServerTransID, strReturnURL);

                                string strJSONBase64 = Base64Encode(strJSON);
                                Dictionary<string, string> postData = new Dictionary<string, string>();
                                postData.Add("threeDSMethodData", strJSONBase64);
                                strInlineForm = RedirectFormWithData(threeDSMethodURL, postData);

                                strthreeDSServerTransID = threeDSServerTransID;
                                Logger_AddLogMessage(string.Format("StandardPayment3DSStep1 threeDSMethodURL not NULL strJSON={0} \r\nInlineForm={1}", strJSON, strInlineForm), LogLevels.logINFO);

                                bRet = true;



                            }
                            strProtocolVersion = protocolVersion;
                        }
                        else if (!string.IsNullOrEmpty(sResponseCode))
                        {
                            try
                            {
                                int iResponseCode = Convert.ToInt32(sResponseCode);

                                if ((iResponseCode >= 0) && (iResponseCode < 100))
                                {
                                    sTransactionId = oAPIWs.GetDictionary("Ds_AuthorisationCode");
                                    sDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                                    bRet = true;

                                }
                                else
                                {
                                    Logger_AddLogMessage(string.Format("StandardPayment3DSStep1::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                    bRet = false;

                                }

                            }
                            catch
                            {
                                Logger_AddLogMessage(string.Format("StandardPayment3DSStep1::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                bRet = false;

                            }

                        }
                        else
                        {
                            Logger_AddLogMessage(string.Format("StandardPayment3DSStep1::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                            bRet = false;

                        }                        

                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e,string.Format("StandardPayment3DSStep1::Exception Error (Code={0}, Message='{1}')", eErrorCode, sErrorMessage), LogLevels.logERROR);
                        bRet = false;

                    }

                }
                else
                {
                    Logger_AddLogMessage(string.Format("StandardPayment3DSStep1::Response Error (Code={0}, Message='{1}')", eErrorCode, sErrorMessage), LogLevels.logERROR);
                    bRet = false;

                }

            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "StandardPayment3DSStep1::Exception", LogLevels.logERROR);
                bRet = false;
            }

            return bRet;
        }



        public bool StandardPayment3DSStep2(string sWsUrl, int iMerchantCode, string sMerchantSignature, string sMerchantTerminal, int iServiceTimeout,
                                    string sCardReference, int iQuantity, string sCurNumISOCode, string sMerchantGroup,string strCOFTxnID, string sOpReference,
                                    string strReturnURL,string strProtocolVersion, string strthreeDSServerTransID, string strThreeDSCompInd,
                                    out BSRedsysErrorCode eErrorCode, out string sErrorMessage,out string sTransactionId,out string sDateTime,
                                    out string strInlineForm,out string strMD,out string strPaReq, out string strCreq)
        {
            bool bRet = false;
            eErrorCode = BSRedsysErrorCode.ERROR;
            sErrorMessage = "";
            sTransactionId = "";
            sDateTime = "";
            strInlineForm="";
            strMD = "";
            strPaReq = "";
            strCreq ="";


            AddTLS12Support();

            try
            {
                BSRedsysWs.SerClsWSEntradaService oWs = new BSRedsysWs.SerClsWSEntradaService();

                oWs.Url = sWsUrl;
                oWs.Timeout = iServiceTimeout;

                RedsysAPIWs oAPIWs = new RedsysAPIWs();

                string sTransactionType = "0";
                string s3DSParameters = "";

                  if (strCOFTxnID == sCardReference)
                {
                    strCOFTxnID = UNKNOWN_COF_TXT_INI;
                }


                if (strProtocolVersion == "1.0.2")
                {
                    s3DSParameters = string.Format("{{\"threeDSInfo\":\"AuthenticationData\"," +
                                                    "\"protocolVersion\":\"{0}\"," +
                                                    "\"threeDSCompInd\":\"{1}\"," +
                                                    "\"browserAcceptHeader\":\"text/html,application/xhtml+xml,application/xml;q=0.9," +
                                                    "*/*;q=0.8,application/json\"," +
                                                    "\"browserUserAgent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36" +
                                                    "(KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36\"}}",
                                                    strProtocolVersion,
                                                    strThreeDSCompInd);


                }
                else
                {
                    s3DSParameters = string.Format("{{\"notificationURL\":\"{0}\"," +
                                                    "\"threeDSInfo\":\"AuthenticationData\"," +
                                                    "\"browserLanguage\":\"ES-es\"," +
                                                    "\"browserColorDepth\":\"24\"," +
                                                    "\"browserJavaEnabled\":\"false\"," +
                                                    "\"browserJavascriptEnabled\":\"false\"," +
                                                    "\"browserAcceptHeader\":\"text/html,application/xhtml+xml,application/xml;q=0.9," +
                                                    "*/*;q=0.8,application/json\"," +
                                                    "\"browserUserAgent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                                                    "(KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36\"," +
                                                    "\"browserTZ\":\"52\"," +
                                                    "\"browserScreenHeight\":\"1250\"," +
                                                    "\"protocolVersion\":\"{1}\"," +
                                                    "\"threeDSCompInd\":\"{2}\"," +
                                                    "\"browserScreenWidth\":\"1320\"," +
                                                    "\"threeDSServerTransID\":\"{3}\"}}",
                                                    strReturnURL,
                                                    strProtocolVersion,
                                                    strThreeDSCompInd,
                                                    strthreeDSServerTransID);



                }

                string sInputData = oAPIWs.GenerateDatoEntradaXML3DS(iQuantity.ToString(), iMerchantCode.ToString(), sCurNumISOCode, sCardReference, sTransactionType, sMerchantTerminal, sOpReference, null, sMerchantGroup, strCOFTxnID, s3DSParameters);

                string sSignature = oAPIWs.createMerchantSignatureHostToHost(sMerchantSignature, sInputData);

                string sRequestXML = oAPIWs.GenerateRequestXML(sInputData, sSignature);

                Logger_AddLogMessage(string.Format("StandardPayment3DSStep2 request.url={0}, request.xml={1}", sWsUrl, PrettyXml(sRequestXML)), LogLevels.logINFO);

                string sResult = oWs.trataPeticion(sRequestXML);

                Logger_AddLogMessage(string.Format("StandardPayment3DSStep2 response.xml={0}", PrettyXml(sResult)), LogLevels.logINFO);

                oAPIWs.XMLToDiccionary(sResult);

                string sCode = oAPIWs.GetDictionary("CODIGO");
                eErrorCode = GetErrorInfo(sCode, out sErrorMessage);
                if (eErrorCode == BSRedsysErrorCode.SIS0000)
                {
                    string sCadena = oAPIWs.GenerateCadena(sResult);
                    string sSignatureReceived = oAPIWs.GetDictionary("Ds_Signature");

                    string sNumOrder = oAPIWs.GetDictionary("Ds_Order");
                    try
                    {

                        string sSignatureCalculate = oAPIWs.createSignatureResponseHostToHost(sMerchantSignature, sCadena, sNumOrder);

                        // Check if received signature is equal to calculated signature
                        if (sSignatureCalculate != sSignatureReceived)
                            Logger_AddLogMessage(string.Format("StandardPayment3DSStep2 Received signature is not equal to calculated signature"), LogLevels.logERROR);


                        string sResponseCode = oAPIWs.GetDictionary("Ds_Response");
                        string Ds_EMV3DS = oAPIWs.GetDictionary("Ds_EMV3DS");
                        if (!string.IsNullOrEmpty(Ds_EMV3DS))
                        {

                            string threeDSInfo = null;
                            string acsURL = null;
                            string PAReq = null;
                            string MD = null;
                            string creq = null;
                            string protocolVersion=null;


                            dynamic oDSEMV3DS = JsonConvert.DeserializeObject(Ds_EMV3DS);

                            try
                            {
                                threeDSInfo = oDSEMV3DS["threeDSInfo"];
                            }
                            catch { }
                            try
                            {
                                protocolVersion = oDSEMV3DS["protocolVersion"];
                            }
                            catch { }
                            try
                            {
                                acsURL = oDSEMV3DS["acsURL"];
                            }
                            catch { }
                            try
                            {
                                PAReq = oDSEMV3DS["PAReq"];
                            }
                            catch { }
                            try
                            {
                                MD = oDSEMV3DS["MD"];
                            }
                            catch { }

                            try
                            {
                                creq = oDSEMV3DS["creq"];
                            }
                            catch { }



                            if (threeDSInfo == "ChallengeRequest")
                            {
                                Dictionary<string, string> postData = new Dictionary<string, string>();

                                if (string.IsNullOrEmpty(creq))
                                {

                                    postData.Add("PaReq", PAReq);
                                    postData.Add("MD", MD);
                                    postData.Add("TermUrl", strReturnURL);                                    
                                    strMD = MD;
                                    strPaReq = PAReq;
                                }
                                else
                                {
                                    postData.Add("creq", creq);
                                    strCreq = creq;
                                }

                                strInlineForm = RedirectFormWithData(acsURL, postData);
                                bRet = true;
                                Logger_AddLogMessage(string.Format("StandardPayment3DSStep2 MD={0} / creq={1} / InlineForm={2}", strMD, strCreq, strInlineForm), LogLevels.logINFO);
                            }
                           
                        }
                        else if (!string.IsNullOrEmpty(sResponseCode))
                        {
                            try
                            {
                                int iResponseCode = Convert.ToInt32(sResponseCode);

                                if ((iResponseCode >= 0) && (iResponseCode < 100))
                                {
                                    sTransactionId = oAPIWs.GetDictionary("Ds_AuthorisationCode");
                                    sDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                                    bRet = true;

                                }
                                else
                                {
                                    Logger_AddLogMessage(string.Format("StandardPayment3DSStep2::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                    bRet = false;

                                }

                            }
                            catch
                            {
                                Logger_AddLogMessage(string.Format("StandardPayment3DSStep2::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                bRet = false;

                            }

                        }
                        else
                        {
                            Logger_AddLogMessage(string.Format("StandardPayment3DSStep2::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                            bRet = false;

                        }
                    }
                    catch
                    {
                        Logger_AddLogMessage(string.Format("StandardPayment3DSStep2::Response Error (Code={0}, Message='{1}')", eErrorCode, sErrorMessage), LogLevels.logERROR);
                        bRet = false;

                    }
                }
                else
                {
                    Logger_AddLogMessage(string.Format("StandardPayment3DSStep2::Response Error (Code={0}, Message='{1}')", eErrorCode, sErrorMessage), LogLevels.logERROR);
                    bRet = false;
                }

            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "StandardPayment3DSStep2::Exception", LogLevels.logERROR);
                bRet = false;
            }

            return bRet;
        }


        public bool StandardPayment3DSStep3(string sWsUrl, int iMerchantCode, string sMerchantSignature, string sMerchantTerminal, int iServiceTimeout,
                                          string sCardReference, int iQuantity, string sCurNumISOCode, string sMerchantGroup, string strCOFTxnID,
                                          string sOpReference, string strMD, string strPaRes, string strCres, ref string strProtocolVersion,
                                          out BSRedsysErrorCode eErrorCode, out string sErrorMessage, out string sTransactionId,
                                          out string sDateTime)
        {
            bool bRet = false;
            eErrorCode = BSRedsysErrorCode.ERROR;
            sErrorMessage = "";
            sTransactionId = "";
            sDateTime = "";


            AddTLS12Support();

            try
            {
                BSRedsysWs.SerClsWSEntradaService oWs = new BSRedsysWs.SerClsWSEntradaService();

                oWs.Url = sWsUrl;
                oWs.Timeout = iServiceTimeout;

                RedsysAPIWs oAPIWs = new RedsysAPIWs();

                string sTransactionType = "0";

                if (strCOFTxnID == sCardReference)
                {
                    strCOFTxnID = UNKNOWN_COF_TXT_INI;
                }


                string s3DSParameters="";

                if  (!string.IsNullOrEmpty(strMD))
                {
                    strProtocolVersion = (strProtocolVersion != "1.0.2") ? "1.0.2" : strProtocolVersion;

                    s3DSParameters = string.Format("{{\"threeDSInfo\":\"ChallengeResponse\"," +
                                                    "\"MD\":\"{0}\"," +
                                                    "\"protocolVersion\":\"{1}\"," +
                                                    "\"PARes\":\"{2}\"}}",
                                                    strMD, strProtocolVersion, strPaRes.Replace("\r\n", "\\r\\n"));
                }
                else if (!string.IsNullOrEmpty(strCres))
                {
                    s3DSParameters = string.Format("{{\"threeDSInfo\":\"ChallengeResponse\"," +
                                                "\"protocolVersion\":\"{0}\"," +
                                                "\"cres\":\"{1}\"}}",
                                                strProtocolVersion, strCres);

                }
                

                string sInputData = oAPIWs.GenerateDatoEntradaXML3DS(iQuantity.ToString(), iMerchantCode.ToString(), sCurNumISOCode, sCardReference, sTransactionType, sMerchantTerminal, sOpReference, null, sMerchantGroup, strCOFTxnID, s3DSParameters);

               

                string sSignature = oAPIWs.createMerchantSignatureHostToHost(sMerchantSignature, sInputData);

                string sRequestXML = oAPIWs.GenerateRequestXML(sInputData, sSignature);

                Logger_AddLogMessage(string.Format("StandardPayment3DSStep3 request.url={0}, request.xml={1}", sWsUrl, PrettyXml(sRequestXML)), LogLevels.logINFO);

                string sResult = oWs.trataPeticion(sRequestXML);

                Logger_AddLogMessage(string.Format("StandardPayment3DSStep3 response.xml={0}", PrettyXml(sResult)), LogLevels.logINFO);

                oAPIWs.XMLToDiccionary(sResult);


                string sCode = oAPIWs.GetDictionary("CODIGO");
                eErrorCode = GetErrorInfo(sCode, out sErrorMessage);
                if (eErrorCode == BSRedsysErrorCode.SIS0000)
                {
                    string sCadena = oAPIWs.GenerateCadena(sResult);
                    string sSignatureReceived = oAPIWs.GetDictionary("Ds_Signature");

                    string sNumOrder = oAPIWs.GetDictionary("Ds_Order");
                    string sResponseCode = oAPIWs.GetDictionary("Ds_Response");

                    if (!string.IsNullOrEmpty(sResponseCode))
                    {
                        try
                        {
                            int iResponseCode = Convert.ToInt32(sResponseCode);

                            if ((iResponseCode >= 0) && (iResponseCode < 100))
                            {

                                string sSignatureCalculate = oAPIWs.createSignatureResponseHostToHost(sMerchantSignature, sCadena, sNumOrder);

                                // Check if received signature is equal to calculated signature
                                if (sSignatureCalculate != sSignatureReceived)
                                    Logger_AddLogMessage(string.Format("StandardPayment3DSStep3 Received signature is not equal to calculated signature"), LogLevels.logERROR);

                                sTransactionId = oAPIWs.GetDictionary("Ds_AuthorisationCode");
                                sDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");                               
                                bRet = true;

                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("StandardPayment3DSStep3::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                bRet = false;

                            }

                        }
                        catch
                        {
                            Logger_AddLogMessage(string.Format("StandardPayment3DSStep3::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                            bRet = false;

                        }

                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("StandardPayment3DSStep3::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                        bRet = false;

                    }
                }
                else
                {
                    Logger_AddLogMessage(string.Format("StandardPayment3DSStep3::Response Error (Code={0}({1}), Message='{1}')", sCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                    bRet = false;
                }

            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "StandardPayment3DSStep3::Exception", LogLevels.logERROR);
                bRet = false;
            }

            return bRet;
        }




        public bool RefundTransaction(string sWsUrl, int iMerchantCode, string sMerchantSignature, string sMerchantTerminal, int iServiceTimeout,
                                      string sOpReference, string sTransactionId, int iQuantity, string sCurNumISOCode, string sMerchantGroup,
                                      out BSRedsysErrorCode eErrorCode, out string sErrorMessage, out string sRefundOpReference, out string sRefundTransactionId, out string sDateTime)
        {
            bool bRet = false;
            eErrorCode = BSRedsysErrorCode.ERROR;
            sErrorMessage = "";
            sRefundOpReference = "";
            sRefundTransactionId = "";
            sDateTime = "";

            AddTLS12Support();

            try
            {
                BSRedsysWs.SerClsWSEntradaService oWs = new BSRedsysWs.SerClsWSEntradaService();

                oWs.Url = sWsUrl;
                oWs.Timeout = iServiceTimeout;

                RedsysAPIWs oAPIWs = new RedsysAPIWs();

                string sTransactionType = "3";

                string sInputData = oAPIWs.GenerateDatoEntradaXMLTrataPeticionNO3DS(iQuantity.ToString(), iMerchantCode.ToString(), sCurNumISOCode, null, sTransactionType, sMerchantTerminal, sOpReference, sTransactionId, sMerchantGroup,"");

                string sSignature = oAPIWs.createMerchantSignatureHostToHost(sMerchantSignature, sInputData);

                string sRequestXML = oAPIWs.GenerateRequestXML(sInputData, sSignature);

                Logger_AddLogMessage(string.Format("RefundTransaction request.url={0}, request.xml={1}", sWsUrl, PrettyXml(sRequestXML)), LogLevels.logINFO);

                string sResult = oWs.trataPeticion(sRequestXML);

                Logger_AddLogMessage(string.Format("RefundTransaction response.xml={0}", PrettyXml(sResult)), LogLevels.logINFO);

                oAPIWs.XMLToDiccionary(sResult);

                string sCode = oAPIWs.GetDictionary("CODIGO");
                eErrorCode = GetErrorInfo(sCode, out sErrorMessage);
                if (eErrorCode == BSRedsysErrorCode.SIS0000)
                {
                    string sCadena = oAPIWs.GenerateCadena(sResult);
                    string sSignatureReceived = oAPIWs.GetDictionary("Ds_Signature");

                    string sNumOrder = oAPIWs.GetDictionary("Ds_Order");

                    string sResponseCode = oAPIWs.GetDictionary("Ds_Response");

                    if (!string.IsNullOrEmpty(sResponseCode))
                    {
                        try
                        {
                            int iResponseCode = Convert.ToInt32(sResponseCode);

                            if ((iResponseCode == 900) || (iResponseCode == 400))
                            {
                                string sSignatureCalculate = oAPIWs.createSignatureResponseHostToHost(sMerchantSignature, sCadena, sNumOrder);

                                // Check if received signature is equal to calculated signature
                                if (sSignatureCalculate != sSignatureReceived)
                                {
                                    Logger_AddLogMessage(string.Format("RefundTransaction Received signature is not equal to calculated signature"), LogLevels.logERROR);
                                }


                                sRefundTransactionId = oAPIWs.GetDictionary("Ds_AuthorisationCode");
                                sRefundOpReference = oAPIWs.GetDictionary("Ds_Order");
                                sDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                                bRet = true;

                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("RefundTransaction::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                                bRet = false;

                            }

                        }
                        catch
                        {
                            Logger_AddLogMessage(string.Format("RefundTransaction::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                            bRet = false;

                        }

                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("RefundTransaction::Response Error (Code={0}({1}), Message='{1}')", sResponseCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                        bRet = false;

                    }

                }
                else
                {
                    Logger_AddLogMessage(string.Format("RefundTransaction::Response Error (Code={0}({1}), Message='{1}')", sCode, eErrorCode, sErrorMessage), LogLevels.logERROR);
                    bRet = false;
                }

            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "RefundTransaction::Exception", LogLevels.logERROR);
                bRet = false;
            }

            return bRet;
        }

        private static void AddTLS12Support()        
        {

            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }

            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls11) != 0) //Disable TlS 1.1
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls11;
            }
            
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls) != 0) //Disable TLS 1.0
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
            }
            
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }
        
        private string Base64Encode(string toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes
                 = System.Text.Encoding.UTF8.GetBytes(toEncode);
                string returnValue
                      = System.Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message);
            }
        }



        protected string RedirectFormWithData(string url, Dictionary<string, string> postData)
        {

            StringBuilder s = new StringBuilder();
            
             s.AppendLine("<html><head><title>Title for Page</title></head>");
             s.AppendLine("<SCRIPT LANGUAGE=\"Javascript\">");
             s.AppendLine("<!--");
             s.AppendLine("function OnLoadEvent()");
             s.AppendLine("{");
             s.AppendLine("\tdocument.downloadForm.submit();");
             s.AppendLine("}");
             s.AppendLine("-->");
             s.AppendLine("</SCRIPT>");
             s.AppendLine("<body onload=\"OnLoadEvent()\">");
             s.AppendFormat("<form name=\"downloadForm\" action=\"{0}\" method=\"POST\">\r\n", url);
             s.AppendLine("<noscript>");
             s.AppendLine("<br>");
             s.AppendLine("<br>");
             s.AppendLine("<center>");
             s.AppendLine("<h1>Processing your 3-D Secure Transaction</h1>");
             s.AppendLine("<h2>");
             s.AppendLine("JavaScript is currently disabled or is not supported");
             s.AppendLine("by your browser.<br>");
             s.AppendLine("</h2>");
             s.AppendLine("<h3>Please click on the Submit button to continue");
             s.AppendLine("the processing of your 3-D secure");
             s.AppendLine("transaction.</h3>");
             s.AppendLine("<input type=\"submit\" value=\"Submit\">");
             s.AppendLine("</center>");
             s.AppendLine("</noscript>");
             foreach (string key in postData.Keys)
             {
                 s.AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />\r\n", key, postData[key]);
             }
             s.AppendLine("</form>");
             s.AppendLine("</body>");
             s.AppendLine("</html>");


             return s.ToString();
        }

        protected void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        protected void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        private string PrettyXml(string xml)
        {

            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return "\r\n\t" + stringBuilder.ToString().Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + xml + "\r\n";
            }
        }
    }
}
