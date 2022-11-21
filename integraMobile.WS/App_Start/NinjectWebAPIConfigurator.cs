using Ninject;
using Ninject.Web.Common;
using integraMobile.WS.WebAPI.Services.Interfaces;
using integraMobile.WS.WebAPI.Services;
using integraMobile.Domain.Abstract;

namespace integraMobile.WS.App_Start
{
    public class NinjectWebAPIConfigurator
    {
        #region Private members

        private IKernel _container;

        #endregion

        #region Constructor

        public void Configure(IKernel container)
        {
            _container = container;

            AddBindingServices();
        }

        #endregion

        #region Private methods


        private void AddBindingServices()
        {
            _container.Bind<IMeyparService>().To<MeyparService>()
                 .WithConstructorArgument("oCustomersRepository", _container.Get<ICustomersRepository>())
                 .WithConstructorArgument("oInfraestructureRepository", _container.Get<IInfraestructureRepository>())
                 .WithConstructorArgument("oGeograficAndTariffsRepository", _container.Get<IGeograficAndTariffsRepository>())
                 .WithConstructorArgument("oBackOfficeRepository", _container.Get<IBackOfficeRepository>())
                 .WithConstructorArgument("oThirdPartyOffstreet", _container.Get<integraMobile.ExternalWS.ThirdPartyOffstreet>());


            _container.Bind<integraMobile.ExternalWS.ThirdPartyOffstreet>().To<integraMobile.ExternalWS.ThirdPartyOffstreet>()
                 .WithConstructorArgument("oCustomersRepository", _container.Get<ICustomersRepository>())
                 .WithConstructorArgument("oInfraestructureRepository", _container.Get<IInfraestructureRepository>())
                 .WithConstructorArgument("oGeograficAndTariffsRepository", _container.Get<IGeograficAndTariffsRepository>())
                 .WithConstructorArgument("oBackOfficeRepository", _container.Get<IBackOfficeRepository>());

        }

        #endregion
    }
}