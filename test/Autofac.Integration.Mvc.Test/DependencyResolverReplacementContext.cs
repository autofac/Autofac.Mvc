using System;
using System.Web.Mvc;

namespace Autofac.Integration.Mvc.Test
{
    public class DependencyResolverReplacementContext : IDisposable
    {
        private IDependencyResolver _originalResolver;

        public DependencyResolverReplacementContext()
        {
            this._originalResolver = DependencyResolver.Current;

        }

        public virtual void Dispose()
        {
            DependencyResolver.SetResolver(this._originalResolver);
        }
    }
}
