using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using UIShell.OSGi;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;
using System.Configuration;
using backOffice.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using backOffice.Infrastructure.Maintenances;
using backOffice.Infrastructure.Security;
using PIC.Infrastructure.Logging;
using SecurityPlugin.Security;


namespace PBPPlugin
{
    public class Activator : IBundleActivator, IPluginActivator
    {
        public const string PluginName = "PBPPlugin";

        private readonly static CLogWrapper m_Log = new CLogWrapper(typeof(Activator));

        public void Start(IBundleContext context)
        {
            ResourceBundle resBundle = ResourceBundle.GetInstance();
            resBundle.LocaleRoot = "pbpplugin.Locale";
            resBundle.AddResourceFile("PBPPlugin");
            resBundle.AddResourceFile("Maintenance");
            resBundle.AddResourceFile("Security");

            Assembly assembly = Assembly.GetCallingAssembly();
            Console.WriteLine(assembly.FullName);
        }

        public void Stop(IBundleContext context)
        {
            
        }

        public static void Initialize()
        {
            //InitializeMaintenanceDefinitions();
            //MaintenancePlugin.Models.MaintenanceFactory.CreateDataRepository = () => { return new integraMobile.Domain.NH.Concrete.SQLBaseRepository(typeof(integraMobile.Domain.NH.Concrete.SQLBaseRepository), true); };
            // Verify installations. Syncronize installation between dbPIC and integraMobile databases.
            SyncInstallations();
            InitializeSecurity();
        }

        public static int Priority()
        {
            return 98;
        }

        public static void InjectController(BindingRoot oBindingRoot)
        {
            oBindingRoot.Bind<ICustomersRepository>()
                            .To<SQLCustomersRepository>()
                            .WithConstructorArgument("connectionString",
                                ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );

            oBindingRoot.Bind<IInfraestructureRepository>()
                            .To<SQLInfraestructureRepository>()
                            .WithConstructorArgument("connectionString",
                                ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );

            oBindingRoot.Bind<IBackOfficeRepository>()
                            .To<SQLBackOfficeRepository>()
                            .WithConstructorArgument("connectionString",
                                ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );

            oBindingRoot.Bind<IGeograficAndTariffsRepository>()
                            .To<SQLGeograficAndTariffsRepository>()
                            .WithConstructorArgument("connectionString",
                                ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );

        }

        public static PIC.Domain.Abstract.IBaseRepository CreateDataRepository(string sConnectionString)
        {
            return new integraMobile.Domain.NH.Concrete.SQLBaseRepository(typeof(integraMobile.Domain.NH.Concrete.SQLBaseRepository), sConnectionString, true);
        }

        #region Initialize Maintenances

        private static bool InitializeMaintenanceDefinitions()
        {
            bool bRet = false;

            bRet = MB_Users();

            return bRet;
        }

        private static bool MB_Users()
        {
            bool bRet = false;

            List<MaintenanceField> fields = new List<MaintenanceField>();
            fields.Add(new MaintenanceField()
            {
                Name = "Id",
                Mapping = "UsrId",                
                Type = MaintenancePlugin.Models.MaintenanceFieldDataType.Integer,
                IsPK = true,
                Readonly = false,
                /*FkMaintenance = null,
                FkMapping = null,
                FloatDecimals = null,
                CurrencyId = null,
                CurrencyFieldMapping = null,*/
                Validators = "{Required:true,MaxLength:null,MinLength:null,RegExp:null}"
            });
            //1,'Id','UniId',2,1,0,NULL,NULL,NULL,NULL,NULL, '{Required:true,MaxLength:null,MinLength:null,RegExp:null}'

            fields.Add(new MaintenanceField()
            {
                Name = "Email",
                Mapping = "UsrEmail",
                Type = MaintenancePlugin.Models.MaintenanceFieldDataType.Text,
                IsPK = false,
                Readonly = false,
                Validators = "{Required:false,MaxLength:50,MinLength:null,RegExp:null}"
            });
            //1,'Description','UniDescription',1,0,0,NULL,NULL,NULL,NULL,NULL, '{Required:false,MaxLength:50,MinLength:null,RegExp:null}'

            Maintenance oMaintenance = null;
            bRet = Maintenance.Create(2, "Users", "User", "integraMobile.Domain.NH.Entities.{0}, integraMobile.Domain.NH", "Email", "Units", backOffice.Infrastructure.Security.AccessLevel.Read, fields, out oMaintenance);
            //1, 'Units', 'Unit', 'Description',0,9

            return bRet;

        }

        #endregion

        #region Initialize Security

