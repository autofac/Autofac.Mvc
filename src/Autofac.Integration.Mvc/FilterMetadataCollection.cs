// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// Holds the set of <see cref="FilterMetadata"/> registrations associated with a
/// single filter component under a given metadata key.
/// </summary>
/// <remarks>
/// <para>
/// A single filter registration can be targeted at more than one controller or
/// action in one fluent statement (for example, chaining multiple
/// <c>AsActionFilterFor</c> calls). Each of those calls contributes a separate
/// <see cref="FilterMetadata"/> entry describing the controller/action it applies
/// to. Storing them in this collection - rather than overwriting a single metadata
/// value - is what allows those chained registrations to coexist.
/// </para>
/// </remarks>
internal class FilterMetadataCollection
{
    /// <summary>
    /// Gets the set of filter registrations for the associated metadata key.
    /// </summary>
    public List<FilterMetadata> Filters { get; } = new List<FilterMetadata>();
}
