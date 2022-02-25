// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Builder;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test;

public class ActionFilterFixture : AutofacFilterBaseFixture<TestActionFilter, TestActionFilter2, IActionFilter>
{
    public ActionFilterFixture(AutofacFilterTestContext testContext)
        : base(testContext)
    {
    }

    protected override Action<ContainerBuilder> ConfigureActionFilterOverride()
    {
        return builder => builder.OverrideActionFilterFor<TestController>(c => c.Action1(default));
    }

    protected override Action<IRegistrationBuilder<TestActionFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureActionOverrideRegistration()
    {
        return r => r.AsActionFilterOverrideFor<TestController>(c => c.Action1(default));
    }

    protected override Action<ContainerBuilder> ConfigureControllerFilterOverride()
    {
        return builder => builder.OverrideActionFilterFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestActionFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureControllerOverrideRegistration()
    {
        return r => r.AsActionFilterOverrideFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestActionFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstActionRegistration()
    {
        return r => r.AsActionFilterFor<TestController>(c => c.Action1(default));
    }

    protected override Action<IRegistrationBuilder<TestActionFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstControllerRegistration()
    {
        return r => r.AsActionFilterFor<TestController>();
    }

    protected override Action<IRegistrationBuilder<TestActionFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondActionRegistration()
    {
        return r => r.AsActionFilterFor<TestController>(c => c.Action1(default), 20);
    }

    protected override Action<IRegistrationBuilder<TestActionFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondControllerRegistration()
    {
        return r => r.AsActionFilterFor<TestController>(20);
    }

    protected override Type GetWrapperType()
    {
        return typeof(ActionFilterOverride);
    }
}
