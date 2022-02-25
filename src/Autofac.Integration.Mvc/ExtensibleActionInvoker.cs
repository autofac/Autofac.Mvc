// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Core;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Injects services from the container into the ASP.NET MVC invocation pipeline.
/// This is a Async Controller Action Invoker which can be used for both async and non-async scenarios.
/// </summary>
/// <remarks>
/// <para>
/// Action methods can include parameters that will be resolved from the
/// container, along with regular parameters.
/// </para>
/// </remarks>
public class ExtensibleActionInvoker : System.Web.Mvc.Async.AsyncControllerActionInvoker
{
    /// <summary>
    /// If set, this is used to determine which model properties are injected.
    /// </summary>
    private readonly IPropertySelector _propertySelector;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtensibleActionInvoker"/> class.
    /// </summary>
    public ExtensibleActionInvoker()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtensibleActionInvoker"/> class.
    /// </summary>
    /// <param name="propertySelector">The inject property selector.</param>
    public ExtensibleActionInvoker(IPropertySelector propertySelector)
    {
        _propertySelector = propertySelector;
    }

    /// <summary>
    /// Gets the parameter value.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param><param name="parameterDescriptor">The parameter descriptor.</param>
    /// <returns>
    /// The parameter value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="parameterDescriptor" /> is <see langword="null" />.
    /// </exception>
    protected override object GetParameterValue(ControllerContext controllerContext, ParameterDescriptor parameterDescriptor)
    {
        if (parameterDescriptor == null)
        {
            throw new ArgumentNullException(nameof(parameterDescriptor));
        }

        // Issue #430
        // Model binding to collections (specifically IEnumerable<HttpPostedFileBase>
        // as used in multiple file upload scenarios) breaks if you try to
        // resolve before allowing default model binding to give it a shot.
        // You also can't send in an object that needs to be model bound if
        // it's registered in the container because the container will ignore
        // the POSTed in values.
        //
        // Issue #368
        // The original solution to issue #368 was to fall back to default
        // model binding if the ExtensibleActionInvoker was unable to resolve
        // a parameter AND if parameter injection was enabled. You can no longer
        // disable parameter injection, and it turns out for issue #430 that
        // we need to try model binding first. Unfortunately there's no way
        // to determine if default model binding will fail, so we give it
        // a shot and handle what we can.
        var value = (object)null;
        try
        {
            value = base.GetParameterValue(controllerContext, parameterDescriptor);
        }
        catch (MissingMethodException)
        {
            // Don't do anything - this means the default model binder couldn't
            // activate a new instance (like if it's an interface) or figure
            // out some other way to model bind it.
        }

        var context = AutofacDependencyResolver.Current.RequestLifetimeScope;
        if (value == null)
        {
            // We got nothing from the default model binding, so try to
            // resolve it.
            value = context.ResolveOptional(parameterDescriptor.ParameterType);
        }

        if (_propertySelector != null && value != null)
        {
            context.InjectProperties(value, _propertySelector);
        }

        return value;
    }
}
