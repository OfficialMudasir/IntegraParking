using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using integraMobile.Domain.Abstract;
using System.Collections.Generic;

namespace integraMobile.WS.Test
{
    [TestClass]
    public class UnitTest1
    {
        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }
        [Inject]
        public IGeograficAndTariffsRepository geograficAndTariffsRepository { get; set; }
        [Inject]
        public IBackOfficeRepository backOfficeRepository { get; set; }

        [TestMethod]
        public void TestMethodAddServicesEmpty()
        {

            integraMobileWS oIntegraMobileWS = New  Start(countriesRedirection);
            //listNameParameter.Add(ConstantsEntity.PARAMETER_U);
            //listNameParameter.Add(ConstantsEntity.PARAMETER_SESSION_ID);
            //listNameParameter.Add(ConstantsEntity.PARAMETER_LICENSE);
            //listNameParameter.Add(ConstantsEntity.PARAMETER_CITY_ID);
            //listNameParameter.Add(ConstantsEntity.PARAMETER_ID_SERVICE_TYPE);
            //listNameParameter.Add(ConstantsEntity.PARAMETER_TYPE_OF_SERVICE_TYPE);
        }

        
    }
}
