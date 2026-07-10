// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;
using System.Web.Mvc.Filters;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test;

public class AutofacFilterProviderFixture : IClassFixture<DependencyResolverReplacementContext>
{
    private readonly string _actionName;

    private readonly ControllerContext _baseControllerContext;

    private readonly MethodInfo _baseMethodInfo;

    private readonly ControllerDescriptor _controllerDescriptor;

    private readonly ReflectedActionDescriptor _reflectedActionDescriptor;

    public AutofacFilterProviderFixture()
    {
        this._baseControllerContext = new ControllerContext { Controller = new TestController() };
        this._baseMethodInfo = TestController.GetAction1MethodInfo<TestController>();
        this._actionName = this._baseMethodInfo.Name;
        this._controllerDescriptor = Substitute.For<ControllerDescriptor>();
        this._reflectedActionDescriptor = new ReflectedActionDescriptor(this._baseMethodInfo, this._actionName, this._controllerDescriptor);
    }

    [Fact]
    public void CanRegisterMultipleFilterTypesAgainstSingleService()
    {
        var builder = new ContainerBuilder();
        builder.RegisterInstance(new TestCombinationFilter())
            .AsActionFilterFor<TestController>()
            .AsAuthenticationFilterFor<TestController>()
            .AsAuthorizationFilterFor<TestController>()
            .AsExceptionFilterFor<TestController>()
            .AsResultFilterFor<TestController>();
        var container = builder.Build();

        Assert.NotNull(container.Resolve<IActionFilter>());
        Assert.NotNull(container.Resolve<IAuthenticationFilter>());
        Assert.NotNull(container.Resolve<IAuthorizationFilter>());
        Assert.NotNull(container.Resolve<IExceptionFilter>());
        Assert.NotNull(container.Resolve<IResultFilter>());
    }

    [Fact]
    public void CanRegisterSingleFilterAgainstMultipleControllersInOneStatement()
    {
        // Issue #33: chaining AsActionFilterFor for more than one controller in a
        // single registration statement must not throw "An item with the same key
        // has already been added" and must apply the filter to each controller.
        var builder = new ContainerBuilder();
        builder.Register(c => new TestActionFilter())
            .AsActionFilterFor<TestController>()
            .AsActionFilterFor<TestControllerB>();
        var container = builder.Build();
        SetupMockLifetimeScopeProvider(container);
        var provider = new AutofacFilterProvider();

        var controllerBContext = new ControllerContext { Controller = new TestControllerB() };
        var controllerBDescriptor = new ReflectedActionDescriptor(
            TestController.GetAction1MethodInfo<TestControllerB>(),
            this._actionName,
            this._controllerDescriptor);

        var baseFilters = provider.GetFilters(this._baseControllerContext, this._reflectedActionDescriptor).ToList();
        var derivedFilters = provider.GetFilters(controllerBContext, controllerBDescriptor).ToList();

        Assert.Single(baseFilters);
        Assert.IsType<TestActionFilter>(baseFilters[0].Instance);
        Assert.Single(derivedFilters);
        Assert.IsType<TestActionFilter>(derivedFilters[0].Instance);
    }

    [Fact]
    public void CanRegisterSingleFilterAgainstMultipleActionsInOneStatement()
    {
        // Issue #33: the same collision occurs for action-scoped registrations
        // chained in a single statement.
        var builder = new ContainerBuilder();
        builder.Register(c => new TestActionFilter())
            .AsActionFilterFor<TestController>(c => c.Action1(default!))
            .AsActionFilterFor<TestControllerB>(c => c.Action1(default!));
        var container = builder.Build();
        SetupMockLifetimeScopeProvider(container);
        var provider = new AutofacFilterProvider();

        var filters = provider.GetFilters(this._baseControllerContext, this._reflectedActionDescriptor).ToList();

        Assert.Single(filters);
        Assert.IsType<TestActionFilter>(filters[0].Instance);
        Assert.Equal(FilterScope.Action, filters[0].Scope);
    }

    [Fact]
    public void NullControllerInstanceIsSkipped()
    {
        // Issue #24: when the controller instance is null, the provider should
        // skip filter evaluation rather than throwing a NullReferenceException,
        // matching the behavior of the base FilterAttributeFilterProvider.
        var builder = new ContainerBuilder();
        var container = builder.Build();
        SetupMockLifetimeScopeProvider(container);
        var provider = new AutofacFilterProvider();
        var controllerContext = new ControllerContext { Controller = null };

        var filters = provider.GetFilters(controllerContext, this._reflectedActionDescriptor).ToList();
        Assert.Empty(filters);
    }

    [Fact]
    public void FilterRegistrationsWithoutMetadataIgnored()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<AuthorizeAttribute>().AsImplementedInterfaces();
        var container = builder.Build();
        SetupMockLifetimeScopeProvider(container);
        var provider = new AutofacFilterProvider();

        var filters = provider.GetFilters(this._baseControllerContext, this._reflectedActionDescriptor).ToList();
        Assert.Empty(filters);
    }

    private static void SetupMockLifetimeScopeProvider(ILifetimeScope container)
    {
        var resolver = new AutofacDependencyResolver(container, new StubLifetimeScopeProvider(container));
        DependencyResolver.SetResolver(resolver);
    }
}
