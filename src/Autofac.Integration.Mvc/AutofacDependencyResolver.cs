// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;

namespace Autofac.Integration.Mvc;

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
        _container = container ?? throw new ArgumentNullException(nameof(container));
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
        _configurationAction = configurationAction ?? throw new ArgumentNullException(nameof(configurationAction));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutofacDependencyResolver"/> class.
    /// </summary>
    /// <param name="container">The container that nested lifetime scopes will be create from.</param>
    /// <param name="lifetimeScopeProvider">A <see cref="ILifetimeScopeProvider"/> implementation for
    /// creating new lifetime scopes.</param>
    public AutofacDependencyResolver(ILifetimeScope container, ILifetimeScopeProvider lifetimeScopeProvider)
        : this(container)
    {
        _lifetimeScopeProvider = lifetimeScopeProvider ?? throw new ArgumentNullException(nameof(lifetimeScopeProvider));
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
        _configurationAction = configurationAction ?? throw new ArgumentNullException(nameof(configurationAction));
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
        get { return _container; }
    }

    /// <summary>
    /// Gets the lifetime containing components for processing the current HTTP request.
    /// </summary>
    public ILifetimeScope RequestLifetimeScope
    {
        get
        {
            if (_lifetimeScopeProvider == null)
            {
                _lifetimeScopeProvider = new RequestLifetimeScopeProvider(_container);
            }

            return _lifetimeScopeProvider.GetLifetimeScope(_configurationAction);
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
        _resolverAccessor = accessor ?? DefaultResolverAccessor;
    }

    /// <summary>
    /// Get a single instance of a service.
    /// </summary>
    /// <param name="serviceType">Type of the service.</param>
    /// <returns>The single instance if resolved; otherwise, <c>null</c>.</returns>
    public virtual object GetService(Type serviceType)
    {
        return RequestLifetimeScope.ResolveOptional(serviceType);
    }

    /// <summary>
    /// Gets all available instances of a services.
    /// </summary>
    /// <param name="serviceType">Type of the service.</param>
    /// <returns>The list of instances if any were resolved; otherwise, an empty list.</returns>
    public virtual IEnumerable<object> GetServices(Type serviceType)
    {
        var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
        var instance = RequestLifetimeScope.Resolve(enumerableServiceType);
        return (IEnumerable<object>)instance;
    }

    /// <summary>
    /// Default mechanism for locating the current Autofac service resolver.
    /// </summary>
    /// <returns>
    /// The current <see cref="AutofacDependencyResolver"/> if it can be located.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the current resolver can't be found.
    /// </exception>
    private static AutofacDependencyResolver DefaultResolverAccessor()
    {
        var currentResolver = DependencyResolver.Current;
        if (currentResolver is AutofacDependencyResolver autofacResolver)
        {
            return autofacResolver;
        }

        // Issue 351: We can't necessarily cast the current dependency resolver
        // to AutofacDependencyResolver because diagnostic systems like Glimpse
        // will wrap/proxy the resolver. Here we check to see if the resolver
        // has been wrapped with DynamicProxy and unwrap the target if we can.
        var targetType = currentResolver.GetType().GetField("__target");
        return targetType != null && targetType.FieldType == typeof(AutofacDependencyResolver)
            ? (AutofacDependencyResolver)targetType.GetValue(currentResolver)
            : throw new InvalidOperationException(string.Format(
                CultureInfo.CurrentCulture,
                AutofacDependencyResolverResources.AutofacDependencyResolverNotFound,
                currentResolver.GetType().FullName,
                typeof(AutofacDependencyResolver).FullName));
    }
}
