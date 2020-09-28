using System;
using System.Linq;
using System.Web.Mvc;
using Autofac.Builder;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public abstract class AutofacFilterBaseFixture<TFilter1, TFilter2, TFilterType> : IClassFixture<AutofacFilterTestContext>
        where TFilter1 : new()
        where TFilter2 : new()
    {
        public AutofacFilterBaseFixture(AutofacFilterTestContext testContext)
        {
            this.TestContext = testContext;
        }

        public AutofacFilterTestContext TestContext { get; private set; }

        [Fact]
        public void ResolvesActionScopedFilterForImmediateBaseContoller()
        {
            AssertSingleFilter(
                FilterScope.Action,
                this.TestContext.DerivedActionDescriptor,
                this.ConfigureFirstActionRegistration(),
                this.TestContext.DerivedControllerContext);
        }

        [Fact]
        public void ResolvesActionScopedFilterForMostBaseContoller()
        {
            AssertSingleFilter(
                FilterScope.Action,
                this.TestContext.MostDerivedActionDescriptor,
                this.ConfigureFirstActionRegistration(),
                this.TestContext.MostDerivedControllerContext);
        }

        [Fact]
        public void ResolvesActionScopedFilterForReflectedActionDescriptor()
        {
            this.AssertSingleFilter(
                FilterScope.Action,
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureFirstActionRegistration());
        }

        [Fact]
        public void ResolvesActionScopedFilterForReflectedAsyncActionDescriptor()
        {
            this.AssertSingleFilter(
                FilterScope.Action,
                this.TestContext.ReflectedAsyncActionDescriptor,
                this.ConfigureFirstActionRegistration());
        }

        [Fact]
        public void ResolvesActionScopedFilterForTaskAsyncActionDescriptor()
        {
            this.AssertSingleFilter(
                FilterScope.Action,
                this.TestContext.TaskAsyncActionDescriptor,
                this.ConfigureFirstActionRegistration());
        }

        [Fact]
        public void ResolvesActionScopedOverrideFilterForImmediateBaseContoller()
        {
            AssertOverrideFilter(
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureActionFilterOverride(),
                this.TestContext.DerivedControllerContext);
        }

        [Fact]
        public void ResolvesActionScopedOverrideFilterForMostBaseContoller()
        {
            AssertOverrideFilter(
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureActionFilterOverride(),
                this.TestContext.MostDerivedControllerContext);
        }

        [Fact]
        public void ResolvesActionScopedOverrideFilterForReflectedActionDescriptor()
        {
            this.AssertOverrideFilter(
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureActionFilterOverride());
        }

        [Fact]
        public void ResolvesActionScopedOverrideFilterForReflectedAsyncActionDescriptor()
        {
            this.AssertOverrideFilter(
                this.TestContext.ReflectedAsyncActionDescriptor,
                this.ConfigureActionFilterOverride());
        }

        [Fact]
        public void ResolvesActionScopedOverrideFilterForTaskAsyncActionDescriptor()
        {
            this.AssertOverrideFilter(
                this.TestContext.TaskAsyncActionDescriptor,
                this.ConfigureActionFilterOverride());
        }

        [Fact]
        public void ResolvesControllerScopedFilterForReflectedActionDescriptor()
        {
            this.AssertSingleFilter(
                FilterScope.Controller,
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureFirstControllerRegistration());
        }

        [Fact]
        public void ResolvesControllerScopedOverrideFilter()
        {
            this.AssertOverrideFilter(
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureControllerFilterOverride());
        }

        [Fact]
        public void ResolvesControllerScopedOverrideFilterForImmediateBaseContoller()
        {
            AssertOverrideFilter(
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureControllerFilterOverride(),
                this.TestContext.DerivedControllerContext);
        }

        [Fact]
        public void ResolvesControllerScopedOverrideFilterForMostBaseContoller()
        {
            AssertOverrideFilter(
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureControllerFilterOverride(),
                this.TestContext.MostDerivedControllerContext);
        }

        [Fact]
        public void ResolvesMultipleActionScopedFilters()
        {
            this.AssertMultipleFilters(
                FilterScope.Action,
                this.ConfigureFirstActionRegistration(),
                this.ConfigureSecondActionRegistration());
        }

        [Fact]
        public void ResolvesMultipleControllerScopedFilters()
        {
            this.AssertMultipleFilters(
                FilterScope.Controller,
                this.ConfigureFirstControllerRegistration(),
                this.ConfigureSecondControllerRegistration());
        }

        [Fact]
        public void ResolvesRegisteredActionFilterOverrideForAction()
        {
            this.AssertFilterOverrideRegistration(
                FilterScope.Action,
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureActionOverrideRegistration(),
                this.TestContext.BaseControllerContext);
        }

        [Fact]
        public void ResolvesRegisteredActionFilterOverrideForController()
        {
            this.AssertFilterOverrideRegistration(
                FilterScope.Controller,
                this.TestContext.ReflectedActionDescriptor,
                this.ConfigureControllerOverrideRegistration(),
                this.TestContext.BaseControllerContext);
        }

        protected abstract Action<ContainerBuilder> ConfigureActionFilterOverride();

        protected abstract Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> ConfigureActionOverrideRegistration();

        protected abstract Action<ContainerBuilder> ConfigureControllerFilterOverride();

        protected abstract Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> ConfigureControllerOverrideRegistration();

        protected abstract Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstActionRegistration();

        protected abstract Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> ConfigureFirstControllerRegistration();

        protected abstract Action<IRegistrationBuilder<TFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondActionRegistration();

        protected abstract Action<IRegistrationBuilder<TFilter2, SimpleActivatorData, SingleRegistrationStyle>> ConfigureSecondControllerRegistration();

        protected abstract Type GetWrapperType();

        private static void AssertOverrideFilter(ActionDescriptor actionDescriptor,
            Action<ContainerBuilder> registration, ControllerContext controllerContext)
        {
            var builder = new ContainerBuilder();
            registration(builder);
            var container = builder.Build();
            SetupMockLifetimeScopeProvider(container);
            var provider = new AutofacFilterProvider();

            var filters = provider.GetFilters(controllerContext, actionDescriptor).ToList();

            var filter = filters.Select(info => info.Instance).OfType<AutofacOverrideFilter>().Single();
            Assert.IsType<AutofacOverrideFilter>(filter);
            Assert.Equal(typeof(TFilterType), filter.FiltersToOverride);
        }

        private static void AssertSingleFilter(FilterScope filterScope, ActionDescriptor actionDescriptor,
            Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> configure,
            ControllerContext controllerContext)
        {
            var builder = new ContainerBuilder();
            configure(builder.Register(c => new TFilter1()));
            var container = builder.Build();
            SetupMockLifetimeScopeProvider(container);
            var provider = new AutofacFilterProvider();

            var filters = provider.GetFilters(controllerContext, actionDescriptor).ToList();

            Assert.Single(filters);
            Assert.IsType<TFilter1>(filters[0].Instance);
            Assert.Equal(filterScope, filters[0].Scope);
        }

        private static void SetupMockLifetimeScopeProvider(ILifetimeScope container)
        {
            var resolver = new AutofacDependencyResolver(container, new StubLifetimeScopeProvider(container));
            DependencyResolver.SetResolver(resolver);
        }

        private void AssertFilterOverrideRegistration(FilterScope filterScope, ActionDescriptor actionDescriptor,
            Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> configure,
            ControllerContext controllerContext)
        {
            var builder = new ContainerBuilder();
            configure(builder.Register(c => new TFilter1()));
            var container = builder.Build();
            SetupMockLifetimeScopeProvider(container);
            var provider = new AutofacFilterProvider();

            var filters = provider.GetFilters(controllerContext, actionDescriptor).ToList();

            Assert.Single(filters);
            Assert.IsType(this.GetWrapperType(), filters[0].Instance);
            Assert.Equal(filterScope, filters[0].Scope);
        }

        private void AssertMultipleFilters(FilterScope filterScope,
            Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> configure1,
            Action<IRegistrationBuilder<TFilter2, SimpleActivatorData, SingleRegistrationStyle>> configure2)
        {
            var builder = new ContainerBuilder();
            configure1(builder.Register(c => new TFilter1()));
            configure2(builder.Register(c => new TFilter2()));
            var container = builder.Build();
            SetupMockLifetimeScopeProvider(container);
            var actionDescriptor = new ReflectedActionDescriptor(this.TestContext.BaseMethodInfo, this.TestContext.ActionName, this.TestContext.ControllerDescriptor);
            var provider = new AutofacFilterProvider();

            var filters = provider.GetFilters(this.TestContext.BaseControllerContext, actionDescriptor).ToList();

            Assert.Equal(2, filters.Count);

            var filter = filters.Single(f => f.Instance is TFilter1);
            Assert.Equal(filterScope, filter.Scope);
            Assert.Equal(Filter.DefaultOrder, filter.Order);

            filter = filters.Single(f => f.Instance is TFilter2);
            Assert.Equal(filterScope, filter.Scope);
            Assert.Equal(20, filter.Order);
        }

        private void AssertOverrideFilter(ActionDescriptor actionDescriptor, Action<ContainerBuilder> registration)
        {
            AssertOverrideFilter(actionDescriptor, registration, this.TestContext.BaseControllerContext);
        }

        private void AssertSingleFilter(FilterScope filterScope, ActionDescriptor actionDescriptor,
            Action<IRegistrationBuilder<TFilter1, SimpleActivatorData, SingleRegistrationStyle>> configure)
        {
            AssertSingleFilter(filterScope, actionDescriptor, configure, this.TestContext.BaseControllerContext);
        }
    }
}