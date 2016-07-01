// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// http://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Autofac.Integration.Mvc
{
    /// <summary>
    /// Autofac implementation of the <see cref="IDependencyResolver"/> interface.
    /// </summary>
    public class AutofacDependencyResolver : IDependencyResolver
    {
        private static Func<AutofacDependencyResolver> _resolverAccessor = DefaultResolverAccessor;

        private readonly Action<ContainerBuilder> _configurationAction;

        private readonly ILifetimeScope _container;

        private ILifetimeScopeProvider _lifetimeScopeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container that nested lifetime scopes will be create from.</param>
        public AutofacDependencyResolver(ILifetimeScope container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this._container = container;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container that nested lifetime scopes will be create from.</param>
        /// <param name="configurationAction">Action on a <see cref="ContainerBuilder"/>
        /// that adds component registations visible only in nested lifetime scopes.</param>
        public AutofacDependencyResolver(ILifetimeScope container, Action<ContainerBuilder> configurationAction)
            : this(container)
        {
            if (configurationAction == null)
            {
                throw new ArgumentNullException(nameof(configurationAction));
            }

            this._configurationAction = configurationAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container that nested lifetime scopes will be create from.</param>
        /// <param name="lifetimeScopeProvider">A <see cref="ILifetimeScopeProvider"/> implementation for
        /// creating new lifetime scopes.</param>
        public AutofacDependencyResolver(ILifetimeScope container, ILifetimeScopeProvider lifetimeScopeProvider) :
            this(container)
        {
            if (lifetimeScopeProvider == null)
            {
                throw new ArgumentNullException(nameof(lifetimeScopeProvider));
            }

            this._lifetimeScopeProvider = lifetimeScopeProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container that nested lifetime scopes will be create from.</param>
        /// <param name="lifetimeScopeProvider">A <see cref="ILifetimeScopeProvider"/> implementation for
        /// creating new lifetime scopes.</param>
        /// <param name="configurationAction">Action on a <see cref="ContainerBuilder"/>
        /// that adds component registations visible only in nested lifetime scopes.</param>
        public AutofacDependencyResolver(ILifetimeScope container, ILifetimeScopeProvider lifetimeScopeProvider, Action<ContainerBuilder> configurationAction)
            : this(container, lifetimeScopeProvider)
        {
            if (configurationAction == null)
            {
                throw new ArgumentNullException(nameof(configurationAction));
            }

            this._configurationAction = configurationAction;
        }

        /// <summary>
        /// Gets the Autofac implementation of the dependency resolver.
        /// </summary>
        public static AutofacDependencyResolver Current
        {
            get
            {
                return _resolverAccessor();
            }
        }

        /// <summary>
        /// Gets the application container that was provided to the constructor.
        /// </summary>
        public ILifetimeScope ApplicationContainer
        {
            get { return this._container; }
        }

        /// <summary>
        /// The lifetime containing components for processing the current HTTP request.
        /// </summary>
        public ILifetimeScope RequestLifetimeScope
        {
            get
            {
                if (this._lifetimeScopeProvider == null)
                {
                    this._lifetimeScopeProvider = new RequestLifetimeScopeProvider(this._container);
                }
                return this._lifetimeScopeProvider.GetLifetimeScope(this._configurationAction);
            }
        }

        /// <summary>
        /// Sets the mechanism used to locate the current Autofac dependency resolver.
        /// </summary>
        /// <param name="accessor">
        /// A <see cref="Func{T}"/> that returns an <see cref="AutofacDependencyResolver"/>
        /// based on the current context. Set this to <see langword="null" /> to return to the
        /// default behavior.
        /// </param>
        public static void SetAutofacDependencyResolverAccessor(Func<AutofacDependencyResolver> accessor)
        {
            if (accessor == null)
            {
                _resolverAccessor = DefaultResolverAccessor;
            }
            else
            {
                _resolverAccessor = accessor;
            }
        }

        /// <summary>
        /// Get a single instance of a service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The single instance if resolved; otherwise, <c>null</c>.</returns>
        public virtual object GetService(Type serviceType)
        {
            return this.RequestLifetimeScope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Gets all available instances of a services.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The list of instances if any were resolved; otherwise, an empty list.</returns>
        public virtual IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = this.RequestLifetimeScope.Resolve(enumerableServiceType);
            return (IEnumerable<object>)instance;
        }

        /// <summary>
        /// Default mechanism for locating the current Autofac service resolver.
        /// </summary>
        /// <returns>
        /// The current <see cref="AutofacDependencyResolver"/> if it can be located.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the current resolver can't be found.
        /// </exception>
        private static AutofacDependencyResolver DefaultResolverAccessor()
        {
            var currentResolver = DependencyResolver.Current;
            var autofacResolver = currentResolver as AutofacDependencyResolver;
            if (autofacResolver != null)
            {
                return autofacResolver;
            }

            // Issue 351: We can't necessarily cast the current dependency resolver
            // to AutofacDependencyResolver because diagnostic systems like Glimpse
            // will wrap/proxy the resolver. Here we check to see if the resolver
            // has been wrapped with DynamicProxy and unwrap the target if we can.

            var targetType = currentResolver.GetType().GetField("__target");
            if (targetType != null && targetType.FieldType == typeof(AutofacDependencyResolver))
            {
                return (AutofacDependencyResolver)targetType.GetValue(currentResolver);
            }

            throw new InvalidOperationException(string.Format(
                CultureInfo.CurrentCulture,
                AutofacDependencyResolverResources.AutofacDependencyResolverNotFound,
                    currentResolver.GetType().FullName, typeof(AutofacDependencyResolver).FullName));
        }
    }
}