        private static bool InitializeSecurity()
        {
            bool bRet = false;

            Feature oFeatureGroup = null;
            Feature oFeatureSubGroup = null;
            Feature oFeature = null;
            
            List<string> oOldAllRoles = FormAuthMemberShip.MembershipService.GetAllRoles().ToList();

            if (Feature.CreateFeature("Production", "Production", null, "PBPPlugin", out oFeatureGroup))
            {
                Feature.CreateFeature("OperationsGroup", "Operations", oFeatureGroup, "PBPPlugin", out oFeatureSubGroup);
                Feature.CreateFeature("CurrOperations", "CurrOperations", "", "", "CURROPERATIONS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("Operations", "Operations", "", "", "OPERATIONS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("HisOperations", "HisOperations", "", "", "HISOPERATIONS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("HisOperationsRestricted", "HisOperationsRestricted", "", "", "HISOPERATIONSRESTRICTED_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("TicketPayments", "TicketPayments", "", "", "TICKETPAYMENTS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("Recharges", "Recharges", "", "", "RECHARGES_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("OperationsNoRecharges", "Operations No Recharges", "", "", "OPERATIONSNORECHARGES_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("OperationsDelete", "Operations Deletion", "", "", "OPERATIONS_DELETE", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("ExternalOperations", "ExternalOperations", "", "", "EXTERNALOPERATIONS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("SecurityOperations", "SecurityOperations", "", "", "SECURITYOPERATIONS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("BalanceTransfers", "BalanceTransfers", "", "BALANCETRANSFERS_WRITE", "BALANCETRANSFERS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("Invoices", "Invoices", "", "", "INVOICES_READ", oFeatureGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("UsersDisable", "Users Disable", "", "", "USERS_DISABLE", oFeatureGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("EmailTool", "EmailTool", "", "", "EMAILTOOL_READ", oFeatureGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("CashRechargeTool", "CashRechargeTool", "", "", "CASHRECHARGETOOL_READ", oFeatureGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("Dashboard", "Dashboard", "", "", "DASHBOARD_READ", oFeatureGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("FinantialReports", "Finantial Reports", oFeatureGroup, "PBPPlugin", out oFeatureSubGroup);
                Feature.CreateFeature("FinantialReports.Deposits", "Deposits Report", "", "", "FRDEPOSITS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.Deposits2", "Deposits2 Report", "", "", "FRDEPOSITSOPERATOR_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.Deposits3", "Deposits3 Report", "", "", "FRDEPOSITSCOUNCIL_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.LiquidationDetail", "Liquidation Detail Report", "", "", "FRLIQUIDATIOND_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.Bank", "Bank Report", "", "", "FRBANK_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.GeneralData", "General Data Report", "", "", "FRGENERALDATA_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.GeneralDataInstallation", "General Data Report for Installation", "", "", "FRGENERALDATAINST_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("FinantialReports.RegisteredUsers", "Registered Users", "", "", "FRREGISTEREDUSERS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("UsersGroup", "Users Group", oFeatureGroup, "PBPPlugin", out oFeatureSubGroup);
                Feature.CreateFeature("Users", "Users", "", "", "USERS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("Inscriptions", "Inscriptions", "", "", "INSCRIPTIONS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("UserFriends", "Invitations", "", "", "USERFRIENDS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("UserPushIds", "Devices", "", "", "USERPUSHIDS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);
                Feature.CreateFeature("UserShopkeepers", "Shopkeeper Users", "", "USERSHOPKEEPERS_WRITE", "USERSHOPKEEPERS_READ", oFeatureSubGroup, "PBPPlugin", out oFeature);

                Feature.CreateFeature("RechargeCoupons", "RechargeCoupons", "", "", "RECHARGECOUPONS_READ", oFeatureGroup, "PBPPlugin", out oFeature);

            }

            return bRet;
        }

        #endregion

        public static bool SyncInstallations()
        {
            bool bRet = false;

            PIC.Domain.Abstract.IPICTransaction oTransaction = null;

            try
            {
                var oRepository = CreateDataRepository("");

                PIC.Domain.Abstract.IBackOfficeRepository oBackOfficeRepository = new PIC.Domain.Concrete.SQLBackOfficeRepository(true);

                oTransaction = oBackOfficeRepository.CreateTransaction();
                oTransaction.BeginTransaction();

                foreach (var oIntegraInstallation in oRepository.GetQuery(typeof(integraMobile.Domain.NH.Entities.Installation)).Cast<integraMobile.Domain.NH.Entities.Installation>().Where(i => i.InsEnabled == 1))
                {
                    var oPICInstallation = oBackOfficeRepository.GetInstallation(oIntegraInstallation.InsId, oTransaction);
                    if (oPICInstallation == null)
                    {
                        oPICInstallation = new PIC.Domain.Entities.Installation();
                        oPICInstallation.InsId = oIntegraInstallation.InsId;
                        oPICInstallation.InsDescription = oIntegraInstallation.InsDescription;
                        oPICInstallation.InsShortdesc = oIntegraInstallation.InsShortdesc;
                        oPICInstallation.InsCou = oBackOfficeRepository.GetQuery(typeof(PIC.Domain.Entities.Country), oTransaction).Cast<PIC.Domain.Entities.Country>().Where(c => c.CouDescription == "Spain").FirstOrDefault();
                        oPICInstallation.InsCur = oBackOfficeRepository.GetCurrency(oIntegraInstallation.InsCurId, oTransaction);
                        oPICInstallation.InsTimezoneId = oIntegraInstallation.InsTimezoneId;

                        bRet = oBackOfficeRepository.Save(oPICInstallation, oTransaction);
                        if (!bRet) break;
                    }
                }

            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SyncInstallations", ex);
            }
            finally
            {
                if (oTransaction != null) oTransaction.FinishTransaction(bRet);
            }

            return bRet;
        }

    }
}