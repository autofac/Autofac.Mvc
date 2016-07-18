using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Autofac.Core;
using Moq;

namespace Autofac.Integration.Mvc.Test
{
    public class ExtensibleActionInvokerPropertyTestContext : DependencyResolverReplacementContext
    {
        public ExtensibleActionInvokerPropertyTestContext()
        {
            var builder = new ContainerBuilder();

            // The difference between this context and the non-property one
            // is that we have a property selector so we should get properties
            // populated.
            builder.RegisterType<ExtensibleActionInvoker>().WithParameter("propertySelector", new DefaultPropertySelector(true)).As<IActionInvoker>();
            builder.Register(c => new ActionDependency()).As<IActionDependency>();
            builder.RegisterType<ActionDependencyProperty>();
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
            ActionDependencyProperty Property { get; }
        }

        public IContainer Container { get; private set; }

        public TestController Controller { get; private set; }

        public ControllerContext ControllerContext { get; private set; }

        public class ActionDependency : IActionDependency
        {
            public ActionDependencyProperty Property { get; set; }
        }

        public class ActionDependencyProperty
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