using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using WebParking.WS;
using WebParking.WS.Data;
using WebParking.Helper;
using WebParking.Models;
using WebParking.Properties;

namespace WebParking.Controllers
{
    public class ParkController : Controller
    {
        #region Actions

        //
        // GET: /Park/

        /*public ActionResult Test()
        {
            WSIntegraMobile oWS = new WSIntegraMobile();
            Dictionary<int, string> oCities = null;
            SortedList oParametersOut = new SortedList();
            ResultType oRes = oWS.GetListOfCities(null, null, out oCities, ref oParametersOut);

            if (oCities.Count > 0)
            {
                string sSessionID = "";
                oParametersOut = new SortedList();
                oRes = oWS.QueryLogin("hbusque@integraparking.com", "hbusque", "ca-ES", true, oCities.Keys.First(), null, null, out sSessionID, ref oParametersOut);

                if (oRes == ResultType.Result_OK)
                {
                    WSUserPreferences oUserPreferences = new WSUserPreferences(oParametersOut);
                    WSUserPlates oUserPlates = new WSUserPlates(oParametersOut);

                    string sLicenseTerms = (oParametersOut["legaltermsver"] != null ? oParametersOut["legaltermsver"].ToString() : "");
                    oParametersOut = new SortedList();
                    oRes = oWS.QueryCity("hbusque@integraparking.com", sSessionID, oCities.Keys.Last(), sLicenseTerms, ref oParametersOut);
                    if (oRes == ResultType.Result_OK)
                    {
                        WSZoneTar oZoneTar = new WSZoneTar(oParametersOut);
                        int iSector = oZoneTar.Zones[0].SubZones[0].Id;
                        int iTariff = oZoneTar.Tariffs.Where(t => t.SubZones.Contains(iSector)).FirstOrDefault().Id;

                        oParametersOut = new SortedList();
                        oRes = oWS.QueryParkingOperationWithTimeSteps("hbusque@integraparking.com", sSessionID, oUserPlates.Plates[0], DateTime.Now, iSector, iTariff, ref oParametersOut);
                        if (oRes == ResultType.Result_OK)
                        {
                            WSQueryParkingOperation oQueryParking = new WSQueryParkingOperation(oParametersOut);

                        }
                    }
                }
            }
            return View();
        }*/

