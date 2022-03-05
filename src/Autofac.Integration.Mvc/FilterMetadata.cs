// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel;
using System.Reflection;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Metadata interface for filter registrations.
/// </summary>
internal class FilterMetadata
{
    /// <summary>
    /// Gets or sets the type of the controller.
    /// </summary>
    [DefaultValue(null)]
    public Type ControllerType { get; set; }

    /// <summary>
    /// Gets or sets the filter scope.
    /// </summary>
    [DefaultValue(FilterScope.First)]
    public FilterScope FilterScope { get; set; }

    /// <summary>
    /// Gets or sets the method info.
    /// </summary>
    [DefaultValue(null)]
    public MethodInfo MethodInfo { get; set; }

    /// <summary>
    /// Gets or sets the order in which the filter is applied.
    /// </summary>
    [DefaultValue(-1)]
    public int Order { get; set; }
}
