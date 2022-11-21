using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace $rootnamespace$
{
    public class DependencyScope : IDependencyScope
    {
        private readonly IContainer container;
        public DependencyScope(IContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return serviceType.IsAbstract || serviceType.IsInterface ? container.TryGetInstance(serviceType)
                : container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType).Cast<object>();
        }
    

        public void Dispose()
        {
            if (container != null)
                container.Dispose();
        }
    }

    public class DependencyResolver : DependencyScope, IDependencyResolver
    {
        private readonly IContainer container;
        public DependencyResolver(IContainer container) :
            base(container.GetNestedContainer())
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            // Create a Nested Container.So that we can dispose the container once the request is completed.
            var NestedContainer = this.container.GetNestedContainer();
            return new DependencyScope(NestedContainer);
        }
    }
}