        public ActionResult Index(string auth)
        {
            UserInfo oUserInfo = null;

            if (GetCredentials(auth) || SessionExist(out oUserInfo))
            {
                bool bCitySelected = false;
                if (oUserInfo != null) bCitySelected = (oUserInfo.CityId > 0);

                if (!bCitySelected)
                {
                    WSCities oCities = new WSCities();

                    WSIntegraMobile oWS = new WSIntegraMobile();
                    SortedList oParametersOut = new SortedList();
                    ResultType oRes = oWS.GetListOfCities(null, null, out oCities.Cities, ref oParametersOut);

                    if (oRes == ResultType.Result_OK)
                        return View("Cities", oCities);
                    else
                        return View("WSResponseError");
                }
                else
                    return StartCity(oUserInfo.CityId, oUserInfo.CityName);
            }
            else
                return View("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password, string CityId, string CityName, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                WSIntegraMobile oWS = new WSIntegraMobile();
                SortedList oParametersOut = new SortedList();

                string sSessionID = "";

                int iCityId = Convert.ToInt32(CityId);
                string sLanguage = ((CultureInfo)Session["Culture"]).Name;

                ResultType oRes = oWS.QueryLogin(Username, Password, sLanguage, true, iCityId, null, null, out sSessionID, ref oParametersOut);
                if (oRes == ResultType.Result_OK)
                {
                    GetCredentials(Username + "*" + Password + "*" + DateTime.Now.Ticks + "*" + sLanguage, false);

                    UserInfo oUserInfo = null;
                    if (CredentialsExist(out oUserInfo))
                    {
                        oUserInfo.SessionID = sSessionID;
                        oUserInfo.CityId = iCityId;
                        oUserInfo.CityName = CityName;
                        oUserInfo.UserPlates = new WSUserPlates(oParametersOut);
                        oUserInfo.UserPreferences = new WSUserPreferences(oParametersOut);
                        oUserInfo.Cur = oParametersOut.GetValueString("cur");
                        oUserInfo.Balance = oParametersOut.GetValueInt("bal");

                        oParametersOut = new SortedList();
                        oRes = oWS.QueryCity(oUserInfo.User, oUserInfo.SessionID, oUserInfo.CityId, oUserInfo.LegalTermsVersion, ref oParametersOut);
                        if (oRes == ResultType.Result_OK)
                        {
                            oUserInfo.ZoneTar = new WSZoneTar(oParametersOut);

                            ViewData["CityId"] = oUserInfo.CityId;

                            return View("Plates", oUserInfo.UserPlates);
                            //oUserInfo.CityId = 0;
                            //return StartCity(oUserInfo.CityId, oUserInfo.CityName);
                        }
                        else
                        {
                            ModelState.AddModelError("", Resources.ParkController_Login_InvalidUsernameOrPassword);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.ParkController_Login_InvalidUsernameOrPassword);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.ParkController_Login_InvalidUsernameOrPassword);
                }

            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        public ActionResult Logout()
        {
            ClearSession();

            return Index(null);
        }

        public ActionResult Cities_Read()
        {
            
            WSCities oCities = new WSCities();

            WSIntegraMobile oWS = new WSIntegraMobile();
            SortedList oParametersOut = new SortedList();
            ResultType oRes = oWS.GetListOfCities(null, null, out oCities.Cities, ref oParametersOut);

            if (oRes == ResultType.Result_OK)
            {

            }

            return Json( oCities.Cities.Keys.Select(id => new { Id = id, Description = oCities.Cities[id] }).ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult StartCity(int id, string name)
        {
            int iCityId = id;

            if (iCityId > 0)
            {
                ViewData["CityId"] = iCityId;

                UserInfo oUserInfo;

                WSIntegraMobile oWS = new WSIntegraMobile();
                SortedList oParametersOut = new SortedList();
                ResultType oRes;

                if (SessionExist(out oUserInfo))
                {
                    if (iCityId != oUserInfo.CityId)
                    {
                        oUserInfo.CityId = iCityId;
                        oUserInfo.CityName = name;
                        oRes = oWS.QueryCity(oUserInfo.User, oUserInfo.SessionID, oUserInfo.CityId, oUserInfo.LegalTermsVersion, ref oParametersOut);
                        if (oRes == ResultType.Result_OK)
                        {
                            oUserInfo.ZoneTar = new WSZoneTar(oParametersOut);
                            return View("Plates", oUserInfo.UserPlates);
                        }
                        else
                            return View("WSResponseError");
                    }
                    return View("Plates", oUserInfo.UserPlates);
                }
                else if (CredentialsExist(out oUserInfo))
                {
                    oUserInfo.CityId = iCityId;
                    oUserInfo.CityName = name;

                    oRes = oWS.QueryLogin(oUserInfo.User, oUserInfo.Pwd, oUserInfo.Language, true, oUserInfo.CityId, null, null, out oUserInfo.SessionID, ref oParametersOut);
                    if (oRes == ResultType.Result_OK)
                    {                       
                        oUserInfo.UserPlates = new WSUserPlates(oParametersOut);
                        oUserInfo.UserPreferences = new WSUserPreferences(oParametersOut);
                        oUserInfo.Cur = oParametersOut.GetValueString("cur");
                        oUserInfo.Balance = oParametersOut.GetValueInt("bal");

                        oParametersOut = new SortedList();
                        oRes = oWS.QueryCity(oUserInfo.User, oUserInfo.SessionID, oUserInfo.CityId, oUserInfo.LegalTermsVersion, ref oParametersOut);
                        if (oRes == ResultType.Result_OK)
                        {
                            oUserInfo.ZoneTar = new WSZoneTar(oParametersOut);

                            //SaveUserInfo(oUserInfo);

                            return View("Plates", oUserInfo.UserPlates);
                        }
                        else
                            return View("WSResponseError");
                    }
                    else
                    {
                        return View("WSResponseError");
                    }
                }
                else
                    return View("Error");

            }
            else
                return Index("");

        }

        public ActionResult PlateSelected(string id)
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                //ViewData["CityId"] = cityId;

                if (oUserInfo.UserPlates.Plates.Contains(id))
                {
                    oUserInfo.Plate = id;

                    WSFavArea oFavArea = oUserInfo.UserPreferences.GetFavArea(oUserInfo.CityId, oUserInfo.ZoneTar.Zones);
                    if (oFavArea != null)
                        return View("SelectorFav", oFavArea);
                    else
                        return Zones();

                }
                else
                    return View("Plates", oUserInfo.UserPlates);

            }
            else
                return View("Login");

        }

        public ActionResult Zones()
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                if (oUserInfo.ZoneTar.Zones.Count > 1)
                {                    
                    return View("Zones", oUserInfo.ZoneTar);
                }
                else if (oUserInfo.ZoneTar.Zones.Count == 1)
                {
                    if (oUserInfo.ZoneTar.Zones[0].SubZones.Count == 1)
                        return ZoneSelected(oUserInfo.ZoneTar.Zones[0].SubZones[0].Id);
                    else if (oUserInfo.ZoneTar.Zones[0].SubZones.Count == 0)
                        return ZoneSelected(oUserInfo.ZoneTar.Zones[0].Id);
                    else
                        return View("Zones", oUserInfo.ZoneTar);
                }
                else
                    return View("Error");
            }
            else
                return View("Login");
        }

