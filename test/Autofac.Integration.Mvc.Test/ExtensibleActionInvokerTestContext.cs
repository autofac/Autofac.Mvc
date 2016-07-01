using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Moq;

namespace Autofac.Integration.Mvc.Test
{
    public class ExtensibleActionInvokerTestContext : DependencyResolverReplacementContext
    {
        public ExtensibleActionInvokerTestContext()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ExtensibleActionInvoker>().As<IActionInvoker>();
            builder.Register(c => new ActionDependency()).As<IActionDependency>();
            this.Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(this.Container, new StubLifetimeScopeProvider(this.Container)));

            var request = new Mock<HttpRequestBase>();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(mock => mock.Request).Returns(request.Object);
            this.Controller = new TestController { ValidateRequest = false };
            this.ControllerContext = new ControllerContext { Controller = this.Controller, HttpContext = httpContext.Object };
            this.Controller.ControllerContext = this.ControllerContext;
            this.Controller.ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture);
        }

        public interface IActionDependency
        {
        }

        public IContainer Container { get; private set; }

        public TestController Controller { get; private set; }

        public ControllerContext ControllerContext { get; private set; }

        public class ActionDependency : IActionDependency
        {
        }

        public class TestController : Controller
        {
            public IActionDependency Dependency { get; private set; }

            public ActionResult Index(IActionDependency dependency)
            {
                this.Dependency = dependency;
                return null;
            }
        }
    }
}