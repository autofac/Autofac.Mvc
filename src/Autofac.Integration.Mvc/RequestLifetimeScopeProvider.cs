// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using System.Web;
using Autofac.Core.Lifetime;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Creates and disposes HTTP request based lifetime scopes.
/// </summary>
/// <remarks>
/// The provider is notified when a HTTP request ends by the <see cref="RequestLifetimeHttpModule"/>.
/// </remarks>
public class RequestLifetimeScopeProvider : ILifetimeScopeProvider
{
    private readonly ILifetimeScope _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestLifetimeScopeProvider"/> class.
    /// </summary>
    /// <param name="container">The parent container, usually the application container.</param>
    public RequestLifetimeScopeProvider(ILifetimeScope container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        RequestLifetimeHttpModule.SetLifetimeScopeProvider(this);
    }

    /// <summary>
    /// Gets the global, application-wide container.
    /// </summary>
    public ILifetimeScope ApplicationContainer
    {
        get { return _container; }
    }

    private static ILifetimeScope LifetimeScope
    {
        get { return (ILifetimeScope)HttpContext.Current.Items[typeof(ILifetimeScope)]; }
        set { HttpContext.Current.Items[typeof(ILifetimeScope)] = value; }
    }

    /// <summary>
    /// Gets a nested lifetime scope that services can be resolved from.
    /// </summary>
    /// <param name="configurationAction">
    /// A configuration action that will execute during lifetime scope creation.
    /// </param>
    /// <returns>A new or existing nested lifetime scope.</returns>
    public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
    {
        if (HttpContext.Current == null)
        {
            throw new InvalidOperationException(RequestLifetimeScopeProviderResources.HttpContextNotAvailable);
        }

        if (LifetimeScope == null)
        {
            if ((LifetimeScope = GetLifetimeScopeCore(configurationAction)) == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, RequestLifetimeScopeProviderResources.NullLifetimeScopeReturned, GetType().FullName));
            }
        }

        return LifetimeScope;
    }

    /// <summary>
    /// Ends the current HTTP request lifetime scope.
    /// </summary>
    public void EndLifetimeScope()
    {
        if (HttpContext.Current == null)
        {
            return;
        }

        var lifetimeScope = LifetimeScope;
        if (lifetimeScope != null)
        {
            lifetimeScope.Dispose();
        }
    }

    /// <summary>
    /// Gets a lifetime scope for the current HTTP request. This method can be overridden
    /// to alter the way that the life time scope is constructed.
    /// </summary>
    /// <param name="configurationAction">
    /// A configuration action that will execute during lifetime scope creation.
    /// </param>
    /// <returns>A new lifetime scope for the current HTTP request.</returns>
    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    protected virtual ILifetimeScope GetLifetimeScopeCore(Action<ContainerBuilder> configurationAction)
    {
        return (configurationAction == null)
                   ? ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
                   : ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, configurationAction);
    }
}
