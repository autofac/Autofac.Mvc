// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// An override for an authorization filter.
/// </summary>
internal class AuthorizationFilterOverride : IAuthorizationFilter, IOverrideFilter
{
    private readonly IAuthorizationFilter _wrappedFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationFilterOverride"/> class.
    /// </summary>
    /// <param name="wrappedFilter">
    /// The filter to execute as the override.
    /// </param>
    public AuthorizationFilterOverride(IAuthorizationFilter wrappedFilter)
    {
        _wrappedFilter = wrappedFilter;
    }

    /// <inheritdoc/>
    public Type FiltersToOverride
    {
        get { return typeof(IAuthorizationFilter); }
    }

    /// <inheritdoc/>
    public void OnAuthorization(AuthorizationContext filterContext)
    {
        _wrappedFilter.OnAuthorization(filterContext);
    }
}
