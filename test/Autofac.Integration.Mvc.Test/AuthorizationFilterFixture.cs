// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Builder;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test;

public class AuthorizationFilterFixture : AutofacFilterBaseFixture<TestAuthorizationFilter, TestAuthorizationFilter2, IAuthorizationFilter>
{
    public AuthorizationFilterFixture(AutofacFilterTestContext testContext)
        : base(testContext)
    {
    }

    protected override Action<ContainerBuilder> ConfigureActionFilterOverride()
    {
        return builder => builder.OverrideAuthorizationFilterFor<TestController>(c => c.Action1(default));
    }

    protected override Action<IRegistrationBuilder<TestAuthorizationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureActionOverrideRegistration()
    {
        return r => r.AsAuthorizationFilterOverrideFor<TestController>(c => c.Action1(default));
    }

    protected override Action<ContainerBuilder> ConfigureControllerFilterOverride()
    {
        return builder => builder.OverrideAuthorizationFilterFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestAuthorizationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureControllerOverrideRegistration()
    {
        return r => r.AsAuthorizationFilterOverrideFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestAuthorizationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstActionRegistration()
    {
        return r => r.AsAuthorizationFilterFor<TestController>(c => c.Action1(default));
    }

    protected override Action<IRegistrationBuilder<TestAuthorizationFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstControllerRegistration()
    {
        return r => r.AsAuthorizationFilterFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestAuthorizationFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondActionRegistration()
    {
        return r => r.AsAuthorizationFilterFor<TestController>(c => c.Action1(default), 20);
    }

    protected override Action<IRegistrationBuilder<TestAuthorizationFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondControllerRegistration()
    {
        return r => r.AsAuthorizationFilterFor<TestController>(20);
    }

    protected override Type GetWrapperType()
    {
        return typeof(AuthorizationFilterOverride);
    }
}
