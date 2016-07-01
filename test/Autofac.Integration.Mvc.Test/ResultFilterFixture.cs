using System;
using System.Web.Mvc;
using Autofac.Builder;

namespace Autofac.Integration.Mvc.Test
{
    public class ResultFilterFixture : AutofacFilterBaseFixture<TestResultFilter, TestResultFilter2, IResultFilter>
    {
        public ResultFilterFixture(AutofacFilterTestContext testContext)
            : base(testContext)
        {
        }

        protected override Action<ContainerBuilder> ConfigureActionFilterOverride()
        {
            return builder => builder.OverrideResultFilterFor<TestController>(c => c.Action1(default(string)));
        }

        protected override Action<IRegistrationBuilder<TestResultFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureActionOverrideRegistration()
        {
            return r => r.AsResultFilterOverrideFor<TestController>(c => c.Action1(default(string)));
        }

        protected override Action<ContainerBuilder> ConfigureControllerFilterOverride()
        {
            return builder => builder.OverrideResultFilterFor<TestController>();
        }

        protected override Action<IRegistrationBuilder<TestResultFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureControllerOverrideRegistration()
        {
            return r => r.AsResultFilterOverrideFor<TestController>();
        }

        protected override Action<IRegistrationBuilder<TestResultFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstActionRegistration()
        {
            return r => r.AsResultFilterFor<TestController>(c => c.Action1(default(string)));
        }

        protected override Action<IRegistrationBuilder<TestResultFilter, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstControllerRegistration()
        {
            return r => r.AsResultFilterFor<TestController>();
        }

        protected override Action<IRegistrationBuilder<TestResultFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondActionRegistration()
        {
            return r => r.AsResultFilterFor<TestController>(c => c.Action1(default(string)), 20);
        }

        protected override Action<IRegistrationBuilder<TestResultFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondControllerRegistration()
        {
            return r => r.AsResultFilterFor<TestController>(20);
        }

        protected override Type GetWrapperType()
        {
            return typeof(ResultFilterOverride);
        }
    }
}