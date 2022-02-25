// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web;

namespace Autofac.Integration.Mvc;

/// <summary>
/// An <see cref="IHttpModule"/> and <see cref="ILifetimeScopeProvider"/> implementation
/// that creates a nested lifetime scope for each HTTP request.
/// </summary>
internal class RequestLifetimeHttpModule : IHttpModule
{
    /// <summary>
    /// Gets the lifetime scope provider that should be notified when a HTTP request ends.
    /// </summary>
    internal static ILifetimeScopeProvider LifetimeScopeProvider { get; private set; }

    /// <summary>
    /// Initializes a module and prepares it to handle requests.
    /// </summary>
    /// <param name="context">An <see cref="HttpApplication"/> that provides access to the
    /// methods, properties, and events common to all application objects within an ASP.NET application.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="context" /> is <see langword="null" />.
    /// </exception>
    public void Init(HttpApplication context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        context.EndRequest += OnEndRequest;
    }

    /// <summary>
    /// Disposes of the resources (other than memory) used by the module that implements <see cref="IHttpModule"/>.
    /// </summary>
    public void Dispose()
    {
    }

    /// <summary>
    /// Sets the global lifetime scope provider.
    /// </summary>
    /// <param name="lifetimeScopeProvider">
    /// The <see cref="ILifetimeScopeProvider"/> that manages the ambient request lifetime scope.
    /// </param>
    public static void SetLifetimeScopeProvider(ILifetimeScopeProvider lifetimeScopeProvider)
    {
        LifetimeScopeProvider = lifetimeScopeProvider ?? throw new ArgumentNullException(nameof(lifetimeScopeProvider));
    }

    private static void OnEndRequest(object sender, EventArgs e)
    {
        if (LifetimeScopeProvider != null)
        {
            LifetimeScopeProvider.EndLifetimeScope();
        }
    }
}
