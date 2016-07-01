using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Moq;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class AutofacFilterProviderFixture : IClassFixture<DependencyResolverReplacementContext>
    {
        private string _actionName;

        private ControllerContext _baseControllerContext;

        private MethodInfo _baseMethodInfo;

        private ControllerDescriptor _controllerDescriptor;

        private ReflectedActionDescriptor _reflectedActionDescriptor;

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
            Assert.Equal(0, filters.Count);
        }

        private static void SetupMockLifetimeScopeProvider(ILifetimeScope container)
        {
            var resolver = new AutofacDependencyResolver(container, new StubLifetimeScopeProvider(container));
            DependencyResolver.SetResolver(resolver);
        }
    }
}
