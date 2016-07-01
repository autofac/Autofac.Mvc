using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
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
}
