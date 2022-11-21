using VehicleType.WS.Domain.Abstract;
using VehicleType.WS.Domain.Concrete;
using Ninject;

namespace VehicleType.WS.App_Start
{
    public class NinjectConfigurator
    {
        #region Private members

        private IKernel _container;

        #endregion

        #region Constructor

        public void Configure(IKernel container)
        {
            _container = container;

            AddBindingRepositories();            
        }

        #endregion

        #region Private methods

        private void AddBindingRepositories()
        {
            NHSessionManager.ConnectionConfiguration oConnectionConfig = null;

            _container.Bind<IVehicleTypeRepository>().To<OracleVehicleTypeRepository>().WithConstructorArgument("oConnectionConfig", oConnectionConfig).WithConstructorArgument("bOpenSession", false);

        }

        #endregion
    }
}