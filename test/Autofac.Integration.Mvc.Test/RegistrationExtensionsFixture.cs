// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;
using System.Web.Mvc.Filters;
using Autofac.Builder;
using Autofac.Features.Metadata;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test;

public class RegistrationExtensionsFixture
{
    [Fact]
    public void AsActionFilterForActionScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestActionFilter, IActionFilter>(
            FilterScope.Action,
            TestController.GetAction1MethodInfo<TestController>(),
            r => r.AsActionFilterFor<TestController>(c => c.Action1(default), 20),
            AutofacFilterProvider.ActionFilterMetadataKey);
    }

    [Fact]
    public void AsActionFilterForActionScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Autofac.Integration.Mvc.RegistrationExtensions.AsActionFilterFor<TestController>(
                null,
                c => c.Action1(default)));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsActionFilterForControllerScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestActionFilter, IActionFilter>(
            FilterScope.Controller,
            null,
            r => r.AsActionFilterFor<TestController>(20),
            AutofacFilterProvider.ActionFilterMetadataKey);
    }

    [Fact]
    public void AsActionFilterForControllerScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.AsActionFilterFor<TestController>(null));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsActionFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.Register(c => new TestActionFilter()).AsActionFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    [Fact]
    public void AsActionFilterForServiceTypeMustBeActionFilter()
    {
        var builder = new ContainerBuilder();

        var exception = Assert.Throws<ArgumentException>(
            () => builder.RegisterInstance(new object()).AsActionFilterFor<TestController>());

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsAuthenticationFilterForActionScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestAuthenticationFilter, IAuthenticationFilter>(
            FilterScope.Action,
            TestController.GetAction1MethodInfo<TestController>(),
            r => r.AsAuthenticationFilterFor<TestController>(c => c.Action1(default), 20),
            AutofacFilterProvider.AuthenticationFilterMetadataKey);
    }

    [Fact]
    public void AsAuthenticationFilterForActionScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Autofac.Integration.Mvc.RegistrationExtensions.AsAuthenticationFilterFor<TestController>(
                null,
                c => c.Action1(default)));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsAuthenticationFilterForControllerScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestAuthenticationFilter, IAuthenticationFilter>(
            FilterScope.Controller,
            null,
            r => r.AsAuthenticationFilterFor<TestController>(20),
            AutofacFilterProvider.AuthenticationFilterMetadataKey);
    }

    [Fact]
    public void AsAuthenticationFilterForControllerScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.AsAuthenticationFilterFor<TestController>(null));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsAuthenticationFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.Register(c => new TestAuthenticationFilter()).AsAuthenticationFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    [Fact]
    public void AsAuthenticationFilterForServiceTypeMustBeAuthenticationFilter()
    {
        var builder = new ContainerBuilder();

        var exception = Assert.Throws<ArgumentException>(
            () => builder.RegisterInstance(new object()).AsAuthenticationFilterFor<TestController>());

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsAuthorizationFilterForActionScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestAuthorizationFilter, IAuthorizationFilter>(
            FilterScope.Action,
            TestController.GetAction1MethodInfo<TestController>(),
            r => r.AsAuthorizationFilterFor<TestController>(c => c.Action1(default), 20),
            AutofacFilterProvider.AuthorizationFilterMetadataKey);
    }

    [Fact]
    public void AsAuthorizationFilterForActionScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Autofac.Integration.Mvc.RegistrationExtensions.AsAuthorizationFilterFor<TestController>(
                null,
                c => c.Action1(default)));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsAuthorizationFilterForControllerScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestAuthorizationFilter, IAuthorizationFilter>(
            FilterScope.Controller,
            null,
            r => r.AsAuthorizationFilterFor<TestController>(20),
            AutofacFilterProvider.AuthorizationFilterMetadataKey);
    }

    [Fact]
    public void AsAuthorizationFilterForControllerScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.AsAuthorizationFilterFor<TestController>(null));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsAuthorizationFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.Register(c => new TestAuthorizationFilter()).AsAuthorizationFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    [Fact]
    public void AsAuthorizationFilterForServiceTypeMustBeAuthorizationFilter()
    {
        var builder = new ContainerBuilder();

        var exception = Assert.Throws<ArgumentException>(
            () => builder.RegisterInstance(new object()).AsAuthorizationFilterFor<TestController>());

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsExceptionFilterForActionScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestExceptionFilter, IExceptionFilter>(
            FilterScope.Action,
            TestController.GetAction1MethodInfo<TestController>(),
            r => r.AsExceptionFilterFor<TestController>(c => c.Action1(default), 20),
            AutofacFilterProvider.ExceptionFilterMetadataKey);
    }

    [Fact]
    public void AsExceptionFilterForActionScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Autofac.Integration.Mvc.RegistrationExtensions.AsExceptionFilterFor<TestController>(
                null,
                c => c.Action1(default)));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsExceptionFilterForControllerScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestExceptionFilter, IExceptionFilter>(
            FilterScope.Controller,
            null,
            r => r.AsExceptionFilterFor<TestController>(20),
            AutofacFilterProvider.ExceptionFilterMetadataKey);
    }

    [Fact]
    public void AsExceptionFilterForControllerScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.AsExceptionFilterFor<TestController>(null));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsExceptionFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.Register(c => new TestExceptionFilter()).AsExceptionFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    [Fact]
    public void AsExceptionFilterForServiceTypeMustBeExceptionFilter()
    {
        var builder = new ContainerBuilder();

        var exception = Assert.Throws<ArgumentException>(
            () => builder.RegisterInstance(new object()).AsExceptionFilterFor<TestController>());

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsModelBinderForTypesRegistersInstanceModelBinder()
    {
        var originalResolver = (IDependencyResolver)null;
        try
        {
            originalResolver = DependencyResolver.Current;
            var builder = new ContainerBuilder();
            var binder = new TestModelBinder();
            builder.RegisterInstance(binder).AsModelBinderForTypes(typeof(TestModel1));
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container, new StubLifetimeScopeProvider(container)));
            var provider = new AutofacModelBinderProvider();
            Assert.Same(binder, provider.GetBinder(typeof(TestModel1)));
        }
        finally
        {
            DependencyResolver.SetResolver(originalResolver);
        }
    }

    [Fact]
    public void AsModelBinderForTypesRegistersTypeModelBinder()
    {
        var originalResolver = (IDependencyResolver)null;
        try
        {
            originalResolver = DependencyResolver.Current;
            var builder = new ContainerBuilder();
            builder.RegisterType<TestModelBinder>().AsModelBinderForTypes(typeof(TestModel1), typeof(TestModel2));
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container, new StubLifetimeScopeProvider(container)));
            var provider = new AutofacModelBinderProvider();
            Assert.IsType<TestModelBinder>(provider.GetBinder(typeof(TestModel1)));
            Assert.IsType<TestModelBinder>(provider.GetBinder(typeof(TestModel2)));
        }
        finally
        {
            DependencyResolver.SetResolver(originalResolver);
        }
    }

    [Fact]
    public void AsModelBinderForTypesThrowsExceptionForEmptyTypeList()
    {
        var types = new Type[0];
        var builder = new ContainerBuilder();
        var registration = builder.RegisterType<TestModelBinder>();
        Assert.Throws<ArgumentException>(() => registration.AsModelBinderForTypes(types));
    }

    [Fact]
    public void AsModelBinderForTypesThrowsExceptionForNullRegistration()
    {
        var registration = (IRegistrationBuilder<RegistrationExtensionsFixture, ConcreteReflectionActivatorData, SingleRegistrationStyle>)null;
        Assert.Throws<ArgumentNullException>(() => registration.AsModelBinderForTypes(typeof(TestModel1)));
    }

    [Fact]
    public void AsModelBinderForTypesThrowsExceptionForNullTypeList()
    {
        var types = (Type[])null;
        var builder = new ContainerBuilder();
        var registration = builder.RegisterType<TestModelBinder>();
        Assert.Throws<ArgumentNullException>(() => registration.AsModelBinderForTypes(types));
    }

    [Fact]
    public void AsModelBinderForTypesThrowsExceptionWhenAllTypesNullInList()
    {
        var builder = new ContainerBuilder();
        var registration = builder.RegisterType<TestModelBinder>();
        Assert.Throws<ArgumentException>(() => registration.AsModelBinderForTypes(null, null, null));
    }

    [Fact]
    public void AsResultFilterForActionScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestResultFilter, IResultFilter>(
            FilterScope.Action,
            TestController.GetAction1MethodInfo<TestController>(),
            r => r.AsResultFilterFor<TestController>(c => c.Action1(default), 20),
            AutofacFilterProvider.ResultFilterMetadataKey);
    }

    [Fact]
    public void AsResultFilterForActionScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Autofac.Integration.Mvc.RegistrationExtensions.AsResultFilterFor<TestController>(
                null,
                c => c.Action1(default)));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsResultFilterForControllerScopedFilterAddsCorrectMetadata()
    {
        AssertFilterRegistration<TestResultFilter, IResultFilter>(
            FilterScope.Controller,
            null,
            r => r.AsResultFilterFor<TestController>(20),
            AutofacFilterProvider.ResultFilterMetadataKey);
    }

    [Fact]
    public void AsResultFilterForControllerScopedFilterThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.AsResultFilterFor<TestController>(null));

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void AsResultFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.Register(c => new TestResultFilter()).AsResultFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    [Fact]
    public void AsResultFilterForServiceTypeMustBeResultFilter()
    {
        var builder = new ContainerBuilder();

        var exception = Assert.Throws<ArgumentException>(
            () => builder.RegisterInstance(new object()).AsResultFilterFor<TestController>());

        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void CacheInSessionThrowsExceptionForNullRegistration()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.CacheInSession<object, SimpleActivatorData, SingleRegistrationStyle>(null));
        Assert.Equal("registration", exception.ParamName);
    }

    [Fact]
    public void DoesNotRegisterControllerTypesThatDoNotEndWithControllerString()
    {
        var builder = new ContainerBuilder();
        builder.RegisterControllers(GetType().Assembly);

        var container = builder.Build();

        Assert.False(container.IsRegistered<IsAControllerNot>());
    }

    [Fact]
    public void InjectsInvoker()
    {
        var builder = new ContainerBuilder();
        builder.RegisterControllers(GetType().Assembly)
            .InjectActionInvoker();
        builder.RegisterType<TestActionInvoker>().As<IActionInvoker>();
        var container = builder.Build();

        var controller = container.Resolve<TestController>();
        Assert.IsType<TestActionInvoker>(controller.ActionInvoker);
    }

    [Fact]
    public void InvokesCustomActivating()
    {
        var builder = new ContainerBuilder();
        builder.RegisterControllers(GetType().Assembly)
            .OnActivating(e => ((TestController)e.Instance).Dependency = new object());

        var container = builder.Build();

        var controller = container.Resolve<TestController>();
        Assert.NotNull(controller.Dependency);
    }

    // Action filter override
    [Fact]
    public void OverrideActionFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.OverrideActionFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    // Authentication filter override
    [Fact]
    public void OverrideAuthenticationFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.OverrideAuthenticationFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    // Authorization filter override
    [Fact]
    public void OverrideAuthorizationFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.OverrideAuthorizationFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    // Exception filter override
    [Fact]
    public void OverrideExceptionFilterForRequiresActionSelector()
    {
        var builder = new ContainerBuilder();
        var exception = Assert.Throws<ArgumentNullException>(
            () => builder.OverrideExceptionFilterFor<TestController>(null));
        Assert.Equal("actionSelector", exception.ParamName);
    }

    [Fact]
    public void RegisterFilterProviderCanSafelyBeCalledTwice()
    {
        var builder = new ContainerBuilder();
        builder.RegisterFilterProvider();
        builder.RegisterFilterProvider();
    }

    [Fact]
    public void RegisterFilterProviderRemovesExistingProvider()
    {
        var builder = new ContainerBuilder();
        builder.RegisterFilterProvider();
        Assert.False(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().Any());
    }

    [Fact]
    public void RegisterFilterProviderThrowsExceptionForNullBuilder()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.RegisterFilterProvider(null));
        Assert.Equal("builder", exception.ParamName);
    }

    [Fact]
    public void RegisterModelBinderProviderThrowsExceptionForNullBuilder()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.RegisterModelBinderProvider(null));
        Assert.Equal("builder", exception.ParamName);
    }

    [Fact]
    public void RegisterModelBindersThrowsExceptionForNullAssemblies()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new ContainerBuilder().RegisterModelBinders(null));
        Assert.Equal("modelBinderAssemblies", exception.ParamName);
    }

    [Fact]
    public void RegisterModelBindersThrowsExceptionForNullBuilder()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Autofac.Integration.Mvc.RegistrationExtensions.RegisterModelBinders(null, Assembly.GetExecutingAssembly()));
        Assert.Equal("builder", exception.ParamName);
    }

    private static void AssertFilterRegistration<TFilter, TService>(
        FilterScope filterScope,
        MethodInfo methodInfo,
        Action<IRegistrationBuilder<TFilter, SimpleActivatorData, SingleRegistrationStyle>> configure,
        string metadataKey)
            where TFilter : new()
    {
        var builder = new ContainerBuilder();
        configure(builder.Register(c => new TFilter()));
        var container = builder.Build();

        var service = container.Resolve<Meta<TService>>();

        var metadata = (FilterMetadata)service.Metadata[metadataKey];

        Assert.Equal(typeof(TestController), metadata.ControllerType);
        Assert.Equal(filterScope, metadata.FilterScope);
        Assert.Equal(methodInfo, metadata.MethodInfo);
        Assert.Equal(20, metadata.Order);
        Assert.IsAssignableFrom<TService>(service.Value);
    }
}
