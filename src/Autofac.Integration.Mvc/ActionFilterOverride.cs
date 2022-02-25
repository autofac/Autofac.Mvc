// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// An override for an action filter.
/// </summary>
internal class ActionFilterOverride : IActionFilter, IOverrideFilter
{
    private readonly IActionFilter _wrappedFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionFilterOverride"/> class.
    /// </summary>
    /// <param name="wrappedFilter">
    /// The filter to execute as the override.
    /// </param>
    public ActionFilterOverride(IActionFilter wrappedFilter)
    {
        _wrappedFilter = wrappedFilter;
    }

    /// <inheritdoc/>
    public Type FiltersToOverride
    {
        get { return typeof(IActionFilter); }
    }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
        _wrappedFilter.OnActionExecuted(filterContext);
    }

    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        _wrappedFilter.OnActionExecuting(filterContext);
    }
}
