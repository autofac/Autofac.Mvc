using System;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class RequestLifetimeScopeProviderFixture
    {
        [Fact]
        public void ContainerMustBeProvided()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new RequestLifetimeScopeProvider(null));
            Assert.Equal("container", exception.ParamName);
        }

        [Fact]
        public void MeaningfulExceptionThrowWhenHttpContextNotAvailable()
        {
            var container = new ContainerBuilder().Build();
            var provider = new RequestLifetimeScopeProvider(container);
            var exception = Assert.Throws<InvalidOperationException>(() => provider.GetLifetimeScope(b => { }));
            Assert.Equal(RequestLifetimeScopeProviderResources.HttpContextNotAvailable, exception.Message);
        }

        [Fact]
        public void ProviderRegisteredWithHttpModule()
        {
            var container = new ContainerBuilder().Build();
            var provider = new RequestLifetimeScopeProvider(container);
            Assert.Equal(provider, RequestLifetimeHttpModule.LifetimeScopeProvider);
        }
    }
}
