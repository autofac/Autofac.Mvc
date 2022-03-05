// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// Allows other filters to be overridden at the control and action level.
/// </summary>
internal class AutofacOverrideFilter : IOverrideFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutofacOverrideFilter"/> class.
    /// </summary>
    /// <param name="filtersToOverride">The filter type to override.</param>
    public AutofacOverrideFilter(Type filtersToOverride)
    {
        FiltersToOverride = filtersToOverride;
    }

    /// <inheritdoc/>
    public Type FiltersToOverride
    {
        get;
        private set;
    }
}
