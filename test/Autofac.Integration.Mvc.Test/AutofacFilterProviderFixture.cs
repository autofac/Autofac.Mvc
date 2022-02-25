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
        this._controllerDescriptor = new Mock<ControllerDescriptor>().Object;
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
