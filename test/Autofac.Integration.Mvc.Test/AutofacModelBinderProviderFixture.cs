// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test;

public class AutofacModelBinderProviderFixture
{
    [Fact]
    public void ModelBinderHasDependenciesInjected()
    {
        using var httpRequestScope = BuildContainer().BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
        var modelBinder = httpRequestScope.Resolve<IEnumerable<IModelBinder>>()
            .OfType<ModelBinder>()
            .FirstOrDefault();
        Assert.NotNull(modelBinder);
        Assert.NotNull(modelBinder.Dependency);
    }

    [Fact]
    public void ModelBindersAreRegistered()
    {
        using var httpRequestScope = BuildContainer().BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
        var modelBinders = httpRequestScope.Resolve<IEnumerable<IModelBinder>>();
        Assert.Single(modelBinders);
    }

    [Fact]
    public void MultipleTypesCanBeDeclaredWithMultipleAttribute()
    {
        BuildContainer();
        var provider = (AutofacModelBinderProvider)DependencyResolver.Current.GetService<IModelBinderProvider>();
        Assert.IsType<ModelBinder>(provider.GetBinder(typeof(string)));
        Assert.IsType<ModelBinder>(provider.GetBinder(typeof(DateTime)));
    }

    [Fact]
    public void MultipleTypesCanBeDeclaredWithSingleAttribute()
    {
        BuildContainer();
        var provider = (AutofacModelBinderProvider)DependencyResolver.Current.GetService<IModelBinderProvider>();
        Assert.IsType<ModelBinder>(provider.GetBinder(typeof(TestModel1)));
        Assert.IsType<ModelBinder>(provider.GetBinder(typeof(string)));
    }

    [Fact]
    public void ProviderIsRegisteredAsSingleInstance()
    {
        var container = BuildContainer();
        var modelBinderProvider = container.Resolve<IModelBinderProvider>();
        Assert.IsType<AutofacModelBinderProvider>(modelBinderProvider);

        using var httpRequestScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
        Assert.Equal(modelBinderProvider, httpRequestScope.Resolve<IModelBinderProvider>());
    }

    [Fact]
    public void ReturnsNullWhenModelBinderRegisteredWithoutMetadata()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<ModelBinderWithoutAttribute>().As<IModelBinder>().InstancePerRequest();
        builder.RegisterModelBinderProvider();
        var container = builder.Build();

        using var httpRequestScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
        var modelBinders = httpRequestScope.Resolve<IEnumerable<IModelBinder>>().ToList();
        Assert.Single(modelBinders);
        Assert.IsType<ModelBinderWithoutAttribute>(modelBinders.First());

        var provider = (AutofacModelBinderProvider)httpRequestScope.Resolve<IModelBinderProvider>();
        Assert.Null(provider.GetBinder(typeof(object)));
    }

    private static ILifetimeScope BuildContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Dependency>().AsSelf();
        builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
        builder.RegisterModelBinderProvider();

        var container = builder.Build();
        var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
        DependencyResolver.SetResolver(new AutofacDependencyResolver(container, lifetimeScopeProvider));
        return container;
    }
}
