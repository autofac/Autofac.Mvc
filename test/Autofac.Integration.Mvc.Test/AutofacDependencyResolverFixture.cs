// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// https://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class AutofacDependencyResolverFixture : IClassFixture<DependencyResolverReplacementContext>
    {
        [Fact]
        public void ApplicationContainerExposed()
        {
            var container = new ContainerBuilder().Build();
            var dependencyResolver = new AutofacDependencyResolver(container);

            Assert.Equal(container, dependencyResolver.ApplicationContainer);
        }

        [Fact]
        public void ConfigurationActionInvokedForNestedLifetime()
        {
            var container = new ContainerBuilder().Build();
            Action<ContainerBuilder> configurationAction = builder => builder.Register(c => new object());
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider, configurationAction);

            var service = resolver.GetService(typeof(object));
            var services = resolver.GetServices(typeof(object));

            Assert.NotNull(service);
            Assert.Equal(1, services.Count());
        }

        [Fact]
        public void CurrentPropertyExposesTheCorrectResolver()
        {
            var container = new ContainerBuilder().Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);

            DependencyResolver.SetResolver(resolver);

            Assert.Equal(DependencyResolver.Current, AutofacDependencyResolver.Current);
        }

        [Fact]
        public void DerivedResolverTypesCanStillBeCurrentResolver()
        {
            var container = new ContainerBuilder().Build();
            var resolver = new DerivedAutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
            Assert.Equal(resolver, AutofacDependencyResolver.Current);
            Assert.NotNull(resolver.GetService(typeof(object)));
            Assert.Equal(1, resolver.GetServices(typeof(object)).Count());
        }

        [Fact]
        public void GetServiceReturnsNullForUnregisteredService()
        {
            var container = new ContainerBuilder().Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);

            var service = resolver.GetService(typeof(object));

            Assert.Null(service);
        }

        [Fact]
        public void GetServiceReturnsRegisteredService()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new object());
            var container = builder.Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);

            var service = resolver.GetService(typeof(object));

            Assert.NotNull(service);
        }

        [Fact]
        public void GetServicesReturnsEmptyEnumerableForUnregisteredService()
        {
            var container = new ContainerBuilder().Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);

            var services = resolver.GetServices(typeof(object));

            Assert.Equal(0, services.Count());
        }

        [Fact]
        public void GetServicesReturnsRegisteredService()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new object());
            var container = builder.Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);

            var services = resolver.GetServices(typeof(object));

            Assert.Equal(1, services.Count());
        }

        [Fact]
        public void NestedLifetimeScopeIsCreated()
        {
            var container = new ContainerBuilder().Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider);

            Assert.NotNull(resolver.RequestLifetimeScope);
        }

        [Fact]
        public void NullConfigurationActionThrowsException()
        {
            var container = new ContainerBuilder().Build();

            var exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(container, (Action<ContainerBuilder>)null));
            Assert.Equal("configurationAction", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(container, new Mock<ILifetimeScopeProvider>().Object, null));
            Assert.Equal("configurationAction", exception.ParamName);
        }

        [Fact]
        public void NullContainerThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(null));
            Assert.Equal("container", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(null, cb => { }));
            Assert.Equal("container", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(null, new Mock<ILifetimeScopeProvider>().Object));
            Assert.Equal("container", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(null, new Mock<ILifetimeScopeProvider>().Object, cb => { }));
            Assert.Equal("container", exception.ParamName);
        }

        [Fact]
        public void NullLifetimeScopeProviderThrowsException()
        {
            var container = new ContainerBuilder().Build();

            var exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(container, (ILifetimeScopeProvider)null));
            Assert.Equal("lifetimeScopeProvider", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(
                () => new AutofacDependencyResolver(container, null, cb => { }));
            Assert.Equal("lifetimeScopeProvider", exception.ParamName);
        }

        private class DerivedAutofacDependencyResolver : AutofacDependencyResolver
        {
            public DerivedAutofacDependencyResolver(IContainer container) : base(container)
            {
            }

            public override object GetService(Type serviceType)
            {
                return serviceType == typeof(object) ? new object() : base.GetService(serviceType);
            }

            public override IEnumerable<object> GetServices(Type serviceType)
            {
                return serviceType == typeof(object) ? new[] { new object() } : base.GetServices(serviceType);
            }
        }
    }
}
