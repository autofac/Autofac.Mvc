// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web.Mvc.Filters;
using Autofac.Builder;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test;

public class AuthenticationFilterFixture : AutofacFilterBaseFixture<TestAuthenticationFilter, TestAuthenticationFilter2, IAuthenticationFilter>
{
    public AuthenticationFilterFixture(AutofacFilterTestContext testContext)
        : base(testContext)
    {
    }

    protected override Action<ContainerBuilder> ConfigureActionFilterOverride()
    {
        return builder => builder.OverrideAuthenticationFilterFor<TestController>(c => c.Action1(default));
    }

    protected override Action<IRegistrationBuilder<TestAuthenticationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureActionOverrideRegistration()
    {
        return r => r.AsAuthenticationFilterOverrideFor<TestController>(c => c.Action1(default));
    }

    protected override Action<ContainerBuilder> ConfigureControllerFilterOverride()
    {
        return builder => builder.OverrideAuthenticationFilterFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestAuthenticationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureControllerOverrideRegistration()
    {
        return r => r.AsAuthenticationFilterOverrideFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestAuthenticationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstActionRegistration()
    {
        return r => r.AsAuthenticationFilterFor<TestController>(c => c.Action1(default));
    }

    protected override Action<IRegistrationBuilder<TestAuthenticationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstControllerRegistration()
    {
        return r => r.AsAuthenticationFilterFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestAuthenticationFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondActionRegistration()
    {
        return r => r.AsAuthenticationFilterFor<TestController>(c => c.Action1(default), 20);
    }

    protected override Action<IRegistrationBuilder<TestAuthenticationFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondControllerRegistration()
    {
        return r => r.AsAuthenticationFilterFor<TestController>(20);
    }

    protected override Type GetWrapperType()
    {
        return typeof(AuthenticationFilterOverride);
    }
}
