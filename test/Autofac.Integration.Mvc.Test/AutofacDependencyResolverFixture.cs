// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test;

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
        static void ConfigurationAction(ContainerBuilder builder) => builder.Register(c => new object());
        var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
        var resolver = new AutofacDependencyResolver(container, lifetimeScopeProvider, ConfigurationAction);

        var service = resolver.GetService(typeof(object));
        var services = resolver.GetServices(typeof(object));

        Assert.NotNull(service);
        Assert.Single(services);
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
        Assert.Single(resolver.GetServices(typeof(object)));
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

        Assert.Empty(services);
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

        Assert.Single(services);
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
        public DerivedAutofacDependencyResolver(IContainer container)
            : base(container)
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
