// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// An override for a result filter.
/// </summary>
internal class ResultFilterOverride : IResultFilter, IOverrideFilter
{
    private readonly IResultFilter _wrappedFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultFilterOverride"/> class.
    /// </summary>
    /// <param name="wrappedFilter">
    /// The filter to execute as the override.
    /// </param>
    public ResultFilterOverride(IResultFilter wrappedFilter)
    {
        _wrappedFilter = wrappedFilter;
    }

    /// <inheritdoc/>
    public Type FiltersToOverride
    {
        get { return typeof(IResultFilter); }
    }

    /// <inheritdoc/>
    public void OnResultExecuted(ResultExecutedContext filterContext)
    {
        _wrappedFilter.OnResultExecuted(filterContext);
    }

    /// <inheritdoc/>
    public void OnResultExecuting(ResultExecutingContext filterContext)
    {
        _wrappedFilter.OnResultExecuting(filterContext);
    }
}
