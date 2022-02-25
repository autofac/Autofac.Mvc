// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// An override for an authentication filter.
/// </summary>
internal class AuthenticationFilterOverride : IAuthenticationFilter, IOverrideFilter
{
    private readonly IAuthenticationFilter _wrappedFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationFilterOverride"/> class.
    /// </summary>
    /// <param name="wrappedFilter">
    /// The filter to execute as the override.
    /// </param>
    public AuthenticationFilterOverride(IAuthenticationFilter wrappedFilter)
    {
        _wrappedFilter = wrappedFilter;
    }

    /// <inheritdoc/>
    public Type FiltersToOverride
    {
        get { return typeof(IAuthenticationFilter); }
    }

    /// <inheritdoc/>
    public void OnAuthentication(AuthenticationContext filterContext)
    {
        _wrappedFilter.OnAuthentication(filterContext);
    }

    /// <inheritdoc/>
    public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    {
        _wrappedFilter.OnAuthenticationChallenge(filterContext);
    }
}
