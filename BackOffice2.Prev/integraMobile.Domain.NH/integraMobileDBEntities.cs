using System;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Validator.Constraints;

namespace integraMobile.Domain.NH.Entities
{
  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Country
  {
    public virtual decimal CouId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CouDescription { get; set; }
    [Length(Max=10)]
    public virtual string CouCode { get; set; }
    [Length(Max=10)]
    public virtual string CouTelPrefix { get; set; }
    public virtual System.Nullable<decimal> CouCurId { get; set; }

    private IList<CustomerInscription> _customerInscriptionsByCusinsMainTelCountry = new List<CustomerInscription>();

    public virtual IList<CustomerInscription> CustomerInscriptionsByCusinsMainTelCountry
    {
      get { return _customerInscriptionsByCusinsMainTelCountry; }
      set { _customerInscriptionsByCusinsMainTelCountry = value; }
    }

    private IList<CustomerInscription> _customerInscriptionsByCusinsSecundTelCountry = new List<CustomerInscription>();

    public virtual IList<CustomerInscription> CustomerInscriptionsByCusinsSecundTelCountry
    {
      get { return _customerInscriptionsByCusinsSecundTelCountry; }
      set { _customerInscriptionsByCusinsSecundTelCountry = value; }
    }

    private IList<Customer> _customers = new List<Customer>();

    public virtual IList<Customer> Customers
    {
      get { return _customers; }
      set { _customers = value; }
    }

    private IList<User> _usersByUsrCou = new List<User>();

    public virtual IList<User> UsersByUsrCou
    {
      get { return _usersByUsrCou; }
      set { _usersByUsrCou = value; }
    }

    private IList<User> _usersByUsrMainTelCountry = new List<User>();

    public virtual IList<User> UsersByUsrMainTelCountry
    {
      get { return _usersByUsrMainTelCountry; }
      set { _usersByUsrMainTelCountry = value; }
    }

    private IList<User> _usersByUsrSecundTelCountry = new List<User>();

    public virtual IList<User> UsersByUsrSecundTelCountry
    {
      get { return _usersByUsrSecundTelCountry; }
      set { _usersByUsrSecundTelCountry = value; }
    }

    private IList<UsersSecurityOperation> _usersSecurityOperations = new List<UsersSecurityOperation>();

    public virtual IList<UsersSecurityOperation> UsersSecurityOperations
    {
      get { return _usersSecurityOperations; }
      set { _usersSecurityOperations = value; }
    }

    public virtual Currency CouCur { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Country).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Country'
         table='`COUNTRIES`'
         >
    <id name='CouId'
        column='`COU_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CouDescription'
              column='`COU_DESCRIPTION`'
              />
    <property name='CouCode'
              column='`COU_CODE`'
              />
    <property name='CouTelPrefix'
              column='`COU_TEL_PREFIX`'
              />
    <property name='CouCurId'
              column='`COU_CUR_ID`'
              />
    <bag name='CustomerInscriptionsByCusinsMainTelCountry'
          inverse='true'
          >
      <key column='`CUSINS_MAIN_TEL_COUNTRY`' />
      <one-to-many class='CustomerInscription' />
    </bag>
    <bag name='CustomerInscriptionsByCusinsSecundTelCountry'
          inverse='false'
          >
      <key column='`CUSINS_SECUND_TEL_COUNTRY`' />
      <one-to-many class='CustomerInscription' />
    </bag>
    <bag name='Customers'
          inverse='true'
          >
      <key column='`CUS_COU_ID`' />
      <one-to-many class='Customer' />
    </bag>
    <bag name='UsersByUsrCou'
          inverse='true'
          >
      <key column='`USR_COU_ID`' />
      <one-to-many class='User' />
    </bag>
    <bag name='UsersByUsrMainTelCountry'
          inverse='true'
          >
      <key column='`USR_MAIN_TEL_COUNTRY`' />
      <one-to-many class='User' />
    </bag>
    <bag name='UsersByUsrSecundTelCountry'
          inverse='false'
          >
      <key column='`USR_SECUND_TEL_COUNTRY`' />
      <one-to-many class='User' />
    </bag>
    <bag name='UsersSecurityOperations'
          inverse='false'
          >
      <key column='`USOP_NEW_MAIN_TEL_COUNTRY`' />
      <one-to-many class='UsersSecurityOperation' />
    </bag>
    <many-to-one name='CouCur' class='Currency' column='`COU_CUR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UsersSecurityOperation
  {
    public virtual decimal UsopId { get; set; }
    public virtual int UsopOpType { get; set; }
    public virtual int UsopStatus { get; set; }
    public virtual System.DateTime UsopUtcdatetime { get; set; }
    public virtual decimal UsopUsrId { get; set; }
    public virtual decimal UsopActivationRetries { get; set; }
    public virtual System.Nullable<System.DateTime> UsopLastSentDate { get; set; }
    public virtual System.Nullable<decimal> UsopNewMainTelCountryId { get; set; }
    [Length(Max=50)]
    public virtual string UsopNewMainTel { get; set; }
    [Length(Max=50)]
    public virtual string UsopNewEmail { get; set; }
    [Length(Max=100)]
    public virtual string UsopUrlParameter { get; set; }
    [Length(Max=20)]
    public virtual string UsopActivationCode { get; set; }

    public virtual Country UsopNewMainTelCountry { get; set; }

    public virtual User UsopUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UsersSecurityOperation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UsersSecurityOperation'
         table='`USERS_SECURITY_OPERATIONS`'
         >
    <id name='UsopId'
        column='`USOP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsopOpType'
              column='`USOP_OP_TYPE`'
              />
    <property name='UsopStatus'
              column='`USOP_STATUS`'
              />
    <property name='UsopUtcdatetime'
              column='`USOP_UTCDATETIME`'
              />
    <property name='UsopUsrId'
              column='`USOP_USR_ID`'
              />
    <property name='UsopActivationRetries'
              column='`USOP_ACTIVATION_RETRIES`'
              />
    <property name='UsopLastSentDate'
              column='`USOP_LAST_SENT_DATE`'
              />
    <property name='UsopNewMainTelCountryId'
              column='`USOP_NEW_MAIN_TEL_COUNTRY`'
              />
    <property name='UsopNewMainTel'
              column='`USOP_NEW_MAIN_TEL`'
              />
    <property name='UsopNewEmail'
              column='`USOP_NEW_EMAIL`'
              />
    <property name='UsopUrlParameter'
              column='`USOP_URL_PARAMETER`'
              />
    <property name='UsopActivationCode'
              column='`USOP_ACTIVATION_CODE`'
              />
    <many-to-one name='UsopNewMainTelCountry' class='Country' column='`USOP_NEW_MAIN_TEL_COUNTRY`' />
    <many-to-one name='UsopUsr' class='User' column='`USOP_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Currency
  {
    public virtual decimal CurId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CurName { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string CurIsoCode { get; set; }
    [Length(Max=10)]
    public virtual string CurSymbol { get; set; }
    public virtual System.Nullable<double> CurFact { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string CurIsoCodeNum { get; set; }
    public virtual System.Nullable<int> CurMinorUnit { get; set; }

    private IList<Country> _countries = new List<Country>();

    public virtual IList<Country> Countries
    {
      get { return _countries; }
      set { _countries = value; }
    }

    private IList<CurrencyRechargeValue> _currencyRechargeValues = new List<CurrencyRechargeValue>();

    public virtual IList<CurrencyRechargeValue> CurrencyRechargeValues
    {
      get { return _currencyRechargeValues; }
      set { _currencyRechargeValues = value; }
    }

    private IList<CustomerInvoice> _customerInvoices = new List<CustomerInvoice>();

    public virtual IList<CustomerInvoice> CustomerInvoices
    {
      get { return _customerInvoices; }
      set { _customerInvoices = value; }
    }

    private IList<CustomerPaymentMean> _customerPaymentMeans = new List<CustomerPaymentMean>();

    public virtual IList<CustomerPaymentMean> CustomerPaymentMeans
    {
      get { return _customerPaymentMeans; }
      set { _customerPaymentMeans = value; }
    }

    private IList<Operation> _operationsByOpeAmountCur = new List<Operation>();

    public virtual IList<Operation> OperationsByOpeAmountCur
    {
      get { return _operationsByOpeAmountCur; }
      set { _operationsByOpeAmountCur = value; }
    }

    private IList<Operation> _operationsByOpeBalanceCur = new List<Operation>();

    public virtual IList<Operation> OperationsByOpeBalanceCur
    {
      get { return _operationsByOpeBalanceCur; }
      set { _operationsByOpeBalanceCur = value; }
    }

    private IList<OperationsDiscount> _operationsDiscountsByOpedisAmountCur = new List<OperationsDiscount>();

    public virtual IList<OperationsDiscount> OperationsDiscountsByOpedisAmountCur
    {
      get { return _operationsDiscountsByOpedisAmountCur; }
      set { _operationsDiscountsByOpedisAmountCur = value; }
    }

    private IList<OperationsDiscount> _operationsDiscountsByOpedisBalanceCur = new List<OperationsDiscount>();

    public virtual IList<OperationsDiscount> OperationsDiscountsByOpedisBalanceCur
    {
      get { return _operationsDiscountsByOpedisBalanceCur; }
      set { _operationsDiscountsByOpedisBalanceCur = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreetsByOpeoffAmountCur = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreetsByOpeoffAmountCur
    {
      get { return _operationsOffstreetsByOpeoffAmountCur; }
      set { _operationsOffstreetsByOpeoffAmountCur = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreetsByOpeoffBalanceCur = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreetsByOpeoffBalanceCur
    {
      get { return _operationsOffstreetsByOpeoffBalanceCur; }
      set { _operationsOffstreetsByOpeoffBalanceCur = value; }
    }

    private IList<PaymentSubtype> _paymentSubtypes = new List<PaymentSubtype>();

    public virtual IList<PaymentSubtype> PaymentSubtypes
    {
      get { return _paymentSubtypes; }
      set { _paymentSubtypes = value; }
    }

    private IList<PaymentType> _paymentTypes = new List<PaymentType>();

    public virtual IList<PaymentType> PaymentTypes
    {
      get { return _paymentTypes; }
      set { _paymentTypes = value; }
    }

    private IList<RechargeCoupon> _rechargeCoupons = new List<RechargeCoupon>();

    public virtual IList<RechargeCoupon> RechargeCoupons
    {
      get { return _rechargeCoupons; }
      set { _rechargeCoupons = value; }
    }

    private IList<ServiceCharge> _serviceChargesBySechAmountCur = new List<ServiceCharge>();

    public virtual IList<ServiceCharge> ServiceChargesBySechAmountCur
    {
      get { return _serviceChargesBySechAmountCur; }
      set { _serviceChargesBySechAmountCur = value; }
    }

    private IList<ServiceCharge> _serviceChargesBySechBalanceCur = new List<ServiceCharge>();

    public virtual IList<ServiceCharge> ServiceChargesBySechBalanceCur
    {
      get { return _serviceChargesBySechBalanceCur; }
      set { _serviceChargesBySechBalanceCur = value; }
    }

    private IList<TicketPayment> _ticketPaymentsByTipaAmountCur = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPaymentsByTipaAmountCur
    {
      get { return _ticketPaymentsByTipaAmountCur; }
      set { _ticketPaymentsByTipaAmountCur = value; }
    }

    private IList<TicketPayment> _ticketPaymentsByTipaBalanceCur = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPaymentsByTipaBalanceCur
    {
      get { return _ticketPaymentsByTipaBalanceCur; }
      set { _ticketPaymentsByTipaBalanceCur = value; }
    }

    private IList<User> _users = new List<User>();

    public virtual IList<User> Users
    {
      get { return _users; }
      set { _users = value; }
    }

    private IList<Installation> _installationsByInsCur = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsCur
    {
      get { return _installationsByInsCur; }
      set { _installationsByInsCur = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatAmountCur = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatAmountCur
    {
      get { return _balanceTransfersByBatAmountCur; }
      set { _balanceTransfersByBatAmountCur = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatDstBalanceCur = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatDstBalanceCur
    {
      get { return _balanceTransfersByBatDstBalanceCur; }
      set { _balanceTransfersByBatDstBalanceCur = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Currency).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Currency'
         table='`CURRENCIES`'
         >
    <id name='CurId'
        column='`CUR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CurName'
              column='`CUR_NAME`'
              />
    <property name='CurIsoCode'
              column='`CUR_ISO_CODE`'
              />
    <property name='CurSymbol'
              column='`CUR_SYMBOL`'
              />
    <property name='CurFact'
              column='`CUR_FACT`'
              />
    <property name='CurIsoCodeNum'
              column='`CUR_ISO_CODE_NUM`'
              />
    <property name='CurMinorUnit'
              column='`CUR_MINOR_UNIT`'
              />
    <bag name='Countries'
          inverse='false'
          >
      <key column='`COU_CUR_ID`' />
      <one-to-many class='Country' />
    </bag>
    <bag name='CurrencyRechargeValues'
          inverse='true'
          >
      <key column='`CURV_CUR_ID`' />
      <one-to-many class='CurrencyRechargeValue' />
    </bag>
    <bag name='CustomerInvoices'
          inverse='true'
          >
      <key column='`CUSINV_CUR_ID`' />
      <one-to-many class='CustomerInvoice' />
    </bag>
    <bag name='CustomerPaymentMeans'
          inverse='true'
          >
      <key column='`CUSPM_CUR_ID`' />
      <one-to-many class='CustomerPaymentMean' />
    </bag>
    <bag name='OperationsByOpeAmountCur'
          inverse='true'
          >
      <key column='`OPE_AMOUNT_CUR_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsByOpeBalanceCur'
          inverse='true'
          >
      <key column='`OPE_BALANCE_CUR_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsDiscountsByOpedisAmountCur'
          inverse='true'
          >
      <key column='`OPEDIS_AMOUNT_CUR_ID`' />
      <one-to-many class='OperationsDiscount' />
    </bag>
    <bag name='OperationsDiscountsByOpedisBalanceCur'
          inverse='true'
          >
      <key column='`OPEDIS_BALANCE_CUR_ID`' />
      <one-to-many class='OperationsDiscount' />
    </bag>
    <bag name='OperationsOffstreetsByOpeoffAmountCur'
          inverse='true'
          >
      <key column='`OPEOFF_AMOUNT_CUR_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='OperationsOffstreetsByOpeoffBalanceCur'
          inverse='true'
          >
      <key column='`OPEOFF_BALANCE_CUR_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='PaymentSubtypes'
          inverse='false'
          >
      <key column='`PAST_FIXED_FEE_CUR_ID`' />
      <one-to-many class='PaymentSubtype' />
    </bag>
    <bag name='PaymentTypes'
          inverse='false'
          >
      <key column='`PAT_FIXED_FEE_CUR_ID`' />
      <one-to-many class='PaymentType' />
    </bag>
    <bag name='RechargeCoupons'
          inverse='true'
          >
      <key column='`RCOUP_CUR_ID`' />
      <one-to-many class='RechargeCoupon' />
    </bag>
    <bag name='ServiceChargesBySechAmountCur'
          inverse='true'
          >
      <key column='`SECH_AMOUNT_CUR_ID`' />
      <one-to-many class='ServiceCharge' />
    </bag>
    <bag name='ServiceChargesBySechBalanceCur'
          inverse='true'
          >
      <key column='`SECH_BALANCE_CUR_ID`' />
      <one-to-many class='ServiceCharge' />
    </bag>
    <bag name='TicketPaymentsByTipaAmountCur'
          inverse='true'
          >
      <key column='`TIPA_AMOUNT_CUR_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <bag name='TicketPaymentsByTipaBalanceCur'
          inverse='true'
          >
      <key column='`TIPA_BALANCE_CUR_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <bag name='Users'
          inverse='true'
          >
      <key column='`USR_CUR_ID`' />
      <one-to-many class='User' />
    </bag>
    <bag name='InstallationsByInsCur'
          inverse='true'
          >
      <key column='`INS_CUR_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='BalanceTransfersByBatAmountCur'
          inverse='true'
          >
      <key column='`BAT_AMOUNT_CUR_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
    <bag name='BalanceTransfersByBatDstBalanceCur'
          inverse='true'
          >
      <key column='`BAT_DST_BALANCE_CUR_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class CurrencyRechargeValue
  {
    public virtual decimal CurvId { get; set; }
    public virtual decimal CurvCurId { get; set; }
    public virtual int CurvValueType { get; set; }
    public virtual int CurvValue { get; set; }

    public virtual Currency CurvCur { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(CurrencyRechargeValue).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='CurrencyRechargeValue'
         table='`CURRENCY_RECHARGE_VALUES`'
         >
    <id name='CurvId'
        column='`CURV_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CurvCurId'
              column='`CURV_CUR_ID`'
              />
    <property name='CurvValueType'
              column='`CURV_VALUE_TYPE`'
              />
    <property name='CurvValue'
              column='`CURV_VALUE`'
              />
    <many-to-one name='CurvCur' class='Currency' column='`CURV_CUR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class CustomerInscription
  {
    public virtual decimal CusinsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusinsName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusinsSurname1 { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusinsDocId { get; set; }
    public virtual decimal CusinsMainTelCountryId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusinsMainTel { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusinsEmail { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string CusinsActivationCode { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string CusinsUrlParameter { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string CuisinsCulture { get; set; }
    public virtual System.Nullable<decimal> CuisinsCusId { get; set; }
    public virtual System.Nullable<decimal> CusinsActivationRetries { get; set; }
    public virtual System.Nullable<System.DateTime> CusinsLastSentDate { get; set; }
    public virtual System.Nullable<decimal> CusinsSecundTelCountryId { get; set; }
    [Length(Max=50)]
    public virtual string CusinsSecundTel { get; set; }
    [Length(Max=50)]
    public virtual string CusinsSurname2 { get; set; }

    private IList<UsersEmail> _usersEmails = new List<UsersEmail>();

    public virtual IList<UsersEmail> UsersEmails
    {
      get { return _usersEmails; }
      set { _usersEmails = value; }
    }

    private IList<UsersSmss> _usersSmsses = new List<UsersSmss>();

    public virtual IList<UsersSmss> UsersSmsses
    {
      get { return _usersSmsses; }
      set { _usersSmsses = value; }
    }

    public virtual Country CusinsMainTelCountry { get; set; }

    public virtual Country CusinsSecundTelCountry { get; set; }

    public virtual Customer CuisinsCus { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(CustomerInscription).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='CustomerInscription'
         table='`CUSTOMER_INSCRIPTIONS`'
         >
    <id name='CusinsId'
        column='`CUSINS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CusinsName'
              column='`CUSINS_NAME`'
              />
    <property name='CusinsSurname1'
              column='`CUSINS_SURNAME1`'
              />
    <property name='CusinsDocId'
              column='`CUSINS_DOC_ID`'
              />
    <property name='CusinsMainTelCountryId'
              column='`CUSINS_MAIN_TEL_COUNTRY`'
              />
    <property name='CusinsMainTel'
              column='`CUSINS_MAIN_TEL`'
              />
    <property name='CusinsEmail'
              column='`CUSINS_EMAIL`'
              />
    <property name='CusinsActivationCode'
              column='`CUSINS_ACTIVATION_CODE`'
              />
    <property name='CusinsUrlParameter'
              column='`CUSINS_URL_PARAMETER`'
              />
    <property name='CuisinsCulture'
              column='`CUISINS_CULTURE`'
              />
    <property name='CuisinsCusId'
              column='`CUISINS_CUS_ID`'
              />
    <property name='CusinsActivationRetries'
              column='`CUSINS_ACTIVATION_RETRIES`'
              />
    <property name='CusinsLastSentDate'
              column='`CUSINS_LAST_SENT_DATE`'
              />
    <property name='CusinsSecundTelCountryId'
              column='`CUSINS_SECUND_TEL_COUNTRY`'
              />
    <property name='CusinsSecundTel'
              column='`CUSINS_SECUND_TEL`'
              />
    <property name='CusinsSurname2'
              column='`CUSINS_SURNAME2`'
              />
    <bag name='UsersEmails'
          inverse='false'
          >
      <key column='`USRE_CUSINS_ID`' />
      <one-to-many class='UsersEmail' />
    </bag>
    <bag name='UsersSmsses'
          inverse='false'
          >
      <key column='`USRS_CUSINS_ID`' />
      <one-to-many class='UsersSmss' />
    </bag>
    <many-to-one name='CusinsMainTelCountry' class='Country' column='`CUSINS_MAIN_TEL_COUNTRY`' />
    <many-to-one name='CusinsSecundTelCountry' class='Country' column='`CUSINS_SECUND_TEL_COUNTRY`' />
    <many-to-one name='CuisinsCus' class='Customer' column='`CUISINS_CUS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class CustomerInvoice
  {
    public virtual decimal CusinvId { get; set; }
    public virtual decimal CusinvCusId { get; set; }
    public virtual decimal CusinvOprId { get; set; }
    [Length(Max=50)]
    [NotNull]
    public virtual string CusinvInvNumber { get; set; }
    [NotNull]
    public virtual System.Nullable<System.DateTime> CusinvInvDate { get; set; }
    [NotNull]
    public virtual System.Nullable<System.DateTime> CusinvGenerationDate { get; set; }
    public virtual System.DateTime CusinvDateini { get; set; }
    public virtual System.DateTime CusinvDateend { get; set; }
    [NotNull]
    public virtual System.Nullable<decimal> CusinvOprInitialInvoiceNumber { get; set; }
    [NotNull]
    public virtual System.Nullable<decimal> CusinvOprEndInvoiceNumber { get; set; }
    public virtual int CusinvInvAmount { get; set; }
    public virtual decimal CusinvCurId { get; set; }
    public virtual int CusinvInvoiceVersion { get; set; }
    public virtual System.Nullable<int> CusinvInvAmountOps { get; set; }

    private IList<CustomerPaymentMeansRecharge> _customerPaymentMeansRecharges = new List<CustomerPaymentMeansRecharge>();

    public virtual IList<CustomerPaymentMeansRecharge> CustomerPaymentMeansRecharges
    {
      get { return _customerPaymentMeansRecharges; }
      set { _customerPaymentMeansRecharges = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<OperationsDiscount> _operationsDiscounts = new List<OperationsDiscount>();

    public virtual IList<OperationsDiscount> OperationsDiscounts
    {
      get { return _operationsDiscounts; }
      set { _operationsDiscounts = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<ServiceCharge> _serviceCharges = new List<ServiceCharge>();

    public virtual IList<ServiceCharge> ServiceCharges
    {
      get { return _serviceCharges; }
      set { _serviceCharges = value; }
    }

    private IList<TicketPayment> _ticketPayments = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPayments
    {
      get { return _ticketPayments; }
      set { _ticketPayments = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatSrcCusinv = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatSrcCusinv
    {
      get { return _balanceTransfersByBatSrcCusinv; }
      set { _balanceTransfersByBatSrcCusinv = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatDstCusinv = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatDstCusinv
    {
      get { return _balanceTransfersByBatDstCusinv; }
      set { _balanceTransfersByBatDstCusinv = value; }
    }

    public virtual Currency CusinvCur { get; set; }

    public virtual Customer CusinvCus { get; set; }

    public virtual Operator CusinvOpr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(CustomerInvoice).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='CustomerInvoice'
         table='`CUSTOMER_INVOICES`'
         >
    <id name='CusinvId'
        column='`CUSINV_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CusinvCusId'
              column='`CUSINV_CUS_ID`'
              />
    <property name='CusinvOprId'
              column='`CUSINV_OPR_ID`'
              />
    <property name='CusinvInvNumber'
              column='`CUSINV_INV_NUMBER`'
              />
    <property name='CusinvInvDate'
              column='`CUSINV_INV_DATE`'
              />
    <property name='CusinvGenerationDate'
              column='`CUSINV_GENERATION_DATE`'
              />
    <property name='CusinvDateini'
              column='`CUSINV_DATEINI`'
              />
    <property name='CusinvDateend'
              column='`CUSINV_DATEEND`'
              />
    <property name='CusinvOprInitialInvoiceNumber'
              column='`CUSINV_OPR_INITIAL_INVOICE_NUMBER`'
              />
    <property name='CusinvOprEndInvoiceNumber'
              column='`CUSINV_OPR_END_INVOICE_NUMBER`'
              />
    <property name='CusinvInvAmount'
              column='`CUSINV_INV_AMOUNT`'
              />
    <property name='CusinvCurId'
              column='`CUSINV_CUR_ID`'
              />
    <property name='CusinvInvoiceVersion'
              column='`CUSINV_INVOICE_VERSION`'
              />
    <property name='CusinvInvAmountOps'
              column='`CUSINV_INV_AMOUNT_OPS`'
              />
    <bag name='CustomerPaymentMeansRecharges'
          inverse='false'
          >
      <key column='`CUSPMR_CUSINV_ID`' />
      <one-to-many class='CustomerPaymentMeansRecharge' />
    </bag>
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_CUSINV_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsDiscounts'
          inverse='false'
          >
      <key column='`OPEDIS_CUSINV_ID`' />
      <one-to-many class='OperationsDiscount' />
    </bag>
    <bag name='OperationsOffstreets'
          inverse='false'
          >
      <key column='`OPEOFF_CUSINV_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='ServiceCharges'
          inverse='false'
          >
      <key column='`SECH_CUSINV_ID`' />
      <one-to-many class='ServiceCharge' />
    </bag>
    <bag name='TicketPayments'
          inverse='false'
          >
      <key column='`TIPA_CUSINV_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <bag name='BalanceTransfersByBatSrcCusinv'
          inverse='false'
          >
      <key column='`BAT_SRC_CUSINV_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
    <bag name='BalanceTransfersByBatDstCusinv'
          inverse='false'
          >
      <key column='`BAT_DST_CUSINV_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
    <many-to-one name='CusinvCur' class='Currency' column='`CUSINV_CUR_ID`' />
    <many-to-one name='CusinvCus' class='Customer' column='`CUSINV_CUS_ID`' />
    <many-to-one name='CusinvOpr' class='Operator' column='`CUSINV_OPR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class CustomerPaymentMean
  {
    public virtual decimal CuspmId { get; set; }
    public virtual decimal CuspmCurId { get; set; }
    public virtual decimal CuspmCusId { get; set; }
    public virtual int CuspmPatId { get; set; }
    public virtual int CuspmPastId { get; set; }
    public virtual int CuspmAutomaticRecharge { get; set; }
    public virtual int CuspmAutomaticFailedRetries { get; set; }
    public virtual int CuspmValid { get; set; }
    public virtual int CuspmEnabled { get; set; }
    public virtual System.Nullable<int> CuspmAmountToRecharge { get; set; }
    public virtual System.Nullable<int> CuspmRechargeWhenAmountIsLess { get; set; }
    [Length(Max=50)]
    public virtual string CuspmDescription { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmLastTimeUserd { get; set; }
    [Length(Max=100)]
    public virtual string CuspmTokenCardHash { get; set; }
    [Length(Max=100)]
    public virtual string CuspmTokenCardReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmTokenMaskedCardNumber { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmTokenCardExpirationDate { get; set; }
    [Length(Max=50)]
    public virtual string CuspmTokenCardSchema { get; set; }
    [Length(Max=100)]
    public virtual string CuspmTokenPaypalId { get; set; }
    [Length(Max=100)]
    public virtual string CuspmTokenPaypalPreapprovalKey { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmTokenPaypalPreapprovalStartDate { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmTokenPaypalPreapprovalEndDate { get; set; }
    public virtual System.Nullable<int> CuspmTokenPaypalPreapprovalMaxNumberPayments { get; set; }
    public virtual System.Nullable<decimal> CuspmTokenPaypalPreapprovalMaxTotalAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmTokenPaypalPreapprovalMaxAmountPerPayment { get; set; }
    public virtual int CuspmCreditCardPaymentProvider { get; set; }

    private IList<CustomerPaymentMeansRecharge> _customerPaymentMeansRecharges = new List<CustomerPaymentMeansRecharge>();

    public virtual IList<CustomerPaymentMeansRecharge> CustomerPaymentMeansRecharges
    {
      get { return _customerPaymentMeansRecharges; }
      set { _customerPaymentMeansRecharges = value; }
    }

    private IList<User> _users = new List<User>();

    public virtual IList<User> Users
    {
      get { return _users; }
      set { _users = value; }
    }

    public virtual Currency CuspmCur { get; set; }

    public virtual Customer CuspmCus { get; set; }

    public virtual PaymentSubtype CuspmPast { get; set; }

    public virtual PaymentType CuspmPat { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(CustomerPaymentMean).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='CustomerPaymentMean'
         table='`CUSTOMER_PAYMENT_MEANS`'
         >
    <id name='CuspmId'
        column='`CUSPM_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CuspmCurId'
              column='`CUSPM_CUR_ID`'
              />
    <property name='CuspmCusId'
              column='`CUSPM_CUS_ID`'
              />
    <property name='CuspmPatId'
              column='`CUSPM_PAT_ID`'
              />
    <property name='CuspmPastId'
              column='`CUSPM_PAST_ID`'
              />
    <property name='CuspmAutomaticRecharge'
              column='`CUSPM_AUTOMATIC_RECHARGE`'
              />
    <property name='CuspmAutomaticFailedRetries'
              column='`CUSPM_AUTOMATIC_FAILED_RETRIES`'
              />
    <property name='CuspmValid'
              column='`CUSPM_VALID`'
              />
    <property name='CuspmEnabled'
              column='`CUSPM_ENABLED`'
              />
    <property name='CuspmAmountToRecharge'
              column='`CUSPM_AMOUNT_TO_RECHARGE`'
              />
    <property name='CuspmRechargeWhenAmountIsLess'
              column='`CUSPM_RECHARGE_WHEN_AMOUNT_IS_LESS`'
              />
    <property name='CuspmDescription'
              column='`CUSPM_DESCRIPTION`'
              />
    <property name='CuspmLastTimeUserd'
              column='`CUSPM_LAST_TIME_USERD`'
              />
    <property name='CuspmTokenCardHash'
              column='`CUSPM_TOKEN_CARD_HASH`'
              />
    <property name='CuspmTokenCardReference'
              column='`CUSPM_TOKEN_CARD_REFERENCE`'
              />
    <property name='CuspmTokenMaskedCardNumber'
              column='`CUSPM_TOKEN_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmTokenCardExpirationDate'
              column='`CUSPM_TOKEN_CARD_EXPIRATION_DATE`'
              />
    <property name='CuspmTokenCardSchema'
              column='`CUSPM_TOKEN_CARD_SCHEMA`'
              />
    <property name='CuspmTokenPaypalId'
              column='`CUSPM_TOKEN_PAYPAL_ID`'
              />
    <property name='CuspmTokenPaypalPreapprovalKey'
              column='`CUSPM_TOKEN_PAYPAL_PREAPPROVAL_KEY`'
              />
    <property name='CuspmTokenPaypalPreapprovalStartDate'
              column='`CUSPM_TOKEN_PAYPAL_PREAPPROVAL_START_DATE`'
              />
    <property name='CuspmTokenPaypalPreapprovalEndDate'
              column='`CUSPM_TOKEN_PAYPAL_PREAPPROVAL_END_DATE`'
              />
    <property name='CuspmTokenPaypalPreapprovalMaxNumberPayments'
              column='`CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_NUMBER_PAYMENTS`'
              />
    <property name='CuspmTokenPaypalPreapprovalMaxTotalAmount'
              column='`CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_TOTAL_AMOUNT`'
              />
    <property name='CuspmTokenPaypalPreapprovalMaxAmountPerPayment'
              column='`CUSPM_TOKEN_PAYPAL_PREAPPROVAL_MAX_AMOUNT_PER_PAYMENT`'
              />
    <property name='CuspmCreditCardPaymentProvider'
              column='`CUSPM_CREDIT_CARD_PAYMENT_PROVIDER`'
              />
    <bag name='CustomerPaymentMeansRecharges'
          inverse='false'
          >
      <key column='`CUSPMR_CUSPM_ID`' />
      <one-to-many class='CustomerPaymentMeansRecharge' />
    </bag>
    <bag name='Users'
          inverse='false'
          >
      <key column='`USR_CUSPM_ID`' />
      <one-to-many class='User' />
    </bag>
    <many-to-one name='CuspmCur' class='Currency' column='`CUSPM_CUR_ID`' />
    <many-to-one name='CuspmCus' class='Customer' column='`CUSPM_CUS_ID`' />
    <many-to-one name='CuspmPast' class='PaymentSubtype' column='`CUSPM_PAST_ID`' />
    <many-to-one name='CuspmPat' class='PaymentType' column='`CUSPM_PAT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class CustomerPaymentMeansRecharge
  {
    public virtual decimal CuspmrId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CuspmrTransactionId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CuspmrGatewayDate { get; set; }
    public virtual decimal CuspmrTotalAmountCharged { get; set; }
    public virtual int CuspmrSuscriptionType { get; set; }
    public virtual int CuspmrBalanceBefore { get; set; }
    public virtual int CuspmrTransStatus { get; set; }
    public virtual System.DateTime CuspmrStatusDate { get; set; }
    public virtual int CuspmrRetriesNum { get; set; }
    public virtual int CuspmrMoseOs { get; set; }
    public virtual int CuspmrAmount { get; set; }
    public virtual decimal CuspmrCurId { get; set; }
    public virtual int CuspmrDateUtcOffset { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrOpReference { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrUtcDate { get; set; }
    public virtual System.Nullable<decimal> CuspmrCusId { get; set; }
    public virtual System.Nullable<decimal> CuspmrCuspmId { get; set; }
    public virtual System.Nullable<decimal> CuspmrRcoupId { get; set; }
    public virtual System.Nullable<decimal> CuspmrUsrId { get; set; }
    public virtual System.Nullable<decimal> CuspmrCusinvId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> CuspmrLatitude { get; set; }
    public virtual System.Nullable<decimal> CuspmrLongitude { get; set; }
    [Length(Max=20)]
    public virtual string CuspmrAppVersion { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrAuthCode { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrAuthResult { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrSecondOpReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrSecondTransactionId { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrSecondGatewayDate { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrSecondAuthResult { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardHash { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardScheme { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrCardExpirationDate { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrPaypal3TToken { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrPaypal3TPayerId { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrPaypalPreapprovedPayKey { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercVat1 { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercVat2 { get; set; }
    public virtual System.Nullable<decimal> CuspmrPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> CuspmrPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrFixedFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrPartialFixedFee { get; set; }
    public virtual int CuspmrType { get; set; }
    public virtual System.Nullable<int> CuspmrPagateliaNewBalance { get; set; }
    public virtual int CuspmrCreationType { get; set; }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<ServiceCharge> _serviceCharges = new List<ServiceCharge>();

    public virtual IList<ServiceCharge> ServiceCharges
    {
      get { return _serviceCharges; }
      set { _serviceCharges = value; }
    }

    private IList<TicketPayment> _ticketPayments = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPayments
    {
      get { return _ticketPayments; }
      set { _ticketPayments = value; }
    }

    public virtual CustomerInvoice CuspmrCusinv { get; set; }

    public virtual CustomerPaymentMean CuspmrCuspm { get; set; }

    public virtual Customer CuspmrCus { get; set; }

    public virtual RechargeCoupon CuspmrRcoup { get; set; }

    public virtual User CuspmrUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(CustomerPaymentMeansRecharge).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='CustomerPaymentMeansRecharge'
         table='`CUSTOMER_PAYMENT_MEANS_RECHARGES`'
         >
    <id name='CuspmrId'
        column='`CUSPMR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrGatewayDate'
              column='`CUSPMR_GATEWAY_DATE`'
              />
    <property name='CuspmrTotalAmountCharged'
              column='`CUSPMR_TOTAL_AMOUNT_CHARGED`'
              />
    <property name='CuspmrSuscriptionType'
              column='`CUSPMR_SUSCRIPTION_TYPE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrTransStatus'
              column='`CUSPMR_TRANS_STATUS`'
              />
    <property name='CuspmrStatusDate'
              column='`CUSPMR_STATUS_DATE`'
              />
    <property name='CuspmrRetriesNum'
              column='`CUSPMR_RETRIES_NUM`'
              />
    <property name='CuspmrMoseOs'
              column='`CUSPMR_MOSE_OS`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CuspmrDateUtcOffset'
              column='`CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrUtcDate'
              column='`CUSPMR_UTC_DATE`'
              />
    <property name='CuspmrCusId'
              column='`CUSPMR_CUS_ID`'
              />
    <property name='CuspmrCuspmId'
              column='`CUSPMR_CUSPM_ID`'
              />
    <property name='CuspmrRcoupId'
              column='`CUSPMR_RCOUP_ID`'
              />
    <property name='CuspmrUsrId'
              column='`CUSPMR_USR_ID`'
              />
    <property name='CuspmrCusinvId'
              column='`CUSPMR_CUSINV_ID`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrLatitude'
              column='`CUSPMR_LATITUDE`'
              />
    <property name='CuspmrLongitude'
              column='`CUSPMR_LONGITUDE`'
              />
    <property name='CuspmrAppVersion'
              column='`CUSPMR_APP_VERSION`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrAuthResult'
              column='`CUSPMR_AUTH_RESULT`'
              />
    <property name='CuspmrSecondOpReference'
              column='`CUSPMR_SECOND_OP_REFERENCE`'
              />
    <property name='CuspmrSecondTransactionId'
              column='`CUSPMR_SECOND_TRANSACTION_ID`'
              />
    <property name='CuspmrSecondGatewayDate'
              column='`CUSPMR_SECOND_GATEWAY_DATE`'
              />
    <property name='CuspmrSecondAuthResult'
              column='`CUSPMR_SECOND_AUTH_RESULT`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='CuspmrPaypal3TToken'
              column='`CUSPMR_PAYPAL_3T_TOKEN`'
              />
    <property name='CuspmrPaypal3TPayerId'
              column='`CUSPMR_PAYPAL_3T_PAYER_ID`'
              />
    <property name='CuspmrPaypalPreapprovedPayKey'
              column='`CUSPMR_PAYPAL_PREAPPROVED_PAY_KEY`'
              />
    <property name='CuspmrPercVat1'
              column='`CUSPMR_PERC_VAT1`'
              />
    <property name='CuspmrPercVat2'
              column='`CUSPMR_PERC_VAT2`'
              />
    <property name='CuspmrPartialVat1'
              column='`CUSPMR_PARTIAL_VAT1`'
              />
    <property name='CuspmrPercFee'
              column='`CUSPMR_PERC_FEE`'
              />
    <property name='CuspmrPercFeeTopped'
              column='`CUSPMR_PERC_FEE_TOPPED`'
              />
    <property name='CuspmrPartialPercFee'
              column='`CUSPMR_PARTIAL_PERC_FEE`'
              />
    <property name='CuspmrFixedFee'
              column='`CUSPMR_FIXED_FEE`'
              />
    <property name='CuspmrPartialFixedFee'
              column='`CUSPMR_PARTIAL_FIXED_FEE`'
              />
    <property name='CuspmrType'
              column='`CUSPMR_TYPE`'
              />
    <property name='CuspmrPagateliaNewBalance'
              column='`CUSPMR_PAGATELIA_NEW_BALANCE`'
              />
    <property name='CuspmrCreationType'
              column='`CUSPMR_CREATION_TYPE`'
              />
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_CUSPMR_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsOffstreets'
          inverse='false'
          >
      <key column='`OPEOFF_CUSPMR_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='ServiceCharges'
          inverse='false'
          >
      <key column='`SECH_CUSPMR_ID`' />
      <one-to-many class='ServiceCharge' />
    </bag>
    <bag name='TicketPayments'
          inverse='false'
          >
      <key column='`TIPA_CUSPMR_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <many-to-one name='CuspmrCusinv' class='CustomerInvoice' column='`CUSPMR_CUSINV_ID`' />
    <many-to-one name='CuspmrCuspm' class='CustomerPaymentMean' column='`CUSPMR_CUSPM_ID`' />
    <many-to-one name='CuspmrCus' class='Customer' column='`CUSPMR_CUS_ID`' />
    <many-to-one name='CuspmrRcoup' class='RechargeCoupon' column='`CUSPMR_RCOUP_ID`' />
    <many-to-one name='CuspmrUsr' class='User' column='`CUSPMR_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Customer
  {
    public virtual decimal CusId { get; set; }
    public virtual decimal CusType { get; set; }
    public virtual System.DateTime CusInsertUtcDate { get; set; }
    public virtual decimal CusCouId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusDocId { get; set; }
    public virtual decimal CusDocIdType { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusStreet { get; set; }
    public virtual int CusStreeNumber { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusCity { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusState { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string CusZipcode { get; set; }
    public virtual int CusEnabled { get; set; }
    public virtual System.Nullable<int> CusLevelNum { get; set; }
    [Length(Max=10)]
    public virtual string CusDoor { get; set; }
    [Length(Max=10)]
    public virtual string CusLetter { get; set; }
    [Length(Max=10)]
    public virtual string CusStair { get; set; }
    [Length(Max=50)]
    public virtual string CusSurname1 { get; set; }
    [Length(Max=50)]
    public virtual string CusSurname2 { get; set; }
    public virtual System.Nullable<decimal> CusUsrId { get; set; }

    private IList<CustomerInscription> _customerInscriptions = new List<CustomerInscription>();

    public virtual IList<CustomerInscription> CustomerInscriptions
    {
      get { return _customerInscriptions; }
      set { _customerInscriptions = value; }
    }

    private IList<CustomerInvoice> _customerInvoices = new List<CustomerInvoice>();

    public virtual IList<CustomerInvoice> CustomerInvoices
    {
      get { return _customerInvoices; }
      set { _customerInvoices = value; }
    }

    private IList<CustomerPaymentMean> _customerPaymentMeans = new List<CustomerPaymentMean>();

    public virtual IList<CustomerPaymentMean> CustomerPaymentMeans
    {
      get { return _customerPaymentMeans; }
      set { _customerPaymentMeans = value; }
    }

    private IList<CustomerPaymentMeansRecharge> _customerPaymentMeansRecharges = new List<CustomerPaymentMeansRecharge>();

    public virtual IList<CustomerPaymentMeansRecharge> CustomerPaymentMeansRecharges
    {
      get { return _customerPaymentMeansRecharges; }
      set { _customerPaymentMeansRecharges = value; }
    }

    private IList<User> _users = new List<User>();

    public virtual IList<User> Users
    {
      get { return _users; }
      set { _users = value; }
    }

    public virtual Country CusCou { get; set; }

    public virtual User CusUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Customer).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Customer'
         table='`CUSTOMERS`'
         >
    <id name='CusId'
        column='`CUS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CusType'
              column='`CUS_TYPE`'
              />
    <property name='CusInsertUtcDate'
              column='`CUS_INSERT_UTC_DATE`'
              />
    <property name='CusCouId'
              column='`CUS_COU_ID`'
              />
    <property name='CusDocId'
              column='`CUS_DOC_ID`'
              />
    <property name='CusDocIdType'
              column='`CUS_DOC_ID_TYPE`'
              />
    <property name='CusName'
              column='`CUS_NAME`'
              />
    <property name='CusStreet'
              column='`CUS_STREET`'
              />
    <property name='CusStreeNumber'
              column='`CUS_STREE_NUMBER`'
              />
    <property name='CusCity'
              column='`CUS_CITY`'
              />
    <property name='CusState'
              column='`CUS_STATE`'
              />
    <property name='CusZipcode'
              column='`CUS_ZIPCODE`'
              />
    <property name='CusEnabled'
              column='`CUS_ENABLED`'
              />
    <property name='CusLevelNum'
              column='`CUS_LEVEL_NUM`'
              />
    <property name='CusDoor'
              column='`CUS_DOOR`'
              />
    <property name='CusLetter'
              column='`CUS_LETTER`'
              />
    <property name='CusStair'
              column='`CUS_STAIR`'
              />
    <property name='CusSurname1'
              column='`CUS_SURNAME1`'
              />
    <property name='CusSurname2'
              column='`CUS_SURNAME2`'
              />
    <property name='CusUsrId'
              column='`CUS_USR_ID`'
              />
    <bag name='CustomerInscriptions'
          inverse='false'
          >
      <key column='`CUISINS_CUS_ID`' />
      <one-to-many class='CustomerInscription' />
    </bag>
    <bag name='CustomerInvoices'
          inverse='true'
          >
      <key column='`CUSINV_CUS_ID`' />
      <one-to-many class='CustomerInvoice' />
    </bag>
    <bag name='CustomerPaymentMeans'
          inverse='true'
          >
      <key column='`CUSPM_CUS_ID`' />
      <one-to-many class='CustomerPaymentMean' />
    </bag>
    <bag name='CustomerPaymentMeansRecharges'
          inverse='false'
          >
      <key column='`CUSPMR_CUS_ID`' />
      <one-to-many class='CustomerPaymentMeansRecharge' />
    </bag>
    <bag name='Users'
          inverse='true'
          >
      <key column='`USR_CUS_ID`' />
      <one-to-many class='User' />
    </bag>
    <many-to-one name='CusCou' class='Country' column='`CUS_COU_ID`' />
    <many-to-one name='CusUsr' class='User' column='`CUS_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class ExternalParkingOperation
  {
    public virtual decimal EpoId { get; set; }
    public virtual decimal EpoInsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string EpoPlate { get; set; }
    public virtual System.DateTime EpoDate { get; set; }
    public virtual System.DateTime EpoEnddate { get; set; }
    public virtual System.DateTime EpoDateUtc { get; set; }
    public virtual int EpoInsertionNotified { get; set; }
    public virtual int EpoEndingNotified { get; set; }
    public virtual decimal EpoExpId { get; set; }
    public virtual int EpoSrctype { get; set; }
    public virtual int EpoType { get; set; }
    public virtual System.DateTime EpoInsertionUtcDate { get; set; }
    public virtual int EpoDateUtcOffset { get; set; }
    public virtual System.DateTime EpoEnddateUtc { get; set; }
    public virtual int EpoEnddateUtcOffset { get; set; }
    [Length(Max=100)]
    public virtual string EpoOperationId1 { get; set; }
    [Length(Max=100)]
    public virtual string EpoOperationId2 { get; set; }
    public virtual System.Nullable<int> EpoAmount { get; set; }
    public virtual System.Nullable<int> EpoTime { get; set; }
    public virtual System.Nullable<int> EpoInidateUtcOffset { get; set; }
    [Length(Max=50)]
    public virtual string EpoSrcident { get; set; }
    public virtual System.Nullable<System.DateTime> EpoInidateUtc { get; set; }
    public virtual System.Nullable<System.DateTime> EpoInidate { get; set; }
    public virtual System.Nullable<decimal> EpoZoneId { get; set; }
    public virtual System.Nullable<decimal> EpoTariffId { get; set; }

    public virtual ExternalProvider EpoExp { get; set; }

    public virtual Group EpoZone { get; set; }

    public virtual Tariff EpoTariff { get; set; }

    public virtual Installation EpoIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ExternalParkingOperation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='ExternalParkingOperation'
         table='`EXTERNAL_PARKING_OPERATIONS`'
         >
    <id name='EpoId'
        column='`EPO_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='EpoInsId'
              column='`EPO_INS_ID`'
              />
    <property name='EpoPlate'
              column='`EPO_PLATE`'
              />
    <property name='EpoDate'
              column='`EPO_DATE`'
              />
    <property name='EpoEnddate'
              column='`EPO_ENDDATE`'
              />
    <property name='EpoDateUtc'
              column='`EPO_DATE_UTC`'
              />
    <property name='EpoInsertionNotified'
              column='`EPO_INSERTION_NOTIFIED`'
              />
    <property name='EpoEndingNotified'
              column='`EPO_ENDING_NOTIFIED`'
              />
    <property name='EpoExpId'
              column='`EPO_EXP_ID`'
              />
    <property name='EpoSrctype'
              column='`EPO_SRCTYPE`'
              />
    <property name='EpoType'
              column='`EPO_TYPE`'
              />
    <property name='EpoInsertionUtcDate'
              column='`EPO_INSERTION_UTC_DATE`'
              />
    <property name='EpoDateUtcOffset'
              column='`EPO_DATE_UTC_OFFSET`'
              />
    <property name='EpoEnddateUtc'
              column='`EPO_ENDDATE_UTC`'
              />
    <property name='EpoEnddateUtcOffset'
              column='`EPO_ENDDATE_UTC_OFFSET`'
              />
    <property name='EpoOperationId1'
              column='`EPO_OPERATION_ID1`'
              />
    <property name='EpoOperationId2'
              column='`EPO_OPERATION_ID2`'
              />
    <property name='EpoAmount'
              column='`EPO_AMOUNT`'
              />
    <property name='EpoTime'
              column='`EPO_TIME`'
              />
    <property name='EpoInidateUtcOffset'
              column='`EPO_INIDATE_UTC_OFFSET`'
              />
    <property name='EpoSrcident'
              column='`EPO_SRCIDENT`'
              />
    <property name='EpoInidateUtc'
              column='`EPO_INIDATE_UTC`'
              />
    <property name='EpoInidate'
              column='`EPO_INIDATE`'
              />
    <property name='EpoZoneId'
              column='`EPO_ZONE`'
              />
    <property name='EpoTariffId'
              column='`EPO_TARIFF`'
              />
    <many-to-one name='EpoExp' class='ExternalProvider' column='`EPO_EXP_ID`' />
    <many-to-one name='EpoZone' class='Group' column='`EPO_ZONE`' />
    <many-to-one name='EpoTariff' class='Tariff' column='`EPO_TARIFF`' />
    <many-to-one name='EpoIns' class='Installation' column='`EPO_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class ExternalProvider
  {
    public virtual decimal ExpId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string ExpName { get; set; }

    private IList<ExternalParkingOperation> _externalParkingOperations = new List<ExternalParkingOperation>();

    public virtual IList<ExternalParkingOperation> ExternalParkingOperations
    {
      get { return _externalParkingOperations; }
      set { _externalParkingOperations = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ExternalProvider).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='ExternalProvider'
         table='`EXTERNAL_PROVIDERS`'
         >
    <id name='ExpId'
        column='`EXP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='ExpName'
              column='`EXP_NAME`'
              />
    <bag name='ExternalParkingOperations'
          inverse='true'
          >
      <key column='`EPO_EXP_ID`' />
      <one-to-many class='ExternalParkingOperation' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class ExternalTicket
  {
    public virtual decimal ExtiId { get; set; }
    public virtual decimal ExtiInsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string ExtiPlate { get; set; }
    public virtual System.DateTime ExtiDate { get; set; }
    public virtual System.DateTime ExtiDateUtc { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string ExtiTicketNumber { get; set; }
    public virtual System.DateTime ExtiLimitDate { get; set; }
    public virtual System.DateTime ExtiLimitDateUtc { get; set; }
    public virtual int ExtiInsertionNotified { get; set; }
    [Length(Max=512)]
    public virtual string ExtiArticleType { get; set; }
    [Length(Max=512)]
    public virtual string ExtiArticleDescription { get; set; }
    public virtual System.Nullable<int> ExtiAmount { get; set; }

    public virtual Installation ExtiIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ExternalTicket).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='ExternalTicket'
         table='`EXTERNAL_TICKET`'
         >
    <id name='ExtiId'
        column='`EXTI_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='ExtiInsId'
              column='`EXTI_INS_ID`'
              />
    <property name='ExtiPlate'
              column='`EXTI_PLATE`'
              />
    <property name='ExtiDate'
              column='`EXTI_DATE`'
              />
    <property name='ExtiDateUtc'
              column='`EXTI_DATE_UTC`'
              />
    <property name='ExtiTicketNumber'
              column='`EXTI_TICKET_NUMBER`'
              />
    <property name='ExtiLimitDate'
              column='`EXTI_LIMIT_DATE`'
              />
    <property name='ExtiLimitDateUtc'
              column='`EXTI_LIMIT_DATE_UTC`'
              />
    <property name='ExtiInsertionNotified'
              column='`EXTI_INSERTION_NOTIFIED`'
              />
    <property name='ExtiArticleType'
              column='`EXTI_ARTICLE_TYPE`'
              />
    <property name='ExtiArticleDescription'
              column='`EXTI_ARTICLE_DESCRIPTION`'
              />
    <property name='ExtiAmount'
              column='`EXTI_AMOUNT`'
              />
    <many-to-one name='ExtiIns' class='Installation' column='`EXTI_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Group
  {
    public virtual decimal GrpId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string GrpDescription { get; set; }
    public virtual decimal GrpInsId { get; set; }
    public virtual decimal GrpLitId { get; set; }
    public virtual int GrpType { get; set; }
    public virtual System.Nullable<int> GrpOffstreetType { get; set; }
    [Length(Max=20)]
    public virtual string GrpShowId { get; set; }
    [Length(Max=10)]
    public virtual string GrpColour { get; set; }
    [Length(Max=20)]
    public virtual string GrpQueryExtId { get; set; }
    [Length(Max=20)]
    public virtual string GrpExt1Id { get; set; }
    [Length(Max=20)]
    public virtual string GrpExt2Id { get; set; }
    [Length(Max=20)]
    public virtual string GrpExt3Id { get; set; }
    [Length(Max=50)]
    public virtual string GrpIdForExtOps { get; set; }
    public virtual System.Nullable<System.DateTime> GrpFreeSpacesAcquisitionUtcDate { get; set; }
    public virtual System.Nullable<int> GrpFreeSpacesNum { get; set; }
    public virtual System.Nullable<decimal> GrpFreeSpacesPerc { get; set; }
    [Length(Max=50)]
    public virtual string GrpDescription2 { get; set; }

    private IList<ExternalParkingOperation> _externalParkingOperations = new List<ExternalParkingOperation>();

    public virtual IList<ExternalParkingOperation> ExternalParkingOperations
    {
      get { return _externalParkingOperations; }
      set { _externalParkingOperations = value; }
    }

    private IList<GroupsGeometry> _groupsGeometries = new List<GroupsGeometry>();

    public virtual IList<GroupsGeometry> GroupsGeometries
    {
      get { return _groupsGeometries; }
      set { _groupsGeometries = value; }
    }

    private IList<GroupsHierarchy> _groupsHierarchiesByGrhiGprIdChild = new List<GroupsHierarchy>();

    public virtual IList<GroupsHierarchy> GroupsHierarchiesByGrhiGprIdChild
    {
      get { return _groupsHierarchiesByGrhiGprIdChild; }
      set { _groupsHierarchiesByGrhiGprIdChild = value; }
    }

    private IList<GroupsHierarchy> _groupsHierarchiesByGrhiGprIdParent = new List<GroupsHierarchy>();

    public virtual IList<GroupsHierarchy> GroupsHierarchiesByGrhiGprIdParent
    {
      get { return _groupsHierarchiesByGrhiGprIdParent; }
      set { _groupsHierarchiesByGrhiGprIdParent = value; }
    }

    private IList<GroupsOffstreetWsConfiguration> _groupsOffstreetWsConfigurations = new List<GroupsOffstreetWsConfiguration>();

    public virtual IList<GroupsOffstreetWsConfiguration> GroupsOffstreetWsConfigurations
    {
      get { return _groupsOffstreetWsConfigurations; }
      set { _groupsOffstreetWsConfigurations = value; }
    }

    private IList<GroupsTariffsExternalTranslation> _groupsTariffsExternalTranslations = new List<GroupsTariffsExternalTranslation>();

    public virtual IList<GroupsTariffsExternalTranslation> GroupsTariffsExternalTranslations
    {
      get { return _groupsTariffsExternalTranslations; }
      set { _groupsTariffsExternalTranslations = value; }
    }

    private IList<GroupsTypesAssignation> _groupsTypesAssignations = new List<GroupsTypesAssignation>();

    public virtual IList<GroupsTypesAssignation> GroupsTypesAssignations
    {
      get { return _groupsTypesAssignations; }
      set { _groupsTypesAssignations = value; }
    }

    private IList<OffstreetAutomaticOperation> _offstreetAutomaticOperations = new List<OffstreetAutomaticOperation>();

    public virtual IList<OffstreetAutomaticOperation> OffstreetAutomaticOperations
    {
      get { return _offstreetAutomaticOperations; }
      set { _offstreetAutomaticOperations = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<OperationsOffstreetSessionInfo> _operationsOffstreetSessionInfos = new List<OperationsOffstreetSessionInfo>();

    public virtual IList<OperationsOffstreetSessionInfo> OperationsOffstreetSessionInfos
    {
      get { return _operationsOffstreetSessionInfos; }
      set { _operationsOffstreetSessionInfos = value; }
    }

    private IList<PlatesTariff> _platesTariffs = new List<PlatesTariff>();

    public virtual IList<PlatesTariff> PlatesTariffs
    {
      get { return _platesTariffs; }
      set { _platesTariffs = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroups = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroups
    {
      get { return _tariffsInGroups; }
      set { _tariffsInGroups = value; }
    }

    private IList<TicketPayment> _ticketPayments = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPayments
    {
      get { return _ticketPayments; }
      set { _ticketPayments = value; }
    }

    private IList<TicketPaymentsSessionInfo> _ticketPaymentsSessionInfos = new List<TicketPaymentsSessionInfo>();

    public virtual IList<TicketPaymentsSessionInfo> TicketPaymentsSessionInfos
    {
      get { return _ticketPaymentsSessionInfos; }
      set { _ticketPaymentsSessionInfos = value; }
    }

    public virtual Literal GrpLit { get; set; }

    public virtual Installation GrpIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Group).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Group'
         table='`GROUPS`'
         >
    <id name='GrpId'
        column='`GRP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GrpDescription'
              column='`GRP_DESCRIPTION`'
              />
    <property name='GrpInsId'
              column='`GRP_INS_ID`'
              />
    <property name='GrpLitId'
              column='`GRP_LIT_ID`'
              />
    <property name='GrpType'
              column='`GRP_TYPE`'
              />
    <property name='GrpOffstreetType'
              column='`GRP_OFFSTREET_TYPE`'
              />
    <property name='GrpShowId'
              column='`GRP_SHOW_ID`'
              />
    <property name='GrpColour'
              column='`GRP_COLOUR`'
              />
    <property name='GrpQueryExtId'
              column='`GRP_QUERY_EXT_ID`'
              />
    <property name='GrpExt1Id'
              column='`GRP_EXT1_ID`'
              />
    <property name='GrpExt2Id'
              column='`GRP_EXT2_ID`'
              />
    <property name='GrpExt3Id'
              column='`GRP_EXT3_ID`'
              />
    <property name='GrpIdForExtOps'
              column='`GRP_ID_FOR_EXT_OPS`'
              />
    <property name='GrpFreeSpacesAcquisitionUtcDate'
              column='`GRP_FREE_SPACES_ACQUISITION_UTC_DATE`'
              />
    <property name='GrpFreeSpacesNum'
              column='`GRP_FREE_SPACES_NUM`'
              />
    <property name='GrpFreeSpacesPerc'
              column='`GRP_FREE_SPACES_PERC`'
              />
    <property name='GrpDescription2'
              column='`GRP_DESCRIPTION2`'
              />
    <bag name='ExternalParkingOperations'
          inverse='false'
          >
      <key column='`EPO_ZONE`' />
      <one-to-many class='ExternalParkingOperation' />
    </bag>
    <bag name='GroupsGeometries'
          inverse='true'
          >
      <key column='`GRGE_GRP_ID`' />
      <one-to-many class='GroupsGeometry' />
    </bag>
    <bag name='GroupsHierarchiesByGrhiGprIdChild'
          inverse='true'
          >
      <key column='`GRHI_GPR_ID_CHILD`' />
      <one-to-many class='GroupsHierarchy' />
    </bag>
    <bag name='GroupsHierarchiesByGrhiGprIdParent'
          inverse='false'
          >
      <key column='`GRHI_GPR_ID_PARENT`' />
      <one-to-many class='GroupsHierarchy' />
    </bag>
    <bag name='GroupsOffstreetWsConfigurations'
          inverse='true'
          >
      <key column='`GOWC_GRP_ID`' />
      <one-to-many class='GroupsOffstreetWsConfiguration' />
    </bag>
    <bag name='GroupsTariffsExternalTranslations'
          inverse='true'
          >
      <key column='`GTET_IN_GRP_ID`' />
      <one-to-many class='GroupsTariffsExternalTranslation' />
    </bag>
    <bag name='GroupsTypesAssignations'
          inverse='true'
          >
      <key column='`GTA_GRP_ID`' />
      <one-to-many class='GroupsTypesAssignation' />
    </bag>
    <bag name='OffstreetAutomaticOperations'
          inverse='true'
          >
      <key column='`OAUOP_GRP_ID`' />
      <one-to-many class='OffstreetAutomaticOperation' />
    </bag>
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_GRP_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsOffstreets'
          inverse='true'
          >
      <key column='`OPEOFF_GRP_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='OperationsOffstreetSessionInfos'
          inverse='true'
          >
      <key column='`OOSI_GRP_ID`' />
      <one-to-many class='OperationsOffstreetSessionInfo' />
    </bag>
    <bag name='PlatesTariffs'
          inverse='false'
          >
      <key column='`PLTA_GRP_ID`' />
      <one-to-many class='PlatesTariff' />
    </bag>
    <bag name='TariffsInGroups'
          inverse='false'
          >
      <key column='`TARGR_GRP_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <bag name='TicketPayments'
          inverse='false'
          >
      <key column='`TIPA_GRP_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <bag name='TicketPaymentsSessionInfos'
          inverse='false'
          >
      <key column='`TPSI_GRP_ID`' />
      <one-to-many class='TicketPaymentsSessionInfo' />
    </bag>
    <many-to-one name='GrpLit' class='Literal' column='`GRP_LIT_ID`' />
    <many-to-one name='GrpIns' class='Installation' column='`GRP_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class GroupsGeometry
  {
    public virtual decimal GrgeId { get; set; }
    public virtual decimal GrgeGrpId { get; set; }
    public virtual decimal GrgeOrder { get; set; }
    public virtual decimal GrgeLatitude { get; set; }
    public virtual decimal GrgeLongitude { get; set; }
    public virtual System.DateTime GrgeIniApplyDate { get; set; }
    public virtual System.DateTime GrgeEndApplyDate { get; set; }
    public virtual int GrgePolNumber { get; set; }

    public virtual Group GrgeGrp { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(GroupsGeometry).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='GroupsGeometry'
         table='`GROUPS_GEOMETRY`'
         >
    <id name='GrgeId'
        column='`GRGE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GrgeGrpId'
              column='`GRGE_GRP_ID`'
              />
    <property name='GrgeOrder'
              column='`GRGE_ORDER`'
              />
    <property name='GrgeLatitude'
              column='`GRGE_LATITUDE`'
              />
    <property name='GrgeLongitude'
              column='`GRGE_LONGITUDE`'
              />
    <property name='GrgeIniApplyDate'
              column='`GRGE_INI_APPLY_DATE`'
              />
    <property name='GrgeEndApplyDate'
              column='`GRGE_END_APPLY_DATE`'
              />
    <property name='GrgePolNumber'
              column='`GRGE_POL_NUMBER`'
              />
    <many-to-one name='GrgeGrp' class='Group' column='`GRGE_GRP_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class GroupsHierarchy
  {
    public virtual decimal GrhiId { get; set; }
    public virtual decimal GrhiGprIdChildId { get; set; }
    public virtual System.DateTime GrhiIniApplyDate { get; set; }
    public virtual System.DateTime GrhiEndApplyDate { get; set; }
    public virtual System.Nullable<decimal> GrhiGprIdParentId { get; set; }

    public virtual Group GrhiGprIdChild { get; set; }

    public virtual Group GrhiGprIdParent { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(GroupsHierarchy).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='GroupsHierarchy'
         table='`GROUPS_HIERARCHY`'
         >
    <id name='GrhiId'
        column='`GRHI_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GrhiGprIdChildId'
              column='`GRHI_GPR_ID_CHILD`'
              />
    <property name='GrhiIniApplyDate'
              column='`GRHI_INI_APPLY_DATE`'
              />
    <property name='GrhiEndApplyDate'
              column='`GRHI_END_APPLY_DATE`'
              />
    <property name='GrhiGprIdParentId'
              column='`GRHI_GPR_ID_PARENT`'
              />
    <many-to-one name='GrhiGprIdChild' class='Group' column='`GRHI_GPR_ID_CHILD`' />
    <many-to-one name='GrhiGprIdParent' class='Group' column='`GRHI_GPR_ID_PARENT`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class GroupsOffstreetWsConfiguration
  {
    public virtual decimal GowcId { get; set; }
    public virtual int GowcExitWs1SignatureType { get; set; }
    public virtual decimal GowcGrpId { get; set; }
    public virtual int GowcEntryWsSignatureType { get; set; }
    public virtual int GowcQueryExitWsSignatureType { get; set; }
    [Length(Max=500)]
    public virtual string GowcQueryExitWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string GowcQueryExitWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string GowcQueryExitWsHttpUser { get; set; }
    [Length(Max=50)]
    public virtual string GowcQueryExitWsHttpPassword { get; set; }
    [Length(Max=500)]
    public virtual string GowcEntryWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string GowcEntryWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string GowcEntryWsHttpUser { get; set; }
    [Length(Max=50)]
    public virtual string GowcEntryWsHttpPassword { get; set; }
    [Length(Max=500)]
    public virtual string GowcExitWs1Url { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs1AuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs1HttpUser { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs1HttpPassword { get; set; }
    public virtual System.Nullable<int> GowcExitWs2SignatureType { get; set; }
    [Length(Max=500)]
    public virtual string GowcExitWs2Url { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs2AuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs2HttpUser { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs2HttpPassword { get; set; }
    public virtual System.Nullable<int> GowcExitWs3SignatureType { get; set; }
    [Length(Max=500)]
    public virtual string GowcExitWs3Url { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs3AuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs3HttpUser { get; set; }
    [Length(Max=50)]
    public virtual string GowcExitWs3HttpPassword { get; set; }
    public virtual System.Nullable<int> GowcOptOperationconfirmMode { get; set; }

    public virtual Group GowcGrp { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(GroupsOffstreetWsConfiguration).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='GroupsOffstreetWsConfiguration'
         table='`GROUPS_OFFSTREET_WS_CONFIGURATION`'
         >
    <id name='GowcId'
        column='`GOWC_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GowcExitWs1SignatureType'
              column='`GOWC_EXIT_WS1_SIGNATURE_TYPE`'
              />
    <property name='GowcGrpId'
              column='`GOWC_GRP_ID`'
              />
    <property name='GowcEntryWsSignatureType'
              column='`GOWC_ENTRY_WS_SIGNATURE_TYPE`'
              />
    <property name='GowcQueryExitWsSignatureType'
              column='`GOWC_QUERY_EXIT_WS_SIGNATURE_TYPE`'
              />
    <property name='GowcQueryExitWsUrl'
              column='`GOWC_QUERY_EXIT_WS_URL`'
              />
    <property name='GowcQueryExitWsAuthHashKey'
              column='`GOWC_QUERY_EXIT_WS_AUTH_HASH_KEY`'
              />
    <property name='GowcQueryExitWsHttpUser'
              column='`GOWC_QUERY_EXIT_WS_HTTP_USER`'
              />
    <property name='GowcQueryExitWsHttpPassword'
              column='`GOWC_QUERY_EXIT_WS_HTTP_PASSWORD`'
              />
    <property name='GowcEntryWsUrl'
              column='`GOWC_ENTRY_WS_URL`'
              />
    <property name='GowcEntryWsAuthHashKey'
              column='`GOWC_ENTRY_WS_AUTH_HASH_KEY`'
              />
    <property name='GowcEntryWsHttpUser'
              column='`GOWC_ENTRY_WS_HTTP_USER`'
              />
    <property name='GowcEntryWsHttpPassword'
              column='`GOWC_ENTRY_WS_HTTP_PASSWORD`'
              />
    <property name='GowcExitWs1Url'
              column='`GOWC_EXIT_WS1_URL`'
              />
    <property name='GowcExitWs1AuthHashKey'
              column='`GOWC_EXIT_WS1_AUTH_HASH_KEY`'
              />
    <property name='GowcExitWs1HttpUser'
              column='`GOWC_EXIT_WS1_HTTP_USER`'
              />
    <property name='GowcExitWs1HttpPassword'
              column='`GOWC_EXIT_WS1_HTTP_PASSWORD`'
              />
    <property name='GowcExitWs2SignatureType'
              column='`GOWC_EXIT_WS2_SIGNATURE_TYPE`'
              />
    <property name='GowcExitWs2Url'
              column='`GOWC_EXIT_WS2_URL`'
              />
    <property name='GowcExitWs2AuthHashKey'
              column='`GOWC_EXIT_WS2_AUTH_HASH_KEY`'
              />
    <property name='GowcExitWs2HttpUser'
              column='`GOWC_EXIT_WS2_HTTP_USER`'
              />
    <property name='GowcExitWs2HttpPassword'
              column='`GOWC_EXIT_WS2_HTTP_PASSWORD`'
              />
    <property name='GowcExitWs3SignatureType'
              column='`GOWC_EXIT_WS3_SIGNATURE_TYPE`'
              />
    <property name='GowcExitWs3Url'
              column='`GOWC_EXIT_WS3_URL`'
              />
    <property name='GowcExitWs3AuthHashKey'
              column='`GOWC_EXIT_WS3_AUTH_HASH_KEY`'
              />
    <property name='GowcExitWs3HttpUser'
              column='`GOWC_EXIT_WS3_HTTP_USER`'
              />
    <property name='GowcExitWs3HttpPassword'
              column='`GOWC_EXIT_WS3_HTTP_PASSWORD`'
              />
    <property name='GowcOptOperationconfirmMode'
              column='`GOWC_OPT_OPERATIONCONFIRM_MODE`'
              />
    <many-to-one name='GowcGrp' class='Group' column='`GOWC_GRP_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class GroupsTariffsExternalTranslation
  {
    public virtual decimal GtetId { get; set; }
    public virtual int GtetWsNumber { get; set; }
    public virtual decimal GtetInGrpId { get; set; }
    public virtual decimal GtetInTarId { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string GtetOutGrpExtId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string GtetOutTarExtId { get; set; }

    public virtual Group GtetInGrp { get; set; }

    public virtual Tariff GtetInTar { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(GroupsTariffsExternalTranslation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='GroupsTariffsExternalTranslation'
         table='`GROUPS_TARIFFS_EXTERNAL_TRANSLATIONS`'
         >
    <id name='GtetId'
        column='`GTET_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GtetWsNumber'
              column='`GTET_WS_NUMBER`'
              />
    <property name='GtetInGrpId'
              column='`GTET_IN_GRP_ID`'
              />
    <property name='GtetInTarId'
              column='`GTET_IN_TAR_ID`'
              />
    <property name='GtetOutGrpExtId'
              column='`GTET_OUT_GRP_EXT_ID`'
              />
    <property name='GtetOutTarExtId'
              column='`GTET_OUT_TAR_EXT_ID`'
              />
    <many-to-one name='GtetInGrp' class='Group' column='`GTET_IN_GRP_ID`' />
    <many-to-one name='GtetInTar' class='Tariff' column='`GTET_IN_TAR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class GroupsType
  {
    public virtual decimal GrptId { get; set; }
    public virtual decimal GrptInsId { get; set; }
    [Length(Max=50)]
    public virtual string GrptDescription { get; set; }

    private IList<GroupsTypesAssignation> _groupsTypesAssignations = new List<GroupsTypesAssignation>();

    public virtual IList<GroupsTypesAssignation> GroupsTypesAssignations
    {
      get { return _groupsTypesAssignations; }
      set { _groupsTypesAssignations = value; }
    }

    private IList<PlatesTariff> _platesTariffs = new List<PlatesTariff>();

    public virtual IList<PlatesTariff> PlatesTariffs
    {
      get { return _platesTariffs; }
      set { _platesTariffs = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroups = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroups
    {
      get { return _tariffsInGroups; }
      set { _tariffsInGroups = value; }
    }

    public virtual Installation GrptIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(GroupsType).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='GroupsType'
         table='`GROUPS_TYPES`'
         >
    <id name='GrptId'
        column='`GRPT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GrptInsId'
              column='`GRPT_INS_ID`'
              />
    <property name='GrptDescription'
              column='`GRPT_DESCRIPTION`'
              />
    <bag name='GroupsTypesAssignations'
          inverse='true'
          >
      <key column='`GTA_GRPT_ID`' />
      <one-to-many class='GroupsTypesAssignation' />
    </bag>
    <bag name='PlatesTariffs'
          inverse='false'
          >
      <key column='`PLTA_GRPT_ID`' />
      <one-to-many class='PlatesTariff' />
    </bag>
    <bag name='TariffsInGroups'
          inverse='false'
          >
      <key column='`TARGR_GRPT_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <many-to-one name='GrptIns' class='Installation' column='`GRPT_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class GroupsTypesAssignation
  {
    public virtual decimal GtaId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string GtaDescription { get; set; }
    public virtual decimal GtaGrpId { get; set; }
    public virtual decimal GtaGrptId { get; set; }

    public virtual Group GtaGrp { get; set; }

    public virtual GroupsType GtaGrpt { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(GroupsTypesAssignation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='GroupsTypesAssignation'
         table='`GROUPS_TYPES_ASSIGNATIONS`'
         >
    <id name='GtaId'
        column='`GTA_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='GtaDescription'
              column='`GTA_DESCRIPTION`'
              />
    <property name='GtaGrpId'
              column='`GTA_GRP_ID`'
              />
    <property name='GtaGrptId'
              column='`GTA_GRPT_ID`'
              />
    <many-to-one name='GtaGrp' class='Group' column='`GTA_GRP_ID`' />
    <many-to-one name='GtaGrpt' class='GroupsType' column='`GTA_GRPT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class InstallationsGeometry
  {
    public virtual decimal InsgeId { get; set; }
    public virtual decimal InsgeInsId { get; set; }
    public virtual decimal InsgeOrder { get; set; }
    public virtual decimal InsgeLatitude { get; set; }
    public virtual decimal InsgeLongitude { get; set; }
    public virtual System.DateTime InsgeIniApplyDate { get; set; }
    public virtual System.DateTime InsgeEndApplyDate { get; set; }
    public virtual int InsgePolNumber { get; set; }

    public virtual Installation InsgeIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(InstallationsGeometry).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='InstallationsGeometry'
         table='`INSTALLATIONS_GEOMETRY`'
         >
    <id name='InsgeId'
        column='`INSGE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='InsgeInsId'
              column='`INSGE_INS_ID`'
              />
    <property name='InsgeOrder'
              column='`INSGE_ORDER`'
              />
    <property name='InsgeLatitude'
              column='`INSGE_LATITUDE`'
              />
    <property name='InsgeLongitude'
              column='`INSGE_LONGITUDE`'
              />
    <property name='InsgeIniApplyDate'
              column='`INSGE_INI_APPLY_DATE`'
              />
    <property name='InsgeEndApplyDate'
              column='`INSGE_END_APPLY_DATE`'
              />
    <property name='InsgePolNumber'
              column='`INSGE_POL_NUMBER`'
              />
    <many-to-one name='InsgeIns' class='Installation' column='`INSGE_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class InvoicingSchema
  {
    public virtual decimal InvschId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InvschDescription { get; set; }

    private IList<RetailerPayment> _retailerPayments = new List<RetailerPayment>();

    public virtual IList<RetailerPayment> RetailerPayments
    {
      get { return _retailerPayments; }
      set { _retailerPayments = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(InvoicingSchema).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='InvoicingSchema'
         table='`INVOICING_SCHEMAS`'
         >
    <id name='InvschId'
        column='`INVSCH_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='InvschDescription'
              column='`INVSCH_DESCRIPTION`'
              />
    <bag name='RetailerPayments'
          inverse='false'
          >
      <key column='`RTLPY_INVSCH_ID`' />
      <one-to-many class='RetailerPayment' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Language
  {
    public virtual decimal LanId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string LanDescription { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string LanCulture { get; set; }

    private IList<LiteralLanguage> _literalLanguages = new List<LiteralLanguage>();

    public virtual IList<LiteralLanguage> LiteralLanguages
    {
      get { return _literalLanguages; }
      set { _literalLanguages = value; }
    }

    private IList<LicenseTermsParam> _licenseTermsParams = new List<LicenseTermsParam>();

    public virtual IList<LicenseTermsParam> LicenseTermsParams
    {
      get { return _licenseTermsParams; }
      set { _licenseTermsParams = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Language).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Language'
         table='`LANGUAGES`'
         >
    <id name='LanId'
        column='`LAN_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='LanDescription'
              column='`LAN_DESCRIPTION`'
              />
    <property name='LanCulture'
              column='`LAN_CULTURE`'
              />
    <bag name='LiteralLanguages'
          inverse='true'
          >
      <key column='`LITL_LAN_ID`' />
      <one-to-many class='LiteralLanguage' />
    </bag>
    <bag name='LicenseTermsParams'
          inverse='true'
          >
      <key column='`LTP_LAN_ID`' />
      <one-to-many class='LicenseTermsParam' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class LiteralLanguage
  {
    public virtual decimal LitlLitId { get; set; }
    public virtual decimal LitlLanId { get; set; }
    [NotNull]
    [Length(Max=1024)]
    public virtual string LitlLiteral { get; set; }

    public virtual Language LitlLan { get; set; }

    public virtual Literal LitlLit { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != GetType())
      {
        return false;
      }

      LiteralLanguage other = (LiteralLanguage)obj;
      if (other.LitlLitId != LitlLitId)
      {
        return false;
      }
      if (other.LitlLanId != LitlLanId)
      {
        return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      hashCode = 19 * hashCode + LitlLitId.GetHashCode();
      hashCode = 19 * hashCode + LitlLanId.GetHashCode();
      return hashCode;
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(LiteralLanguage).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='LiteralLanguage'
         table='`LITERAL_LANGUAGES`'
         >
    <composite-id>
      <key-property name='LitlLitId'
                    column='`LITL_LIT_ID`'
                    />
      <key-property name='LitlLanId'
                    column='`LITL_LAN_ID`'
                    />
    </composite-id>
    <property name='LitlLiteral'
              column='`LITL_LITERAL`'
              />
    <many-to-one name='LitlLan' class='Language' column='`LITL_LAN_ID`' />
    <many-to-one name='LitlLit' class='Literal' column='`LITL_LIT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Literal
  {
    public virtual decimal LitId { get; set; }
    [Length(Max=50)]
    public virtual string LitDescription { get; set; }

    private IList<Group> _groups = new List<Group>();

    public virtual IList<Group> Groups
    {
      get { return _groups; }
      set { _groups = value; }
    }

    private IList<LiteralLanguage> _literalLanguages = new List<LiteralLanguage>();

    public virtual IList<LiteralLanguage> LiteralLanguages
    {
      get { return _literalLanguages; }
      set { _literalLanguages = value; }
    }

    private IList<PaymentSubtype> _paymentSubtypes = new List<PaymentSubtype>();

    public virtual IList<PaymentSubtype> PaymentSubtypes
    {
      get { return _paymentSubtypes; }
      set { _paymentSubtypes = value; }
    }

    private IList<PaymentType> _paymentTypes = new List<PaymentType>();

    public virtual IList<PaymentType> PaymentTypes
    {
      get { return _paymentTypes; }
      set { _paymentTypes = value; }
    }

    private IList<RechargeCouponsStatus> _rechargeCouponsStatuses = new List<RechargeCouponsStatus>();

    public virtual IList<RechargeCouponsStatus> RechargeCouponsStatuses
    {
      get { return _rechargeCouponsStatuses; }
      set { _rechargeCouponsStatuses = value; }
    }

    private IList<ServiceChargeType> _serviceChargeTypes = new List<ServiceChargeType>();

    public virtual IList<ServiceChargeType> ServiceChargeTypes
    {
      get { return _serviceChargeTypes; }
      set { _serviceChargeTypes = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroupsByTargrLit = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroupsByTargrLit
    {
      get { return _tariffsInGroupsByTargrLit; }
      set { _tariffsInGroupsByTargrLit = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroupsByTargrStep1Lit = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroupsByTargrStep1Lit
    {
      get { return _tariffsInGroupsByTargrStep1Lit; }
      set { _tariffsInGroupsByTargrStep1Lit = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroupsByTargrStep2Lit = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroupsByTargrStep2Lit
    {
      get { return _tariffsInGroupsByTargrStep2Lit; }
      set { _tariffsInGroupsByTargrStep2Lit = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroupsByTargrStep3Lit = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroupsByTargrStep3Lit
    {
      get { return _tariffsInGroupsByTargrStep3Lit; }
      set { _tariffsInGroupsByTargrStep3Lit = value; }
    }

    private IList<Tariff> _tariffs = new List<Tariff>();

    public virtual IList<Tariff> Tariffs
    {
      get { return _tariffs; }
      set { _tariffs = value; }
    }

    private IList<Installation> _installationsByInsLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsLit
    {
      get { return _installationsByInsLit; }
      set { _installationsByInsLit = value; }
    }

    private IList<Installation> _installationsByInsServiceFeeLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsServiceFeeLit
    {
      get { return _installationsByInsServiceFeeLit; }
      set { _installationsByInsServiceFeeLit = value; }
    }

    private IList<Installation> _installationsByInsServiceParkLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsServiceParkLit
    {
      get { return _installationsByInsServiceParkLit; }
      set { _installationsByInsServiceParkLit = value; }
    }

    private IList<Installation> _installationsByInsServiceSubtotalLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsServiceSubtotalLit
    {
      get { return _installationsByInsServiceSubtotalLit; }
      set { _installationsByInsServiceSubtotalLit = value; }
    }

    private IList<Installation> _installationsByInsServiceTotalLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsServiceTotalLit
    {
      get { return _installationsByInsServiceTotalLit; }
      set { _installationsByInsServiceTotalLit = value; }
    }

    private IList<Installation> _installationsByInsServiceVatLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsServiceVatLit
    {
      get { return _installationsByInsServiceVatLit; }
      set { _installationsByInsServiceVatLit = value; }
    }

    private IList<Installation> _installationsByInsTollPendingMsgLit = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsTollPendingMsgLit
    {
      get { return _installationsByInsTollPendingMsgLit; }
      set { _installationsByInsTollPendingMsgLit = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Literal).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Literal'
         table='`LITERALS`'
         >
    <id name='LitId'
        column='`LIT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='LitDescription'
              column='`LIT_DESCRIPTION`'
              />
    <bag name='Groups'
          inverse='true'
          >
      <key column='`GRP_LIT_ID`' />
      <one-to-many class='Group' />
    </bag>
    <bag name='LiteralLanguages'
          inverse='true'
          >
      <key column='`LITL_LIT_ID`' />
      <one-to-many class='LiteralLanguage' />
    </bag>
    <bag name='PaymentSubtypes'
          inverse='false'
          >
      <key column='`PAST_LIT_ID`' />
      <one-to-many class='PaymentSubtype' />
    </bag>
    <bag name='PaymentTypes'
          inverse='false'
          >
      <key column='`PAT_LIT_ID`' />
      <one-to-many class='PaymentType' />
    </bag>
    <bag name='RechargeCouponsStatuses'
          inverse='false'
          >
      <key column='`RCOUPS_LIT_ID`' />
      <one-to-many class='RechargeCouponsStatus' />
    </bag>
    <bag name='ServiceChargeTypes'
          inverse='false'
          >
      <key column='`SECHT_LIT_ID`' />
      <one-to-many class='ServiceChargeType' />
    </bag>
    <bag name='TariffsInGroupsByTargrLit'
          inverse='true'
          >
      <key column='`TARGR_LIT_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <bag name='TariffsInGroupsByTargrStep1Lit'
          inverse='false'
          >
      <key column='`TARGR_STEP1_LIT_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <bag name='TariffsInGroupsByTargrStep2Lit'
          inverse='false'
          >
      <key column='`TARGR_STEP2_LIT_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <bag name='TariffsInGroupsByTargrStep3Lit'
          inverse='false'
          >
      <key column='`TARGR_STEP3_LIT_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <bag name='Tariffs'
          inverse='true'
          >
      <key column='`TAR_LIT_ID`' />
      <one-to-many class='Tariff' />
    </bag>
    <bag name='InstallationsByInsLit'
          inverse='true'
          >
      <key column='`INS_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='InstallationsByInsServiceFeeLit'
          inverse='false'
          >
      <key column='`INS_SERVICE_FEE_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='InstallationsByInsServiceParkLit'
          inverse='false'
          >
      <key column='`INS_SERVICE_PARK_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='InstallationsByInsServiceSubtotalLit'
          inverse='false'
          >
      <key column='`INS_SERVICE_SUBTOTAL_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='InstallationsByInsServiceTotalLit'
          inverse='false'
          >
      <key column='`INS_SERVICE_TOTAL_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='InstallationsByInsServiceVatLit'
          inverse='false'
          >
      <key column='`INS_SERVICE_VAT_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
    <bag name='InstallationsByInsTollPendingMsgLit'
          inverse='false'
          >
      <key column='`INS_TOLL_PENDING_MSG_LIT_ID`' />
      <one-to-many class='Installation' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class MobileSession
  {
    public virtual decimal MoseId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string MoseSessionid { get; set; }
    public virtual decimal MoseUsrId { get; set; }
    public virtual decimal MoseInsId { get; set; }
    public virtual System.DateTime MoseCreationTime { get; set; }
    public virtual System.DateTime MoseLastUpdateTime { get; set; }
    public virtual int MoseStatus { get; set; }
    [Length(Max=20)]
    public virtual string MoseCultureLang { get; set; }
    public virtual System.Nullable<decimal> MoseUpidId { get; set; }
    [Length(Max=100)]
    public virtual string MoseCellWifiMac { get; set; }
    [Length(Max=100)]
    public virtual string MoseCellImei { get; set; }
    [Length(Max=100)]
    public virtual string MoseCellModel { get; set; }
    public virtual System.Nullable<int> MoseOs { get; set; }
    [Length(Max=100)]
    public virtual string MoseOsVersion { get; set; }
    [Length(Max=100)]
    public virtual string MoseCellSerialnumber { get; set; }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<OperationsOffstreetSessionInfo> _operationsOffstreetSessionInfos = new List<OperationsOffstreetSessionInfo>();

    public virtual IList<OperationsOffstreetSessionInfo> OperationsOffstreetSessionInfos
    {
      get { return _operationsOffstreetSessionInfos; }
      set { _operationsOffstreetSessionInfos = value; }
    }

    private IList<OperationsSessionInfo> _operationsSessionInfos = new List<OperationsSessionInfo>();

    public virtual IList<OperationsSessionInfo> OperationsSessionInfos
    {
      get { return _operationsSessionInfos; }
      set { _operationsSessionInfos = value; }
    }

    private IList<RechargeCouponsUse> _rechargeCouponsUses = new List<RechargeCouponsUse>();

    public virtual IList<RechargeCouponsUse> RechargeCouponsUses
    {
      get { return _rechargeCouponsUses; }
      set { _rechargeCouponsUses = value; }
    }

    private IList<TicketPaymentsSessionInfo> _ticketPaymentsSessionInfos = new List<TicketPaymentsSessionInfo>();

    public virtual IList<TicketPaymentsSessionInfo> TicketPaymentsSessionInfos
    {
      get { return _ticketPaymentsSessionInfos; }
      set { _ticketPaymentsSessionInfos = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatMose = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatMose
    {
      get { return _balanceTransfersByBatMose; }
      set { _balanceTransfersByBatMose = value; }
    }

    public virtual User MoseUsr { get; set; }

    public virtual UsersPushId MoseUpid { get; set; }

    public virtual Installation MoseIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(MobileSession).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='MobileSession'
         table='`MOBILE_SESSIONS`'
         >
    <id name='MoseId'
        column='`MOSE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='MoseSessionid'
              column='`MOSE_SESSIONID`'
              />
    <property name='MoseUsrId'
              column='`MOSE_USR_ID`'
              />
    <property name='MoseInsId'
              column='`MOSE_INS_ID`'
              />
    <property name='MoseCreationTime'
              column='`MOSE_CREATION_TIME`'
              />
    <property name='MoseLastUpdateTime'
              column='`MOSE_LAST_UPDATE_TIME`'
              />
    <property name='MoseStatus'
              column='`MOSE_STATUS`'
              />
    <property name='MoseCultureLang'
              column='`MOSE_CULTURE_LANG`'
              />
    <property name='MoseUpidId'
              column='`MOSE_UPID_ID`'
              />
    <property name='MoseCellWifiMac'
              column='`MOSE_CELL_WIFI_MAC`'
              />
    <property name='MoseCellImei'
              column='`MOSE_CELL_IMEI`'
              />
    <property name='MoseCellModel'
              column='`MOSE_CELL_MODEL`'
              />
    <property name='MoseOs'
              column='`MOSE_OS`'
              />
    <property name='MoseOsVersion'
              column='`MOSE_OS_VERSION`'
              />
    <property name='MoseCellSerialnumber'
              column='`MOSE_CELL_SERIALNUMBER`'
              />
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_MOSE_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsOffstreets'
          inverse='false'
          >
      <key column='`OPEOFF_MOSE_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='OperationsOffstreetSessionInfos'
          inverse='true'
          >
      <key column='`OOSI_MOSE_ID`' />
      <one-to-many class='OperationsOffstreetSessionInfo' />
    </bag>
    <bag name='OperationsSessionInfos'
          inverse='true'
          >
      <key column='`OSI_MOSE_ID`' />
      <one-to-many class='OperationsSessionInfo' />
    </bag>
    <bag name='RechargeCouponsUses'
          inverse='false'
          >
      <key column='`RCOUPU_MOSE_ID`' />
      <one-to-many class='RechargeCouponsUse' />
    </bag>
    <bag name='TicketPaymentsSessionInfos'
          inverse='true'
          >
      <key column='`TPSI_MOSE_ID`' />
      <one-to-many class='TicketPaymentsSessionInfo' />
    </bag>
    <bag name='BalanceTransfersByBatMose'
          inverse='false'
          >
      <key column='`BAT_MOSE_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
    <many-to-one name='MoseUsr' class='User' column='`MOSE_USR_ID`' />
    <many-to-one name='MoseUpid' class='UsersPushId' column='`MOSE_UPID_ID`' />
    <many-to-one name='MoseIns' class='Installation' column='`MOSE_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class OffstreetAutomaticOperation
  {
    public virtual decimal OauopId { get; set; }
    public virtual int OauopType { get; set; }
    public virtual decimal OauopInsId { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string OauopOpeId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OauopPlate { get; set; }
    public virtual decimal OauopGrpId { get; set; }
    public virtual System.DateTime OauopDate { get; set; }
    public virtual System.DateTime OauopDateUtc { get; set; }
    public virtual System.Nullable<int> OauopAmount { get; set; }
    [Length(Max=100)]
    public virtual string OauopGate { get; set; }
    [Length(Max=100)]
    public virtual string OauopTariff { get; set; }

    public virtual Group OauopGrp { get; set; }

    public virtual Installation OauopIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(OffstreetAutomaticOperation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='OffstreetAutomaticOperation'
         table='`OFFSTREET_AUTOMATIC_OPERATIONS`'
         >
    <id name='OauopId'
        column='`OAUOP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OauopType'
              column='`OAUOP_TYPE`'
              />
    <property name='OauopInsId'
              column='`OAUOP_INS_ID`'
              />
    <property name='OauopOpeId'
              column='`OAUOP_OPE_ID`'
              />
    <property name='OauopPlate'
              column='`OAUOP_PLATE`'
              />
    <property name='OauopGrpId'
              column='`OAUOP_GRP_ID`'
              />
    <property name='OauopDate'
              column='`OAUOP_DATE`'
              />
    <property name='OauopDateUtc'
              column='`OAUOP_DATE_UTC`'
              />
    <property name='OauopAmount'
              column='`OAUOP_AMOUNT`'
              />
    <property name='OauopGate'
              column='`OAUOP_GATE`'
              />
    <property name='OauopTariff'
              column='`OAUOP_TARIFF`'
              />
    <many-to-one name='OauopGrp' class='Group' column='`OAUOP_GRP_ID`' />
    <many-to-one name='OauopIns' class='Installation' column='`OAUOP_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Operation
  {
    public virtual decimal OpeId { get; set; }
    public virtual System.DateTime OpeDate { get; set; }
    public virtual System.DateTime OpeInidate { get; set; }
    public virtual System.DateTime OpeEnddate { get; set; }
    public virtual System.DateTime OpeUtcDate { get; set; }
    public virtual System.DateTime OpeUtcInidate { get; set; }
    public virtual System.DateTime OpeUtcEnddate { get; set; }
    public virtual int OpeDateUtcOffset { get; set; }
    public virtual int OpeInidateUtcOffset { get; set; }
    public virtual int OpeEnddateUtcOffset { get; set; }
    public virtual int OpeAmount { get; set; }
    public virtual int OpeTime { get; set; }
    public virtual decimal OpeAmountCurId { get; set; }
    public virtual decimal OpeBalanceCurId { get; set; }
    public virtual decimal OpeChangeApplied { get; set; }
    public virtual decimal OpeChangeFeeApplied { get; set; }
    public virtual int OpeFinalAmount { get; set; }
    public virtual int OpeBalanceBefore { get; set; }
    public virtual int OpeSuscriptionType { get; set; }
    public virtual int OpeConfirmedInWs1 { get; set; }
    public virtual int OpeConfirmedInWs2 { get; set; }
    public virtual int OpeConfirmedInWs3 { get; set; }
    public virtual decimal OpeUsrId { get; set; }
    public virtual int OpeMoseOs { get; set; }
    public virtual decimal OpeInsId { get; set; }
    public virtual int OpeType { get; set; }
    public virtual System.Nullable<decimal> OpeGrpId { get; set; }
    public virtual System.Nullable<decimal> OpeTarId { get; set; }
    public virtual System.Nullable<decimal> OpeUsrpId { get; set; }
    public virtual System.Nullable<int> OpeConfirmInWs1RetriesNum { get; set; }
    public virtual System.Nullable<int> OpeConfirmInWs2RetriesNum { get; set; }
    public virtual System.Nullable<int> OpeConfirmInWs3RetriesNum { get; set; }
    public virtual System.Nullable<System.DateTime> OpeConfirmInWs1Date { get; set; }
    public virtual System.Nullable<System.DateTime> OpeConfirmInWs2Date { get; set; }
    public virtual System.Nullable<System.DateTime> OpeConfirmInWs3Date { get; set; }
    public virtual System.Nullable<decimal> OpeMoseId { get; set; }
    public virtual System.Nullable<decimal> OpeLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeLongitude { get; set; }
    [Length(Max=20)]
    public virtual string OpeAppVersion { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs3 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId3 { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeOpedisId { get; set; }
    public virtual System.Nullable<decimal> OpePercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercVat2 { get; set; }
    public virtual System.Nullable<decimal> OpePartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercFee { get; set; }
    public virtual System.Nullable<decimal> OpePercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OpePartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpeFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpePartialFixedFee { get; set; }
    public virtual System.Nullable<int> OpeTotalAmount { get; set; }
    public virtual System.Nullable<decimal> OpeCusinvId { get; set; }
    public virtual System.Nullable<decimal> OpePercBonus { get; set; }
    public virtual System.Nullable<decimal> OpePartialBonusFee { get; set; }
    [Length(Max=50)]
    public virtual string OpeBonusId { get; set; }
    [Length(Max=50)]
    public virtual string OpeBonusMarca { get; set; }
    public virtual System.Nullable<int> OpeBonusType { get; set; }

    public virtual Currency OpeAmountCur { get; set; }

    public virtual Currency OpeBalanceCur { get; set; }

    public virtual CustomerInvoice OpeCusinv { get; set; }

    public virtual CustomerPaymentMeansRecharge OpeCuspmr { get; set; }

    public virtual Group OpeGrp { get; set; }

    public virtual MobileSession OpeMose { get; set; }

    public virtual OperationsDiscount OpeOpedis { get; set; }

    public virtual Tariff OpeTar { get; set; }

    public virtual UserPlate OpeUsrp { get; set; }

    public virtual User OpeUsr { get; set; }

    public virtual Installation OpeIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Operation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Operation'
         table='`OPERATIONS`'
         >
    <id name='OpeId'
        column='`OPE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeDate'
              column='`OPE_DATE`'
              />
    <property name='OpeInidate'
              column='`OPE_INIDATE`'
              />
    <property name='OpeEnddate'
              column='`OPE_ENDDATE`'
              />
    <property name='OpeUtcDate'
              column='`OPE_UTC_DATE`'
              />
    <property name='OpeUtcInidate'
              column='`OPE_UTC_INIDATE`'
              />
    <property name='OpeUtcEnddate'
              column='`OPE_UTC_ENDDATE`'
              />
    <property name='OpeDateUtcOffset'
              column='`OPE_DATE_UTC_OFFSET`'
              />
    <property name='OpeInidateUtcOffset'
              column='`OPE_INIDATE_UTC_OFFSET`'
              />
    <property name='OpeEnddateUtcOffset'
              column='`OPE_ENDDATE_UTC_OFFSET`'
              />
    <property name='OpeAmount'
              column='`OPE_AMOUNT`'
              />
    <property name='OpeTime'
              column='`OPE_TIME`'
              />
    <property name='OpeAmountCurId'
              column='`OPE_AMOUNT_CUR_ID`'
              />
    <property name='OpeBalanceCurId'
              column='`OPE_BALANCE_CUR_ID`'
              />
    <property name='OpeChangeApplied'
              column='`OPE_CHANGE_APPLIED`'
              />
    <property name='OpeChangeFeeApplied'
              column='`OPE_CHANGE_FEE_APPLIED`'
              />
    <property name='OpeFinalAmount'
              column='`OPE_FINAL_AMOUNT`'
              />
    <property name='OpeBalanceBefore'
              column='`OPE_BALANCE_BEFORE`'
              />
    <property name='OpeSuscriptionType'
              column='`OPE_SUSCRIPTION_TYPE`'
              />
    <property name='OpeConfirmedInWs1'
              column='`OPE_CONFIRMED_IN_WS1`'
              />
    <property name='OpeConfirmedInWs2'
              column='`OPE_CONFIRMED_IN_WS2`'
              />
    <property name='OpeConfirmedInWs3'
              column='`OPE_CONFIRMED_IN_WS3`'
              />
    <property name='OpeUsrId'
              column='`OPE_USR_ID`'
              />
    <property name='OpeMoseOs'
              column='`OPE_MOSE_OS`'
              />
    <property name='OpeInsId'
              column='`OPE_INS_ID`'
              />
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='OpeGrpId'
              column='`OPE_GRP_ID`'
              />
    <property name='OpeTarId'
              column='`OPE_TAR_ID`'
              />
    <property name='OpeUsrpId'
              column='`OPE_USRP_ID`'
              />
    <property name='OpeConfirmInWs1RetriesNum'
              column='`OPE_CONFIRM_IN_WS1_RETRIES_NUM`'
              />
    <property name='OpeConfirmInWs2RetriesNum'
              column='`OPE_CONFIRM_IN_WS2_RETRIES_NUM`'
              />
    <property name='OpeConfirmInWs3RetriesNum'
              column='`OPE_CONFIRM_IN_WS3_RETRIES_NUM`'
              />
    <property name='OpeConfirmInWs1Date'
              column='`OPE_CONFIRM_IN_WS1_DATE`'
              />
    <property name='OpeConfirmInWs2Date'
              column='`OPE_CONFIRM_IN_WS2_DATE`'
              />
    <property name='OpeConfirmInWs3Date'
              column='`OPE_CONFIRM_IN_WS3_DATE`'
              />
    <property name='OpeMoseId'
              column='`OPE_MOSE_ID`'
              />
    <property name='OpeLatitude'
              column='`OPE_LATITUDE`'
              />
    <property name='OpeLongitude'
              column='`OPE_LONGITUDE`'
              />
    <property name='OpeAppVersion'
              column='`OPE_APP_VERSION`'
              />
    <property name='OpeConfirmationTimeInWs1'
              column='`OPE_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeConfirmationTimeInWs2'
              column='`OPE_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeConfirmationTimeInWs3'
              column='`OPE_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs1'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs2'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs3'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeExternalId1'
              column='`OPE_EXTERNAL_ID1`'
              />
    <property name='OpeExternalId2'
              column='`OPE_EXTERNAL_ID2`'
              />
    <property name='OpeExternalId3'
              column='`OPE_EXTERNAL_ID3`'
              />
    <property name='OpeInsertionUtcDate'
              column='`OPE_INSERTION_UTC_DATE`'
              />
    <property name='OpeCuspmrId'
              column='`OPE_CUSPMR_ID`'
              />
    <property name='OpeOpedisId'
              column='`OPE_OPEDIS_ID`'
              />
    <property name='OpePercVat1'
              column='`OPE_PERC_VAT1`'
              />
    <property name='OpePercVat2'
              column='`OPE_PERC_VAT2`'
              />
    <property name='OpePartialVat1'
              column='`OPE_PARTIAL_VAT1`'
              />
    <property name='OpePercFee'
              column='`OPE_PERC_FEE`'
              />
    <property name='OpePercFeeTopped'
              column='`OPE_PERC_FEE_TOPPED`'
              />
    <property name='OpePartialPercFee'
              column='`OPE_PARTIAL_PERC_FEE`'
              />
    <property name='OpeFixedFee'
              column='`OPE_FIXED_FEE`'
              />
    <property name='OpePartialFixedFee'
              column='`OPE_PARTIAL_FIXED_FEE`'
              />
    <property name='OpeTotalAmount'
              column='`OPE_TOTAL_AMOUNT`'
              />
    <property name='OpeCusinvId'
              column='`OPE_CUSINV_ID`'
              />
    <property name='OpePercBonus'
              column='`OPE_PERC_BONUS`'
              />
    <property name='OpePartialBonusFee'
              column='`OPE_PARTIAL_BONUS_FEE`'
              />
    <property name='OpeBonusId'
              column='`OPE_BONUS_ID`'
              />
    <property name='OpeBonusMarca'
              column='`OPE_BONUS_MARCA`'
              />
    <property name='OpeBonusType'
              column='`OPE_BONUS_TYPE`'
              />
    <many-to-one name='OpeAmountCur' class='Currency' column='`OPE_AMOUNT_CUR_ID`' />
    <many-to-one name='OpeBalanceCur' class='Currency' column='`OPE_BALANCE_CUR_ID`' />
    <many-to-one name='OpeCusinv' class='CustomerInvoice' column='`OPE_CUSINV_ID`' />
    <many-to-one name='OpeCuspmr' class='CustomerPaymentMeansRecharge' column='`OPE_CUSPMR_ID`' />
    <many-to-one name='OpeGrp' class='Group' column='`OPE_GRP_ID`' />
    <many-to-one name='OpeMose' class='MobileSession' column='`OPE_MOSE_ID`' />
    <many-to-one name='OpeOpedis' class='OperationsDiscount' column='`OPE_OPEDIS_ID`' />
    <many-to-one name='OpeTar' class='Tariff' column='`OPE_TAR_ID`' />
    <many-to-one name='OpeUsrp' class='UserPlate' column='`OPE_USRP_ID`' />
    <many-to-one name='OpeUsr' class='User' column='`OPE_USR_ID`' />
    <many-to-one name='OpeIns' class='Installation' column='`OPE_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class OperationsDiscount
  {
    public virtual decimal OpedisId { get; set; }
    public virtual decimal OpedisUsrId { get; set; }
    public virtual int OpedisMoseOs { get; set; }
    public virtual System.DateTime OpedisDate { get; set; }
    public virtual System.DateTime OpedisUtcDate { get; set; }
    public virtual int OpedisDateUtcOffset { get; set; }
    public virtual int OpedisAmount { get; set; }
    public virtual decimal OpedisAmountCurId { get; set; }
    public virtual decimal OpedisBalanceCurId { get; set; }
    public virtual decimal OpedisChangeApplied { get; set; }
    public virtual decimal OpedisChangeFeeApplied { get; set; }
    public virtual int OpedisFinalAmount { get; set; }
    public virtual int OpedisSuscriptionType { get; set; }
    public virtual int OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<decimal> OpedisLatitude { get; set; }
    public virtual System.Nullable<decimal> OpedisLongitude { get; set; }
    [Length(Max=20)]
    public virtual string OpedisAppVersion { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpedisPercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpedisPercVat2 { get; set; }
    public virtual System.Nullable<decimal> OpedisPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpedisPercFee { get; set; }
    public virtual System.Nullable<decimal> OpedisPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OpedisPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpedisFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpedisPartialFixedFee { get; set; }
    public virtual System.Nullable<int> OpedisTotalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisCusinvId { get; set; }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<TicketPayment> _ticketPayments = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPayments
    {
      get { return _ticketPayments; }
      set { _ticketPayments = value; }
    }

    public virtual Currency OpedisAmountCur { get; set; }

    public virtual Currency OpedisBalanceCur { get; set; }

    public virtual CustomerInvoice OpedisCusinv { get; set; }

    public virtual User OpedisUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(OperationsDiscount).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='OperationsDiscount'
         table='`OPERATIONS_DISCOUNTS`'
         >
    <id name='OpedisId'
        column='`OPEDIS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpedisUsrId'
              column='`OPEDIS_USR_ID`'
              />
    <property name='OpedisMoseOs'
              column='`OPEDIS_MOSE_OS`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisUtcDate'
              column='`OPEDIS_UTC_DATE`'
              />
    <property name='OpedisDateUtcOffset'
              column='`OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisChangeFeeApplied'
              column='`OPEDIS_CHANGE_FEE_APPLIED`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisSuscriptionType'
              column='`OPEDIS_SUSCRIPTION_TYPE`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisLatitude'
              column='`OPEDIS_LATITUDE`'
              />
    <property name='OpedisLongitude'
              column='`OPEDIS_LONGITUDE`'
              />
    <property name='OpedisAppVersion'
              column='`OPEDIS_APP_VERSION`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='OpedisPercVat1'
              column='`OPEDIS_PERC_VAT1`'
              />
    <property name='OpedisPercVat2'
              column='`OPEDIS_PERC_VAT2`'
              />
    <property name='OpedisPartialVat1'
              column='`OPEDIS_PARTIAL_VAT1`'
              />
    <property name='OpedisPercFee'
              column='`OPEDIS_PERC_FEE`'
              />
    <property name='OpedisPercFeeTopped'
              column='`OPEDIS_PERC_FEE_TOPPED`'
              />
    <property name='OpedisPartialPercFee'
              column='`OPEDIS_PARTIAL_PERC_FEE`'
              />
    <property name='OpedisFixedFee'
              column='`OPEDIS_FIXED_FEE`'
              />
    <property name='OpedisPartialFixedFee'
              column='`OPEDIS_PARTIAL_FIXED_FEE`'
              />
    <property name='OpedisTotalAmount'
              column='`OPEDIS_TOTAL_AMOUNT`'
              />
    <property name='OpedisCusinvId'
              column='`OPEDIS_CUSINV_ID`'
              />
    <bag name='OperationsOffstreets'
          inverse='false'
          >
      <key column='`OPEOFF_OPEDIS_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_OPEDIS_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='TicketPayments'
          inverse='false'
          >
      <key column='`TIPA_OPEDIS_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <many-to-one name='OpedisAmountCur' class='Currency' column='`OPEDIS_AMOUNT_CUR_ID`' />
    <many-to-one name='OpedisBalanceCur' class='Currency' column='`OPEDIS_BALANCE_CUR_ID`' />
    <many-to-one name='OpedisCusinv' class='CustomerInvoice' column='`OPEDIS_CUSINV_ID`' />
    <many-to-one name='OpedisUsr' class='User' column='`OPEDIS_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class OperationsOffstreet
  {
    public virtual decimal OpeoffId { get; set; }
    public virtual int OpeoffType { get; set; }
    public virtual decimal OpeoffUsrId { get; set; }
    public virtual int OpeoffMoseOs { get; set; }
    public virtual decimal OpeoffUsrpId { get; set; }
    public virtual decimal OpeoffInsId { get; set; }
    public virtual decimal OpeoffGrpId { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string OpeoffLogicalId { get; set; }
    public virtual System.DateTime OpeoffEntryDate { get; set; }
    public virtual System.DateTime OpeoffNotifyEntryDate { get; set; }
    public virtual System.DateTime OpeoffUtcEntryDate { get; set; }
    public virtual System.DateTime OpeoffUtcNotifyEntryDate { get; set; }
    public virtual int OpeoffBalanceBefore { get; set; }
    public virtual int OpeoffSuscriptionType { get; set; }
    public virtual int OpeoffConfirmedInWs1 { get; set; }
    public virtual int OpeoffConfirmedInWs2 { get; set; }
    public virtual int OpeoffConfirmedInWs3 { get; set; }
    public virtual int OpeoffAmount { get; set; }
    public virtual int OpeoffTime { get; set; }
    public virtual decimal OpeoffAmountCurId { get; set; }
    public virtual decimal OpeoffBalanceCurId { get; set; }
    public virtual decimal OpeoffChangeApplied { get; set; }
    public virtual decimal OpeoffChangeFeeApplied { get; set; }
    public virtual int OpeoffFinalAmount { get; set; }
    public virtual int OpeoffEntryDateUtcOffset { get; set; }
    public virtual int OpeoffNotifyEntryDateUtcOffset { get; set; }
    public virtual int OpeoffMustNotify { get; set; }
    public virtual int OpeoffNotified { get; set; }
    public virtual System.Nullable<int> OpeoffPaymentDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeoffEndDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeoffExitLimitDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeoffExitDateUtcOffset { get; set; }
    [Length(Max=50)]
    public virtual string OpeoffExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeoffExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeoffExternalId3 { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeoffCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeoffOpedisId { get; set; }
    public virtual System.Nullable<int> OpeoffConfirmInWs1RetriesNum { get; set; }
    public virtual System.Nullable<int> OpeoffConfirmInWs2RetriesNum { get; set; }
    public virtual System.Nullable<int> OpeoffConfirmInWs3RetriesNum { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffConfirmInWs1Date { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffConfirmInWs2Date { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffConfirmInWs3Date { get; set; }
    public virtual System.Nullable<decimal> OpeoffMoseId { get; set; }
    public virtual System.Nullable<decimal> OpeoffLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeoffLongitude { get; set; }
    [Length(Max=20)]
    public virtual string OpeoffAppVersion { get; set; }
    public virtual System.Nullable<decimal> OpeoffConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeoffConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeoffConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeoffQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<int> OpeoffQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<int> OpeoffQueueLengthBeforeConfirmWs3 { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitDate { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffTariff { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffGate { get; set; }
    [Length(Max=10)]
    public virtual string OpeoffSpaceDescription { get; set; }
    public virtual System.Nullable<decimal> OpeoffCusinvId { get; set; }

    public virtual Currency OpeoffAmountCur { get; set; }

    public virtual Currency OpeoffBalanceCur { get; set; }

    public virtual CustomerInvoice OpeoffCusinv { get; set; }

    public virtual CustomerPaymentMeansRecharge OpeoffCuspmr { get; set; }

    public virtual Group OpeoffGrp { get; set; }

    public virtual MobileSession OpeoffMose { get; set; }

    public virtual OperationsDiscount OpeoffOpedis { get; set; }

    public virtual UserPlate OpeoffUsrp { get; set; }

    public virtual User OpeoffUsr { get; set; }

    public virtual Installation OpeoffIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(OperationsOffstreet).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='OperationsOffstreet'
         table='`OPERATIONS_OFFSTREET`'
         >
    <id name='OpeoffId'
        column='`OPEOFF_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeoffType'
              column='`OPEOFF_TYPE`'
              />
    <property name='OpeoffUsrId'
              column='`OPEOFF_USR_ID`'
              />
    <property name='OpeoffMoseOs'
              column='`OPEOFF_MOSE_OS`'
              />
    <property name='OpeoffUsrpId'
              column='`OPEOFF_USRP_ID`'
              />
    <property name='OpeoffInsId'
              column='`OPEOFF_INS_ID`'
              />
    <property name='OpeoffGrpId'
              column='`OPEOFF_GRP_ID`'
              />
    <property name='OpeoffLogicalId'
              column='`OPEOFF_LOGICAL_ID`'
              />
    <property name='OpeoffEntryDate'
              column='`OPEOFF_ENTRY_DATE`'
              />
    <property name='OpeoffNotifyEntryDate'
              column='`OPEOFF_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffUtcEntryDate'
              column='`OPEOFF_UTC_ENTRY_DATE`'
              />
    <property name='OpeoffUtcNotifyEntryDate'
              column='`OPEOFF_UTC_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffBalanceBefore'
              column='`OPEOFF_BALANCE_BEFORE`'
              />
    <property name='OpeoffSuscriptionType'
              column='`OPEOFF_SUSCRIPTION_TYPE`'
              />
    <property name='OpeoffConfirmedInWs1'
              column='`OPEOFF_CONFIRMED_IN_WS1`'
              />
    <property name='OpeoffConfirmedInWs2'
              column='`OPEOFF_CONFIRMED_IN_WS2`'
              />
    <property name='OpeoffConfirmedInWs3'
              column='`OPEOFF_CONFIRMED_IN_WS3`'
              />
    <property name='OpeoffAmount'
              column='`OPEOFF_AMOUNT`'
              />
    <property name='OpeoffTime'
              column='`OPEOFF_TIME`'
              />
    <property name='OpeoffAmountCurId'
              column='`OPEOFF_AMOUNT_CUR_ID`'
              />
    <property name='OpeoffBalanceCurId'
              column='`OPEOFF_BALANCE_CUR_ID`'
              />
    <property name='OpeoffChangeApplied'
              column='`OPEOFF_CHANGE_APPLIED`'
              />
    <property name='OpeoffChangeFeeApplied'
              column='`OPEOFF_CHANGE_FEE_APPLIED`'
              />
    <property name='OpeoffFinalAmount'
              column='`OPEOFF_FINAL_AMOUNT`'
              />
    <property name='OpeoffEntryDateUtcOffset'
              column='`OPEOFF_ENTRY_DATE_UTC_OFFSET`'
              />
    <property name='OpeoffNotifyEntryDateUtcOffset'
              column='`OPEOFF_NOTIFY_ENTRY_DATE_UTC_OFFSET`'
              />
    <property name='OpeoffMustNotify'
              column='`OPEOFF_MUST_NOTIFY`'
              />
    <property name='OpeoffNotified'
              column='`OPEOFF_NOTIFIED`'
              />
    <property name='OpeoffPaymentDateUtcOffset'
              column='`OPEOFF_PAYMENT_DATE_UTC_OFFSET`'
              />
    <property name='OpeoffEndDateUtcOffset'
              column='`OPEOFF_END_DATE_UTC_OFFSET`'
              />
    <property name='OpeoffExitLimitDateUtcOffset'
              column='`OPEOFF_EXIT_LIMIT_DATE_UTC_OFFSET`'
              />
    <property name='OpeoffExitDateUtcOffset'
              column='`OPEOFF_EXIT_DATE_UTC_OFFSET`'
              />
    <property name='OpeoffExternalId1'
              column='`OPEOFF_EXTERNAL_ID1`'
              />
    <property name='OpeoffExternalId2'
              column='`OPEOFF_EXTERNAL_ID2`'
              />
    <property name='OpeoffExternalId3'
              column='`OPEOFF_EXTERNAL_ID3`'
              />
    <property name='OpeoffInsertionUtcDate'
              column='`OPEOFF_INSERTION_UTC_DATE`'
              />
    <property name='OpeoffCuspmrId'
              column='`OPEOFF_CUSPMR_ID`'
              />
    <property name='OpeoffOpedisId'
              column='`OPEOFF_OPEDIS_ID`'
              />
    <property name='OpeoffConfirmInWs1RetriesNum'
              column='`OPEOFF_CONFIRM_IN_WS1_RETRIES_NUM`'
              />
    <property name='OpeoffConfirmInWs2RetriesNum'
              column='`OPEOFF_CONFIRM_IN_WS2_RETRIES_NUM`'
              />
    <property name='OpeoffConfirmInWs3RetriesNum'
              column='`OPEOFF_CONFIRM_IN_WS3_RETRIES_NUM`'
              />
    <property name='OpeoffConfirmInWs1Date'
              column='`OPEOFF_CONFIRM_IN_WS1_DATE`'
              />
    <property name='OpeoffConfirmInWs2Date'
              column='`OPEOFF_CONFIRM_IN_WS2_DATE`'
              />
    <property name='OpeoffConfirmInWs3Date'
              column='`OPEOFF_CONFIRM_IN_WS3_DATE`'
              />
    <property name='OpeoffMoseId'
              column='`OPEOFF_MOSE_ID`'
              />
    <property name='OpeoffLatitude'
              column='`OPEOFF_LATITUDE`'
              />
    <property name='OpeoffLongitude'
              column='`OPEOFF_LONGITUDE`'
              />
    <property name='OpeoffAppVersion'
              column='`OPEOFF_APP_VERSION`'
              />
    <property name='OpeoffConfirmationTimeInWs1'
              column='`OPEOFF_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeoffConfirmationTimeInWs2'
              column='`OPEOFF_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeoffConfirmationTimeInWs3'
              column='`OPEOFF_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeoffQueueLengthBeforeConfirmWs1'
              column='`OPEOFF_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeoffQueueLengthBeforeConfirmWs2'
              column='`OPEOFF_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeoffQueueLengthBeforeConfirmWs3'
              column='`OPEOFF_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeoffUtcPaymentDate'
              column='`OPEOFF_UTC_PAYMENT_DATE`'
              />
    <property name='OpeoffUtcEndDate'
              column='`OPEOFF_UTC_END_DATE`'
              />
    <property name='OpeoffUtcExitLimitDate'
              column='`OPEOFF_UTC_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffUtcExitDate'
              column='`OPEOFF_UTC_EXIT_DATE`'
              />
    <property name='OpeoffPaymentDate'
              column='`OPEOFF_PAYMENT_DATE`'
              />
    <property name='OpeoffEndDate'
              column='`OPEOFF_END_DATE`'
              />
    <property name='OpeoffExitLimitDate'
              column='`OPEOFF_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffExitDate'
              column='`OPEOFF_EXIT_DATE`'
              />
    <property name='OpeoffTariff'
              column='`OPEOFF_TARIFF`'
              />
    <property name='OpeoffGate'
              column='`OPEOFF_GATE`'
              />
    <property name='OpeoffSpaceDescription'
              column='`OPEOFF_SPACE_DESCRIPTION`'
              />
    <property name='OpeoffCusinvId'
              column='`OPEOFF_CUSINV_ID`'
              />
    <many-to-one name='OpeoffAmountCur' class='Currency' column='`OPEOFF_AMOUNT_CUR_ID`' />
    <many-to-one name='OpeoffBalanceCur' class='Currency' column='`OPEOFF_BALANCE_CUR_ID`' />
    <many-to-one name='OpeoffCusinv' class='CustomerInvoice' column='`OPEOFF_CUSINV_ID`' />
    <many-to-one name='OpeoffCuspmr' class='CustomerPaymentMeansRecharge' column='`OPEOFF_CUSPMR_ID`' />
    <many-to-one name='OpeoffGrp' class='Group' column='`OPEOFF_GRP_ID`' />
    <many-to-one name='OpeoffMose' class='MobileSession' column='`OPEOFF_MOSE_ID`' />
    <many-to-one name='OpeoffOpedis' class='OperationsDiscount' column='`OPEOFF_OPEDIS_ID`' />
    <many-to-one name='OpeoffUsrp' class='UserPlate' column='`OPEOFF_USRP_ID`' />
    <many-to-one name='OpeoffUsr' class='User' column='`OPEOFF_USR_ID`' />
    <many-to-one name='OpeoffIns' class='Installation' column='`OPEOFF_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class OperationsOffstreetSessionInfo
  {
    public virtual decimal OosiId { get; set; }
    public virtual int OosiAmount { get; set; }
    public virtual int OosiTime { get; set; }
    public virtual System.DateTime OosiEntryUtcDate { get; set; }
    public virtual System.DateTime OosiEndUtcDate { get; set; }
    public virtual System.DateTime OosiExitLimitUtcDate { get; set; }
    public virtual System.DateTime OosiInsDate { get; set; }
    public virtual int OosiOpeoffType { get; set; }
    public virtual decimal OosiChangeApplied { get; set; }
    public virtual decimal OosiMoseId { get; set; }
    public virtual System.DateTime OosiUtcDate { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string OosiLogicalId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OosiPlate { get; set; }
    public virtual decimal OosiGrpId { get; set; }
    [Length(Max=100)]
    public virtual string OosiTariff { get; set; }

    public virtual Group OosiGrp { get; set; }

    public virtual MobileSession OosiMose { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(OperationsOffstreetSessionInfo).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='OperationsOffstreetSessionInfo'
         table='`OPERATIONS_OFFSTREET_SESSION_INFO`'
         >
    <id name='OosiId'
        column='`OOSI_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OosiAmount'
              column='`OOSI_AMOUNT`'
              />
    <property name='OosiTime'
              column='`OOSI_TIME`'
              />
    <property name='OosiEntryUtcDate'
              column='`OOSI_ENTRY_UTC_DATE`'
              />
    <property name='OosiEndUtcDate'
              column='`OOSI_END_UTC_DATE`'
              />
    <property name='OosiExitLimitUtcDate'
              column='`OOSI_EXIT_LIMIT_UTC_DATE`'
              />
    <property name='OosiInsDate'
              column='`OOSI_INS_DATE`'
              />
    <property name='OosiOpeoffType'
              column='`OOSI_OPEOFF_TYPE`'
              />
    <property name='OosiChangeApplied'
              column='`OOSI_CHANGE_APPLIED`'
              />
    <property name='OosiMoseId'
              column='`OOSI_MOSE_ID`'
              />
    <property name='OosiUtcDate'
              column='`OOSI_UTC_DATE`'
              />
    <property name='OosiLogicalId'
              column='`OOSI_LOGICAL_ID`'
              />
    <property name='OosiPlate'
              column='`OOSI_PLATE`'
              />
    <property name='OosiGrpId'
              column='`OOSI_GRP_ID`'
              />
    <property name='OosiTariff'
              column='`OOSI_TARIFF`'
              />
    <many-to-one name='OosiGrp' class='Group' column='`OOSI_GRP_ID`' />
    <many-to-one name='OosiMose' class='MobileSession' column='`OOSI_MOSE_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class OperationsSessionInfo
  {
    public virtual decimal OsiId { get; set; }
    public virtual decimal OsiMoseId { get; set; }
    public virtual System.DateTime OsiUtcDate { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OsiPlate { get; set; }
    public virtual int OsiOpeType { get; set; }
    public virtual System.DateTime OsiInsDate { get; set; }
    public virtual decimal OsiChangeApplied { get; set; }
    public virtual System.Nullable<decimal> OsiGrpId { get; set; }
    public virtual System.Nullable<decimal> OsiTarId { get; set; }
    public virtual System.Nullable<int> OsiAmountToRefund { get; set; }
    public virtual System.Nullable<int> OsiTimeForAmount { get; set; }
    public virtual System.Nullable<System.DateTime> OsiUtcInidate { get; set; }
    public virtual System.Nullable<System.DateTime> OsiUtcEnddate { get; set; }
    public virtual System.Nullable<decimal> OsiAuthId { get; set; }
    public virtual System.Nullable<decimal> OsiPercVat1 { get; set; }
    public virtual System.Nullable<decimal> OsiPercVat2 { get; set; }
    public virtual System.Nullable<decimal> OsiPercFee { get; set; }
    public virtual System.Nullable<decimal> OsiPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OsiFixedFee { get; set; }
    public virtual System.Nullable<decimal> OsiPercBonus { get; set; }
    [Length(Max=50)]
    public virtual string OsiBonusId { get; set; }
    [Length(Max=500)]
    public virtual string OsiEysaMarca { get; set; }
    public virtual System.Nullable<int> OsiEysaTicketType { get; set; }

    public virtual MobileSession OsiMose { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(OperationsSessionInfo).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='OperationsSessionInfo'
         table='`OPERATIONS_SESSION_INFO`'
         >
    <id name='OsiId'
        column='`OSI_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OsiMoseId'
              column='`OSI_MOSE_ID`'
              />
    <property name='OsiUtcDate'
              column='`OSI_UTC_DATE`'
              />
    <property name='OsiPlate'
              column='`OSI_PLATE`'
              />
    <property name='OsiOpeType'
              column='`OSI_OPE_TYPE`'
              />
    <property name='OsiInsDate'
              column='`OSI_INS_DATE`'
              />
    <property name='OsiChangeApplied'
              column='`OSI_CHANGE_APPLIED`'
              />
    <property name='OsiGrpId'
              column='`OSI_GRP_ID`'
              />
    <property name='OsiTarId'
              column='`OSI_TAR_ID`'
              />
    <property name='OsiAmountToRefund'
              column='`OSI_AMOUNT_TO_REFUND`'
              />
    <property name='OsiTimeForAmount'
              column='`OSI_TIME_FOR_AMOUNT`'
              />
    <property name='OsiUtcInidate'
              column='`OSI_UTC_INIDATE`'
              />
    <property name='OsiUtcEnddate'
              column='`OSI_UTC_ENDDATE`'
              />
    <property name='OsiAuthId'
              column='`OSI_AUTH_ID`'
              />
    <property name='OsiPercVat1'
              column='`OSI_PERC_VAT1`'
              />
    <property name='OsiPercVat2'
              column='`OSI_PERC_VAT2`'
              />
    <property name='OsiPercFee'
              column='`OSI_PERC_FEE`'
              />
    <property name='OsiPercFeeTopped'
              column='`OSI_PERC_FEE_TOPPED`'
              />
    <property name='OsiFixedFee'
              column='`OSI_FIXED_FEE`'
              />
    <property name='OsiPercBonus'
              column='`OSI_PERC_BONUS`'
              />
    <property name='OsiBonusId'
              column='`OSI_BONUS_ID`'
              />
    <property name='OsiEysaMarca'
              column='`OSI_EYSA_MARCA`'
              />
    <property name='OsiEysaTicketType'
              column='`OSI_EYSA_TICKET_TYPE`'
              />
    <many-to-one name='OsiMose' class='MobileSession' column='`OSI_MOSE_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Operator
  {
    public virtual decimal OprId { get; set; }
    public virtual decimal OprVatPerc { get; set; }
    public virtual decimal OprInitialInvoiceNumber { get; set; }
    public virtual decimal OprEndInvoiceNumber { get; set; }
    public virtual decimal OprCurrentInvoiceNumber { get; set; }
    [Length(Max=50)]
    public virtual string OprDescription { get; set; }
    [Length(Max=50)]
    public virtual string OprVatNumber { get; set; }
    [Length(Max=100)]
    public virtual string OprNameForInvoice { get; set; }
    [Length(Max=200)]
    public virtual string OprAddressForInvoice { get; set; }
    [Length(Max=50)]
    public virtual string OprInvoiceNumberFormat { get; set; }
    [Length(Max=255)]
    public virtual string OprInvoiceFormatLastPageFile { get; set; }
    [Length(Max=255)]
    public virtual string OprInvoiceFormatNoLastPageFile { get; set; }
    public virtual int OprDefault { get; set; }
    public virtual int OprFeeLayout { get; set; }
    public virtual System.Nullable<decimal> OprServiceParkLitId { get; set; }
    public virtual System.Nullable<decimal> OprServiceFeeLitId { get; set; }
    public virtual System.Nullable<decimal> OprServiceFeeccLitId { get; set; }
    public virtual System.Nullable<decimal> OprServiceVatLitId { get; set; }
    public virtual System.Nullable<decimal> OprServiceTotalLitId { get; set; }
    public virtual System.Nullable<decimal> OprServiceSubtotalLitId { get; set; }

    private IList<CustomerInvoice> _customerInvoices = new List<CustomerInvoice>();

    public virtual IList<CustomerInvoice> CustomerInvoices
    {
      get { return _customerInvoices; }
      set { _customerInvoices = value; }
    }

    private IList<Installation> _installationsByInsOpr = new List<Installation>();

    public virtual IList<Installation> InstallationsByInsOpr
    {
      get { return _installationsByInsOpr; }
      set { _installationsByInsOpr = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Operator).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Operator'
         table='`OPERATORS`'
         >
    <id name='OprId'
        column='`OPR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OprVatPerc'
              column='`OPR_VAT_PERC`'
              />
    <property name='OprInitialInvoiceNumber'
              column='`OPR_INITIAL_INVOICE_NUMBER`'
              />
    <property name='OprEndInvoiceNumber'
              column='`OPR_END_INVOICE_NUMBER`'
              />
    <property name='OprCurrentInvoiceNumber'
              column='`OPR_CURRENT_INVOICE_NUMBER`'
              />
    <property name='OprDescription'
              column='`OPR_DESCRIPTION`'
              />
    <property name='OprVatNumber'
              column='`OPR_VAT_NUMBER`'
              />
    <property name='OprNameForInvoice'
              column='`OPR_NAME_FOR_INVOICE`'
              />
    <property name='OprAddressForInvoice'
              column='`OPR_ADDRESS_FOR_INVOICE`'
              />
    <property name='OprInvoiceNumberFormat'
              column='`OPR_INVOICE_NUMBER_FORMAT`'
              />
    <property name='OprInvoiceFormatLastPageFile'
              column='`OPR_INVOICE_FORMAT_LAST_PAGE_FILE`'
              />
    <property name='OprInvoiceFormatNoLastPageFile'
              column='`OPR_INVOICE_FORMAT_NO_LAST_PAGE_FILE`'
              />
    <property name='OprDefault'
              column='`OPR_DEFAULT`'
              />
    <property name='OprFeeLayout'
              column='`OPR_FEE_LAYOUT`'
              />
    <property name='OprServiceParkLitId'
              column='`OPR_SERVICE_PARK_LIT_ID`'
              />
    <property name='OprServiceFeeLitId'
              column='`OPR_SERVICE_FEE_LIT_ID`'
              />
    <property name='OprServiceFeeccLitId'
              column='`OPR_SERVICE_FEECC_LIT_ID`'
              />
    <property name='OprServiceVatLitId'
              column='`OPR_SERVICE_VAT_LIT_ID`'
              />
    <property name='OprServiceTotalLitId'
              column='`OPR_SERVICE_TOTAL_LIT_ID`'
              />
    <property name='OprServiceSubtotalLitId'
              column='`OPR_SERVICE_SUBTOTAL_LIT_ID`'
              />
    <bag name='CustomerInvoices'
          inverse='true'
          >
      <key column='`CUSINV_OPR_ID`' />
      <one-to-many class='CustomerInvoice' />
    </bag>
    <bag name='InstallationsByInsOpr'
          inverse='true'
          >
      <key column='`INS_OPR_ID`' />
      <one-to-many class='Installation' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Parameter
  {
    public virtual decimal ParId { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string ParName { get; set; }
    [Length(Max=1024)]
    public virtual string ParValue { get; set; }
    [Length(Max=1024)]
    public virtual string ParDescription { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Parameter).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Parameter'
         table='`PARAMETERS`'
         >
    <id name='ParId'
        column='`PAR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='ParName'
              column='`PAR_NAME`'
              />
    <property name='ParValue'
              column='`PAR_VALUE`'
              />
    <property name='ParDescription'
              column='`PAR_DESCRIPTION`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class PaymentSubtype
  {
    public virtual int PastId { get; set; }
    public virtual int PastPatId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string PastDescription { get; set; }
    public virtual System.Nullable<decimal> PastLitId { get; set; }
    public virtual System.Nullable<decimal> PastFixedFee { get; set; }
    public virtual System.Nullable<decimal> PastPercFee { get; set; }
    public virtual System.Nullable<decimal> PastFixedFeeCurId { get; set; }

    private IList<CustomerPaymentMean> _customerPaymentMeans = new List<CustomerPaymentMean>();

    public virtual IList<CustomerPaymentMean> CustomerPaymentMeans
    {
      get { return _customerPaymentMeans; }
      set { _customerPaymentMeans = value; }
    }

    public virtual Currency PastFixedFeeCur { get; set; }

    public virtual Literal PastLit { get; set; }

    public virtual PaymentType PastPat { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(PaymentSubtype).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='PaymentSubtype'
         table='`PAYMENT_SUBTYPES`'
         >
    <id name='PastId'
        column='`PAST_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='PastPatId'
              column='`PAST_PAT_ID`'
              />
    <property name='PastDescription'
              column='`PAST_DESCRIPTION`'
              />
    <property name='PastLitId'
              column='`PAST_LIT_ID`'
              />
    <property name='PastFixedFee'
              column='`PAST_FIXED_FEE`'
              />
    <property name='PastPercFee'
              column='`PAST_PERC_FEE`'
              />
    <property name='PastFixedFeeCurId'
              column='`PAST_FIXED_FEE_CUR_ID`'
              />
    <bag name='CustomerPaymentMeans'
          inverse='true'
          >
      <key column='`CUSPM_PAST_ID`' />
      <one-to-many class='CustomerPaymentMean' />
    </bag>
    <many-to-one name='PastFixedFeeCur' class='Currency' column='`PAST_FIXED_FEE_CUR_ID`' />
    <many-to-one name='PastLit' class='Literal' column='`PAST_LIT_ID`' />
    <many-to-one name='PastPat' class='PaymentType' column='`PAST_PAT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class PaymentType
  {
    public virtual int PatId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string PatDescripcion { get; set; }
    public virtual System.Nullable<decimal> PatLitId { get; set; }
    public virtual System.Nullable<decimal> PatFixedFee { get; set; }
    public virtual System.Nullable<decimal> PatPercFee { get; set; }
    public virtual System.Nullable<decimal> PatFixedFeeCurId { get; set; }

    private IList<CustomerPaymentMean> _customerPaymentMeans = new List<CustomerPaymentMean>();

    public virtual IList<CustomerPaymentMean> CustomerPaymentMeans
    {
      get { return _customerPaymentMeans; }
      set { _customerPaymentMeans = value; }
    }

    private IList<PaymentSubtype> _paymentSubtypes = new List<PaymentSubtype>();

    public virtual IList<PaymentSubtype> PaymentSubtypes
    {
      get { return _paymentSubtypes; }
      set { _paymentSubtypes = value; }
    }

    public virtual Currency PatFixedFeeCur { get; set; }

    public virtual Literal PatLit { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(PaymentType).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='PaymentType'
         table='`PAYMENT_TYPES`'
         >
    <id name='PatId'
        column='`PAT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='PatDescripcion'
              column='`PAT_DESCRIPCION`'
              />
    <property name='PatLitId'
              column='`PAT_LIT_ID`'
              />
    <property name='PatFixedFee'
              column='`PAT_FIXED_FEE`'
              />
    <property name='PatPercFee'
              column='`PAT_PERC_FEE`'
              />
    <property name='PatFixedFeeCurId'
              column='`PAT_FIXED_FEE_CUR_ID`'
              />
    <bag name='CustomerPaymentMeans'
          inverse='true'
          >
      <key column='`CUSPM_PAT_ID`' />
      <one-to-many class='CustomerPaymentMean' />
    </bag>
    <bag name='PaymentSubtypes'
          inverse='true'
          >
      <key column='`PAST_PAT_ID`' />
      <one-to-many class='PaymentSubtype' />
    </bag>
    <many-to-one name='PatFixedFeeCur' class='Currency' column='`PAT_FIXED_FEE_CUR_ID`' />
    <many-to-one name='PatLit' class='Literal' column='`PAT_LIT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class PlatesTariff
  {
    public virtual decimal PltaId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string PltaPlate { get; set; }
    public virtual decimal PltaTarId { get; set; }
    public virtual System.DateTime PltaIniApplyDate { get; set; }
    public virtual System.DateTime PltaEndApplyDate { get; set; }
    public virtual System.Nullable<decimal> PltaGrpId { get; set; }
    public virtual System.Nullable<decimal> PltaGrptId { get; set; }

    public virtual Group PltaGrp { get; set; }

    public virtual GroupsType PltaGrpt { get; set; }

    public virtual Tariff PltaTar { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(PlatesTariff).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='PlatesTariff'
         table='`PLATES_TARIFFS`'
         >
    <id name='PltaId'
        column='`PLTA_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='PltaPlate'
              column='`PLTA_PLATE`'
              />
    <property name='PltaTarId'
              column='`PLTA_TAR_ID`'
              />
    <property name='PltaIniApplyDate'
              column='`PLTA_INI_APPLY_DATE`'
              />
    <property name='PltaEndApplyDate'
              column='`PLTA_END_APPLY_DATE`'
              />
    <property name='PltaGrpId'
              column='`PLTA_GRP_ID`'
              />
    <property name='PltaGrptId'
              column='`PLTA_GRPT_ID`'
              />
    <many-to-one name='PltaGrp' class='Group' column='`PLTA_GRP_ID`' />
    <many-to-one name='PltaGrpt' class='GroupsType' column='`PLTA_GRPT_ID`' />
    <many-to-one name='PltaTar' class='Tariff' column='`PLTA_TAR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class PushidNotification
  {
    public virtual decimal PnoId { get; set; }
    public virtual decimal PnoUtnoId { get; set; }
    public virtual int PnoOs { get; set; }
    [NotNull]
    [Length(Max=512)]
    public virtual string PnoPushid { get; set; }
    public virtual int PnoStatus { get; set; }
    public virtual int PnoRetries { get; set; }
    public virtual System.Nullable<System.DateTime> PnoLastRetryDatetime { get; set; }
    [Length(Max=512)]
    public virtual string PnoWpText1 { get; set; }
    [Length(Max=512)]
    public virtual string PnoWpText2 { get; set; }
    [Length(Max=512)]
    public virtual string PnoWpParam { get; set; }
    public virtual System.Nullable<int> PnoWpCount { get; set; }
    [Length(Max=512)]
    public virtual string PnoWpTileTitle { get; set; }
    [Length(Max=512)]
    public virtual string PnoWpBackgroundImage { get; set; }
    [Length(Max=1024)]
    public virtual string PnoWpRawData { get; set; }
    [Length(Max=1024)]
    public virtual string PnoAndroidRawData { get; set; }
    [Length(Max=1024)]
    public virtual string PnoIOsRawData { get; set; }
    public virtual System.Nullable<System.DateTime> PnoLimitdatetime { get; set; }

    public virtual UsersNotification PnoUtno { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(PushidNotification).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='PushidNotification'
         table='`PUSHID_NOTIFICATIONS`'
         >
    <id name='PnoId'
        column='`PNO_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='PnoUtnoId'
              column='`PNO_UTNO_ID`'
              />
    <property name='PnoOs'
              column='`PNO_OS`'
              />
    <property name='PnoPushid'
              column='`PNO_PUSHID`'
              />
    <property name='PnoStatus'
              column='`PNO_STATUS`'
              />
    <property name='PnoRetries'
              column='`PNO_RETRIES`'
              />
    <property name='PnoLastRetryDatetime'
              column='`PNO_LAST_RETRY_DATETIME`'
              />
    <property name='PnoWpText1'
              column='`PNO_WP_TEXT1`'
              />
    <property name='PnoWpText2'
              column='`PNO_WP_TEXT2`'
              />
    <property name='PnoWpParam'
              column='`PNO_WP_PARAM`'
              />
    <property name='PnoWpCount'
              column='`PNO_WP_COUNT`'
              />
    <property name='PnoWpTileTitle'
              column='`PNO_WP_TILE_TITLE`'
              />
    <property name='PnoWpBackgroundImage'
              column='`PNO_WP_BACKGROUND_IMAGE`'
              />
    <property name='PnoWpRawData'
              column='`PNO_WP_RAW_DATA`'
              />
    <property name='PnoAndroidRawData'
              column='`PNO_ANDROID_RAW_DATA`'
              />
    <property name='PnoIOsRawData'
              column='`PNO_iOS_RAW_DATA`'
              />
    <property name='PnoLimitdatetime'
              column='`PNO_LIMITDATETIME`'
              />
    <many-to-one name='PnoUtno' class='UsersNotification' column='`PNO_UTNO_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class RechargeCoupon
  {
    public virtual decimal RcoupId { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string RcoupCode { get; set; }
    public virtual int RcoupCoupsId { get; set; }
    public virtual decimal RcoupValue { get; set; }
    public virtual decimal RcoupCurId { get; set; }
    public virtual System.DateTime RcoupStartDate { get; set; }
    public virtual System.DateTime RcoupExpDate { get; set; }
    public virtual System.Nullable<decimal> RcoupRtlpyId { get; set; }
    [Length(Max=100)]
    public virtual string RcoupKeycode { get; set; }

    private IList<CustomerPaymentMeansRecharge> _customerPaymentMeansRecharges = new List<CustomerPaymentMeansRecharge>();

    public virtual IList<CustomerPaymentMeansRecharge> CustomerPaymentMeansRecharges
    {
      get { return _customerPaymentMeansRecharges; }
      set { _customerPaymentMeansRecharges = value; }
    }

    private IList<RechargeCouponsUse> _rechargeCouponsUses = new List<RechargeCouponsUse>();

    public virtual IList<RechargeCouponsUse> RechargeCouponsUses
    {
      get { return _rechargeCouponsUses; }
      set { _rechargeCouponsUses = value; }
    }

    public virtual Currency RcoupCur { get; set; }

    public virtual RechargeCouponsStatus RcoupCoups { get; set; }

    public virtual RetailerPayment RcoupRtlpy { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(RechargeCoupon).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='RechargeCoupon'
         table='`RECHARGE_COUPONS`'
         >
    <id name='RcoupId'
        column='`RCOUP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RcoupCode'
              column='`RCOUP_CODE`'
              />
    <property name='RcoupCoupsId'
              column='`RCOUP_COUPS_ID`'
              />
    <property name='RcoupValue'
              column='`RCOUP_VALUE`'
              />
    <property name='RcoupCurId'
              column='`RCOUP_CUR_ID`'
              />
    <property name='RcoupStartDate'
              column='`RCOUP_START_DATE`'
              />
    <property name='RcoupExpDate'
              column='`RCOUP_EXP_DATE`'
              />
    <property name='RcoupRtlpyId'
              column='`RCOUP_RTLPY_ID`'
              />
    <property name='RcoupKeycode'
              column='`RCOUP_KEYCODE`'
              />
    <bag name='CustomerPaymentMeansRecharges'
          inverse='false'
          >
      <key column='`CUSPMR_RCOUP_ID`' />
      <one-to-many class='CustomerPaymentMeansRecharge' />
    </bag>
    <bag name='RechargeCouponsUses'
          inverse='true'
          >
      <key column='`RCOUPU_RCOUP_ID`' />
      <one-to-many class='RechargeCouponsUse' />
    </bag>
    <many-to-one name='RcoupCur' class='Currency' column='`RCOUP_CUR_ID`' />
    <many-to-one name='RcoupCoups' class='RechargeCouponsStatus' column='`RCOUP_COUPS_ID`' />
    <many-to-one name='RcoupRtlpy' class='RetailerPayment' column='`RCOUP_RTLPY_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class RechargeCouponsStatus
  {
    public virtual int RcoupsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RcoupsDescription { get; set; }
    public virtual System.Nullable<decimal> RcoupsLitId { get; set; }

    private IList<RechargeCoupon> _rechargeCoupons = new List<RechargeCoupon>();

    public virtual IList<RechargeCoupon> RechargeCoupons
    {
      get { return _rechargeCoupons; }
      set { _rechargeCoupons = value; }
    }

    public virtual Literal RcoupsLit { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(RechargeCouponsStatus).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='RechargeCouponsStatus'
         table='`RECHARGE_COUPONS_STATUS`'
         >
    <id name='RcoupsId'
        column='`RCOUPS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RcoupsDescription'
              column='`RCOUPS_DESCRIPTION`'
              />
    <property name='RcoupsLitId'
              column='`RCOUPS_LIT_ID`'
              />
    <bag name='RechargeCoupons'
          inverse='true'
          >
      <key column='`RCOUP_COUPS_ID`' />
      <one-to-many class='RechargeCoupon' />
    </bag>
    <many-to-one name='RcoupsLit' class='Literal' column='`RCOUPS_LIT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class RechargeCouponsUse
  {
    public virtual decimal RcoupuId { get; set; }
    public virtual decimal RcoupuUsrId { get; set; }
    public virtual decimal RcoupuRcoupId { get; set; }
    public virtual System.DateTime RcoupuDate { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RcoupuCode { get; set; }
    public virtual System.Nullable<decimal> RcoupuMoseId { get; set; }

    public virtual MobileSession RcoupuMose { get; set; }

    public virtual RechargeCoupon RcoupuRcoup { get; set; }

    public virtual User RcoupuUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(RechargeCouponsUse).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='RechargeCouponsUse'
         table='`RECHARGE_COUPONS_USES`'
         >
    <id name='RcoupuId'
        column='`RCOUPU_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RcoupuUsrId'
              column='`RCOUPU_USR_ID`'
              />
    <property name='RcoupuRcoupId'
              column='`RCOUPU_RCOUP_ID`'
              />
    <property name='RcoupuDate'
              column='`RCOUPU_DATE`'
              />
    <property name='RcoupuCode'
              column='`RCOUPU_CODE`'
              />
    <property name='RcoupuMoseId'
              column='`RCOUPU_MOSE_ID`'
              />
    <many-to-one name='RcoupuMose' class='MobileSession' column='`RCOUPU_MOSE_ID`' />
    <many-to-one name='RcoupuRcoup' class='RechargeCoupon' column='`RCOUPU_RCOUP_ID`' />
    <many-to-one name='RcoupuUsr' class='User' column='`RCOUPU_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class RechargeValue
  {
    public virtual decimal RechvalId { get; set; }
    public virtual decimal RechvalInsId { get; set; }
    public virtual int RechvalValueType { get; set; }
    public virtual int RechvalValue { get; set; }

    public virtual Installation RechvalIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(RechargeValue).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='RechargeValue'
         table='`RECHARGE_VALUES`'
         >
    <id name='RechvalId'
        column='`RECHVAL_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RechvalInsId'
              column='`RECHVAL_INS_ID`'
              />
    <property name='RechvalValueType'
              column='`RECHVAL_VALUE_TYPE`'
              />
    <property name='RechvalValue'
              column='`RECHVAL_VALUE`'
              />
    <many-to-one name='RechvalIns' class='Installation' column='`RECHVAL_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class RetailerPayment
  {
    public virtual decimal RtlpyId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RtlpyTransactionId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RtlpyGatewayDate { get; set; }
    public virtual decimal RtlpyTotalAmountCharged { get; set; }
    public virtual int RtlpySuscriptionType { get; set; }
    public virtual int RtlpyTransStatus { get; set; }
    public virtual System.DateTime RtlpyStatusDate { get; set; }
    public virtual int RtlpyRetriesNum { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RtlpyInvNumber { get; set; }
    public virtual int RtlpyAmount { get; set; }
    public virtual decimal RtlpyCurId { get; set; }
    public virtual decimal RtlpyServiceFixedFee { get; set; }
    public virtual decimal RtlpyServicePercFee { get; set; }
    public virtual System.Nullable<decimal> RtlpyInvschId { get; set; }
    public virtual System.Nullable<decimal> RtlpyRtlId { get; set; }
    public virtual System.Nullable<System.DateTime> RtlpyDate { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyOpReference { get; set; }
    public virtual System.Nullable<System.DateTime> RtlpyInsertionUtcDate { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyAuthCode { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyAuthResult { get; set; }
    [Length(Max=50)]
    public virtual string RtlpySecondOpReference { get; set; }
    [Length(Max=50)]
    public virtual string RtlpySecondTransactionId { get; set; }
    [Length(Max=50)]
    public virtual string RtlpySecondGatewayDate { get; set; }
    [Length(Max=50)]
    public virtual string RtlpySecondAuthResult { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyCardHash { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyCardReference { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyCardScheme { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyMaskedCardNumber { get; set; }
    public virtual System.Nullable<System.DateTime> RtlpyCardExpirationDate { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyPaypal3TToken { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyPaypal3TPayerId { get; set; }
    [Length(Max=50)]
    public virtual string RtlpyPaypalPreapprovedPayKey { get; set; }
    public virtual System.Nullable<decimal> RtlpyPercVat1 { get; set; }
    public virtual System.Nullable<decimal> RtlpyPercVat2 { get; set; }
    public virtual System.Nullable<decimal> RtlpyPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> RtlpyPercFee { get; set; }
    public virtual System.Nullable<decimal> RtlpyPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> RtlpyPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> RtlpyFixedFee { get; set; }
    public virtual System.Nullable<decimal> RtlpyPartialFixedFee { get; set; }
    public virtual int RtlpyCreditCardPaymentProvider { get; set; }

    private IList<RechargeCoupon> _rechargeCoupons = new List<RechargeCoupon>();

    public virtual IList<RechargeCoupon> RechargeCoupons
    {
      get { return _rechargeCoupons; }
      set { _rechargeCoupons = value; }
    }

    public virtual InvoicingSchema RtlpyInvsch { get; set; }

    public virtual Retailer RtlpyRtl { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(RetailerPayment).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='RetailerPayment'
         table='`RETAILER_PAYMENTS`'
         >
    <id name='RtlpyId'
        column='`RTLPY_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RtlpyTransactionId'
              column='`RTLPY_TRANSACTION_ID`'
              />
    <property name='RtlpyGatewayDate'
              column='`RTLPY_GATEWAY_DATE`'
              />
    <property name='RtlpyTotalAmountCharged'
              column='`RTLPY_TOTAL_AMOUNT_CHARGED`'
              />
    <property name='RtlpySuscriptionType'
              column='`RTLPY_SUSCRIPTION_TYPE`'
              />
    <property name='RtlpyTransStatus'
              column='`RTLPY_TRANS_STATUS`'
              />
    <property name='RtlpyStatusDate'
              column='`RTLPY_STATUS_DATE`'
              />
    <property name='RtlpyRetriesNum'
              column='`RTLPY_RETRIES_NUM`'
              />
    <property name='RtlpyInvNumber'
              column='`RTLPY_INV_NUMBER`'
              />
    <property name='RtlpyAmount'
              column='`RTLPY_AMOUNT`'
              />
    <property name='RtlpyCurId'
              column='`RTLPY_CUR_ID`'
              />
    <property name='RtlpyServiceFixedFee'
              column='`RTLPY_SERVICE_FIXED_FEE`'
              />
    <property name='RtlpyServicePercFee'
              column='`RTLPY_SERVICE_PERC_FEE`'
              />
    <property name='RtlpyInvschId'
              column='`RTLPY_INVSCH_ID`'
              />
    <property name='RtlpyRtlId'
              column='`RTLPY_RTL_ID`'
              />
    <property name='RtlpyDate'
              column='`RTLPY_DATE`'
              />
    <property name='RtlpyOpReference'
              column='`RTLPY_OP_REFERENCE`'
              />
    <property name='RtlpyInsertionUtcDate'
              column='`RTLPY_INSERTION_UTC_DATE`'
              />
    <property name='RtlpyAuthCode'
              column='`RTLPY_AUTH_CODE`'
              />
    <property name='RtlpyAuthResult'
              column='`RTLPY_AUTH_RESULT`'
              />
    <property name='RtlpySecondOpReference'
              column='`RTLPY_SECOND_OP_REFERENCE`'
              />
    <property name='RtlpySecondTransactionId'
              column='`RTLPY_SECOND_TRANSACTION_ID`'
              />
    <property name='RtlpySecondGatewayDate'
              column='`RTLPY_SECOND_GATEWAY_DATE`'
              />
    <property name='RtlpySecondAuthResult'
              column='`RTLPY_SECOND_AUTH_RESULT`'
              />
    <property name='RtlpyCardHash'
              column='`RTLPY_CARD_HASH`'
              />
    <property name='RtlpyCardReference'
              column='`RTLPY_CARD_REFERENCE`'
              />
    <property name='RtlpyCardScheme'
              column='`RTLPY_CARD_SCHEME`'
              />
    <property name='RtlpyMaskedCardNumber'
              column='`RTLPY_MASKED_CARD_NUMBER`'
              />
    <property name='RtlpyCardExpirationDate'
              column='`RTLPY_CARD_EXPIRATION_DATE`'
              />
    <property name='RtlpyPaypal3TToken'
              column='`RTLPY_PAYPAL_3T_TOKEN`'
              />
    <property name='RtlpyPaypal3TPayerId'
              column='`RTLPY_PAYPAL_3T_PAYER_ID`'
              />
    <property name='RtlpyPaypalPreapprovedPayKey'
              column='`RTLPY_PAYPAL_PREAPPROVED_PAY_KEY`'
              />
    <property name='RtlpyPercVat1'
              column='`RTLPY_PERC_VAT1`'
              />
    <property name='RtlpyPercVat2'
              column='`RTLPY_PERC_VAT2`'
              />
    <property name='RtlpyPartialVat1'
              column='`RTLPY_PARTIAL_VAT1`'
              />
    <property name='RtlpyPercFee'
              column='`RTLPY_PERC_FEE`'
              />
    <property name='RtlpyPercFeeTopped'
              column='`RTLPY_PERC_FEE_TOPPED`'
              />
    <property name='RtlpyPartialPercFee'
              column='`RTLPY_PARTIAL_PERC_FEE`'
              />
    <property name='RtlpyFixedFee'
              column='`RTLPY_FIXED_FEE`'
              />
    <property name='RtlpyPartialFixedFee'
              column='`RTLPY_PARTIAL_FIXED_FEE`'
              />
    <property name='RtlpyCreditCardPaymentProvider'
              column='`RTLPY_CREDIT_CARD_PAYMENT_PROVIDER`'
              />
    <bag name='RechargeCoupons'
          inverse='false'
          >
      <key column='`RCOUP_RTLPY_ID`' />
      <one-to-many class='RechargeCoupon' />
    </bag>
    <many-to-one name='RtlpyInvsch' class='InvoicingSchema' column='`RTLPY_INVSCH_ID`' />
    <many-to-one name='RtlpyRtl' class='Retailer' column='`RTLPY_RTL_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Retailer
  {
    public virtual decimal RtlId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RtlName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RtlEmail { get; set; }
    [NotNull]
    [Length(Max=200)]
    public virtual string RtlAddress { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string RtlDocId { get; set; }

    private IList<RetailerPayment> _retailerPayments = new List<RetailerPayment>();

    public virtual IList<RetailerPayment> RetailerPayments
    {
      get { return _retailerPayments; }
      set { _retailerPayments = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Retailer).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Retailer'
         table='`RETAILERS`'
         >
    <id name='RtlId'
        column='`RTL_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RtlName'
              column='`RTL_NAME`'
              />
    <property name='RtlEmail'
              column='`RTL_EMAIL`'
              />
    <property name='RtlAddress'
              column='`RTL_ADDRESS`'
              />
    <property name='RtlDocId'
              column='`RTL_DOC_ID`'
              />
    <bag name='RetailerPayments'
          inverse='false'
          >
      <key column='`RTLPY_RTL_ID`' />
      <one-to-many class='RetailerPayment' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class ServiceChargeType
  {
    public virtual int SechtId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string SechtDescripcion { get; set; }
    public virtual System.Nullable<decimal> SechtLitId { get; set; }

    private IList<ServiceCharge> _serviceCharges = new List<ServiceCharge>();

    public virtual IList<ServiceCharge> ServiceCharges
    {
      get { return _serviceCharges; }
      set { _serviceCharges = value; }
    }

    public virtual Literal SechtLit { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ServiceChargeType).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='ServiceChargeType'
         table='`SERVICE_CHARGE_TYPES`'
         >
    <id name='SechtId'
        column='`SECHT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='SechtDescripcion'
              column='`SECHT_DESCRIPCION`'
              />
    <property name='SechtLitId'
              column='`SECHT_LIT_ID`'
              />
    <bag name='ServiceCharges'
          inverse='true'
          >
      <key column='`SECH_SECHT_ID`' />
      <one-to-many class='ServiceCharge' />
    </bag>
    <many-to-one name='SechtLit' class='Literal' column='`SECHT_LIT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class ServiceCharge
  {
    public virtual decimal SechId { get; set; }
    public virtual int SechSechtId { get; set; }
    public virtual decimal SechUsrId { get; set; }
    public virtual int SechMoseOs { get; set; }
    public virtual System.DateTime SechDate { get; set; }
    public virtual System.DateTime SechUtcDate { get; set; }
    public virtual int SechDateUtcOffset { get; set; }
    public virtual int SechAmount { get; set; }
    public virtual decimal SechAmountCurId { get; set; }
    public virtual decimal SechBalanceCurId { get; set; }
    public virtual decimal SechChangeApplied { get; set; }
    public virtual decimal SechChangeFeeApplied { get; set; }
    public virtual int SechFinalAmount { get; set; }
    public virtual int SechSuscriptionType { get; set; }
    public virtual int SechBalanceBefore { get; set; }
    [Length(Max=20)]
    public virtual string SechAppVersion { get; set; }
    public virtual System.Nullable<System.DateTime> SechInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> SechCuspmrId { get; set; }
    public virtual System.Nullable<decimal> SechPercVat1 { get; set; }
    public virtual System.Nullable<decimal> SechPercVat2 { get; set; }
    public virtual System.Nullable<decimal> SechPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> SechPercFee { get; set; }
    public virtual System.Nullable<decimal> SechPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> SechPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> SechFixedFee { get; set; }
    public virtual System.Nullable<decimal> SechPartialFixedFee { get; set; }
    public virtual System.Nullable<int> SechTotalAmount { get; set; }
    public virtual System.Nullable<decimal> SechCusinvId { get; set; }

    public virtual Currency SechAmountCur { get; set; }

    public virtual Currency SechBalanceCur { get; set; }

    public virtual CustomerInvoice SechCusinv { get; set; }

    public virtual CustomerPaymentMeansRecharge SechCuspmr { get; set; }

    public virtual ServiceChargeType SechSecht { get; set; }

    public virtual User SechUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ServiceCharge).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='ServiceCharge'
         table='`SERVICE_CHARGES`'
         >
    <id name='SechId'
        column='`SECH_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='SechSechtId'
              column='`SECH_SECHT_ID`'
              />
    <property name='SechUsrId'
              column='`SECH_USR_ID`'
              />
    <property name='SechMoseOs'
              column='`SECH_MOSE_OS`'
              />
    <property name='SechDate'
              column='`SECH_DATE`'
              />
    <property name='SechUtcDate'
              column='`SECH_UTC_DATE`'
              />
    <property name='SechDateUtcOffset'
              column='`SECH_DATE_UTC_OFFSET`'
              />
    <property name='SechAmount'
              column='`SECH_AMOUNT`'
              />
    <property name='SechAmountCurId'
              column='`SECH_AMOUNT_CUR_ID`'
              />
    <property name='SechBalanceCurId'
              column='`SECH_BALANCE_CUR_ID`'
              />
    <property name='SechChangeApplied'
              column='`SECH_CHANGE_APPLIED`'
              />
    <property name='SechChangeFeeApplied'
              column='`SECH_CHANGE_FEE_APPLIED`'
              />
    <property name='SechFinalAmount'
              column='`SECH_FINAL_AMOUNT`'
              />
    <property name='SechSuscriptionType'
              column='`SECH_SUSCRIPTION_TYPE`'
              />
    <property name='SechBalanceBefore'
              column='`SECH_BALANCE_BEFORE`'
              />
    <property name='SechAppVersion'
              column='`SECH_APP_VERSION`'
              />
    <property name='SechInsertionUtcDate'
              column='`SECH_INSERTION_UTC_DATE`'
              />
    <property name='SechCuspmrId'
              column='`SECH_CUSPMR_ID`'
              />
    <property name='SechPercVat1'
              column='`SECH_PERC_VAT1`'
              />
    <property name='SechPercVat2'
              column='`SECH_PERC_VAT2`'
              />
    <property name='SechPartialVat1'
              column='`SECH_PARTIAL_VAT1`'
              />
    <property name='SechPercFee'
              column='`SECH_PERC_FEE`'
              />
    <property name='SechPercFeeTopped'
              column='`SECH_PERC_FEE_TOPPED`'
              />
    <property name='SechPartialPercFee'
              column='`SECH_PARTIAL_PERC_FEE`'
              />
    <property name='SechFixedFee'
              column='`SECH_FIXED_FEE`'
              />
    <property name='SechPartialFixedFee'
              column='`SECH_PARTIAL_FIXED_FEE`'
              />
    <property name='SechTotalAmount'
              column='`SECH_TOTAL_AMOUNT`'
              />
    <property name='SechCusinvId'
              column='`SECH_CUSINV_ID`'
              />
    <many-to-one name='SechAmountCur' class='Currency' column='`SECH_AMOUNT_CUR_ID`' />
    <many-to-one name='SechBalanceCur' class='Currency' column='`SECH_BALANCE_CUR_ID`' />
    <many-to-one name='SechCusinv' class='CustomerInvoice' column='`SECH_CUSINV_ID`' />
    <many-to-one name='SechCuspmr' class='CustomerPaymentMeansRecharge' column='`SECH_CUSPMR_ID`' />
    <many-to-one name='SechSecht' class='ServiceChargeType' column='`SECH_SECHT_ID`' />
    <many-to-one name='SechUsr' class='User' column='`SECH_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Tariff
  {
    public virtual decimal TarId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TarDescription { get; set; }
    public virtual decimal TarLitId { get; set; }
    public virtual decimal TarInsId { get; set; }
    [Length(Max=50)]
    public virtual string TarQueryExtId { get; set; }
    [Length(Max=50)]
    public virtual string TarExt1Id { get; set; }
    [Length(Max=50)]
    public virtual string TarExt2Id { get; set; }
    [Length(Max=50)]
    public virtual string TarExt3Id { get; set; }
    [Length(Max=50)]
    public virtual string TarIdForExtOps { get; set; }

    private IList<ExternalParkingOperation> _externalParkingOperations = new List<ExternalParkingOperation>();

    public virtual IList<ExternalParkingOperation> ExternalParkingOperations
    {
      get { return _externalParkingOperations; }
      set { _externalParkingOperations = value; }
    }

    private IList<GroupsTariffsExternalTranslation> _groupsTariffsExternalTranslations = new List<GroupsTariffsExternalTranslation>();

    public virtual IList<GroupsTariffsExternalTranslation> GroupsTariffsExternalTranslations
    {
      get { return _groupsTariffsExternalTranslations; }
      set { _groupsTariffsExternalTranslations = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<PlatesTariff> _platesTariffs = new List<PlatesTariff>();

    public virtual IList<PlatesTariff> PlatesTariffs
    {
      get { return _platesTariffs; }
      set { _platesTariffs = value; }
    }

    private IList<TariffsInGroup> _tariffsInGroups = new List<TariffsInGroup>();

    public virtual IList<TariffsInGroup> TariffsInGroups
    {
      get { return _tariffsInGroups; }
      set { _tariffsInGroups = value; }
    }

    public virtual Literal TarLit { get; set; }

    public virtual Installation TarIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Tariff).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Tariff'
         table='`TARIFFS`'
         >
    <id name='TarId'
        column='`TAR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='TarDescription'
              column='`TAR_DESCRIPTION`'
              />
    <property name='TarLitId'
              column='`TAR_LIT_ID`'
              />
    <property name='TarInsId'
              column='`TAR_INS_ID`'
              />
    <property name='TarQueryExtId'
              column='`TAR_QUERY_EXT_ID`'
              />
    <property name='TarExt1Id'
              column='`TAR_EXT1_ID`'
              />
    <property name='TarExt2Id'
              column='`TAR_EXT2_ID`'
              />
    <property name='TarExt3Id'
              column='`TAR_EXT3_ID`'
              />
    <property name='TarIdForExtOps'
              column='`TAR_ID_FOR_EXT_OPS`'
              />
    <bag name='ExternalParkingOperations'
          inverse='false'
          >
      <key column='`EPO_TARIFF`' />
      <one-to-many class='ExternalParkingOperation' />
    </bag>
    <bag name='GroupsTariffsExternalTranslations'
          inverse='true'
          >
      <key column='`GTET_IN_TAR_ID`' />
      <one-to-many class='GroupsTariffsExternalTranslation' />
    </bag>
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_TAR_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='PlatesTariffs'
          inverse='true'
          >
      <key column='`PLTA_TAR_ID`' />
      <one-to-many class='PlatesTariff' />
    </bag>
    <bag name='TariffsInGroups'
          inverse='true'
          >
      <key column='`TARGR_TAR_ID`' />
      <one-to-many class='TariffsInGroup' />
    </bag>
    <many-to-one name='TarLit' class='Literal' column='`TAR_LIT_ID`' />
    <many-to-one name='TarIns' class='Installation' column='`TAR_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class TariffsInGroup
  {
    public virtual decimal TargrId { get; set; }
    public virtual decimal TargrTarId { get; set; }
    public virtual int TargrTimeStepsValue { get; set; }
    public virtual decimal TargrLitId { get; set; }
    public virtual int TargrUserSelectable { get; set; }
    public virtual System.DateTime TargrIniApplyDate { get; set; }
    public virtual System.DateTime TargrEndApplyDate { get; set; }
    public virtual System.Nullable<decimal> TargrGrpId { get; set; }
    public virtual System.Nullable<decimal> TargrGrptId { get; set; }
    public virtual System.Nullable<int> TargrStep1Min { get; set; }
    public virtual System.Nullable<decimal> TargrStep1LitId { get; set; }
    public virtual System.Nullable<int> TargrStep2Min { get; set; }
    public virtual System.Nullable<decimal> TargrStep2LitId { get; set; }
    public virtual System.Nullable<int> TargrStep3Min { get; set; }
    public virtual System.Nullable<decimal> TargrStep3LitId { get; set; }

    public virtual Group TargrGrp { get; set; }

    public virtual GroupsType TargrGrpt { get; set; }

    public virtual Literal TargrLit { get; set; }

    public virtual Literal TargrStep1Lit { get; set; }

    public virtual Literal TargrStep2Lit { get; set; }

    public virtual Literal TargrStep3Lit { get; set; }

    public virtual Tariff TargrTar { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(TariffsInGroup).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='TariffsInGroup'
         table='`TARIFFS_IN_GROUPS`'
         >
    <id name='TargrId'
        column='`TARGR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='TargrTarId'
              column='`TARGR_TAR_ID`'
              />
    <property name='TargrTimeStepsValue'
              column='`TARGR_TIME_STEPS_VALUE`'
              />
    <property name='TargrLitId'
              column='`TARGR_LIT_ID`'
              />
    <property name='TargrUserSelectable'
              column='`TARGR_USER_SELECTABLE`'
              />
    <property name='TargrIniApplyDate'
              column='`TARGR_INI_APPLY_DATE`'
              />
    <property name='TargrEndApplyDate'
              column='`TARGR_END_APPLY_DATE`'
              />
    <property name='TargrGrpId'
              column='`TARGR_GRP_ID`'
              />
    <property name='TargrGrptId'
              column='`TARGR_GRPT_ID`'
              />
    <property name='TargrStep1Min'
              column='`TARGR_STEP1_MIN`'
              />
    <property name='TargrStep1LitId'
              column='`TARGR_STEP1_LIT_ID`'
              />
    <property name='TargrStep2Min'
              column='`TARGR_STEP2_MIN`'
              />
    <property name='TargrStep2LitId'
              column='`TARGR_STEP2_LIT_ID`'
              />
    <property name='TargrStep3Min'
              column='`TARGR_STEP3_MIN`'
              />
    <property name='TargrStep3LitId'
              column='`TARGR_STEP3_LIT_ID`'
              />
    <many-to-one name='TargrGrp' class='Group' column='`TARGR_GRP_ID`' />
    <many-to-one name='TargrGrpt' class='GroupsType' column='`TARGR_GRPT_ID`' />
    <many-to-one name='TargrLit' class='Literal' column='`TARGR_LIT_ID`' />
    <many-to-one name='TargrStep1Lit' class='Literal' column='`TARGR_STEP1_LIT_ID`' />
    <many-to-one name='TargrStep2Lit' class='Literal' column='`TARGR_STEP2_LIT_ID`' />
    <many-to-one name='TargrStep3Lit' class='Literal' column='`TARGR_STEP3_LIT_ID`' />
    <many-to-one name='TargrTar' class='Tariff' column='`TARGR_TAR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class TextNotification
  {
    public virtual decimal TenoId { get; set; }
    public virtual decimal TenoUsrId { get; set; }
    [NotNull]
    [Length(Max=255)]
    public virtual string TenoTitle { get; set; }
    [NotNull]
    [Length(Max=512)]
    public virtual string TenoMessage { get; set; }
    [Length(Max=255)]
    public virtual string TenoUrl { get; set; }

    public virtual User TenoUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(TextNotification).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='TextNotification'
         table='`TEXT_NOTIFICATIONS`'
         >
    <id name='TenoId'
        column='`TENO_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='TenoUsrId'
              column='`TENO_USR_ID`'
              />
    <property name='TenoTitle'
              column='`TENO_TITLE`'
              />
    <property name='TenoMessage'
              column='`TENO_MESSAGE`'
              />
    <property name='TenoUrl'
              column='`TENO_URL`'
              />
    <many-to-one name='TenoUsr' class='User' column='`TENO_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class TicketPayment
  {
    public virtual decimal TipaId { get; set; }
    public virtual decimal TipaUsrId { get; set; }
    public virtual int TipaMoseOs { get; set; }
    public virtual decimal TipaInsId { get; set; }
    public virtual System.DateTime TipaDate { get; set; }
    public virtual System.DateTime TipaUtcDate { get; set; }
    public virtual int TipaDateUtcOffset { get; set; }
    public virtual int TipaAmount { get; set; }
    public virtual decimal TipaAmountCurId { get; set; }
    public virtual decimal TipaBalanceCurId { get; set; }
    public virtual decimal TipaChangeApplied { get; set; }
    public virtual decimal TipaChangeFeeApplied { get; set; }
    public virtual int TipaFinalAmount { get; set; }
    public virtual int TipaSuscriptionType { get; set; }
    public virtual int TipaBalanceBefore { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TipaTicketNumber { get; set; }
    [Length(Max=300)]
    public virtual string TipaTicketData { get; set; }
    public virtual System.Nullable<int> TipaConfirmedInWs { get; set; }
    public virtual System.Nullable<int> TipaConfirmInWsRetriesNum { get; set; }
    public virtual System.Nullable<System.DateTime> TipaConfirmInWsDate { get; set; }
    public virtual System.Nullable<decimal> TipaLatitude { get; set; }
    public virtual System.Nullable<decimal> TipaLongitude { get; set; }
    [Length(Max=20)]
    public virtual string TipaAppVersion { get; set; }
    public virtual System.Nullable<decimal> TipaConfirmationTimeInWs { get; set; }
    public virtual System.Nullable<int> TipaQueueLengthBeforeConfirmWs { get; set; }
    [Length(Max=50)]
    public virtual string TipaExternalId { get; set; }
    public virtual System.Nullable<System.DateTime> TipaInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> TipaCuspmrId { get; set; }
    public virtual System.Nullable<decimal> TipaOpedisId { get; set; }
    public virtual System.Nullable<decimal> TipaUsrpId { get; set; }
    [Length(Max=50)]
    public virtual string TipaPlateString { get; set; }
    public virtual System.Nullable<decimal> TipaGrpId { get; set; }
    public virtual System.Nullable<decimal> TipaPercVat1 { get; set; }
    public virtual System.Nullable<decimal> TipaPercVat2 { get; set; }
    public virtual System.Nullable<decimal> TipaPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> TipaPercFee { get; set; }
    public virtual System.Nullable<decimal> TipaPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> TipaPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> TipaFixedFee { get; set; }
    public virtual System.Nullable<decimal> TipaPartialFixedFee { get; set; }
    public virtual System.Nullable<int> TipaTotalAmount { get; set; }
    public virtual System.Nullable<decimal> TipaCusinvId { get; set; }

    public virtual Currency TipaAmountCur { get; set; }

    public virtual Currency TipaBalanceCur { get; set; }

    public virtual CustomerInvoice TipaCusinv { get; set; }

    public virtual CustomerPaymentMeansRecharge TipaCuspmr { get; set; }

    public virtual Group TipaGrp { get; set; }

    public virtual OperationsDiscount TipaOpedis { get; set; }

    public virtual User TipaUsr { get; set; }

    public virtual Installation TipaIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(TicketPayment).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='TicketPayment'
         table='`TICKET_PAYMENTS`'
         >
    <id name='TipaId'
        column='`TIPA_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='TipaUsrId'
              column='`TIPA_USR_ID`'
              />
    <property name='TipaMoseOs'
              column='`TIPA_MOSE_OS`'
              />
    <property name='TipaInsId'
              column='`TIPA_INS_ID`'
              />
    <property name='TipaDate'
              column='`TIPA_DATE`'
              />
    <property name='TipaUtcDate'
              column='`TIPA_UTC_DATE`'
              />
    <property name='TipaDateUtcOffset'
              column='`TIPA_DATE_UTC_OFFSET`'
              />
    <property name='TipaAmount'
              column='`TIPA_AMOUNT`'
              />
    <property name='TipaAmountCurId'
              column='`TIPA_AMOUNT_CUR_ID`'
              />
    <property name='TipaBalanceCurId'
              column='`TIPA_BALANCE_CUR_ID`'
              />
    <property name='TipaChangeApplied'
              column='`TIPA_CHANGE_APPLIED`'
              />
    <property name='TipaChangeFeeApplied'
              column='`TIPA_CHANGE_FEE_APPLIED`'
              />
    <property name='TipaFinalAmount'
              column='`TIPA_FINAL_AMOUNT`'
              />
    <property name='TipaSuscriptionType'
              column='`TIPA_SUSCRIPTION_TYPE`'
              />
    <property name='TipaBalanceBefore'
              column='`TIPA_BALANCE_BEFORE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='TipaConfirmedInWs'
              column='`TIPA_CONFIRMED_IN_WS`'
              />
    <property name='TipaConfirmInWsRetriesNum'
              column='`TIPA_CONFIRM_IN_WS_RETRIES_NUM`'
              />
    <property name='TipaConfirmInWsDate'
              column='`TIPA_CONFIRM_IN_WS_DATE`'
              />
    <property name='TipaLatitude'
              column='`TIPA_LATITUDE`'
              />
    <property name='TipaLongitude'
              column='`TIPA_LONGITUDE`'
              />
    <property name='TipaAppVersion'
              column='`TIPA_APP_VERSION`'
              />
    <property name='TipaConfirmationTimeInWs'
              column='`TIPA_CONFIRMATION_TIME_IN_WS`'
              />
    <property name='TipaQueueLengthBeforeConfirmWs'
              column='`TIPA_QUEUE_LENGTH_BEFORE_CONFIRM_WS`'
              />
    <property name='TipaExternalId'
              column='`TIPA_EXTERNAL_ID`'
              />
    <property name='TipaInsertionUtcDate'
              column='`TIPA_INSERTION_UTC_DATE`'
              />
    <property name='TipaCuspmrId'
              column='`TIPA_CUSPMR_ID`'
              />
    <property name='TipaOpedisId'
              column='`TIPA_OPEDIS_ID`'
              />
    <property name='TipaUsrpId'
              column='`TIPA_USRP_ID`'
              />
    <property name='TipaPlateString'
              column='`TIPA_PLATE_STRING`'
              />
    <property name='TipaGrpId'
              column='`TIPA_GRP_ID`'
              />
    <property name='TipaPercVat1'
              column='`TIPA_PERC_VAT1`'
              />
    <property name='TipaPercVat2'
              column='`TIPA_PERC_VAT2`'
              />
    <property name='TipaPartialVat1'
              column='`TIPA_PARTIAL_VAT1`'
              />
    <property name='TipaPercFee'
              column='`TIPA_PERC_FEE`'
              />
    <property name='TipaPercFeeTopped'
              column='`TIPA_PERC_FEE_TOPPED`'
              />
    <property name='TipaPartialPercFee'
              column='`TIPA_PARTIAL_PERC_FEE`'
              />
    <property name='TipaFixedFee'
              column='`TIPA_FIXED_FEE`'
              />
    <property name='TipaPartialFixedFee'
              column='`TIPA_PARTIAL_FIXED_FEE`'
              />
    <property name='TipaTotalAmount'
              column='`TIPA_TOTAL_AMOUNT`'
              />
    <property name='TipaCusinvId'
              column='`TIPA_CUSINV_ID`'
              />
    <many-to-one name='TipaAmountCur' class='Currency' column='`TIPA_AMOUNT_CUR_ID`' />
    <many-to-one name='TipaBalanceCur' class='Currency' column='`TIPA_BALANCE_CUR_ID`' />
    <many-to-one name='TipaCusinv' class='CustomerInvoice' column='`TIPA_CUSINV_ID`' />
    <many-to-one name='TipaCuspmr' class='CustomerPaymentMeansRecharge' column='`TIPA_CUSPMR_ID`' />
    <many-to-one name='TipaGrp' class='Group' column='`TIPA_GRP_ID`' />
    <many-to-one name='TipaOpedis' class='OperationsDiscount' column='`TIPA_OPEDIS_ID`' />
    <many-to-one name='TipaUsr' class='User' column='`TIPA_USR_ID`' />
    <many-to-one name='TipaIns' class='Installation' column='`TIPA_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class TicketPaymentsSessionInfo
  {
    public virtual decimal TpsiId { get; set; }
    public virtual decimal TpsiMoseId { get; set; }
    public virtual System.DateTime TpsiUtcDate { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TpsiTicketNumber { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TpsiPlate { get; set; }
    public virtual decimal TpsiAmount { get; set; }
    public virtual System.DateTime TpsiInsDate { get; set; }
    public virtual decimal TpsiChangeApplied { get; set; }
    [Length(Max=512)]
    public virtual string TpsiArticleType { get; set; }
    [Length(Max=512)]
    public virtual string TpsiArticleDescription { get; set; }
    public virtual System.Nullable<decimal> TpsiAuthId { get; set; }
    public virtual System.Nullable<decimal> TpsiGrpId { get; set; }
    public virtual System.Nullable<decimal> TpsiPercVat1 { get; set; }
    public virtual System.Nullable<decimal> TpsiPercVat2 { get; set; }
    public virtual System.Nullable<decimal> TpsiPercFee { get; set; }
    public virtual System.Nullable<decimal> TpsiPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> TpsiFixedFee { get; set; }

    public virtual Group TpsiGrp { get; set; }

    public virtual MobileSession TpsiMose { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(TicketPaymentsSessionInfo).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='TicketPaymentsSessionInfo'
         table='`TICKET_PAYMENTS_SESSION_INFO`'
         >
    <id name='TpsiId'
        column='`TPSI_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='TpsiMoseId'
              column='`TPSI_MOSE_ID`'
              />
    <property name='TpsiUtcDate'
              column='`TPSI_UTC_DATE`'
              />
    <property name='TpsiTicketNumber'
              column='`TPSI_TICKET_NUMBER`'
              />
    <property name='TpsiPlate'
              column='`TPSI_PLATE`'
              />
    <property name='TpsiAmount'
              column='`TPSI_AMOUNT`'
              />
    <property name='TpsiInsDate'
              column='`TPSI_INS_DATE`'
              />
    <property name='TpsiChangeApplied'
              column='`TPSI_CHANGE_APPLIED`'
              />
    <property name='TpsiArticleType'
              column='`TPSI_ARTICLE_TYPE`'
              />
    <property name='TpsiArticleDescription'
              column='`TPSI_ARTICLE_DESCRIPTION`'
              />
    <property name='TpsiAuthId'
              column='`TPSI_AUTH_ID`'
              />
    <property name='TpsiGrpId'
              column='`TPSI_GRP_ID`'
              />
    <property name='TpsiPercVat1'
              column='`TPSI_PERC_VAT1`'
              />
    <property name='TpsiPercVat2'
              column='`TPSI_PERC_VAT2`'
              />
    <property name='TpsiPercFee'
              column='`TPSI_PERC_FEE`'
              />
    <property name='TpsiPercFeeTopped'
              column='`TPSI_PERC_FEE_TOPPED`'
              />
    <property name='TpsiFixedFee'
              column='`TPSI_FIXED_FEE`'
              />
    <many-to-one name='TpsiGrp' class='Group' column='`TPSI_GRP_ID`' />
    <many-to-one name='TpsiMose' class='MobileSession' column='`TPSI_MOSE_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UserPlateMov
  {
    public virtual decimal UsrpmId { get; set; }
    [NotNull]
    [Length(Max=1)]
    public virtual string UsrpmMovType { get; set; }
    public virtual System.DateTime UsrpmDate { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrpmPlate { get; set; }
    public virtual int UsrpmSendInsertion { get; set; }

    private IList<UserPlateMovsSending> _userPlateMovsSendings = new List<UserPlateMovsSending>();

    public virtual IList<UserPlateMovsSending> UserPlateMovsSendings
    {
      get { return _userPlateMovsSendings; }
      set { _userPlateMovsSendings = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UserPlateMov).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UserPlateMov'
         table='`USER_PLATE_MOVS`'
         >
    <id name='UsrpmId'
        column='`USRPM_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrpmMovType'
              column='`USRPM_MOV_TYPE`'
              />
    <property name='UsrpmDate'
              column='`USRPM_DATE`'
              />
    <property name='UsrpmPlate'
              column='`USRPM_PLATE`'
              />
    <property name='UsrpmSendInsertion'
              column='`USRPM_SEND_INSERTION`'
              />
    <bag name='UserPlateMovsSendings'
          inverse='true'
          >
      <key column='`USRPMS_USRPMD_ID`' />
      <one-to-many class='UserPlateMovsSending' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UserPlateMovsSending
  {
    public virtual decimal UsrpmsId { get; set; }
    public virtual decimal UsrpmsUsrpmdId { get; set; }
    public virtual decimal UsrpmsInsId { get; set; }
    public virtual System.DateTime UsrpmsLastDate { get; set; }
    public virtual int UsrpmsStatus { get; set; }

    public virtual UserPlateMov UsrpmsUsrpmd { get; set; }

    public virtual Installation UsrpmsIns { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UserPlateMovsSending).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UserPlateMovsSending'
         table='`USER_PLATE_MOVS_SENDING`'
         >
    <id name='UsrpmsId'
        column='`USRPMS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrpmsUsrpmdId'
              column='`USRPMS_USRPMD_ID`'
              />
    <property name='UsrpmsInsId'
              column='`USRPMS_INS_ID`'
              />
    <property name='UsrpmsLastDate'
              column='`USRPMS_LAST_DATE`'
              />
    <property name='UsrpmsStatus'
              column='`USRPMS_STATUS`'
              />
    <many-to-one name='UsrpmsUsrpmd' class='UserPlateMov' column='`USRPMS_USRPMD_ID`' />
    <many-to-one name='UsrpmsIns' class='Installation' column='`USRPMS_INS_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UserPlate
  {
    public virtual decimal UsrpId { get; set; }
    public virtual decimal UsrpUsrId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrpPlate { get; set; }
    public virtual int UsrpIsDefault { get; set; }
    public virtual int UsrpEnabled { get; set; }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    public virtual User UsrpUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UserPlate).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UserPlate'
         table='`USER_PLATES`'
         >
    <id name='UsrpId'
        column='`USRP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrpUsrId'
              column='`USRP_USR_ID`'
              />
    <property name='UsrpPlate'
              column='`USRP_PLATE`'
              />
    <property name='UsrpIsDefault'
              column='`USRP_IS_DEFAULT`'
              />
    <property name='UsrpEnabled'
              column='`USRP_ENABLED`'
              />
    <bag name='OperationsOffstreets'
          inverse='true'
          >
      <key column='`OPEOFF_USRP_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='Operations'
          inverse='false'
          >
      <key column='`OPE_USRP_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <many-to-one name='UsrpUsr' class='User' column='`USRP_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class User
  {
    public virtual decimal UsrId { get; set; }
    public virtual decimal UsrCusId { get; set; }
    public virtual System.DateTime UsrInsertUtcDate { get; set; }
    public virtual decimal UsrCouId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrEmail { get; set; }
    public virtual decimal UsrMainTelCountryId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrMainTel { get; set; }
    public virtual int UsrBalance { get; set; }
    public virtual decimal UsrCurId { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string UsrCultureLang { get; set; }
    public virtual int UsrUtcOffset { get; set; }
    public virtual int UsrEnabled { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    public virtual System.Nullable<int> UsrSuscriptionType { get; set; }
    public virtual System.Nullable<decimal> UsrCuspmId { get; set; }
    public virtual System.Nullable<decimal> UsrSecundTelCountryId { get; set; }
    [Length(Max=50)]
    public virtual string UsrSecundTel { get; set; }
    public virtual System.Nullable<decimal> UsrLctId { get; set; }
    public virtual System.Nullable<int> UsrInsertMoseOs { get; set; }
    public virtual System.Nullable<System.DateTime> UsrDisableUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> UsrOperativeUtcDate { get; set; }
    [Length(Max=256)]
    public virtual string UsrPagateliaLastUser { get; set; }
    [Length(Max=128)]
    public virtual string UsrPagateliaLastPwd { get; set; }
    public virtual int UsrPaymeth { get; set; }

    private IList<CustomerPaymentMeansRecharge> _customerPaymentMeansRecharges = new List<CustomerPaymentMeansRecharge>();

    public virtual IList<CustomerPaymentMeansRecharge> CustomerPaymentMeansRecharges
    {
      get { return _customerPaymentMeansRecharges; }
      set { _customerPaymentMeansRecharges = value; }
    }

    private IList<Customer> _customers = new List<Customer>();

    public virtual IList<Customer> Customers
    {
      get { return _customers; }
      set { _customers = value; }
    }

    private IList<MobileSession> _mobileSessions = new List<MobileSession>();

    public virtual IList<MobileSession> MobileSessions
    {
      get { return _mobileSessions; }
      set { _mobileSessions = value; }
    }

    private IList<OperationsDiscount> _operationsDiscounts = new List<OperationsDiscount>();

    public virtual IList<OperationsDiscount> OperationsDiscounts
    {
      get { return _operationsDiscounts; }
      set { _operationsDiscounts = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<RechargeCouponsUse> _rechargeCouponsUses = new List<RechargeCouponsUse>();

    public virtual IList<RechargeCouponsUse> RechargeCouponsUses
    {
      get { return _rechargeCouponsUses; }
      set { _rechargeCouponsUses = value; }
    }

    private IList<ServiceCharge> _serviceCharges = new List<ServiceCharge>();

    public virtual IList<ServiceCharge> ServiceCharges
    {
      get { return _serviceCharges; }
      set { _serviceCharges = value; }
    }

    private IList<TextNotification> _textNotifications = new List<TextNotification>();

    public virtual IList<TextNotification> TextNotifications
    {
      get { return _textNotifications; }
      set { _textNotifications = value; }
    }

    private IList<TicketPayment> _ticketPayments = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPayments
    {
      get { return _ticketPayments; }
      set { _ticketPayments = value; }
    }

    private IList<UserPlate> _userPlates = new List<UserPlate>();

    public virtual IList<UserPlate> UserPlates
    {
      get { return _userPlates; }
      set { _userPlates = value; }
    }

    private IList<UsersEmail> _usersEmails = new List<UsersEmail>();

    public virtual IList<UsersEmail> UsersEmails
    {
      get { return _usersEmails; }
      set { _usersEmails = value; }
    }

    private IList<UsersPushId> _usersPushIds = new List<UsersPushId>();

    public virtual IList<UsersPushId> UsersPushIds
    {
      get { return _usersPushIds; }
      set { _usersPushIds = value; }
    }

    private IList<UsersSecurityOperation> _usersSecurityOperations = new List<UsersSecurityOperation>();

    public virtual IList<UsersSecurityOperation> UsersSecurityOperations
    {
      get { return _usersSecurityOperations; }
      set { _usersSecurityOperations = value; }
    }

    private IList<UsersSmss> _usersSmsses = new List<UsersSmss>();

    public virtual IList<UsersSmss> UsersSmsses
    {
      get { return _usersSmsses; }
      set { _usersSmsses = value; }
    }

    private IList<UsersNotification> _usersNotifications = new List<UsersNotification>();

    public virtual IList<UsersNotification> UsersNotifications
    {
      get { return _usersNotifications; }
      set { _usersNotifications = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatSrcUsr = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatSrcUsr
    {
      get { return _balanceTransfersByBatSrcUsr; }
      set { _balanceTransfersByBatSrcUsr = value; }
    }

    private IList<BalanceTransfer> _balanceTransfersByBatDstUsr = new List<BalanceTransfer>();

    public virtual IList<BalanceTransfer> BalanceTransfersByBatDstUsr
    {
      get { return _balanceTransfersByBatDstUsr; }
      set { _balanceTransfersByBatDstUsr = value; }
    }

    private IList<UsersFriend> _usersFriendsByUsrfAcceptUsr = new List<UsersFriend>();

    public virtual IList<UsersFriend> UsersFriendsByUsrfAcceptUsr
    {
      get { return _usersFriendsByUsrfAcceptUsr; }
      set { _usersFriendsByUsrfAcceptUsr = value; }
    }

    private IList<UsersFriend> _usersFriendsByUsrfUsr = new List<UsersFriend>();

    public virtual IList<UsersFriend> UsersFriendsByUsrfUsr
    {
      get { return _usersFriendsByUsrfUsr; }
      set { _usersFriendsByUsrfUsr = value; }
    }

    public virtual Country UsrCou { get; set; }

    public virtual Country UsrMainTelCountry { get; set; }

    public virtual Country UsrSecundTelCountry { get; set; }

    public virtual Currency UsrCur { get; set; }

    public virtual CustomerPaymentMean UsrCuspm { get; set; }

    public virtual Customer UsrCus { get; set; }

    public virtual LicenseTerm UsrLct { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(User).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='User'
         table='`USERS`'
         >
    <id name='UsrId'
        column='`USR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrCusId'
              column='`USR_CUS_ID`'
              />
    <property name='UsrInsertUtcDate'
              column='`USR_INSERT_UTC_DATE`'
              />
    <property name='UsrCouId'
              column='`USR_COU_ID`'
              />
    <property name='UsrEmail'
              column='`USR_EMAIL`'
              />
    <property name='UsrMainTelCountryId'
              column='`USR_MAIN_TEL_COUNTRY`'
              />
    <property name='UsrMainTel'
              column='`USR_MAIN_TEL`'
              />
    <property name='UsrBalance'
              column='`USR_BALANCE`'
              />
    <property name='UsrCurId'
              column='`USR_CUR_ID`'
              />
    <property name='UsrCultureLang'
              column='`USR_CULTURE_LANG`'
              />
    <property name='UsrUtcOffset'
              column='`USR_UTC_OFFSET`'
              />
    <property name='UsrEnabled'
              column='`USR_ENABLED`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='UsrSuscriptionType'
              column='`USR_SUSCRIPTION_TYPE`'
              />
    <property name='UsrCuspmId'
              column='`USR_CUSPM_ID`'
              />
    <property name='UsrSecundTelCountryId'
              column='`USR_SECUND_TEL_COUNTRY`'
              />
    <property name='UsrSecundTel'
              column='`USR_SECUND_TEL`'
              />
    <property name='UsrLctId'
              column='`USR_LCT_ID`'
              />
    <property name='UsrInsertMoseOs'
              column='`USR_INSERT_MOSE_OS`'
              />
    <property name='UsrDisableUtcDate'
              column='`USR_DISABLE_UTC_DATE`'
              />
    <property name='UsrOperativeUtcDate'
              column='`USR_OPERATIVE_UTC_DATE`'
              />
    <property name='UsrPagateliaLastUser'
              column='`USR_PAGATELIA_LAST_USER`'
              />
    <property name='UsrPagateliaLastPwd'
              column='`USR_PAGATELIA_LAST_PWD`'
              />
    <property name='UsrPaymeth'
              column='`USR_PAYMETH`'
              />
    <bag name='CustomerPaymentMeansRecharges'
          inverse='false'
          >
      <key column='`CUSPMR_USR_ID`' />
      <one-to-many class='CustomerPaymentMeansRecharge' />
    </bag>
    <bag name='Customers'
          inverse='false'
          >
      <key column='`CUS_USR_ID`' />
      <one-to-many class='Customer' />
    </bag>
    <bag name='MobileSessions'
          inverse='true'
          >
      <key column='`MOSE_USR_ID`' />
      <one-to-many class='MobileSession' />
    </bag>
    <bag name='OperationsDiscounts'
          inverse='true'
          >
      <key column='`OPEDIS_USR_ID`' />
      <one-to-many class='OperationsDiscount' />
    </bag>
    <bag name='OperationsOffstreets'
          inverse='true'
          >
      <key column='`OPEOFF_USR_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='Operations'
          inverse='true'
          >
      <key column='`OPE_USR_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='RechargeCouponsUses'
          inverse='true'
          >
      <key column='`RCOUPU_USR_ID`' />
      <one-to-many class='RechargeCouponsUse' />
    </bag>
    <bag name='ServiceCharges'
          inverse='true'
          >
      <key column='`SECH_USR_ID`' />
      <one-to-many class='ServiceCharge' />
    </bag>
    <bag name='TextNotifications'
          inverse='true'
          >
      <key column='`TENO_USR_ID`' />
      <one-to-many class='TextNotification' />
    </bag>
    <bag name='TicketPayments'
          inverse='true'
          >
      <key column='`TIPA_USR_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <bag name='UserPlates'
          inverse='true'
          >
      <key column='`USRP_USR_ID`' />
      <one-to-many class='UserPlate' />
    </bag>
    <bag name='UsersEmails'
          inverse='false'
          >
      <key column='`USRE_USR_ID`' />
      <one-to-many class='UsersEmail' />
    </bag>
    <bag name='UsersPushIds'
          inverse='true'
          >
      <key column='`UPID_USR_ID`' />
      <one-to-many class='UsersPushId' />
    </bag>
    <bag name='UsersSecurityOperations'
          inverse='true'
          >
      <key column='`USOP_USR_ID`' />
      <one-to-many class='UsersSecurityOperation' />
    </bag>
    <bag name='UsersSmsses'
          inverse='false'
          >
      <key column='`USRS_USR_ID`' />
      <one-to-many class='UsersSmss' />
    </bag>
    <bag name='UsersNotifications'
          inverse='true'
          >
      <key column='`UNO_USR_ID`' />
      <one-to-many class='UsersNotification' />
    </bag>
    <bag name='BalanceTransfersByBatSrcUsr'
          inverse='true'
          >
      <key column='`BAT_SRC_USR_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
    <bag name='BalanceTransfersByBatDstUsr'
          inverse='true'
          >
      <key column='`BAT_DST_USR_ID`' />
      <one-to-many class='BalanceTransfer' />
    </bag>
    <bag name='UsersFriendsByUsrfAcceptUsr'
          inverse='false'
          >
      <key column='`USRF_ACCEPT_USR_ID`' />
      <one-to-many class='UsersFriend' />
    </bag>
    <bag name='UsersFriendsByUsrfUsr'
          inverse='true'
          >
      <key column='`USRF_USR_ID`' />
      <one-to-many class='UsersFriend' />
    </bag>
    <many-to-one name='UsrCou' class='Country' column='`USR_COU_ID`' />
    <many-to-one name='UsrMainTelCountry' class='Country' column='`USR_MAIN_TEL_COUNTRY`' />
    <many-to-one name='UsrSecundTelCountry' class='Country' column='`USR_SECUND_TEL_COUNTRY`' />
    <many-to-one name='UsrCur' class='Currency' column='`USR_CUR_ID`' />
    <many-to-one name='UsrCuspm' class='CustomerPaymentMean' column='`USR_CUSPM_ID`' />
    <many-to-one name='UsrCus' class='Customer' column='`USR_CUS_ID`' />
    <many-to-one name='UsrLct' class='LicenseTerm' column='`USR_LCT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UsersEmail
  {
    public virtual decimal UsreId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsreRecipientAddress { get; set; }
    [NotNull]
    [Length(Max=512)]
    public virtual string UsreSubject { get; set; }
    [NotNull]
    [Length(Max=8000)]
    public virtual string UsreBody { get; set; }
    public virtual int UsreStatus { get; set; }
    public virtual System.Nullable<decimal> UsreSenderId { get; set; }
    public virtual System.Nullable<decimal> UsreUsrId { get; set; }
    public virtual System.Nullable<decimal> UsreCusinsId { get; set; }
    public virtual System.Nullable<System.DateTime> UsreDate { get; set; }

    public virtual CustomerInscription UsreCusins { get; set; }

    public virtual User UsreUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UsersEmail).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UsersEmail'
         table='`USERS_EMAILS`'
         >
    <id name='UsreId'
        column='`USRE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsreRecipientAddress'
              column='`USRE_RECIPIENT_ADDRESS`'
              />
    <property name='UsreSubject'
              column='`USRE_SUBJECT`'
              />
    <property name='UsreBody'
              column='`USRE_BODY`'
              />
    <property name='UsreStatus'
              column='`USRE_STATUS`'
              />
    <property name='UsreSenderId'
              column='`USRE_SENDER_ID`'
              />
    <property name='UsreUsrId'
              column='`USRE_USR_ID`'
              />
    <property name='UsreCusinsId'
              column='`USRE_CUSINS_ID`'
              />
    <property name='UsreDate'
              column='`USRE_DATE`'
              />
    <many-to-one name='UsreCusins' class='CustomerInscription' column='`USRE_CUSINS_ID`' />
    <many-to-one name='UsreUsr' class='User' column='`USRE_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UsersNotification
  {
    public virtual decimal UnoId { get; set; }
    public virtual decimal UnoUsrId { get; set; }
    public virtual int UnoStatus { get; set; }
    [Length(Max=512)]
    public virtual string UnoWpText1 { get; set; }
    [Length(Max=512)]
    public virtual string UnoWpText2 { get; set; }
    [Length(Max=512)]
    public virtual string UnoWpParam { get; set; }
    public virtual System.Nullable<int> UnoWpCount { get; set; }
    [Length(Max=512)]
    public virtual string UnoWpTileTitle { get; set; }
    [Length(Max=512)]
    public virtual string UnoWpBackgroundImage { get; set; }
    [Length(Max=1024)]
    public virtual string UnoWpRawData { get; set; }
    [Length(Max=1024)]
    public virtual string UnoAndroidRawData { get; set; }
    [Length(Max=1024)]
    public virtual string UnoIOsRawData { get; set; }
    public virtual System.Nullable<System.DateTime> UnoStartdatetime { get; set; }
    public virtual System.Nullable<System.DateTime> UnoLimitdatetime { get; set; }

    private IList<PushidNotification> _pushidNotifications = new List<PushidNotification>();

    public virtual IList<PushidNotification> PushidNotifications
    {
      get { return _pushidNotifications; }
      set { _pushidNotifications = value; }
    }

    public virtual User UnoUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UsersNotification).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UsersNotification'
         table='`USERS_NOTIFICATIONS`'
         >
    <id name='UnoId'
        column='`UNO_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UnoUsrId'
              column='`UNO_USR_ID`'
              />
    <property name='UnoStatus'
              column='`UNO_STATUS`'
              />
    <property name='UnoWpText1'
              column='`UNO_WP_TEXT1`'
              />
    <property name='UnoWpText2'
              column='`UNO_WP_TEXT2`'
              />
    <property name='UnoWpParam'
              column='`UNO_WP_PARAM`'
              />
    <property name='UnoWpCount'
              column='`UNO_WP_COUNT`'
              />
    <property name='UnoWpTileTitle'
              column='`UNO_WP_TILE_TITLE`'
              />
    <property name='UnoWpBackgroundImage'
              column='`UNO_WP_BACKGROUND_IMAGE`'
              />
    <property name='UnoWpRawData'
              column='`UNO_WP_RAW_DATA`'
              />
    <property name='UnoAndroidRawData'
              column='`UNO_ANDROID_RAW_DATA`'
              />
    <property name='UnoIOsRawData'
              column='`UNO_iOS_RAW_DATA`'
              />
    <property name='UnoStartdatetime'
              column='`UNO_STARTDATETIME`'
              />
    <property name='UnoLimitdatetime'
              column='`UNO_LIMITDATETIME`'
              />
    <bag name='PushidNotifications'
          inverse='true'
          >
      <key column='`PNO_UTNO_ID`' />
      <one-to-many class='PushidNotification' />
    </bag>
    <many-to-one name='UnoUsr' class='User' column='`UNO_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UsersPushId
  {
    public virtual decimal UpidId { get; set; }
    public virtual int UpidOs { get; set; }
    [NotNull]
    [Length(Max=512)]
    public virtual string UpidPushid { get; set; }
    public virtual decimal UpidUsrId { get; set; }
    public virtual System.DateTime UpidLastUpdateDatetime { get; set; }
    public virtual int UpidPushRetries { get; set; }
    public virtual System.Nullable<System.DateTime> UpidLastRetryDatetime { get; set; }
    public virtual System.Nullable<System.DateTime> UpidLastSucessfulPush { get; set; }
    [Length(Max=20)]
    public virtual string UpidAppVersion { get; set; }
    public virtual System.Nullable<int> UpidAppSessionKeepAlive { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellWifiMac { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellImei { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellModel { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellSerialnumber { get; set; }
    [Length(Max=100)]
    public virtual string UpidOsVersion { get; set; }

    private IList<MobileSession> _mobileSessions = new List<MobileSession>();

    public virtual IList<MobileSession> MobileSessions
    {
      get { return _mobileSessions; }
      set { _mobileSessions = value; }
    }

    public virtual User UpidUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UsersPushId).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UsersPushId'
         table='`USERS_PUSH_ID`'
         >
    <id name='UpidId'
        column='`UPID_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UpidOs'
              column='`UPID_OS`'
              />
    <property name='UpidPushid'
              column='`UPID_PUSHID`'
              />
    <property name='UpidUsrId'
              column='`UPID_USR_ID`'
              />
    <property name='UpidLastUpdateDatetime'
              column='`UPID_LAST_UPDATE_DATETIME`'
              />
    <property name='UpidPushRetries'
              column='`UPID_PUSH_RETRIES`'
              />
    <property name='UpidLastRetryDatetime'
              column='`UPID_LAST_RETRY_DATETIME`'
              />
    <property name='UpidLastSucessfulPush'
              column='`UPID_LAST_SUCESSFUL_PUSH`'
              />
    <property name='UpidAppVersion'
              column='`UPID_APP_VERSION`'
              />
    <property name='UpidAppSessionKeepAlive'
              column='`UPID_APP_SESSION_KEEP_ALIVE`'
              />
    <property name='UpidCellWifiMac'
              column='`UPID_CELL_WIFI_MAC`'
              />
    <property name='UpidCellImei'
              column='`UPID_CELL_IMEI`'
              />
    <property name='UpidCellModel'
              column='`UPID_CELL_MODEL`'
              />
    <property name='UpidCellSerialnumber'
              column='`UPID_CELL_SERIALNUMBER`'
              />
    <property name='UpidOsVersion'
              column='`UPID_OS_VERSION`'
              />
    <bag name='MobileSessions'
          inverse='false'
          >
      <key column='`MOSE_UPID_ID`' />
      <one-to-many class='MobileSession' />
    </bag>
    <many-to-one name='UpidUsr' class='User' column='`UPID_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UsersSmss
  {
    public virtual decimal UsrsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrsRecipientTelephone { get; set; }
    [NotNull]
    [Length(Max=512)]
    public virtual string UsrsSms { get; set; }
    public virtual int UsrsStatus { get; set; }
    public virtual System.Nullable<decimal> UsrsSenderId { get; set; }
    public virtual System.Nullable<decimal> UsrsUsrId { get; set; }
    public virtual System.Nullable<decimal> UsrsCusinsId { get; set; }
    public virtual System.Nullable<System.DateTime> UsrsDate { get; set; }

    public virtual CustomerInscription UsrsCusins { get; set; }

    public virtual User UsrsUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UsersSmss).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UsersSmss'
         table='`USERS_SMSS`'
         >
    <id name='UsrsId'
        column='`USRS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrsRecipientTelephone'
              column='`USRS_RECIPIENT_TELEPHONE`'
              />
    <property name='UsrsSms'
              column='`USRS_SMS`'
              />
    <property name='UsrsStatus'
              column='`USRS_STATUS`'
              />
    <property name='UsrsSenderId'
              column='`USRS_SENDER_ID`'
              />
    <property name='UsrsUsrId'
              column='`USRS_USR_ID`'
              />
    <property name='UsrsCusinsId'
              column='`USRS_CUSINS_ID`'
              />
    <property name='UsrsDate'
              column='`USRS_DATE`'
              />
    <many-to-one name='UsrsCusins' class='CustomerInscription' column='`USRS_CUSINS_ID`' />
    <many-to-one name='UsrsUsr' class='User' column='`USRS_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class AllOperationsExt
  {
    public virtual decimal id { get; set; }
    public virtual int OpeSuscriptionType { get; set; }
    public virtual int OpeBalanceBefore { get; set; }
    public virtual int OpeMoseOs { get; set; }
    public virtual decimal OpeAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeAmountCurIsoCode { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    public virtual decimal OpeId { get; set; }
    public virtual int OpeType { get; set; }
    public virtual System.Nullable<decimal> OpeUsrId { get; set; }
    [Length(Max=20)]
    public virtual string OpeAppVersion { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffNotifyEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcNotifyEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitDate { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffLogicalId { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffTariff { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffGate { get; set; }
    [Length(Max=10)]
    public virtual string OpeoffSpaceDescription { get; set; }
    [Length(Max=50)]
    public virtual string OpeAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId3 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs1 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs2 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeLongitude { get; set; }
    public virtual System.Nullable<int> OpeFinalAmount { get; set; }
    [NotNull]
    public virtual decimal OpeBalanceCurId { get; set; }
    [Length(Max=10)]
    [NotNull]
    public virtual string OpeBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpeChangeApplied { get; set; }
    public virtual System.Nullable<decimal> UsrpId { get; set; }
    [Length(Max=50)]
    public virtual string UsrpPlate { get; set; }
    [Length(Max=50)]
    public virtual string TipaTicketNumber { get; set; }
    [Length(Max=300)]
    public virtual string TipaTicketData { get; set; }
    public virtual System.Nullable<int> SechSechtId { get; set; }
    public virtual System.Nullable<decimal> GrpId { get; set; }
    public virtual System.Nullable<decimal> TarId { get; set; }
    [Length(Max=50)]
    public virtual string GrpDescription { get; set; }
    [Length(Max=50)]
    public virtual string TarDescription { get; set; }
    public virtual System.Nullable<decimal> OpeInsId { get; set; }
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual System.Nullable<System.DateTime> OpeDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInidate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeEnddate { get; set; }
    public virtual System.Nullable<int> OpeAmount { get; set; }
    public virtual System.Nullable<int> OpeTime { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeOpedisId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<int> CuspmrAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmrCurId { get; set; }
    [Length(Max=10)]
    public virtual string CuspmrAmountIsoCode { get; set; }
    public virtual System.Nullable<int> CuspmrBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisDate { get; set; }
    public virtual System.Nullable<int> OpedisAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisAmountCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpedisFinalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisBalanceCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpedisChangeApplied { get; set; }
    public virtual System.Nullable<int> OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrOpReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrTransactionId { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrAuthCode { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardHash { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardScheme { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrCardExpirationDate { get; set; }
    public virtual System.Nullable<int> OpeDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeInidateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeEnddateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeCuspmrDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeOpedisDateUtcOffset { get; set; }
    public virtual System.Nullable<decimal> OpePercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercVat2 { get; set; }
    public virtual System.Nullable<decimal> OpePartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercFee { get; set; }
    public virtual System.Nullable<decimal> OpePercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OpePartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpeFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpePartialFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpeTotalAmount { get; set; }
    public virtual decimal OpePercBonus { get; set; }
    public virtual System.Nullable<int> OpePartialBonusFee { get; set; }
    public virtual System.Nullable<int> OpeCuspmrType { get; set; }
    public virtual System.Nullable<int> OpeCuspmrPagateliaNewBalance { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(AllOperationsExt).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='AllOperationsExt'
         table='`ALL_OPERATIONS_EXT`'
         >
    <id name='id'
        column='`OPE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeSuscriptionType'
              column='`OPE_SUSCRIPTION_TYPE`'
              />
    <property name='OpeBalanceBefore'
              column='`OPE_BALANCE_BEFORE`'
              />
    <property name='OpeMoseOs'
              column='`OPE_MOSE_OS`'
              />
    <property name='OpeAmountCurId'
              column='`OPE_AMOUNT_CUR_ID`'
              />
    <property name='OpeAmountCurIsoCode'
              column='`OPE_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='OpeId'
              column='`OPE_ID`'
              />
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='OpeUsrId'
              column='`OPE_USR_ID`'
              />
    <property name='OpeAppVersion'
              column='`OPE_APP_VERSION`'
              />
    <property name='OpeoffEntryDate'
              column='`OPEOFF_ENTRY_DATE`'
              />
    <property name='OpeoffNotifyEntryDate'
              column='`OPEOFF_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffPaymentDate'
              column='`OPEOFF_PAYMENT_DATE`'
              />
    <property name='OpeoffEndDate'
              column='`OPEOFF_END_DATE`'
              />
    <property name='OpeoffExitLimitDate'
              column='`OPEOFF_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffExitDate'
              column='`OPEOFF_EXIT_DATE`'
              />
    <property name='OpeoffUtcEntryDate'
              column='`OPEOFF_UTC_ENTRY_DATE`'
              />
    <property name='OpeoffUtcNotifyEntryDate'
              column='`OPEOFF_UTC_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffUtcPaymentDate'
              column='`OPEOFF_UTC_PAYMENT_DATE`'
              />
    <property name='OpeoffUtcEndDate'
              column='`OPEOFF_UTC_END_DATE`'
              />
    <property name='OpeoffUtcExitLimitDate'
              column='`OPEOFF_UTC_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffUtcExitDate'
              column='`OPEOFF_UTC_EXIT_DATE`'
              />
    <property name='OpeoffLogicalId'
              column='`OPEOFF_LOGICAL_ID`'
              />
    <property name='OpeoffTariff'
              column='`OPEOFF_TARIFF`'
              />
    <property name='OpeoffGate'
              column='`OPEOFF_GATE`'
              />
    <property name='OpeoffSpaceDescription'
              column='`OPEOFF_SPACE_DESCRIPTION`'
              />
    <property name='OpeAmountCurName'
              column='`OPE_AMOUNT_CUR_NAME`'
              />
    <property name='OpeBalanceCurName'
              column='`OPE_BALANCE_CUR_NAME`'
              />
    <property name='CuspmrCurName'
              column='`CUSPMR_CUR_NAME`'
              />
    <property name='OpedisAmountCurName'
              column='`OPEDIS_AMOUNT_CUR_NAME`'
              />
    <property name='OpedisBalanceCurName'
              column='`OPEDIS_BALANCE_CUR_NAME`'
              />
    <property name='OpeExternalId1'
              column='`OPE_EXTERNAL_ID1`'
              />
    <property name='OpeExternalId2'
              column='`OPE_EXTERNAL_ID2`'
              />
    <property name='OpeExternalId3'
              column='`OPE_EXTERNAL_ID3`'
              />
    <property name='OpeConfirmedInWs1'
              column='`OPE_CONFIRMED_IN_WS1`'
              />
    <property name='OpeConfirmedInWs2'
              column='`OPE_CONFIRMED_IN_WS2`'
              />
    <property name='OpeConfirmedInWs3'
              column='`OPE_CONFIRMED_IN_WS3`'
              />
    <property name='OpeConfirmationTimeInWs1'
              column='`OPE_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs1'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeConfirmationTimeInWs2'
              column='`OPE_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs2'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeConfirmationTimeInWs3'
              column='`OPE_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs3'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeLatitude'
              column='`OPE_LATITUDE`'
              />
    <property name='OpeLongitude'
              column='`OPE_LONGITUDE`'
              />
    <property name='OpeFinalAmount'
              column='`OPE_FINAL_AMOUNT`'
              />
    <property name='OpeBalanceCurId'
              column='`OPE_BALANCE_CUR_ID`'
              />
    <property name='OpeBalanceCurIsoCode'
              column='`OPE_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpeChangeApplied'
              column='`OPE_CHANGE_APPLIED`'
              />
    <property name='UsrpId'
              column='`USRP_ID`'
              />
    <property name='UsrpPlate'
              column='`USRP_PLATE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='SechSechtId'
              column='`SECH_SECHT_ID`'
              />
    <property name='GrpId'
              column='`GRP_ID`'
              />
    <property name='TarId'
              column='`TAR_ID`'
              />
    <property name='GrpDescription'
              column='`GRP_DESCRIPTION`'
              />
    <property name='TarDescription'
              column='`TAR_DESCRIPTION`'
              />
    <property name='OpeInsId'
              column='`OPE_INS_ID`'
              />
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='OpeDate'
              column='`OPE_DATE`'
              />
    <property name='OpeUtcDate'
              column='`OPE_UTC_DATE`'
              />
    <property name='OpeInidate'
              column='`OPE_INIDATE`'
              />
    <property name='OpeEnddate'
              column='`OPE_ENDDATE`'
              />
    <property name='OpeAmount'
              column='`OPE_AMOUNT`'
              />
    <property name='OpeTime'
              column='`OPE_TIME`'
              />
    <property name='OpeInsertionUtcDate'
              column='`OPE_INSERTION_UTC_DATE`'
              />
    <property name='OpeCuspmrId'
              column='`OPE_CUSPMR_ID`'
              />
    <property name='OpeOpedisId'
              column='`OPE_OPEDIS_ID`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CuspmrAmountIsoCode'
              column='`CUSPMR_AMOUNT_ISO_CODE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisAmountCurIsoCode'
              column='`OPEDIS_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisBalanceCurIsoCode'
              column='`OPEDIS_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='OpeDateUtcOffset'
              column='`OPE_DATE_UTC_OFFSET`'
              />
    <property name='OpeInidateUtcOffset'
              column='`OPE_INIDATE_UTC_OFFSET`'
              />
    <property name='OpeEnddateUtcOffset'
              column='`OPE_ENDDATE_UTC_OFFSET`'
              />
    <property name='OpeCuspmrDateUtcOffset'
              column='`OPE_CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='OpeOpedisDateUtcOffset'
              column='`OPE_OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='OpePercVat1'
              column='`OPE_PERC_VAT1`'
              />
    <property name='OpePercVat2'
              column='`OPE_PERC_VAT2`'
              />
    <property name='OpePartialVat1'
              column='`OPE_PARTIAL_VAT1`'
              />
    <property name='OpePercFee'
              column='`OPE_PERC_FEE`'
              />
    <property name='OpePercFeeTopped'
              column='`OPE_PERC_FEE_TOPPED`'
              />
    <property name='OpePartialPercFee'
              column='`OPE_PARTIAL_PERC_FEE`'
              />
    <property name='OpeFixedFee'
              column='`OPE_FIXED_FEE`'
              />
    <property name='OpePartialFixedFee'
              column='`OPE_PARTIAL_FIXED_FEE`'
              />
    <property name='OpeTotalAmount'
              column='`OPE_TOTAL_AMOUNT`'
              />
    <property name='OpePercBonus'
              column='`OPE_PERC_BONUS`'
              />
    <property name='OpePartialBonusFee'
              column='`OPE_PARTIAL_BONUS_FEE`'
              />
    <property name='OpeCuspmrType'
              column='`OPE_CUSPMR_TYPE`'
              />
    <property name='OpeCuspmrPagateliaNewBalance'
              column='`OPE_CUSPMR_PAGATELIA_NEW_BALANCE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class Installation
  {
    public virtual decimal InsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual decimal InsOprId { get; set; }
    public virtual decimal InsCurId { get; set; }
    public virtual decimal InsLitId { get; set; }
    public virtual int InsVersion { get; set; }
    public virtual int InsTarVersion { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsTimezoneId { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string InsCultureLang { get; set; }
    public virtual int InsEnabled { get; set; }
    public virtual int InsFineWsSignatureType { get; set; }
    public virtual int InsParkWsSignatureType { get; set; }
    public virtual int InsUnparkWsSignatureType { get; set; }
    public virtual int InsParkConfirmWsSignatureType { get; set; }
    public virtual int InsOptPark { get; set; }
    public virtual int InsOptUnpark { get; set; }
    public virtual int InsOptTicket { get; set; }
    public virtual int InsOptRecharge { get; set; }
    public virtual int InsOptMoreFuncts { get; set; }
    public virtual int InsOptCurOpers { get; set; }
    public virtual int InsOptHisOpers { get; set; }
    public virtual int InsOptOcup { get; set; }
    public virtual int InsOptParkbyplate { get; set; }
    public virtual int InsOptParkbyplatelisttype { get; set; }
    public virtual int InsOptParkbyspace { get; set; }
    public virtual int InsOptParkbyzoneandsector { get; set; }
    public virtual int InsOptParkbyqr { get; set; }
    public virtual int InsOptParkbymap { get; set; }
    public virtual int InsOptParkbyspaceformat { get; set; }
    public virtual int InsOptParkbyspaceformatFormat { get; set; }
    public virtual int InsOptParkiszonemandatory { get; set; }
    public virtual int InsOptParkissectormandatory { get; set; }
    public virtual int InsOptParkzonecrit { get; set; }
    public virtual int InsOptParkpaybyqrformatmandatory { get; set; }
    public virtual int InsOptParkpaybyqrformat { get; set; }
    public virtual int InsOptParkmultitariffNum { get; set; }
    public virtual int InsOptTicketNum { get; set; }
    public virtual int InsOptTicketQr { get; set; }
    public virtual int InsOptRechargeQr { get; set; }
    public virtual int InsOptRechargeCode { get; set; }
    public virtual int InsOptRechargePaymentMean { get; set; }
    public virtual int InsPlateUpdateWsSignatureType { get; set; }
    public virtual int InsOptOffstreetParkEntry { get; set; }
    public virtual int InsOptOffstreetParkExit { get; set; }
    public virtual int InsOptOffstreetParkOcup { get; set; }
    [Length(Max=50)]
    public virtual string InsPhyZoneCodSystem { get; set; }
    [Length(Max=50)]
    public virtual string InsPhyZoneCodGeoZone { get; set; }
    [Length(Max=50)]
    public virtual string InsPhyZoneCodCity { get; set; }
    [Length(Max=500)]
    public virtual string InsPlateUpdateWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string InsPlateUpdateWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsStandardCityId { get; set; }
    [Length(Max=50)]
    public virtual string InsEysaContrataId { get; set; }
    [Length(Max=50)]
    public virtual string InsGtechnaCityCode { get; set; }
    [Length(Max=50)]
    public virtual string InsEysaUserName { get; set; }
    public virtual System.Nullable<decimal> InsEysaSerialNumber { get; set; }
    [Length(Max=50)]
    public virtual string InsEysaUserId { get; set; }
    [Length(Max=50)]
    public virtual string InsEysaPassword { get; set; }
    [Length(Max=50)]
    public virtual string InsEysaCompanyNameToSend { get; set; }
    public virtual System.Nullable<int> InsOptQrparkzoneformat { get; set; }
    public virtual System.Nullable<int> InsOptQrticketformat { get; set; }
    public virtual System.Nullable<int> InsOptQrrechargeformat { get; set; }
    public virtual System.Nullable<int> InsOptOperationconfirmMode { get; set; }
    public virtual System.Nullable<int> InsOptFineconfirmMode { get; set; }
    [Length(Max=500)]
    public virtual string InsParkConfirmWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWsHttpUser { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWsHttpPassword { get; set; }
    public virtual System.Nullable<int> InsParkConfirmWs2SignatureType { get; set; }
    [Length(Max=500)]
    public virtual string InsParkConfirmWs2Url { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWs2AuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWs2HttpUser { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWs2HttpPassword { get; set; }
    public virtual System.Nullable<int> InsParkConfirmWs3SignatureType { get; set; }
    [Length(Max=500)]
    public virtual string InsParkConfirmWs3Url { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWs3AuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWs3HttpUser { get; set; }
    [Length(Max=50)]
    public virtual string InsParkConfirmWs3HttpPassword { get; set; }
    [Length(Max=500)]
    public virtual string InsUnparkWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string InsUnparkWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsUnparkWsHttpUser { get; set; }
    [Length(Max=50)]
    public virtual string InsUnparkWsHttpPassword { get; set; }
    [Length(Max=500)]
    public virtual string InsParkWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string InsParkWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsParkWsHttpUser { get; set; }
    [Length(Max=50)]
    public virtual string InsParkWsHttpPassword { get; set; }
    [Length(Max=500)]
    public virtual string InsFineWsUrl { get; set; }
    [Length(Max=50)]
    public virtual string InsFineWsAuthHashKey { get; set; }
    [Length(Max=50)]
    public virtual string InsFineWsHttpUser { get; set; }
    [Length(Max=50)]
    public virtual string InsFineWsHttpPassword { get; set; }
    public virtual decimal InsPercVat1 { get; set; }
    public virtual decimal InsPercVat2 { get; set; }
    public virtual int InsFeeLayout { get; set; }
    public virtual System.Nullable<decimal> InsServiceParkLitId { get; set; }
    public virtual System.Nullable<decimal> InsServiceFeeLitId { get; set; }
    public virtual System.Nullable<decimal> InsServiceVatLitId { get; set; }
    public virtual System.Nullable<decimal> InsServiceTotalLitId { get; set; }
    public virtual System.Nullable<decimal> InsServiceSubtotalLitId { get; set; }
    public virtual System.Nullable<int> InsOptTransferBalance { get; set; }
    public virtual System.Nullable<int> InsOptModifyP { get; set; }
    public virtual System.Nullable<int> InsOptRechargePagatelia { get; set; }
    public virtual System.Nullable<int> InsOptRechargeSpotycoins { get; set; }
    public virtual int InsMaxUnpaidBalance { get; set; }
    public virtual int InsTrustAverageBalance { get; set; }
    public virtual int InsOptRechsponen { get; set; }
    public virtual int InsOptIsgpsforparkingmandatory { get; set; }
    public virtual int InsOptGpsparktimeout { get; set; }
    public virtual int InsOptToll { get; set; }
    public virtual int InsTollPayMode { get; set; }
    public virtual System.Nullable<decimal> InsTollPendingMsgLitId { get; set; }
    public virtual string InsOptConfigmenu { get; set; }

    private IList<ExternalParkingOperation> _externalParkingOperations = new List<ExternalParkingOperation>();

    public virtual IList<ExternalParkingOperation> ExternalParkingOperations
    {
      get { return _externalParkingOperations; }
      set { _externalParkingOperations = value; }
    }

    private IList<ExternalTicket> _externalTickets = new List<ExternalTicket>();

    public virtual IList<ExternalTicket> ExternalTickets
    {
      get { return _externalTickets; }
      set { _externalTickets = value; }
    }

    private IList<Group> _groups = new List<Group>();

    public virtual IList<Group> Groups
    {
      get { return _groups; }
      set { _groups = value; }
    }

    private IList<GroupsType> _groupsTypes = new List<GroupsType>();

    public virtual IList<GroupsType> GroupsTypes
    {
      get { return _groupsTypes; }
      set { _groupsTypes = value; }
    }

    private IList<InstallationsGeometry> _installationsGeometries = new List<InstallationsGeometry>();

    public virtual IList<InstallationsGeometry> InstallationsGeometries
    {
      get { return _installationsGeometries; }
      set { _installationsGeometries = value; }
    }

    private IList<MobileSession> _mobileSessions = new List<MobileSession>();

    public virtual IList<MobileSession> MobileSessions
    {
      get { return _mobileSessions; }
      set { _mobileSessions = value; }
    }

    private IList<OffstreetAutomaticOperation> _offstreetAutomaticOperations = new List<OffstreetAutomaticOperation>();

    public virtual IList<OffstreetAutomaticOperation> OffstreetAutomaticOperations
    {
      get { return _offstreetAutomaticOperations; }
      set { _offstreetAutomaticOperations = value; }
    }

    private IList<Operation> _operations = new List<Operation>();

    public virtual IList<Operation> Operations
    {
      get { return _operations; }
      set { _operations = value; }
    }

    private IList<OperationsOffstreet> _operationsOffstreets = new List<OperationsOffstreet>();

    public virtual IList<OperationsOffstreet> OperationsOffstreets
    {
      get { return _operationsOffstreets; }
      set { _operationsOffstreets = value; }
    }

    private IList<RechargeValue> _rechargeValues = new List<RechargeValue>();

    public virtual IList<RechargeValue> RechargeValues
    {
      get { return _rechargeValues; }
      set { _rechargeValues = value; }
    }

    private IList<Tariff> _tariffs = new List<Tariff>();

    public virtual IList<Tariff> Tariffs
    {
      get { return _tariffs; }
      set { _tariffs = value; }
    }

    private IList<TicketPayment> _ticketPayments = new List<TicketPayment>();

    public virtual IList<TicketPayment> TicketPayments
    {
      get { return _ticketPayments; }
      set { _ticketPayments = value; }
    }

    private IList<UserPlateMovsSending> _userPlateMovsSendings = new List<UserPlateMovsSending>();

    public virtual IList<UserPlateMovsSending> UserPlateMovsSendings
    {
      get { return _userPlateMovsSendings; }
      set { _userPlateMovsSendings = value; }
    }

    public virtual Currency InsCur { get; set; }

    public virtual Literal InsLit { get; set; }

    public virtual Literal InsServiceFeeLit { get; set; }

    public virtual Literal InsServiceParkLit { get; set; }

    public virtual Literal InsServiceSubtotalLit { get; set; }

    public virtual Literal InsServiceTotalLit { get; set; }

    public virtual Literal InsServiceVatLit { get; set; }

    public virtual Literal InsTollPendingMsgLit { get; set; }

    public virtual Operator InsOpr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(Installation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='Installation'
         table='`INSTALLATIONS`'
         >
    <id name='InsId'
        column='`INS_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='InsOprId'
              column='`INS_OPR_ID`'
              />
    <property name='InsCurId'
              column='`INS_CUR_ID`'
              />
    <property name='InsLitId'
              column='`INS_LIT_ID`'
              />
    <property name='InsVersion'
              column='`INS_VERSION`'
              />
    <property name='InsTarVersion'
              column='`INS_TAR_VERSION`'
              />
    <property name='InsTimezoneId'
              column='`INS_TIMEZONE_ID`'
              />
    <property name='InsCultureLang'
              column='`INS_CULTURE_LANG`'
              />
    <property name='InsEnabled'
              column='`INS_ENABLED`'
              />
    <property name='InsFineWsSignatureType'
              column='`INS_FINE_WS_SIGNATURE_TYPE`'
              />
    <property name='InsParkWsSignatureType'
              column='`INS_PARK_WS_SIGNATURE_TYPE`'
              />
    <property name='InsUnparkWsSignatureType'
              column='`INS_UNPARK_WS_SIGNATURE_TYPE`'
              />
    <property name='InsParkConfirmWsSignatureType'
              column='`INS_PARK_CONFIRM_WS_SIGNATURE_TYPE`'
              />
    <property name='InsOptPark'
              column='`INS_OPT_PARK`'
              />
    <property name='InsOptUnpark'
              column='`INS_OPT_UNPARK`'
              />
    <property name='InsOptTicket'
              column='`INS_OPT_TICKET`'
              />
    <property name='InsOptRecharge'
              column='`INS_OPT_RECHARGE`'
              />
    <property name='InsOptMoreFuncts'
              column='`INS_OPT_MORE_FUNCTS`'
              />
    <property name='InsOptCurOpers'
              column='`INS_OPT_CUR_OPERS`'
              />
    <property name='InsOptHisOpers'
              column='`INS_OPT_HIS_OPERS`'
              />
    <property name='InsOptOcup'
              column='`INS_OPT_OCUP`'
              />
    <property name='InsOptParkbyplate'
              column='`INS_OPT_PARKBYPLATE`'
              />
    <property name='InsOptParkbyplatelisttype'
              column='`INS_OPT_PARKBYPLATELISTTYPE`'
              />
    <property name='InsOptParkbyspace'
              column='`INS_OPT_PARKBYSPACE`'
              />
    <property name='InsOptParkbyzoneandsector'
              column='`INS_OPT_PARKBYZONEANDSECTOR`'
              />
    <property name='InsOptParkbyqr'
              column='`INS_OPT_PARKBYQR`'
              />
    <property name='InsOptParkbymap'
              column='`INS_OPT_PARKBYMAP`'
              />
    <property name='InsOptParkbyspaceformat'
              column='`INS_OPT_PARKBYSPACEFORMAT`'
              />
    <property name='InsOptParkbyspaceformatFormat'
              column='`INS_OPT_PARKBYSPACEFORMAT_FORMAT`'
              />
    <property name='InsOptParkiszonemandatory'
              column='`INS_OPT_PARKISZONEMANDATORY`'
              />
    <property name='InsOptParkissectormandatory'
              column='`INS_OPT_PARKISSECTORMANDATORY`'
              />
    <property name='InsOptParkzonecrit'
              column='`INS_OPT_PARKZONECRIT`'
              />
    <property name='InsOptParkpaybyqrformatmandatory'
              column='`INS_OPT_PARKPAYBYQRFORMATMANDATORY`'
              />
    <property name='InsOptParkpaybyqrformat'
              column='`INS_OPT_PARKPAYBYQRFORMAT`'
              />
    <property name='InsOptParkmultitariffNum'
              column='`INS_OPT_PARKMULTITARIFF_NUM`'
              />
    <property name='InsOptTicketNum'
              column='`INS_OPT_TICKET_NUM`'
              />
    <property name='InsOptTicketQr'
              column='`INS_OPT_TICKET_QR`'
              />
    <property name='InsOptRechargeQr'
              column='`INS_OPT_RECHARGE_QR`'
              />
    <property name='InsOptRechargeCode'
              column='`INS_OPT_RECHARGE_CODE`'
              />
    <property name='InsOptRechargePaymentMean'
              column='`INS_OPT_RECHARGE_PAYMENT_MEAN`'
              />
    <property name='InsPlateUpdateWsSignatureType'
              column='`INS_PLATE_UPDATE_WS_SIGNATURE_TYPE`'
              />
    <property name='InsOptOffstreetParkEntry'
              column='`INS_OPT_OFFSTREET_PARK_ENTRY`'
              />
    <property name='InsOptOffstreetParkExit'
              column='`INS_OPT_OFFSTREET_PARK_EXIT`'
              />
    <property name='InsOptOffstreetParkOcup'
              column='`INS_OPT_OFFSTREET_PARK_OCUP`'
              />
    <property name='InsPhyZoneCodSystem'
              column='`INS_PHY_ZONE_COD_SYSTEM`'
              />
    <property name='InsPhyZoneCodGeoZone'
              column='`INS_PHY_ZONE_COD_GEO_ZONE`'
              />
    <property name='InsPhyZoneCodCity'
              column='`INS_PHY_ZONE_COD_CITY`'
              />
    <property name='InsPlateUpdateWsUrl'
              column='`INS_PLATE_UPDATE_WS_URL`'
              />
    <property name='InsPlateUpdateWsAuthHashKey'
              column='`INS_PLATE_UPDATE_WS_AUTH_HASH_KEY`'
              />
    <property name='InsStandardCityId'
              column='`INS_STANDARD_CITY_ID`'
              />
    <property name='InsEysaContrataId'
              column='`INS_EYSA_CONTRATA_ID`'
              />
    <property name='InsGtechnaCityCode'
              column='`INS_GTECHNA_CITY_CODE`'
              />
    <property name='InsEysaUserName'
              column='`INS_EYSA_USER_NAME`'
              />
    <property name='InsEysaSerialNumber'
              column='`INS_EYSA_SERIAL_NUMBER`'
              />
    <property name='InsEysaUserId'
              column='`INS_EYSA_USER_ID`'
              />
    <property name='InsEysaPassword'
              column='`INS_EYSA_PASSWORD`'
              />
    <property name='InsEysaCompanyNameToSend'
              column='`INS_EYSA_COMPANY_NAME_TO_SEND`'
              />
    <property name='InsOptQrparkzoneformat'
              column='`INS_OPT_QRPARKZONEFORMAT`'
              />
    <property name='InsOptQrticketformat'
              column='`INS_OPT_QRTICKETFORMAT`'
              />
    <property name='InsOptQrrechargeformat'
              column='`INS_OPT_QRRECHARGEFORMAT`'
              />
    <property name='InsOptOperationconfirmMode'
              column='`INS_OPT_OPERATIONCONFIRM_MODE`'
              />
    <property name='InsOptFineconfirmMode'
              column='`INS_OPT_FINECONFIRM_MODE`'
              />
    <property name='InsParkConfirmWsUrl'
              column='`INS_PARK_CONFIRM_WS_URL`'
              />
    <property name='InsParkConfirmWsAuthHashKey'
              column='`INS_PARK_CONFIRM_WS_AUTH_HASH_KEY`'
              />
    <property name='InsParkConfirmWsHttpUser'
              column='`INS_PARK_CONFIRM_WS_HTTP_USER`'
              />
    <property name='InsParkConfirmWsHttpPassword'
              column='`INS_PARK_CONFIRM_WS_HTTP_PASSWORD`'
              />
    <property name='InsParkConfirmWs2SignatureType'
              column='`INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE`'
              />
    <property name='InsParkConfirmWs2Url'
              column='`INS_PARK_CONFIRM_WS2_URL`'
              />
    <property name='InsParkConfirmWs2AuthHashKey'
              column='`INS_PARK_CONFIRM_WS2_AUTH_HASH_KEY`'
              />
    <property name='InsParkConfirmWs2HttpUser'
              column='`INS_PARK_CONFIRM_WS2_HTTP_USER`'
              />
    <property name='InsParkConfirmWs2HttpPassword'
              column='`INS_PARK_CONFIRM_WS2_HTTP_PASSWORD`'
              />
    <property name='InsParkConfirmWs3SignatureType'
              column='`INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE`'
              />
    <property name='InsParkConfirmWs3Url'
              column='`INS_PARK_CONFIRM_WS3_URL`'
              />
    <property name='InsParkConfirmWs3AuthHashKey'
              column='`INS_PARK_CONFIRM_WS3_AUTH_HASH_KEY`'
              />
    <property name='InsParkConfirmWs3HttpUser'
              column='`INS_PARK_CONFIRM_WS3_HTTP_USER`'
              />
    <property name='InsParkConfirmWs3HttpPassword'
              column='`INS_PARK_CONFIRM_WS3_HTTP_PASSWORD`'
              />
    <property name='InsUnparkWsUrl'
              column='`INS_UNPARK_WS_URL`'
              />
    <property name='InsUnparkWsAuthHashKey'
              column='`INS_UNPARK_WS_AUTH_HASH_KEY`'
              />
    <property name='InsUnparkWsHttpUser'
              column='`INS_UNPARK_WS_HTTP_USER`'
              />
    <property name='InsUnparkWsHttpPassword'
              column='`INS_UNPARK_WS_HTTP_PASSWORD`'
              />
    <property name='InsParkWsUrl'
              column='`INS_PARK_WS_URL`'
              />
    <property name='InsParkWsAuthHashKey'
              column='`INS_PARK_WS_AUTH_HASH_KEY`'
              />
    <property name='InsParkWsHttpUser'
              column='`INS_PARK_WS_HTTP_USER`'
              />
    <property name='InsParkWsHttpPassword'
              column='`INS_PARK_WS_HTTP_PASSWORD`'
              />
    <property name='InsFineWsUrl'
              column='`INS_FINE_WS_URL`'
              />
    <property name='InsFineWsAuthHashKey'
              column='`INS_FINE_WS_AUTH_HASH_KEY`'
              />
    <property name='InsFineWsHttpUser'
              column='`INS_FINE_WS_HTTP_USER`'
              />
    <property name='InsFineWsHttpPassword'
              column='`INS_FINE_WS_HTTP_PASSWORD`'
              />
    <property name='InsPercVat1'
              column='`INS_PERC_VAT1`'
              />
    <property name='InsPercVat2'
              column='`INS_PERC_VAT2`'
              />
    <property name='InsFeeLayout'
              column='`INS_FEE_LAYOUT`'
              />
    <property name='InsServiceParkLitId'
              column='`INS_SERVICE_PARK_LIT_ID`'
              />
    <property name='InsServiceFeeLitId'
              column='`INS_SERVICE_FEE_LIT_ID`'
              />
    <property name='InsServiceVatLitId'
              column='`INS_SERVICE_VAT_LIT_ID`'
              />
    <property name='InsServiceTotalLitId'
              column='`INS_SERVICE_TOTAL_LIT_ID`'
              />
    <property name='InsServiceSubtotalLitId'
              column='`INS_SERVICE_SUBTOTAL_LIT_ID`'
              />
    <property name='InsOptTransferBalance'
              column='`INS_OPT_TRANSFER_BALANCE`'
              />
    <property name='InsOptModifyP'
              column='`INS_OPT_MODIFY_P`'
              />
    <property name='InsOptRechargePagatelia'
              column='`INS_OPT_RECHARGE_PAGATELIA`'
              />
    <property name='InsOptRechargeSpotycoins'
              column='`INS_OPT_RECHARGE_SPOTYCOINS`'
              />
    <property name='InsMaxUnpaidBalance'
              column='`INS_MAX_UNPAID_BALANCE`'
              />
    <property name='InsTrustAverageBalance'
              column='`INS_TRUST_AVERAGE_BALANCE`'
              />
    <property name='InsOptRechsponen'
              column='`INS_OPT_RECHSPONEN`'
              />
    <property name='InsOptIsgpsforparkingmandatory'
              column='`INS_OPT_ISGPSFORPARKINGMANDATORY`'
              />
    <property name='InsOptGpsparktimeout'
              column='`INS_OPT_GPSPARKTIMEOUT`'
              />
    <property name='InsOptToll'
              column='`INS_OPT_TOLL`'
              />
    <property name='InsTollPayMode'
              column='`INS_TOLL_PAY_MODE`'
              />
    <property name='InsTollPendingMsgLitId'
              column='`INS_TOLL_PENDING_MSG_LIT_ID`'
              />
    <property name='InsOptConfigmenu'
              column='`INS_OPT_CONFIGMENU`'
              />
    <bag name='ExternalParkingOperations'
          inverse='true'
          >
      <key column='`EPO_INS_ID`' />
      <one-to-many class='ExternalParkingOperation' />
    </bag>
    <bag name='ExternalTickets'
          inverse='true'
          >
      <key column='`EXTI_INS_ID`' />
      <one-to-many class='ExternalTicket' />
    </bag>
    <bag name='Groups'
          inverse='true'
          >
      <key column='`GRP_INS_ID`' />
      <one-to-many class='Group' />
    </bag>
    <bag name='GroupsTypes'
          inverse='true'
          >
      <key column='`GRPT_INS_ID`' />
      <one-to-many class='GroupsType' />
    </bag>
    <bag name='InstallationsGeometries'
          inverse='true'
          >
      <key column='`INSGE_INS_ID`' />
      <one-to-many class='InstallationsGeometry' />
    </bag>
    <bag name='MobileSessions'
          inverse='true'
          >
      <key column='`MOSE_INS_ID`' />
      <one-to-many class='MobileSession' />
    </bag>
    <bag name='OffstreetAutomaticOperations'
          inverse='true'
          >
      <key column='`OAUOP_INS_ID`' />
      <one-to-many class='OffstreetAutomaticOperation' />
    </bag>
    <bag name='Operations'
          inverse='true'
          >
      <key column='`OPE_INS_ID`' />
      <one-to-many class='Operation' />
    </bag>
    <bag name='OperationsOffstreets'
          inverse='true'
          >
      <key column='`OPEOFF_INS_ID`' />
      <one-to-many class='OperationsOffstreet' />
    </bag>
    <bag name='RechargeValues'
          inverse='true'
          >
      <key column='`RECHVAL_INS_ID`' />
      <one-to-many class='RechargeValue' />
    </bag>
    <bag name='Tariffs'
          inverse='true'
          >
      <key column='`TAR_INS_ID`' />
      <one-to-many class='Tariff' />
    </bag>
    <bag name='TicketPayments'
          inverse='true'
          >
      <key column='`TIPA_INS_ID`' />
      <one-to-many class='TicketPayment' />
    </bag>
    <bag name='UserPlateMovsSendings'
          inverse='true'
          >
      <key column='`USRPMS_INS_ID`' />
      <one-to-many class='UserPlateMovsSending' />
    </bag>
    <many-to-one name='InsCur' class='Currency' column='`INS_CUR_ID`' />
    <many-to-one name='InsLit' class='Literal' column='`INS_LIT_ID`' />
    <many-to-one name='InsServiceFeeLit' class='Literal' column='`INS_SERVICE_FEE_LIT_ID`' />
    <many-to-one name='InsServiceParkLit' class='Literal' column='`INS_SERVICE_PARK_LIT_ID`' />
    <many-to-one name='InsServiceSubtotalLit' class='Literal' column='`INS_SERVICE_SUBTOTAL_LIT_ID`' />
    <many-to-one name='InsServiceTotalLit' class='Literal' column='`INS_SERVICE_TOTAL_LIT_ID`' />
    <many-to-one name='InsServiceVatLit' class='Literal' column='`INS_SERVICE_VAT_LIT_ID`' />
    <many-to-one name='InsTollPendingMsgLit' class='Literal' column='`INS_TOLL_PENDING_MSG_LIT_ID`' />
    <many-to-one name='InsOpr' class='Operator' column='`INS_OPR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwUser
  {
    public virtual decimal id { get; set; }
    public virtual decimal UsrId { get; set; }
    public virtual decimal UsrCusId { get; set; }
    public virtual System.DateTime UsrInsertUtcDate { get; set; }
    public virtual decimal UsrCouId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrEmail { get; set; }
    public virtual decimal UsrMainTelCountry { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrMainTel { get; set; }
    public virtual int UsrBalance { get; set; }
    public virtual decimal UsrCurId { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string UsrCultureLang { get; set; }
    public virtual int UsrUtcOffset { get; set; }
    public virtual int UsrEnabled { get; set; }
    public virtual decimal CusId { get; set; }
    public virtual decimal CusType { get; set; }
    public virtual System.DateTime CusInsertUtcDate { get; set; }
    public virtual decimal CusCouId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusDocId { get; set; }
    public virtual decimal CusDocIdType { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusStreet { get; set; }
    public virtual int CusStreeNumber { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusCity { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CusState { get; set; }
    [NotNull]
    [Length(Max=20)]
    public virtual string CusZipcode { get; set; }
    public virtual int CusEnabled { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    public virtual System.Nullable<int> UsrSuscriptionType { get; set; }
    public virtual System.Nullable<decimal> UsrCuspmId { get; set; }
    [Length(Max=500)]
    public virtual string Plates { get; set; }
    public virtual System.Nullable<int> CusLevelNum { get; set; }
    [Length(Max=10)]
    public virtual string CusDoor { get; set; }
    [Length(Max=10)]
    public virtual string CusLetter { get; set; }
    [Length(Max=10)]
    public virtual string CusStair { get; set; }
    [Length(Max=50)]
    public virtual string CusSurname1 { get; set; }
    [Length(Max=50)]
    public virtual string CusSurname2 { get; set; }
    public virtual System.Nullable<decimal> CusUsrId { get; set; }
    public virtual System.Nullable<decimal> UsrSecundTelCountry { get; set; }
    [Length(Max=50)]
    public virtual string UsrSecundTel { get; set; }
    public virtual System.Nullable<decimal> UsrLctId { get; set; }
    public virtual System.Nullable<int> UsrInsertMoseOs { get; set; }
    public virtual System.Nullable<System.DateTime> UsrDisableUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> UsrOperativeUtcDate { get; set; }
    [Length(Max=256)]
    public virtual string UsrPagateliaLastUser { get; set; }
    [Length(Max=128)]
    public virtual string UsrPagateliaLastPwd { get; set; }
    public virtual int UsrPaymeth { get; set; }
    public virtual int PaymentMethod { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwUser).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwUser'
         table='`VW_USERS`'
         >
    <id name='id'
        column='`USR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrId'
              column='`USR_ID`'
              />
    <property name='UsrCusId'
              column='`USR_CUS_ID`'
              />
    <property name='UsrInsertUtcDate'
              column='`USR_INSERT_UTC_DATE`'
              />
    <property name='UsrCouId'
              column='`USR_COU_ID`'
              />
    <property name='UsrEmail'
              column='`USR_EMAIL`'
              />
    <property name='UsrMainTelCountry'
              column='`USR_MAIN_TEL_COUNTRY`'
              />
    <property name='UsrMainTel'
              column='`USR_MAIN_TEL`'
              />
    <property name='UsrBalance'
              column='`USR_BALANCE`'
              />
    <property name='UsrCurId'
              column='`USR_CUR_ID`'
              />
    <property name='UsrCultureLang'
              column='`USR_CULTURE_LANG`'
              />
    <property name='UsrUtcOffset'
              column='`USR_UTC_OFFSET`'
              />
    <property name='UsrEnabled'
              column='`USR_ENABLED`'
              />
    <property name='CusId'
              column='`CUS_ID`'
              />
    <property name='CusType'
              column='`CUS_TYPE`'
              />
    <property name='CusInsertUtcDate'
              column='`CUS_INSERT_UTC_DATE`'
              />
    <property name='CusCouId'
              column='`CUS_COU_ID`'
              />
    <property name='CusDocId'
              column='`CUS_DOC_ID`'
              />
    <property name='CusDocIdType'
              column='`CUS_DOC_ID_TYPE`'
              />
    <property name='CusName'
              column='`CUS_NAME`'
              />
    <property name='CusStreet'
              column='`CUS_STREET`'
              />
    <property name='CusStreeNumber'
              column='`CUS_STREE_NUMBER`'
              />
    <property name='CusCity'
              column='`CUS_CITY`'
              />
    <property name='CusState'
              column='`CUS_STATE`'
              />
    <property name='CusZipcode'
              column='`CUS_ZIPCODE`'
              />
    <property name='CusEnabled'
              column='`CUS_ENABLED`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='UsrSuscriptionType'
              column='`USR_SUSCRIPTION_TYPE`'
              />
    <property name='UsrCuspmId'
              column='`USR_CUSPM_ID`'
              />
    <property name='Plates'
              column='`Plates`'
              />
    <property name='CusLevelNum'
              column='`CUS_LEVEL_NUM`'
              />
    <property name='CusDoor'
              column='`CUS_DOOR`'
              />
    <property name='CusLetter'
              column='`CUS_LETTER`'
              />
    <property name='CusStair'
              column='`CUS_STAIR`'
              />
    <property name='CusSurname1'
              column='`CUS_SURNAME1`'
              />
    <property name='CusSurname2'
              column='`CUS_SURNAME2`'
              />
    <property name='CusUsrId'
              column='`CUS_USR_ID`'
              />
    <property name='UsrSecundTelCountry'
              column='`USR_SECUND_TEL_COUNTRY`'
              />
    <property name='UsrSecundTel'
              column='`USR_SECUND_TEL`'
              />
    <property name='UsrLctId'
              column='`USR_LCT_ID`'
              />
    <property name='UsrInsertMoseOs'
              column='`USR_INSERT_MOSE_OS`'
              />
    <property name='UsrDisableUtcDate'
              column='`USR_DISABLE_UTC_DATE`'
              />
    <property name='UsrOperativeUtcDate'
              column='`USR_OPERATIVE_UTC_DATE`'
              />
    <property name='UsrPagateliaLastUser'
              column='`USR_PAGATELIA_LAST_USER`'
              />
    <property name='UsrPagateliaLastPwd'
              column='`USR_PAGATELIA_LAST_PWD`'
              />
    <property name='UsrPaymeth'
              column='`USR_PAYMETH`'
              />
    <property name='PaymentMethod'
              column='`PaymentMethod`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class LicenseTerm
  {
    public virtual decimal LctId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string LctVersion { get; set; }

    private IList<LicenseTermsParam> _licenseTermsParams = new List<LicenseTermsParam>();

    public virtual IList<LicenseTermsParam> LicenseTermsParams
    {
      get { return _licenseTermsParams; }
      set { _licenseTermsParams = value; }
    }

    private IList<User> _users = new List<User>();

    public virtual IList<User> Users
    {
      get { return _users; }
      set { _users = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(LicenseTerm).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='LicenseTerm'
         table='`LICENSE_TERMS`'
         >
    <id name='LctId'
        column='`LCT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='LctVersion'
              column='`LCT_VERSION`'
              />
    <bag name='LicenseTermsParams'
          inverse='true'
          >
      <key column='`LTP_LCT_ID`' />
      <one-to-many class='LicenseTermsParam' />
    </bag>
    <bag name='Users'
          inverse='false'
          >
      <key column='`USR_LCT_ID`' />
      <one-to-many class='User' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class LicenseTermsParam
  {
    public virtual decimal LtpId { get; set; }
    public virtual decimal LtpLctId { get; set; }
    public virtual decimal LtpLanId { get; set; }
    [NotNull]
    [Length(Max=500)]
    public virtual string LtpUrl1 { get; set; }
    [NotNull]
    [Length(Max=500)]
    public virtual string LtpUrl2 { get; set; }

    public virtual Language LtpLan { get; set; }

    public virtual LicenseTerm LtpLct { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(LicenseTermsParam).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='LicenseTermsParam'
         table='`LICENSE_TERMS_PARAMS`'
         >
    <id name='LtpId'
        column='`LTP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='LtpLctId'
              column='`LTP_LCT_ID`'
              />
    <property name='LtpLanId'
              column='`LTP_LAN_ID`'
              />
    <property name='LtpUrl1'
              column='`LTP_URL1`'
              />
    <property name='LtpUrl2'
              column='`LTP_URL2`'
              />
    <many-to-one name='LtpLan' class='Language' column='`LTP_LAN_ID`' />
    <many-to-one name='LtpLct' class='LicenseTerm' column='`LTP_LCT_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class AllOperationsExtNorecharge
  {
    public virtual decimal id { get; set; }
    public virtual int OpeSuscriptionType { get; set; }
    public virtual int OpeBalanceBefore { get; set; }
    public virtual int OpeType { get; set; }
    public virtual decimal OpeUsrId { get; set; }
    public virtual int OpeMoseOs { get; set; }
    public virtual decimal OpeBalanceCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeBalanceCurIsoCode { get; set; }
    public virtual decimal OpeChangeApplied { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OpeAmountCurName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OpeBalanceCurName { get; set; }
    public virtual decimal OpeId { get; set; }
    public virtual decimal OpeAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpeFinalAmount { get; set; }
    [Length(Max=20)]
    public virtual string OpeAppVersion { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffNotifyEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcNotifyEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitDate { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffLogicalId { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffTariff { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffGate { get; set; }
    [Length(Max=10)]
    public virtual string OpeoffSpaceDescription { get; set; }
    public virtual System.Nullable<decimal> OpePercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercVat2 { get; set; }
    public virtual System.Nullable<decimal> OpePartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercFee { get; set; }
    public virtual System.Nullable<decimal> OpePercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OpePartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpeFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpePartialFixedFee { get; set; }
    public virtual System.Nullable<int> OpeTotalAmount { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId3 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs1 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs2 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeLongitude { get; set; }
    public virtual System.Nullable<decimal> UsrpId { get; set; }
    [Length(Max=50)]
    public virtual string UsrpPlate { get; set; }
    [Length(Max=50)]
    public virtual string TipaTicketNumber { get; set; }
    [Length(Max=300)]
    public virtual string TipaTicketData { get; set; }
    public virtual System.Nullable<int> SechSechtId { get; set; }
    public virtual System.Nullable<decimal> GrpId { get; set; }
    public virtual System.Nullable<decimal> TarId { get; set; }
    [Length(Max=50)]
    public virtual string GrpDescription { get; set; }
    [Length(Max=50)]
    public virtual string TarDescription { get; set; }
    public virtual System.Nullable<decimal> OpeInsId { get; set; }
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual System.Nullable<System.DateTime> OpeDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInidate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeEnddate { get; set; }
    public virtual System.Nullable<int> OpeAmount { get; set; }
    public virtual System.Nullable<int> OpeTime { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeOpedisId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<int> CuspmrAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmrCurId { get; set; }
    [Length(Max=10)]
    public virtual string CuspmrAmountIsoCode { get; set; }
    public virtual System.Nullable<int> CuspmrBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisDate { get; set; }
    public virtual System.Nullable<int> OpedisAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisAmountCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpedisFinalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisBalanceCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpedisChangeApplied { get; set; }
    public virtual System.Nullable<int> OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    public virtual System.Nullable<int> CuspmrOpReference { get; set; }
    public virtual System.Nullable<int> CuspmrTransactionId { get; set; }
    public virtual System.Nullable<int> CuspmrAuthCode { get; set; }
    public virtual System.Nullable<int> CuspmrCardHash { get; set; }
    public virtual System.Nullable<int> CuspmrCardReference { get; set; }
    public virtual System.Nullable<int> CuspmrCardScheme { get; set; }
    public virtual System.Nullable<int> CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<int> CuspmrCardExpirationDate { get; set; }
    public virtual System.Nullable<int> OpeDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeInidateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeEnddateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeCuspmrDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeOpedisDateUtcOffset { get; set; }
    public virtual decimal OpePercBonus { get; set; }
    public virtual System.Nullable<int> OpePartialBonusFee { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(AllOperationsExtNorecharge).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='AllOperationsExtNorecharge'
         table='`ALL_OPERATIONS_EXT_NORECHARGES`'
         >
    <id name='id'
        column='`OPE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeSuscriptionType'
              column='`OPE_SUSCRIPTION_TYPE`'
              />
    <property name='OpeBalanceBefore'
              column='`OPE_BALANCE_BEFORE`'
              />
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='OpeUsrId'
              column='`OPE_USR_ID`'
              />
    <property name='OpeMoseOs'
              column='`OPE_MOSE_OS`'
              />
    <property name='OpeBalanceCurId'
              column='`OPE_BALANCE_CUR_ID`'
              />
    <property name='OpeBalanceCurIsoCode'
              column='`OPE_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpeChangeApplied'
              column='`OPE_CHANGE_APPLIED`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='OpeAmountCurName'
              column='`OPE_AMOUNT_CUR_NAME`'
              />
    <property name='OpeBalanceCurName'
              column='`OPE_BALANCE_CUR_NAME`'
              />
    <property name='OpeId'
              column='`OPE_ID`'
              />
    <property name='OpeAmountCurId'
              column='`OPE_AMOUNT_CUR_ID`'
              />
    <property name='OpeAmountCurIsoCode'
              column='`OPE_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpeFinalAmount'
              column='`OPE_FINAL_AMOUNT`'
              />
    <property name='OpeAppVersion'
              column='`OPE_APP_VERSION`'
              />
    <property name='OpeoffEntryDate'
              column='`OPEOFF_ENTRY_DATE`'
              />
    <property name='OpeoffNotifyEntryDate'
              column='`OPEOFF_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffPaymentDate'
              column='`OPEOFF_PAYMENT_DATE`'
              />
    <property name='OpeoffEndDate'
              column='`OPEOFF_END_DATE`'
              />
    <property name='OpeoffExitLimitDate'
              column='`OPEOFF_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffExitDate'
              column='`OPEOFF_EXIT_DATE`'
              />
    <property name='OpeoffUtcEntryDate'
              column='`OPEOFF_UTC_ENTRY_DATE`'
              />
    <property name='OpeoffUtcNotifyEntryDate'
              column='`OPEOFF_UTC_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffUtcPaymentDate'
              column='`OPEOFF_UTC_PAYMENT_DATE`'
              />
    <property name='OpeoffUtcEndDate'
              column='`OPEOFF_UTC_END_DATE`'
              />
    <property name='OpeoffUtcExitLimitDate'
              column='`OPEOFF_UTC_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffUtcExitDate'
              column='`OPEOFF_UTC_EXIT_DATE`'
              />
    <property name='OpeoffLogicalId'
              column='`OPEOFF_LOGICAL_ID`'
              />
    <property name='OpeoffTariff'
              column='`OPEOFF_TARIFF`'
              />
    <property name='OpeoffGate'
              column='`OPEOFF_GATE`'
              />
    <property name='OpeoffSpaceDescription'
              column='`OPEOFF_SPACE_DESCRIPTION`'
              />
    <property name='OpePercVat1'
              column='`OPE_PERC_VAT1`'
              />
    <property name='OpePercVat2'
              column='`OPE_PERC_VAT2`'
              />
    <property name='OpePartialVat1'
              column='`OPE_PARTIAL_VAT1`'
              />
    <property name='OpePercFee'
              column='`OPE_PERC_FEE`'
              />
    <property name='OpePercFeeTopped'
              column='`OPE_PERC_FEE_TOPPED`'
              />
    <property name='OpePartialPercFee'
              column='`OPE_PARTIAL_PERC_FEE`'
              />
    <property name='OpeFixedFee'
              column='`OPE_FIXED_FEE`'
              />
    <property name='OpePartialFixedFee'
              column='`OPE_PARTIAL_FIXED_FEE`'
              />
    <property name='OpeTotalAmount'
              column='`OPE_TOTAL_AMOUNT`'
              />
    <property name='CuspmrCurName'
              column='`CUSPMR_CUR_NAME`'
              />
    <property name='OpedisAmountCurName'
              column='`OPEDIS_AMOUNT_CUR_NAME`'
              />
    <property name='OpedisBalanceCurName'
              column='`OPEDIS_BALANCE_CUR_NAME`'
              />
    <property name='OpeExternalId1'
              column='`OPE_EXTERNAL_ID1`'
              />
    <property name='OpeExternalId2'
              column='`OPE_EXTERNAL_ID2`'
              />
    <property name='OpeExternalId3'
              column='`OPE_EXTERNAL_ID3`'
              />
    <property name='OpeConfirmedInWs1'
              column='`OPE_CONFIRMED_IN_WS1`'
              />
    <property name='OpeConfirmedInWs2'
              column='`OPE_CONFIRMED_IN_WS2`'
              />
    <property name='OpeConfirmedInWs3'
              column='`OPE_CONFIRMED_IN_WS3`'
              />
    <property name='OpeConfirmationTimeInWs1'
              column='`OPE_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs1'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeConfirmationTimeInWs2'
              column='`OPE_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs2'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeConfirmationTimeInWs3'
              column='`OPE_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs3'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeLatitude'
              column='`OPE_LATITUDE`'
              />
    <property name='OpeLongitude'
              column='`OPE_LONGITUDE`'
              />
    <property name='UsrpId'
              column='`USRP_ID`'
              />
    <property name='UsrpPlate'
              column='`USRP_PLATE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='SechSechtId'
              column='`SECH_SECHT_ID`'
              />
    <property name='GrpId'
              column='`GRP_ID`'
              />
    <property name='TarId'
              column='`TAR_ID`'
              />
    <property name='GrpDescription'
              column='`GRP_DESCRIPTION`'
              />
    <property name='TarDescription'
              column='`TAR_DESCRIPTION`'
              />
    <property name='OpeInsId'
              column='`OPE_INS_ID`'
              />
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='OpeDate'
              column='`OPE_DATE`'
              />
    <property name='OpeUtcDate'
              column='`OPE_UTC_DATE`'
              />
    <property name='OpeInidate'
              column='`OPE_INIDATE`'
              />
    <property name='OpeEnddate'
              column='`OPE_ENDDATE`'
              />
    <property name='OpeAmount'
              column='`OPE_AMOUNT`'
              />
    <property name='OpeTime'
              column='`OPE_TIME`'
              />
    <property name='OpeInsertionUtcDate'
              column='`OPE_INSERTION_UTC_DATE`'
              />
    <property name='OpeCuspmrId'
              column='`OPE_CUSPMR_ID`'
              />
    <property name='OpeOpedisId'
              column='`OPE_OPEDIS_ID`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CuspmrAmountIsoCode'
              column='`CUSPMR_AMOUNT_ISO_CODE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisAmountCurIsoCode'
              column='`OPEDIS_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisBalanceCurIsoCode'
              column='`OPEDIS_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='OpeDateUtcOffset'
              column='`OPE_DATE_UTC_OFFSET`'
              />
    <property name='OpeInidateUtcOffset'
              column='`OPE_INIDATE_UTC_OFFSET`'
              />
    <property name='OpeEnddateUtcOffset'
              column='`OPE_ENDDATE_UTC_OFFSET`'
              />
    <property name='OpeCuspmrDateUtcOffset'
              column='`OPE_CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='OpeOpedisDateUtcOffset'
              column='`OPE_OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='OpePercBonus'
              column='`OPE_PERC_BONUS`'
              />
    <property name='OpePartialBonusFee'
              column='`OPE_PARTIAL_BONUS_FEE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class AllCurrOperationsExt
  {
    public virtual decimal id { get; set; }
    public virtual int OpeSuscriptionType { get; set; }
    public virtual int OpeBalanceBefore { get; set; }
    public virtual int OpeMoseOs { get; set; }
    public virtual decimal OpeAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeAmountCurIsoCode { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    public virtual decimal OpeId { get; set; }
    public virtual int OpeType { get; set; }
    public virtual System.Nullable<decimal> OpeUsrId { get; set; }
    [Length(Max=20)]
    public virtual string OpeAppVersion { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffNotifyEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffExitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcNotifyEntryDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcPaymentDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcEndDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitLimitDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeoffUtcExitDate { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffLogicalId { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffTariff { get; set; }
    [Length(Max=100)]
    public virtual string OpeoffGate { get; set; }
    [Length(Max=10)]
    public virtual string OpeoffSpaceDescription { get; set; }
    public virtual System.Nullable<decimal> OpePercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercVat2 { get; set; }
    public virtual System.Nullable<decimal> OpePartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercFee { get; set; }
    public virtual System.Nullable<decimal> OpePercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OpePartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpeFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpePartialFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpeTotalAmount { get; set; }
    [Length(Max=50)]
    public virtual string OpeAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId3 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs1 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs2 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeLongitude { get; set; }
    public virtual System.Nullable<int> OpeFinalAmount { get; set; }
    [NotNull]
    public virtual decimal OpeBalanceCurId { get; set; }
    [Length(Max=10)]
    [NotNull]
    public virtual string OpeBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpeChangeApplied { get; set; }
    public virtual System.Nullable<decimal> UsrpId { get; set; }
    [Length(Max=50)]
    public virtual string UsrpPlate { get; set; }
    [Length(Max=50)]
    public virtual string TipaTicketNumber { get; set; }
    [Length(Max=300)]
    public virtual string TipaTicketData { get; set; }
    public virtual System.Nullable<int> SechSechtId { get; set; }
    public virtual System.Nullable<decimal> GrpId { get; set; }
    public virtual System.Nullable<decimal> TarId { get; set; }
    [Length(Max=50)]
    public virtual string GrpDescription { get; set; }
    [Length(Max=50)]
    public virtual string TarDescription { get; set; }
    public virtual System.Nullable<decimal> OpeInsId { get; set; }
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual System.Nullable<System.DateTime> OpeDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInidate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeEnddate { get; set; }
    public virtual System.Nullable<int> OpeAmount { get; set; }
    public virtual System.Nullable<int> OpeTime { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeOpedisId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<int> CuspmrAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmrCurId { get; set; }
    [Length(Max=10)]
    public virtual string CuspmrAmountIsoCode { get; set; }
    public virtual System.Nullable<int> CuspmrBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisDate { get; set; }
    public virtual System.Nullable<int> OpedisAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisAmountCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpedisFinalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisBalanceCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpedisChangeApplied { get; set; }
    public virtual System.Nullable<int> OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrOpReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrTransactionId { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrAuthCode { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardHash { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardScheme { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrCardExpirationDate { get; set; }
    public virtual System.Nullable<int> OpeDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeInidateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeEnddateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeCuspmrDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeOpedisDateUtcOffset { get; set; }
    public virtual decimal OpePercBonus { get; set; }
    public virtual System.Nullable<int> OpePartialBonusFee { get; set; }
    public virtual System.Nullable<int> OpeCuspmrType { get; set; }
    public virtual System.Nullable<int> OpeCuspmrPagateliaNewBalance { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(AllCurrOperationsExt).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='AllCurrOperationsExt'
         table='`ALL_CURR_OPERATIONS_EXT`'
         >
    <id name='id'
        column='`OPE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeSuscriptionType'
              column='`OPE_SUSCRIPTION_TYPE`'
              />
    <property name='OpeBalanceBefore'
              column='`OPE_BALANCE_BEFORE`'
              />
    <property name='OpeMoseOs'
              column='`OPE_MOSE_OS`'
              />
    <property name='OpeAmountCurId'
              column='`OPE_AMOUNT_CUR_ID`'
              />
    <property name='OpeAmountCurIsoCode'
              column='`OPE_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='OpeId'
              column='`OPE_ID`'
              />
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='OpeUsrId'
              column='`OPE_USR_ID`'
              />
    <property name='OpeAppVersion'
              column='`OPE_APP_VERSION`'
              />
    <property name='OpeoffEntryDate'
              column='`OPEOFF_ENTRY_DATE`'
              />
    <property name='OpeoffNotifyEntryDate'
              column='`OPEOFF_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffPaymentDate'
              column='`OPEOFF_PAYMENT_DATE`'
              />
    <property name='OpeoffEndDate'
              column='`OPEOFF_END_DATE`'
              />
    <property name='OpeoffExitLimitDate'
              column='`OPEOFF_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffExitDate'
              column='`OPEOFF_EXIT_DATE`'
              />
    <property name='OpeoffUtcEntryDate'
              column='`OPEOFF_UTC_ENTRY_DATE`'
              />
    <property name='OpeoffUtcNotifyEntryDate'
              column='`OPEOFF_UTC_NOTIFY_ENTRY_DATE`'
              />
    <property name='OpeoffUtcPaymentDate'
              column='`OPEOFF_UTC_PAYMENT_DATE`'
              />
    <property name='OpeoffUtcEndDate'
              column='`OPEOFF_UTC_END_DATE`'
              />
    <property name='OpeoffUtcExitLimitDate'
              column='`OPEOFF_UTC_EXIT_LIMIT_DATE`'
              />
    <property name='OpeoffUtcExitDate'
              column='`OPEOFF_UTC_EXIT_DATE`'
              />
    <property name='OpeoffLogicalId'
              column='`OPEOFF_LOGICAL_ID`'
              />
    <property name='OpeoffTariff'
              column='`OPEOFF_TARIFF`'
              />
    <property name='OpeoffGate'
              column='`OPEOFF_GATE`'
              />
    <property name='OpeoffSpaceDescription'
              column='`OPEOFF_SPACE_DESCRIPTION`'
              />
    <property name='OpePercVat1'
              column='`OPE_PERC_VAT1`'
              />
    <property name='OpePercVat2'
              column='`OPE_PERC_VAT2`'
              />
    <property name='OpePartialVat1'
              column='`OPE_PARTIAL_VAT1`'
              />
    <property name='OpePercFee'
              column='`OPE_PERC_FEE`'
              />
    <property name='OpePercFeeTopped'
              column='`OPE_PERC_FEE_TOPPED`'
              />
    <property name='OpePartialPercFee'
              column='`OPE_PARTIAL_PERC_FEE`'
              />
    <property name='OpeFixedFee'
              column='`OPE_FIXED_FEE`'
              />
    <property name='OpePartialFixedFee'
              column='`OPE_PARTIAL_FIXED_FEE`'
              />
    <property name='OpeTotalAmount'
              column='`OPE_TOTAL_AMOUNT`'
              />
    <property name='OpeAmountCurName'
              column='`OPE_AMOUNT_CUR_NAME`'
              />
    <property name='OpeBalanceCurName'
              column='`OPE_BALANCE_CUR_NAME`'
              />
    <property name='CuspmrCurName'
              column='`CUSPMR_CUR_NAME`'
              />
    <property name='OpedisAmountCurName'
              column='`OPEDIS_AMOUNT_CUR_NAME`'
              />
    <property name='OpedisBalanceCurName'
              column='`OPEDIS_BALANCE_CUR_NAME`'
              />
    <property name='OpeExternalId1'
              column='`OPE_EXTERNAL_ID1`'
              />
    <property name='OpeExternalId2'
              column='`OPE_EXTERNAL_ID2`'
              />
    <property name='OpeExternalId3'
              column='`OPE_EXTERNAL_ID3`'
              />
    <property name='OpeConfirmedInWs1'
              column='`OPE_CONFIRMED_IN_WS1`'
              />
    <property name='OpeConfirmedInWs2'
              column='`OPE_CONFIRMED_IN_WS2`'
              />
    <property name='OpeConfirmedInWs3'
              column='`OPE_CONFIRMED_IN_WS3`'
              />
    <property name='OpeConfirmationTimeInWs1'
              column='`OPE_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs1'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeConfirmationTimeInWs2'
              column='`OPE_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs2'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeConfirmationTimeInWs3'
              column='`OPE_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs3'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeLatitude'
              column='`OPE_LATITUDE`'
              />
    <property name='OpeLongitude'
              column='`OPE_LONGITUDE`'
              />
    <property name='OpeFinalAmount'
              column='`OPE_FINAL_AMOUNT`'
              />
    <property name='OpeBalanceCurId'
              column='`OPE_BALANCE_CUR_ID`'
              />
    <property name='OpeBalanceCurIsoCode'
              column='`OPE_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpeChangeApplied'
              column='`OPE_CHANGE_APPLIED`'
              />
    <property name='UsrpId'
              column='`USRP_ID`'
              />
    <property name='UsrpPlate'
              column='`USRP_PLATE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='SechSechtId'
              column='`SECH_SECHT_ID`'
              />
    <property name='GrpId'
              column='`GRP_ID`'
              />
    <property name='TarId'
              column='`TAR_ID`'
              />
    <property name='GrpDescription'
              column='`GRP_DESCRIPTION`'
              />
    <property name='TarDescription'
              column='`TAR_DESCRIPTION`'
              />
    <property name='OpeInsId'
              column='`OPE_INS_ID`'
              />
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='OpeDate'
              column='`OPE_DATE`'
              />
    <property name='OpeUtcDate'
              column='`OPE_UTC_DATE`'
              />
    <property name='OpeInidate'
              column='`OPE_INIDATE`'
              />
    <property name='OpeEnddate'
              column='`OPE_ENDDATE`'
              />
    <property name='OpeAmount'
              column='`OPE_AMOUNT`'
              />
    <property name='OpeTime'
              column='`OPE_TIME`'
              />
    <property name='OpeInsertionUtcDate'
              column='`OPE_INSERTION_UTC_DATE`'
              />
    <property name='OpeCuspmrId'
              column='`OPE_CUSPMR_ID`'
              />
    <property name='OpeOpedisId'
              column='`OPE_OPEDIS_ID`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CuspmrAmountIsoCode'
              column='`CUSPMR_AMOUNT_ISO_CODE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisAmountCurIsoCode'
              column='`OPEDIS_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisBalanceCurIsoCode'
              column='`OPEDIS_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='OpeDateUtcOffset'
              column='`OPE_DATE_UTC_OFFSET`'
              />
    <property name='OpeInidateUtcOffset'
              column='`OPE_INIDATE_UTC_OFFSET`'
              />
    <property name='OpeEnddateUtcOffset'
              column='`OPE_ENDDATE_UTC_OFFSET`'
              />
    <property name='OpeCuspmrDateUtcOffset'
              column='`OPE_CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='OpeOpedisDateUtcOffset'
              column='`OPE_OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='OpePercBonus'
              column='`OPE_PERC_BONUS`'
              />
    <property name='OpePartialBonusFee'
              column='`OPE_PARTIAL_BONUS_FEE`'
              />
    <property name='OpeCuspmrType'
              column='`OPE_CUSPMR_TYPE`'
              />
    <property name='OpeCuspmrPagateliaNewBalance'
              column='`OPE_CUSPMR_PAGATELIA_NEW_BALANCE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class EmailtoolRecipient
  {
    public virtual long EtrId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string EtrEmail { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != GetType())
      {
        return false;
      }

      EmailtoolRecipient other = (EmailtoolRecipient)obj;
      if (other.EtrId != EtrId)
      {
        return false;
      }
      if (other.EtrEmail != EtrEmail)
      {
        return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      hashCode = 19 * hashCode + EtrId.GetHashCode();
      hashCode = 19 * hashCode + EtrEmail.GetHashCode();
      return hashCode;
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(EmailtoolRecipient).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='EmailtoolRecipient'
         table='`EMAILTOOL_RECIPIENTS`'
         >
    <composite-id>
      <key-property name='EtrId'
                    column='`ETR_ID`'
                    />
      <key-property name='EtrEmail'
                    column='`ETR_EMAIL`'
                    />
    </composite-id>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwCustomerInvoice
  {
    public virtual decimal Id { get; set; }
    public virtual decimal CusinvId { get; set; }
    public virtual decimal CusinvCusId { get; set; }
    public virtual decimal CusinvOprId { get; set; }
    public virtual System.DateTime CusinvDateini { get; set; }
    public virtual System.DateTime CusinvDateend { get; set; }
    public virtual int CusinvInvAmount { get; set; }
    public virtual decimal CusinvCurId { get; set; }
    public virtual int CusinvInvoiceVersion { get; set; }
    public virtual System.Nullable<int> CusinvInvAmountOps { get; set; }
    public virtual System.Nullable<decimal> CusinvOprInitialInvoiceNumber { get; set; }
    public virtual System.Nullable<decimal> CusinvOprEndInvoiceNumber { get; set; }
    [Length(Max=50)]
    public virtual string CusinvInvNumber { get; set; }
    public virtual System.Nullable<System.DateTime> CusinvInvDate { get; set; }
    public virtual System.Nullable<System.DateTime> CusinvGenerationDate { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwCustomerInvoice).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwCustomerInvoice'
         table='`VW_CUSTOMER_INVOICES`'
         >
    <id name='Id'
        column='`CUSINV_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='CusinvId'
              column='`CUSINV_ID`'
              />
    <property name='CusinvCusId'
              column='`CUSINV_CUS_ID`'
              />
    <property name='CusinvOprId'
              column='`CUSINV_OPR_ID`'
              />
    <property name='CusinvDateini'
              column='`CUSINV_DATEINI`'
              />
    <property name='CusinvDateend'
              column='`CUSINV_DATEEND`'
              />
    <property name='CusinvInvAmount'
              column='`CUSINV_INV_AMOUNT`'
              />
    <property name='CusinvCurId'
              column='`CUSINV_CUR_ID`'
              />
    <property name='CusinvInvoiceVersion'
              column='`CUSINV_INVOICE_VERSION`'
              />
    <property name='CusinvInvAmountOps'
              column='`CUSINV_INV_AMOUNT_OPS`'
              />
    <property name='CusinvOprInitialInvoiceNumber'
              column='`CUSINV_OPR_INITIAL_INVOICE_NUMBER`'
              />
    <property name='CusinvOprEndInvoiceNumber'
              column='`CUSINV_OPR_END_INVOICE_NUMBER`'
              />
    <property name='CusinvInvNumber'
              column='`CUSINV_INV_NUMBER`'
              />
    <property name='CusinvInvDate'
              column='`CUSINV_INV_DATE`'
              />
    <property name='CusinvGenerationDate'
              column='`CUSINV_GENERATION_DATE`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwHisOperation
  {
    public virtual decimal id { get; set; }
    public virtual int OpeType { get; set; }
    public virtual decimal OpeUsrId { get; set; }
    public virtual int OpeMoseOs { get; set; }
    public virtual decimal OpeInsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual System.DateTime OpeDate { get; set; }
    public virtual System.DateTime OpeUtcDate { get; set; }
    public virtual System.DateTime OpeInidate { get; set; }
    public virtual System.DateTime OpeEnddate { get; set; }
    public virtual decimal OpeBalanceCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeBalanceCurIsoCode { get; set; }
    public virtual decimal OpeChangeApplied { get; set; }
    public virtual int OpeSuscriptionType { get; set; }
    public virtual int OpeBalanceBefore { get; set; }
    public virtual int OpeTime { get; set; }
    public virtual decimal OpeAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeAmountCurIsoCode { get; set; }
    public virtual int OpeDateUtcOffset { get; set; }
    public virtual int OpeInidateUtcOffset { get; set; }
    public virtual int OpeEnddateUtcOffset { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OpeAmountCurName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OpeBalanceCurName { get; set; }
    public virtual decimal OpeId { get; set; }
    public virtual int OpeConfirmedInWs1 { get; set; }
    public virtual int OpeConfirmedInWs2 { get; set; }
    public virtual int OpeConfirmedInWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeLongitude { get; set; }
    [Length(Max=20)]
    public virtual string OpeAppVersion { get; set; }
    public virtual System.Nullable<decimal> OpePercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercVat2 { get; set; }
    public virtual System.Nullable<int> OpePartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercFee { get; set; }
    public virtual System.Nullable<decimal> OpePercFeeTopped { get; set; }
    public virtual System.Nullable<int> OpePartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpeFixedFee { get; set; }
    public virtual System.Nullable<int> OpePartialFixedFee { get; set; }
    public virtual System.Nullable<int> OpeTotalAmount { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId3 { get; set; }
    public virtual System.Nullable<int> OpeCuspmrDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeOpedisDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeFinalAmount { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeOpedisId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<int> CuspmrAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmrCurId { get; set; }
    [Length(Max=10)]
    public virtual string CuspmrAmountIsoCode { get; set; }
    public virtual System.Nullable<int> CuspmrBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisDate { get; set; }
    public virtual System.Nullable<int> OpedisAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisAmountCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpedisFinalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisBalanceCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpedisChangeApplied { get; set; }
    public virtual System.Nullable<int> OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    public virtual System.Nullable<int> CuspmrOpReference { get; set; }
    public virtual System.Nullable<int> CuspmrTransactionId { get; set; }
    public virtual System.Nullable<int> CuspmrAuthCode { get; set; }
    public virtual System.Nullable<int> CuspmrCardHash { get; set; }
    public virtual System.Nullable<int> CuspmrCardReference { get; set; }
    public virtual System.Nullable<int> CuspmrCardScheme { get; set; }
    public virtual System.Nullable<int> CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<int> CuspmrCardExpirationDate { get; set; }
    public virtual System.Nullable<decimal> UsrpId { get; set; }
    [Length(Max=50)]
    public virtual string UsrpPlate { get; set; }
    public virtual System.Nullable<int> TipaTicketNumber { get; set; }
    public virtual System.Nullable<int> TipaTicketData { get; set; }
    public virtual System.Nullable<int> SechSechtId { get; set; }
    public virtual System.Nullable<decimal> GrpId { get; set; }
    public virtual System.Nullable<decimal> TarId { get; set; }
    [Length(Max=50)]
    public virtual string GrpDescription { get; set; }
    [Length(Max=50)]
    public virtual string TarDescription { get; set; }
    public virtual System.Nullable<int> OpeAmount { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrMainTel { get; set; }
    public virtual decimal OpePercBonus { get; set; }
    public virtual System.Nullable<int> OpePartialBonusFee { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwHisOperation).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwHisOperation'
         table='`VW_HIS_OPERATIONS`'
         >
    <id name='id'
        column='`OPE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='OpeUsrId'
              column='`OPE_USR_ID`'
              />
    <property name='OpeMoseOs'
              column='`OPE_MOSE_OS`'
              />
    <property name='OpeInsId'
              column='`OPE_INS_ID`'
              />
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='OpeDate'
              column='`OPE_DATE`'
              />
    <property name='OpeUtcDate'
              column='`OPE_UTC_DATE`'
              />
    <property name='OpeInidate'
              column='`OPE_INIDATE`'
              />
    <property name='OpeEnddate'
              column='`OPE_ENDDATE`'
              />
    <property name='OpeBalanceCurId'
              column='`OPE_BALANCE_CUR_ID`'
              />
    <property name='OpeBalanceCurIsoCode'
              column='`OPE_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpeChangeApplied'
              column='`OPE_CHANGE_APPLIED`'
              />
    <property name='OpeSuscriptionType'
              column='`OPE_SUSCRIPTION_TYPE`'
              />
    <property name='OpeBalanceBefore'
              column='`OPE_BALANCE_BEFORE`'
              />
    <property name='OpeTime'
              column='`OPE_TIME`'
              />
    <property name='OpeAmountCurId'
              column='`OPE_AMOUNT_CUR_ID`'
              />
    <property name='OpeAmountCurIsoCode'
              column='`OPE_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpeDateUtcOffset'
              column='`OPE_DATE_UTC_OFFSET`'
              />
    <property name='OpeInidateUtcOffset'
              column='`OPE_INIDATE_UTC_OFFSET`'
              />
    <property name='OpeEnddateUtcOffset'
              column='`OPE_ENDDATE_UTC_OFFSET`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='OpeAmountCurName'
              column='`OPE_AMOUNT_CUR_NAME`'
              />
    <property name='OpeBalanceCurName'
              column='`OPE_BALANCE_CUR_NAME`'
              />
    <property name='OpeId'
              column='`OPE_ID`'
              />
    <property name='OpeConfirmedInWs1'
              column='`OPE_CONFIRMED_IN_WS1`'
              />
    <property name='OpeConfirmedInWs2'
              column='`OPE_CONFIRMED_IN_WS2`'
              />
    <property name='OpeConfirmedInWs3'
              column='`OPE_CONFIRMED_IN_WS3`'
              />
    <property name='OpeConfirmationTimeInWs1'
              column='`OPE_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs1'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeConfirmationTimeInWs2'
              column='`OPE_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs2'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeConfirmationTimeInWs3'
              column='`OPE_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs3'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeLatitude'
              column='`OPE_LATITUDE`'
              />
    <property name='OpeLongitude'
              column='`OPE_LONGITUDE`'
              />
    <property name='OpeAppVersion'
              column='`OPE_APP_VERSION`'
              />
    <property name='OpePercVat1'
              column='`OPE_PERC_VAT1`'
              />
    <property name='OpePercVat2'
              column='`OPE_PERC_VAT2`'
              />
    <property name='OpePartialVat1'
              column='`OPE_PARTIAL_VAT1`'
              />
    <property name='OpePercFee'
              column='`OPE_PERC_FEE`'
              />
    <property name='OpePercFeeTopped'
              column='`OPE_PERC_FEE_TOPPED`'
              />
    <property name='OpePartialPercFee'
              column='`OPE_PARTIAL_PERC_FEE`'
              />
    <property name='OpeFixedFee'
              column='`OPE_FIXED_FEE`'
              />
    <property name='OpePartialFixedFee'
              column='`OPE_PARTIAL_FIXED_FEE`'
              />
    <property name='OpeTotalAmount'
              column='`OPE_TOTAL_AMOUNT`'
              />
    <property name='CuspmrCurName'
              column='`CUSPMR_CUR_NAME`'
              />
    <property name='OpedisAmountCurName'
              column='`OPEDIS_AMOUNT_CUR_NAME`'
              />
    <property name='OpedisBalanceCurName'
              column='`OPEDIS_BALANCE_CUR_NAME`'
              />
    <property name='OpeExternalId1'
              column='`OPE_EXTERNAL_ID1`'
              />
    <property name='OpeExternalId2'
              column='`OPE_EXTERNAL_ID2`'
              />
    <property name='OpeExternalId3'
              column='`OPE_EXTERNAL_ID3`'
              />
    <property name='OpeCuspmrDateUtcOffset'
              column='`OPE_CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='OpeOpedisDateUtcOffset'
              column='`OPE_OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='OpeFinalAmount'
              column='`OPE_FINAL_AMOUNT`'
              />
    <property name='OpeInsertionUtcDate'
              column='`OPE_INSERTION_UTC_DATE`'
              />
    <property name='OpeCuspmrId'
              column='`OPE_CUSPMR_ID`'
              />
    <property name='OpeOpedisId'
              column='`OPE_OPEDIS_ID`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CuspmrAmountIsoCode'
              column='`CUSPMR_AMOUNT_ISO_CODE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisAmountCurIsoCode'
              column='`OPEDIS_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisBalanceCurIsoCode'
              column='`OPEDIS_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='UsrpId'
              column='`USRP_ID`'
              />
    <property name='UsrpPlate'
              column='`USRP_PLATE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='SechSechtId'
              column='`SECH_SECHT_ID`'
              />
    <property name='GrpId'
              column='`GRP_ID`'
              />
    <property name='TarId'
              column='`TAR_ID`'
              />
    <property name='GrpDescription'
              column='`GRP_DESCRIPTION`'
              />
    <property name='TarDescription'
              column='`TAR_DESCRIPTION`'
              />
    <property name='OpeAmount'
              column='`OPE_AMOUNT`'
              />
    <property name='UsrMainTel'
              column='`USR_MAIN_TEL`'
              />
    <property name='OpePercBonus'
              column='`OPE_PERC_BONUS`'
              />
    <property name='OpePartialBonusFee'
              column='`OPE_PARTIAL_BONUS_FEE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwTicketPayment
  {
    public virtual decimal id { get; set; }
    public virtual int OpeType { get; set; }
    public virtual decimal TipaUsrId { get; set; }
    public virtual int TipaMoseOs { get; set; }
    public virtual decimal TipaInsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual System.DateTime TipaDate { get; set; }
    public virtual System.DateTime TipaUtcDate { get; set; }
    public virtual decimal TipaBalanceCurId { get; set; }
    public virtual decimal TipaChangeApplied { get; set; }
    public virtual decimal TipaAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeAmountCurIsoCode { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TipaTicketNumber { get; set; }
    public virtual int TipaSuscriptionType { get; set; }
    public virtual int TipaBalanceBefore { get; set; }
    public virtual decimal TipaId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TipaAmountCurName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string TipaBalanceCurName { get; set; }
    public virtual int TipaDateUtcOffset { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrMainTel { get; set; }
    public virtual System.Nullable<int> CuspmrDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpedisDateUtcOffset { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisBalanceCurName { get; set; }
    public virtual System.Nullable<int> TipaConfirmedInWs { get; set; }
    public virtual System.Nullable<decimal> TipaConfirmationTimeInWs { get; set; }
    public virtual System.Nullable<int> TipaQueueLengthBeforeConfirmWs { get; set; }
    public virtual System.Nullable<decimal> TipaLatitude { get; set; }
    public virtual System.Nullable<decimal> TipaLongitude { get; set; }
    [Length(Max=20)]
    public virtual string TipaAppVersion { get; set; }
    public virtual System.Nullable<decimal> TipaPercVat1 { get; set; }
    public virtual System.Nullable<decimal> TipaPercVat2 { get; set; }
    public virtual System.Nullable<decimal> TipaPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> TipaPercFee { get; set; }
    public virtual System.Nullable<decimal> TipaPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> TipaPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> TipaFixedFee { get; set; }
    public virtual System.Nullable<decimal> TipaPartialFixedFee { get; set; }
    public virtual System.Nullable<int> TipaTotalAmount { get; set; }
    public virtual System.Nullable<System.DateTime> TipaInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> TipaCuspmrId { get; set; }
    public virtual System.Nullable<decimal> TipaOpedisId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<int> CuspmrAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmrCurId { get; set; }
    [Length(Max=10)]
    public virtual string CurIsoCode { get; set; }
    public virtual System.Nullable<int> CuspmrBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisDate { get; set; }
    public virtual System.Nullable<int> OpedisAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisAmountCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpedisFinalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisBalanceCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpedisChangeApplied { get; set; }
    public virtual System.Nullable<int> OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    public virtual System.Nullable<int> CuspmrOpReference { get; set; }
    public virtual System.Nullable<int> CuspmrTransactionId { get; set; }
    public virtual System.Nullable<int> CuspmrAuthCode { get; set; }
    public virtual System.Nullable<int> CuspmrCardHash { get; set; }
    public virtual System.Nullable<int> CuspmrCardReference { get; set; }
    public virtual System.Nullable<int> CuspmrCardScheme { get; set; }
    public virtual System.Nullable<int> CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<int> CuspmrCardExpirationDate { get; set; }
    [Length(Max=300)]
    public virtual string TipaTicketData { get; set; }
    public virtual System.Nullable<decimal> GrpId { get; set; }
    public virtual System.Nullable<int> TipaFinalAmount { get; set; }
    public virtual System.Nullable<decimal> UsrpId { get; set; }
    [Length(Max=50)]
    public virtual string TipaPlateString { get; set; }
    public virtual System.Nullable<int> TipaAmount { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string TipaBalanceCurIsoCode { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwTicketPayment).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwTicketPayment'
         table='`VW_TICKET_PAYMENTS`'
         >
    <id name='id'
        column='`TIPA_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='TipaUsrId'
              column='`TIPA_USR_ID`'
              />
    <property name='TipaMoseOs'
              column='`TIPA_MOSE_OS`'
              />
    <property name='TipaInsId'
              column='`TIPA_INS_ID`'
              />
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='TipaDate'
              column='`TIPA_DATE`'
              />
    <property name='TipaUtcDate'
              column='`TIPA_UTC_DATE`'
              />
    <property name='TipaBalanceCurId'
              column='`TIPA_BALANCE_CUR_ID`'
              />
    <property name='TipaChangeApplied'
              column='`TIPA_CHANGE_APPLIED`'
              />
    <property name='TipaAmountCurId'
              column='`TIPA_AMOUNT_CUR_ID`'
              />
    <property name='OpeAmountCurIsoCode'
              column='`OPE_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaSuscriptionType'
              column='`TIPA_SUSCRIPTION_TYPE`'
              />
    <property name='TipaBalanceBefore'
              column='`TIPA_BALANCE_BEFORE`'
              />
    <property name='TipaId'
              column='`TIPA_ID`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='TipaAmountCurName'
              column='`TIPA_AMOUNT_CUR_NAME`'
              />
    <property name='TipaBalanceCurName'
              column='`TIPA_BALANCE_CUR_NAME`'
              />
    <property name='TipaDateUtcOffset'
              column='`TIPA_DATE_UTC_OFFSET`'
              />
    <property name='UsrMainTel'
              column='`USR_MAIN_TEL`'
              />
    <property name='CuspmrDateUtcOffset'
              column='`CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='OpedisDateUtcOffset'
              column='`OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='CuspmrCurName'
              column='`CUSPMR_CUR_NAME`'
              />
    <property name='OpedisAmountCurName'
              column='`OPEDIS_AMOUNT_CUR_NAME`'
              />
    <property name='OpedisBalanceCurName'
              column='`OPEDIS_BALANCE_CUR_NAME`'
              />
    <property name='TipaConfirmedInWs'
              column='`TIPA_CONFIRMED_IN_WS`'
              />
    <property name='TipaConfirmationTimeInWs'
              column='`TIPA_CONFIRMATION_TIME_IN_WS`'
              />
    <property name='TipaQueueLengthBeforeConfirmWs'
              column='`TIPA_QUEUE_LENGTH_BEFORE_CONFIRM_WS`'
              />
    <property name='TipaLatitude'
              column='`TIPA_LATITUDE`'
              />
    <property name='TipaLongitude'
              column='`TIPA_LONGITUDE`'
              />
    <property name='TipaAppVersion'
              column='`TIPA_APP_VERSION`'
              />
    <property name='TipaPercVat1'
              column='`TIPA_PERC_VAT1`'
              />
    <property name='TipaPercVat2'
              column='`TIPA_PERC_VAT2`'
              />
    <property name='TipaPartialVat1'
              column='`TIPA_PARTIAL_VAT1`'
              />
    <property name='TipaPercFee'
              column='`TIPA_PERC_FEE`'
              />
    <property name='TipaPercFeeTopped'
              column='`TIPA_PERC_FEE_TOPPED`'
              />
    <property name='TipaPartialPercFee'
              column='`TIPA_PARTIAL_PERC_FEE`'
              />
    <property name='TipaFixedFee'
              column='`TIPA_FIXED_FEE`'
              />
    <property name='TipaPartialFixedFee'
              column='`TIPA_PARTIAL_FIXED_FEE`'
              />
    <property name='TipaTotalAmount'
              column='`TIPA_TOTAL_AMOUNT`'
              />
    <property name='TipaInsertionUtcDate'
              column='`TIPA_INSERTION_UTC_DATE`'
              />
    <property name='TipaCuspmrId'
              column='`TIPA_CUSPMR_ID`'
              />
    <property name='TipaOpedisId'
              column='`TIPA_OPEDIS_ID`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CurIsoCode'
              column='`CUR_ISO_CODE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisAmountCurIsoCode'
              column='`OPEDIS_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisBalanceCurIsoCode'
              column='`OPEDIS_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='GrpId'
              column='`GRP_ID`'
              />
    <property name='TipaFinalAmount'
              column='`TIPA_FINAL_AMOUNT`'
              />
    <property name='UsrpId'
              column='`USRP_ID`'
              />
    <property name='TipaPlateString'
              column='`TIPA_PLATE_STRING`'
              />
    <property name='TipaAmount'
              column='`TIPA_AMOUNT`'
              />
    <property name='TipaBalanceCurIsoCode'
              column='`TIPA_BALANCE_CUR_ISO_CODE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwCustomerPaymentMeansRecharge
  {
    public virtual decimal id { get; set; }
    public virtual int OpeType { get; set; }
    public virtual int CuspmrMoseOs { get; set; }
    public virtual int CuspmrAmount { get; set; }
    public virtual decimal CuspmrCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string CurIsoCode { get; set; }
    public virtual int CuspmrSuscriptionType { get; set; }
    public virtual int CuspmrBalanceBefore { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string CuspmrTransactionId { get; set; }
    public virtual int CuspmrDateUtcOffset { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    public virtual decimal CuspmrId { get; set; }
    public virtual decimal CuspmrTotalAmount { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrMainTel { get; set; }
    [Length(Max=20)]
    public virtual string CuspmrAppVersion { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercVat1 { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercVat2 { get; set; }
    public virtual System.Nullable<decimal> CuspmrPartialVat1 { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrPercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> CuspmrPartialPercFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrFixedFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrPartialFixedFee { get; set; }
    public virtual System.Nullable<decimal> CuspmrLatitude { get; set; }
    public virtual System.Nullable<decimal> CuspmrLongitude { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrAuthCode { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardHash { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardReference { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCardScheme { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrCardExpirationDate { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrOpReference { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrUtcDate { get; set; }
    public virtual System.Nullable<decimal> CuspmrUsrId { get; set; }
    public virtual int CuspmrType { get; set; }
    public virtual System.Nullable<int> CuspmrPagateliaNewBalance { get; set; }
    public virtual int CuspmrCreationType { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwCustomerPaymentMeansRecharge).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwCustomerPaymentMeansRecharge'
         table='`VW_CUSTOMER_PAYMENT_MEANS_RECHARGES`'
         >
    <id name='id'
        column='`CUSPMR_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='CuspmrMoseOs'
              column='`CUSPMR_MOSE_OS`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CurIsoCode'
              column='`CUR_ISO_CODE`'
              />
    <property name='CuspmrSuscriptionType'
              column='`CUSPMR_SUSCRIPTION_TYPE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrDateUtcOffset'
              column='`CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='CuspmrId'
              column='`CUSPMR_ID`'
              />
    <property name='CuspmrTotalAmount'
              column='`CUSPMR_TOTAL_AMOUNT`'
              />
    <property name='UsrMainTel'
              column='`USR_MAIN_TEL`'
              />
    <property name='CuspmrAppVersion'
              column='`CUSPMR_APP_VERSION`'
              />
    <property name='CuspmrPercVat1'
              column='`CUSPMR_PERC_VAT1`'
              />
    <property name='CuspmrPercVat2'
              column='`CUSPMR_PERC_VAT2`'
              />
    <property name='CuspmrPartialVat1'
              column='`CUSPMR_PARTIAL_VAT1`'
              />
    <property name='CuspmrPercFee'
              column='`CUSPMR_PERC_FEE`'
              />
    <property name='CuspmrPercFeeTopped'
              column='`CUSPMR_PERC_FEE_TOPPED`'
              />
    <property name='CuspmrPartialPercFee'
              column='`CUSPMR_PARTIAL_PERC_FEE`'
              />
    <property name='CuspmrFixedFee'
              column='`CUSPMR_FIXED_FEE`'
              />
    <property name='CuspmrPartialFixedFee'
              column='`CUSPMR_PARTIAL_FIXED_FEE`'
              />
    <property name='CuspmrLatitude'
              column='`CUSPMR_LATITUDE`'
              />
    <property name='CuspmrLongitude'
              column='`CUSPMR_LONGITUDE`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrUtcDate'
              column='`CUSPMR_UTC_DATE`'
              />
    <property name='CuspmrUsrId'
              column='`CUSPMR_USR_ID`'
              />
    <property name='CuspmrType'
              column='`CUSPMR_TYPE`'
              />
    <property name='CuspmrPagateliaNewBalance'
              column='`CUSPMR_PAGATELIA_NEW_BALANCE`'
              />
    <property name='CuspmrCreationType'
              column='`CUSPMR_CREATION_TYPE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class BalanceTransfer
  {
    public virtual decimal BatId { get; set; }
    public virtual int BatMoseOs { get; set; }
    public virtual System.DateTime BatDate { get; set; }
    public virtual System.DateTime BatUtcDate { get; set; }
    public virtual int BatDateUtcOffset { get; set; }
    public virtual int BatAmount { get; set; }
    public virtual decimal BatChangeApplied { get; set; }
    public virtual decimal BatChangeFeeApplied { get; set; }
    public virtual int BatDstAmount { get; set; }
    public virtual int BatSrcBalanceBefore { get; set; }
    public virtual int BatDstBalanceBefore { get; set; }
    [Length(Max=20)]
    public virtual string BatAppVersion { get; set; }
    public virtual System.Nullable<System.DateTime> BatInsertionUtcDate { get; set; }

    public virtual Currency BatAmountCur { get; set; }

    public virtual Currency BatDstBalanceCur { get; set; }

    public virtual CustomerInvoice BatSrcCusinv { get; set; }

    public virtual CustomerInvoice BatDstCusinv { get; set; }

    public virtual MobileSession BatMose { get; set; }

    public virtual User BatSrcUsr { get; set; }

    public virtual User BatDstUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(BalanceTransfer).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='BalanceTransfer'
         table='`BALANCE_TRANSFERS`'
         >
    <id name='BatId'
        column='`BAT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='BatMoseOs'
              column='`BAT_MOSE_OS`'
              />
    <property name='BatDate'
              column='`BAT_DATE`'
              />
    <property name='BatUtcDate'
              column='`BAT_UTC_DATE`'
              />
    <property name='BatDateUtcOffset'
              column='`BAT_DATE_UTC_OFFSET`'
              />
    <property name='BatAmount'
              column='`BAT_AMOUNT`'
              />
    <property name='BatChangeApplied'
              column='`BAT_CHANGE_APPLIED`'
              />
    <property name='BatChangeFeeApplied'
              column='`BAT_CHANGE_FEE_APPLIED`'
              />
    <property name='BatDstAmount'
              column='`BAT_DST_AMOUNT`'
              />
    <property name='BatSrcBalanceBefore'
              column='`BAT_SRC_BALANCE_BEFORE`'
              />
    <property name='BatDstBalanceBefore'
              column='`BAT_DST_BALANCE_BEFORE`'
              />
    <property name='BatAppVersion'
              column='`BAT_APP_VERSION`'
              />
    <property name='BatInsertionUtcDate'
              column='`BAT_INSERTION_UTC_DATE`'
              />
    <many-to-one name='BatAmountCur' class='Currency' column='`BAT_AMOUNT_CUR_ID`' />
    <many-to-one name='BatDstBalanceCur' class='Currency' column='`BAT_DST_BALANCE_CUR_ID`' />
    <many-to-one name='BatSrcCusinv' class='CustomerInvoice' column='`BAT_SRC_CUSINV_ID`' />
    <many-to-one name='BatDstCusinv' class='CustomerInvoice' column='`BAT_DST_CUSINV_ID`' />
    <many-to-one name='BatMose' class='MobileSession' column='`BAT_MOSE_ID`' />
    <many-to-one name='BatSrcUsr' class='User' column='`BAT_SRC_USR_ID`' />
    <many-to-one name='BatDstUsr' class='User' column='`BAT_DST_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwBalanceTransfer
  {
    public virtual decimal id { get; set; }
    public virtual decimal BatId { get; set; }
    public virtual decimal BatSrcUsrId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string BatSrcUsrUsername { get; set; }
    public virtual decimal BatDstUsrId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string BatDstUsrUsername { get; set; }
    public virtual int BatMoseOs { get; set; }
    public virtual System.DateTime BatDate { get; set; }
    public virtual System.DateTime BatUtcDate { get; set; }
    public virtual int BatDateUtcOffset { get; set; }
    public virtual int BatAmount { get; set; }
    public virtual decimal BatAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string BatAmountCurIsoCode { get; set; }
    public virtual decimal BatDstBalanceCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string BatDstBalanceCurIsoCode { get; set; }
    public virtual decimal BatChangeApplied { get; set; }
    public virtual decimal BatChangeFeeApplied { get; set; }
    public virtual int BatDstAmount { get; set; }
    public virtual int BatSrcBalanceBefore { get; set; }
    public virtual int BatDstBalanceBefore { get; set; }
    public virtual System.Nullable<decimal> BatMoseId { get; set; }
    [Length(Max=20)]
    public virtual string BatAppVersion { get; set; }
    public virtual System.Nullable<decimal> BatSrcCusinvId { get; set; }
    public virtual System.Nullable<decimal> BatDstCusinvId { get; set; }
    public virtual System.Nullable<System.DateTime> BatInsertionUtcDate { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwBalanceTransfer).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwBalanceTransfer'
         table='`VW_BALANCE_TRANSFERS`'
         >
    <id name='id'
        column='`BAT_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='BatId'
              column='`BAT_ID`'
              />
    <property name='BatSrcUsrId'
              column='`BAT_SRC_USR_ID`'
              />
    <property name='BatSrcUsrUsername'
              column='`BAT_SRC_USR_USERNAME`'
              />
    <property name='BatDstUsrId'
              column='`BAT_DST_USR_ID`'
              />
    <property name='BatDstUsrUsername'
              column='`BAT_DST_USR_USERNAME`'
              />
    <property name='BatMoseOs'
              column='`BAT_MOSE_OS`'
              />
    <property name='BatDate'
              column='`BAT_DATE`'
              />
    <property name='BatUtcDate'
              column='`BAT_UTC_DATE`'
              />
    <property name='BatDateUtcOffset'
              column='`BAT_DATE_UTC_OFFSET`'
              />
    <property name='BatAmount'
              column='`BAT_AMOUNT`'
              />
    <property name='BatAmountCurId'
              column='`BAT_AMOUNT_CUR_ID`'
              />
    <property name='BatAmountCurIsoCode'
              column='`BAT_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='BatDstBalanceCurId'
              column='`BAT_DST_BALANCE_CUR_ID`'
              />
    <property name='BatDstBalanceCurIsoCode'
              column='`BAT_DST_BALANCE_CUR_ISO_CODE`'
              />
    <property name='BatChangeApplied'
              column='`BAT_CHANGE_APPLIED`'
              />
    <property name='BatChangeFeeApplied'
              column='`BAT_CHANGE_FEE_APPLIED`'
              />
    <property name='BatDstAmount'
              column='`BAT_DST_AMOUNT`'
              />
    <property name='BatSrcBalanceBefore'
              column='`BAT_SRC_BALANCE_BEFORE`'
              />
    <property name='BatDstBalanceBefore'
              column='`BAT_DST_BALANCE_BEFORE`'
              />
    <property name='BatMoseId'
              column='`BAT_MOSE_ID`'
              />
    <property name='BatAppVersion'
              column='`BAT_APP_VERSION`'
              />
    <property name='BatSrcCusinvId'
              column='`BAT_SRC_CUSINV_ID`'
              />
    <property name='BatDstCusinvId'
              column='`BAT_DST_CUSINV_ID`'
              />
    <property name='BatInsertionUtcDate'
              column='`BAT_INSERTION_UTC_DATE`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class UsersFriend
  {
    public virtual decimal UsrfId { get; set; }
    public virtual System.DateTime UsrfDate { get; set; }
    [NotNull]
    [Length(Max=256)]
    public virtual string UsrfFriendEmail { get; set; }
    public virtual decimal UsrfSenderId { get; set; }
    public virtual int UsrfStatus { get; set; }
    public virtual System.Nullable<System.DateTime> UsrfAcceptDate { get; set; }

    public virtual User UsrfAcceptUsr { get; set; }

    public virtual User UsrfUsr { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(UsersFriend).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='UsersFriend'
         table='`USERS_FRIENDS`'
         >
    <id name='UsrfId'
        column='`USRF_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrfDate'
              column='`USRF_DATE`'
              />
    <property name='UsrfFriendEmail'
              column='`USRF_FRIEND_EMAIL`'
              />
    <property name='UsrfSenderId'
              column='`USRF_SENDER_ID`'
              />
    <property name='UsrfStatus'
              column='`USRF_STATUS`'
              />
    <property name='UsrfAcceptDate'
              column='`USRF_ACCEPT_DATE`'
              />
    <many-to-one name='UsrfAcceptUsr' class='User' column='`USRF_ACCEPT_USR_ID`' />
    <many-to-one name='UsrfUsr' class='User' column='`USRF_USR_ID`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwUsersFriend
  {
    public virtual decimal Id { get; set; }
    public virtual decimal UsrfId { get; set; }
    public virtual decimal UsrfUsrId { get; set; }
    public virtual System.DateTime UsrfDate { get; set; }
    [NotNull]
    [Length(Max=256)]
    public virtual string UsrfFriendEmail { get; set; }
    public virtual decimal UsrfSenderId { get; set; }
    public virtual int UsrfStatus { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    [Length(Max=50)]
    public virtual string AcceptUsrUsername { get; set; }
    public virtual System.Nullable<System.DateTime> UsrfAcceptDate { get; set; }
    public virtual System.Nullable<decimal> UsrfAcceptUsrId { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwUsersFriend).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwUsersFriend'
         table='`VW_USERS_FRIENDS`'
         >
    <id name='Id'
        column='`USRF_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UsrfId'
              column='`USRF_ID`'
              />
    <property name='UsrfUsrId'
              column='`USRF_USR_ID`'
              />
    <property name='UsrfDate'
              column='`USRF_DATE`'
              />
    <property name='UsrfFriendEmail'
              column='`USRF_FRIEND_EMAIL`'
              />
    <property name='UsrfSenderId'
              column='`USRF_SENDER_ID`'
              />
    <property name='UsrfStatus'
              column='`USRF_STATUS`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='AcceptUsrUsername'
              column='`ACCEPT_USR_USERNAME`'
              />
    <property name='UsrfAcceptDate'
              column='`USRF_ACCEPT_DATE`'
              />
    <property name='UsrfAcceptUsrId'
              column='`USRF_ACCEPT_USR_ID`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwUsersPushId
  {
    public virtual decimal Id { get; set; }
    public virtual decimal UpidId { get; set; }
    public virtual int UpidOs { get; set; }
    [NotNull]
    [Length(Max=512)]
    public virtual string UpidPushid { get; set; }
    public virtual decimal UpidUsrId { get; set; }
    public virtual System.DateTime UpidLastUpdateDatetime { get; set; }
    public virtual int UpidPushRetries { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    public virtual System.Nullable<System.DateTime> UpidLastRetryDatetime { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellModel { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellSerialnumber { get; set; }
    [Length(Max=100)]
    public virtual string UpidOsVersion { get; set; }
    public virtual System.Nullable<System.DateTime> UpidLastSucessfulPush { get; set; }
    [Length(Max=20)]
    public virtual string UpidAppVersion { get; set; }
    public virtual System.Nullable<int> UpidAppSessionKeepAlive { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellWifiMac { get; set; }
    [Length(Max=100)]
    public virtual string UpidCellImei { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwUsersPushId).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwUsersPushId'
         table='`VW_USERS_PUSH_ID`'
         >
    <id name='Id'
        column='`UPID_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='UpidId'
              column='`UPID_ID`'
              />
    <property name='UpidOs'
              column='`UPID_OS`'
              />
    <property name='UpidPushid'
              column='`UPID_PUSHID`'
              />
    <property name='UpidUsrId'
              column='`UPID_USR_ID`'
              />
    <property name='UpidLastUpdateDatetime'
              column='`UPID_LAST_UPDATE_DATETIME`'
              />
    <property name='UpidPushRetries'
              column='`UPID_PUSH_RETRIES`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='UpidLastRetryDatetime'
              column='`UPID_LAST_RETRY_DATETIME`'
              />
    <property name='UpidCellModel'
              column='`UPID_CELL_MODEL`'
              />
    <property name='UpidCellSerialnumber'
              column='`UPID_CELL_SERIALNUMBER`'
              />
    <property name='UpidOsVersion'
              column='`UPID_OS_VERSION`'
              />
    <property name='UpidLastSucessfulPush'
              column='`UPID_LAST_SUCESSFUL_PUSH`'
              />
    <property name='UpidAppVersion'
              column='`UPID_APP_VERSION`'
              />
    <property name='UpidAppSessionKeepAlive'
              column='`UPID_APP_SESSION_KEEP_ALIVE`'
              />
    <property name='UpidCellWifiMac'
              column='`UPID_CELL_WIFI_MAC`'
              />
    <property name='UpidCellImei'
              column='`UPID_CELL_IMEI`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwRechargeCoupon
  {
    public virtual decimal Id { get; set; }
    public virtual decimal RcoupId { get; set; }
    [NotNull]
    [Length(Max=100)]
    public virtual string RcoupCode { get; set; }
    public virtual int RcoupCoupsId { get; set; }
    public virtual decimal RcoupValue { get; set; }
    public virtual decimal RcoupCurId { get; set; }
    public virtual System.DateTime RcoupStartDate { get; set; }
    public virtual System.DateTime RcoupExpDate { get; set; }
    public virtual System.Nullable<decimal> RcoupRtlpyId { get; set; }
    [Length(Max=100)]
    public virtual string RcoupKeycode { get; set; }
    [Length(Max=50)]
    public virtual string RtlName { get; set; }
    [Length(Max=50)]
    public virtual string RtlEmail { get; set; }
    [Length(Max=200)]
    public virtual string RtlAddress { get; set; }
    [Length(Max=50)]
    public virtual string RtlDocId { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwRechargeCoupon).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwRechargeCoupon'
         table='`VW_RECHARGE_COUPONS`'
         >
    <id name='Id'
        column='`RCOUP_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='RcoupId'
              column='`RCOUP_ID`'
              />
    <property name='RcoupCode'
              column='`RCOUP_CODE`'
              />
    <property name='RcoupCoupsId'
              column='`RCOUP_COUPS_ID`'
              />
    <property name='RcoupValue'
              column='`RCOUP_VALUE`'
              />
    <property name='RcoupCurId'
              column='`RCOUP_CUR_ID`'
              />
    <property name='RcoupStartDate'
              column='`RCOUP_START_DATE`'
              />
    <property name='RcoupExpDate'
              column='`RCOUP_EXP_DATE`'
              />
    <property name='RcoupRtlpyId'
              column='`RCOUP_RTLPY_ID`'
              />
    <property name='RcoupKeycode'
              column='`RCOUP_KEYCODE`'
              />
    <property name='RtlName'
              column='`RTL_NAME`'
              />
    <property name='RtlEmail'
              column='`RTL_EMAIL`'
              />
    <property name='RtlAddress'
              column='`RTL_ADDRESS`'
              />
    <property name='RtlDocId'
              column='`RTL_DOC_ID`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class VwHisOperationsRestricted
  {
    public virtual decimal id { get; set; }
    public virtual int OpeType { get; set; }
    public virtual decimal OpeUsrId { get; set; }
    public virtual int OpeMoseOs { get; set; }
    public virtual decimal OpeInsId { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsDescription { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string InsShortdesc { get; set; }
    public virtual System.DateTime OpeDate { get; set; }
    public virtual System.DateTime OpeUtcDate { get; set; }
    public virtual decimal OpeBalanceCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeBalanceCurIsoCode { get; set; }
    public virtual decimal OpeChangeApplied { get; set; }
    public virtual int OpeSuscriptionType { get; set; }
    public virtual int OpeBalanceBefore { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrUsername { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OpeAmountCurName { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string OpeBalanceCurName { get; set; }
    public virtual decimal OpeId { get; set; }
    public virtual int OpeDateUtcOffset { get; set; }
    public virtual decimal OpeAmountCurId { get; set; }
    [NotNull]
    [Length(Max=10)]
    public virtual string OpeAmountCurIsoCode { get; set; }
    public virtual decimal OpePercBonus { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string UsrMainTel { get; set; }
    public virtual System.Nullable<int> OpePartialBonusFee { get; set; }
    public virtual System.Nullable<int> OpeTotalAmount { get; set; }
    public virtual System.Nullable<int> OpeFinalAmount { get; set; }
    public virtual System.Nullable<int> OpeInidateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeEnddateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeCuspmrDateUtcOffset { get; set; }
    public virtual System.Nullable<int> OpeOpedisDateUtcOffset { get; set; }
    [Length(Max=20)]
    public virtual string OpeAppVersion { get; set; }
    public virtual System.Nullable<decimal> OpePercVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercVat2 { get; set; }
    public virtual System.Nullable<decimal> OpePartialVat1 { get; set; }
    public virtual System.Nullable<decimal> OpePercFee { get; set; }
    public virtual System.Nullable<decimal> OpePercFeeTopped { get; set; }
    public virtual System.Nullable<decimal> OpePartialPercFee { get; set; }
    public virtual System.Nullable<decimal> OpeFixedFee { get; set; }
    public virtual System.Nullable<decimal> OpePartialFixedFee { get; set; }
    [Length(Max=50)]
    public virtual string CuspmrCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisAmountCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpedisBalanceCurName { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId1 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId2 { get; set; }
    [Length(Max=50)]
    public virtual string OpeExternalId3 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs1 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs2 { get; set; }
    public virtual System.Nullable<int> OpeConfirmedInWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs1 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs1 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs2 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs2 { get; set; }
    public virtual System.Nullable<decimal> OpeConfirmationTimeInWs3 { get; set; }
    public virtual System.Nullable<int> OpeQueueLengthBeforeConfirmWs3 { get; set; }
    public virtual System.Nullable<decimal> OpeLatitude { get; set; }
    public virtual System.Nullable<decimal> OpeLongitude { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInsertionUtcDate { get; set; }
    public virtual System.Nullable<decimal> OpeCuspmrId { get; set; }
    public virtual System.Nullable<decimal> OpeOpedisId { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrDate { get; set; }
    public virtual System.Nullable<int> CuspmrAmount { get; set; }
    public virtual System.Nullable<decimal> CuspmrCurId { get; set; }
    [Length(Max=10)]
    public virtual string CuspmrAmountIsoCode { get; set; }
    public virtual System.Nullable<int> CuspmrBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> CuspmrInsertionUtcDate { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisDate { get; set; }
    public virtual System.Nullable<int> OpedisAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisAmountCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisAmountCurIsoCode { get; set; }
    public virtual System.Nullable<int> OpedisFinalAmount { get; set; }
    public virtual System.Nullable<decimal> OpedisBalanceCurId { get; set; }
    [Length(Max=10)]
    public virtual string OpedisBalanceCurIsoCode { get; set; }
    public virtual System.Nullable<decimal> OpedisChangeApplied { get; set; }
    public virtual System.Nullable<int> OpedisBalanceBefore { get; set; }
    public virtual System.Nullable<System.DateTime> OpedisInsertionUtcDate { get; set; }
    public virtual System.Nullable<int> CuspmrOpReference { get; set; }
    public virtual System.Nullable<int> CuspmrTransactionId { get; set; }
    public virtual System.Nullable<int> CuspmrAuthCode { get; set; }
    public virtual System.Nullable<int> CuspmrCardHash { get; set; }
    public virtual System.Nullable<int> CuspmrCardReference { get; set; }
    public virtual System.Nullable<int> CuspmrCardScheme { get; set; }
    public virtual System.Nullable<int> CuspmrMaskedCardNumber { get; set; }
    public virtual System.Nullable<int> CuspmrCardExpirationDate { get; set; }
    public virtual System.Nullable<decimal> UsrpId { get; set; }
    [Length(Max=50)]
    public virtual string UsrpPlate { get; set; }
    [Length(Max=50)]
    public virtual string TipaTicketNumber { get; set; }
    [Length(Max=300)]
    public virtual string TipaTicketData { get; set; }
    public virtual System.Nullable<int> SechSechtId { get; set; }
    public virtual System.Nullable<decimal> GrpId { get; set; }
    public virtual System.Nullable<decimal> TarId { get; set; }
    [Length(Max=50)]
    public virtual string GrpDescription { get; set; }
    [Length(Max=50)]
    public virtual string TarDescription { get; set; }
    public virtual System.Nullable<System.DateTime> OpeInidate { get; set; }
    public virtual System.Nullable<System.DateTime> OpeEnddate { get; set; }
    public virtual System.Nullable<int> OpeAmount { get; set; }
    public virtual System.Nullable<int> OpeTime { get; set; }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(VwHisOperationsRestricted).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
  <class name='VwHisOperationsRestricted'
         table='`VW_HIS_OPERATIONS_RESTRICTED`'
         >
    <id name='id'
        column='`OPE_ID`'
        >
      <generator class='identity'>
      </generator>
    </id>
    <property name='OpeType'
              column='`OPE_TYPE`'
              />
    <property name='OpeUsrId'
              column='`OPE_USR_ID`'
              />
    <property name='OpeMoseOs'
              column='`OPE_MOSE_OS`'
              />
    <property name='OpeInsId'
              column='`OPE_INS_ID`'
              />
    <property name='InsDescription'
              column='`INS_DESCRIPTION`'
              />
    <property name='InsShortdesc'
              column='`INS_SHORTDESC`'
              />
    <property name='OpeDate'
              column='`OPE_DATE`'
              />
    <property name='OpeUtcDate'
              column='`OPE_UTC_DATE`'
              />
    <property name='OpeBalanceCurId'
              column='`OPE_BALANCE_CUR_ID`'
              />
    <property name='OpeBalanceCurIsoCode'
              column='`OPE_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpeChangeApplied'
              column='`OPE_CHANGE_APPLIED`'
              />
    <property name='OpeSuscriptionType'
              column='`OPE_SUSCRIPTION_TYPE`'
              />
    <property name='OpeBalanceBefore'
              column='`OPE_BALANCE_BEFORE`'
              />
    <property name='UsrUsername'
              column='`USR_USERNAME`'
              />
    <property name='OpeAmountCurName'
              column='`OPE_AMOUNT_CUR_NAME`'
              />
    <property name='OpeBalanceCurName'
              column='`OPE_BALANCE_CUR_NAME`'
              />
    <property name='OpeId'
              column='`OPE_ID`'
              />
    <property name='OpeDateUtcOffset'
              column='`OPE_DATE_UTC_OFFSET`'
              />
    <property name='OpeAmountCurId'
              column='`OPE_AMOUNT_CUR_ID`'
              />
    <property name='OpeAmountCurIsoCode'
              column='`OPE_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpePercBonus'
              column='`OPE_PERC_BONUS`'
              />
    <property name='UsrMainTel'
              column='`USR_MAIN_TEL`'
              />
    <property name='OpePartialBonusFee'
              column='`OPE_PARTIAL_BONUS_FEE`'
              />
    <property name='OpeTotalAmount'
              column='`OPE_TOTAL_AMOUNT`'
              />
    <property name='OpeFinalAmount'
              column='`OPE_FINAL_AMOUNT`'
              />
    <property name='OpeInidateUtcOffset'
              column='`OPE_INIDATE_UTC_OFFSET`'
              />
    <property name='OpeEnddateUtcOffset'
              column='`OPE_ENDDATE_UTC_OFFSET`'
              />
    <property name='OpeCuspmrDateUtcOffset'
              column='`OPE_CUSPMR_DATE_UTC_OFFSET`'
              />
    <property name='OpeOpedisDateUtcOffset'
              column='`OPE_OPEDIS_DATE_UTC_OFFSET`'
              />
    <property name='OpeAppVersion'
              column='`OPE_APP_VERSION`'
              />
    <property name='OpePercVat1'
              column='`OPE_PERC_VAT1`'
              />
    <property name='OpePercVat2'
              column='`OPE_PERC_VAT2`'
              />
    <property name='OpePartialVat1'
              column='`OPE_PARTIAL_VAT1`'
              />
    <property name='OpePercFee'
              column='`OPE_PERC_FEE`'
              />
    <property name='OpePercFeeTopped'
              column='`OPE_PERC_FEE_TOPPED`'
              />
    <property name='OpePartialPercFee'
              column='`OPE_PARTIAL_PERC_FEE`'
              />
    <property name='OpeFixedFee'
              column='`OPE_FIXED_FEE`'
              />
    <property name='OpePartialFixedFee'
              column='`OPE_PARTIAL_FIXED_FEE`'
              />
    <property name='CuspmrCurName'
              column='`CUSPMR_CUR_NAME`'
              />
    <property name='OpedisAmountCurName'
              column='`OPEDIS_AMOUNT_CUR_NAME`'
              />
    <property name='OpedisBalanceCurName'
              column='`OPEDIS_BALANCE_CUR_NAME`'
              />
    <property name='OpeExternalId1'
              column='`OPE_EXTERNAL_ID1`'
              />
    <property name='OpeExternalId2'
              column='`OPE_EXTERNAL_ID2`'
              />
    <property name='OpeExternalId3'
              column='`OPE_EXTERNAL_ID3`'
              />
    <property name='OpeConfirmedInWs1'
              column='`OPE_CONFIRMED_IN_WS1`'
              />
    <property name='OpeConfirmedInWs2'
              column='`OPE_CONFIRMED_IN_WS2`'
              />
    <property name='OpeConfirmedInWs3'
              column='`OPE_CONFIRMED_IN_WS3`'
              />
    <property name='OpeConfirmationTimeInWs1'
              column='`OPE_CONFIRMATION_TIME_IN_WS1`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs1'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS1`'
              />
    <property name='OpeConfirmationTimeInWs2'
              column='`OPE_CONFIRMATION_TIME_IN_WS2`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs2'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS2`'
              />
    <property name='OpeConfirmationTimeInWs3'
              column='`OPE_CONFIRMATION_TIME_IN_WS3`'
              />
    <property name='OpeQueueLengthBeforeConfirmWs3'
              column='`OPE_QUEUE_LENGTH_BEFORE_CONFIRM_WS3`'
              />
    <property name='OpeLatitude'
              column='`OPE_LATITUDE`'
              />
    <property name='OpeLongitude'
              column='`OPE_LONGITUDE`'
              />
    <property name='OpeInsertionUtcDate'
              column='`OPE_INSERTION_UTC_DATE`'
              />
    <property name='OpeCuspmrId'
              column='`OPE_CUSPMR_ID`'
              />
    <property name='OpeOpedisId'
              column='`OPE_OPEDIS_ID`'
              />
    <property name='CuspmrDate'
              column='`CUSPMR_DATE`'
              />
    <property name='CuspmrAmount'
              column='`CUSPMR_AMOUNT`'
              />
    <property name='CuspmrCurId'
              column='`CUSPMR_CUR_ID`'
              />
    <property name='CuspmrAmountIsoCode'
              column='`CUSPMR_AMOUNT_ISO_CODE`'
              />
    <property name='CuspmrBalanceBefore'
              column='`CUSPMR_BALANCE_BEFORE`'
              />
    <property name='CuspmrInsertionUtcDate'
              column='`CUSPMR_INSERTION_UTC_DATE`'
              />
    <property name='OpedisDate'
              column='`OPEDIS_DATE`'
              />
    <property name='OpedisAmount'
              column='`OPEDIS_AMOUNT`'
              />
    <property name='OpedisAmountCurId'
              column='`OPEDIS_AMOUNT_CUR_ID`'
              />
    <property name='OpedisAmountCurIsoCode'
              column='`OPEDIS_AMOUNT_CUR_ISO_CODE`'
              />
    <property name='OpedisFinalAmount'
              column='`OPEDIS_FINAL_AMOUNT`'
              />
    <property name='OpedisBalanceCurId'
              column='`OPEDIS_BALANCE_CUR_ID`'
              />
    <property name='OpedisBalanceCurIsoCode'
              column='`OPEDIS_BALANCE_CUR_ISO_CODE`'
              />
    <property name='OpedisChangeApplied'
              column='`OPEDIS_CHANGE_APPLIED`'
              />
    <property name='OpedisBalanceBefore'
              column='`OPEDIS_BALANCE_BEFORE`'
              />
    <property name='OpedisInsertionUtcDate'
              column='`OPEDIS_INSERTION_UTC_DATE`'
              />
    <property name='CuspmrOpReference'
              column='`CUSPMR_OP_REFERENCE`'
              />
    <property name='CuspmrTransactionId'
              column='`CUSPMR_TRANSACTION_ID`'
              />
    <property name='CuspmrAuthCode'
              column='`CUSPMR_AUTH_CODE`'
              />
    <property name='CuspmrCardHash'
              column='`CUSPMR_CARD_HASH`'
              />
    <property name='CuspmrCardReference'
              column='`CUSPMR_CARD_REFERENCE`'
              />
    <property name='CuspmrCardScheme'
              column='`CUSPMR_CARD_SCHEME`'
              />
    <property name='CuspmrMaskedCardNumber'
              column='`CUSPMR_MASKED_CARD_NUMBER`'
              />
    <property name='CuspmrCardExpirationDate'
              column='`CUSPMR_CARD_EXPIRATION_DATE`'
              />
    <property name='UsrpId'
              column='`USRP_ID`'
              />
    <property name='UsrpPlate'
              column='`USRP_PLATE`'
              />
    <property name='TipaTicketNumber'
              column='`TIPA_TICKET_NUMBER`'
              />
    <property name='TipaTicketData'
              column='`TIPA_TICKET_DATA`'
              />
    <property name='SechSechtId'
              column='`SECH_SECHT_ID`'
              />
    <property name='GrpId'
              column='`GRP_ID`'
              />
    <property name='TarId'
              column='`TAR_ID`'
              />
    <property name='GrpDescription'
              column='`GRP_DESCRIPTION`'
              />
    <property name='TarDescription'
              column='`TAR_DESCRIPTION`'
              />
    <property name='OpeInidate'
              column='`OPE_INIDATE`'
              />
    <property name='OpeEnddate'
              column='`OPE_ENDDATE`'
              />
    <property name='OpeAmount'
              column='`OPE_AMOUNT`'
              />
    <property name='OpeTime'
              column='`OPE_TIME`'
              />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }


  /// <summary>
  /// Provides a NHibernate configuration object containing mappings for this model.
  /// </summary>
  public static class ConfigurationHelper
  {
    /// <summary>
    /// Creates a NHibernate configuration object containing mappings for this model.
    /// </summary>
    /// <returns>A NHibernate configuration object containing mappings for this model.</returns>
    public static Configuration CreateConfiguration()
    {
      var configuration = new Configuration();
      configuration.Configure();
      ApplyConfiguration(configuration);
      return configuration;
    }

    /// <summary>
    /// Adds mappings for this model to a NHibernate configuration object.
    /// </summary>
    /// <param name="configuration">A NHibernate configuration object to which to add mappings for this model.</param>
    public static void ApplyConfiguration(Configuration configuration)
    {
      configuration.AddXml(ModelMappingXml.ToString());
      configuration.AddXml(Country.MappingXml.ToString());
      configuration.AddXml(UsersSecurityOperation.MappingXml.ToString());
      configuration.AddXml(Currency.MappingXml.ToString());
      configuration.AddXml(CurrencyRechargeValue.MappingXml.ToString());
      configuration.AddXml(CustomerInscription.MappingXml.ToString());
      configuration.AddXml(CustomerInvoice.MappingXml.ToString());
      configuration.AddXml(CustomerPaymentMean.MappingXml.ToString());
      configuration.AddXml(CustomerPaymentMeansRecharge.MappingXml.ToString());
      configuration.AddXml(Customer.MappingXml.ToString());
      configuration.AddXml(ExternalParkingOperation.MappingXml.ToString());
      configuration.AddXml(ExternalProvider.MappingXml.ToString());
      configuration.AddXml(ExternalTicket.MappingXml.ToString());
      configuration.AddXml(Group.MappingXml.ToString());
      configuration.AddXml(GroupsGeometry.MappingXml.ToString());
      configuration.AddXml(GroupsHierarchy.MappingXml.ToString());
      configuration.AddXml(GroupsOffstreetWsConfiguration.MappingXml.ToString());
      configuration.AddXml(GroupsTariffsExternalTranslation.MappingXml.ToString());
      configuration.AddXml(GroupsType.MappingXml.ToString());
      configuration.AddXml(GroupsTypesAssignation.MappingXml.ToString());
      configuration.AddXml(InstallationsGeometry.MappingXml.ToString());
      configuration.AddXml(InvoicingSchema.MappingXml.ToString());
      configuration.AddXml(Language.MappingXml.ToString());
      configuration.AddXml(LiteralLanguage.MappingXml.ToString());
      configuration.AddXml(Literal.MappingXml.ToString());
      configuration.AddXml(MobileSession.MappingXml.ToString());
      configuration.AddXml(OffstreetAutomaticOperation.MappingXml.ToString());
      configuration.AddXml(Operation.MappingXml.ToString());
      configuration.AddXml(OperationsDiscount.MappingXml.ToString());
      configuration.AddXml(OperationsOffstreet.MappingXml.ToString());
      configuration.AddXml(OperationsOffstreetSessionInfo.MappingXml.ToString());
      configuration.AddXml(OperationsSessionInfo.MappingXml.ToString());
      configuration.AddXml(Operator.MappingXml.ToString());
      configuration.AddXml(Parameter.MappingXml.ToString());
      configuration.AddXml(PaymentSubtype.MappingXml.ToString());
      configuration.AddXml(PaymentType.MappingXml.ToString());
      configuration.AddXml(PlatesTariff.MappingXml.ToString());
      configuration.AddXml(PushidNotification.MappingXml.ToString());
      configuration.AddXml(RechargeCoupon.MappingXml.ToString());
      configuration.AddXml(RechargeCouponsStatus.MappingXml.ToString());
      configuration.AddXml(RechargeCouponsUse.MappingXml.ToString());
      configuration.AddXml(RechargeValue.MappingXml.ToString());
      configuration.AddXml(RetailerPayment.MappingXml.ToString());
      configuration.AddXml(Retailer.MappingXml.ToString());
      configuration.AddXml(ServiceChargeType.MappingXml.ToString());
      configuration.AddXml(ServiceCharge.MappingXml.ToString());
      configuration.AddXml(Tariff.MappingXml.ToString());
      configuration.AddXml(TariffsInGroup.MappingXml.ToString());
      configuration.AddXml(TextNotification.MappingXml.ToString());
      configuration.AddXml(TicketPayment.MappingXml.ToString());
      configuration.AddXml(TicketPaymentsSessionInfo.MappingXml.ToString());
      configuration.AddXml(UserPlateMov.MappingXml.ToString());
      configuration.AddXml(UserPlateMovsSending.MappingXml.ToString());
      configuration.AddXml(UserPlate.MappingXml.ToString());
      configuration.AddXml(User.MappingXml.ToString());
      configuration.AddXml(UsersEmail.MappingXml.ToString());
      configuration.AddXml(UsersNotification.MappingXml.ToString());
      configuration.AddXml(UsersPushId.MappingXml.ToString());
      configuration.AddXml(UsersSmss.MappingXml.ToString());
      configuration.AddXml(AllOperationsExt.MappingXml.ToString());
      configuration.AddXml(Installation.MappingXml.ToString());
      configuration.AddXml(VwUser.MappingXml.ToString());
      configuration.AddXml(LicenseTerm.MappingXml.ToString());
      configuration.AddXml(LicenseTermsParam.MappingXml.ToString());
      configuration.AddXml(AllOperationsExtNorecharge.MappingXml.ToString());
      configuration.AddXml(AllCurrOperationsExt.MappingXml.ToString());
      configuration.AddXml(EmailtoolRecipient.MappingXml.ToString());
      configuration.AddXml(VwCustomerInvoice.MappingXml.ToString());
      configuration.AddXml(VwHisOperation.MappingXml.ToString());
      configuration.AddXml(VwTicketPayment.MappingXml.ToString());
      configuration.AddXml(VwCustomerPaymentMeansRecharge.MappingXml.ToString());
      configuration.AddXml(BalanceTransfer.MappingXml.ToString());
      configuration.AddXml(VwBalanceTransfer.MappingXml.ToString());
      configuration.AddXml(UsersFriend.MappingXml.ToString());
      configuration.AddXml(VwUsersFriend.MappingXml.ToString());
      configuration.AddXml(VwUsersPushId.MappingXml.ToString());
      configuration.AddXml(VwRechargeCoupon.MappingXml.ToString());
      configuration.AddXml(VwHisOperationsRestricted.MappingXml.ToString());
      configuration.AddAssembly(typeof(ConfigurationHelper).Assembly);
    }

    /// <summary>
    /// Provides global mappings not associated with a specific entity.
    /// </summary>
    public static System.Xml.Linq.XDocument ModelMappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ConfigurationHelper).Assembly.GetName().Name + @"'
                   namespace='integraMobile.Domain.NH.Entities'
                   >
</hibernate-mapping>");
        return mappingDocument;
      }
    }
  }
}