        public ActionResult ZoneSelected(int groupId)
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                ViewData["GroupId"] = groupId;
                oUserInfo.GroupId = groupId;

                var oTariffs = oUserInfo.ZoneTar.Tariffs.Where(t => t.SubZones.Contains(groupId)).ToList();
                if (oTariffs.Count > 1)
                {
                    return View("Tariffs", oTariffs);
                }
                else if (oTariffs.Count == 1)
                    return QueryParking(groupId, oTariffs[0].Id);
                else
                    return View("Error");
            }
            else
                return View("Login");
        }



        public ActionResult QueryParking(int groupId, int tariffId)
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                oUserInfo.TariffId = tariffId;
                oUserInfo.ParkConfirmed = false;

                WSIntegraMobile oWS = new WSIntegraMobile();
                SortedList oParametersOut = new SortedList();

                ResultType oRes = oWS.QueryParkingOperationWithTimeSteps(oUserInfo.User, oUserInfo.SessionID, oUserInfo.Plate, DateTime.Now, groupId, tariffId, ref oParametersOut);
                if (oRes == ResultType.Result_OK)
                {
                    oUserInfo.QueryParkingOperation = new WSQueryParkingOperation(oParametersOut);
  
                    if (oUserInfo.QueryParkingOperation.ForceDisp)
                        return View("Occupation", oUserInfo);
                    else
                        return View("QueryParking", oUserInfo);
                }
                else
                    return View("WSResponseError", oRes);
            }
            else
                return View("Login");
        }

        public ActionResult QueryParkingOccupation(string madtarinfo)
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                oUserInfo.MadTarInfo = (madtarinfo == "on");
                return View("QueryParking", oUserInfo);
            }
            else
                return View("Login");
        }

        public ActionResult ConfirmParking(string stepindex)
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                int iStepIndex = -1;
                try {
                    iStepIndex = Convert.ToInt32(stepindex);
                }
                catch (Exception ex) {}

                if (iStepIndex >= 0)
                {
                    oUserInfo.StepIndex = iStepIndex;

                    return View("ConfirmParking", oUserInfo);
                }
                else
                    return View("Error");
            }
            else
                return View("Login");
        }

        public ActionResult TicketParking()
        {
            UserInfo oUserInfo = null;

            if (SessionExist(out oUserInfo))
            {
                if (oUserInfo.StepIndex >= 0)
                {
                    WSIntegraMobile oWS = new WSIntegraMobile();
                    SortedList oParametersOut = new SortedList();

                    var oCurrentStep = oUserInfo.CurrentStep();

                    if (!oUserInfo.ParkConfirmed)
                    {
                        ResultType oRes = oWS.ConfirmParkingOperation(oUserInfo.User, oUserInfo.SessionID, oUserInfo.Plate, DateTime.Now, oUserInfo.GroupId, oUserInfo.TariffId, oCurrentStep.Q, oCurrentStep.QFee, oCurrentStep.QVat, oCurrentStep.QTotal, oCurrentStep.T, oUserInfo.QueryParkingOperation.InitialDate, oCurrentStep.D, oUserInfo.MadTarInfo, ref oParametersOut);
                        if (oRes == ResultType.Result_OK)
                        {
                            oUserInfo.Balance = oParametersOut.GetValueInt("newbal", oUserInfo.Balance);
                            oUserInfo.ParkConfirmed = true;

                            return View("ConfirmParking", oUserInfo);
                        }
                        else
                            return View("WSResponseError", oRes);
                    }
                    else
                        return View("ConfirmParking", oUserInfo);
                }
                else
                    return View("Error");
            }
            else
                return View("Login");
        }

        #endregion

        #region Private Methods

        private bool GetCredentials(string sAuth, bool bEncrypted = true)
        {
            bool bRet = false;

            try
            {

                if (!string.IsNullOrEmpty(sAuth))
                {
                    string sDescryptAuth = sAuth;
                    if (bEncrypted)
                        sDescryptAuth = Encryptor.Decrypt(sAuth, ConfigurationManager.AppSettings["EncryptorKey"].ToString());

                    var arr = sDescryptAuth.Split('*');
                    if (arr.Length >= 3)
                    {
                        string sUser = arr[0];
                        string sPwd = arr[1];
                        string sUtcTicks = arr[2];

                        if (arr.Length >= 4)
                        {
                            this.Session["Culture"] = new CultureInfo(arr[3]);
                        }

                        DateTime dt = new DateTime(Convert.ToInt64(sUtcTicks));

                        // ***
                        if (dt.AddSeconds(60) >= DateTime.UtcNow)
                        {
                            UserInfo oUserInfo;
                            //if (Session["UserInfo"] != null)
                            //    oUserInfo = (UserInfo)Session["UserInfo"];
                            //else
                                oUserInfo = new UserInfo();
                            oUserInfo.User = sUser;
                            oUserInfo.Pwd = sPwd;
                            oUserInfo.Language = ((CultureInfo)Session["Culture"]).Name;
                            Session["UserInfo"] = oUserInfo;

                            bRet = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                bRet = false;
            }

            return bRet;
        }

        private bool SessionExist(out UserInfo oUserInfo)
        {
            bool bRet = false;
            oUserInfo = null;

            if (Session["UserInfo"] != null)
            {
                oUserInfo = (UserInfo)Session["UserInfo"];
                bRet = (!string.IsNullOrEmpty(oUserInfo.SessionID) &&
                        !string.IsNullOrEmpty(oUserInfo.User) &&
                        oUserInfo.UserPlates != null &&
                        oUserInfo.UserPreferences != null);

            }

            return bRet;
        }

        private bool CredentialsExist(out UserInfo oUserInfo)
        {
            bool bRet = false;
            oUserInfo = null;

            if (Session["UserInfo"] != null)
            {
                oUserInfo = (UserInfo)Session["UserInfo"];
                bRet = (!string.IsNullOrEmpty(oUserInfo.User) &&
                        !string.IsNullOrEmpty(oUserInfo.Pwd));

            }
            
            return bRet;
        }

        private void ClearSession()
        {
            Session.Remove("UserInfo");
        }

        private void SaveUserInfo(UserInfo oUserInfo)
        {
            Session["UserInfo"] = oUserInfo;
        }
        
        /*private bool GetCredentials(out string sUser, out sSessionID, out string sPwd, out WSUserPlates oUserPlates, out WSUserPreferences oUserPreferences)
        {


        }*/

        #endregion

    }
}
