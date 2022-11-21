using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Ninject;



namespace InvoiceGenerator
{

    class InvoiceGenerator
    {
        #region -- Constant definitions --


        const System.String ct_INVOICE_PERIOD_TAG = "InvoiceGenerationPeriod";
        const System.String ct_DEFAULT_OPERATOR_ID_TAG = "DefaultOperatorID";

       
        #endregion

        #region -- Member Variables --     
  
        private IKernel m_kernel = null;

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }
        [Inject]
        public IGeograficAndTariffsRepository geograficAndTariffsRepository { get; set; }

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(InvoiceGenerator));


        // Invoice Period Type Generation
        private InvoicePeriodType m_iInvoiceGenerationPeriod;
        private int m_iDefaultOperatorID;
        

        #endregion

		#region -- Constructor / Destructor --

        public InvoiceGenerator()
		{
            m_iInvoiceGenerationPeriod = (InvoicePeriodType)Convert.ToInt32(ConfigurationManager.AppSettings[ct_INVOICE_PERIOD_TAG].ToString());
            m_iDefaultOperatorID = Convert.ToInt32(ConfigurationManager.AppSettings[ct_DEFAULT_OPERATOR_ID_TAG].ToString());

            m_kernel = new StandardKernel(new InvoiceGeneratorModule());
            m_kernel.Inject(this);
         
        }


        
        #endregion 

        #region -- Threads Bodies --

        public void Main()
		{
            m_Log.LogMessage(LogLevels.logDEBUG, ">> InvoiceGenerator::Main");

            DateTime dtNow = DateTime.Now;
            int iNumOperations=0;
            customersRepository.GenerateInvoicesForRecharges(dtNow.AddMinutes(-5), out iNumOperations);
            m_Log.LogMessage(LogLevels.logDEBUG, string.Format("InvoiceGenerator::GenerateInvoicesForRecharges. Recharges updated {0}",
                                                                iNumOperations));
            customersRepository.GenerateInvoicesForTicketPayments(dtNow.AddMinutes(-5), out iNumOperations);
            m_Log.LogMessage(LogLevels.logDEBUG, string.Format("InvoiceGenerator::GenerateInvoicesForTicketPayments. Ticket Payments updated {0}",
                                                                iNumOperations));
            customersRepository.GenerateInvoicesForOperations(dtNow.AddMinutes(-5), out iNumOperations);
            m_Log.LogMessage(LogLevels.logDEBUG, string.Format("InvoiceGenerator::GenerateInvoicesForOperations. Operations updated {0}",
                                                                iNumOperations));


            DateTime dtLastDateToInvoice = dtNow;

            switch (m_iInvoiceGenerationPeriod)
            {
                case InvoicePeriodType.Weekly:

                    
                    DateTime dtLastWeek = dtNow.AddDays(-7);
                    int iLastWeekNumber = DateHelpers.GetIso8601WeekOfYear(dtLastWeek);
                    dtLastDateToInvoice = DateHelpers.FirstDateOfWeek(dtLastWeek.Year, iLastWeekNumber).AddDays(7);
                    m_Log.LogMessage(LogLevels.logDEBUG, string.Format("InvoiceGenerator::Last day to invoice: {0:dd/MM/yyyy HH:mm:ss}", dtLastDateToInvoice));

                    break;

                case InvoicePeriodType.Monthly:
                        
                    dtLastDateToInvoice = new DateTime(dtNow.Year, dtNow.Month, 1);
                    m_Log.LogMessage(LogLevels.logDEBUG, string.Format("InvoiceGenerator::Last day to invoice: {0:dd/MM/yyyy HH:mm:ss}", dtLastDateToInvoice));

                    break;

                default:
                    break;
            }

              

            int iNumInvoices = 0;

            if (customersRepository.GenerateInvoices(dtLastDateToInvoice, out  iNumInvoices))
            {
                m_Log.LogMessage(LogLevels.logDEBUG, string.Format("InvoiceGenerator::End of generation of Invoices. End Date of generation: {0:dd/MM/yyyy HH:mm:ss}. {1} new invoices generated",
                                                                    dtLastDateToInvoice, iNumInvoices));
            }
            else
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("InvoiceGenerator::Error in the  generation of Invoices. End Date of generation: {0:dd/MM/yyyy HH:mm:ss}",
                                                                    dtLastDateToInvoice));
            }                                 
            
            
            m_Log.LogMessage(LogLevels.logDEBUG, "<< InvoiceGenerator::Main");
        }




		#endregion



    }
}
