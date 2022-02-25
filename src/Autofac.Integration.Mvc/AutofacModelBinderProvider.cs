// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Features.Metadata;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Autofac implementation of the <see cref="IModelBinderProvider"/> interface.
/// </summary>
public class AutofacModelBinderProvider : IModelBinderProvider
{
    /// <summary>
    /// Metadata key for the supported model types.
    /// </summary>
    internal const string MetadataKey = "SupportedModelTypes";

    /// <summary>
    /// Gets the model binder associated with the provided model type.
    /// </summary>
    /// <param name="modelType">Type of the model.</param>
    /// <returns>An <see cref="IModelBinder"/> instance if found; otherwise, <c>null</c>.</returns>
    public IModelBinder GetBinder(Type modelType)
    {
        var modelBinders = DependencyResolver.Current.GetServices<Meta<Lazy<IModelBinder>>>();

        var modelBinder = modelBinders
            .Where(binder => binder.Metadata.ContainsKey(MetadataKey))
            .FirstOrDefault(binder => ((List<Type>)binder.Metadata[MetadataKey]).Contains(modelType));
        return modelBinder?.Value.Value;
    }
}
