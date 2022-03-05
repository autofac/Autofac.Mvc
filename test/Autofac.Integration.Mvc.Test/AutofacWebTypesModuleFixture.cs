// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web;
using System.Web.Hosting;
using System.Web.Routing;

namespace Autofac.Integration.Mvc.Test;

public class AutofacWebTypesModuleFixture
{
    [Theory]
    [InlineData(typeof(HttpContextBase))]
    [InlineData(typeof(HttpRequestBase))]
    [InlineData(typeof(HttpResponseBase))]
    [InlineData(typeof(HttpServerUtilityBase))]
    [InlineData(typeof(HttpSessionStateBase))]
    [InlineData(typeof(HttpApplicationStateBase))]
    [InlineData(typeof(HttpBrowserCapabilitiesBase))]
    [InlineData(typeof(HttpFileCollectionBase))]
    [InlineData(typeof(RequestContext))]
    [InlineData(typeof(HttpCachePolicyBase))]
    [InlineData(typeof(VirtualPathProvider))]
    [InlineData(typeof(UrlHelper))]
    public void EnsureWebTypeIsRegistered(Type serviceType)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new AutofacWebTypesModule());
        var container = builder.Build();
        Assert.True(container.IsRegistered(serviceType));
    }
}
