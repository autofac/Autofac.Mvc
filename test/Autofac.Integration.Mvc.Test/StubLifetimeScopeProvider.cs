using System;
using Autofac.Core.Lifetime;

namespace Autofac.Integration.Mvc.Test
{
    public class StubLifetimeScopeProvider : ILifetimeScopeProvider
    {
        private readonly ILifetimeScope _container;

        private ILifetimeScope _lifetimeScope;

        public StubLifetimeScopeProvider(ILifetimeScope container)
        {
            this._container = container;
        }

        public ILifetimeScope ApplicationContainer
        {
            get { return this._container; }
        }

        public void EndLifetimeScope()
        {
            if (this._lifetimeScope != null)
            {
                this._lifetimeScope.Dispose();
            }
        }

        public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return this._lifetimeScope ?? (this._lifetimeScope = this.BuildLifetimeScope(configurationAction));
        }

        private ILifetimeScope BuildLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return (configurationAction == null)
                       ? this._container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
                       : this._container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, configurationAction);
        }
    }
}