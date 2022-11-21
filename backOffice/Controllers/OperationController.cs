using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Resources;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Properties;
using backOffice.Models;
using backOffice.Helper;

namespace backOffice.Controllers
{
    [HandleError]
    //[NoCache] 
    public class OperationController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;

        public OperationController(IBackOfficeRepository backOfficeRepository, IInfraestructureRepository infrastructureRepository)
        {
            this.backOfficeRepository = backOfficeRepository;
            this.infrastructureRepository = infrastructureRepository;
        }

        #region Actions

        public ActionResult Index()
        {
            return RedirectToAction("OperationsExt");
        }
        
        public ActionResult OperationsExt()
        {
            if (Helper.Helper.MenuOptionEnabled("Operation#OperationsExt"))
            {
                backOffice.Helper.Helper.PopulateChargeOperationTypes(ViewData);
                backOffice.Helper.Helper.PopulateMobileOSs(ViewData);
                backOffice.Helper.Helper.PopulatePaymentSuscryptionTypes(ViewData);
                backOffice.Helper.Helper.PopulateUsers(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateGroups(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateTariffs(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateServiceChargeTypes(ViewData, backOfficeRepository);
                var predicate = PredicateBuilder.False<ALL_OPERATIONS_EXT>();
                return View(OperationExtDataModel.List(backOfficeRepository, false, predicate));
            }
            else
                return RedirectToAction("BlankPage", "Home");
        }

        public ActionResult OperationsExt_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(OperationExtDataModel.List(backOfficeRepository, false, predicate).ToDataSourceResult(request));
        }

        public ActionResult OperationsExtNoRecharge()
        {
            if (Helper.Helper.MenuOptionEnabled("Operation#OperationsExtNoRecharge"))
            {
                backOffice.Helper.Helper.PopulateChargeOperationTypes(ViewData, true);
                backOffice.Helper.Helper.PopulateMobileOSs(ViewData);
                backOffice.Helper.Helper.PopulatePaymentSuscryptionTypes(ViewData);
                backOffice.Helper.Helper.PopulateUsers(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateGroups(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateTariffs(ViewData, backOfficeRepository);                
                return View(OperationExtNoRechargeDataModel.List(backOfficeRepository, PredicateBuilder.False<ALL_OPERATIONS_EXT>(), false));
            }
            else
                return RedirectToAction("BlankPage", "Home");
        }

        public ActionResult OperationsExtNoRecharge_Read([DataSourceRequest] DataSourceRequest request)                            
        {
            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(OperationExtNoRechargeDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        public ActionResult ExternalOperations()
        {
            if (Helper.Helper.MenuOptionEnabled("Operation#ExternalOperations"))
            {
                backOffice.Helper.Helper.PopulateChargeOperationTypes(ViewData);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateGroups(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateTariffs(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateOperationSourceTypes(ViewData);
                backOffice.Helper.Helper.PopulateBooleans(ViewData);
                backOffice.Helper.Helper.PopulateExternalProviders(ViewData, backOfficeRepository);
                var predicate = PredicateBuilder.False<EXTERNAL_PARKING_OPERATION>();
                return View(ExternalOperationDataModel.List(backOfficeRepository, predicate, false));
            }
            else
                return RedirectToAction("BlankPage", "Home");
        }

        public ActionResult ExternalOperations_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<EXTERNAL_PARKING_OPERATION>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(ExternalOperationDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        public ActionResult OperationExt_Delete(int typeId, decimal operationId)
        {
            bool bRet = false;
            string sErrorInfo = "";
            
            object oObjDeleted = null;
            int iBalanceBefore = 0;
            USER oUser = null;
            OPERATIONS_DISCOUNT oDiscountDeleted = null;
            bool bIsHisOperation = false;

            if ((ChargeOperationsType) typeId != ChargeOperationsType.Discount)
                bRet = backOfficeRepository.DeleteOperation((ChargeOperationsType)typeId, operationId, out oObjDeleted, out iBalanceBefore, out oUser, out oDiscountDeleted, out bIsHisOperation);

            if (bRet)
            {
                // send notification emails
                string sEmails = ConfigurationManager.AppSettings["DeleteOperation_NotificationEmails"] ?? "";
                if (!string.IsNullOrWhiteSpace(sEmails))
                {
                    List<string> emails = sEmails.Split(';').ToList().Where(email => !string.IsNullOrWhiteSpace(email)).ToList();
                    if (emails.Count > 0)
                    {
                        string sSubject = "";
                        string sBody = "";
                        string sPaymentInfo = "";
                        
                        switch ((ChargeOperationsType) typeId)
                        {
                            case ChargeOperationsType.ParkingOperation:
                            case ChargeOperationsType.ExtensionOperation:
                            case ChargeOperationsType.ParkingRefund:

                                if ((ChargeOperationsType)typeId == ChargeOperationsType.ParkingOperation)
                                {
                                    sSubject = Resources.OperationExt_Delete_Parking_EmailHeader;
                                    sBody = Resources.OperationExt_Delete_Parking_EmailBody;
                                }
                                else if ((ChargeOperationsType)typeId == ChargeOperationsType.ExtensionOperation)
                                {
                                    sSubject = Resources.OperationExt_Delete_Extension_EmailHeader;
                                    sBody = Resources.OperationExt_Delete_Extension_EmailBody;
                                }
                                else
                                {
                                    sSubject = Resources.OperationExt_Delete_UnParking_EmailHeader;
                                    sBody = Resources.OperationExt_Delete_UnParking_EmailBody;
                                }

                                dynamic oOperation;
                                if (!bIsHisOperation)
                                    oOperation = (OPERATION)oObjDeleted;
                                else
                                    oOperation = (HIS_OPERATION)oObjDeleted;

                                if ((PaymentSuscryptionType)oOperation.OPE_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                {
                                    sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                 Resources.OperationExtDataModel_OpReference, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_OP_REFERENCE,
                                                                 Resources.OperationExtDataModel_TransactionId, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANSACTION_ID,
                                                                 Resources.OperationExtDataModel_AuthCode, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AUTH_CODE,
                                                                 Resources.OperationExtDataModel_CardHash, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_HASH,
                                                                 Resources.OperationExtDataModel_CardReference, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_REFERENCE,
                                                                 Resources.OperationExtDataModel_CardScheme, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_SCHEME,
                                                                 Resources.OperationExtDataModel_MaskedCardNumber, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_MASKED_CARD_NUMBER,
                                                                 Resources.OperationExtDataModel_CardExpirationDate, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_EXPIRATION_DATE);
                                }

                                string sDiscountInfo = "";
                                if (oDiscountDeleted != null)
                                {
                                    sDiscountInfo = string.Format(Resources.OperationExt_Deleted_EmailBody_Discount,
                                                (oDiscountDeleted.OPEDIS_AMOUNT_CUR_ID == oDiscountDeleted.OPEDIS_BALANCE_CUR_ID ?
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(oDiscountDeleted.OPEDIS_AMOUNT) / 100, oDiscountDeleted.CURRENCy.CUR_ISO_CODE) :
                                                string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oDiscountDeleted.OPEDIS_AMOUNT) / 100, oDiscountDeleted.CURRENCy.CUR_ISO_CODE,
                                                                                             Convert.ToDouble(oDiscountDeleted.OPEDIS_FINAL_AMOUNT) / 100,
                                                                                             oDiscountDeleted.CURRENCy1.CUR_ISO_CODE)));
                                }

                                sBody = string.Format(sBody,
                                            oOperation.OPE_ID,                                            
                                            oOperation.USER_PLATE.USRP_PLATE,
                                            oOperation.INSTALLATION.INS_DESCRIPTION,
                                            (oOperation.GROUP != null ? oOperation.GROUP.GRP_DESCRIPTION : ""),
                                            (oOperation.TARIFF != null ? oOperation.TARIFF.TAR_DESCRIPTION : ""),
                                            oOperation.OPE_DATE,
                                            oOperation.OPE_INIDATE,
                                            oOperation.OPE_ENDDATE,
                                            (oOperation.OPE_AMOUNT_CUR_ID == oOperation.OPE_BALANCE_CUR_ID ?
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(oOperation.OPE_AMOUNT) / 100, oOperation.CURRENCy.CUR_ISO_CODE) :
                                                string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oOperation.OPE_AMOUNT) / 100, oOperation.CURRENCy.CUR_ISO_CODE,
                                                                                             Convert.ToDouble(oOperation.OPE_FINAL_AMOUNT) / 100, 
                                                                                             oOperation.CURRENCy1.CUR_ISO_CODE)),
                                            oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                                  Convert.ToDouble(iBalanceBefore) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) : 
                                                "",
                                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                            oUser.USR_USERNAME,
                                            oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                  Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) : 
                                                "",
                                            sPaymentInfo,
                                            sDiscountInfo);
                                break;

                            case ChargeOperationsType.TicketPayment:

                                sSubject = Resources.OperationExt_Delete_TicketPayment_EmailHeader;
                                sBody = Resources.OperationExt_Delete_TicketPayment_EmailBody;

                                TICKET_PAYMENT oTicketPayment = (TICKET_PAYMENT)oObjDeleted;

                                sBody = string.Format(sBody,
                                            oTicketPayment.TIPA_ID,
                                            oTicketPayment.TIPA_TICKET_NUMBER,
                                            oTicketPayment.TIPA_PLATE_STRING,
                                            oTicketPayment.INSTALLATION.INS_DESCRIPTION,
                                            oTicketPayment.TIPA_DATE,
                                            oTicketPayment.TIPA_TICKET_DATA,
                                            (oTicketPayment.TIPA_AMOUNT_CUR_ID == oTicketPayment.TIPA_BALANCE_CUR_ID ?
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(oTicketPayment.TIPA_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE) :
                                                string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oTicketPayment.TIPA_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE,
                                                                                            Convert.ToDouble(oTicketPayment.TIPA_FINAL_AMOUNT) / 100, oTicketPayment.CURRENCy1.CUR_ISO_CODE)),
                                            oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                                  Convert.ToDouble(iBalanceBefore) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) : 
                                                    "",
                                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                            oUser.USR_USERNAME,
                                            oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                  Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                "");

                                break;

                            case ChargeOperationsType.BalanceRecharge:

                                sSubject = Resources.OperationExt_Delete_Recharge_EmailHeader;
                                sBody = Resources.OperationExt_Delete_Recharge_EmailBody;

                                CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = (CUSTOMER_PAYMENT_MEANS_RECHARGE)oObjDeleted;

                                sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                             Resources.OperationExtDataModel_OpReference, oRecharge.CUSPMR_OP_REFERENCE,
                                                             Resources.OperationExtDataModel_TransactionId, oRecharge.CUSPMR_TRANSACTION_ID,
                                                             Resources.OperationExtDataModel_AuthCode, oRecharge.CUSPMR_AUTH_CODE,
                                                             Resources.OperationExtDataModel_CardHash, oRecharge.CUSPMR_CARD_HASH,
                                                             Resources.OperationExtDataModel_CardReference, oRecharge.CUSPMR_CARD_REFERENCE,
                                                             Resources.OperationExtDataModel_CardScheme, oRecharge.CUSPMR_CARD_SCHEME,
                                                             Resources.OperationExtDataModel_MaskedCardNumber, oRecharge.CUSPMR_MASKED_CARD_NUMBER,
                                                             Resources.OperationExtDataModel_CardExpirationDate, oRecharge.CUSPMR_CARD_EXPIRATION_DATE);

                                sBody = string.Format(sBody,
                                            oRecharge.CUSPMR_ID,
                                            oRecharge.CUSPMR_DATE,
                                            string.Format("{0:0.00} {1}", Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100,
                                                                          infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                            string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                              Convert.ToDouble(iBalanceBefore) / 100,
                                                                              infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                            oUser.USR_USERNAME,
                                            string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                              Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                              infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                            sPaymentInfo);

                                break;

                            case ChargeOperationsType.ServiceCharge:

                                sSubject = Resources.OperationExt_Delete_ServiceCharge_EmailHeader;
                                sBody = Resources.OperationExt_Delete_ServiceCharge_EmailBody;

                                SERVICE_CHARGE oServiceCharge = (SERVICE_CHARGE)oObjDeleted;

                                if ((PaymentSuscryptionType)oServiceCharge.SECH_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                {
                                    sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                 Resources.OperationExtDataModel_OpReference, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_OP_REFERENCE,
                                                                 Resources.OperationExtDataModel_TransactionId, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANSACTION_ID,
                                                                 Resources.OperationExtDataModel_AuthCode, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AUTH_CODE,
                                                                 Resources.OperationExtDataModel_CardHash, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_HASH,
                                                                 Resources.OperationExtDataModel_CardReference, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_REFERENCE,
                                                                 Resources.OperationExtDataModel_CardScheme, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_SCHEME,
                                                                 Resources.OperationExtDataModel_MaskedCardNumber, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_MASKED_CARD_NUMBER,
                                                                 Resources.OperationExtDataModel_CardExpirationDate, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_EXPIRATION_DATE);
                                }

                                sBody = string.Format(sBody,
                                            oServiceCharge.SECH_ID,
                                            oServiceCharge.SECH_DATE,
                                            (oServiceCharge.SECH_AMOUNT_CUR_ID == oServiceCharge.SECH_BALANCE_CUR_ID ?
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(oServiceCharge.SECH_AMOUNT) / 100, oServiceCharge.CURRENCy.CUR_ISO_CODE) :
                                                string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oServiceCharge.SECH_AMOUNT) / 100, oServiceCharge.CURRENCy.CUR_ISO_CODE,
                                                                                            Convert.ToDouble(oServiceCharge.SECH_FINAL_AMOUNT) / 100, oServiceCharge.CURRENCy1.CUR_ISO_CODE)),
                                            string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                              Convert.ToDouble(iBalanceBefore) / 100,
                                                                              infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                            oUser.USR_USERNAME,
                                            oServiceCharge.SECH_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                  Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                "",
                                            sPaymentInfo);

                                break;

                            case ChargeOperationsType.Discount:


                                break;

                        }
                        infrastructureRepository.SendEmailToMultiRecipients(emails, sSubject, sBody, integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                    }
                }
            }
            else
            {
                sErrorInfo = Resources.OperationExt_Delete_Error;
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        /*public ActionResult OperationExt_DeleteList()
        {
            bool bRet = false;
            string sErrorInfo = "";

            object oObjDeleted = null;
            int iBalanceBefore = 0;
            USER oUser = null;
            OPERATIONS_DISCOUNT oDiscountDeleted = null;

            decimal[] idArr = new decimal[] {125256,
125260,
125270,
125271,
125278,
125279,
125280,
125289,
125293,
125295,
125299,
125300,
125304,
125305,
125307,
125308,
125309,
125310,
125312,
125313,
125314,
125315,
125321,
125328,
125333,
125334,
125335,
125336,
125337,
125340,
125342,
125352,
125353,
125354,
125355,
125361,
125370,
125373,
125376,
125377,
125378,
125379,
125380,
125381,
125387,
125390,
125395,
125404,
125412,
125413,
125415,
125416,
125421,
125425,
125428,
125434,
125437,
125438,
125440,
125441,
125444,
125446,
125450,
125454,
125457,
125458,
125459,
125460,
125461,
125462,
125463,
125468,
125471,
125473,
125475,
125478,
125481,
125482,
125487,
125488,
125490,
125491,
125492,
125494,
125495,
125497,
125499,
125501,
125502,
125505,
125506,
125507,
125510,
125513,
125514,
125516,
125517,
125519,
125520,
125523,
125524,
125526,
125527,
125528,
125529,
125530,
125531,
125534,
125535,
125536,
125537,
125538,
125540,
125543,
125545,
125556,
125557,
125560,
125561,
125562,
125564,
125566,
125567,
125569,
125570,
125574,
125576,
125581,
125591,
125592,
125593,
125594,
125595,
125599,
125602,
125605,
125606,
125608,
125609,
125610,
125614,
125617,
125618,
125619,
125629,
125630,
125631,
125633,
125635,
125636,
125637,
125638,
125640,
125641,
125642,
125643,
125644,
125646,
125648,
125651,
125652,
125653,
125656,
125658,
125659,
125662,
125664,
125672,
125677,
125678,
125680,
125681,
125683,
125697,
125698,
125700,
125701,
125704,
125706,
125708,
125709,
125711,
125721,
125723,
125724,
125725,
125731,
125733,
125735,
125736,
125740,
125741,
125742,
125743,
125744,
125745,
125746,
125748,
125750,
125755,
125757,
125758,
125759,
125762,
125763,
125764,
125769,
125770,
125773,
125774,
125776,
125777,
125778,
125781,
125783,
125785,
125787,
125789,
125790,
125792,
125793,
125797,
125798,
125803,
125805,
125808,
125809,
125812,
125813,
125815,
125817,
125819,
125822,
125823,
125825,
125826,
125827,
125828,
125829,
125833,
125835,
125836,
125838,
125839,
125841,
125842,
125846,
125848,
125852,
125853,
125854,
125856,
125859,
125860,
125867,
125869,
125870,
125872,
125873,
125874,
125875,
125876,
125879,
125882,
125885,
125886,
125890,
125891,
125893,
125894,
125896,
125897,
125898,
125899,
125900,
125903,
125904,
125907,
125909,
125910,
125912,
125914,
125915,
125916,
125920,
125923,
125925,
125930,
125931,
125937,
125939,
125941,
125942,
125943,
125944,
125945,
125947,
125948,
125950,
125958,
125967,
125968,
125971,
125974,
125976,
125979,
125981,
125982,
125987,
125988,
125989,
125990,
125991,
125997,
125998,
126000,
126002,
126004,
126005,
126013,
126014,
126016,
126019,
126020,
126022,
126028,
126030,
126033,
126034,
126038,
126041,
126044,
126046,
126047,
126048,
126051,
126055,
126057,
126061,
126063,
126066,
126071,
126074,
126075,
126077,
126079,
126081,
126082,
126083,
126085,
126086,
126090,
126092,
126095,
126097,
126098,
126099,
126100,
126101,
126104,
126107,
126108,
126112,
126113,
126114,
126118,
126122,
126124,
126127,
126128,
126129,
126131,
126133,
126135,
126140,
126142,
126146,
126147,
126148,
126149,
126150,
126152,
126156,
126158,
126159,
126160,
126162,
126163,
126165,
126166,
126167,
126168,
126169,
126170,
126176,
126178,
126179,
126182,
126183,
126184,
126185,
126187,
126190,
126191,
126192,
126194,
126195,
126203,
126204,
126208,
126209,
126210,
126215,
126218,
126219,
126223,
126230,
126232,
126235,
126236,
126237,
126238,
126239,
126241,
126243,
126244,
126245,
126246,
126251,
126259,
126262,
126264,
126265,
126266,
126267,
126270,
126273,
126277,
126278,
126279,
126280,
126282,
126290,
126295,
126296,
126297,
126302,
126304,
126317,
126325,
126328,
126329,
126334,
126338,
126339,
126342,
126343,
126345,
126346,
126347,
126349,
126365,
126366,
126368,
126372,
126373,
126381,
126382,
126383,
126385,
126387,
126391,
126401,
126402,
126406,
126407,
126408,
126409,
126413,
126417,
126419,
126423,
126424,
126431,
126432,
126436,
126444,
126445,
126447,
126451,
126452,
126454,
126456,
126457,
126458,
126459,
126460,
126463,
126467,
126469,
126471,
126472,
126473,
126474,
126475,
126479,
126481,
126483,
126484,
126487,
126488,
126491,
126494,
126495,
126496,
126499,
126500,
126502,
126503,
126504,
126505,
126509,
126510,
126511,
126516,
126517,
126521,
126524,
126526,
126530,
126531,
126532,
126534,
126537,
126538,
126540,
126542,
126543,
126544,
126545,
126546,
126548,
126552,
126553,
126554,
126555,
126557,
126559,
126560,
126562,
126565,
126568,
126570,
126571,
126574,
126575,
126577,
126580,
126581,
126589,
126591,
126595,
126596,
126597,
126601,
126602,
126603,
126605,
126607,
126609,
126612,
126614,
126615,
126616,
126618,
126625,
126628,
126631,
126634,
126638,
126639,
126642,
126644,
126645,
126648,
126652,
126654,
126655,
126660,
126667,
126668,
126670,
126671,
126672,
126673,
126674,
126675,
126677,
126679,
126681,
126682,
126684,
126686,
126696,
126697,
126699,
126700,
126703,
126707,
126711,
126714,
126715,
126716,
126718,
126719,
126721,
126726,
126728,
126729,
126731,
126733,
126734,
126742,
126743,
126745,
126750,
126754,
126758,
126759,
126760,
126761,
126762,
126766,
126767,
126769,
126771,
126773,
126777,
126778,
126780,
126782,
126783,
126787,
126788,
126792,
126793,
126794,
126795,
126796,
126802,
126804,
126805,
126806,
126810,
126811,
126812,
126815,
126816,
126818,
126819,
126820,
126824,
126826,
126827,
126828,
126829,
126830,
126834,
126840,
126842,
126843,
126844,
126845,
126852,
126853,
126854,
126855,
126857,
126860,
126862,
126863,
126866,
126867,
126868,
126870,
126871,
126875,
126876,
126877,
126878,
126879,
126880,
126888,
126890,
126891,
126892,
126893,
126894,
126895,
126896,
126898,
126900,
126901,
126902,
126904,
126905,
126906,
126907,
126911,
126914,
126918,
126919,
126920,
126921,
126922,
126924,
126925,
126935,
126938,
126941,
126957,
126959,
126962,
126964,
126965,
126966,
126968,
126969,
126972,
126973,
126975,
126976,
126982,
126984,
126985,
126987,
126988,
126989,
126991,
126996,
126999,
127000,
127001,
127002,
127003,
127009,
127010,
127013,
127014,
127027,
127030,
127034,
127035,
127036,
127043,
127044,
127047,
127049,
127052,
127063,
127066,
127067,
127068,
127069,
127070,
127072,
127074,
127076,
127077,
127079,
127081,
127082,
127083,
127085,
127086,
127089,
127090,
127098,
127099,
127109,
127111,
127114,
127117,
127120,
127122,
127123,
127124,
127125,
127133,
127136,
127137,
127138,
127140,
127141,
127142,
127145,
127146,
127152,
127155,
127157,
127161,
127162,
127163,
127164,
127165,
127166,
127167,
127174,
127175,
127177,
127178,
127182,
127185,
127186,
127187,
127188,
127190,
127195,
127196,
127197,
127198,
127199,
127200,
127201,
127202,
127207,
127212,
127216,
127217,
127222,
127223,
127227,
127231,
127233,
127234,
127235,
127236,
127237,
127240,
127241,
127243,
127244,
127245,
127247,
127249,
127252,
127254,
127258,
127259,
127260,
127264,
127265,
127266,
127267,
127268,
127270,
127275,
127284,
127285,
127288,
127290,
127296,
127297,
127298,
127299,
127300,
127304,
127306,
127307,
127308,
127310,
127311,
127317,
127322,
127323,
127325,
127326,
127328,
127329,
127330,
127332,
127334,
127336,
127338,
127340,
127341,
127342,
127346,
127347,
127349,
127354,
127355,
127358,
127359,
127360,
127362,
127364,
127365,
127368,
127369,
127371,
127372,
127380,
127381,
127382,
127383,
127385,
127386,
127388,
127389,
127391,
127392,
127394,
127395,
127398,
127409,
127411,
127414,
127415,
127418,
127422,
127423,
127425,
127432,
127433,
127436,
127437,
127439,
127440,
127442,
127444,
127445,
127448,
127449,
127450,
127453,
127455,
127456,
127457,
127459,
127460,
127461,
127463,
127466,
127467,
127473,
127474,
127476,
127481,
127483,
127485,
127486,
127491,
127495,
127496,
127497,
127498,
127502,
127504,
127511,
127513,
127514,
127516,
127520,
127521,
127522,
127523,
127524,
127525,
127526,
127527,
127529,
127530,
127531,
127532,
127535,
127537,
127538,
127539,
127540,
127542,
127547,
127549,
127550,
127551,
127552,
127554,
127556,
127557,
127558,
127559,
127565,
127566,
127567,
127570,
127576,
127577,
127579,
127583,
127584,
127585,
127591,
127592,
127593,
127594,
127596,
127597,
127598,
127599,
127600,
127603,
127604,
127607,
127608,
127612,
127616,
127617,
127621,
127623,
127625,
127626,
127627,
127629,
127631,
127633,
127634,
127635,
127639,
127641,
127642,
127643,
127647,
127651,
127652,
127653,
127654,
127655,
127662,
127666,
127668,
127671,
127672,
127673,
127675,
127676,
127680,
127681,
127682,
127684,
127685,
127686,
127687,
127688,
127690,
127692,
127694,
127696,
127697,
127698,
127700,
127702,
127703,
127706,
127709,
127710,
127712,
127713,
127715,
127719,
127720,
127721,
127722,
127724,
127726,
127729,
127731,
127732,
127733,
127741,
127744,
127747,
127754,
127757,
127759,
127760,
127761,
127764,
127766,
127768,
127769,
127771,
127772,
127773,
127775,
127782,
127783,
127784,
127788,
127789,
127793,
127795,
127796,
127797,
127798,
127803,
127804,
127805,
127806,
127808,
127809,
127813,
127814,
127817,
127818,
127821,
127823,
127824,
127827,
127828,
127832,
127836,
127837,
127838,
127844,
127848};
 

            foreach (decimal operationId in idArr)
            {
                ChargeOperationsType typeId = ChargeOperationsType.BalanceRecharge;
                bRet = backOfficeRepository.DeleteOperation((ChargeOperationsType)typeId, operationId, out oObjDeleted, out iBalanceBefore, out oUser, out oDiscountDeleted);

                if (bRet)
                {
                    // send notification emails
                    string sEmails = ConfigurationManager.AppSettings["DeleteOperation_NotificationEmails"] ?? "";
                    if (!string.IsNullOrWhiteSpace(sEmails))
                    {
                        List<string> emails = sEmails.Split(';').ToList().Where(email => !string.IsNullOrWhiteSpace(email)).ToList();
                        if (emails.Count > 0)
                        {
                            string sSubject = "";
                            string sBody = "";
                            string sPaymentInfo = "";

                            switch ((ChargeOperationsType)typeId)
                            {
                                case ChargeOperationsType.ParkingOperation:
                                case ChargeOperationsType.ExtensionOperation:
                                case ChargeOperationsType.ParkingRefund:

                                    if ((ChargeOperationsType)typeId == ChargeOperationsType.ParkingOperation)
                                    {
                                        sSubject = Resources.OperationExt_Delete_Parking_EmailHeader;
                                        sBody = Resources.OperationExt_Delete_Parking_EmailBody;
                                    }
                                    else if ((ChargeOperationsType)typeId == ChargeOperationsType.ExtensionOperation)
                                    {
                                        sSubject = Resources.OperationExt_Delete_Extension_EmailHeader;
                                        sBody = Resources.OperationExt_Delete_Extension_EmailBody;
                                    }
                                    else
                                    {
                                        sSubject = Resources.OperationExt_Delete_UnParking_EmailHeader;
                                        sBody = Resources.OperationExt_Delete_UnParking_EmailBody;
                                    }

                                    OPERATION oOperation = (OPERATION)oObjDeleted;

                                    if ((PaymentSuscryptionType)oOperation.OPE_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                    {
                                        sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                     Resources.OperationExtDataModel_OpReference, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_OP_REFERENCE,
                                                                     Resources.OperationExtDataModel_TransactionId, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANSACTION_ID,
                                                                     Resources.OperationExtDataModel_AuthCode, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AUTH_CODE,
                                                                     Resources.OperationExtDataModel_CardHash, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_HASH,
                                                                     Resources.OperationExtDataModel_CardReference, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_REFERENCE,
                                                                     Resources.OperationExtDataModel_CardScheme, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_SCHEME,
                                                                     Resources.OperationExtDataModel_MaskedCardNumber, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_MASKED_CARD_NUMBER,
                                                                     Resources.OperationExtDataModel_CardExpirationDate, oOperation.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_EXPIRATION_DATE);
                                    }

                                    string sDiscountInfo = "";
                                    if (oDiscountDeleted != null)
                                    {
                                        sDiscountInfo = string.Format(Resources.OperationExt_Deleted_EmailBody_Discount,
                                                    (oDiscountDeleted.OPEDIS_AMOUNT_CUR_ID == oDiscountDeleted.OPEDIS_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oDiscountDeleted.OPEDIS_AMOUNT) / 100, oDiscountDeleted.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oDiscountDeleted.OPEDIS_AMOUNT) / 100, oDiscountDeleted.CURRENCy.CUR_ISO_CODE,
                                                                                                 Convert.ToDouble(oDiscountDeleted.OPEDIS_FINAL_AMOUNT) / 100,
                                                                                                 oDiscountDeleted.CURRENCy1.CUR_ISO_CODE)));
                                    }

                                    sBody = string.Format(sBody,
                                                oOperation.OPE_ID,
                                                oOperation.USER_PLATE.USRP_PLATE,
                                                oOperation.INSTALLATION.INS_DESCRIPTION,
                                                (oOperation.GROUP != null ? oOperation.GROUP.GRP_DESCRIPTION : ""),
                                                (oOperation.TARIFF != null ? oOperation.TARIFF.TAR_DESCRIPTION : ""),
                                                oOperation.OPE_DATE,
                                                oOperation.OPE_INIDATE,
                                                oOperation.OPE_ENDDATE,
                                                (oOperation.OPE_AMOUNT_CUR_ID == oOperation.OPE_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oOperation.OPE_AMOUNT) / 100, oOperation.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oOperation.OPE_AMOUNT) / 100, oOperation.CURRENCy.CUR_ISO_CODE,
                                                                                                 Convert.ToDouble(oOperation.OPE_FINAL_AMOUNT) / 100,
                                                                                                 oOperation.CURRENCy1.CUR_ISO_CODE)),
                                                oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                                      Convert.ToDouble(iBalanceBefore) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "",
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                oOperation.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                      Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "",
                                                sPaymentInfo,
                                                sDiscountInfo);
                                    break;

                                case ChargeOperationsType.TicketPayment:

                                    sSubject = Resources.OperationExt_Delete_TicketPayment_EmailHeader;
                                    sBody = Resources.OperationExt_Delete_TicketPayment_EmailBody;

                                    TICKET_PAYMENT oTicketPayment = (TICKET_PAYMENT)oObjDeleted;

                                    sBody = string.Format(sBody,
                                                oTicketPayment.TIPA_ID,
                                                oTicketPayment.TIPA_TICKET_NUMBER,
                                                oTicketPayment.TIPA_PLATE_STRING,
                                                oTicketPayment.INSTALLATION.INS_DESCRIPTION,
                                                oTicketPayment.TIPA_DATE,
                                                oTicketPayment.TIPA_TICKET_DATA,
                                                (oTicketPayment.TIPA_AMOUNT_CUR_ID == oTicketPayment.TIPA_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oTicketPayment.TIPA_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oTicketPayment.TIPA_AMOUNT) / 100, oTicketPayment.CURRENCy.CUR_ISO_CODE,
                                                                                                Convert.ToDouble(oTicketPayment.TIPA_FINAL_AMOUNT) / 100, oTicketPayment.CURRENCy1.CUR_ISO_CODE)),
                                                oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                                      Convert.ToDouble(iBalanceBefore) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                        "",
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                oTicketPayment.TIPA_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                      Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "");

                                    break;

                                case ChargeOperationsType.BalanceRecharge:

                                    sSubject = Resources.OperationExt_Delete_Recharge_EmailHeader;
                                    sBody = Resources.OperationExt_Delete_Recharge_EmailBody;

                                    CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = (CUSTOMER_PAYMENT_MEANS_RECHARGE)oObjDeleted;

                                    sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                 Resources.OperationExtDataModel_OpReference, oRecharge.CUSPMR_OP_REFERENCE,
                                                                 Resources.OperationExtDataModel_TransactionId, oRecharge.CUSPMR_TRANSACTION_ID,
                                                                 Resources.OperationExtDataModel_AuthCode, oRecharge.CUSPMR_AUTH_CODE,
                                                                 Resources.OperationExtDataModel_CardHash, oRecharge.CUSPMR_CARD_HASH,
                                                                 Resources.OperationExtDataModel_CardReference, oRecharge.CUSPMR_CARD_REFERENCE,
                                                                 Resources.OperationExtDataModel_CardScheme, oRecharge.CUSPMR_CARD_SCHEME,
                                                                 Resources.OperationExtDataModel_MaskedCardNumber, oRecharge.CUSPMR_MASKED_CARD_NUMBER,
                                                                 Resources.OperationExtDataModel_CardExpirationDate, oRecharge.CUSPMR_CARD_EXPIRATION_DATE);

                                    sBody = string.Format(sBody,
                                                oRecharge.CUSPMR_ID,
                                                oRecharge.CUSPMR_DATE,
                                                string.Format("{0:0.00} {1}", Convert.ToDouble(oRecharge.CUSPMR_AMOUNT) / 100,
                                                                              infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                                  Convert.ToDouble(iBalanceBefore) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                  Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                sPaymentInfo);

                                    break;

                                case ChargeOperationsType.ServiceCharge:

                                    sSubject = Resources.OperationExt_Delete_ServiceCharge_EmailHeader;
                                    sBody = Resources.OperationExt_Delete_ServiceCharge_EmailBody;

                                    SERVICE_CHARGE oServiceCharge = (SERVICE_CHARGE)oObjDeleted;

                                    if ((PaymentSuscryptionType)oServiceCharge.SECH_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction && oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                    {
                                        sPaymentInfo = string.Format("<br>{0}:{1}<br>{2}:{3}<br>{4}:{5}<br>{6}:{7}<br>{8}:{9}<br>{10}:{11}<br>{12}:{13}<br>{14}:{15}",
                                                                     Resources.OperationExtDataModel_OpReference, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_OP_REFERENCE,
                                                                     Resources.OperationExtDataModel_TransactionId, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_TRANSACTION_ID,
                                                                     Resources.OperationExtDataModel_AuthCode, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_AUTH_CODE,
                                                                     Resources.OperationExtDataModel_CardHash, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_HASH,
                                                                     Resources.OperationExtDataModel_CardReference, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_REFERENCE,
                                                                     Resources.OperationExtDataModel_CardScheme, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_SCHEME,
                                                                     Resources.OperationExtDataModel_MaskedCardNumber, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_MASKED_CARD_NUMBER,
                                                                     Resources.OperationExtDataModel_CardExpirationDate, oServiceCharge.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSPMR_CARD_EXPIRATION_DATE);
                                    }

                                    sBody = string.Format(sBody,
                                                oServiceCharge.SECH_ID,
                                                oServiceCharge.SECH_DATE,
                                                (oServiceCharge.SECH_AMOUNT_CUR_ID == oServiceCharge.SECH_BALANCE_CUR_ID ?
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(oServiceCharge.SECH_AMOUNT) / 100, oServiceCharge.CURRENCy.CUR_ISO_CODE) :
                                                    string.Format("{0:0.00} {1} / {2:0.00} {3}", Convert.ToDouble(oServiceCharge.SECH_AMOUNT) / 100, oServiceCharge.CURRENCy.CUR_ISO_CODE,
                                                                                                Convert.ToDouble(oServiceCharge.SECH_FINAL_AMOUNT) / 100, oServiceCharge.CURRENCy1.CUR_ISO_CODE)),
                                                string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceBefore,
                                                                                  Convert.ToDouble(iBalanceBefore) / 100,
                                                                                  infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                oUser.USR_USERNAME,
                                                oServiceCharge.SECH_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay ?
                                                    string.Format("{0} {1:0.00} {2}", Resources.OperationExt_Deleted_EmailBody_BalanceAfter,
                                                                                      Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                                                                      infrastructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) :
                                                    "",
                                                sPaymentInfo);

                                    break;

                                case ChargeOperationsType.Discount:


                                    break;

                            }
                            infrastructureRepository.SendEmailToMultiRecipients(emails, sSubject, sBody, integraMobile.Domain.integraSenderWS.EmailPriority.VeryLow);
                        }
                    }
                }
                else
                {
                    sErrorInfo = Resources.OperationExt_Delete_Error;
                }
            }
            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }
        */


        #endregion

        #region Methods

        public static string GetOperationStringType(int opType)
        {
            return Resources.ResourceManager.GetString("ChargeOperationsType_" + ((ChargeOperationsType)opType).ToString());
        }

        public static string GetChargeOperationsTypeEnum()
        {
            //string[] names = Enum.GetNames(typeof(ChargeOperationsType));
            List<string> lstNames = new List<string>();
            lstNames.Add("");
            foreach (var type in Enum.GetValues(typeof(ChargeOperationsType)))
            {
                lstNames.Add(GetOperationStringType((int)type));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(lstNames.ToArray());

        }        

        #endregion

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }

}
