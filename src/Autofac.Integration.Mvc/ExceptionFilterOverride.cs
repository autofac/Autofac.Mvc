// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// An override for an exception filter.
/// </summary>
internal class ExceptionFilterOverride : IExceptionFilter, IOverrideFilter
{
    private readonly IExceptionFilter _wrappedFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionFilterOverride"/> class.
    /// </summary>
    /// <param name="wrappedFilter">
    /// The filter to execute as the override.
    /// </param>
    public ExceptionFilterOverride(IExceptionFilter wrappedFilter)
    {
        _wrappedFilter = wrappedFilter;
    }

    /// <inheritdoc/>
    public Type FiltersToOverride
    {
        get { return typeof(IExceptionFilter); }
    }

    /// <inheritdoc/>
    public void OnException(ExceptionContext filterContext)
    {
        _wrappedFilter.OnException(filterContext);
    }
}
