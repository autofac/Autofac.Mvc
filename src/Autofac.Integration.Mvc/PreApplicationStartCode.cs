// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Container class for the ASP.NET application startup method.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class PreApplicationStartCode
{
    private static bool _startWasCalled;

    /// <summary>
    /// Performs ASP.NET application startup logic early in the pipeline.
    /// </summary>
    public static void Start()
    {
        // Guard against multiple calls. All Start calls are made on the same thread, so no lock needed here.
        if (_startWasCalled)
        {
            return;
        }

        _startWasCalled = true;
        DynamicModuleUtility.RegisterModule(typeof(RequestLifetimeHttpModule));
    }
}
