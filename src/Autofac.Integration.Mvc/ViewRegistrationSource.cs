﻿// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Integration.Mvc;

/// <summary>
/// A registration source for building view registrations.
/// </summary>
/// <remarks>
/// Supports view registrations for <see cref="WebViewPage"/>, <see cref="ViewPage"/>,
/// <see cref="ViewMasterPage"/> and <see cref="ViewUserControl"/> derived types.
/// </remarks>
public class ViewRegistrationSource : IRegistrationSource
{
    /// <summary>
    /// Retrieve registrations for an unregistered service, to be used
    /// by the container.
    /// </summary>
    /// <param name="service">The service that was requested.</param>
    /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
    /// <returns>Registrations providing the service.</returns>
    public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
    {
        if (service is IServiceWithType typedService && IsSupportedView(typedService.ServiceType))
        {
            yield return RegistrationBuilder.ForType(typedService.ServiceType)
                .PropertiesAutowired()
                .InstancePerDependency()
                .CreateRegistration();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the registrations provided by this source are 1:1 adapters on top
    /// of other components (I.e. like Meta, Func or Owned.)
    /// </summary>
    public bool IsAdapterForIndividualComponents
    {
        get { return false; }
    }

    private static bool IsSupportedView(Type serviceType)
    {
        return serviceType.IsAssignableTo<WebViewPage>()
            || serviceType.IsAssignableTo<ViewPage>()
            || serviceType.IsAssignableTo<ViewMasterPage>()
            || serviceType.IsAssignableTo<ViewUserControl>();
    }
}
