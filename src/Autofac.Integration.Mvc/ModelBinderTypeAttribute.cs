// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc;

/// <summary>
/// Indicates what types a model binder supports.
/// </summary>
[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class ModelBinderTypeAttribute : Attribute
{
    /// <summary>
    /// Gets the target types.
    /// </summary>
    public IEnumerable<Type> TargetTypes { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelBinderTypeAttribute"/> class.
    /// </summary>
    /// <param name="targetTypes">The target types.</param>
    public ModelBinderTypeAttribute(params Type[] targetTypes)
    {
        TargetTypes = targetTypes ?? throw new ArgumentNullException(nameof(targetTypes));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelBinderTypeAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The target type.</param>
    public ModelBinderTypeAttribute(Type targetType)
    {
        if (targetType == null)
        {
            throw new ArgumentNullException(nameof(targetType));
        }

        TargetTypes = new Type[] { targetType };
    }
}
