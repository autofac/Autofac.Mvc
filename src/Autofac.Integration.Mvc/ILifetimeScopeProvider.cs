// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// Implementors are able to control the creation of nested lifetime scopes.
/// </summary>
public interface ILifetimeScopeProvider
{
    /// <summary>
    /// Gets a nested lifetime scope that services can be resolved from.
    /// </summary>
    /// <param name="configurationAction">
    /// A configuration action that will execute during lifetime scope creation.
    /// </param>
    /// <returns>A new or existing nested lifetime scope.</returns>
    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction);

    /// <summary>
    /// Ends the current lifetime scope.
    /// </summary>
    void EndLifetimeScope();

    /// <summary>
    /// Gets the global, application-wide container.
    /// </summary>
    ILifetimeScope ApplicationContainer { get; }
}
